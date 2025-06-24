using Android;

namespace AndroidCompound5.AimforceUtils
{
	public static class Constants
	{
#if PM90
        public const string AppVersion = "Version: 1.0.0.57(PM90)";
#else
		public const string AppVersion = "Version-MAUI: 0.0.0.1";
#endif
		public const int AppVersionNumber = 0001;
		public const string AppName = "Sistem Kompaun Android";
		public const string COMPANYNAME = "Majlis Bandaraya Iskandar Puteri";

		public const string ProgramPath = "/MbipCompound5/";
		public const string DCIMPath = "/DCIM/";
		public const string DevicePrefix = "H";

		public const string SignPath = "FILES/";
		public const string LogPath = "LOGS/";
		public const string TempPath = "TEMP/";

		public const string MasterPath = "MASTER/";
		public const string TransPath = "TRANS/";
		public const string ImgsPath = "IMGS/";
		public const string BackupPath = "BACKUP/";

		public const string SysInit = "SYSINIT.TMP";

		// File definition
		public const string InfoDat = "DH01.DAT";
		public const string TableFil = "DH04.FIL";
		public const string EnforcerFil = "DH05.FIL";
		public const string StreetFil = "DH06.FIL";
		public const string OffRateFil = "DH08.FIL";
		public const string UsedCouponFil = "DH09.FIL";
		public const string HandheldFil = "DH10.FIL";
		public const string PassFil = "DH11.FIL";
		public const string CompDesc = "DH12.FIL";
		public const string ActTitleFil = "DH13.FIL";
		public const string ConfigApp = "ConfigAppmbip.xml";
		public const string LesenFil = "LESEN.FIL";
		public const string MessageFil = "DH15.FIL";


		// TRANSACTION FILES
		public const string CompoundFil = "COMPOUND.FIL";//"DH02.FIL";
		public const string NoteFil = "NOTE.FIL";
		public const string SitaFil = "SITA.FIL";
		public const string Notice = "NOTICE.FIL";
		public const string PrintToFile = "SAMPLE.DAT";
		public const string GpsFil = "GPS.FIL";
		public const string NewCouponFil = "NEWCOUPON.FIL";
		public const string DATETIMELOG = "DATETIME.LOG";
		public const string ENQUIRYLOGFIL = "ENQUIRYLOG.TXT";
		public const string SEMAKPASSFIL = "SEMAKPASS.FIL";
		public const string LASTTRANS = "LASTTRANS.TXT";



		public const string SendOnlinePath = "SENDONLINE/";

		public const string UnexpectedErrorMessage = "Unexpexted Error : Sila hubung pejabat.";
		public const string ConfigNotFound = "Fail Config tak dijumpai. Sila hubung pejabat.";
		public const int DefaultWaitingMilisecond = 100;
		public const int DefaultWaitingConnectionToBluetooth = 3000;

		public const string FindZone = "1";
		public const string FindAct = "2";
		public const string FindOffend = "3";
		public const string FindStreet = "4";
		public const string FindCarColor = "5";
		public const string FindCarType = "6";
		public const string FindDelivery = "7";
		public const string FindEnforcer = "8";
		public const string FindDetail = "9";
		public const string FindNoticeOffend = "10";
		public const string FindNoticeStreet = "11";
		public const string FindCarCategory = "12";
		public const string FindTempatJadi = "13";
		public const string FindPondok = "14";
		public const string FindOffend2 = "15";
		public const string FindMukim = "16";
		public const string FindSita = "17";
		public const string FindAction = "18";
		public const string FindKesalahan = "19";
		public const string FindLot = "20";
		public const string FindTujuan = "21";
		public const string FindPerniagaan = "22";

		public const string CompType1 = "1";
		public const string CompType2 = "2";
		public const string CompType3 = "3";
		public const string CompType4 = "4";
		public const string CompType5 = "5";

		public const int MaxPhoto = 8;
		public const int MinPhoto = 3;
		public const string SignName = "SIGN";
		public const string NewLine = "\r\n";

		public const string RecDeleted = "*";           // Deleted
		public const string RecActive = " ";            // Active

		public const int MaxRetry = 3;
		public const int SleepRetryActiveParking = 2000;//in second = 2 detik
		public const int MaxRetryCompound = 3;
		public const int SleepRetryCompound = 2000;//in second = 2 detik

		public const string COMP_TYPE1 = "1";
		public const string COMP_TYPE2 = "2";
		public const string COMP_TYPE3 = "3";
		public const string COMP_TYPE4 = "4";
		public const string COMP_TYPE5 = "5";

		public const string SeparateData = "**********";
		public const string SepertiDiatas = "SEPERTI DI ATAS";

		public const int MaxPrintRetry = 3;

		public static string SendSuccess = "Success";

		public const string NewZoneCode = "0000";
		public const string NewColorCode = "00";
		public const string NewCarTypeCode = "000";
		public const string NewStreetCode = "0000";

		public const int Success = 1;
		public const int Failed = -1;
		public const int DuplicateCompoundNumber = 2;

		public const string VideoPath = "Video/";
		public const string ErrorMessageInvalidCharacter = "Terdapat character tidak sah. Sila semak input anda.";

		public const int MaxLengthCompound3Address = 80;
		public const int MaxLengthPaper = 35;

		public const string LessDefaultDateMessage = "Tidak dibenarkan masuk tarikh kurang dari tarikh 31/12/2016. Sila hubungi Admin.";
		public const string OutOfRangeDateMessage = "Tidak dibenarkan masuk tarikh melebihi {0} hari dari tarikh akhir transaksi. Sila hubungi Admin.";
		public const string LessDateMessage = "Tidak dibenarkan masuk tarikh kurang dari tarikh akhir transaksi. Sila hubungi Admin.";

		// Font Name
		public const string Roman = "TIMENEWROMAN.TTF";
		public const string WSFB = "WSTTFB.TTF";
		public const string WSTL = "WSTTFL.TTF";
		public const string OPENSANSSERIS = "OPENSANS.TTF";
		public const string OPENSANSLIGHT = "OPENSANS-LIGHT.TTF";
		public const string OPENSANSREGULAR = "OPENSANS-REGULAR.TTF";
		public const string OPENSANSSEMIBOLD = "OPENSANS-SEMIBOLD.TTF";
		public const string OPENSANS = "OPENSANS.TTF";
		public const string OPENSANSBOLD = "OPENSANSBOLD.TTF";
		public const string PRIMESANS = "PRIMESANS.TTF";
		public const string PRIMESANSBOLD = "PRIMESANSBOLD.TTF";
		//public const string PRIMESANS = "RobotoMono-Regular.ttf";
		//public const string PRIMESANSBOLD = "RobotoMono-Bold.ttf";

		public const string PauseCompoundService = "1";
		public const string PrintSignature = "true";

		public static readonly string[] Permissions =
	   {
			Manifest.Permission.AccessCoarseLocation,
			Manifest.Permission.AccessFineLocation,
			Manifest.Permission.WriteExternalStorage,
			Manifest.Permission.ReadExternalStorage,
#if DebugSM29 || ReleaseSM29

#else
            //Manifest.Permission.ManageExternalStorage,
#endif
            Manifest.Permission.Bluetooth,
			Manifest.Permission.BluetoothAdmin,
			Manifest.Permission.Internet,
			Manifest.Permission.Camera,
            //Manifest.Permission.RequestInstallPackages,
            //Manifest.Permission.InstallPackages,
            //Manifest.Permission.DeletePackages,

        };
		public static readonly string[] PermissionsAndroid11 =
		{
			Manifest.Permission.AccessCoarseLocation,
			Manifest.Permission.AccessFineLocation,

			Manifest.Permission.Bluetooth,
			Manifest.Permission.BluetoothAdmin,
			Manifest.Permission.Internet,
			Manifest.Permission.Camera,
		};

		public static readonly string[] PermissionsAndroid12 =
		{
			Manifest.Permission.AccessCoarseLocation,
			Manifest.Permission.AccessFineLocation,
#if DebugSM29 || ReleaseSM29

#else
            //Manifest.Permission.ManageExternalStorage,   // no need to define in permission array
            Manifest.Permission.Bluetooth,
			Manifest.Permission.BluetoothAdmin,
			Manifest.Permission.BluetoothScan,
			Manifest.Permission.BluetoothConnect,
#endif
            Manifest.Permission.Internet,
			Manifest.Permission.Camera,
            //Manifest.Permission.ChangeNetworkState,
        };

		public static class AndroidVersion
		{
			public const int Android10 = 29;
			public const int Android11 = 30;
			public const int Android12 = 31;
		}
		public static class LabelText
		{
			public const string Yes = "Yes";
			public const string No = "No";
			public const string OK = "OK";
			public const string Cancel = "Cancel";
			public const string Cash = "Cash";
			public const string Credit = "Credit";
			public const string Vendor = "Vendor";
			public const string Consignment = "Consignment";
			public const string OutletOff = "Outlet off";
			public const string Issue = "Issue";
			public const string Return = "Return";
			public const string Total = "Total";
			public const string Printing = "Printing";
			public const string ReturnDiscount = "Return Discount";
			public const string Promo = "Pomo";
			public const string Qty = "QTY";
			public const string DecimalZero = "0.00";
			public const string Amt = "Amt";
			public const string PromotionType = "Promotion Type";
			public const string Min = "-";
			public const string MoneySymbol = "$$$";
			public const string GpsSetting = "GPS Setting";
			public const string NotVisit = "NOT VISIT";
			public const string UsbTethering = "USB Tethering";
		}


		public const string Key = "mP8N@2O!9";
		public const string CouncilidKey = "MPKN";
		public const string RefsourceKey = "P";

		public const string FtpUser = "mbsuser";
		public const string FtpPassword = "78742833";
		public const string FtpLocationNewApk = "ftp://1.9.46.170:521/";

		public static class PrinterMessage
		{
			public const int MESSAGE_READ = 3;
		}
		public static class Messages
		{
			public const string SavingData = "Sedang proses simpan data... Sila tunggu";
			public const string SuccessSave = "Data berjaya disimpan";
			public const string ReplaceImageQuestion = "Ganti gambar?";
			public const string SuccessSavePasukan = "Ahli pasukan baru berjaya dimasukkan";
			public const string KompaunIzinApproved = "<b>Keputusan izin Kompaun</b> : DIBENARKAN.<br/><br/><b>Catatan TPR</b> : {0}";
			public const string KompaunIzinDenied = "<b>Keputusan izin Kompaun</b> : TIDAK DIBENARKAN.<br/><br/><b>Catatan TPR</b> : {0}";

			public const string KompaunIzinWaiting =
					"Permohonan izin kompaun telah dihantar. Sila tunggu dan klik semula butang TINDAKAN untuk mendapatkan keputusan";

			public const string InsertData = "Sedang muat turun maklumat";
			public const string SendDataOnline = "Hantar Data";
			public const string DialogRePrint = "Cetak Semula ?";
			public const string SendData = "Hantar Data";
			public const string ReSendData = "Gagal hantar data. Hantar semula?";
			public const string Yes = "Ya";
			public const string No = "Tidak";

			public const string HaveNewVersion = "Terdapat versi baru Aplikasi IEMS";

			public const string BackNotSave = "Data belum disimpan , kembali ?";
			public const string WaitingPlease = "Sila tunggu...";

			public const string Downloading = " Muat turun...";

			public const string SuccessPrint = " Cetakan berjaya";
			public const string TurnOnGpsMessage = "Sila ON tetapan GPS";
			public const string GPSSetting = "Tetapan GPS";
			public const string PrintWaitMessage = "Sedang cetak... Sila tunggu";
			public const string PrintPrepareBMPMessage = "Sedang sedia printout ... Sila tunggu";
			public const string SelectYourItem = "Sila buat pilihan";
			public const string SuccessSendData = "Data berjaya dihantar";
			public const string FinishLawatan = "Anda pasti untuk tamatkan lawatan ini?";

			public const string SkipMessage = "Tiada Keputusan Izin, Masa Menunggu Telah Lebih {0} Minit. Skip Permohonan Izin KOTS";
			public const string PermissionDenied = "Permission Denied ! Please restart application";
			public const string PermissionMessage = "All Permission are needed to grant to use this application.";
			public const string QuestionStopPrinting = "Stop printing ?";
			public const string HapusData = "Hapus";
			public const string Move = "Move";
		}
		public const string FWCODE = "1202T1";
		public const int MAXNUM = 13;
		public const int MINNUM = 10;
		public const int LESSNUM = 11;


		public class Message
		{
			public const string InfoMessage = "INFO";
			public const string ErrorMessage = "ERROR";
			public const string OKMessage = "OK";
		}

	}
}
