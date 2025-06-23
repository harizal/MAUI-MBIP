namespace AndroidCompound5.Pages;

public partial class MainMenuPage : ContentPage
{
	public MainMenuPage()
	{
		InitializeComponent();
	}

	private async void IssueKompaun_Tapped(object sender, TappedEventArgs e)
	{
		await Navigation.PushAsync(new OptionPage());
	}

	private async void KompaunHarian_Tapped(object sender, TappedEventArgs e)
	{
		await Navigation.PushAsync(new RingkasanPage());
	}

	private async void LihatKompaun_Tapped(object sender, TappedEventArgs e)
	{
		//await Navigation.PushAsync(new ViewCompaundPage());
	}

	private async void Communication_Tapped(object sender, TappedEventArgs e)
	{
		//await Navigation.PushAsync(new CommunicationPage());
	}

	private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
	{
		var loginpage = new LoginPage();
		var navigationPage = new NavigationPage(loginpage);
		Application.Current.MainPage = navigationPage;
	}
}