using Android;

namespace AndroidCompound5.AimforceUtils
{
	public static class AndroidConstants
	{
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
	}
}
