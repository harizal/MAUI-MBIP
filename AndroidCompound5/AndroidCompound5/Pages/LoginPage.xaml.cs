using Android.Content;
using AndroidCompound5.AimforceUtils;
using System.Net;
using System.Threading.Tasks;

namespace AndroidCompound5.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{		
		InitializeComponent();
		NavigationPage.SetHasNavigationBar(this, false);

		Clear();
#if DEBUG
		txtUser.Text = "2143";
		txtPasswd.Text = "1093";
#endif
		lblVersi.Text = Constants.AppVersion;
	}

	private void Clear()
	{
		lblNamaPengguna.Text = string.Empty;
		txtUser.Text = string.Empty;
		txtPasswd.Text = string.Empty;
	}

	private string GetFile()
	{
		string strfile = "";
		try
		{
			var configDto = GeneralBll.GetConfig();
			if (configDto == null)
				return string.Empty;

			LogFile.WriteLogFile("APKFtpURL : " + configDto.ApkFtpUrl, Enums.LogType.Info);

			FtpWebRequest request = (FtpWebRequest)WebRequest.Create(configDto.ApkFtpUrl);
			request.Method = WebRequestMethods.Ftp.ListDirectory;

			request.Credentials = new NetworkCredential(Constants.FtpUser, Constants.FtpPassword);
			FtpWebResponse response = (FtpWebResponse)request.GetResponse();
			Stream responseStream = response.GetResponseStream();
			StreamReader reader = new StreamReader(responseStream);
			string fileNames = reader.ReadToEnd();
			reader.Close();
			response.Close();

			if (!string.IsNullOrEmpty(fileNames))
			{
				foreach (var fileName in fileNames.Split("\r\n"))
				{
					strfile = fileName.Substring(0, 3);
					if (strfile.ToUpper() == "MBIP")
					{
						if (GeneralBll.GetVersionIntFromString(fileName) > Constants.AppVersionNumber)
							return fileName;
					}
				}
			}
		}
		catch (Exception ex)
		{
			LogFile.WriteLogFile($"ERROR: {ex.Message} {ex.StackTrace}");
		}
		return string.Empty;
	}

	private async void btnLogin_Clicked(object sender, EventArgs e)
	{
		try
		{
			if (txtUser.Text == "RSET")
			{
				if (txtPasswd.Text == "ZOYO")
				{
					InfoBll.ReInitializeCounter();
					Clear();
					await DisplayAlert(Constants.Message.InfoMessage, "Reset counter completed", Constants.Message.OKMessage);
				}
				else
					await DisplayAlert(Constants.Message.InfoMessage, "Katalaluan tidak sah", Constants.Message.OKMessage);

				//txtUser.Focus();
				return;
			}
			else if (txtUser.Text == "ZZZZ")
			{
				if (txtPasswd.Text == "2363")
				{
					Application.Current?.Quit();
				}
				else
					await DisplayAlert(Constants.Message.InfoMessage, "Katalaluan tidak sah", Constants.Message.OKMessage);
			}
			else
			{
				var infoDto = InfoBll.GetInfo();
				if (infoDto == null)
				{
					await DisplayAlert(Constants.Message.InfoMessage, "Fail control tak dijumpai. Sila hubung pejabat.", Constants.Message.OKMessage);
					return;
				}

				if (txtUser.Text.Length == 0)
				{
					await DisplayAlert(Constants.Message.InfoMessage, "Penguatkuasa belum diisi", Constants.Message.OKMessage);
					txtUser.Focus();
				}
				else
				{
					if (EnforcerBll.IsValidPassword(txtUser.Text, txtPasswd.Text))
					{
						var loginpage = new MainMenuPage();
						var navigationPage = new NavigationPage(loginpage);
						Application.Current.MainPage = navigationPage;
					}
					else
						await DisplayAlert(Constants.Message.InfoMessage, "Katalaluan tidak sah", Constants.Message.OKMessage);
				}

			}
		}
		catch (Exception ex)
		{
			await DisplayAlert("ERROR", ex.Message, Constants.Message.OKMessage);
		}
	}

	private async void Ringkasan_Tapped(object sender, TappedEventArgs e)
	{
		var infoDto = InfoBll.GetInfo();
		if (infoDto == null)
		{
			await DisplayAlert(Constants.Message.ErrorMessage, "Fail control tak dijumpai. Sila hubung pejabat.", Constants.Message.OKMessage);
			return;
		}

		await Navigation.PushAsync(new RingkasanPage());

	}

	private void txtUser_Unfocused(object sender, FocusEventArgs e)
	{
		if (txtUser.Text == "AAAA" || txtUser.Text == "ZZZZ")
		{
			lblNamaPengguna.Text = "Administrator";
		}
		else
		{
			var enforcer = EnforcerBll.GetEnforcerById(txtUser.Text);
			lblNamaPengguna.Text = enforcer == null ? "" : enforcer.EnforcerName;
		}
	}

	private async void Logo_Tapped(object sender, TappedEventArgs e)
	{
		var fileName = GetFile();
		if (!string.IsNullOrEmpty(fileName))
		{
			await DisplayAlert(Constants.Message.InfoMessage, "Update File", Constants.Message.OKMessage);
		}
	}
}