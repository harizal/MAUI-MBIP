using Android.App;
using Android.Graphics;
using Android.Widget;
using AndroidCompound5.AimforceUtils;
using AndroidCompound5.Classes;
using System.Timers;
using static AndroidCompound5.AimforceUtils.Enums;

namespace AndroidCompound5.Pages;

public partial class RingkasanPage : ContentPage
{
	private System.Timers.Timer timer;
	private int _counter = 0;
	public RingkasanPage()
	{
		InitializeComponent();
	}
	
	protected override void OnAppearing()
	{
		base.OnAppearing();
		SetInit();
	}

	private async void SetInit()
	{
		int photocnt;
		var infoDto = InfoBll.GetInfo();
		if (infoDto == null)
		{
			await DisplayAlert(Constants.Message.ErrorMessage, "Fail control tak dijumpai. Sila hubung pejabat.", Constants.Message.OKMessage);
			return;
		}

		//photocnt = CountNumPhoto();
		lblIssued.Text = ": " + infoDto.CompCnt.ToString();
		lblNote.Text = ": " + infoDto.NoteCnt.ToString(); ;
		lblFhoto.Text = ": " + GeneralBll.getTotalPhotoCount().ToString();
		lblUnSendCompaund.Text = ": " + GeneralBll.GetTotalUnsendCompound().ToString();
		lblUnSendPhoto.Text = ": " + GeneralBll.GetTotalUnsendCompoundPhoto().ToString();
		lblStanum.Text = ": " + infoDto.StartCmp;
		if (infoDto.CurrComp >= 0)
		{
			lblEndNum.Text = ": " + Constants.DevicePrefix + infoDto.DolphinId + infoDto.CurrDate + infoDto.CurrComp.ToString("000");
		}



		lblLatitude.Text = ": " + GlobalClass.Longitude;
		lblLongitude.Text = ": " + GlobalClass.Latitude;

		lblDate.Text = GeneralBll.GetLocalDate();
		lblTime.Text = GeneralBll.GetLocalTime();

		timer = new System.Timers.Timer();
		timer.Interval = 1000;
		timer.Elapsed += timer_Elapsed;
		timer.Start();
	}

	private void timer_Elapsed(object? sender, ElapsedEventArgs e)
	{
		_counter++;
		MainThread.BeginInvokeOnMainThread(() =>
		{
			lblDate.Text = GeneralBll.GetLocalDate();
			lblTime.Text = GeneralBll.GetLocalTime();
			if ((_counter % 5) == 0)
			{
				lblLongitude.Text = GlobalClass.Longitude;
				lblLatitude.Text = GlobalClass.Latitude;
			}
		});
	}

	private async void btnCetak_Clicked(object sender, EventArgs e)
	{
		try
		{
			var printFunction = new Classes.Print.PrintRingkasan(this);
			await printFunction.Print(true);
		}
		catch (Exception ex)
		{
			await DisplayAlert(Constants.Message.ErrorMessage, Constants.UnexpectedErrorMessage, Constants.Message.OKMessage);
			LogFile.WriteLogFile(typeof(RingkasanPage).Name, "OnReprint", ex.Message, Enums.LogType.Error);
		}
	}

	//protected async void OnShowPrintPageSentString(Enums.PrintType printType, string printData, bool isSecondPrint = false)
	//{
	//	await Navigation.PushAsync(new PrintPage(this, printType.ToString(), isSecondPrint, printData));
	//}

	//private void SetPrintBufferBmp(Bitmap bitmap1)
	//{
	//	var insPrint = new PrintDataBll();
	//	insPrint.PrintCompoundBitmap(bitmap1);

	//	var stringData = GeneralBll.SetPrintDataToString(insPrint.GetListPrintData());
	//	OnShowPrintPageSentString(Enums.PrintType.TestPrint, stringData);
	//}

	//private async void OnPrinting(ProgressDialog pDialog)
	//{
	//	var bitmap1 = new PrintImageBll().CreateTestBitmap();
	//	try
	//	{
	//		SetPrintBufferBmp(bitmap1);
	//	}
	//	catch (Exception ex)
	//	{
	//		await DisplayAlert(Constants.Message.ErrorMessage, "UnExpected Error OnPrinting() : " + ex.Message, Constants.Message.OKMessage);
	//		LogFile.WriteLogFile("UnExpected Error OnPrinting() : " + ex.Message, Enums.LogType.Error);
	//	}
	//	finally
	//	{
	//		pDialog.Dismiss();
	//	}
	//}

	//private async void PrintCompound(bool isNeedCheck)
	//{
	//	try
	//	{
	//		ProgressDialog pDialog = ProgressDialog.Show(Microsoft.Maui.ApplicationModel.Platform.CurrentActivity, "Test Print", "Sila tunggu", true);
	//		MainThread.BeginInvokeOnMainThread(() =>
	//		{
	//			Thread.CurrentThread.IsBackground = true;
	//			OnPrinting(pDialog);
	//		});
			

	//	}
	//	catch (Exception ex)
	//	{
	//		await DisplayAlert(Constants.Message.ErrorMessage, "UnExpected Error PrintCompound() : " + ex.Message, Constants.Message.OKMessage);
	//		LogFile.WriteLogFile("UnExpected Error PrintCompound() : " + ex.Message, Enums.LogType.Error);
	//	}
	//}
}