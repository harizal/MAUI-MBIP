using AndroidCompound5.AimforceUtils;
using AndroidCompound5.Classes;
using Newtonsoft.Json;

namespace AndroidCompound5.Pages;

public partial class FormCompaund52Page : ContentPage
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
	private string _stringLockKey;
	private string _stringUnlock;
	private string _stringTow;
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

	public FormCompaund52Page(string compoundnumber, string compoundamt, string compoundamt2, string compoundamt3, string witnesscode,
		string witnesspub, string kesalahan, string notedesc, string actcode, string offendcode, string lockkey, string unlock,
		string tow, string tempohdate, string carnum, string carcategory, string petak, string namapesalah, string noicpesalah,
		string idpenguatkuasa, string namapenguatkuasa, string tempatjadi)
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
		_stringLockKey = lockkey;
		_stringUnlock = unlock;
		_stringTow = tow;
		_stringCarNum = carnum;
		_stringCarCategory = carcategory;
		_stringPetak = petak;
		_stringNamaPesalah = namapesalah;
		_stringNoICPesalah = noicpesalah;
		_stringIDPenguatkuasa = idpenguatkuasa;
		_stringNamaPenguatkuasa = namapenguatkuasa;
		_stringTempatJadi = tempatjadi;

		SetInit();
	}

	private async void SetInit()
	{
		try
		{

			txtCompound.Text = _stringCompound;

			txtPubWitness.Text = _stringPubWitness;
			txtLockKey.Text = _stringLockKey;
			//txtKesalahan.Text = _stringKesalahan;
			txtWitness.Text = _stringWitnessId;
			//if (string.IsNullOrEmpty(txtKesalahan.Text))
			//{
			//	_stringKesalahan = getButirKesalahan(_stringAct, _stringOffend);
			//	txtKesalahan.Text = _stringKesalahan;
			//}

			if (!string.IsNullOrEmpty(_stringWitnessId))
				IsValidWitness(_stringWitnessId);

			if (!string.IsNullOrEmpty(_stringUnlock))
				txtUnLock.Text = _stringUnlock;
			else
				txtUnLock.Text = "0.00";

			if (!string.IsNullOrEmpty(_stringTow))
				txtTow.Text = _stringTow;
			else
				txtTow.Text = "0.00";

			//txtCompAmount.Text = GeneralBll.FormatViewAmount(_stringOffendAmount);

		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
			LogFile.WriteLogFile(typeof(FormCompaund12Page).Name, "SetInit", ex.Message, Enums.LogType.Error);
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

			if (txtLockKey?.Text?.TrimEnd() == "")
			{
				MainThread.BeginInvokeOnMainThread(async () =>
				{
					await DisplayAlert("INFO", "Nombor Kunci tak noleh dikosongkan!!!", "OK");
					txtLockKey.Focus();
				});
				return false;
			}


			var splitData = GeneralBll.GetSplitData(txtWitness.Text, ' ');
			_stringWitnessId = splitData.Code;
			//_stringKesalahan = txtKesalahan.Text;
			_stringLockKey = txtLockKey.Text;
			_stringUnlock = txtUnLock.Text;
			_stringTow = txtTow.Text;
			_stringPubWitness = txtPubWitness.Text;
			GlobalClass.FindResult = true;
			GlobalClass.ReturnCodeFind = _stringWitnessId + ";" + _stringKesalahan + ";" + _stringNoteDesc;
			GlobalClass.ReturnCodeFind += ";" + _stringLockKey + ";" + _stringUnlock + ";" + _stringTow + ";" + _stringPubWitness + ";" + _stringTempohDate;
		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
			LogFile.WriteLogFile(typeof(FormCompaund12Page).Name, "OnBack", ex.Message, Enums.LogType.Error);

			return false;
		}

		return true;
	}

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

	private async void noteMenu_Clicked(object sender, EventArgs e)
	{
		_activeForm = Enums.ActiveForm.FormNote;
		await Navigation.PushAsync(new NotePage(_stringCompound, _stringNoteDesc, _stringKesalahan, _stringTempohDate, HandleAfterBackPage));
	}

	private async void signatureMenu_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new SignaturePage(_stringCompound, ""));
	}

	private async void StartActivityFind(string sFindType)
	{
		GlobalClass.FindResult = false;		
		await Navigation.PushAsync(new CarianPage(HandleAfterBackPage, sFindType, "", "", "", _stringAct, _stringOffend));
	}

	private void btnFindWitness_Clicked(object sender, EventArgs e)
	{
		_activeForm = Enums.ActiveForm.FindEnforcer;
		StartActivityFind(Constants.FindEnforcer);
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
}