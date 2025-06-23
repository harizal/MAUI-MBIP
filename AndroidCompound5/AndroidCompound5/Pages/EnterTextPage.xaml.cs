using AndroidCompound5.AimforceUtils;
using AndroidCompound5.Classes;
using System.Xml.Linq;

namespace AndroidCompound5.Pages;

public partial class EnterTextPage : ContentPage
{
	private readonly Action _onDataReturned;
	private readonly string _sFindType;
	public EnterTextPage(Action onDataReturned, string type, string activityName)
	{
		InitializeComponent();

		_sFindType = type;
		_onDataReturned = onDataReturned;
	}

	private bool Validate()
	{
		txtText.Text = txtText.Text.TrimStart();
		txtText.Text = txtText.Text.Replace(Constants.NewLine, "");
		txtText.Text = txtText.Text.Replace("\n", " ");

		if (!GeneralBll.IsAlphaNumericAndDotCommaSpaceMinusSlashBraket(txtText.Text))
		{
			DisplayAlert("ERROR", "Terdapat character tidak sah. Sila semak input anda.", "OK");
			return false;
		}

		return true;
	}

	private async void saveMenu_Clicked(object sender, EventArgs e)
	{
		if (Validate())
		{
			var stringText = txtText.Text;
			switch (_sFindType)
			{
				case Constants.FindZone:
					GlobalClass.ReturnCodeFind = Constants.NewZoneCode + ";" + stringText;
					GlobalClass.FindResult = true;
					break;
				case Constants.FindCarColor:
					GlobalClass.ReturnCodeFind = Constants.NewColorCode + ";" + stringText;
					GlobalClass.FindResult = true;
					break;
				case Constants.FindCarType:
					GlobalClass.ReturnCodeFind = Constants.NewCarTypeCode + ";" + stringText;
					GlobalClass.FindResult = true;
					break;
				case Constants.FindStreet:
					GlobalClass.ReturnCodeFind = Constants.NewStreetCode + ";" + stringText;
					GlobalClass.FindResult = true;
					break;
			}
			await Navigation.PopAsync();
		}
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
}