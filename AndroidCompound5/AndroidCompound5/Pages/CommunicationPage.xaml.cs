using Android.Content;
using AndroidCompound.Platforms.Android.Services;
using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;

namespace AndroidCompound5.Pages;

public partial class CommunicationPage : ContentPage
{
	private string _ftpUnload;
	private string _ftpDownload;
	private string _ftpControl;
	private string _ftpHost;
	private string _ftpUser;
	private string _ftpPassword;
	string _apkFileName;

	private string _serviceUrl, _serviceazureUrl, _serviceUser, _servicePassword, _photoUrl;
	private List<string> _listMessage = new List<string>();
	private bool IsFinished = false;

	public CommunicationPage()
	{
		InitializeComponent();

		LogFile.WriteLogFile("Communication(Oncreate): Stop Services");

		StopSendCompoundService();
		StopSendGpsService();
		StopLocationService();

		SetConfig();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();

		StartSendGpsService();
		StartLocationService();
	}

	private async void SetConfig()
	{
		var configDto = GeneralBll.GetConfig();
		if (configDto == null)
		{
			await DisplayAlert("ERROR", Constants.ConfigNotFound, "OK");
			return;
		}

		_ftpUnload = configDto.FtpUnload;
		_ftpDownload = configDto.FtpDownload;
		_ftpControl = configDto.FtpControl;
		_ftpHost = configDto.FtpHost;
		_ftpUser = configDto.FtpUser;
		_ftpPassword = configDto.FtpPassword;
		_apkFileName = configDto.ApkFileName;
		_serviceUrl = configDto.ServiceUrl;
		_serviceUser = configDto.ServiceUser;
		_servicePassword = configDto.ServicePassword;
		_photoUrl = configDto.UrlPhoto;
		//_serviceazureUrl = configDto.ServiceAzureUrl;
	}

	private void StopSendCompoundService()
	{
		LogFile.WriteLogFile("Communication(StopSendCompoundService): Stop Services");
#if ANDROID
		var intent = new Intent(Android.App.Application.Context, typeof(SendCompoundService));
		Android.App.Application.Context.StopService(intent);
#endif
	}

	private void StopSendGpsService()
	{
		LogFile.WriteLogFile("Communication(StopSendGpsService): Stop Services");
#if ANDROID
		var intent = new Intent(Android.App.Application.Context, typeof(SendGpsService));
		Android.App.Application.Context.StopService(intent);
#endif
	}
	private void StopLocationService()
	{
		LogFile.WriteLogFile("Communication(StopLocationService): Stop Services");
#if ANDROID
		var intent = new Intent(Android.App.Application.Context, typeof(MpsLocationService));
		Android.App.Application.Context.StopService(intent);
#endif
	}

	private void StartSendGpsService()
	{
		LogFile.WriteLogFile("Communication(SendGpsService): Start Services");
#if ANDROID
		var intent = new Intent(Android.App.Application.Context, typeof(SendGpsService));
		Android.App.Application.Context.StartService(intent);
#endif
	}

	private void StartLocationService()
	{
		LogFile.WriteLogFile("Communication(StartLocationService): Start Services");
#if ANDROID
		var intent = new Intent(Android.App.Application.Context, typeof(MpsLocationService));
		Android.App.Application.Context.StartService(intent);
#endif
	}

	private void SetStartControl(bool blValue)
	{
		NavigationPage.SetHasBackButton(this, blValue);
		btnStart.IsVisible = blValue;
		indicator.IsVisible = !blValue;
	}

	private async void btnStart_Clicked(object sender, EventArgs e)
	{
		var infoDto = InfoBll.GetInfo();
		if (infoDto == null)
		{
			await DisplayAlert("ERROR", Constants.ConfigNotFound, "OK");
			return;
		}

		LogFile.WriteLogFile("Start", Enums.LogType.Info);
		tvMessage.Text = "";
		SetStartControl(false);


		LogFile.WriteLogFile("OnSync Files - Get Server Info", Enums.LogType.Info);


		var host = "ftp://" + _ftpHost;
		var user = _ftpUser;
		var password = _ftpPassword;

		LogFile.WriteLogFile("OnSync Files - Get Server Info - Host : " + host, Enums.LogType.Info);
		LogFile.WriteLogFile("OnSync Files - Get Server Info - user : " + user, Enums.LogType.Info);
		LogFile.WriteLogFile("OnSync Files - Get Server Info - password : " + password, Enums.LogType.Info);

		ThreadPool.QueueUserWorkItem(o => StartSync(infoDto));
	}

	private void UpdateMessage(string message)
	{
		tvMessage.Text += "\r\n" + message;

		progressBar.Progress += 0.02;
		if (IsFinished)
		{

			progressBar.Progress = 1;
		}
		Thread.Sleep(300);
	}

	private void ShowMessage(string message)
	{
		MainThread.BeginInvokeOnMainThread(() => UpdateMessage(message));
	}

	public void AddErrorMessage(string message)
	{
		if (!string.IsNullOrEmpty(message))
			_listMessage.Add(message);

	}

	private bool ManageTransAfterUpload(string dolphinId)
	{
		try
		{
			LogFile.WriteLogFile("Start Move Files TRANS For Backup", Enums.LogType.Info);
			ShowMessage("Start Move Files TRANS For Backup");

			var pathSource = GeneralBll.GetTransPath();

			var destSource = GeneralBll.GetTransBackupPath();

			var listFiles = GeneralBll.GetListTransFiles();



			var strPrefix = dolphinId + GeneralBll.GetLocalDateTime().ToString("yyyyMMdd") + GeneralBll.GetLocalDateTime().ToString("HHmm");

			foreach (var listFile in listFiles)
			{
				if (GeneralBll.IsFileExist(pathSource + listFile, true))
					GeneralBll.MoveFile(pathSource + listFile, destSource + strPrefix + listFile);
			}


			//GetFilesFromBackupFolder();
			//trans backup
			ManageTransFileBackup();
			//image backup
			//ManageImageFolderAfterUpload();
			return true;
		}
		catch (Exception ex)
		{
			LogFile.WriteLogFile("Error ManageTransAfterUpload : " + ex.Message, Enums.LogType.Error);
			AddErrorMessage("Error ManageTransAfterUpload");
			return false;
		}

	}

	private void ManageTransFileBackup()
	{


		const int maxFile = 60;

		ShowMessage("Start ManageTransFileBackup");
		LogFile.WriteLogFile("Start ManageTransFileBackup", Enums.LogType.Info);

		var pathBackup = GeneralBll.GetTransBackupPath();

		var di = new DirectoryInfo(pathBackup);

		var listTransFile = GeneralBll.GetListTransFiles();

		foreach (var transFile in listTransFile)
		{

			FileInfo[] rgFiles = di.GetFiles("*" + transFile);

			if (rgFiles.Count() > maxFile)
			{
				var listFiles = rgFiles.OrderByDescending(c => c.Name).Select(x => x.FullName).ToList();

				var listToDelete = listFiles.Skip(maxFile).Take(listFiles.Count - maxFile).ToList();

				foreach (var fileName in listToDelete)
				{
					GeneralBll.DeleteFile(fileName, true);
				}
			}

		}

		LogFile.WriteLogFile("Done ManageTransFileBackup", Enums.LogType.Info);
		ShowMessage("Done ManageTransFileBackup");

	}

	private void ManageImageBackup(string pathBackupImage)
	{
		//remove all image that more than 3 days
		//var pathBackupImage = GeneralAndroidClass.GetExternalStorageDirectory() + GlobalClass.PROG_PATH +
		//             GlobalClass.TRANSBACKUPPATH + "IMGS/";

		//get all image in folder
		var di = new DirectoryInfo(pathBackupImage);
		FileInfo[] rgFiles = di.GetFiles("*.*");

		foreach (FileInfo fi in rgFiles)
		{

			var fileName = fi.Name;
			var strDate = fileName.Substring(0, 8);//format yyyyMMdd

			var dtDate = GeneralBll.ConvertStringToDate(strDate);
			if (dtDate == null) continue;

			var dateTimeServer = GeneralBll.GetLocalDateTime();
			var dateLocal = new DateTime(dateTimeServer.Year, dateTimeServer.Month, dateTimeServer.Day);
			var difference = dateLocal - dtDate.Value;

			//FileSystemAndroid.WriteLogFile("difference : " + difference.TotalDays);
			//keep for 3 days
			if (difference.TotalDays > 14)
				GeneralBll.DeleteFile(fi.FullName, true);

		}

	}

	private void ManageSignBackup(string pathBackupSign)
	{

		var di = new DirectoryInfo(pathBackupSign);
		FileInfo[] rgFiles = di.GetFiles("*.*");

		foreach (FileInfo fi in rgFiles)
		{

			var fileName = fi.Name;
			var strDate = fileName.Substring(0, 8);//format yyyyMMdd

			var dtDate = GeneralBll.ConvertStringToDate(strDate);
			if (dtDate == null) continue;

			var dateTimeServer = GeneralBll.GetLocalDateTime();
			var dateLocal = new DateTime(dateTimeServer.Year, dateTimeServer.Month, dateTimeServer.Day);
			var difference = dateLocal - dtDate.Value;

			//FileSystemAndroid.WriteLogFile("difference : " + difference.TotalDays);
			//keep for 3 days
			if (difference.TotalDays > 3)
				GeneralBll.DeleteFile(fi.FullName, true);

		}

	}

	private bool ManageSignFolderAfterUpload()
	{
		try
		{
			LogFile.WriteLogFile("Start ManageSignFolderAfterUpload", Enums.LogType.Info);

			var pathSource = GeneralBll.GetSignaturePath();


			var pathDest = GeneralBll.GetSignatureBackupPath();

			var strDate = GeneralBll.GetLocalDateTime().ToString("yyyyMMdd");

			//get all Signature in folder
			var di = new DirectoryInfo(pathSource);
			FileInfo[] rgFiles = di.GetFiles("*.*");

			foreach (FileInfo fi in rgFiles)
			{
				if (GeneralBll.IsFileExist(fi.FullName, true))
					GeneralBll.MoveFile(fi.FullName, pathDest + strDate + fi.Name);
			}

			ManageSignBackup(pathDest);

			LogFile.WriteLogFile("Done ManageImageFolderAfterUpload", Enums.LogType.Info);
			return true;
		}
		catch (Exception ex)
		{
			LogFile.WriteLogFile("Error ManageImageFolderAfterUpload : " + ex.Message, Enums.LogType.Error);
			AddErrorMessage("Error ManageImageFolderAfterUpload");
			return false;
		}

	}

	private bool ManageImageFolderAfterUpload()
	{
		try
		{
			ShowMessage("Start ManageImageFolderAfterUpload");
			LogFile.WriteLogFile("Start ManageImageFolderAfterUpload", Enums.LogType.Info);

			var pathSource = GeneralBll.GetInternalImagePath();


			var pathDest = GeneralBll.GetImageBackupPath();

			var strDate = GeneralBll.GetLocalDateTime().ToString("yyyyMMdd");

			if (Directory.Exists(pathSource))
			{
				//get all image in folder
				var di = new DirectoryInfo(pathSource);
				FileInfo[] rgFiles = di.GetFiles("*.*");

				foreach (FileInfo fi in rgFiles)
				{
					if (GeneralBll.IsFileExist(fi.FullName, true))
						GeneralBll.MoveFile(fi.FullName, pathDest + strDate + fi.Name);
				}
			}

			ManageImageBackup(pathDest);
			ManageSignFolderAfterUpload();

			LogFile.WriteLogFile("Done ManageImageFolderAfterUpload", Enums.LogType.Info);
			ShowMessage("Done ManageImageFolderAfterUpload");
			return true;
		}
		catch (Exception ex)
		{
			LogFile.WriteLogFile("Error ManageImageFolderAfterUpload : " + ex.Message, Enums.LogType.Error);
			AddErrorMessage("Error ManageImageFolderAfterUpload");
			return false;
		}

	}

	private void StartSync(InfoDto infoDto)
	{
		_listMessage = new List<string>();
		try
		{
			LogFile.WriteLogFile("StartSync", Enums.LogType.Info);

			IsFinished = false;
			progressBar.Progress = 0;

			//isFileUnload = false;
			//prefix = infoDto.DolphinId + GeneralBll.GetLocalDateTime().ToString("yyyyMMdd") +
			//         GeneralBll.GetLocalDateTime().ToString("HHmm");
			//ShowMessage("Start FTP connection ...");
			//ShowMessage(string.Format("Host : {0}", _ftpHost));
			//ShowMessage(string.Format("User : {0}", _ftpUser));
			//ShowMessage("Connection ...");
			//var ftp = new Ftp(_ftpHost, _ftpUser, _ftpPassword);
			//                FtpTransFiles(ftp, prefix);

			ShowMessage("Start Transfer Unsend Compound Data");

			GeneralBll.ResumeServiceSendCompound();
			LogFile.WriteLogFile("StartSync - Start Transfer Unsend Compound Data", Enums.LogType.Info);
			CompoundBll.ProcessCompoundOnlineService();
			ShowMessage("Finish Transfer Unsend Compound Data");
			LogFile.WriteLogFile("StartSync - Finish Transfer Unsend Compound Data", Enums.LogType.Info);

			LogFile.WriteLogFile("StartSync - Start Transfer Unsend Images", Enums.LogType.Info);
			ShowMessage("Start Transfer Unsend Image files");
			CompoundBll.SendAllUnsendImage(_photoUrl);
			ShowMessage("Finish Transfer Unsend Image files");
			LogFile.WriteLogFile("StartSync - Finish Transfer Unsend Images", Enums.LogType.Info);


			if (_listMessage.Count == 0)
			{
				ShowMessage("Start Transfer Unsend GPS Data");
				LogFile.WriteLogFile("StartSync - Start Transfer unsend GPS data", Enums.LogType.Info);
				CompoundBll.ProcessGPSOnlineService(_serviceUrl, _serviceUser, _servicePassword);
				LogFile.WriteLogFile("StartSync - Finish Transfer unsend GPS data", Enums.LogType.Info);
				ShowMessage("Finish Transfer Unsend GPS Data");
			}

			if (_listMessage.Count == 0)
			{
				ShowMessage("Start Transfer EnquiryLog Data");
				LogFile.WriteLogFile("StartSync - Start Transfer EnquiryLog data", Enums.LogType.Info);
				CompoundBll.ProcessEnquiryLogOnlineService(_serviceUrl, _serviceUser, _servicePassword);
				LogFile.WriteLogFile("StartSync - Finish Transfer EnquiryLog data", Enums.LogType.Info);
				ShowMessage("Finish Transfer Unsend GPS Data");
			}

			var listFileCompoundOnline = CompoundBll.ListFileCompoundOnline();
			if (listFileCompoundOnline.Count > 0)
			{
				AddErrorMessage("still have unsend compound data : " + listFileCompoundOnline.Count.ToString());
				ShowMessage("still have unsend compound data : " + listFileCompoundOnline.Count.ToString());
			}
			else
			{
				if (_listMessage.Count == 0)
				{
					if (ManageTransAfterUpload(infoDto.DolphinId))
					{
						if (ManageImageFolderAfterUpload())
						{
							//remove file compound online 

							//                            CompoundBll.RemoveAllFilesCompoundOnline();

							ShowMessage("Start InitTrans");
							GeneralBll.InitFileTrans();
							ShowMessage("Done InitTrans");

							//infoDto.CurrComp = 0;
							//infoDto.CurrSita = 90000000;
							infoDto.CompCnt = 0;            // Total compound issued
							infoDto.NoteSize = 0;           // Total note count in bytes
							infoDto.NoteCnt = 0;            // Total note 
							infoDto.SitaCnt = 0;            // Total Sitaan issued
							infoDto.NoticeCnt = 0;      // Total Notice issued
							infoDto.PhotoCnt = 0;           // Total Photo captured

							InfoBll.UpdateInfo(infoDto, Enums.FormName.Communication);

							//ShowMessage("Start download files ...");
							IsFinished = true;
							if (_listMessage.Count == 0)
							{
								ShowCompleteDialog();
								MainThread.BeginInvokeOnMainThread(() => SetStartControl(true));
								return;
							}
						}//end if ManageImageFolderAfterUpload
					}
				}
			} //_listMessage.Count == 0
			  //}

			ShowErrorDialog();
		}
		catch (Exception ex)
		{
			ShowMessage("Gagal hantar data, Sila cuba lagi.");
			LogFile.WriteLogFile("StartSync - Error - " + ex.Message, Enums.LogType.Error);
			//RunOnUiThread(() => GeneralAndroidClass.SetWifi(this, false));
		}
		MainThread.BeginInvokeOnMainThread(() => SetStartControl(true));

	}

	private void ShowErrorDialog()
	{
		string message = _listMessage.Aggregate("", (current, str) => current + (str + Constants.NewLine));
		MainThread.BeginInvokeOnMainThread(() => OnDialogStay(message));
	}

	private async void OnDialogStay(string message)
	{
		await DisplayAlert("INFO", message, "OK");
	}

	private void ShowCompleteDialog()
	{
		string message = "Selesai ";
		MainThread.BeginInvokeOnMainThread(() => OnDialogStay(message));
	}
}
