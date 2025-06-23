using AndroidCompound5.Pages;

namespace AndroidCompound5
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
			MainPage = new SplashScreenPage();
			Application.Current.UserAppTheme = AppTheme.Light;
		}
    }
}