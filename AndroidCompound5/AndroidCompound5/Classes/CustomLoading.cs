using Android.Bluetooth;
using AndroidCompound5.Pages;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidCompound5.Classes
{
	public class CustomLoading
	{
		private ContentPage _contentPage;
		private ModalPage _modalPage;
		private Label _label;
		private ActivityIndicator _activityIndicator;
		private CollectionView _collectionView;
		private List<BluetoothDevice> _bluetoothDevices;
		private Action _action;

		public CustomLoading(ContentPage contentPage)
		{
			_contentPage = contentPage;
			_modalPage = new ModalPage();
			// Initialize the UI elements for the loading popup
			_label = new Label
			{
				Text = "Loading, please wait...",
				HorizontalTextAlignment = TextAlignment.Center
			};

			_activityIndicator = new ActivityIndicator
			{
				IsRunning = true,  // Start the spinner animation
				Color = Colors.Gray
			};

			// Create the vertical stack layout for the popup
			var verticalStackLayout = new VerticalStackLayout
			{
				Spacing = 25,
				Padding = 30,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				Children =
			{
				_label,
				_activityIndicator
			}
			};

			// Create the modal page to show the loading popup
			_modalPage.Content = verticalStackLayout;
		}

		public CustomLoading(ContentPage contentPage, bool listView, Action action)
		{
			_bluetoothDevices = new List<BluetoothDevice>();
			_action = action;
			_contentPage = contentPage;
			_modalPage = new ModalPage();
			// Initialize the UI elements for the loading popup
			_label = new Label
			{
				Text = "Select your item",
				HorizontalTextAlignment = TextAlignment.Center
			};

			_collectionView = new CollectionView
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
				SelectionMode = SelectionMode.Single
			};

			if (GlobalClass.BluetoothAndroid?._listDevice != null)
			{
				_bluetoothDevices.AddRange(from item in GlobalClass.BluetoothAndroid._listDevice
										   select item);
			}

			_activityIndicator = new ActivityIndicator
			{
				IsRunning = true,  // Start the spinner animation
				Color = Colors.Gray
			};

			_collectionView.ItemsSource = _bluetoothDevices;
			_collectionView.SelectionChanged += async (sender, e) => await CollectionView_SelectionChangedAsync(sender, e);

			_activityIndicator.IsVisible = false;
			// Create the vertical stack layout for the popup
			var verticalStackLayout = new VerticalStackLayout
			{
				Spacing = 25,
				Padding = 30,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				Children =
				{
					_label,
					_activityIndicator,
					_collectionView,
				}
			};

			// Create the modal page to show the loading popup
			_modalPage.Content = verticalStackLayout;
		}

		private async Task CollectionView_SelectionChangedAsync(object? sender, SelectionChangedEventArgs e)
		{
			var cv = (CollectionView)sender;
			if (cv?.SelectedItem == null)
				return;
			var selectedItem = (BluetoothDevice)cv.SelectedItem;
			_activityIndicator.IsVisible = true;
			_collectionView.IsVisible = false;
			await Task.Delay(500);
			GlobalClass.BluetoothDevice = selectedItem;
			_contentPage?.DisplaySnackbar($"You selected {selectedItem?.Name} with address {selectedItem?.Address}.");
			try
			{
				_action?.Invoke();
				_modalPage?.Close();
			}
			catch (Exception ex)
			{
				_contentPage?.DisplaySnackbar(ex.Message);
				_modalPage?.Close();
			}
		}

		public async Task ShowPopupAsync()
		{
			await _contentPage.ShowPopupAsync(_modalPage);
		}

		// Close the loading popup
		public void ClosePopup()
		{
			_modalPage?.Close();
		}

		// Optionally, update the message
		public void UpdateMessage(string message)
		{
			_label.Text = message;
		}
	}
}
