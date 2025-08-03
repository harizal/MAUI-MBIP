using Android.Health.Connect.DataTypes;
using AndroidCompound5.AimforceUtils;
using AndroidCompound5.Classes;
using AndroidCompound5.DataAccess;
using static Android.Media.MediaDrm;

namespace AndroidCompound5.Pages;

public partial class SplashScreenPage : ContentPage
{
	public SplashScreenPage()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		Dispatcher.Dispatch(async () =>
		{
			await InitializationApplication();
		});
	}

	private void InitConfig()
	{
		var configDto = GeneralBll.GetConfig();
		if (configDto != null)
		{
			LogFile.WriteLogFile("Config GpsLog : " + configDto.GpsLog, Enums.LogType.Info);
			LogFile.WriteLogFile("Config GpsInterval : " + configDto.GpsInterval, Enums.LogType.Info);
			LogFile.WriteLogFile("Config SendCompoundInterval : " + configDto.SendCompoundInterval, Enums.LogType.Info);
		}

		if (GeneralBll.IsFileExist(Constants.SysInit, false))
		{
			LogFile.WriteLogFile("Found sysinit", Enums.LogType.Info);

			GeneralBll.InitFileTrans();

			//delete init file
			GeneralBll.DeleteFile(Constants.SysInit, false);

			var infoDto = InfoBll.GetInfo();
			//reset counter
			infoDto.CompCnt = 0;
			infoDto.NoteSize = 0;
			infoDto.NoteCnt = 0;
			infoDto.PhotoCnt = 0;
			InfoBll.UpdateInfo(infoDto, Enums.FormName.Login);
		}
	}

	private List<string> CheckFolderAndMasterFiles()
	{
		GeneralBll.InitFolder();
		LogFile.WriteLogFile(Constants.AppVersion, Enums.LogType.Info);

		GeneralBll.InitAssetFiles();
		return GeneralBll.CheckMasterFile();
	}

	public async Task InitializationApplication()
	{
		var permissionResult = await CheckPermissions();
		var message = string.Empty;

		if (!permissionResult)
		{
			message = "Not all permissions were accepted. Please close the Application.";
			await DisplayAlert("Warning", message, "OK");
			return;
		}

		var folderResult = CheckFolderAndMasterFiles();
		if (folderResult.Count > 0)
		{
			message = folderResult.Aggregate("", (cur, str) => cur + str);
			await DisplayAlert("Error", message, "OK");
			return;
		}

		//Initialize database
		string databasePath = GeneralBll.GetDatabasePath();
		if (!File.Exists(databasePath))
		{
			message = "Database missing. Please close the Application.";
			await DisplayAlert("Warning", message, "OK");
			return;
		}
		DbContextProvider.Init(databasePath);
		try
		{
			GeneralBll.InitDatabase();
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", ex.Message, "OK");
		}		

		//var wifiReesult = await SetWifi();
		//if (!wifiReesult)
		//{
		//	await DisplayAlert("Warning", "Wi-Fi was not enabled. Please enable Wi-Fi to continue.", "OK");
		//}

		InitConfig();

		

		

		var loginpage = new LoginPage();
		var navigationPage = new NavigationPage(loginpage);
		Application.Current.MainPage = navigationPage;
		Application.Current.UserAppTheme = AppTheme.Light;
	}

	private async Task<PermissionStatus> CheckPermissions<TPermission>() where TPermission : Permissions.BasePermission, new()
	{
		PermissionStatus status = await Permissions.CheckStatusAsync<TPermission>();

		if (status != PermissionStatus.Granted)
		{
			status = await Permissions.RequestAsync<TPermission>();
		}

		return status;
	}

	private async Task<PermissionStatus> CheckBluetoothPermissions()
	{
		PermissionStatus bluetoothStatus = PermissionStatus.Granted;
		if (DeviceInfo.Platform == DevicePlatform.Android)
		{
			if (DeviceInfo.Version.Major >= 12)
				bluetoothStatus = await CheckPermissions<BluetoothPermissions>();
			else
				bluetoothStatus = await CheckPermissions<Permissions.LocationWhenInUse>();

			return bluetoothStatus;
		}

		return bluetoothStatus;
	}

	private bool IsGranted(PermissionStatus status)
	{
		return status == PermissionStatus.Granted || status == PermissionStatus.Limited;
	}

	private async Task<bool> CheckPermissions()
	{
		PermissionStatus bluetoothStatus = await CheckBluetoothPermissions();
		if (!IsGranted(bluetoothStatus)) return false;

		PermissionStatus camera = await CheckPermissions<Permissions.Camera>();
		if (!IsGranted(camera)) return false;

		if (DeviceInfo.Platform == DevicePlatform.Android)
		{
			if (OperatingSystem.IsAndroidVersionAtLeast(30))
			{
				// Android 11+ requires MANAGE_EXTERNAL_STORAGE permission
				if (!Android.OS.Environment.IsExternalStorageManager)
				{
					try
					{
						var context = Android.App.Application.Context;
						var uri = Android.Net.Uri.Parse("package:" + context.PackageName);
						var intent = new Android.Content.Intent(
							Android.Provider.Settings.ActionManageAppAllFilesAccessPermission,
							uri);

						// FIX: Add FLAG_ACTIVITY_NEW_TASK to avoid RuntimeException
						intent.AddFlags(Android.Content.ActivityFlags.NewTask);

						context.StartActivity(intent);

						await Task.Delay(5000); // Allow time for user to grant permission
					}
					catch (Exception ex)
					{
						Console.WriteLine("Error opening settings for MANAGE_EXTERNAL_STORAGE: " + ex.Message);
						return false;
					}

					if (!Android.OS.Environment.IsExternalStorageManager)
						return false; // Permission still not granted
				}
			}
			else
			{
				// Android 10 and below: request normal storage permissions
				PermissionStatus storageReadStatus = await CheckPermissions<Permissions.StorageRead>();
				if (!IsGranted(storageReadStatus)) return false;

				PermissionStatus storageWriteStatus = await CheckPermissions<Permissions.StorageWrite>();
				if (!IsGranted(storageWriteStatus)) return false;
			}
		}

		return true;
	}
}