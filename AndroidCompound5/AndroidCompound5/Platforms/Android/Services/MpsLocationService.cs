using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using AndroidCompound5.AimforceUtils;
using AndroidCompound5.Classes;
using Location = Android.Locations.Location;

namespace AndroidCompound.Platforms.Android.Services
{
	[Service]
	public class MpsLocationService : Service
	{
		LocationManager _locationManager;
		string _locationProvider;
		private const int LocationInterval = 1000;
		private const float LocationDistance = 1;
		private LocationListener _mLocationListeners;

		public override IBinder OnBind(Intent intent) { return null; }

		public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
		{
			LogFile.WriteLogFile("Location Service OnStartCommand", Enums.FileLogType.GpsService);

			InitializeLocationManager();

			_mLocationListeners = new LocationListener(_locationProvider);

			_locationManager.RequestLocationUpdates(_locationProvider, LocationInterval, LocationDistance,
			_mLocationListeners);

			return StartCommandResult.NotSticky;

		}

		public override void OnDestroy()
		{
			LogFile.WriteLogFile("Location Service OnDestroy", Enums.FileLogType.GpsService);

			base.OnDestroy();

			if (_locationManager != null)
				_locationManager.RemoveUpdates(_mLocationListeners);
		}

		private void InitializeLocationManager()
		{
			if (_locationManager == null)
				_locationManager = (LocationManager)GetSystemService(LocationService);

			var criteriaForLocationService = new Criteria
			{
				Accuracy = Accuracy.Fine
			};

			_locationProvider = _locationManager.GetBestProvider(criteriaForLocationService, true);
		}

		//public override IBinder? OnBind(Intent? intent)
		//{
		//	throw new NotImplementedException();
		//}

		private class LocationListener : Java.Lang.Object, ILocationListener
		{
			Location _currentLocation;


			public LocationListener(string provider)
			{
				_currentLocation = new Location(provider);
			}

			public void OnLocationChanged(Location location)
			{
				_currentLocation = location;
				if (_currentLocation != null)
				{
					GlobalClass.Longitude = _currentLocation.Longitude.ToString("f6");
					GlobalClass.Latitude = _currentLocation.Latitude.ToString("f6");
				}
			}

			public void OnProviderDisabled(string provider)
			{

			}

			public void OnProviderEnabled(string provider)
			{

			}

			public void OnStatusChanged(string provider, Availability status, Bundle extras)
			{

			}
		}

	}
}