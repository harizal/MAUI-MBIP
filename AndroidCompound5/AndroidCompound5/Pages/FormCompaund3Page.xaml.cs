using Android.Content;
using Android.Views;
using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.BusinessObject.DTOs.Responses;
using AndroidCompound5.Classes;
using Newtonsoft.Json;

namespace AndroidCompound5.Pages;

public partial class FormCompaund3Page : Microsoft.Maui.Controls.TabbedPage
{
	private string _stringMukim;
	private string _stringZone;
	private string _stringAct;
	private string _stringOffend;
	private string _stringOffendAmount;
	private string _stringOffendAmount2;
	private string _stringOffendAmount3;
	private string _stringKesalahan;
	private string _stringNoteDesc;
	private string _stringWitnessCode;
	private string _stringWitnessPub;
	private string _stringTujuan;
	private string _stringPerniagaan;
	private string _stringTempohDate = "";
	private string _stringCarNum = "";
	private string _stringCarCategory = "";
	private string _stringPetak = "";
	private string _stringNamaPesalah;
	private string _stringNoICPesalah;
	private string _stringIDPenguatkuasa;
	private string _stringNamaPenguatkuasa;
	private string _stringTempatJadi;
	Enums.ActiveForm _activeForm = Enums.ActiveForm.None;
	private const string ActivityName = "Compound3";
	private bool m_blSave = false, m_bCameraClick = false, m_bPrint = false;
	int iActive = 0;
	CustomLoading _modal;
	private static int REQUEST_CAMERA = 1001;
	private static int REQUEST_ALPR = 1002;
	private static int REQUEST_MYKAD = 1001;
	private DateTime dateTime;

	public FormCompaund3Page(string mukim, string zone, string descZone, string act, string offend, string offendAmount, string offendAmount2, string offendAmount3)
	{
		InitializeComponent();

		_stringMukim = mukim;
		_stringZone = zone;
		_stringAct = act;
		_stringOffend = offend;
		_stringOffend = offendAmount;
		_stringOffendAmount2 = offendAmount2;
		_stringOffendAmount3 = offendAmount3;

		_modal = new CustomLoading((ContentPage)this.Children[0]);
		SetInit();
	}

	private async void SetInit()
	{
		var infoDto = InfoBll.GetInfo();
		if (infoDto == null)
		{
			await DisplayAlert("INFO", "Fail control tak dijumpai. Sila hubung pejabat.", "OK");
			return;
		}


		_stringIDPenguatkuasa = infoDto.EnforcerId;
		var enforcer = EnforcerBll.GetEnforcerById(_stringIDPenguatkuasa);
		if (enforcer == null)
			_stringNamaPenguatkuasa = " ";
		else
			_stringNamaPenguatkuasa = enforcer.EnforcerName;

		var output = CompoundBll.GenerateCompoundNumber(infoDto);
		if (output.Result)
		{
			txtCompound.Text = output.Message;
		}
		else
		{
			await DisplayAlert("INFO", output.Message, "OK");
		}

		GlobalClass.FileImages = new List<string>();
		EnableControl(true);

		txtDelivery.Text = "";
		txtLocation.Text = "";

		txtRujukan.Text = "";
		txtCompany.Text = "";
		txtCompanyName.Text = "";
		txtIC.Text = "";
		txtName.Text = "";
		txtDelivery.Text = "";
		txtAddress1.Text = "";
		txtAddress2.Text = "";
		txtAddress3.Text = "";
		chkTempatJadi.IsChecked = false;
		await IsValidOffend();
		m_blSave = false;

		SetPrintControl(false);
	}

	private async Task<bool> IsValidOffend()
	{
		var offend = TableFilBll.GetOffendByCodeAndAct(_stringOffend, _stringAct);
		if (offend == null)
			return false;
		_stringOffend = offend.OfdCode;

		try
		{

			if (_stringTempohDate == "")
			{
				if (offend.PrintFlag == "Y")
				{
					dateTime = GeneralBll.AddDaysExcludeWeekEnd(dateTime, 3);
					_stringTempohDate = dateTime.ToString("d");
				}
			}
		}

		catch (Exception ex)
		{
			await DisplayAlert("INFO", "UnExpected Error isvalidoffend() : " + ex.Message, "OK");
		}

		return true;
	}


	private void EnableControl(bool enable)
	{
		IMenuItem menuItem;

		txtLocation.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		txtLocation.IsEnabled = enable;
		btnFindLocation.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		btnFindLocation.IsEnabled = enable;
		btnTempatJadi.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		btnTempatJadi.IsEnabled = enable;

		txtRujukan.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		txtRujukan.IsEnabled = enable;

		txtCompany.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		txtCompany.IsEnabled = enable;

		txtCompanyName.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		txtCompanyName.IsEnabled = enable;

		txtIC.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		txtIC.IsEnabled = enable;

		txtName.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		txtName.IsEnabled = enable;


		txtDelivery.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		txtDelivery.IsEnabled = enable;
		btnFindDelivery.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		btnFindDelivery.IsEnabled = enable;

		txtAddress1.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		txtAddress1.IsEnabled = enable;
		txtAddress2.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		txtAddress2.IsEnabled = enable;
		txtAddress3.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		txtAddress3.IsEnabled = enable;

		txtTempatJadi.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		txtTempatJadi.IsEnabled = enable;
		chkTempatJadi.IsEnabled = enable;

		rbIndividu.IsEnabled = enable;
		rbSyarikat.IsEnabled = enable;

		nextMenu.IsEnabled = enable;
		aktaMenu.IsEnabled = enable;
		//gambarMenu.IsEnabled = enable;
		cameraMenu.IsEnabled = enable;
		newMenu.IsEnabled = !enable;
	}

	private void EnablePrint()
	{
		string txtCompDesc = "";
		SetPrintControl(false);

		if (string.IsNullOrEmpty(txtCompany.Text) && string.IsNullOrEmpty(txtIC.Text) && string.IsNullOrEmpty(txtRujukan.Text)) return;
		if (string.IsNullOrEmpty(_stringKesalahan)) return;
		if (string.IsNullOrEmpty(_stringWitnessCode)) return;
		if (string.IsNullOrEmpty(txtDelivery.Text)) return;
		if (string.IsNullOrEmpty(txtLocation.Text)) return;
		if (string.IsNullOrEmpty(txtAddress1.Text)) return;

		if (GlobalClass.FileImages.Count < Constants.MinPhoto) return;

		SetPrintControl(true);
	}

	private void HandleAfterBackPage()
	{
		if (m_bCameraClick)
		{
			m_bCameraClick = false;
			LogFile.WriteLogFile("Camera Click : Finish");
		}

		if (GlobalClass.FindResult)
		{
			switch (_activeForm)
			{
				case Enums.ActiveForm.FindStreet:
					var sTemp = GlobalClass.ReturnCodeFind.Split(';');
					txtLocation.Text = sTemp[0] + " " + sTemp[1];
					_stringMukim = sTemp[2];
					_stringZone = sTemp[3];
					break;

				case Enums.ActiveForm.FindDelivery:
					txtDelivery.Text = GlobalClass.ReturnCodeFind.Replace(";", " ");
					break;

				case Enums.ActiveForm.FindTempatJadi:
					txtTempatJadi.Text = GlobalClass.ReturnCodeFind.Split(';')[1];
					break;

				case Enums.ActiveForm.FormAkta:
					var sResult1 = GlobalClass.ReturnCodeFind.Split(';');
					_stringAct = sResult1[0];
					_stringOffend = sResult1[1];
					_stringOffendAmount = sResult1[2];
					_stringOffendAmount2 = sResult1[3];
					_stringOffendAmount3 = sResult1[4];
					break;

				case Enums.ActiveForm.FormNext:
					var sResult = GlobalClass.ReturnCodeFind.Split(';');
					_stringWitnessCode = sResult[0];
					_stringKesalahan = sResult[1];
					_stringNoteDesc = sResult[2];
					_stringTujuan = sResult[3];
					_stringPerniagaan = sResult[4];
					_stringWitnessPub = sResult[5];
					_stringTempohDate = GlobalClass.ReturnCodeFind.Split(';')[6];

					break;
			}
			GlobalClass.FindResult = false;
			GlobalClass.ReturnCodeFind = "";
		}

		GlobalClass.FindResult = false;
		GlobalClass.ReturnCodeFind = "";
		_activeForm = Enums.ActiveForm.None;
		iActive = 0;
		EnablePrint();

		if (GlobalClass.FileImages == null)
		{
			GlobalClass.FileImages = new List<string>();
			GeneralAndroidClass.GetBackFileImage(txtCompound.Text);

		}
	}

	private bool IsValidDelivery()
	{
		bool result = false;

		if (txtDelivery.Text.Length >= 2)
		{
			string sDelivery = txtDelivery.Text.Substring(0, 2);
			var delivery = TableFilBll.GetDeliveryByCode(sDelivery);
			if (delivery != null)
			{
				txtDelivery.Text = delivery.Code + " " + delivery.ShortDesc;
				result = true;
			}
		}


		return result;
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
					result = true;
				}
			}

		}

		return result;
	}

	private async Task<bool> ValidateCompound()
	{
		if (rbSyarikat.IsChecked)
		{
			if (string.IsNullOrEmpty(txtCompanyName.Text))
			{
				await DisplayAlert("INFO", "Nama syarikat belum diisi", "OK");
				return false;
			}
		}

		if (string.IsNullOrEmpty(txtAddress1.Text))
		{
			await DisplayAlert("INFO", "Alamat belum diisi", "OK");
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

		if (!GeneralBll.IsAlphaNumericAndMinusSlash(txtRujukan.Text))
		{
			await DisplayAlert("INFO", "No SSM : Terdapat character tidak sah. Sila semak input anda.", "OK");
			txtRujukan.Focus();
			return false;
		}

		if (!GeneralBll.IsAlphaNumericAndMinusSlash(txtCompany.Text))
		{
			await DisplayAlert("INFO", "No Syarikat : Terdapat character tidak sah. Sila semak input anda.", "OK");
			txtCompany.Focus();
			return false;
		}

		if (!GeneralBll.IsAlphaNumericAndOthers1(txtCompanyName.Text))
		{
			await DisplayAlert("INFO", "Nama Syarikat : Terdapat character tidak sah. Sila semak input anda.", "OK");
			txtCompanyName.Focus();
			return false;
		}

		if (!GeneralBll.IsAlphaNumericAndMinus(txtIC.Text))
		{
			await DisplayAlert("INFO", "No IC : Terdapat character tidak sah. Sila semak input anda.", "OK");
			txtIC.Focus();
			return false;
		}

		if (!GeneralBll.IsAlphaNumericAndOthers1(txtName.Text))
		{
			await DisplayAlert("INFO", "Nama Pelanggar : Terdapat character tidak sah. Sila semak input anda.", "OK");
			txtName.Focus();
			return false;
		}

		if (!GeneralBll.IsAlphaNumericAndOthers1(txtTempatJadi.Text))
		{
			await DisplayAlert("INFO", "Tempat Kejadian: Terdapat character tidak sah. Sila semak input anda.", "OK");
			txtTempatJadi.Focus();
			return false;
		}

		if (!GeneralBll.IsAlphaNumericAndOthers1(txtAddress1.Text))
		{
			await DisplayAlert("INFO", "Alamat 1 : Terdapat character tidak sah. Sila semak input anda.", "OK");
			txtAddress1.Focus();
			return false;
		}

		if (!GeneralBll.IsAlphaNumericAndOthers1(txtAddress2.Text))
		{
			await DisplayAlert("INFO", "Alamat 2 : Terdapat character tidak sah. Sila semak input anda.", "OK");
			txtAddress2.Focus();
			return false;
		}

		if (!GeneralBll.IsAlphaNumericAndOthers1(txtAddress3.Text))
		{
			await DisplayAlert("INFO", "Alamat 3 : Terdapat character tidak sah. Sila semak input anda.", "OK");
			txtAddress3.Focus();
			return false;
		}
		return true;


	}

	private void SetPrintControl(bool blValue)
	{
		printMenu.IsEnabled = blValue;
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
			var activity = Platform.CurrentActivity;
			activity?.StartActivityForResult(intent, REQUEST_CAMERA);
		}
		catch (Exception ex)
		{
			await DisplayAlert("Info", ex.Message, "OK");
		}
	}

	private async void newMenu_Clicked(object sender, EventArgs e)
	{
		try
		{
			EnableControl(true);
			SetInit();
		}
		catch (Exception ex)
		{
			await DisplayAlert("Info", ex.Message, "OK");
			LogFile.WriteLogFile("FormCompaun3", "OnNew", ex.Message, Enums.LogType.Error);
		}
	}

	private async void aktaMenu_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new AktaPage(_stringAct, _stringOffend, _stringOffendAmount, _stringOffendAmount2, _stringOffendAmount3, Constants.CompType3));
	}

	private async void cardReaderMenu_Clicked(object sender, EventArgs e)
	{
		try
		{
			var intent = new Intent(Intent.ActionMain);
			string packageName = "my.com.aimforce.bluetooth_printer";
			string className = "my.com.aimforce.bluetooth_printer.activity.QueryActivity";
			intent.SetComponent(new ComponentName(packageName, className));
			intent.PutExtra(Intent.ExtraText, "smart-card");

			var activity = Platform.CurrentActivity;
			activity?.StartActivityForResult(intent, REQUEST_MYKAD);
			EnablePrint();
		}
		catch (Exception ex)
		{
			await DisplayAlert("Info", ex.Message, "OK");
			LogFile.WriteLogFile("FormCompaun3", "cardReaderMenu_Clicked", ex.Message, Enums.LogType.Error);

		}
	}

	public static bool IsPrinterExist()
	{
		GlobalClass.BluetoothAndroid = new BluetoothAndroid();

		var openresult = GlobalClass.BluetoothAndroid.BluetoothOpen();

		if (!openresult.Succes)
		{
			return false;
		}

		var result = GlobalClass.BluetoothAndroid.BluetoothScan();
		return result != 0;
	}

	private async void OnPrinting()
	{
		try
		{
			var insPrint = new PrintBll();
			var printResponse = insPrint.BluetoothConnect(GlobalClass.BluetoothDevice);
			if (printResponse.Succes)
			{
				insPrint.PrintCompound(txtCompound.Text);
				CompoundBll.UpdatePrintCompound(txtCompound.Text);
			}
			else
			{
				GlobalClass.BluetoothDevice = null;
				await DisplayAlert("Info", printResponse.Message, "OK");
			}
		}
		catch (Exception ex)
		{
			await DisplayAlert("Info", "UnExpected Error OnPrinting() : " + ex.Message, "OK");
			LogFile.WriteLogFile("UnExpected Error OnPrinting() : " + ex.Message, Enums.LogType.Error);
			LogFile.WriteLogFile("Stack Trace " + ex.StackTrace, Enums.LogType.Error);
		}

	}

	private async void PreparePrinterDevice()
	{
		try
		{
			if (!IsPrinterExist())
			{
				await DisplayAlert("Info", "Printer Device not found.", "OK");
				return;
			}

			if (GlobalClass.BluetoothDevice == null)
			{
				var customLoading = new CustomLoading((ContentPage)this.Children[0], true, () =>
				{
					PrintCompound(false);
				});
				MainThread.BeginInvokeOnMainThread(() => customLoading.ShowPopupAsync());

				//lvResult = new ListView(this);
				//var adapter = new DeviceListAdapter(this, GlobalClass.BluetoothAndroid._listDevice);
				//lvResult.Adapter = adapter;
				//lvResult.ItemClick += lvResult_ItemClick;

				//AlertDialog.Builder builder = new AlertDialog.Builder(this);
				//_alert = builder.Create();
				//_alert.SetMessage("Select your item");
				//_alert.SetView(lvResult);

				//_alert.Show();
			}
		}
		catch (Exception ex)
		{
			await DisplayAlert("Info", ex.Message, "OK");
			LogFile.WriteLogFile("FormCompaun3", "PreparePrinterDevice", ex.Message, Enums.LogType.Error);
		}

	}

	private async void PrintCompound(bool isNeedCheck)
	{
		try
		{
			if (isNeedCheck)
			{
				PreparePrinterDevice();

				if (GlobalClass.BluetoothDevice == null)
				{
					return;
				}
			}
			OnPrinting();

		}
		catch (Exception ex)
		{
			await DisplayAlert("Info", ex.Message, "OK");
			LogFile.WriteLogFile("FormCompaun3", "OnReprint", ex.Message, Enums.LogType.Error);
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
			await DisplayAlert("Info", ex.Message, "OK");
			LogFile.WriteLogFile("FormCompaun3", "OnReprint", ex.Message, Enums.LogType.Error);
		}
	}

	private async Task<int> SaveCompound()
	{
		try
		{
			var infoDto = InfoBll.GetInfo();
			if (infoDto == null)
			{
				await DisplayAlert("INFO", "Fail control tak dijumpai. Sila hubung pejabat.", "OK");
				return Constants.Failed;
			}
			var compoundDto = new CompoundDto();
			compoundDto.CompType = Constants.CompType3;

			compoundDto.Deleted = Constants.RecActive;
			compoundDto.CompNum = txtCompound.Text;
			compoundDto.ActCode = _stringAct;
			compoundDto.OfdCode = _stringOffend;
			compoundDto.Mukim = _stringMukim;
			compoundDto.Zone = _stringZone;
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

			if (txtTempatJadi.Text == "SEPERTI DI ATAS")
				txtTempatJadi.Text = "Seperti Di atas";
			else
			{
				txtTempatJadi.Text = txtTempatJadi.Text + "," + compoundDto.StreetDesc;
			}
			compoundDto.Tempatjadi = txtTempatJadi.Text;

			compoundDto.TotalPhoto = GlobalClass.FileImages.Count;

			compoundDto.Compound3Type.Rujukan = txtRujukan.Text;
			compoundDto.Compound3Type.Company = txtCompany.Text;
			compoundDto.Compound3Type.CompanyName = txtCompanyName.Text;
			compoundDto.Compound3Type.OffenderIc = txtIC.Text;
			compoundDto.Compound3Type.OffenderName = txtName.Text;

			compoundDto.Compound3Type.Address1 = txtAddress1.Text;
			compoundDto.Compound3Type.Address2 = txtAddress2.Text;
			compoundDto.Compound3Type.Address3 = txtAddress3.Text;

			compoundDto.Compound3Type.CompAmt = _stringOffendAmount;
			compoundDto.Compound3Type.CompAmt2 = _stringOffendAmount2;
			compoundDto.Compound3Type.CompAmt3 = _stringOffendAmount3;
			compoundDto.Compound3Type.DeliveryCode = txtDelivery.Text;
			compoundDto.Compound3Type.CompDesc = _stringKesalahan;
			compoundDto.NoteDesc = _stringNoteDesc;
			compoundDto.TempohDate = GeneralBll.ConvertDateToYyyyMmDdFormat(_stringTempohDate);

			var offend = TableFilBll.GetOffendByCodeAndAct(_stringOffend, _stringAct);
			if (offend == null)
				compoundDto.Kadar = "N";
			else
				compoundDto.Kadar = offend.PrintFlag;

			//if (_stringZone == Constants.NewZoneCode)
			//    CompoundBll.SaveNote(_stringZoneDesc, compoundDto.CompNum, compoundDto.EnforcerId, "ZN");

			//compoundDto = GlobalClass.SetGpsData(compoundDto);


			return CompoundBll.SaveCompound(compoundDto);
		}
		catch (Exception ex)
		{
			await DisplayAlert("Info", ex.Message, "OK");
			LogFile.WriteLogFile("FormCompaun3", "SAVE", ex.Message, Enums.LogType.Error);
		}

		return Constants.Failed;
	}

	private async void OnPrintValidated()
	{
		try
		{
			var result = await SaveCompound();
			if (result == Constants.Success)
			{
				m_blSave = true;
				await DisplayAlert("INFO", "Kompaun " + txtCompound.Text + " berhasil disimpan ", "OK");
				PrintCompound(true);
			}
			else if (result == Constants.DuplicateCompoundNumber)
			{
				await DisplayAlert("INFO", "Duplicate Compound Number, Sila Hubung Pejabat", "OK");
				return;
			}
			else
			{
				EnableControl(true);
				await DisplayAlert("INFO", "Gagal menyimpan Kompaun.", "OK");
			}
		}
		catch (Exception ex)
		{
			EnableControl(true);

			await DisplayAlert("INFO", Constants.UnexpectedErrorMessage, "OK");
			LogFile.WriteLogFile("Compaund3", "OnPrintValidated", ex.Message, Enums.LogType.Error);


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

					}

				}
			}

		}
		catch (Exception ex)
		{
			await DisplayAlert("Info", ex.Message, "OK");
			LogFile.WriteLogFile("FormCompaun3", "OnPrint", ex.Message, Enums.LogType.Error);
		}
	}

	private async void printMenu_Clicked(object sender, EventArgs e)
	{
		await OnPrint();
	}

	private void enablePrint_TextChanged(object sender, TextChangedEventArgs e)
	{
		try
		{
			EnablePrint();
		}
		catch { }
	}

	private async Task StartActivityFind(string sFindType)
	{
		GlobalClass.FindResult = false;		
		await Navigation.PushAsync(new CarianPage(HandleAfterBackPage, sFindType, ActivityName, _stringMukim, _stringZone, string.Empty, string.Empty));
	}

	private async void btnFindDelivery_Clicked(object sender, EventArgs e)
	{
		try
		{
			_activeForm = Enums.ActiveForm.FindDelivery;
			await StartActivityFind(Constants.FindDelivery);
		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
		}
	}

	private async void btnFindLocation_Clicked(object sender, EventArgs e)
	{
		try
		{
			_activeForm = Enums.ActiveForm.FindStreet;
			await StartActivityFind(Constants.FindStreet);
		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
		}
	}

	private async void txtDelivery_Unfocused(object sender, FocusEventArgs e)
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

	private void txtTempatJadi_Unfocused(object sender, FocusEventArgs e)
	{
		_stringTempatJadi = txtTempatJadi.Text;
	}

	private void txtName_Unfocused(object sender, FocusEventArgs e)
	{
		_stringNamaPesalah = txtName.Text;
	}

	private void txtIC_Unfocused(object sender, FocusEventArgs e)
	{
		_stringNoICPesalah = txtIC.Text;
	}

	private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
	{
		if (rbIndividu.IsChecked)
		{
			borderRujukan.IsVisible = false;
			lblRujukan.IsVisible = false;

			llNoSyarikat.IsVisible = false;
			noSyarikatSection.IsVisible = false;

			llNamaSyarikat.IsVisible = false;
			borderNamaSyarikat.IsVisible = false;

			txtCompany.Text = string.Empty;
			txtCompanyName.Text = string.Empty;
			txtRujukan.Text = string.Empty;
		}

		if (rbSyarikat.IsChecked)
		{
			borderRujukan.IsVisible = true;
			lblRujukan.IsVisible = true;

			llNoSyarikat.IsVisible = true;
			noSyarikatSection.IsVisible = true;

			llNamaSyarikat.IsVisible = true;
			borderNamaSyarikat.IsVisible = true;
		}
	}

	private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
	{
		txtTempatJadi.Text = chkTempatJadi.IsChecked ? "SEPERTI DI ATAS" : "";
		txtTempatJadi.IsEnabled = !chkTempatJadi.IsChecked;
		txtTempatJadi.Background = !chkTempatJadi.IsChecked ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
	}

	private async void GetCompoundDetails(bool isNoIC, string value)
	{
		var parameters = "";
		var modal = new CustomLoading((ContentPage)this.Children[0]);
		MainThread.BeginInvokeOnMainThread(() => modal.ShowPopupAsync());

		try
		{
			MainThread.BeginInvokeOnMainThread(() => modal.UpdateMessage("Please wait..."));
			if (!GeneralBll.IsOnline())
			{
				MainThread.BeginInvokeOnMainThread(async () => await DisplayAlert("INFO", "No Internet Connection !!", "OK"));
				return;
			}

			var compoundDetails = CompoundBll.GetCompoundDetails(isNoIC, value);

#if DEBUG
			compoundDetails = [];
			for (int i = 0; i<10; i++)
			{
				compoundDetails.Add(new CompoundDetailDto
				{
					tarikhkmp = DateTime.Now,
					nokompaun = $"No Kompaun {i}",
					nokenderaan = $"No Kendaraan {i}",
					kesalahan = $"Kesalahan {i}",
					noic = $"No IC {i}",
					namasyarikat = $"Nama Syarikat {i}",
					nosyarikat = $"No Syarikat {i}"
				});
			}
#endif

			if (compoundDetails.Any())
			{
				if (!string.IsNullOrEmpty(compoundDetails[0].ERROR))
				{
					MainThread.BeginInvokeOnMainThread(async () => await DisplayAlert("INFO", compoundDetails[0].ERROR, "OK"));
				}
				else
				{
					parameters = JsonConvert.SerializeObject(compoundDetails);
				}
			}
			else
				MainThread.BeginInvokeOnMainThread(async () => await DisplayAlert("INFO", "Dont have compound", "OK"));

		}
		catch (Exception ex)
		{
			MainThread.BeginInvokeOnMainThread(async () => await DisplayAlert("INFO", ex.Message, "OK"));
		}
		finally
		{
			MainThread.BeginInvokeOnMainThread(() =>
			{
				modal.ClosePopup();
			});

			if (!string.IsNullOrEmpty(parameters))
			{
				await Navigation.PushAsync(new CompaundDetailPage(parameters));
			}
		}
	}

	private async void btnFindCompany_Clicked(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(txtCompany.Text))
			await DisplayAlert("ERROR", "No Syarikat is empty", "OK");
		else
		{
			GetCompoundDetails(false, txtCompany.Text);
		}
	}

	private async void nextMenu_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(
			new FormCompaund12Page(txtCompound.Text, _stringOffendAmount, _stringOffendAmount2, _stringOffendAmount3,
			_stringWitnessCode, _stringWitnessPub, _stringKesalahan, _stringNoteDesc, _stringAct, _stringOffend,
			_stringTujuan, _stringPerniagaan, _stringTempohDate, _stringCarNum, _stringCarCategory, _stringPetak, _stringNamaPesalah,
			_stringNoICPesalah, _stringIDPenguatkuasa, _stringNamaPenguatkuasa, _stringTempatJadi));
	}

	private async void btnFindIC_Clicked(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(txtCompany.Text))
			await DisplayAlert("ERROR", "No Syarikat is empty", "OK");
		else
		{
			GetCompoundDetails(false, txtIC.Text);
		}
	}

	private async void btnTempatJadi_Clicked(object sender, EventArgs e)
	{
		try
		{
			_activeForm = Enums.ActiveForm.FindTempatJadi;
			await StartActivityFind(Constants.FindTempatJadi);

		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
		}

	}
}