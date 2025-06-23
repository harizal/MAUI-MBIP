using AndroidCompound5.AimforceUtils;
using AndroidCompound5.Classes;
using Newtonsoft.Json;

namespace AndroidCompound5.Pages;

public partial class FormCompaund12Page : ContentPage
{
	private string _stringCompound;
	private string _stringOffendAmount;
	private string _stringOffendAmount2;
	private string _stringOffendAmount3;
	private string _stringWitnessId;
	private string _stringPubWitness;

	private string _stringKesalahan;
	private string _stringAct;
	private string _stringOffend;
	private string _stringNoteDesc;
	private string _stringTujuan;
	private string _stringPerniagaan;
	private string _stringTempohDate;
	private string _stringCarNum;
	private string _stringCarCategory;
	private string _stringPetak = "";
	private string _stringNamaPesalah = "";
	private string _stringNoICPesalah = "";
	private string _stringIDPenguatkuasa;
	private string _stringNamaPenguatkuasa;
	private string _stringTempatJadi;
	Enums.ActiveForm _activeForm = Enums.ActiveForm.None;
	public FormCompaund12Page(string compoundnumber, string compoundamt, string compoundamt2, string compoundamt3,
		string witnesscode, string witnesspub, string kesalahan, string notedesc, string actcode, string offendcode,
		string tujuan, string perniagaan, string tempohdate, string carnum, string carcategory, string petak,
		string namapesalah, string noicpesalah, string idpenguatkuasa, string namapenguatkuasa, string tempatjadi)
	{
		InitializeComponent();

		_stringCompound = compoundnumber;
		_stringOffendAmount = compoundamt;
		_stringOffendAmount2 = compoundamt2;
		_stringOffendAmount3 = compoundamt3;
		_stringWitnessId = witnesscode;
		_stringPubWitness = witnesspub;

		_stringKesalahan = kesalahan;
		_stringAct = actcode;
		_stringOffend = offendcode;
		_stringNoteDesc = notedesc;
		_stringTempohDate = tempohdate;
		_stringTujuan = tujuan;
		_stringPerniagaan = perniagaan;

		_stringCarNum = carnum;
		_stringCarCategory = carcategory;
		_stringPetak = petak;
		_stringNamaPesalah = namapesalah;
		_stringNoICPesalah = noicpesalah;
		_stringIDPenguatkuasa = idpenguatkuasa;
		//_stringNamaPenguatkuasa = namapebguatkuasa;
		_stringTempatJadi = tempatjadi;

		SetInit();
	}

	private async void SetInit()
	{
		try
		{
			txtCompound.Text = _stringCompound;

			txtPubWitness.Text = _stringPubWitness;
			//txtTujuan.Text = _stringTujuan;
			//txtPerniagaan.Text = _stringPerniagaan;
			//txtKesalahan.Text = _stringKesalahan;
			txtWitness.Text = _stringWitnessId;
			//if (string.IsNullOrEmpty(txtKesalahan.Text))
			//{
			//	_stringKesalahan = getButirKesalahan(_stringAct, _stringOffend);
			//	txtKesalahan.Text = _stringKesalahan;
			//}

			if (!string.IsNullOrEmpty(_stringWitnessId))
				IsValidWitness(_stringWitnessId);

			txtCompAmount.Text = GeneralBll.FormatViewAmount(_stringOffendAmount);

		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", Constants.UnexpectedErrorMessage, "OK");
			LogFile.WriteLogFile("FormCompaund12Page", "SetInit ", ex.Message, Enums.LogType.Error);
		}
	}

	private bool IsValidWitness(string witnessCode)
	{
		var enforcer = EnforcerBll.GetEnforcerById(witnessCode);
		if (enforcer == null)
			return false;

		txtWitness.Text = enforcer.EnforcerId + " " + enforcer.EnforcerName;

		return true;
	}

	private void HandleAfterBackPage()
	{
		if (GlobalClass.FindResult)
		{
			switch (_activeForm)
			{
				case Enums.ActiveForm.FindEnforcer:
					txtWitness.Text = GlobalClass.ReturnCodeFind.Replace(";", " ");
					break;
				case Enums.ActiveForm.FindKesalahan:
					//txtKesalahan.Text = GlobalClass.ReturnCodeFind.Split(';')[1];
					//txtKesalahan.Text = ReplaceButir(txtKesalahan.Text);
					break;
				case Enums.ActiveForm.FormNote:
					_stringNoteDesc = GlobalClass.ReturnCodeFind.Split(';')[0];
					_stringTempohDate = GlobalClass.ReturnCodeFind.Split(';')[1];
					break;
				case Enums.ActiveForm.FindTujuan:
					//txtTujuan.Text = GlobalClass.ReturnCodeFind.Split(';')[1];
					break;
				case Enums.ActiveForm.FindPerniagaan:
					//txtPerniagaan.Text = GlobalClass.ReturnCodeFind.Split(';')[1];
					break;
			}

			GlobalClass.FindResult = false;
			GlobalClass.ReturnCodeFind = "";
		}
		_activeForm = Enums.ActiveForm.None;
	}

	private async Task StartActivityFind(string sFindType)
	{
		GlobalClass.FindResult = false;
		await Navigation.PushAsync(new CarianPage(HandleAfterBackPage, sFindType, "", string.Empty, string.Empty, _stringAct, _stringOffend));
	}

	private async void btnFindWitness_Clicked(object sender, EventArgs e)
	{
		_activeForm = Enums.ActiveForm.FindEnforcer;
		await StartActivityFind(Constants.FindEnforcer);
	}

	private async void btnFindKesalahan_Clicked(object sender, EventArgs e)
	{
		_activeForm = Enums.ActiveForm.FindKesalahan;
		await StartActivityFind(Constants.FindKesalahan);

	}

	private async void txtWitness_TextChanged(object sender, TextChangedEventArgs e)
	{
		try
		{
			IsValidWitness(txtWitness.Text);
		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
		}
	}

	private string ReplaceButir(string strButir)
	{
		strButir = strButir.Replace("<JENKEN>", _stringCarCategory);
		strButir = strButir.Replace("<NODAFTAR>", _stringCarNum);
		strButir = strButir.Replace("<NOPETAK>", _stringPetak);
		strButir = strButir.Replace("<NAMAPESALAH>", _stringNamaPesalah);
		strButir = strButir.Replace("<NOICPESALAH>", _stringNoICPesalah);
		strButir = strButir.Replace("<NOKMP>", _stringCompound);
		strButir = strButir.Replace("<NAMAPENGUATKUASA>", _stringNamaPenguatkuasa);
		strButir = strButir.Replace("<TEMPATJADI>", _stringTempatJadi);
		strButir = strButir.Replace("<TUJUAN>", _stringTujuan);
		strButir = strButir.Replace("<PERNIAGAAN>", _stringPerniagaan);

		return strButir;
	}

	private async Task<bool> OnBack()
	{
		try
		{

			if (!GeneralBll.IsAlphaNumericAndOthers1(txtPubWitness.Text))
			{
				MainThread.BeginInvokeOnMainThread(async () =>
				{
					await DisplayAlert("INFO", "Saksi Awan : Terdapat character tidak sah. Sila semak input anda.", "OK");
					txtPubWitness.Focus();
				});
				return false;
			}

			//if (!GeneralBll.IsAlphaNumericAndOthers1(txtTujuan.Text))
			//{
			//	GeneralAndroidClass.ShowToast(this, "Tujuan : Terdapat character tidak sah. Sila semak input anda.");
			//	txtTujuan.RequestFocus();
			//	return;
			//}

			//if (!GeneralBll.IsAlphaNumericAndOthers1(txtPerniagaan.Text))
			//{
			//	GeneralAndroidClass.ShowToast(this, "Perniagaan : Terdapat character tidak sah. Sila semak input anda.");
			//	txtPerniagaan.RequestFocus();
			//	return;
			//}

			if (!GeneralBll.IsAlphaNumericAndOthers1(txtKesalahan.Text ?? "") || string.IsNullOrEmpty(txtKesalahan.Text))
			{
				MainThread.BeginInvokeOnMainThread(async () =>
				{
					await DisplayAlert("INFO", "Butir Kesalahan : Terdapat character tidak sah. Sila semak input anda.", "OK");
					txtKesalahan.Focus();
				});
				return false;
			}


			var splitData = GeneralBll.GetSplitData(txtWitness.Text ?? "", ' ');
			_stringWitnessId = splitData.Code;
			_stringKesalahan = txtKesalahan?.Text ?? "";
			//_stringTujuan = txtTujuan.Text;
			//_stringPerniagaan = txtPerniagaan.Text;
			_stringPubWitness = txtPubWitness?.Text?? "";
			GlobalClass.FindResult = true;
			GlobalClass.ReturnCodeFind = _stringWitnessId + ";" + _stringKesalahan + ";" + _stringNoteDesc;
			GlobalClass.ReturnCodeFind += ";" + _stringTujuan + ";" + _stringPerniagaan + ";" + _stringPubWitness + ";" + _stringTempohDate;


		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
			LogFile.WriteLogFile(typeof(FormCompaund12Page).Name, "OnBack", ex.Message, Enums.LogType.Error);

			return false;
		}

		return true;
	}

#if ANDROID
	protected override bool OnBackButtonPressed()
	{
		//Change to NEXT button if still not working
		var result = Task.Run(async () => await OnBack()).Result;
		if (!result)
		{
			return true;
		}
		return base.OnBackButtonPressed();
	}
#endif

	private async void noteMenu_Clicked(object sender, EventArgs e)
	{
		_activeForm = Enums.ActiveForm.FormNote;
		await Navigation.PushAsync(new NotePage(_stringCompound, _stringNoteDesc, _stringKesalahan, _stringTempohDate, HandleAfterBackPage));
	}

	private async void signatureMenu_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new SignaturePage(_stringCompound, ""));
	}
}