using Android.Content;
using Android.Graphics;
using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.Classes;
using System.Globalization;
using Color = Microsoft.Maui.Graphics.Color;

namespace AndroidCompound5.Pages;

public partial class FormCompaund1Page : ContentPage
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
	private string _stringPetak;
	private string _stringNamaPesalah = "";
	private string _stringNoICPesalah = "";
	private string _stringIDPenguatkuasa;
	private string _stringNamaPenguatkuasa;
	private string _stringTempatJadi;
	private string _stringScanDateTime = "";
	private string _stringScanCarNum = "";

	private string _compoundTimeStart;
	private bool m_blSave = false, m_bCameraClick = false, m_bPrint = false;

	Enums.ActiveForm _activeForm = Enums.ActiveForm.None;
	private static int REQUEST_CAMERA = 1001;
	private static int REQUEST_ALPR = 1002;

	const int DATE_DIALOG_ID = 0;
	const int TIME_DIALOG_ID = 1;
	private int Date;
	private DateTime dateTime, dtPrintDateTime;
	private int hour;
	private int minute;

	//ServicetHandler handler;
	public const int MESSAGE_DEVICE_NAME = 1;
	public const int MESSAGE_TOAST = 2;
	public const int MESSAGE_READ = 3;
	public const string DEVICE_NAME = "device_name";

	public FormCompaund1Page(string mukim, string zone, string zoneDesc, string actCode, string offendCode, string offendamount, string offendamount2, string offendamount3)
	{
		InitializeComponent();

		_stringMukim = mukim;
		_stringZone = zone;
		_stringZoneDesc = zoneDesc;
		_stringAct = actCode;
		_stringOffend = offendCode;
		_stringOffendAmount = offendamount;
		_stringOffendAmount2 = offendamount2;
		_stringOffendAmount3 = offendamount3;
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();
		EnablePrint();
	}

	public async void SetInit()
	{
		try
		{
			var configDto = GeneralBll.GetConfig();
			if (configDto == null)
			{
				LogFile.WriteLogFile("Get Config null", Enums.LogType.Error);
			}

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
				await DisplayAlert("Error", output.Message, "OK");
			}

			GlobalClass.FileImages = [];

			_stringWitnessPub = "";
			_stringNoteDesc = "";

			txtCarNum.Text = "";
			txtRoadTax.Text = "";
			txtMake.Text = "";
			txtColor.Text = "";
			txtPerenggan.Text = "";
			txtPetak.Text = "";
			//txtAfCode.Text = "";
			_stringScanDateTime = "";

			m_blSave = false;

			//chkDateTime.Checked = false;
			//btnDate.Enabled = false;
			//btnTime.Enabled = false;
			//hour = GeneralBll.GetLocalDateTime().Hour;
			//minute = GeneralBll.GetLocalDateTime().Minute;

			//dateTime = GeneralBll.GetLocalDateTime();
			//DateTimeFormatInfo dtfi = CultureInfo.CreateSpecificCulture("en-US").DateTimeFormat;
			//btnDate.Text = "";

			await IsValidOffend(true);
			m_bPrint = false;
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", ex.Message, "OK");
			LogFile.WriteLogFile($"ERROR : {ex.Message} | {ex.StackTrace}", Enums.LogType.Error);
		}
	}

	public void EnableControl(bool enable)
	{
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

		//txtTempatJadi.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		//txtTempatJadi.IsEnabled = enable;
		//btnFindTempatJadi.IsEnabled = enable;
		//btnFindTempatJadi.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];

		txtPetak.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		txtPetak.IsEnabled = enable;
		btnFindPetak.IsEnabled = enable;
		btnFindPetak.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];

		//txtSerah.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		//txtSerah.IsEnabled = enable;
		//btnFindSerah.IsEnabled = enable;
		//btnFindSerah.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];

		//txtAfCode.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		//txtAfCode.IsEnabled = enable;
		//
		//btnDate.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		//btnDate.IsEnabled = enable;
		//
		//btnRoadTaxDate.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		//btnRoadTaxDate.IsEnabled = enable;
		//
		//btnTime.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		//btnTime.IsEnabled = enable;
		//
		//txtAfCode.BackgroundColor = enable ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["Gray300"];
		//txtAfCode.IsEnabled = enable;
	}

	private void EnablePrint()
	{
		LogFile.WriteLogFile("EnablePrint - EnablePrint called");

		SetPrintControl(false);
		LogFile.WriteLogFile("txtCarNum : " + txtCarNum.Text);
		if (string.IsNullOrEmpty(txtCarNum.Text)) return;
		LogFile.WriteLogFile("txtCategory : " + txtCategory.Text);
		if (string.IsNullOrEmpty(txtCategory.Text)) return;
		LogFile.WriteLogFile("txtMake : " + txtMake.Text);
		if (string.IsNullOrEmpty(txtMake.Text)) return;
		LogFile.WriteLogFile("txtPerenggan : " + txtPerenggan.Text);
		if (string.IsNullOrEmpty(txtPerenggan.Text)) return;
		LogFile.WriteLogFile("txtLocation : " + txtLocation.Text);
		if (string.IsNullOrEmpty(txtLocation.Text)) return;

		LogFile.WriteLogFile("PhotoCount : " + GeneralBll.getPhotoCount(txtCompound.Text) + " - " + Constants.MinPhoto);
		if (GeneralBll.getPhotoCount(txtCompound.Text) < Constants.MinPhoto) return;

		SetPrintControl(true);

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

		//if (m_bPrint)
		//{
		//	menuItem = myMenu.FindItem(Resource.Id.back_menu);
		//	menuItem.SetIcon(!blValue ? Resource.Drawable.IconkeluarSystemClick : Resource.Drawable.iconBackActionBar);
		//	menuItem.SetEnabled(blValue);
		//}
		//else
		//{
		//	menuItem = myMenu.FindItem(Resource.Id.back_menu);
		//	menuItem.SetIcon(Resource.Drawable.iconBackActionBar);
		//	menuItem.SetEnabled(true);
		//}
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
			await DisplayAlert("Error", "UnExpected Error isvalidoffend() : " + ex.Message, "OK");
		}

		return true;
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

	private async Task CheckCarNumber()
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
		await CheckCarNumber();
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
					_stringZoneDesc = getZoneDesc(_stringZone);
					_stringTempatJadi = GlobalClass.ReturnCodeFind.Split(';')[1] + "," + _stringZoneDesc;
					break;
				case Enums.ActiveForm.FindDelivery:
					//txtSerah.Text = GlobalClass.ReturnCodeFind.Replace(";", " ");
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
				case Enums.ActiveForm.FindLot:
					txtPetak.Text = GlobalClass.ReturnCodeFind.Split(';')[1];
					break;
				case Enums.ActiveForm.FindTempatJadi:
					//txtTempatJadi.Text = GlobalClass.ReturnCodeFind.Replace(";", " ");
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

		///_optionDTO = JsonConvert.DeserializeObject<OptionDTO>(str);
		///txtMukim.Text = _optionDTO.Mukim;
		///txtZone.Text = _optionDTO.Zone;
		///txtAct.Text = _optionDTO.ActCode;
		///txtOffend.Text = _optionDTO.OffendCode;
		//await DisplayAlert("INFO", option.CarCategory, "OK");
	}

	private async Task StartActivityFind(string sFindType)
	{
		GlobalClass.FindResult = false;
		await Navigation.PushAsync(new CarianPage(HandleAfterBackPage, sFindType, "", _stringMukim, _stringZone, "", "", carcategory: txtCategory.Text));
	}

	private async void btnFindCategory_Clicked(object sender, EventArgs e)
	{
		_activeForm = Enums.ActiveForm.FindCategory;
		await StartActivityFind(Constants.FindCarCategory);
	}

	private async void txtCarNum_Unfocused(object sender, FocusEventArgs e)
	{
		await CheckCarNumber();
	}

	private void EnablePrint_TextChanged(object sender, TextChangedEventArgs e)
	{
		try
		{
			EnablePrint();
		}
		catch { }
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
		await Navigation.PushAsync(new FindOffendPage(HandleAfterBackPage, Constants.FindOffend2,  "", _stringAct, Constants.CompType1));
	}

	private void btnFindLocation_Clicked(object sender, EventArgs e)
	{
		_activeForm = Enums.ActiveForm.FindStreet;
		StartActivityFind(Constants.FindStreet);
	}

	private void btnFindPetak_Clicked(object sender, EventArgs e)
	{
		_activeForm = Enums.ActiveForm.FindLot;
		StartActivityFind(Constants.FindLot);
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

	private bool IsValidDelivery()
	{
		//var splitData = GeneralBll.GetSplitData(txtSerah.Text, ' ');
		//var delivery = TableFilBll.GetDeliveryByCode(splitData.Code);
		//if (delivery == null)
		//	return false;
		//
		//txtSerah.Text = delivery.Code + " " + delivery.ShortDesc;
		return true;
	}

	private async Task<bool> ValidateCompound()
	{
		if (!IsValidCategory())
		{
			await DisplayAlert("Info", "Kod Jenis tidak sah", "OK");
			return false;
		}

		if (!IsValidModel())
		{
			await DisplayAlert("Info", "Kod Model tidak sah", "OK");
			return false;
		}
		if (!IsValidColor())
		{
			await DisplayAlert("Info", "Kod Warna tidak sah", "OK");
			return false;
		}
		//if (!IsValidOffend(false))
		//{
		//	await DisplayAlert("Info", "Kod Offend tidak sah", "OK");
		//	return false;
		//}

		if (!IsValidLocation())
		{
			await DisplayAlert("Info", "Kod lokasi tidak sah", "OK");
			return false;
		}

		if (!GeneralBll.IsAlphaNumeric(txtCarNum.Text))
		{
			await DisplayAlert("Info", "No Kenderaan : Terdapat character tidak sah. Sila semak input anda.", "OK");
			txtCarNum.Focus();
			return false;
		}

		if (!GeneralBll.IsAlphaNumeric(txtRoadTax.Text))
		{
			await DisplayAlert("Info", "No Cukai Jalan : Terdapat character tidak sah. Sila semak input anda.", "OK");
			txtRoadTax.Focus();
			return false;
		}

		if (!GeneralBll.IsAlphaNumericAndDotCommaSpaceMinusSlash(txtPetak.Text))
		{
			await DisplayAlert("Info", "No Petak : Terdapat character tidak sah. Sila semak input anda.", "OK");
			txtPetak.Focus();
			return false;
		}

		//if (!GeneralBll.IsAlphaNumeric(txtAfCode.Text))
		//{
		//	await DisplayAlert("Info", "AFCode : Terdapat character tidak sah. Sila semak input anda.", "OK");
		//	txtAfCode.Focus();
		//	return false;
		//}
		//
		//if (!GeneralBll.IsAlphaNumericAndOthers1(txtTempatJadi.Text))
		//{
		//	await DisplayAlert("Info", "Tempat Kejadian : Terdapat character tidak sah. Sila semak input anda.", "OK");
		//	txtTempatJadi.RequestFocus();
		//	return false;
		//}

		return true;

	}

	private async Task<int> SaveCompoundAsync()
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
			compoundDto.CompType = Constants.CompType1;

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

			int iScanMinutes = 0;
			int iCompTimeMiniutes = 0;
			int iLocalTimeMinutes = 0;

			iCompTimeMiniutes = GeneralBll.ConvertTimeToMinutes(_compoundTimeStart);
			iLocalTimeMinutes = GeneralBll.ConvertTimeToMinutes(GeneralBll.GetLocalDateTime().ToString("HHmmss"));

			if (_stringScanDateTime.TrimEnd().Length > 10)
			{

				iScanMinutes = GeneralBll.ConvertTimeToMinutes(GeneralBll.GetTimeHHMMSS(_stringScanDateTime));
				LogFile.WriteLogFile("Compound Number : " + compoundDto.CompNum + "   iCompTimeMiniutes : " + iCompTimeMiniutes);  //174
				LogFile.WriteLogFile("iLocalTimeMinutes : " + iLocalTimeMinutes);   //174
				LogFile.WriteLogFile("iScanMinutes : " + iScanMinutes);     // 169
				LogFile.WriteLogFile("_compoundTimeStart : " + _compoundTimeStart);
				LogFile.WriteLogFile("_stringScanDateTime : " + _stringScanDateTime);   //dd/MM/yyyy hh:mm:ss
				LogFile.WriteLogFile("_stringScanCarNum : " + _stringScanCarNum);

				if (txtCarNum.Text != _stringScanCarNum)
				{
					compoundDto.CompDate = GeneralBll.GetLocalDateTime().ToString("yyyyMMdd");
					compoundDto.CompTime = _compoundTimeStart;
					compoundDto.Compound1Type.ScanDateTime = "";

				}
				else
				{
					if (iLocalTimeMinutes - iScanMinutes >= 5)
					{
						compoundDto.CompDate = GeneralBll.GetLocalDateTime().ToString("yyyyMMdd");
						compoundDto.CompTime = GeneralBll.GetLocalDateTime().ToString("HHmmss");
						compoundDto.Compound1Type.ScanDateTime = GeneralBll.ConvertStringddmmyyyyhhmmssToyyyymmddhhmmss(_stringScanDateTime);

					}
					else
					{
						compoundDto.CompDate = GeneralBll.GetDateDDMMYYYY(_stringScanDateTime);
						compoundDto.CompTime = GeneralBll.GetTimeHHMMSS(_stringScanDateTime);
						compoundDto.Compound1Type.ScanDateTime = GeneralBll.ConvertStringddmmyyyyhhmmssToyyyymmddhhmmss(_stringScanDateTime);
					}
				}

			}
			else
			{
				if (iLocalTimeMinutes - iCompTimeMiniutes >= 5)
				{
					compoundDto.CompDate = GeneralBll.GetLocalDateTime().ToString("yyyyMMdd");
					compoundDto.CompTime = GeneralBll.GetLocalDateTime().ToString("HHmmss");
					compoundDto.Compound1Type.ScanDateTime = "";
				}
				else
				{
					compoundDto.CompDate = GeneralBll.GetLocalDateTime().ToString("yyyyMMdd");
					compoundDto.CompTime = _compoundTimeStart;
					compoundDto.Compound1Type.ScanDateTime = "";
				}
			}

			compoundDto.EnforcerId = infoDto.EnforcerId;
			compoundDto.WitnessId = _stringWitnessCode;
			compoundDto.PubWitness = _stringWitnessPub;
			compoundDto.Tujuan = _stringTujuan;
			compoundDto.Perniagaan = _stringPerniagaan;
			compoundDto.PrintCnt = "0";


			compoundDto.TotalPhoto = GlobalClass.FileImages.Count();

			compoundDto.Compound1Type.CarNum = txtCarNum.Text;
			compoundDto.Compound1Type.Category = txtCategory.Text.Length > 2 ? txtCategory.Text.Substring(0, 2) : "";

			splitData = GeneralBll.GetSplitData(txtMake.Text, ' ');
			compoundDto.Compound1Type.CarType = splitData.Code;
			compoundDto.Compound1Type.CarTypeDesc = splitData.Description;

			splitData = GeneralBll.GetSplitData(txtColor.Text, ' ');
			compoundDto.Compound1Type.CarColor = splitData.Code;
			compoundDto.Compound1Type.CarColorDesc = splitData.Description;

			compoundDto.Compound1Type.LotNo = txtPetak.Text;
			compoundDto.Compound1Type.RoadTax = txtRoadTax.Text;
			//compoundDto.Compound1Type.RoadTaxDate = GeneralBll.ConvertDateToYyyyMmDdFormat(btnRoadTaxDate.Text);
			compoundDto.Compound1Type.CompAmt = _stringOffendAmount;
			compoundDto.Compound1Type.CompAmt2 = _stringOffendAmount2;
			compoundDto.Compound1Type.CompAmt3 = _stringOffendAmount3;

			//compoundDto.Compound1Type.CouponNumber = txtAfCode.Text;
			//if (chkDateTime.Checked)
			//{
			//	compoundDto.Compound1Type.CouponDate = GeneralBll.ConvertDateToYyyyMmDdFormat(btnDate.Text);
			//	compoundDto.Compound1Type.CouponTime = GeneralBll.ConvertTimeToHhMmFormat(btnTime.Text);
			//}
			//else
			//{
			compoundDto.Compound1Type.CouponDate = "19000101";
			compoundDto.Compound1Type.CouponTime = "1200";
			//}

			compoundDto.TempohDate = GeneralBll.ConvertDateToYyyyMmDdFormat(_stringTempohDate);

			//splitData = GeneralBll.GetSplitData(txtSerah.Text, ' ');
			splitData = GeneralBll.GetSplitData("", ' ');
			compoundDto.Compound1Type.DeliveryCode = splitData.Code;
			compoundDto.Compound1Type.CompDesc = _stringKesalahan;

			compoundDto.Kadar =   "Y";
			//splitData = GeneralBll.GetSplitData(txtTempatJadi.Text, ' ');
			splitData = GeneralBll.GetSplitData("", ' ');
			compoundDto.Tempatjadi = splitData.Description;

			compoundDto.NoteDesc = _stringNoteDesc;
			compoundDto = GlobalClass.SetGpsData(compoundDto);

			if (_stringZone == Constants.NewZoneCode)
				CompoundBll.SaveNote(_stringZoneDesc, compoundDto.CompNum, compoundDto.EnforcerId, "ZN");
			if (compoundDto.StreetCode == Constants.NewStreetCode)
			{
				CompoundBll.SaveNote(compoundDto.StreetDesc, compoundDto.CompNum, compoundDto.EnforcerId, "JL");
			}
			if (compoundDto.Compound1Type.CarType == Constants.NewCarTypeCode)
			{
				CompoundBll.SaveNote(compoundDto.Compound1Type.CarTypeDesc, compoundDto.CompNum, compoundDto.EnforcerId, "VH");
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
			LogFile.WriteLogFile("FormCompaun1Page", "SaveCompound", ex.Message, Enums.LogType.Error);
			return Constants.Failed;
		}

	}

	private async void OnPrintValidated()
	{
		try
		{
			var result = await SaveCompoundAsync();
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
				await DisplayAlert("INFO", "Duplicate Compound Number, Sila Hubung Pejabat", "OK");
				return;
			}
			else
			{
				EnableControl(true);
				await DisplayAlert("Info", "Gagal menyimpan Kompaun.", "OK");
			}
		}
		catch (Exception ex)
		{
			EnableControl(true);
			await DisplayAlert("Info", Constants.UnexpectedErrorMessage, "OK");
			LogFile.WriteLogFile("FormCompaun1Page", "OnPrintValidated", ex.Message, Enums.LogType.Error);
		}
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

	private async void printMenu_Clicked(object sender, EventArgs e)
	{
		try
		{
			if (m_blSave)
			{

			}
			else
			{
				bool answer = await DisplayAlert("Confirmation", "Cetak Kompaun# " + txtCompound.Text, "Yes", "No");
				if (answer)
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
			await DisplayAlert("Info", ex.Message, "OK");
		}
	}
}