using AndroidCompound5.AimforceUtils;
using AndroidCompound5.Classes;

namespace AndroidCompound5.Pages;

public partial class OptionPage : ContentPage
{
	int _iActive = 0;
	private string _offendAmount;
	private string _offendAmount2;
	private string _offendAmount3;
	private string _compType;

	public OptionPage()
	{
		InitializeComponent();
		SetInit();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();		
	}

	private async void SetInit()
	{
		var infoDto = InfoBll.GetInfo();
		if (infoDto == null)
		{
			await DisplayAlert(Constants.Message.ErrorMessage, "Fail control tak dijumpai. Sila hubung pejabat.", Constants.Message.OKMessage);
			return;
		}

		txtDescMukim.Text = "";
		txtDescZone.Text = "";
		txtDescAct.Text = "";
		txtDescOffend.Text = "";
		txtMukim.Text = "";
		txtZone.Text = "";
		txtAct.Text = "";
		txtOffend.Text = "";
	}

	private void HandleAfterBackPage()
	{
		if (GlobalClass.FindResult)
		{
			if (_iActive == 1)
			{
				txtMukim.Text = GlobalClass.ReturnCodeFind.Split(';')[0];
				txtDescMukim.Text = GlobalClass.ReturnCodeFind.Split(';')[1];

				//IsValidMukim(false);
				txtZone.Text = string.Empty;
				txtDescZone.Text = string.Empty;
			}
			else if (_iActive == 2)
			{
				txtZone.Text = GlobalClass.ReturnCodeFind.Split(';')[0];
				txtDescZone.Text = GlobalClass.ReturnCodeFind.Split(';')[1];

				//IsValidZone(false);
			}
			else if (_iActive == 3)
			{
				txtAct.Text = GlobalClass.ReturnCodeFind.Split(';')[0];
				txtDescAct.Text = GlobalClass.ReturnCodeFind.Split(';')[1];

				//IsValidAct(false);
				txtOffend.Text = string.Empty;
				txtDescOffend.Text = string.Empty;
			}
			else if (_iActive == 4)
			{
				txtOffend.Text = GlobalClass.ReturnCodeFind.Split(';')[0];
				txtDescOffend.Text = GlobalClass.ReturnCodeFind.Split(';')[1];

				//IsValidOffend(false);
			}
		}
		_iActive = 0;
	}

	private async Task StartActivityFindAsync(string sFindType)
	{
		GlobalClass.FindResult = false;
		await Navigation.PushAsync(new CarianPage(HandleAfterBackPage, sFindType, "Option", txtMukim.Text, txtZone.Text, txtAct.Text, txtOffend.Text));
	}

	//private void StartActivityFindOffend(string sFindType)
	//{
	//	GlobalClass.FindResult = false;
	//	var find = new Intent(this, typeof(FindOffendActivity));

	//	find.PutExtra("Type", sFindType);
	//	find.PutExtra("actcode", txtAct.Text);

	//	StartActivity(find);
	//}

	private async void ToolbarItem_Clicked(object sender, EventArgs e)
	{
		if (!await ValidateOption()) return;
		var infoDto = InfoBll.GetInfo();
		if (infoDto == null)
		{
			await DisplayAlert(Constants.Message.ErrorMessage, "Fail control tak dijumpai. Sila hubung pejabat.", Constants.Message.OKMessage);
			return;
		}

		infoDto.CurrMukim = txtMukim.Text;
		infoDto.CurrZone = txtZone.Text;
		InfoBll.UpdateInfo(infoDto, Enums.FormName.OptionLocation);

		switch (_compType)
		{
			case Constants.CompType1:
				await Navigation.PushAsync(new FormCompaund1Page(txtMukim.Text, txtZone.Text, txtDescZone.Text, txtAct.Text, txtOffend.Text, _offendAmount, _offendAmount2, _offendAmount3));
				break;
			//case Constants.CompType2:
			//	await Navigation.PushAsync(new FormCompaund2Page(
			//		_optionDTO.Mukim,
			//		_optionDTO.Zone,
			//		_optionDTO.ZoneDesc,
			//		_optionDTO.ActCode,
			//		_optionDTO.OffendCode,
			//		_optionDTO.OffendAmount,
			//		_optionDTO.OffendAmount2,
			//		_optionDTO.OffendAmount3));
			//	break;
			//case Constants.CompType3:
			//	await Navigation.PushAsync(new FormCompaund3Page(
			//		_optionDTO.Mukim,
			//		_optionDTO.Zone,
			//		_optionDTO.ZoneDesc,
			//		_optionDTO.ActCode,
			//		_optionDTO.OffendCode,
			//		_optionDTO.OffendAmount,
			//		_optionDTO.OffendAmount2,
			//		_optionDTO.OffendAmount3));
			//	break;
			//case Constants.CompType5:
			//	await Navigation.PushAsync(new FormCompaund5Page(
			//		_optionDTO.Mukim,
			//		_optionDTO.Zone,
			//		_optionDTO.ZoneDesc,
			//		_optionDTO.ActCode,
			//		_optionDTO.OffendCode,
			//		_optionDTO.OffendAmount,
			//		_optionDTO.OffendAmount2,
			//		_optionDTO.OffendAmount3));
			//	break;
			default:
				await DisplayAlert(Constants.Message.ErrorMessage, "In Construction " + _compType, Constants.Message.OKMessage);
				return;
		}
	}

	private async void btnFindMukim_Clicked(object sender, EventArgs e)
	{
		try
		{
			_iActive = 1;
			await StartActivityFindAsync(Constants.FindMukim);
		}
		catch (Exception ex)
		{
			await DisplayAlert(Constants.Message.ErrorMessage, Constants.UnexpectedErrorMessage, Constants.Message.OKMessage);
			LogFile.WriteLogFile(typeof(OptionPage).Name, "_btnFindMukim_Click", ex.Message, Enums.LogType.Error);
		}
	}

	private async void btnFindZone_Clicked(object sender, EventArgs e)
	{
		try
		{
			_iActive = 2;
			await StartActivityFindAsync(Constants.FindZone);
		}
		catch (System.Exception ex)
		{
			await DisplayAlert(Constants.Message.ErrorMessage, Constants.UnexpectedErrorMessage, Constants.Message.OKMessage);
			LogFile.WriteLogFile(typeof(OptionPage).Name, "btnFindZone_Clicked", ex.Message, Enums.LogType.Error);
		}
	}

	private async void btnFindAct_Clicked(object sender, EventArgs e)
	{
		try
		{
			_iActive = 3;
			await StartActivityFindAsync(Constants.FindAct);
		}
		catch (System.Exception ex)
		{
			await DisplayAlert(Constants.Message.ErrorMessage, Constants.UnexpectedErrorMessage, Constants.Message.OKMessage);
			LogFile.WriteLogFile(typeof(OptionPage).Name, "btnFindAct_Clicked", ex.Message, Enums.LogType.Error);
		}
	}

	private async void btnFindOffend_Clicked(object sender, EventArgs e)
	{
		try
		{
			if (txtAct.Text.Length == 0)
			{
				await DisplayAlert(Constants.Message.InfoMessage, "Act Empty!", Constants.Message.OKMessage);
				txtAct.Focus();
				return;
			}
			_iActive = 4;

			GlobalClass.FindResult = false;
			await Navigation.PushAsync(new FindOffendPage(HandleAfterBackPage, Constants.FindOffend, "Option", txtAct.Text, ""));
		}
		catch (System.Exception ex)
		{
			await DisplayAlert("ERROR", Constants.UnexpectedErrorMessage, "OK");
			LogFile.WriteLogFile("Option", "btnFindOffend_Clicked", ex.Message, Enums.LogType.Error);
		}
	}

	private async void txtMukim_TextChanged(object sender, TextChangedEventArgs e)
	{
		try
		{
			IsValidMukim();
		}
		catch (Exception ex)
		{
			await DisplayAlert(Constants.Message.ErrorMessage, ex.Message, Constants.Message.OKMessage);
		}
	}

	private async void txtZone_TextChanged(object sender, TextChangedEventArgs e)
	{
		try
		{
			IsValidZone();
		}
		catch (Exception ex)
		{
			await DisplayAlert(Constants.Message.ErrorMessage, ex.Message, Constants.Message.OKMessage);
		}
	}

	private async void txtAct_TextChanged(object sender, TextChangedEventArgs e)
	{
		try
		{
			IsValidAct();
		}
		catch (Exception ex)
		{
			await DisplayAlert(Constants.Message.ErrorMessage, ex.Message, Constants.Message.OKMessage);
		}
	}

	private async void txtOffend_TextChanged(object sender, TextChangedEventArgs e)
	{
		try
		{
			IsValidOffend();
		}
		catch (Exception ex)
		{
			await DisplayAlert(Constants.Message.ErrorMessage, ex.Message, Constants.Message.OKMessage);
		}
	}

	#region Validation

	private async Task<bool> ValidateOption()
	{
		bool result = true;
		if (!IsValidMukim())
		{
			result = false;
			await DisplayAlert(Constants.Message.ErrorMessage, "Kod mukim tidak sah", Constants.Message.OKMessage);
		}
		else if (!IsValidZone())
		{
			result = false;
			await DisplayAlert(Constants.Message.ErrorMessage, "Kod kawasan tidak sah", Constants.Message.OKMessage);
		}
		else if (!IsValidAct())
		{
			result = false;
			await DisplayAlert(Constants.Message.ErrorMessage, "Kod akta tidak sah", Constants.Message.OKMessage);
		}

		else if (!IsValidOffend())
		{
			result = false;
			await DisplayAlert(Constants.Message.ErrorMessage, "Kod kesalahan tidak sah", Constants.Message.OKMessage);
		}

		return result;
	}

	private bool IsValidMukim()
	{
		var mukim = TableFilBll.GetMukimByCode(txtMukim.Text);

		if (mukim == null)
		{
			txtDescMukim.Text = string.Empty;
			return false;
		}

		txtDescMukim.Text = mukim.LongDesc;
		return true;

	}

	private bool IsValidZone()
	{
		if (txtZone.Text == Constants.NewZoneCode)
		{
			if (string.IsNullOrEmpty(txtDescZone.Text))
				return false;
			return true;
		}

		var zone = TableFilBll.GetZoneByCodeAndMukim(txtZone.Text, txtMukim.Text);
		if (zone == null)
		{
			txtDescZone.Text = string.Empty;
			return false;
		}
		txtDescZone.Text = zone.LongDesc;
		return true;
	}

	private bool IsValidAct()
	{
		var act = TableFilBll.GetActByCode(txtAct.Text);
		if (act == null)
		{
			txtDescAct.Text = string.Empty;
			return false;
		}
		txtDescAct.Text = act.ShortDesc;
		return true;

	}

	private bool IsValidOffend()
	{
		var offend = TableFilBll.GetOffendByCodeAndAct(txtOffend.Text, txtAct.Text);
		if (offend == null)
		{
			txtDescOffend.Text = string.Empty;
			return false;
		}
		txtDescOffend.Text = offend.LongDesc;
		_compType = offend.CompType;
		_offendAmount = offend.OffendAmt;
		_offendAmount2 = offend.OffendAmt2;
		_offendAmount3 = offend.OffendAmt3;

		return true;

	}
	#endregion
}