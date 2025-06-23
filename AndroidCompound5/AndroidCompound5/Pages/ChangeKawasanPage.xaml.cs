using Android.Text;
using AndroidCompound5.AimforceUtils;
using AndroidCompound5.Classes;
using Newtonsoft.Json;

namespace AndroidCompound5.Pages;

public partial class ChangeKawasanPage : ContentPage
{
	private readonly string _mukim, _zone;
	private readonly Action _onDataReturned;

	public ChangeKawasanPage(Action onDataReturned, string mukim, string zone)
	{
		_mukim = mukim;
		_zone = zone;
		_onDataReturned = onDataReturned;
		SetInit();

		InitializeComponent();
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
	private void ClosePage()
	{
		_onDataReturned?.Invoke();
	}
	protected override void OnDisappearing()
	{
		base.OnDisappearing();
		ClosePage();
	}

	private int _iActive;
	private async void HandleAfterBackPage()
	{
		if (GlobalClass.FindResult)
		{
			if (_iActive == 1)
			{
				txtMukim.Text = GlobalClass.ReturnCodeFind.Split(';')[0];
				txtDescMukim.Text = GlobalClass.ReturnCodeFind.Split(';')[1];

				IsValidMukim();
			}
			else if (_iActive == 2)
			{
				txtZone.Text = GlobalClass.ReturnCodeFind.Split(';')[0];
				txtDescZone.Text = GlobalClass.ReturnCodeFind.Split(';')[1];

				IsValidZone();
			}
		}
	}

	private async void StartActivityFind(string sFindType)
	{
		GlobalClass.FindResult = false;
		await Navigation.PushAsync(new CarianPage(HandleAfterBackPage, sFindType, "ChangeKawasan", txtMukim.Text, string.Empty, string.Empty, string.Empty));

	}

	private bool IsValidZone()
	{
		var zone = TableFilBll.GetZoneByCodeAndMukim(txtZone.Text, txtMukim.Text);
		if (zone == null)
		{
			txtDescZone.Text = string.Empty;
			return false;
		}
		txtDescZone.Text = zone.LongDesc;
		return true;
	}

	private async void SetInit()
	{
		try
		{
			txtDescMukim.Text = "";
			txtDescZone.Text = "";
			txtMukim.Text = "";
			txtZone.Text = "";

			txtMukim.Text = _mukim;
			txtZone.Text = _zone;

			IsValidMukim();
			IsValidZone();
		}
		catch (System.Exception ex)
		{
			await DisplayAlert("ERROR", Constants.UnexpectedErrorMessage, "OK");
			LogFile.WriteLogFile("Change Kawasan Page", "SetInit", ex.Message, Enums.LogType.Error);
		}
	}


	private async void txtMukim_TextChanged(object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
	{
		try
		{
			IsValidMukim();
		}
		catch (Exception ex)
		{

			await DisplayAlert("ERROR", ex.Message, "OK");
		}
	}

	private async void btnFindMukim_Clicked(object sender, EventArgs e)
	{
		try
		{
			_iActive = 1;
			StartActivityFind(Constants.FindMukim);
		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
		}
	}

	private async void txtZone_TextChanged(object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
	{
		try
		{
			IsValidZone();
		}
		catch (Exception ex)
		{

			await DisplayAlert("ERROR", ex.Message, "OK");
		}
		
	}

	private async void btnFindZone_Clicked(object sender, EventArgs e)
	{
		try
		{
			_iActive = 2;
			StartActivityFind(Constants.FindZone);
		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
		}
	}

	private async void ToolbarItem_Clicked(object sender, EventArgs e)
	{
		try
		{
			if (await ValidateOption())
			{
				GlobalClass.FindResult = true;
				GlobalClass.ReturnCodeFind = txtMukim.Text + ";" + txtZone.Text;
				await Navigation.PopAsync();
			}
		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
		}
	}

	private async Task<bool> ValidateOption()
	{
		bool result = true;
		if (!IsValidMukim())
		{
			result = false;
			await DisplayAlert("ERROR", "Kod mukim tidak sah", "OK");
		}
		else if (!IsValidZone())
		{
			result = false;
			await DisplayAlert("ERROR", "Kod kawasan tidak sah", "OK");
		}
		return result;
	}
}