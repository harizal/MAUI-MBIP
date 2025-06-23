using Android.App;
using Android.Content;
using Android.OS;
using AndroidCompound5;
using AndroidCompound5.AimforceUtils;

namespace AndroidCompound.Platforms.Android.Services
{
	[Service]
	public class SendCompoundService : Service
	{
		private bool _isRunning;
		int _intervalServices;
		private string _serviceUrl = "";
		private string _serviceUser = "";
		private string _servicePassword = "";

		public override IBinder? OnBind(Intent? intent)
		{
			return null;
		}
		public override void OnDestroy()
		{

			LogFile.WriteLogFile("SendCompoundService OnDestroy", Enums.LogType.Info, Enums.FileLogType.CompoundService);
			_isRunning = false;

			base.OnDestroy();
		}

		private void GetIntervalServices()
		{
			_intervalServices = 5;//default;
			var configDto = GeneralBll.GetConfig();
			if (configDto != null)
			{
				_intervalServices = configDto.SendCompoundInterval;
				_serviceUrl = configDto.ServiceUrl;
				_serviceUser = configDto.ServiceUser;
				_servicePassword = configDto.ServicePassword;

				LogFile.WriteLogFile("_intervalServices : " + _intervalServices, Enums.LogType.Info, Enums.FileLogType.CompoundService);
				LogFile.WriteLogFile("_serviceUrl : " + _serviceUrl, Enums.LogType.Info, Enums.FileLogType.CompoundService);
				LogFile.WriteLogFile("_serviceUser : " + _serviceUser, Enums.LogType.Info, Enums.FileLogType.CompoundService);
				LogFile.WriteLogFile("_servicePassword : " + _servicePassword, Enums.LogType.Info, Enums.FileLogType.CompoundService);
			}
			else
				LogFile.WriteLogFile("Failed Read ConfigApp.", Enums.LogType.Error, Enums.FileLogType.CompoundService);

		}
		public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
		{
			LogFile.WriteLogFile("SendCompoundService OnStartCommand", Enums.LogType.Info, Enums.FileLogType.CompoundService);
			GetIntervalServices();
			_isRunning = true;
			RunAsync();
			return StartCommandResult.NotSticky;
		}

		private void RunAsync()
		{

			Thread t = new Thread(() =>
			{
				while (_isRunning)
				{

					if (_intervalServices == 0)
					{
						GetIntervalServices();
					}
					SendCompoundOnline();
					Thread.Sleep(60000 * _intervalServices);

				}
			});
			t.Start();
		}

		private void SendCompoundOnline()
		{
			try
			{
				if (GeneralBll.IsSendServiceSendCompoundAllow())
				{
					CompoundBll.ProcessCompoundOnlineService();
				}
				LogFile.WriteLogFile("Start ProcessCompoundOnlineService ");
			}
			catch (Exception ex)
			{
				LogFile.WriteLogFile("Error Service SendCompoundOnline : " + ex.Message, Enums.LogType.Error, Enums.FileLogType.CompoundService);
				LogFile.WriteLogFile("Stack Trace : " + ex.StackTrace, Enums.LogType.Error, Enums.FileLogType.CompoundService);
			}
		}
	}
}
