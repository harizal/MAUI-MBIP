using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.Classes;
using Newtonsoft.Json;

namespace AndroidCompound5.Pages;

public partial class FindOffendPage : ContentPage
{
	private readonly Action _onDataReturned;
	private List<OffendItem> _findDtos;
	public List<string> _datas = [];
	public string _type, _activityName, _act, _compType;
	public FindOffendPage(Action action, string type, string activityName, string act, string compType)
	{
		InitializeComponent();

		Title = "Carian Page";
		_onDataReturned = action;
		_type = type;
		_activityName = activityName;
		_act = act;
		_compType = compType;

		GetListTable();
		listView.ItemsSource = _datas;
	}

	private void GetListTable()
	{
		switch (_type)
		{
			case Constants.FindOffend:
				_findDtos = RefreshOffend();
				Title = "Carian (Perenggan)";
				break;
			case Constants.FindOffend2:
				_findDtos = RefreshOffend2();
				Title = "Carian (Perenggan)";
				break;
		}

		_datas = _findDtos?.Select(m => $"{m.Code} {m.Cetak} {m.Description}")?.ToList() ?? [];
	}

	private List<OffendItem> RefreshOffend()
	{

		var listTableitems = new List<OffendItem>();

		var listOffend = TableFilBll.GetOffendByActCode(_act).OrderBy(c => c.ShortDesc);
		foreach (var offendDto in listOffend)
		{

			var find = new OffendItem();
			find.Code = offendDto.OfdCode;
			find.Cetak = offendDto.PrnDesc;
			//find.Cetak = offendDto.ShortDesc;
			find.Description = offendDto.LongDesc;
			find.Amount = GeneralBll.ConvertAmountOffend(offendDto.OffendAmt);
			listTableitems.Add(find);
		}


		return listTableitems;
	}

	private List<OffendItem> RefreshOffend2()
	{

		var listTableitems = new List<OffendItem>();
		var listOffend = TableFilBll.GetOffendByActCode(_act).OrderBy(c => c.ShortDesc);
		foreach (var offendDto in listOffend)
		{
			if (offendDto.CompType == _compType)
			{
				var find = new OffendItem();
				find.Code = offendDto.OfdCode;
				find.Cetak = offendDto.PrnDesc;
				//find.Cetak = offendDto.ShortDesc;
				find.Description = offendDto.LongDesc;
				find.Amount = GeneralBll.ConvertAmountOffend(offendDto.OffendAmt);
				listTableitems.Add(find);
			}

		}

		return listTableitems;
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

	private void MySearchBar_TextChanged(object sender, TextChangedEventArgs e)
	{
		string searchText = e.NewTextValue?.ToLower() ?? string.Empty;
		listView.ItemsSource = string.IsNullOrWhiteSpace(searchText)
			? (_findDtos?.Select(m => $"{m.Code} {m.Cetak} {m.Description}")?.ToList() ?? [])
			: _findDtos?.Where(item => $"{item.Code}{item.Cetak}{item.Description}".ToLower().Contains(searchText))
				.Select(m => $"{m.Code} {m.Cetak} {m.Description}")?.ToList() ?? [];
	}

	private async void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
	{
		if (e.SelectedItem == null) return;

		GlobalClass.FindResult = true;
		GlobalClass.ReturnCodeFind = "";
		GlobalClass.ReturnCodeFind = _findDtos[e.SelectedItemIndex].Code + ";"+_findDtos[e.SelectedItemIndex].Description;
		await Navigation.PopAsync();

	}
}