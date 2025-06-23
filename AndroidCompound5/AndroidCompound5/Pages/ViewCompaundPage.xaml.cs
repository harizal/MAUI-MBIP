using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.Classes;

namespace AndroidCompound5.Pages;


public partial class ViewCompaundPage : ContentPage
{
	public List<ViewCompoundDto> _listItems = [];
	private const string _SeparateString = ": ";

	public ViewCompaundPage()
	{
		InitializeComponent();
		listView.ItemsSource = _listItems;
	}

	private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
	{
		var _selectedItem = e.SelectedItem as ViewCompoundDto;
		var compound = CompoundBll.GetCompoundByCompoundNumber(_selectedItem?.CompundNumber);
#if DEBUG
		if (_selectedItem.CompundNumber == $"{_SeparateString}CompundNumber1")
		{
			compound = new CompoundDto
			{
				CompType = Constants.CompType1
			};
		}
		else if (_selectedItem.CompundNumber == $"{_SeparateString}CompundNumber2")
		{
			compound = new CompoundDto
			{
				CompType = Constants.CompType2
			};
		}
		else if (_selectedItem.CompundNumber == $"{_SeparateString}CompundNumber3")
		{
			compound = new CompoundDto
			{
				CompType = Constants.CompType3
			};
		}
		else if (_selectedItem.CompundNumber == $"{_SeparateString}CompundNumber5")
		{
			compound = new CompoundDto
			{
				CompType = Constants.CompType5
			};
		}
		else
		{
			compound = null;
		}
#endif
		if (compound == null)
		{
			await DisplayAlert("ERROR", "Can not find compound Number : " + _selectedItem.CompundNumber, "OK");
			return;
		}
		_selectedItem.CompundNumber = _selectedItem.CompundNumber.Replace(_SeparateString, "");
		if (compound.CompType == Constants.CompType1)
			await Navigation.PushAsync(new ViewCompaund1Page(_selectedItem.CompundNumber));
		else if (compound.CompType == Constants.CompType2)
			await Navigation.PushAsync(new ViewCompaund2Page(_selectedItem.CompundNumber));
		else if (compound.CompType == Constants.CompType3)
			await Navigation.PushAsync(new ViewCompaund3Page(_selectedItem.CompundNumber));
		else if (compound.CompType == Constants.CompType5)
			await Navigation.PushAsync(new ViewCompaund2Page(_selectedItem.CompundNumber));
		else
		{
			await DisplayAlert("INFO", "Compound Found , Type : " + compound.CompType, "OK");
		}

		listView.SelectedItem = null;
	}

	private List<ViewCompoundDto> OnFind(int iKey, string sSearchKey)
	{
		var listViewCompound = new List<ViewCompoundDto>();
		var listCompound = CompoundBll.FindViewCompoundList(iKey, sSearchKey);

		var listCompoundOnline = CompoundBll.ListCompoundNumberOnline();

		foreach (var compoundDto in listCompound)
		{
			var viewCompound = new ViewCompoundDto();
			viewCompound.IsOnlineFileExist = listCompoundOnline.Any(c => c.Contains(compoundDto.CompNum));

			viewCompound.CompundNumber = _SeparateString + compoundDto.CompNum;

			viewCompound.EnforcerId = _SeparateString + compoundDto.EnforcerId;

			viewCompound.OffendDesc = compoundDto.OffendDesc;

			listViewCompound.Add(viewCompound);
		}

		return listViewCompound.OrderByDescending(c => c.CompundNumber).ToList();
	}

	private async void btnLogin_Clicked(object sender, EventArgs e)
	{
		var modal = new CustomLoading(this);

		MainThread.BeginInvokeOnMainThread(() => modal.ShowPopupAsync());
		await Task.Run(async () =>
		{
			await Task.Delay(500);
			MainThread.BeginInvokeOnMainThread(() => modal.UpdateMessage("Processing..."));

			var type = 0;
			if ((string)dropdownPicker.SelectedItem == "No. Kenderaan")
				type = 1;
			else if ((string)dropdownPicker.SelectedItem == "Seksyen")
				type = 2;
			else if ((string)dropdownPicker.SelectedItem == "Jenis Kenderaan")
				type = 3;
			else if ((string)dropdownPicker.SelectedItem == "Waktu")
				type = 4;

			var datas = OnFind(type, txtSearchValue.Text);
#if DEBUG
			int[] values = { 1, 2, 3, 5 };
			for (int i = 0; i < 20; i++)
			{
				var ind = values[i % values.Length];
				datas.Add(new ViewCompoundDto
				{
					IsOnlineFileExist = i % 2 == 0,
					CompundNumber = _SeparateString + "CompundNumber" + ind,
					EnforcerId = _SeparateString + "EnforcerId" + ind,
					OffendDesc = _SeparateString + "Lorem Ipsum is simply dummy text of the printing and typesetting industry."
				});
			}
#endif

			_listItems = datas;
			MainThread.BeginInvokeOnMainThread(() =>
			{
				modal.ClosePopup();
				listView.ItemsSource = _listItems;
			});
		});
	}


	private async void ImageButton_Clicked(object sender, EventArgs e)
	{
		var button = sender as ImageButton;
		var item = button?.CommandParameter as ViewCompoundDto;
		if (item != null)
		{
			await DisplayAlert("ERROR", "Compound Number : " + item.CompundNumber, "OK");

			var result = CompoundBll.ProcessCompoundOnlineServiceManual(item.CompundNumber);
			string message = "";
			if (result)
			{
				message = item.CompundNumber + " Success send.";
				button.IsVisible = false;
			}
			else
				message = item.CompundNumber + " Fail send.";

			await DisplayAlert("ERROR", message, "OK");
		}
	}
}