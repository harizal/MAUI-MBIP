using Android.AdServices.Signals;
using Android.Content;
using Android.Graphics;
using Android.Views;
using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.Classes;
using Newtonsoft.Json;
using System.Globalization;
using Color = Microsoft.Maui.Graphics.Color;
namespace AndroidCompound5.Pages;

public partial class FormCompaund2Page : ContentPage
{
	private string _stringMukim;
	private string _stringZone;
	private string _stringZoneDesc;
	private string _stringAct;
	private string _stringOffend;
	private string _stringOffendAmount;
	private string _stringOffendAmount2;
	private string _stringOffendAmount3;

	private string _stringWitnessCode;
	private string _stringWitnessPub;
	private string _stringKesalahan;
	private string _stringNoteDesc;
	private string _stringTujuan;
	private string _stringPerniagaan;
	private string _stringTempohDate = "";
	private string _stringCarNum;
	private string _stringCarCategory;
	private string _stringNamaPesalah = "";
	private string _stringNoICPesalah = "";
	private string _stringIDPenguatkuasa;
	private string _stringNamaPenguatkuasa;
	private string _stringTempatJadi;
	private DateTime dateTime;
	Enums.ActiveForm _activeForm = Enums.ActiveForm.None;
	private bool m_blSave = false, m_bPrint = false;
	private string _compoundTimeStart;
	private int hour;
	private int minute;
	private static int REQUEST_CAMERA = 1001;
	private static int REQUEST_ALPR = 1002;


	public FormCompaund2Page(string mukim, string zone, string descZone, string act, string offend, string offendAmount, string offendAmount2, string offendAmount3)
	{
		InitializeComponent();

		_stringMukim = mukim;
		_stringZone = zone;
		_stringZoneDesc = descZone;
		_stringAct = act;
		_stringOffend = offend;
		_stringOffendAmount = offendAmount;
		_stringOffendAmount2 = offendAmount2;
		_stringOffendAmount3 = offendAmount3;
		SetInit();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
	}

	private async Task SetInit()
	{
		try
		{
			m_bPrint = false;
			GeneralBll.ResumeServiceSendCompound();
			EnableControl(true);
			SetNewControl(false);

			var infoDto = InfoBll.GetInfo();
			if (infoDto == null)
			{
				await DisplayAlert("Error", "Fail control tak dijumpai. Sila hubung pejabat.", "OK");
				return;
			}

			_stringIDPenguatkuasa = infoDto.EnforcerId;
			var enforcer = EnforcerBll.GetEnforcerById(_stringIDPenguatkuasa);
			if (enforcer == null)
				_stringNamaPenguatkuasa = " ";
			else
				_stringNamaPenguatkuasa = enforcer.EnforcerName;

			_compoundTimeStart = GeneralBll.GetLocalDateTime().ToString("HHmmss");
			var output = CompoundBll.GenerateCompoundNumber(infoDto);
			if (output.Result)
			{
				txtCompound.Text = output.Message;
			}
			else
			{
				await DisplayAlert("ERROR", output.Message, "OK");
			}

			GlobalClass.FileImages = new List<string>();
			_stringWitnessPub = "";
			_stringNoteDesc = "";
			txtCarNum.Text = "";
			txtRoadTax.Text = "";
			txtMake.Text = "";
			txtColor.Text = "";
			txtPerenggan.Text = "";
			m_blSave = false;
			hour = GeneralBll.GetLocalDateTime().Hour;
			minute = GeneralBll.GetLocalDateTime().Minute;
			dateTime = GeneralBll.GetLocalDateTime();
			DateTimeFormatInfo dtfi = CultureInfo.CreateSpecificCulture("en-US").DateTimeFormat;
			await IsValidOffend(true);
			m_bPrint = false;
		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
		}
	}

	private void HandleAfterBackPage()
	{
		if (GlobalClass.FindResult)
		{
			switch (_activeForm)
			{
				case Enums.ActiveForm.FindCategory:
					txtCategory.Text = GlobalClass.ReturnCodeFind.Replace(";", " ");
					break;
				case Enums.ActiveForm.FindModel:
					txtMake.Text = GlobalClass.ReturnCodeFind.Replace(";", " ");
					break;
				case Enums.ActiveForm.FindStreet:
					txtLocation.Text = GlobalClass.ReturnCodeFind.Split(';')[0];
					txtLocation.Text += " " + GlobalClass.ReturnCodeFind.Split(';')[1];
					_stringMukim = GlobalClass.ReturnCodeFind.Split(';')[2];
					_stringZone = GlobalClass.ReturnCodeFind.Split(';')[3];
					_stringZoneDesc = getZoneDesc(_stringZone);
					_stringTempatJadi = GlobalClass.ReturnCodeFind.Split(';')[1] + "," + _stringZoneDesc;
					break;
				case Enums.ActiveForm.FindDelivery:
					txtSerah.Text = GlobalClass.ReturnCodeFind.Replace(";", " ");
					break;
				case Enums.ActiveForm.FormNext:
					_stringWitnessCode = GlobalClass.ReturnCodeFind.Split(';')[0];
					_stringKesalahan = GlobalClass.ReturnCodeFind.Split(';')[1];
					_stringNoteDesc = GlobalClass.ReturnCodeFind.Split(';')[2];
					_stringTujuan = GlobalClass.ReturnCodeFind.Split(';')[3];
					_stringPerniagaan = GlobalClass.ReturnCodeFind.Split(';')[4];
					_stringWitnessPub = GlobalClass.ReturnCodeFind.Split(';')[5];
					_stringTempohDate = GlobalClass.ReturnCodeFind.Split(';')[6];
					break;
				case Enums.ActiveForm.FindOffend2:
					_stringOffend = GlobalClass.ReturnCodeFind.Split(';')[0];
					txtPerenggan.Text = GlobalClass.ReturnCodeFind.Replace(";", " ");
					IsValidOffend(true);
					break;
				case Enums.ActiveForm.FindTempatJadi:
					txtTptJadi.Text = GlobalClass.ReturnCodeFind.Replace(";", " ");
					break;
				case Enums.ActiveForm.FindColor:
					txtColor.Text = GlobalClass.ReturnCodeFind.Replace(";", " ");
					break;
			}
			GlobalClass.FindResult = false;
			GlobalClass.ReturnCodeFind = "";
		}

		_activeForm = Enums.ActiveForm.None;
		EnablePrint();

		GlobalClass.FileImages ??= GeneralAndroidClass.GetBackFileImage(txtCompound.Text);
	}

	private void EnablePrint()
	{
		SetPrintControl(false);

		if (string.IsNullOrEmpty(txtCarNum.Text)) return;
		if (string.IsNullOrEmpty(txtCategory.Text)) return;
		if (string.IsNullOrEmpty(txtMake.Text)) return;
		if (string.IsNullOrEmpty(txtPerenggan.Text)) return;
		if (string.IsNullOrEmpty(txtLocation.Text)) return;
		//if (string.IsNullOrEmpty(txtPetak.Text)) return;
		if (string.IsNullOrEmpty(_stringWitnessCode)) return;
		if (string.IsNullOrEmpty(txtSerah.Text)) return;

		if (GeneralBll.getPhotoCount(txtCompound.Text) < Constants.MinPhoto) return;


		SetPrintControl(true);

	}

	private async Task StartActivityFind(string sFindType)
	{
		GlobalClass.FindResult = false;
		await Navigation.PushAsync(new CarianPage(HandleAfterBackPage, sFindType, string.Empty, _stringMukim, _stringZone, string.Empty, string.Empty, carcategory: txtCategory.Text));
	}

	private async Task OnALPR()
	{
		var modal = new CustomLoading(this);

		try
		{
			// Show the modal on the UI thread WITHOUT waiting
			MainThread.BeginInvokeOnMainThread(() => modal.ShowPopupAsync());

			// Run background processing without blocking the UI
			await Task.Run(async () =>
			{
				await Task.Delay(1000);
				MainThread.BeginInvokeOnMainThread(() => modal.UpdateMessage("Processing..."));

				var intent = new Intent(Intent.ActionMain);
				intent.SetComponent(new ComponentName("my.com.aimforce.lpr", "my.com.aimforce.cv.activity.ScanActivity"));
				var activity = Platform.CurrentActivity;
				activity?.StartActivityForResult(intent, REQUEST_ALPR);


				MainThread.BeginInvokeOnMainThread(() => modal.ClosePopup());
			});

			await DisplayAlert("Info", "NOT IMPLEMENTED YET", "OK");
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", ex.Message, "OK");
			LogFile.WriteLogFile("OnALPR : " + ex.Message);
			MainThread.BeginInvokeOnMainThread(() => modal.ClosePopup());
		}
	}


	private async void btnCheckCarNumber_Clicked(object sender, EventArgs e)
	{
		await OnALPR();
	}

	private async void btnFindCategory_Clicked(object sender, EventArgs e)
	{
		_activeForm = Enums.ActiveForm.FindCategory;
		await StartActivityFind(Constants.FindCarCategory);
	}

	private async void btnFindMake_Clicked(object sender, EventArgs e)
	{
		_activeForm = Enums.ActiveForm.FindModel;
		await StartActivityFind(Constants.FindCarType);
	}

	private async void btnFindColor_Clicked(object sender, EventArgs e)
	{
		_activeForm = Enums.ActiveForm.FindColor;
		await StartActivityFind(Constants.FindCarColor);
	}

	private async void btnFindPerenggan_Clicked(object sender, EventArgs e)
	{
		_activeForm = Enums.ActiveForm.FindOffend2;
		GlobalClass.FindResult = false;
		await Navigation.PushAsync(new FindOffendPage(HandleAfterBackPage, Constants.FindOffend2, string.Empty, _stringAct, Constants.CompType1));
	}

	private async void btnFindLocation_Clicked(object sender, EventArgs e)
	{
		_activeForm = Enums.ActiveForm.FindStreet;
		await StartActivityFind(Constants.FindStreet);
	}

	private async void btnFindTptJadi_Clicked(object sender, EventArgs e)
	{
		_activeForm = Enums.ActiveForm.FindTempatJadi;
		await StartActivityFind(Constants.FindTempatJadi);
	}

	private async void btnFindSerah_Clicked(object sender, EventArgs e)
	{
		_activeForm = Enums.ActiveForm.FindDelivery;
		await StartActivityFind(Constants.FindDelivery);
	}

	private async Task<bool> IsValidCarNum()
	{
		bool retcode = true;
		string strMsg = "";

		if (!GeneralBll.IsAlphaNumeric(txtCarNum.Text))
		{
			await DisplayAlert("INFO", "No Kenderaan : Terdapat character tidak sah. Sila semak input anda.", "OK");
			return false;
		}


		if (txtCarNum.Text.Trim().Length > 0)
		{
			var passbulan = PassBulanBll.GetPassBulanByCarNum(txtCarNum.Text);
			if (passbulan != null)
			{
				if (passbulan.PassType == "F")
					strMsg = "Kereta adalah Nombor daftar palsu !!";
				else
				{
					strMsg = "kereta ada pass bulanan !!! " + Constants.NewLine + "Nombor Siri : " + passbulan.SerialNum + Constants.NewLine + "Tarikh Mula : " + passbulan.StartDate + Constants.NewLine + "Tarikh Tamat " + passbulan.EndDate;
					retcode = false;
				}
				await DisplayAlert("INFO", strMsg, "OK");
			}
		}
		return retcode;
	}

	private bool IsValidCategory()
	{

		if (txtCategory.Text.Trim().Length > 0)
		{
			var splitData = GeneralBll.GetSplitData(txtCategory.Text, ' ');
			var category = TableFilBll.GetCarCategory(splitData.Code);
			if (category == null)
				return false;

			txtCategory.Text = category.Carcategory + " " + category.ShortDesc;
			_stringCarCategory = category.ShortDesc;
		}
		return true;
	}
	private bool IsValidColor()
	{

		if (txtColor.Text.Trim().Length > 0)
		{
			var splitData = GeneralBll.GetSplitData(txtColor.Text, ' ');
			var color = TableFilBll.GetCarColorById(splitData.Code);
			if (color == null)
				return false;

			txtColor.Text = color.Code + " " + color.LongDesc;
		}
		return true;
	}

	private bool IsValidModel()
	{
		var splitCategory = GeneralBll.GetSplitData(txtCategory.Text, ' ');
		if (splitCategory.Code == "")
			return false;

		var splitData = GeneralBll.GetSplitData(txtMake.Text, ' ');
		if (splitData.Code == Constants.NewCarTypeCode)
		{
			if (string.IsNullOrEmpty(splitData.Description))
				return false;
			return true;
		}

		var data = TableFilBll.GetCarTypeByCode(splitData.Code, splitCategory.Code);
		if (data == null)
			return false;

		txtMake.Text = data.Code + " " + data.LongDescCode;
		return true;
	}

	private async Task<bool> IsValidOffend(bool bClear)
	{
		var splitData = GeneralBll.GetSplitData(txtPerenggan.Text, ' ');
		var offend = TableFilBll.GetOffendByCodeAndAct(splitData.Code, _stringAct);
		if (offend == null)
			return false;

		txtPerenggan.Text = offend.OfdCode + " " + offend.LongDesc;
		_stringOffend = offend.OfdCode;
		_stringOffendAmount = offend.OffendAmt;
		_stringOffendAmount2 = offend.OffendAmt2;
		_stringOffendAmount3 = offend.OffendAmt3;

		try
		{

			if (_stringTempohDate == "")
			{
				if (offend.PrintFlag == "Y")
				{
					DateTimeFormatInfo dtfi = CultureInfo.CreateSpecificCulture("en-US").DateTimeFormat;
					dtfi.ShortDatePattern = @"dd/MM/yyyy";
					dateTime = GeneralBll.AddDaysExcludeWeekEnd(dateTime, 3);
					_stringTempohDate = dateTime.ToString("d", dtfi);
				}
			}
		}

		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
		}

		//}

		return true;
	}

	private bool IsValidDelivery()
	{
		var splitData = GeneralBll.GetSplitData(txtSerah.Text, ' ');
		var delivery = TableFilBll.GetDeliveryByCode(splitData.Code);
		if (delivery == null)
			return false;

		txtSerah.Text = delivery.Code + " " + delivery.ShortDesc;
		return true;
	}

	private string getZoneDesc(string strZone)
	{
		string strDesc = "";
		if (strZone == Constants.NewZoneCode)
		{
			if (string.IsNullOrEmpty(strZone))
				return strDesc;
			return strDesc;
		}

		var zone = TableFilBll.GetZoneByCodeAndMukim(strZone, _stringMukim);
		if (zone == null)
		{
			strDesc = string.Empty;
			return strDesc;
		}
		strDesc = zone.LongDesc;
		return strDesc;
	}


	private bool IsValidLocation()
	{

		bool result = false;
		int i = 0;

		if (txtLocation.Text.Length >= 4)
		{
			i = txtLocation.Text.IndexOf(' ');
			string sCode = txtLocation.Text.Substring(0, i).Trim();

			if (sCode == "0000" && txtLocation.Text.Length > 4)
			{
				result = true;
			}
			else
			{
				var location = StreetBll.GetStreetByCodeAndZone(sCode, _stringZone, _stringMukim);
				if (location != null)
				{
					txtLocation.Text = location.Code + " " + location.LongDesc;
					_stringZoneDesc = getZoneDesc(_stringZone);
					_stringTempatJadi = location.LongDesc + "," + _stringZoneDesc;
					result = true;
				}
			}
		}
		return result;
	}


	private async Task<bool> ValidateCompound()
	{
		if (!IsValidCategory())
		{
			await DisplayAlert("INFO", "Kod Jenis tidak sah", "OK");
			return false;
		}

		if (!IsValidModel())
		{
			await DisplayAlert("INFO", "Kod Model tidak sah", "OK");
			return false;
		}
		if (!IsValidColor())
		{
			await DisplayAlert("INFO", "Kod Warna tidak sah", "OK");
			return false;
		}
		if (!await IsValidOffend(false))
		{
			await DisplayAlert("INFO", "Kod Offend tidak sah", "OK");
			return false;
		}

		if (!IsValidDelivery())
		{
			await DisplayAlert("INFO", "Kod cara serahan tidak sah", "OK");
			return false;
		}

		if (!IsValidLocation())
		{
			await DisplayAlert("INFO", "Kod lokasi tidak sah", "OK");
			return false;
		}

		if (!GeneralBll.IsAlphaNumeric(txtCarNum.Text))
		{
			await DisplayAlert("INFO", "No Kenderaan : Terdapat character tidak sah. Sila semak input anda.", "OK");
			txtCarNum.Focus();
			return false;
		}

		if (!GeneralBll.IsAlphaNumeric(txtRoadTax.Text))
		{
			await DisplayAlert("INFO", "No Cukai Jalan : Terdapat character tidak sah. Sila semak input anda.", "OK");
			txtRoadTax.Focus();
			return false;
		}

		if (!GeneralBll.IsAlphaNumericAndOthers1(txtTptJadi.Text))
		{
			await DisplayAlert("INFO", "Tempat Kejadian : Terdapat character tidak sah. Sila semak input anda.", "OK");
			txtTptJadi.Focus();
			return false;
		}

		return true;
	}

	private void SetPrintControl(bool blValue)
	{
		printMenu.IconImageSource = blValue ? ImageSource.FromFile("print.png") : ImageSource.FromFile("print_disable.png");
		printMenu.IsEnabled = blValue;
	}

	private void SetNewControl(bool blValue)
	{
		addNewMenu.IconImageSource = !blValue ? ImageSource.FromFile("add_new_disable.png") : ImageSource.FromFile("add_new.png");
		addNewMenu.IsEnabled = blValue;
	}
	private void EnableControl(bool enable)
	{
		IMenuItem menuItem;

		txtCarNum.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		txtCarNum.IsEnabled = enable;

		txtRoadTax.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		txtRoadTax.IsEnabled = enable;

		txtCategory.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		txtCategory.IsEnabled = enable;
		btnFindCategory.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		btnFindCategory.IsEnabled = enable;

		txtMake.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		txtMake.IsEnabled = enable;
		btnFindMake.IsEnabled = enable;
		btnFindMake.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];

		txtColor.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		txtColor.IsEnabled = enable;
		btnFindColor.IsEnabled = enable;
		btnFindColor.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];

		txtPerenggan.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		txtPerenggan.IsEnabled = enable;
		btnFindPerenggan.IsEnabled = enable;
		btnFindPerenggan.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];

		txtLocation.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		txtLocation.IsEnabled = enable;
		btnFindLocation.IsEnabled = enable;
		btnFindLocation.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];

		txtTptJadi.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		txtTptJadi.IsEnabled = enable;
		btnFindTptJadi.IsEnabled = enable;
		btnFindTptJadi.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];

		txtSerah.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		txtSerah.IsEnabled = enable;
		btnFindSerah.IsEnabled = enable;
		btnFindSerah.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];


		cameraMenu.IsEnabled = true;
		addNewMenu.IsEnabled = !enable;
		//txtCarNum.GetFocusables(FocusSearchDirection.Left);
		//if (!enable) txtCarNum.Background.ClearColorFilter();
	}

	private async void txtCategory_Unfocused(object sender, FocusEventArgs e)
	{
		try
		{
			IsValidCategory();
		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
		}
	}

	private async void txtMake_Unfocused(object sender, FocusEventArgs e)
	{
		try
		{
			IsValidModel();
		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
		}
	}

	private async void txtColor_Unfocused(object sender, FocusEventArgs e)
	{
		try
		{
			IsValidColor();
		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
		}
	}

	private async void txtPerenggan_Unfocused(object sender, FocusEventArgs e)
	{
		try
		{
			await IsValidOffend(true);
		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
		}
	}

	private async void txtLocation_Unfocused(object sender, FocusEventArgs e)
	{
		try
		{
			IsValidLocation();
		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
		}
	}

	private async void txtTptJadi_Unfocused(object sender, FocusEventArgs e)
	{
		try
		{
			_stringTempatJadi = txtTptJadi.Text;
		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
		}
	}

	private async void txtSerah_Unfocused(object sender, FocusEventArgs e)
	{
		try
		{
			IsValidDelivery();
		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
		}
	}

	private async void txtCarNum_Unfocused(object sender, FocusEventArgs e)
	{
		try
		{
			await IsValidCarNum();
		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
		}
	}
	private async Task OnNew()
	{

		try
		{
			EnableControl(true);
			await SetInit();
		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", Constants.UnexpectedErrorMessage, "OK");
			LogFile.WriteLogFile("FormCompaun2", "OnNew", ex.Message, Enums.LogType.Error);
		}
	}

	private async void cameraMenu_Clicked(object sender, EventArgs e)
	{
		try
		{
			string strFolder = GeneralBll.GetExternalStorageDirectory() + Constants.DCIMPath + Constants.ImgsPath;
			GeneralBll.CreateFolder(strFolder);
			var intent = new Intent(Intent.ActionMain);
			if (m_bPrint)
			{
				intent.SetComponent(new ComponentName("my.com.aimforce.multi_capture_camera", "my.com.aimforce.multi_capture_camera.MainActivity"));
				//intent.PutExtra("my.com.aimforce.intent.extra.URL", Constants.ProgramPath + Constants.ImgsPath + txtCompound.Text + "Final{1}.jpg");
				intent.PutExtra("my.com.aimforce.intent.extra.URL", Constants.DCIMPath + Constants.ImgsPath + txtCompound.Text + "Final{1}.jpg");
				intent.PutExtra("my.com.aimforce.intent.extra.LABELS", new System.String[] { "Kompaun" });
			}
			else
			{
				intent.SetComponent(new ComponentName("my.com.aimforce.multi_capture_camera", "my.com.aimforce.multi_capture_camera.MainActivity"));
				//                    intent.PutExtra("my.com.aimforce.intent.extra.URL", Constants.ProgramPath + Constants.ImgsPath + txtCompound.Text + "{4}.jpg");
				//intent.PutExtra("my.com.aimforce.intent.extra.URL", Constants.ProgramPath + Constants.ImgsPath + txtCompound.Text + "{7}.jpg");
				intent.PutExtra("my.com.aimforce.intent.extra.URL", Constants.DCIMPath + Constants.ImgsPath + txtCompound.Text + "{7}.jpg");
				intent.PutExtra("my.com.aimforce.intent.extra.LABELS", new System.String[] { "Depan", "Cukai Jalan", "Belakang", "Jauh", "Kelima", "Keenam", "Ketujuh" });
			}

			//intent.PutExtra("my.com.aimforce.intent.extra.URL", Constants.ProgramPath + Constants.ImgsPath + txtCompound.Text + "{ 4}.jpg");
			//intent.PutExtra("my.com.aimforce.intent.extra.LABELS", new String[] { "Depan", "Cukai Jalan", "Belakang", "Jauh" });
			var activity = Platform.CurrentActivity;
			activity?.StartActivityForResult(intent, REQUEST_CAMERA);
		}
		catch (Exception ex)
		{
			await DisplayAlert("Info", ex.Message, "OK");
		}
	}

	private async void addNewMenu_Clicked(object sender, EventArgs e)
	{
		await OnNew();
	}

	private async void cameraReservePetak_Clicked(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(txtLocation.Text))
		{
			await DisplayAlert("INFO", "Sila pilih lokasi !!", "OK");
			txtLocation.Focus();
			return;
		}
		var splitData = GeneralBll.GetSplitData(txtLocation.Text, ' ');
		await Navigation.PushAsync(new SemakPassPage(splitData.Code));
		EnablePrint();
	}

	protected async void OnShowPrintPageSentString(Enums.PrintType printType, string printData, bool isSecondPrint = false)
	{
		await Navigation.PushAsync(new PrintPage(this, printType.ToString(), isSecondPrint, printData));
	}

	private void SetPrintBufferBmp(Bitmap bitmap1, Bitmap bitmap2)
	{
		var insPrint = new PrintDataBll();
		insPrint.PrintCompoundBitmap(bitmap1, bitmap2);

		var stringData = GeneralBll.SetPrintDataToString(insPrint.GetListPrintData());
		OnShowPrintPageSentString(Enums.PrintType.CompoundType1, stringData);

	}
	private async void PrintCompound(bool isNeedCheck)
	{
		try
		{
			var modal = new CustomLoading(this);
			MainThread.BeginInvokeOnMainThread(() => modal.ShowPopupAsync());
			await Task.Run(async () =>
			{
				await Task.Delay(1000);
				MainThread.BeginInvokeOnMainThread(() => modal.UpdateMessage("Sila tunggu..."));

				var compoundDto = CompoundBll.GetCompoundByCompoundNumber(txtCompound.Text);

				var offendDto = TableFilBll.GetOffendByCodeAndAct(compoundDto.OfdCode, compoundDto.ActCode);

				var actDto = TableFilBll.GetActByCode(compoundDto.ActCode);

				var enforcerDto = EnforcerBll.GetEnforcerById(compoundDto.EnforcerId);

				var bitmap1 = new PrintImageBll().CreateKompaunType1Bitmap_1(Platform.CurrentActivity ?? Android.App.Application.Context, CommunityToolkit.Maui.Resource.Drawable.Logo, 0, 0, compoundDto, offendDto, actDto, enforcerDto);
				var bitmap2 = new PrintImageBll().CreateKompaunType1Bitmap_2(Platform.CurrentActivity ?? Android.App.Application.Context, CommunityToolkit.Maui.Resource.Drawable.Logo, 0, 0, compoundDto, offendDto, actDto, enforcerDto);

				try
				{
					SetPrintBufferBmp(bitmap1, bitmap2);
				}
				catch (Exception ex)
				{
					await Task.Delay(2000);
					MainThread.BeginInvokeOnMainThread(() => modal.UpdateMessage("UnExpected Error OnPrinting() : " + ex.Message));
					LogFile.WriteLogFile("UnExpected Error OnPrinting() : " + ex.Message, Enums.LogType.Error);
				}
				MainThread.BeginInvokeOnMainThread(() => modal.ClosePopup());
			});

			//ProgressDialog pDialog = ProgressDialog.Show(this, "Cetak kompaun", "Sila tunggu", true);

			//new Thread(new ThreadStart(() =>
			//{
			//	Thread.CurrentThread.IsBackground = true;
			//	RunOnUiThread(() => OnPrinting(pDialog));
			//})).Start();

		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", "UnExpected Error PrintCompound() : " + ex.Message, "OK");
			LogFile.WriteLogFile("UnExpected Error PrintCompound() : " + ex.Message, Enums.LogType.Error);
		}
	}

	private async void OnReprint()
	{
		try
		{
			PrintCompound(true);
		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", Constants.UnexpectedErrorMessage, "OK");
			LogFile.WriteLogFile("FormCompaun2", "OnReprint", ex.Message, Enums.LogType.Error);
		}
	}

	private async Task<int> SaveCompound()
	{
		try
		{
			var infoDto = InfoBll.GetInfo();
			if (infoDto == null)
			{
				await DisplayAlert("Error", "Fail control tak dijumpai. Sila hubung pejabat.", "OK");
				return Constants.Failed;
			}
			var compoundDto = new CompoundDto();
			compoundDto.CompType = Constants.CompType2;

			compoundDto.Deleted = Constants.RecActive;
			compoundDto.CompNum = txtCompound.Text;
			compoundDto.ActCode = _stringAct;
			compoundDto.OfdCode = _stringOffend;
			compoundDto.Mukim = _stringMukim;
			compoundDto.Zone = _stringZone;
			compoundDto.ZoneDesc = _stringZoneDesc;

			var splitData = GeneralBll.GetSplitData(txtLocation.Text, ' ');

			compoundDto.StreetCode = splitData.Code;
			compoundDto.StreetDesc = splitData.Description;
			compoundDto.CompDate = GeneralBll.GetLocalDateTime().ToString("yyyyMMdd");
			compoundDto.CompTime = GeneralBll.GetLocalDateTime().ToString("HHmmss"); //_compoundTimeStart;

			compoundDto.EnforcerId = infoDto.EnforcerId;
			compoundDto.WitnessId = _stringWitnessCode;
			compoundDto.PubWitness = _stringWitnessPub;
			compoundDto.Tujuan = _stringTujuan;
			compoundDto.Perniagaan = _stringPerniagaan;
			compoundDto.PrintCnt = "0";


			compoundDto.TotalPhoto = GlobalClass.FileImages.Count();

			compoundDto.Compound2Type.CarNum = txtCarNum.Text;
			compoundDto.Compound2Type.Category = txtCategory.Text.Length > 2 ? txtCategory.Text.Substring(0, 2) : "";

			splitData = GeneralBll.GetSplitData(txtMake.Text, ' ');
			compoundDto.Compound2Type.CarType = splitData.Code;
			compoundDto.Compound2Type.CarTypeDesc = splitData.Description;

			splitData = GeneralBll.GetSplitData(txtColor.Text, ' ');
			compoundDto.Compound2Type.CarColor = splitData.Code;
			compoundDto.Compound2Type.CarColorDesc = splitData.Description;

			splitData = GeneralBll.GetSplitData(txtSerah.Text, ' ');
			compoundDto.Compound2Type.DeliveryCode = splitData.Code; ;
			compoundDto.Compound2Type.RoadTax = txtRoadTax.Text;

			compoundDto.Compound2Type.CompAmt = _stringOffendAmount;
			compoundDto.Compound2Type.CompAmt2 = _stringOffendAmount2;
			compoundDto.Compound2Type.CompAmt3 = _stringOffendAmount3;

			compoundDto.TempohDate = GeneralBll.ConvertDateToYyyyMmDdFormat(_stringTempohDate);

			compoundDto.Kadar = "Y";
			splitData = GeneralBll.GetSplitData(txtTptJadi.Text, ' ');
			compoundDto.Tempatjadi = splitData.Description;

			compoundDto.NoteDesc = _stringNoteDesc;
			compoundDto = GlobalClass.SetGpsData(compoundDto);

			if (_stringZone == Constants.NewZoneCode)
				CompoundBll.SaveNote(_stringZoneDesc, compoundDto.CompNum, compoundDto.EnforcerId, "ZN");
			if (compoundDto.StreetCode == Constants.NewStreetCode)
			{
				CompoundBll.SaveNote(compoundDto.StreetDesc, compoundDto.CompNum, compoundDto.EnforcerId, "JL");
			}
			if (compoundDto.Compound2Type.CarType == Constants.NewCarTypeCode)
			{
				CompoundBll.SaveNote(compoundDto.Compound2Type.CarTypeDesc, compoundDto.CompNum, compoundDto.EnforcerId, "VH");
			}

			if (CompoundBll.SaveCompound(compoundDto) == Constants.Success)
			{
				//new Thread(() =>
				//{
				//    Thread.CurrentThread.IsBackground = true;
				//    this.RunOnUiThread(() => CompoundBll.ProcessCompoundOnlineService());
				//}).Start();

				return Constants.Success;
			}
			return Constants.Failed;
		}
		catch (Exception ex)
		{
			LogFile.WriteLogFile("FormCompaund2", "SaveCompound", ex.Message, Enums.LogType.Error);
			return Constants.Failed;
		}

	}


	private async void OnPrintValidated()
	{
		try
		{
			var result = await SaveCompound();
			if (result == Constants.Success)
			{
				//GeneralBll.PausedServiceSendCompound();
				GeneralBll.CopyImage2OnLineByCompoundNumber(txtCompound.Text);
				m_blSave = true;
				await DisplayAlert("INFO", "Kompaun " + txtCompound.Text + " berhasil disimpan ", "OK");
				PrintCompound(true);
			}
			else if (result == Constants.DuplicateCompoundNumber)
			{
				await DisplayAlert("ERROR", "Duplicate Compound Number, Sila Hubung Pejabat", "OK");
				return;
			}
			else
			{
				EnableControl(true);
				await DisplayAlert("ERROR", "Gagal menyimpan Kompaun.", "OK");
			}
		}
		catch (Exception ex)
		{
			EnableControl(true);
			await DisplayAlert("ERROR", Constants.UnexpectedErrorMessage, "OK");
			LogFile.WriteLogFile("FormCompaun2", "OnPrintValidated", ex.Message, Enums.LogType.Error);
		}
	}


	private async Task OnPrint()
	{
		try
		{
			if (m_blSave)
			{
				OnReprint();
			}
			else
			{
				var ad = await DisplayAlert("INFO", "Cetak Kompaun# " + txtCompound.Text, "Ya", "Tidak");
				if (ad)
				{
					if (await ValidateCompound())
					{
						EnableControl(false);
						OnPrintValidated();
						m_bPrint = true;
						SetNewControl(false);
					}
				}
			}
		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", Constants.UnexpectedErrorMessage, "OK");
			LogFile.WriteLogFile("FormCompaun2", "OnPrint", ex.Message, Enums.LogType.Error);
		}
	}

	private async void printMenu_Clicked(object sender, EventArgs e)
	{
		await OnPrint();
	}
}