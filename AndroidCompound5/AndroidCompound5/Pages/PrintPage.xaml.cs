using Android.App;
using Android.Bluetooth;
using Android.Widget;
using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.Classes;
using AndroidCompound5.PrintService;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using static AndroidCompound5.AimforceUtils.Enums;

namespace AndroidCompound5.Pages;

public partial class PrintPage : ContentPage
{
	private bool _isStop;
	private bool _isPrintingError;

	private const string _closeText = "CLOSE";
	private const string _stopText = "CANCEL";

	private PrintDataDto _listdata = new PrintDataDto();
	private PrinterBaseBll _insPrint;

	private string _printType = "";
	private bool _secondPrint = false;
	private string _printData = "";
	private ContentPage _contentPage;


	private ModalPage _modalPage;
	private Label _label;
	private ActivityIndicator _activityIndicator;
	private CollectionView _collectionView;
	private List<BluetoothDevice> _bluetoothDevices;

	public PrintPage(ContentPage contentPage, string printType, bool isSecondPrint, string printData)
	{
		InitializeComponent();

		_contentPage = contentPage;
		_printType = printType;
		_secondPrint = isSecondPrint;
		_printData = printData;

		_label = GetLabel();
		_activityIndicator = GetActivityIndicator();
		_bluetoothDevices = [];
		_collectionView = GetCollectionView();
		_collectionView.SelectionChanged += async (sender, e) => await CollectionView_SelectionChangedAsync(sender, e);
		_modalPage = new ModalPage();

		MainThread.BeginInvokeOnMainThread(() =>
		{
			Thread.Sleep(Constants.DefaultWaitingMilisecond);
			SetInit();
		});
	}

	private Label GetLabel()
	{
		return new Label
		{
			Text = "Please wait...",
			FontSize = 20,
			HorizontalOptions = LayoutOptions.Center
		};
	}

	private ActivityIndicator GetActivityIndicator()
	{
		return new ActivityIndicator
		{
			Color = (Color)Microsoft.Maui.Controls.Application.Current.Resources["Primary"],
			IsRunning = true
		};
	}

	private CollectionView GetCollectionView()
	{
		var collectionView = new CollectionView
		{
			// Define the layout for the CollectionView
			ItemTemplate = new DataTemplate(() =>
			{
				// Create a StackLayout for each item in the CollectionView
				var stackLayout = new StackLayout
				{
					Padding = new Thickness(10),
					Spacing = 5
				};

				// Create a Label to display the name
				var nameLabel = new Label();
				nameLabel.SetBinding(Label.TextProperty, "Name");

				// Create a Label to display the age
				var ageLabel = new Label();
				ageLabel.SetBinding(Label.TextProperty, "Address");

				// Add labels to the StackLayout
				stackLayout.Children.Add(nameLabel);
				stackLayout.Children.Add(ageLabel);

				return stackLayout;
			}),
			SelectionMode = SelectionMode.Single,
			IsVisible = false
		};

		if (GlobalClass.BluetoothAndroid?._listDevice != null)
		{
			_bluetoothDevices.AddRange(from item in GlobalClass.BluetoothAndroid._listDevice
									   select item);
		}

		collectionView.ItemsSource = _bluetoothDevices;

		return collectionView;
	}

	private async Task PrintAsync(bool isNeedCheck)
	{
		try
		{
			if (isNeedCheck)
			{
				var isPrepared = await PreparePrinterDevice();
				if (!isPrepared)
				{
					UpdatePrintStatusInfo("No Bluetooth device Selected");
					_isPrintingError = true;
					SetButtonStopClose(true);
					return;
				}
			}
			else
			{
				UpdatePrintStatusInfo("Bluetooth device enabled");
				UpdatePrintStatusImage(Enums.PrintStatus.BluetoothEnabled);

				ThreadPool.QueueUserWorkItem(o => OnPrinting());
			}
		}
		catch (Exception ex)
		{
			UpdatePrintStatusInfo("UnExpected Error Print : " + ex.Message);
			UpdatePrintStatusImage(Enums.PrintStatus.Error);
		}
	}

	private async Task CollectionView_SelectionChangedAsync(object? sender, SelectionChangedEventArgs e)
	{
		try
		{
			MainThread.BeginInvokeOnMainThread(() => { _modalPage.Close(); });

			var cv = (CollectionView)sender;
			if (cv?.SelectedItem == null)
				return;
			var selectedItem = (BluetoothDevice)cv.SelectedItem;
			_activityIndicator.IsVisible = true;
			await Task.Delay(500);
			GlobalClass.BluetoothDevice = selectedItem;
			MainThread.BeginInvokeOnMainThread(() =>
			{
				_contentPage?.DisplaySnackbar($"You selected {selectedItem?.Name} with address {selectedItem?.Address}.");
			});

			await PrintAsync(false);
		}
		catch (Exception ex)
		{
			UpdatePrintStatusInfo("UnExpected Error bluetooth device selection: " + ex.Message);
			UpdatePrintStatusImage(Enums.PrintStatus.Error);
		}
	}

	private void SetInit()
	{
		try
		{
			_insPrint = new PrinterBaseBll();

			SharedPreferences.SaveString(SharedPreferencesKeys.StatusPrint, "");
			_listdata = GeneralBll.GetListPrintDataString(_printData);

			Print(true);
		}
		catch (Exception ex)
		{
			LogFile.WriteLogFile(Title, "SetInit", ex.Message, Enums.LogType.Error);
			GeneralAndroidClass.ShowToast(ex.Message);
		}
	}

	private void SetButtonStopClose(bool isClose)
	{

		string textButton = _stopText;
		if (isClose)
		{
			textButton = _closeText;
		}

		btnBack.Text = textButton;
	}

	private async void btnBack_Clicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}


	#region Print
	private void UpdatePrintStatusInfo(string message)
	{

		if (lblStatus != null)
		{
			lblStatus.Text = message;
			Thread.Sleep(20);
		}
	}

	private void UpdatePrintStatusImage(Enums.PrintStatus printStatus)
	{
		switch (printStatus)
		{
			case Enums.PrintStatus.BluetoothEnabled:
				MainThread.BeginInvokeOnMainThread(() => Logo.Source = ImageSource.FromFile("statusprint1a.png"));
				break;
			case Enums.PrintStatus.Connected:
				MainThread.BeginInvokeOnMainThread(() => Logo.Source = ImageSource.FromFile("statusprint2a.png"));
				break;
			case Enums.PrintStatus.InProgress1:
				MainThread.BeginInvokeOnMainThread(() => Logo.Source = ImageSource.FromFile("statusprint3a.png"));
				break;
			case Enums.PrintStatus.InProgress2:
				MainThread.BeginInvokeOnMainThread(() => Logo.Source = ImageSource.FromFile("statusprint4a.png"));
				break;
			case Enums.PrintStatus.InProgress3:
			case Enums.PrintStatus.WaitingClose:
			case Enums.PrintStatus.Completed:
				MainThread.BeginInvokeOnMainThread(() => Logo.Source = ImageSource.FromFile("statusprint5a.png"));
				break;
			case Enums.PrintStatus.Error:
				MainThread.BeginInvokeOnMainThread(() => Logo.Source = ImageSource.FromFile("printererrora.png"));
				break;
		}
		Thread.Sleep(20);
	}

	private async Task<bool> PreparePrinterDevice()
	{
		try
		{
			UpdatePrintStatusInfo("PreparePrinterDevice() - Checking Bluetooth Printer...");
			if (!GeneralAndroidClass.IsPrinterExist())
			{
				GeneralAndroidClass.ShowToast("Printer Device not found.");

				return false;
			}

			if (GlobalClass.BluetoothDevice == null)
			{
				GlobalClass.BluetoothSocket = null;

				_label.Text = "Select the Bluetooth Device!";
				_activityIndicator.IsVisible = false;
				_collectionView.IsVisible = true;

				if ((_bluetoothDevices.Any()))
				{
					_bluetoothDevices?.Clear();
					_bluetoothDevices.AddRange(from item in GlobalClass.BluetoothAndroid._listDevice
											   select item);

					GlobalClass.BluetoothSocket = null;

					var verticalStacklayout = new VerticalStackLayout
					{
						Spacing = 25,
						Padding = 30,
						HorizontalOptions = LayoutOptions.Center,
						VerticalOptions = LayoutOptions.Center,
						Children = {
										_label,
										_activityIndicator,
										_collectionView
										}
					};

					_modalPage.Content = verticalStacklayout;
					await Task.Delay(50);
					MainThread.BeginInvokeOnMainThread(() => _contentPage.ShowPopup(_modalPage));
				}
			}

			return true;
		}
		catch (Exception ex)
		{
			UpdatePrintStatusInfo("UnExpected Error PreparePrinterDevice : " + ex.Message);
			//UpdatePrintStatusImage(Enums.PrintStatus.Error);
			//GeneralAndroidClass.ShowToast("UnExpected Error PreparePrinterDevice() : " + ex.Message);
			return false;
		}

	}


	private async void Print(bool isNeedCheck)
	{
		try
		{
			if (isNeedCheck)
			{
				var isPrepared = await PreparePrinterDevice();
				if (!isPrepared)
				{
					UpdatePrintStatusInfo("No Bluetooth device Selected");
					_isPrintingError = true;
					SetButtonStopClose(true);
					return;
				}
			}

			UpdatePrintStatusInfo("Bluetooth device enabled");
			//UpdatePrintStatusImage(Enums.PrintStatus.BluetoothEnabled);

			ThreadPool.QueueUserWorkItem(o => OnPrinting());

		}
		catch (Exception ex)
		{
			UpdatePrintStatusInfo("UnExpected Error Print : " + ex.Message);
			//UpdatePrintStatusImage(Enums.PrintStatus.Error);
			//GeneralAndroidClass.ShowToast("UnExpected Error lvResult_ItemClick() : " + ex.Message);
		}
	}

	private void OnPrinting()
	{
		bool isSecondPrint = false;
		SetButtonStopClose(false);
		string prnName = "";
		_isPrintingError = false;
		string errMsg = "";

		try
		{
#if PrintFile
                _insPrint = new PrinterWoosimBll();
#else
			if (GlobalClass.BluetoothDevice == null)
			{
				_isPrintingError = true;
				return;
			}
			else
			{
				prnName = GlobalClass.BluetoothDevice.Name;
				if (prnName.ToLower().Contains("ptp"))
					_insPrint = new PrinterPTPBll();
				else if (prnName.ToLower().Contains("qr380"))
					_insPrint = new PrinterSPRTBll();
				else if (prnName.ToLower().Contains("woosim"))
					_insPrint = new PrinterWoosimBll();
				else
					_insPrint = new PrinterZebraBll();
			}
#endif
			//var listData = GeneralBll.GetListPrintDataPreferences();

			UpdatePrintStatusInfo($"Printer Connecting: {prnName}");
			var printResponse = _insPrint.BluetoothConnect(GlobalClass.BluetoothDevice);
			if (printResponse.Succes)
			{
				UpdatePrintStatusInfo($"Printer Connected: {prnName}");
				UpdatePrintStatusImage(Enums.PrintStatus.Connected);

				Thread.Sleep(200);
				//wake up printer
				_insPrint.PrintInitialise();

				int printStatus = _insPrint.PrinterFirmware();


				Random rng = new Random();

				int rand1 = rng.Next(Constants.MINNUM, Constants.MAXNUM); // number between 0 and 99
				LogFile.WriteLogFile("FwCODE : " + GlobalClass.FwCode + "; Random Number : " + rand1.ToString());

				if (rand1 < Constants.LESSNUM)
				{
					if (printStatus == 0 || GlobalClass.FwCode != Constants.FWCODE)
					{
						errMsg =  "Cannot find Printer !! ";
						goto OnErrExit;
					}
				}


				printStatus = _insPrint.PrinterQuery();

				if (printStatus != 0 && _insPrint.GetPrinterMessage().ToLower().Contains("broken pipe"))
				{
					#region reconnect bluetooth printer process
					// try reconnect 
					prnName = GlobalClass.BluetoothDevice.Name;
					if (prnName.ToLower().Contains("ptp"))
						_insPrint = new PrinterPTPBll();
					else if (prnName.ToLower().Contains("qr380"))
						_insPrint = new PrinterSPRTBll();
					else
						_insPrint = new PrinterWoosimBll();

					UpdatePrintStatusInfo($"Printer Re-connecting: {prnName}");

					_insPrint.socket = null;

					printResponse = _insPrint.BluetoothConnect(GlobalClass.BluetoothDevice);

					if (printResponse.Succes)
					{
						UpdatePrintStatusInfo($"Printer Connected: {prnName}");
						UpdatePrintStatusImage(Enums.PrintStatus.Connected);

						//20230222 hsyip: log for future diagnosis
						LogFile.WriteLogFile("PrintPage", "OnPrint()", $"Printer Reconnected: {prnName}", Enums.LogType.Info);

						_insPrint.PrintInitialise();

						printStatus = _insPrint.PrinterQuery();
					}
					else
					{
						//20230222 hsyip: log for future diagnosis
						LogFile.WriteLogFile("PrintPage", "OnPrint()", $"Printer Failed to reconnect: {prnName}", Enums.LogType.Error);

						errMsg = $"Printer error: {printResponse.Message}";
						goto OnErrExit;
					}

					//end reconnect
					#endregion reconnect bluetooth printer process
				}

				//int printStatus = 0;

				if (printStatus == 0)
				{
					UpdatePrintStatusInfo("Printing - In Progress");
					Thread.Sleep(100);
					var lineCount = _listdata.DataItems.Count;
					int lineProgress = 0;
					int statusProgress = 0;
					foreach (var item in _listdata.DataItems)
					{
						if (_isStop) break;

						//processing
						if (_insPrint.PrintData(item) == true)
						{

							if (lineProgress++ <= lineCount)
							{
								UpdatePrintStatusInfo($"Sending: {lineProgress.ToString()} lines");
								if ((lineProgress % 10) == 0)
								{
									statusProgress++;
									if (statusProgress > 3)
										statusProgress = 0;

									switch (statusProgress)
									{
										case 0:
											UpdatePrintStatusImage(Enums.PrintStatus.Connected);
											break;
										case 1:
											UpdatePrintStatusImage(Enums.PrintStatus.InProgress1);
											break;
										case 2:
											UpdatePrintStatusImage(Enums.PrintStatus.InProgress2);
											break;
										case 3:
											UpdatePrintStatusImage(Enums.PrintStatus.InProgress3);
											break;
									}
								}
							}
						}
						else
						{
							errMsg = $"Printer error: {_insPrint.GetPrinterMessage()}";
							goto OnErrExit;
						}
					}
				}
				else
				{
					errMsg = $"Printer error: {_insPrint.GetPrinterMessage()}";
					goto OnErrExit;
				}


				if (!isSecondPrint)
				{
					FinishPrintData();
				}

				//printing is good.
				return;
			}
			else
			{
				errMsg = $"Printer error: {printResponse.Message}";
				goto OnErrExit;
				//RunOnUiThread(() => GeneralAndroidClass.ShowToast(printResponse.Message));
			}
		}
		catch (Exception ex)
		{
			errMsg = "UnExpected Error OnPrinting() : " + ex.Message;
			goto OnErrExit;
		}

	OnErrExit:
		_isPrintingError = true;
		UpdatePrintStatusInfo($"{errMsg}");
		UpdatePrintStatusImage(Enums.PrintStatus.Error);
		SetButtonStopClose(true);
		GlobalClass.BluetoothDevice = null;
		return;
	}

	private void FinishPrintData()
	{
		UpdatePrintStatusInfo("Printer waiting to close");
		UpdatePrintStatusImage(Enums.PrintStatus.WaitingClose);
		Thread.Sleep(200);

		int printStatus = _insPrint.PrintClose();

		Thread.Sleep(200);

		if (_isPrintingError)
		{
			//reset the bluetooth socket
			//GlobalClass.BluetoothSocket = null;
			//printStatus = -1;
		}
		else
		{
			UpdatePrintStatusInfo("Printing completed");
			UpdatePrintStatusImage(Enums.PrintStatus.Completed);
		}

		SetButtonStopClose(true);

		if (printStatus == 0)
		{
			SharedPreferences.SaveString(SharedPreferencesKeys.StatusPrint, "1");
			GeneralBll.CleanRefListDataPrint();
			if (!_isPrintingError)
			{
				MainThread.BeginInvokeOnMainThread(() =>
				{
					_contentPage?.DisplaySnackbar($"{Constants.Messages.SuccessPrint}.");
				});
			}

			MainThread.BeginInvokeOnMainThread(() => Navigation.PopAsync());
		}
		else
		{
			UpdatePrintStatusInfo($"Printer error: {_insPrint.GetPrinterMessage()}");
			UpdatePrintStatusImage(Enums.PrintStatus.Error);
			SharedPreferences.SaveString(SharedPreferencesKeys.StatusPrint, "");
		}
	}


	#endregion
}