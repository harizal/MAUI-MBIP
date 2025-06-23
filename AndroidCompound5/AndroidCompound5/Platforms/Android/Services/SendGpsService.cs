using Android.App;
using Android.Content;
using Android.OS;
using AndroidCompound5;
using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.Classes;

namespace AndroidCompound.Platforms.Android.Services
{
	[Service]
	public class SendGpsService : Service
	{
		private Context _context;
		private bool _isRunning;
		int _intervalServices;
		private string _serviceUrl = "";
		private string _serviceUser = "";
		private string _servicePassword = "";

		public override IBinder OnBind(Intent intent)
		{
			return null;
		}

		private void GetIntervalServices()
		{
			_intervalServices = 5;//default;
			var configDto = GeneralBll.GetConfig();
			if (configDto != null)
			{
				_intervalServices = configDto.GpsInterval;
				_serviceUrl = configDto.ServiceUrl;
				_serviceUser = configDto.ServiceUser;
				_servicePassword = configDto.ServicePassword;

				LogFile.WriteLogFile("_intervalServices Gps : " + _intervalServices, Enums.LogType.Info, Enums.FileLogType.GpsService);
				LogFile.WriteLogFile("_serviceUrl : " + _serviceUrl, Enums.LogType.Info, Enums.FileLogType.GpsService);
				LogFile.WriteLogFile("_serviceUser : " + _serviceUser, Enums.LogType.Info, Enums.FileLogType.GpsService);
				LogFile.WriteLogFile("_servicePassword : " + _servicePassword, Enums.LogType.Info, Enums.FileLogType.GpsService);
			}
			else
				LogFile.WriteLogFile("Failed Read ConfigApp.", Enums.LogType.Error, Enums.FileLogType.GpsService);

		}

		public override void OnDestroy()
		{

			_isRunning = false;
			LogFile.WriteLogFile("SendGps destroy", Enums.LogType.Info, Enums.FileLogType.GpsService);
			base.OnDestroy();
		}

		public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
		{
			LogFile.WriteLogFile("SendGpsService OnStartCommand", Enums.LogType.Info, Enums.FileLogType.GpsService);
			GetIntervalServices();
			_isRunning = true;
			RunAsync();
			return StartCommandResult.NotSticky;

		}


		private void RunAsync()
		{
			string enfId = "";
			string dolphinId = "";

			var infoDto = InfoBll.GetInfo();
			if (infoDto == null)
				return;

			enfId = infoDto.EnforcerId;
			dolphinId = infoDto.DolphinId;

			Thread t = new Thread(() =>
			{
				while (_isRunning)
				{

					if (_intervalServices == 0)
					{
						GetIntervalServices();
					}

					if (string.IsNullOrEmpty(enfId))
					{
						infoDto = InfoBll.GetInfo();
						if (infoDto == null)
							break;

						enfId = infoDto.EnforcerId;
						dolphinId = infoDto.DolphinId;
					}
					SendGpsOnline(enfId, dolphinId);

					Thread.Sleep(60000 * _intervalServices); //1 minute

				}
			});
			t.Start();


		}

		private void SendGpsOnline(string enfId, string dolphinId)
		{

			try
			{
				var gpsDto = new GpsDto();

				if (GlobalClass.Longitude != "-" && GlobalClass.Latitude != "-")
				{
					gpsDto.GpsX = GlobalClass.Latitude;
					gpsDto.GpsY = GlobalClass.Longitude;
				}

				if (string.IsNullOrEmpty(gpsDto.GpsX) && string.IsNullOrEmpty(gpsDto.GpsY))
					return;

				gpsDto.Kodpguatkuasa = enfId;
				gpsDto.DhId = dolphinId;
				gpsDto.BatteryLife = GetBatteryLife();

				GeneralBll.LogGpsData(gpsDto, _serviceUrl, _serviceUser, _servicePassword);

			}
			catch (Exception ex)
			{
				LogFile.WriteLogFile("Error Service SendGpsOnline : " + ex.Message, Enums.LogType.Error, Enums.FileLogType.GpsService);
			}
		}

		private string GetBatteryLife()
		{
			string result = "";
			try
			{
				var filter = new IntentFilter(Intent.ActionBatteryChanged);
				var battery = RegisterReceiver(null, filter);
				int level = battery.GetIntExtra(BatteryManager.ExtraLevel, -1);
				int scale = battery.GetIntExtra(BatteryManager.ExtraScale, -1);

				int levelPercentage = (int)Math.Floor(level * 100D / scale);
				result = levelPercentage.ToString();
			}
			catch (Exception ex)
			{
				LogFile.WriteLogFile("Error GetBatteryLife : " + ex.Message, Enums.LogType.Error, Enums.FileLogType.GpsService);
			}

			return result;

		}

	}
}