using AndroidCompound5.BusinessObject.DTOs.Responses;
using Newtonsoft.Json;

namespace AndroidCompound5.Pages;

public partial class CompaundDetailPage : ContentPage
{
	public CompaundDetailPage(string compaundDetail)
	{
		InitializeComponent();
		var compaundDetails = JsonConvert.DeserializeObject<List<CompoundDetailDto>>(compaundDetail);
		listView.ItemsSource = compaundDetails;
	}
}