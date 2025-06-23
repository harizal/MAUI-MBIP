using AndroidCompound5.AimforceUtils;
using AndroidCompound5.Classes;
using Newtonsoft.Json;

namespace AndroidCompound5.Pages;

public partial class AktaPage : ContentPage
{
	private int iActive = 0;
	private const string ActivityName = "Akta";

	private string _stringCompType;
	private string _stringAct;
	private string _stringOffend;
	private string _stringOffendAmount;
	private string _stringOffendAmount2;
	private string _stringOffendAmount3;
	public AktaPage(string actCode, string offend, string offendAmount, string offendAmount2, string offendAmount3, string compType)
	{
		InitializeComponent();
		
		_stringCompType = compType;
		_stringAct = actCode;
		_stringOffend = offend;
		_stringOffendAmount = offendAmount;
		_stringOffendAmount2 = offendAmount2;
		_stringOffendAmount3 = offendAmount3;

		SetInit();
	}

	private void SetInit()
	{
		akta_txtAct.Text = _stringAct;
		akta_txtOffend.Text = _stringOffend;
		IsValidAct(false);

		IsValidOffend(false);
	}
	private bool IsValidAct(bool isMessage)
	{
		var act = TableFilBll.GetActByCode(akta_txtAct.Text);
		if (act == null)
		{
			akta_txtActDesc.Text = string.Empty;
			return false;
		}
		akta_txtActDesc.Text = act.ShortDesc;
		return true;
	}

	private bool IsValidOffend(bool isMessage)
	{
		var offend = TableFilBll.GetOffendByCodeAndAct(akta_txtOffend.Text, akta_txtAct.Text);
		if (offend == null)
		{
			akta_txtOffendDesc.Text = string.Empty;
			return false;
		}
		akta_txtOffendDesc.Text = offend.LongDesc;
		_stringOffend = offend.OfdCode;
		_stringOffendAmount = offend.OffendAmt;
		_stringOffendAmount2 = offend.OffendAmt2;
		_stringOffendAmount3 = offend.OffendAmt3;
		_stringCompType = offend.CompType;

		return true;
	}

	private async Task<bool> ValidateOption()
	{
		bool result = true;

		if (!IsValidOffend(false))
		{
			await DisplayAlert("INFO", "Kod kesalahan tidak sah", "OK");
			result = false;
		}
		return result;
	}

	private async void OnNext()
	{
		if (await ValidateOption())
		{
			GlobalClass.FindResult = true;
			GlobalClass.ReturnCodeFind = akta_txtAct.Text + ";" + akta_txtOffend.Text + ";" + _stringOffendAmount + ";" + _stringOffendAmount2 + ";" + _stringOffendAmount3;

			await Navigation.PopAsync();
		}
	}

	private async void HandleAfterBackPage()
	{
		try
		{
			if (GlobalClass.FindResult)
			{

				if (iActive == 1)
				{
					akta_txtAct.Text = GlobalClass.ReturnCodeFind.Split(';')[0];
					akta_txtActDesc.Text = GlobalClass.ReturnCodeFind.Split(';')[1];
					IsValidAct(false);
				}
				else if (iActive == 2)
				{
					akta_txtOffend.Text = GlobalClass.ReturnCodeFind.Split(';')[0];
					akta_txtOffendDesc.Text = GlobalClass.ReturnCodeFind.Split(';')[1];

					IsValidOffend(false);
				}
			}
		}
		catch (Exception ex)
		{
			await DisplayAlert("INFO", ex.Message, "OK");
		}
		GlobalClass.FindResult = false;
		GlobalClass.ReturnCodeFind = "";
		iActive = 0;

	}

	private async void akta_btnFindOffend_Clicked(object sender, EventArgs e)
	{
		try
		{
			if (akta_txtAct.Text.Length == 0)
			{
				await DisplayAlert("INFO", "Act Empty!", "OK");
				akta_txtAct.Focus();
			}
			else
			{
				iActive = 2;
				GlobalClass.FindResult = false;				
				await Navigation.PushAsync(new FindOffendPage(HandleAfterBackPage, Constants.FindOffend2, ActivityName, _stringAct, Constants.CompType1));
			}
		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", Constants.UnexpectedErrorMessage, "OK");
			LogFile.WriteLogFile("akta_btnFindOffend_Clicked", "_btnFindOffend_Click", ex.Message, Enums.LogType.Error);
		}
	}

	private async void Border_Unfocused(object sender, FocusEventArgs e)
	{
		try
		{
			IsValidOffend(false);
		}
		catch (Exception ex)
		{
			await DisplayAlert("INFO", ex.Message, "OK");
		}
	}

	private void ToolbarItem_Clicked_1(object sender, EventArgs e)
	{
		OnNext();
	}
}