using Android.App;
using Android.Widget;
using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.Classes;
using AndroidX.AppCompat.View.Menu;
using Newtonsoft.Json;

namespace AndroidCompound5.Pages;

public partial class CarianPage : ContentPage
{
	private readonly Action _onDataReturned;
	string _sFindType;
	string _sActivityName;
	int _iActive = 0;
	public List<string> _datas = [];

	private List<FindDto> _listTableitems;

	private string _stringMukim;
	private string _stringActCode;
	private string _stringZone;
	private string _stringOfdCode;
	private string _stringCompType;
	private string _stringVehicleType;
	private string _stringCarCategory;

	public CarianPage(Action onDataReturned, string type, string activityName, string mukim, string zone, string actCode, string offendCode, string comptype = "", string vehicletype = "", string carcategory = "")
	{
		_onDataReturned = onDataReturned;
		Title = "Carian Page";

		_sFindType = type;
		_sActivityName = activityName;

		_stringZone = zone;
		_stringActCode = actCode;
		_stringOfdCode = offendCode;
		_stringCompType = comptype;
		_stringVehicleType = vehicletype;
		_stringMukim = mukim;
		_stringCarCategory = carcategory;

		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		Init();
	}

	private void Init()
	{
		GetListTable();
		listView.ItemsSource = _datas;
		DisableMenus(_sFindType == Constants.FindStreet || _sFindType == Constants.FindCarType);
	}

	private void DisableMenus(bool enable)
	{
		if (!enable)
		{
			ToolbarItems.Remove(newMenu);
			ToolbarItems.Remove(replaceMenu);
		}
	}

	private void GetListTable()
	{
		switch (_sFindType)
		{
			case Constants.FindMukim:
				_listTableitems = RefreshMukim();
				Title = "Carian (Mukim)";
				break;
			case Constants.FindZone:
				_listTableitems = RefreshZone();
				Title = "Carian (Kawasan)";
				break;
			case Constants.FindAct:
				_listTableitems = RefreshAct();
				Title = "Carian (Perundangan)";
				break;
			case Constants.FindCarColor:
				_listTableitems = RefreshCarColor();
				Title = "Carian (Warna)";
				break;
			case Constants.FindCarCategory:
				_listTableitems = RefreshCategory();
				Title = "Carian (Kategori)";
				break;
			case Constants.FindCarType:
				_listTableitems = RefreshCarType();
				Title = "Carian (Jenis Kenderaan)";
				break;
			case Constants.FindStreet:
				_listTableitems = RefreshStreet();
				Title = "Carian (Lokasi)";
				break;
			case Constants.FindEnforcer:
				_listTableitems = RefreshEnforcer();
				Title = "Carian (Saksi-saksi)";
				break;
			case Constants.FindKesalahan:
				_listTableitems = RefreshKesalahan();
				Title = "Carian (Butir Kesalahan)";
				break;
			case Constants.FindDelivery:
				_listTableitems = RefreshDelivery();
				Title = "Carian (Cara Serah)";
				break;
			case Constants.FindLot:
				_listTableitems = RefreshLot();
				Title = "Carian (Petak)";
				break;
			case Constants.FindTempatJadi:
				_listTableitems = RefreshTempatJadi();
				Title = "Carian (Tempat Jadi)";
				break;
			case Constants.FindTujuan:
				_listTableitems = RefreshTujuan();
				Title = "Carian (Tujuan)";
				break;
			case Constants.FindPerniagaan:
				_listTableitems = RefreshPerniagaan();
				Title = "Carian (Perniagaan)";
				break;
		}
		_datas = _listTableitems?.Select(m => $"{m.Code} {m.Description}")?.ToList() ?? [];
	}

	private async void HandleAfterBackPage()
	{
		if (!GlobalClass.FindResult) return;
		if (_iActive == 1)
		{
			_iActive = 0;
			if (_sFindType == Constants.FindStreet)
				GlobalClass.ReturnCodeFind += ";" + _stringMukim + ";" + _stringZone;

			GlobalClass.FindResult = true;
			await Navigation.PopAsync();
		}
		else if (_iActive == 2)
		{
			_iActive = 0;

			_sFindType = Constants.FindStreet;
			_stringMukim = GlobalClass.ReturnCodeFind.Split(';')[0];
			_stringZone = GlobalClass.ReturnCodeFind.Split(';')[1];

			Init();
		}
	}

	private void ClosePage()
	{
		_onDataReturned?.Invoke();
	}

	private List<FindDto> RefreshPerniagaan()
	{
		var listTableitems = new List<FindDto>();
		var data = new FindDto();
		data.Code = "1";
		data.Description = "MAKANAN BERMASAK";
		listTableitems.Add(data);

		data = new FindDto();
		data.Code = "2";
		data.Description = "MINUMAN & BUAH-BUAHAN";
		listTableitems.Add(data);

		data = new FindDto();
		data.Code = "3";
		data.Description = "KERETA TERPAKAI";
		listTableitems.Add(data);

		data = new FindDto();
		data.Code = "4";
		data.Description = "MINUMAN & MAKANAN RINGAN";
		listTableitems.Add(data);


		return listTableitems;
	}
	private List<FindDto> RefreshTujuan()
	{
		var listTableitems = new List<FindDto>();
		var data = new FindDto();
		data.Code = "1";
		data.Description = "PROMOSI KERETA";
		listTableitems.Add(data);

		data = new FindDto();
		data.Code = "2";
		data.Description = "KENDERAAN TERBIAR";
		listTableitems.Add(data);

		data = new FindDto();
		data.Code = "3";
		data.Description = "MEMASANG KHEMAH TANPA KEBENARAN";
		listTableitems.Add(data);

		data = new FindDto();
		data.Code = "4";
		data.Description = "KENDERAAN BURUK";
		listTableitems.Add(data);

		data = new FindDto();
		data.Code = "5";
		data.Description = "MELETAK BARANG";
		listTableitems.Add(data);

		return listTableitems;
	}
	private List<FindDto> RefreshTempatJadi()
	{
		var listTableitems = new List<FindDto>();
		var listData = TableFilBll.GetAllTempatJadi();
		foreach (var data in listData)
		{
			var find = new FindDto();
			find.Code = data.Code;
			find.Description = data.Description;
			listTableitems.Add(find);
		}

		return listTableitems;
	}
	private List<FindDto> RefreshLot()
	{
		var listTableitems = new List<FindDto>();
		var data = new FindDto();
		data.Code = "1";
		data.Description = "OKU";
		listTableitems.Add(data);

		data = new FindDto();
		data.Code = "2";
		data.Description = "PILI BOMBA";
		listTableitems.Add(data);

		//data = new FindDto();
		//data.Code = "3";
		//data.Description = "KECEMASAN";
		//listTableitems.Add(data);

		data = new FindDto();
		data.Code = "3";
		data.Description = "MOTOSIKAL";
		listTableitems.Add(data);

		data = new FindDto();
		data.Code = "4";
		data.Description = "AMBULAN";
		listTableitems.Add(data);

		data = new FindDto();
		data.Code = "5";
		data.Description = "PETAK KHAS";
		listTableitems.Add(data);

		data = new FindDto();
		data.Code = "6";
		data.Description = "TNB";
		listTableitems.Add(data);

		data = new FindDto();
		data.Code = "7";
		data.Description = "MESIN TIKET";
		listTableitems.Add(data);



		return listTableitems;
	}
	private List<FindDto> RefreshMukim()
	{
		var listTableitems = new List<FindDto>();
		var listMukim = TableFilBll.GetAllMukim();
		foreach (var mukimDto in listMukim)
		{
			var find = new FindDto();
			find.Code = mukimDto.Code;
			find.Description = mukimDto.LongDesc;
			listTableitems.Add(find);
		}

		return listTableitems;
	}
	private List<FindDto> RefreshZone()
	{
		var listTableitems = new List<FindDto>();

		var listZone = TableFilBll.GetZoneByMukim(_stringMukim).OrderBy(c => c.LongDesc);
		foreach (var zoneDto in listZone)
		{
			var find = new FindDto();
			find.Code = zoneDto.Code;
			find.Description = zoneDto.LongDesc;
			listTableitems.Add(find);
		}

		return listTableitems;
	}
	private List<FindDto> RefreshAct()
	{
		var listTableitems = new List<FindDto>();

		var listAct = TableFilBll.GetAllAct();
		foreach (var actDto in listAct)
		{
			var find = new FindDto();
			find.Code = actDto.Code;
			find.Description = actDto.ShortDesc;
			listTableitems.Add(find);
		}
		return listTableitems;

	}
	private List<FindDto> RefreshCarColor()
	{
		var listTableitems = new List<FindDto>();

		var listData = TableFilBll.GetAllCarColor();
		foreach (var data in listData)
		{
			var color = new FindDto();
			color.Code = data.Code;
			color.Description = data.ShortDesc;
			listTableitems.Add(color);
		}
		return listTableitems;
	}
	private List<FindDto> RefreshCategory()
	{
		var listTableitems = new List<FindDto>();

		var listData = TableFilBll.GetAllCarCategory();
		foreach (var data in listData)
		{
			var category = new FindDto();
			category.Code = data.Carcategory;
			category.Description = data.ShortDesc;
			listTableitems.Add(category);
		}
		return listTableitems;
	}
	private List<FindDto> RefreshCarType()
	{
		var listTableitems = new List<FindDto>();
		var listCarType = TableFilBll.GetAllCarType().Where(c => c.CarcategoryCode == _stringCarCategory).OrderBy(c => c.ShortDescCode);
		foreach (var carTypeDto in listCarType)
		{
			var find = new FindDto();
			find.Code = carTypeDto.Code;
			find.Description = carTypeDto.LongDescCode;
			listTableitems.Add(find);
		}

		return listTableitems;

	}
	private List<FindDto> RefreshStreet()
	{
		var listTableitems = new List<FindDto>();
		var listStreet = StreetBll.GetStreetByZone(_stringZone, _stringMukim).OrderBy(c => c.ShortDesc);
		foreach (var streetDto in listStreet)
		{
			var find = new FindDto();
			find.Code = streetDto.Code;
			find.Description = streetDto.LongDesc;
			listTableitems.Add(find);
		}
		return listTableitems;
	}
	private List<FindDto> RefreshEnforcer()
	{
		var listTableitems = new List<FindDto>();
		var listEnforcer = EnforcerBll.GetAllEnforcer().OrderBy(c => c.EnforcerName);
		foreach (var enforcerDto in listEnforcer)
		{
			var find = new FindDto();
			find.Code = enforcerDto.EnforcerId;
			find.Description = enforcerDto.EnforcerName;
			listTableitems.Add(find);
		}
		return listTableitems;
	}
	private List<FindDto> RefreshKesalahan()
	{
		var listTableitems = new List<FindDto>();
		var listCompDesc = GeneralBll.GetCompDescByActAndOfdCode(_stringActCode, _stringOfdCode);
		foreach (var compDescDto in listCompDesc)
		{
			var find = new FindDto();
			find.Code = compDescDto.ButirCode;
			find.Description = compDescDto.ButirDesc;
			listTableitems.Add(find);
		}
		return listTableitems;
	}
	private List<FindDto> RefreshDelivery()
	{
		var listTableitems = new List<FindDto>();
		var listDelivery = TableFilBll.GetAllDelivery();

		foreach (var deliveryDto in listDelivery)
		{
			var find = new FindDto();
			find.Code = deliveryDto.Code;
			find.Description = deliveryDto.ShortDesc;
			listTableitems.Add(find);
		}
		return listTableitems;
	}

	private async void newMenu_Clicked(object sender, EventArgs e)
	{
		_iActive = 1;
		await Navigation.PushAsync(new EnterTextPage(HandleAfterBackPage, _sFindType, _sActivityName));
	}

	private async void replaceMenu_Clicked(object sender, EventArgs e)
	{
		_iActive = 1;
		await Navigation.PushAsync(new EnterTextPage(HandleAfterBackPage, _stringMukim, _stringZone));
	}

	private void MySearchBar_TextChanged(object sender, TextChangedEventArgs e)
	{
		string searchText = e.NewTextValue?.ToLower() ?? string.Empty;
		listView.ItemsSource = string.IsNullOrWhiteSpace(searchText)
			? (_listTableitems?.Select(m => $"{m.Code} {m.Description}")?.ToList() ?? [])
			: _listTableitems?.Where(item => $"{item.Code}{item.Description}".ToLower().Contains(searchText)).Select(m => $"{m.Code} {m.Description}")?.ToList() ?? [];
	}

	private async void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
	{
		try
		{
			GlobalClass.FindResult = true;
			GlobalClass.ReturnCodeFind = _listTableitems[e.SelectedItemIndex].Code + ";" +
										 _listTableitems[e.SelectedItemIndex].Description;

			if (_sFindType == Constants.FindStreet)
				GlobalClass.ReturnCodeFind += ";" + _stringMukim + ";" + _stringZone;

			ClosePage();
			await Navigation.PopAsync();
		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, Constants.Message.OKMessage);
			LogFile.WriteLogFile(typeof(CarianPage).Name, "_listView_ItemClick", ex.Message, Enums.LogType.Error);
		}
	}
}