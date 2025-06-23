using AndroidCompound5.AimforceUtils;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;

namespace AndroidCompound5.Pages;

public partial class SignaturePage : ContentPage
{
	public ObservableCollection<IDrawingLine> Lines { get; set; } = new ObservableCollection<IDrawingLine>();
	private string _stringCompoundNumber, _fromForm;
	private bool checkSgnatureFirstLoad;

	public SignaturePage(string compoundnumber, string fromForm)
	{
		InitializeComponent();

		_stringCompoundNumber = compoundnumber;
		_fromForm = fromForm;

		BindingContext = this;
		SetInit();
	}

	private string GetFileName()
	{
		string fileName = GeneralBll.GetSignaturePath();
		fileName += _stringCompoundNumber + Constants.SignName + ".png";
		return fileName;
	}

	private async void SetInit()
	{
		try
		{

			string fileName = GetFileName();

			if (GeneralBll.IsFileExist(fileName, true))
			{
				IsSignatureForm(false);
				using (FileStream stram = new(fileName, FileMode.Open, FileAccess.Read))
				{
					imageSignature.Source = ImageSource.FromStream(() => stram);
				}

				imageSignature.IsVisible = true;
				signature.IsVisible= false;

				checkSgnatureFirstLoad = false;
			}
			else
			{
				IsSignatureForm(true);
				checkSgnatureFirstLoad = true;
			}

		}
		catch (System.Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
			LogFile.WriteLogFile("SignaturePage", "SetInit", ex.Message, Enums.LogType.Error);
		}
	}

	private void IsSignatureForm(bool blValue)
	{
		if (_fromForm != "ViewCompound12")
		{

			if (blValue)
			{
				Lines?.Clear();
				signature.IsVisible = true;
				imageSignature.IsVisible = false;
			}
			else
			{
				signature.IsVisible = false;
				imageSignature.IsVisible = true;
			}

			saveMenu.IsEnabled = blValue;
			deleteMenu.IsEnabled = !blValue;
		}
		else
		{
			saveMenu.IsEnabled = false;
			deleteMenu.IsEnabled = false;
		}
	}

	private async void deleteMenu_Clicked(object sender, EventArgs e)
	{
		Lines?.Clear();
		try
		{
			GeneralBll.DeleteFile(GetFileName());
			IsSignatureForm(true);
		}
		catch (System.Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, "OK");
			LogFile.WriteLogFile("SignaturePage", "onRemove", ex.Message, Enums.LogType.Error);
		}
	}

	private async void saveMenu_Clicked(object sender, EventArgs e)
	{
		if (!Lines.Any())
		{
			await DisplayAlert("INFO", "No signature to save.", "OK");
			return;
		}

		var stream = await DrawingView.GetImageStream(Lines, signature.DesiredSize, Colors.Gray);
		imageSignature.Source = ImageSource.FromStream(() => stream);

		imageSignature.IsVisible = true;
		signature.IsVisible = false;
		Lines?.Clear();
		IsSignatureForm(false);
	}

	protected override bool OnBackButtonPressed()
	{
		if (Lines.Any())
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				var message = await DisplayAlert("INFO", "Pasti untuk keluar?", "Ya", "Tidak");
				if (message)
				{
					await Navigation.PopAsync();
				}
			});
			return true; // Prevent the default back button behavior
		}
		return base.OnBackButtonPressed();
	}
}