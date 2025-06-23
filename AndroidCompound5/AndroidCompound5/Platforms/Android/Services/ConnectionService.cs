using Android.Content;
using Android.Net.Wifi;
using AndroidCompound5.Interfaces;
using AndroidApp = Android.App.Application;

//[assembly: Dependency(typeof(ConnectionService))]
namespace AndroidCompound.Platforms.Android.Services
{    
    public class ConnectionService : IConnectionService
    {
        public void EnableWifi()
        {
            var wifiManager = (WifiManager)AndroidApp.Context.GetSystemService(Context.WifiService);
            if (!wifiManager.IsWifiEnabled)
            {
                wifiManager.SetWifiEnabled(true);
            }
        }
    }
}
