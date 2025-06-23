using Android.Bluetooth;
using Android.Graphics;
using Android.OS;
using AndroidCompound5.AimforceUtils;
using AndroidCompound5.Pages;
using Com.Woosim.Printer;

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using Color = Microsoft.Maui.Graphics.Color;

namespace AndroidCompound5.Classes.Print
{
	public class ServicetHandler : Handler
	{

		public ServicetHandler()
		{
		}

		public override void HandleMessage(Message msg)
		{
			switch (msg.What)
			{
				//case MESSAGE_DEVICE_NAME:
				//	this.InvalidateOptionsMenu();
				//	break;
				//case MESSAGE_TOAST:
				//	//Toast.MakeText(this.Activity, msg.Arg1, ToastLength.Short).Show();
				//	break;
				case 3:
					string rcvMsg = GeneralBll.ProcessRcvData((byte[])msg.Obj);
					GlobalClass.FwCode = GlobalClass.FwCode + rcvMsg;
					break;
			}
		}
	}

	public abstract class PrintBase
	{
		private ContentPage _contentPage;
		private ModalPage _modalPage;

		private Label _label;
		private ActivityIndicator _activityIndicator;
		private CollectionView _collectionView;
		private List<BluetoothDevice> _bluetoothDevices;

		ServicetHandler handler;

		public PrintBase(ContentPage contentPage)
		{
			_modalPage = new ModalPage();

			_label = GetLabel();
			_activityIndicator = GetActivityIndicator();
			_bluetoothDevices = [];
			_collectionView = GetCollectionView();
			_collectionView.SelectionChanged += async (sender, e) => await CollectionView_SelectionChangedAsync(sender, e);
			_contentPage = contentPage;
		}

		private async Task CollectionView_SelectionChangedAsync(object? sender, SelectionChangedEventArgs e)
		{
			var cv = (CollectionView)sender;
			if (cv?.SelectedItem == null)
				return;
			var selectedItem = (BluetoothDevice)cv.SelectedItem;
			_activityIndicator.IsVisible = true;
			await Task.Delay(500);
			GlobalClass.BluetoothDevice = selectedItem;
			_contentPage?.DisplaySnackbar($"You selected {selectedItem?.Name} with address {selectedItem?.Address}.");

			await Print(false);
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
				Color = (Color)Application.Current.Resources["Primary"],
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

		public async Task Print(bool isNeedCheck)
		{
			try
			{
				var verticalStacklayout = new VerticalStackLayout
				{
					Spacing = 25,
					Padding = 30,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
					Children =
					{
						_label,
						_activityIndicator,
						_collectionView
					}
				};
				_modalPage.Content = verticalStacklayout;
				MainThread.BeginInvokeOnMainThread(() => _contentPage.ShowPopup(_modalPage));
				await Task.Run(() =>
				{
					MainThread.BeginInvokeOnMainThread(async () =>
					{
						await Task.Delay(500);
						LogFile.WriteLogFile($"isNeedCheck : {isNeedCheck} - GlobalClass.BluetoothDevice : {GlobalClass.BluetoothDevice == null}");
						if (isNeedCheck || GlobalClass.BluetoothDevice == null || GlobalClass.printService == null)
						{
							if (!GeneralAndroidClass.IsPrinterExist())
							{
								_modalPage.Close();
								await _contentPage.DisplayAlert("ERROR", "Pencetak tidak ditemui", "OK");
								return;
							}

							_label.Text = "Select the Bluetooth Device!";
							_activityIndicator.IsVisible = false;
							_collectionView.IsVisible = true;

							//if ((_bluetoothDevices.Any()))
							if (!_bluetoothDevices.Any() && GlobalClass.BluetoothAndroid?._listDevice != null)
							{
								_bluetoothDevices?.Clear();
								_bluetoothDevices.AddRange(from item in GlobalClass.BluetoothAndroid._listDevice
														   select item);
							}
							else
							{
								_modalPage.Close();
								await _contentPage.DisplayAlert("ERROR", "Pencetak tidak ditemui", "OK");
							}
						}
						else
						{
							_label.Text = Constants.Messages.PrintWaitMessage;
							_collectionView.IsVisible = false;
							await Task.Delay(500);
							await OnPrintingAsync();

							_modalPage.Close();
						}
					});
				});
			}
			catch (Exception ex)
			{
				await _contentPage.DisplayAlert("ERROR", ex.StackTrace, "OK");
				_modalPage.Close();
			}
			finally
			{
				//	_modalPage.Close();
			}
		}

		async Task SendDataAsync(byte[] data)
		{
			if (GlobalClass.printService.GetState() != BluetoothPrintService.STATE_CONNECTED)
			{
				LogFile.WriteLogFile("Kompaun", "OnPrinting", "Not Connected print", Enums.LogType.Info);
				await _contentPage.DisplayAlert("ERROR", "Tiada Sambungan Pencetak", "OK");
			}
			else if (data.Length > 0)
			{
				GlobalClass.printService.Write(data);
			}
		}

		public abstract Bitmap GetBitmap();

		private async Task OnPrintingAsync()
		{
			LogFile.WriteLogFile($" is GlobalClass.printService null : {GlobalClass.printService == null}");
			if (GlobalClass.printService == null)
			{
				if (handler == null)
					handler = new ServicetHandler();

				GlobalClass.printService = new BluetoothPrintService(handler);
				GlobalClass.printService?.Connect(GlobalClass.BluetoothDevice);
				Thread.Sleep(Constants.DefaultWaitingConnectionToBluetooth);
			}
			else
			{
				LogFile.WriteLogFile($" State of GlobalClass.printService : {GlobalClass.printService.GetState()}");
				if (GlobalClass.printService.GetState() != BluetoothPrintService.STATE_CONNECTED)
				{
					GlobalClass.printService?.Connect(GlobalClass.BluetoothDevice);
					Thread.Sleep(Constants.DefaultWaitingConnectionToBluetooth);
				}
			}


			var bitmap1 = GetBitmap();

			await SendDataAsync(WoosimCmd.InitPrinter());
			await SendDataAsync(WoosimCmd.SetPageMode());
			await SendDataAsync(WoosimImage.PrintCompressedBitmap(0, 0, 0, 0, bitmap1));
			await SendDataAsync(WoosimCmd.PM_setStdMode());
			Thread.Sleep(1000);

			bitmap1.Dispose();
		}
	}
}
