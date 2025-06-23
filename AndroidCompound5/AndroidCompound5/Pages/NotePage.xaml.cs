using AndroidCompound5.Classes;

namespace AndroidCompound5.Pages;

public partial class NotePage : ContentPage
{
	string sActivityName;

	private bool _isNeedEdited = false;
	private string _originalText = "";
	string _stringKesalahan;
	string _stringNoteDesc;
	string _strTempohDate;
	const int DATE_DIALOG_ID = 0;
	private DateTime dateTime;
	private readonly Action _onDataReturned;

	public NotePage(string compoundnumber, string notedesc, string kesalahan, string tempohdate, Action onDataReturned)
	{
		InitializeComponent();
		_stringKesalahan = kesalahan;
		_stringNoteDesc = notedesc;
		_strTempohDate = tempohdate;

		//btnDate.SetValue =_strTempohDate;

		if (_strTempohDate.Trim() == "")
		{
			stackDate.IsVisible = false;
		}
		else
		{
			stackDate.IsVisible = false;
		}
		_onDataReturned = onDataReturned;
		SetInit();
	}

	private async void SetInit()
	{
		try
		{
			dateTime = GeneralBll.GetLocalDateTime();
			txtNote.Text = _stringNoteDesc;
			_isNeedEdited = false;
			_originalText = "";
		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
		}

	}

	private async void saveMenu_Clicked(object sender, EventArgs e)
	{
		if (!GeneralBll.IsAlphaNumericAndDotCommaSpaceMinusSlashBraket(txtNote.Text))
		{
			await DisplayAlert("ERROR", "Terdapat character tidak sah. Sila semak input anda.", "OK");
			txtNote.Focus();
			return;
		}

		_stringNoteDesc = txtNote.Text;
		_strTempohDate = btnDate.Date.ToString("dd/MM/yyyy");
		GlobalClass.FindResult = true;
		GlobalClass.ReturnCodeFind = _stringNoteDesc + ";" +_strTempohDate;

		_onDataReturned?.Invoke();
		await Navigation.PopAsync();
	}
}