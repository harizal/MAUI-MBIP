using Android.Content;
using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.Classes;

namespace AndroidCompound5.Pages;

public partial class SemakPassPage : ContentPage
{
	private string strCurrMukim, strCurrZone, strCurrJalan, strCurrStreetDesc;
	public static bool m_blCamera = false;
	private static int REQUEST_CAMERA = 1001;

	public SemakPassPage(string currstreet)
	{
		InitializeComponent();
		strCurrJalan = currstreet;
		lblJalan.Text = getStreetDesc(strCurrZone, strCurrMukim, strCurrJalan);
		strCurrStreetDesc = lblJalan.Text;

		GlobalClass.FileImages = new List<string>();
	}

	string getStreetDesc(string strZone, string strMukim, string strJalan)
	{
		string strStreet = "";

		var location = StreetBll.GetStreetByCodeAndZone(strJalan, strZone, strMukim);
		if (location != null)
			strStreet =  location.LongDesc;

		return strStreet;
	}

	private async void btnSemak_Clicked(object sender, EventArgs e)
	{
		try
		{
			if (string.IsNullOrEmpty(txtNoPetak.Text))
			{
				txtNoPetak.Focus();
				return;
			}

			var modal = new CustomLoading(this);

			// Show the modal on the UI thread WITHOUT waiting
			MainThread.BeginInvokeOnMainThread(() => modal.ShowPopupAsync());

			// Run background processing without blocking the UI
			await Task.Run(async () =>
			{
				await Task.Delay(1000);
				MainThread.BeginInvokeOnMainThread(() => modal.UpdateMessage("Sila tunggu..."));
				if (!string.IsNullOrEmpty(txtNoPetak.Text))
				{
					var infoDto = InfoBll.GetInfo();
					if (infoDto == null)
					{
						MainThread.BeginInvokeOnMainThread(() => modal.UpdateMessage("Fail control tak dijumpai. Sila hubung pejabat."));
						await Task.Delay(2000);
					}
					else
					{

						var info = GeneralBll.GetReserveParking(strCurrJalan, strCurrMukim, txtNoPetak.Text);
						string stringScanDateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
						info.ReturnResponse = Enums.ParkingStatus.SemakPetak;
						CompoundBll.EnquiryLogServer(txtNoPetak.Text, info.Lotstatus, infoDto, GlobalClass.Latitude, GlobalClass.Longitude, strCurrJalan, stringScanDateTime);
						if (info.status == "Y")
						{
							txtNoPetak.Text = txtNoPetak.Text;
							lblNamaPemohon.Text = info.Name;
							//lblStartDate.Text = info.StartDate;
							//lblEndDate.Text = info.EndDate;
							//lblLotstaus.Text = getLotstatus(info.Lotstatus);
							txtReasonDesc.Text = info.Note;
						}
						else
						{
							lblNamaPemohon.Text = "";
							//lblStartDate.Text = "";
							//lblEndDate.Text = "";
							//lblLotstaus.Text = "";
							txtReasonDesc.Text = "";
							MainThread.BeginInvokeOnMainThread(() => modal.UpdateMessage("Tiada Rekod Petak Bermusim ini"));
							await Task.Delay(2000);
						}
						btnSemak.IsEnabled = true;
					}
				}
				MainThread.BeginInvokeOnMainThread(() => modal.ClosePopup());
			});
			if (string.IsNullOrEmpty(lblNamaPemohon.Text))
				await DisplayAlert("Info", "Tiada Rekod Petak Bermusim ini", "OK");
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", ex.Message, "OK");
			LogFile.WriteLogFile("OnALPR : " + ex.Message);
		}
	}

	private async void imageMenu_Clicked(object sender, EventArgs e)
	{
		try
		{
			if (!m_blCamera)
			{
				m_blCamera = false;
				LogFile.WriteLogFile("Camera Semak Pass Click: Started");

				var blImage = true;
				if (txtFileGambar.Text?.Length == 0)
					blImage = false;

				var infoDto = InfoBll.GetInfo();
				if (infoDto == null)
				{
					await DisplayAlert("Info", "Fail control tak dijumpai. Sila hubung pejabat.", "OK");
					return;
				}

				string strSemakPassNum = MaintenanceBll.GetSemakPassNumber(infoDto.DolphinId);

				string strFolder = GeneralBll.GetExternalStorageDirectory() + Constants.DCIMPath + Constants.ImgsPath;
				GeneralBll.CreateFolder(strFolder);
				txtFileGambar.Text = strSemakPassNum + "0.jpg";

#if ANDROID
				var intent = new Intent(Intent.ActionMain);
				intent.SetComponent(new ComponentName("my.com.aimforce.multi_capture_camera", "my.com.aimforce.multi_capture_camera.MainActivity"));

				intent.PutExtra("my.com.aimforce.intent.extra.URL", Constants.DCIMPath + Constants.ImgsPath + "{1}.jpg");

				intent.PutExtra("my.com.aimforce.intent.extra.LABELS", new string[] { "Gambar" });

				var activity = Platform.CurrentActivity;
				activity?.StartActivityForResult(intent, 1001);
#endif
			}
		}
		catch (Exception ex)
		{
			m_blCamera = false;
			await DisplayAlert("Info", ex.Message, "OK");
		}
	}

	private bool Validate()
	{
		if (!GeneralBll.IsAlphaNumeric(txtNoPetak.Text))
		{
			txtNoPetak.Focus();
			return false;
		}
		else if (!GeneralBll.IsAlphaNumericAndSpaceCommaDotPercentMinus(txtRemarks.Text))
		{
			txtRemarks.Focus();
			return false;

		}

		return true;
	}

	private async void UpdateSemakPetak()
	{
		var infoDto = InfoBll.GetInfo();
		if (infoDto == null)
		{
			await DisplayAlert("ERROR", "Fail control tak dijumpai. Sila hubung pejabat.", "OK");
			return;
		}

		var semakpassInfo = new SemakPassDto();

		semakpassInfo.NoPetak = txtNoPetak.Text;
		//semakpassInfo.NamaPemohon = lblNama.Text;
		//semakpassInfo.StartDate = lblStartDate.Text;
		//semakpassInfo.EndDate = lblEndDate.Text;
		semakpassInfo.Remark = txtRemarks.Text;
		semakpassInfo.Pic1 = txtFileGambar.Text;

		MaintenanceBll.UpdateSemakPassInfo(infoDto, semakpassInfo, strCurrZone, strCurrJalan, strCurrStreetDesc);

		await DisplayAlert("Info", "Semak Petak telah disimpan.", "OK");
		await Navigation.PopAsync();
	}

	private async void saveMenu_Clicked(object sender, EventArgs e)
	{
		try
		{
			if (Validate())
			{
				bool answer = await DisplayAlert("Confirmation", "Pasti simpan ini ?", "Yes", "No");
				if (answer)
				{
					UpdateSemakPetak();
				}
			}

		}
		catch (Exception ex)
		{
			await DisplayAlert("Info", ex.Message, "OK");
		}
	}
}