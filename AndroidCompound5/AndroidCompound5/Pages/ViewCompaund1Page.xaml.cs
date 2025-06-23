using Android.Graphics;
using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.Classes;

namespace AndroidCompound5.Pages;


public partial class ViewCompaund1Page : ContentPage
{
	string _stringCompoundNo;
	private CompoundDto _compoundDto;
	public ViewCompaund1Page(string compaundNo)
	{
		InitializeComponent();

		_stringCompoundNo = compaundNo;
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
				txtCarNum.Text = _compoundDto?.Compound1Type?.CarNum;
				txtRoadTax.Text = _compoundDto?.Compound1Type?.RoadTax;

				var category = TableFilBll.GetCarCategory(_compoundDto?.Compound1Type?.Category);
				txtCategory.Text = category != null ? category?.ShortDesc : "";
				txtMake.Text = _compoundDto?.Compound1Type?.CarTypeDesc;
				txtLocation.Text = _compoundDto?.StreetDesc;
				txtPetak.Text = _compoundDto?.Compound1Type?.LotNo;

				var delivery = TableFilBll.GetDeliveryByCode(_compoundDto?.Compound1Type?.DeliveryCode);

				txtSerah.Text = delivery != null ? delivery.ShortDesc : "";

				//txtAfCode.Text = _compoundDto.Compound1Type.CouponNumber;
				var enforcer = EnforcerBll.GetEnforcerById(_compoundDto?.EnforcerId);
				if (enforcer != null)
					txtPenguatKuasa.Text = _compoundDto?.EnforcerId + "- " + enforcer?.EnforcerName;
			}

		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
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

	private void SetImageList()
	{
		GlobalClass.FileImages = new List<string>();

		var listFileName = GeneralBll.GetListFileImageNameByCompoundNumber(txtCompound.Text);

		foreach (var fileName in listFileName)
		{
			GlobalClass.FileImages.Add(fileName);
		}

	}

	private async void cameraMenu_Clicked(object sender, EventArgs e)
	{
		await OnPhoto();
	}

	private async void printMenu_Clicked(object sender, EventArgs e)
	{
		await OnPrint();
	}

	private async void PrintCompound(bool isNeedCheck)
	{
		var modal = new CustomLoading(this);

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
		await Navigation.PushAsync(new PrintPage(this, printType.ToString(), isSecondPrint, printData));
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
}