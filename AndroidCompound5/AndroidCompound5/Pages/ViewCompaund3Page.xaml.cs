using Android.AdServices.Topics;
using Android.Graphics;
using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.Classes;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using System.Xml.Linq;

namespace AndroidCompound5.Pages;


public partial class ViewCompaund3Page : Microsoft.Maui.Controls.TabbedPage
{
	string _stringCompoundNo;
	private CompoundDto _compoundDto;
	public ViewCompaund3Page(string compaundNumber)
	{
		InitializeComponent();
		_stringCompoundNo = compaundNumber;

		if (DeviceInfo.Platform == DevicePlatform.Android)
		{
			// This forces the tab bar to be at the bottom of the screen on Android
			On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
		}
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		SetInit();
	}

	private async Task SetInit()
	{
		try
		{
			_compoundDto = CompoundBll.GetCompoundByCompoundNumber(_stringCompoundNo);
#if DEBUG
			_compoundDto = new CompoundDto();
#endif
			if (_compoundDto == null)
			{
				await DisplayAlert("ERROR", "Could Not Find Compound No : " + _stringCompoundNo, "OK");
				await Navigation.PopAsync();
			}
			else
			{
				txtCompound.Text = _stringCompoundNo;
				txtRujukan.Text = _compoundDto.Compound3Type.Rujukan;
				txtCompany.Text = _compoundDto.Compound3Type.Company;
				txtCompanyName.Text = _compoundDto.Compound3Type.CompanyName;
				txtIC.Text = _compoundDto.Compound3Type.OffenderIc;
				txtName.Text = _compoundDto.Compound3Type.OffenderName;
				txtDelivery.Text = _compoundDto.Compound3Type.DeliveryCode;
				txtTempatJadi.Text = _compoundDto.Tempatjadi;
				txtLocation.Text = _compoundDto.StreetCode + " " + _compoundDto.StreetDesc;
				var offend = TableFilBll.GetOffendByCodeAndAct(_compoundDto.OfdCode, _compoundDto.ActCode);
				if (offend != null)
					txtOffend.Text = offend.LongDesc;

				var delivery = TableFilBll.GetDeliveryByCode(txtDelivery.Text);
				if (delivery != null)
					txtDelivery.Text += " " + delivery.ShortDesc;

				txtAddress1.Text = _compoundDto.Compound3Type.Address1;
				txtAddress2.Text = _compoundDto.Compound3Type.Address2;
				txtAddress3.Text = _compoundDto.Compound3Type.Address3;
				var enforcer = EnforcerBll.GetEnforcerById(_compoundDto.EnforcerId);
				if (enforcer != null)
					txtPenguatKuasa.Text = _compoundDto.EnforcerId + "- " + enforcer.EnforcerName;
			}
		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
		}

	}

	private void SetImageList()
	{
		GlobalClass.FileImages = new List<string>();

		var listFileName = GeneralBll.GetListFileImageNameByCompoundNumber(txtCompound.Text);

		foreach (var fileName in listFileName)
		{
			GlobalClass.FileImages.Add(fileName);
		}

	}

	private async Task OnPhoto()
	{
		try
		{
			SetImageList();

			//Intent fhoto = new Intent(this, typeof(Camera));
			//fhoto.PutExtra("viewMode", true);
			//fhoto.PutExtra("deletemode", false);
			//fhoto.PutExtra("filename", _compoundDto.CompNum);
			//StartActivity(fhoto);
			await DisplayAlert("INFO", "Still not implemented yet", "OK");
		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
		}

	}

	private async void cameraMenu_Clicked(object sender, EventArgs e)
	{
		await OnPhoto();
	}

	private async Task OnPrint()
	{
		try
		{
			PrintCompound(true);
		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
		}
	}

	private async void PrintCompound(bool isNeedCheck)
	{
		var modal = new CustomLoading((ContentPage)this.Children[0]);

		try
		{
			await Task.Run(async () =>
			{
				MainThread.BeginInvokeOnMainThread(async () => await modal.ShowPopupAsync());
				await Task.Delay(50);
				MainThread.BeginInvokeOnMainThread(() => modal.UpdateMessage("Processing..."));

				var compoundDto = CompoundBll.GetCompoundByCompoundNumber(txtCompound.Text);

				var offendDto = TableFilBll.GetOffendByCodeAndAct(compoundDto?.OfdCode, compoundDto?.ActCode);

				var actDto = TableFilBll.GetActByCode(compoundDto?.ActCode);

				var enforcerDto = EnforcerBll.GetEnforcerById(compoundDto?.EnforcerId);

				var bitmap1 = new PrintImageBll().CreateKompaunType1Bitmap_1(Platform.CurrentActivity ?? Android.App.Application.Context, CommunityToolkit.Maui.Resource.Drawable.Logo, Resource.Drawable.jompayLogo, Resource.Drawable.asign, compoundDto, offendDto, actDto, enforcerDto);
				var bitmap2 = new PrintImageBll().CreateKompaunType1Bitmap_2(Platform.CurrentActivity ?? Android.App.Application.Context, CommunityToolkit.Maui.Resource.Drawable.Logo, Resource.Drawable.jompayLogo, Resource.Drawable.asign, compoundDto, offendDto, actDto, enforcerDto);

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

		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
			MainThread.BeginInvokeOnMainThread(() => modal.ClosePopup());
		}
	}

	private void SetPrintBufferBmp(Bitmap bitmap1, Bitmap bitmap2)
	{
		var insPrint = new PrintDataBll();
		insPrint.PrintCompoundBitmap(bitmap1, bitmap2);

		var stringData = GeneralBll.SetPrintDataToString(insPrint.GetListPrintData());
		OnShowPrintPageSentString(Enums.PrintType.CompoundType1, stringData);

	}

	protected async void OnShowPrintPageSentString(Enums.PrintType printType, string printData, bool isSecondPrint = false)
	{
		await Navigation.PushAsync(new PrintPage((ContentPage)this.Children[0], printType.ToString(), isSecondPrint, printData));
	}

	private async void printMenu_Clicked(object sender, EventArgs e)
	{
		await OnPrint();
	}
}