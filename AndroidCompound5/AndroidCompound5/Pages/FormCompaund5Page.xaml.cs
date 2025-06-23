using Android.Content;
using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.Classes;
using Newtonsoft.Json;
using System.Globalization;

namespace AndroidCompound5.Pages;

public partial class FormCompaund5Page : ContentPage
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
	private string _stringLockKey;
	private string _stringUnLock;
	private string _stringTow;
	private string _stringTempohDate = "";
	private string _stringCarNum;
	private string _stringCarCategory;
	private string _stringPetak = "";
	private string _stringNamaPesalah = "";
	private string _stringNoICPesalah = "";
	private string _stringIDPenguatkuasa;
	private string _stringNamaPenguatkuasa;
	private string _stringTempatJadi;

	private DateTime dateTime;
	private string _compoundTimeStart;
	private bool m_blSave = false, m_bPrint = false;

	Enums.ActiveForm _activeForm = Enums.ActiveForm.None;
	private static int REQUEST_CAMERA = 1001;

	public FormCompaund5Page(string mukim, string zone, string descZone, string act, string offend, string offendAmount, string offendAmount2, string offendAmount3)
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

	private async void SetInit()
	{
		try
		{
			EnableControl(true);

			var infoDto = InfoBll.GetInfo();
			if (infoDto == null)
			{
				await DisplayAlert("ERROR", "Fail control tak dijumpai. Sila hubung pejabat.", "OK");
				return;
			}

			_stringIDPenguatkuasa = infoDto.EnforcerId;
			var enforcer = EnforcerBll.GetEnforcerById(_stringIDPenguatkuasa);
			if (enforcer == null)
				_stringNamaPenguatkuasa = " ";
			else
				_stringNamaPenguatkuasa = enforcer.EnforcerName;

			_compoundTimeStart = GeneralBll.GetLocalDateTime().ToString("HHmm");
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
			dateTime = GeneralBll.GetLocalDateTime();
			UpdatedDisplay();

			//_stringWitnessCode = "";
			_stringWitnessPub = "";
			//_stringKesalahan = "";
			_stringNoteDesc = "";

			txtCarNum.Text = "";
			txtRoadTax.Text = "";
			txtCategory.Text = "";
			txtMake.Text = "";
			txtColor.Text = "";
			txtPerenggan.Text = _stringOffend;
			IsValidOffend();
			//txtLocation.Text = "";
			txtTptJadi.Text = "";
			txtSerah.Text = "";
			//txtMuatan.Text = "";

			m_blSave = false;

			_stringWitnessCode = "";
			_stringWitnessPub = "";
			_stringKesalahan = "";
			_stringNoteDesc = "";


		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
			LogFile.WriteLogFile(typeof(FormCompaund5Page).Name, "SetInit", ex.Message, Enums.LogType.Error);
		}
	}

	private void UpdatedDisplay()
	{
		DateTimeFormatInfo dtfi = CultureInfo.CreateSpecificCulture("en-US").DateTimeFormat;
		dtfi.ShortDatePattern = @"dd/MM/yyyy";
		btnRoadTaxDate.Date = dateTime;
	}

	private void EnableControl(bool enable)
	{
		txtLocation.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		txtLocation.IsEnabled = enable;

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

		//txtMuatan.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		//txtMuatan.IsEnabled = enable;

		btnRoadTaxDate.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		btnRoadTaxDate.IsEnabled = enable;


		nextMenu.IsEnabled = enable;
		cameraMenu.IsEnabled = enable;
		newMenu.IsEnabled = enable;
	}

	private async void nextMenu_Clicked(object sender, EventArgs e)
	{
		_activeForm = Enums.ActiveForm.FormNext;
		await Navigation.PushAsync(new FormCompaund52Page(
					txtCompound.Text,
					_stringOffendAmount,
					_stringOffendAmount2,
					_stringOffendAmount3,
					_stringWitnessCode,
					_stringWitnessPub,
					_stringKesalahan,
					_stringNoteDesc,
					_stringAct,
					_stringOffend,
					_stringLockKey,
					_stringUnLock,
					_stringTow,
					_stringTempohDate,
					_stringCarNum,
					_stringCarCategory,
					_stringPetak,
					_stringNamaPesalah,
					_stringNoICPesalah,
					_stringIDPenguatkuasa,
					_stringNamaPenguatkuasa,
					_stringTempatJadi));
		EnablePrint();
	}

	private async Task<bool> IsValidOffend()
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
					dateTime = GeneralBll.AddDaysExcludeWeekEnd(dateTime, 3);
					_stringTempohDate = dateTime.ToString("d");
				}
			}
		}

		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
		}

		return true;
	}

	private void SetPrintControl(bool blValue)
	{
		printMenu.IsEnabled = blValue;
	}

	private void EnablePrint()
	{
		SetPrintControl(false);

		if (string.IsNullOrEmpty(txtCarNum.Text)) return;
		if (string.IsNullOrEmpty(txtCategory.Text)) return;
		if (string.IsNullOrEmpty(txtMake.Text)) return;
		if (string.IsNullOrEmpty(txtPerenggan.Text)) return;
		if (string.IsNullOrEmpty(txtLocation.Text)) return;
		//            if (string.IsNullOrEmpty(_stringKesalahan)) return;
		if (string.IsNullOrEmpty(_stringWitnessCode)) return;
		if (string.IsNullOrEmpty(txtSerah.Text)) return;

		if (GlobalClass.FileImages.Count < Constants.MinPhoto) return;

		SetPrintControl(true);

	}

	private async void HandleAfterBackPage()
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
					break;
				case Enums.ActiveForm.FindDelivery:
					txtSerah.Text = GlobalClass.ReturnCodeFind.Replace(";", " ");
					break;
				case Enums.ActiveForm.FormNext:
					_stringWitnessCode = GlobalClass.ReturnCodeFind.Split(';')[0];
					_stringKesalahan = GlobalClass.ReturnCodeFind.Split(';')[1];
					_stringNoteDesc = GlobalClass.ReturnCodeFind.Split(';')[2];
					_stringLockKey = GlobalClass.ReturnCodeFind.Split(';')[3];
					_stringUnLock = GlobalClass.ReturnCodeFind.Split(';')[4];
					_stringTow = GlobalClass.ReturnCodeFind.Split(';')[5];
					_stringWitnessPub = GlobalClass.ReturnCodeFind.Split(';')[6];
					_stringTempohDate = GlobalClass.ReturnCodeFind.Split(';')[7];
					break;
				case Enums.ActiveForm.FindOffend2:
					_stringOffend = GlobalClass.ReturnCodeFind.Split(';')[0];
					await IsValidOffend();
					txtPerenggan.Text = GlobalClass.ReturnCodeFind.Replace(";", " ");
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

		if (GlobalClass.FileImages == null)
		{
			GlobalClass.FileImages = new List<string>();
			GeneralAndroidClass.GetBackFileImage(txtCompound.Text);
		}
	}

	private async void StartActivityFind(string sFindType)
	{
		GlobalClass.FindResult = false;
		await Navigation.PushAsync(new CarianPage(HandleAfterBackPage, sFindType, "", _stringMukim, _stringZone, "", "", carcategory: txtCategory.Text));
	}

	private void btnFindCategory_Clicked(object sender, EventArgs e)
	{
		_activeForm = Enums.ActiveForm.FindCategory;
		StartActivityFind(Constants.FindCarCategory);
	}

	private void btnFindMake_Clicked(object sender, EventArgs e)
	{
		_activeForm = Enums.ActiveForm.FindModel;
		StartActivityFind(Constants.FindCarType);
	}

	private void btnFindColor_Clicked(object sender, EventArgs e)
	{
		_activeForm = Enums.ActiveForm.FindColor;
		StartActivityFind(Constants.FindCarColor);
	}

	private async void btnFindPerenggan_Clicked(object sender, EventArgs e)
	{
		_activeForm = Enums.ActiveForm.FindOffend2;
		GlobalClass.FindResult = false;

		await Navigation.PushAsync(new FindOffendPage(HandleAfterBackPage, Constants.FindOffend2, "", _stringAct, Constants.CompType2));
	}

	private void btnFindLocation_Clicked(object sender, EventArgs e)
	{
		_activeForm = Enums.ActiveForm.FindStreet;
		StartActivityFind(Constants.FindStreet);
	}

	private void btnFindTptJadi_Clicked(object sender, EventArgs e)
	{
		_activeForm = Enums.ActiveForm.FindTempatJadi;
		StartActivityFind(Constants.FindTempatJadi);
	}

	private void btnFindSerah_Clicked(object sender, EventArgs e)
	{
		_activeForm = Enums.ActiveForm.FindDelivery;
		StartActivityFind(Constants.FindDelivery);
	}

	private bool IsValidCategory()
	{
		var splitData = GeneralBll.GetSplitData(txtCategory.Text, ' ');
		var category = TableFilBll.GetCarCategory(splitData.Code);
		if (category == null)
			return false;

		txtCategory.Text = category.Carcategory + " " + category.ShortDesc;
		_stringCarCategory = category.ShortDesc;

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

	private bool IsValidTempatJadi()
	{
		var splitData = GeneralBll.GetSplitData(txtTptJadi.Text, ' ');
		var tempatJadi = TableFilBll.GetTempatJadi(splitData.Code);
		if (tempatJadi == null)
			return false;

		txtTptJadi.Text = tempatJadi.Code + " " + tempatJadi.Description;
		return true;
	}


	private bool IsValidLocation()
	{
		int i;
		bool result = false;

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

	private bool IsValidDelivery()
	{
		var splitData = GeneralBll.GetSplitData(txtSerah.Text, ' ');
		var delivery = TableFilBll.GetDeliveryByCode(splitData.Code);
		if (delivery == null)
			return false;

		txtSerah.Text = delivery.Code + " "  + delivery.ShortDesc;
		return true;
	}


	private async Task<bool> ValidateCompound()
	{
		if (!IsValidCategory())
		{
			await DisplayAlert("INFO", "Kod Jenis tidak sah", "OK");
			return false;
		}

		if (!IsValidColor())
		{
			await DisplayAlert("INFO", "Kod Warna tidak sah", "OK");
			return false;
		}
		if (!IsValidModel())
		{
			await DisplayAlert("INFO", "Kod Model tidak sah", "OK");
			return false;
		}
		if (!await IsValidOffend())
		{
			await DisplayAlert("INFO", "Kod Offend tidak sah", "OK");
			return false;
		}


		if (!IsValidLocation())
		{
			await DisplayAlert("INFO", "Kod lokasi tidak sah", "OK");
			return false;
		}

		if (!IsValidDelivery())
		{
			await DisplayAlert("INFO", "Kod cara serahan tidak sah", "OK");
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

		//if (!GeneralBll.IsAlphaNumeric(txtMuatan.Text))
		//{
		//	await DisplayAlert("INFO", "Muatan : Terdapat character tidak sah. Sila semak input anda.", "OK");
		//	txtMuatan.RequestFocus();
		//	return false;
		//}
		return true;
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
			IsValidColor(); ;
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
			await IsValidOffend();
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

	private async void txtCarNum_Unfocused(object sender, FocusEventArgs e)
	{
		try
		{
			_stringCarNum = txtCarNum.Text;
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

	void EnablePrint_TextChanged(object sender, TextChangedEventArgs e)
	{
		try
		{
			EnablePrint();
		}
		catch { }
	}

	private async void OnPhoto()
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

	private async void OnNew()
	{

		try
		{
			EnableControl(true);
			SetInit();
		}
		catch (Exception ex)
		{
			await DisplayAlert("Info", ex.Message, "OK");
		}
	}

	private void cameraMenu_Clicked(object sender, EventArgs e)
	{
		OnPhoto();
	}

	private void newMenu_Clicked(object sender, EventArgs e)
	{
		OnNew();
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
				var customLoading = new CustomLoading(this, true, () =>
				{
					PrintCompound(false);
				});
				MainThread.BeginInvokeOnMainThread(() => customLoading.ShowPopupAsync());
			}
		}
		catch (Exception ex)
		{
			await DisplayAlert("Info", ex.Message, "OK");
			LogFile.WriteLogFile("FormCompaun3", "PreparePrinterDevice", ex.Message, Enums.LogType.Error);
		}

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
			LogFile.WriteLogFile("FormCompaun5", "OnReprint", ex.Message, Enums.LogType.Error);
		}
	}

	private async Task<int> SaveCompound()
	{
		try
		{
			var infoDto = InfoBll.GetInfo();
			if (infoDto == null)
			{
				await DisplayAlert("ERROR", "Fail control tak dijumpai. Sila hubung pejabat.", "OK");
				return Constants.Failed;
			}
			var compoundDto = new CompoundDto();
			compoundDto.CompType = Constants.CompType5;

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
			compoundDto.CompTime = GeneralBll.GetLocalDateTime().ToString("HHmmss"); ;
			compoundDto.EnforcerId = infoDto.EnforcerId;
			compoundDto.WitnessId = _stringWitnessCode;
			compoundDto.PubWitness = _stringWitnessPub;
			compoundDto.PrintCnt = "0";
			compoundDto.TotalPhoto = GlobalClass.FileImages.Count();


			compoundDto.Compound5Type.CarNum = txtCarNum.Text;
			compoundDto.Compound5Type.Category = txtCategory.Text.Length > 2 ? txtCategory.Text.Substring(0, 2) : "";

			splitData = GeneralBll.GetSplitData(txtMake.Text, ' ');

			compoundDto.Compound5Type.CarType = splitData.Code;
			compoundDto.Compound5Type.CarTypeDesc = splitData.Description;
			//LogFile.WriteLogFile("Car Type : " + txtMake.Text);

			splitData = GeneralBll.GetSplitData(txtColor.Text, ' ');
			compoundDto.Compound5Type.CarColor = splitData.Code;
			compoundDto.Compound5Type.CarColorDesc = splitData.Description;

			compoundDto.Compound5Type.RoadTax = txtRoadTax.Text;
			compoundDto.Compound5Type.RoadTaxDate = GeneralBll.ConvertDateToYyyyMmDdFormat(btnRoadTaxDate.Date.ToString("dd/MM/yyyy"));
			compoundDto.Compound5Type.CompAmt = _stringOffendAmount;
			compoundDto.Compound5Type.CompAmt2 = _stringOffendAmount2;
			compoundDto.Compound5Type.CompAmt3 = _stringOffendAmount3;
			compoundDto.Compound5Type.LockTime = GeneralBll.GetLocalDateTime().ToString("HHmm"); ;
			compoundDto.Compound5Type.LockKey = _stringLockKey;
			compoundDto.Compound5Type.UnlockAmt = GeneralBll.FormatAmountInCents(_stringUnLock);
			compoundDto.Compound5Type.TowAmt = GeneralBll.FormatAmountInCents(_stringTow);

			compoundDto.TempohDate = GeneralBll.ConvertDateToYyyyMmDdFormat(_stringTempohDate);

			splitData = GeneralBll.GetSplitData(txtSerah.Text, ' ');
			compoundDto.Compound5Type.DeliveryCode = splitData.Code;
			compoundDto.Compound5Type.CompDesc = _stringKesalahan;

			compoundDto.Kadar = "Y";
			splitData = GeneralBll.GetSplitData(txtTptJadi.Text, ' ');
			compoundDto.Tempatjadi = splitData.Description;

			compoundDto.NoteDesc = _stringNoteDesc;
			compoundDto = GlobalClass.SetGpsData(compoundDto);

			if (_stringZone == Constants.NewZoneCode)
				CompoundBll.SaveNote(_stringZoneDesc, compoundDto.CompNum, compoundDto.EnforcerId, "ZN");
			if (txtMake.Text == Constants.NewCarTypeCode)
				CompoundBll.SaveNote(txtMake.Text, compoundDto.CompNum, compoundDto.EnforcerId, "VH");

			return CompoundBll.SaveCompound(compoundDto);
		}
		catch (Exception ex)
		{
			LogFile.WriteLogFile("Compaund5", "SaveCompound ", ex.Message, Enums.LogType.Error);
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
			LogFile.WriteLogFile("Compaund5", "OnPrintValidated", ex.Message, Enums.LogType.Error);


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
}