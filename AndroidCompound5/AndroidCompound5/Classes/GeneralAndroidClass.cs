using Android.Bluetooth;
using Android.Content.PM;
using Android.Content;
using Android.Net.Wifi;
using Android.OS.Storage;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AndroidCompound5.AimforceUtils.Constants;
using Android.App;
using AndroidCompound5.AimforceUtils;

namespace AndroidCompound5.Classes
{
	public class GeneralAndroidClass
	{
		public static void ShowToast(Context context, string sMessage)
		{
			Toast.MakeText(context, sMessage, ToastLength.Short).Show();
		}

		public static void ShowToast(string sMessage)
		{
			Toast.MakeText(Android.App.Application.Context, sMessage, ToastLength.Long).Show();
		}
		public static void ShowToastLong(Context context, string sMessage)
		{
			Toast.MakeText(context, sMessage, ToastLength.Long).Show();
		}



		public static void OnDialogLogin(string message, Context context, bool isUseDefaultMessage)
		{
			if (isUseDefaultMessage)
				message = "Fail control tak dijumpai. Sila hubung pejabat.";

			AlertDialog.Builder builder = new AlertDialog.Builder(context);
			AlertDialog ad = builder.Create();
			ad.SetTitle(Constants.AppName);
			//ad.SetIcon(Resource.Drawable.iconalert);
			ad.SetMessage(message);
			ad.DismissEvent += (s, e) =>
			{
				//var intent = new Intent(context, typeof(LoginNew));
				//intent.AddFlags(ActivityFlags.ReorderToFront);
				//context.StartActivity(intent);
			};
			// Positive
			ad.SetButton2("OK", (s, e) => { });

			ad.Show();

		}

		public static void OnDialogStayErrorAlphaNumeric(Context context)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder(context);
			AlertDialog ad = builder.Create();
			ad.SetTitle(Constants.AppName);
			ad.SetIcon(Resource.Drawable.iconalert);
			ad.SetMessage("Terdapat character tidak sah. Sila semak input anda.");

			// Positive
			ad.SetButton2("OK", (s, e) => { });
			ad.Show();
		}

		public static void OnDialogStayError(Context context, string message)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder(context);
			AlertDialog ad = builder.Create();
			ad.SetTitle(Constants.AppName);
			ad.SetIcon(Resource.Drawable.iconalert);
			ad.SetMessage(message);

			// Positive
			ad.SetButton2("OK", (s, e) => { });
			ad.Show();
		}

		public static int GetApkVersionNumber(Context context, string fullFileName)
		{
			int versionNumber = 1;

			PackageInfo info = context.PackageManager.GetPackageArchiveInfo(fullFileName, 0);
			if (info != null)
			{
				versionNumber = info.VersionCode;
			}

			return versionNumber;
		}

		public static void RunUpdateApk(Context context, string fullFileName)
		{
			Intent intent = new Intent(Intent.ActionView);
			intent.SetDataAndType(Android.Net.Uri.FromFile(new Java.IO.File(fullFileName)), "application/vnd.android.package-archive");
			intent.SetFlags(ActivityFlags.NewTask);
			context.StartActivity(intent);
		}

		public static void SetWifi(Context context, bool blValue)
		{
			var wifiManager = (WifiManager)context.GetSystemService(Context.WifiService);
			wifiManager.SetWifiEnabled(blValue);
		}

		private void EnableWifi()
		{
			string networkSSID = "bbox-xxx";
			string networkPass = "mypass";

			WifiConfiguration wifiConfig = new WifiConfiguration();
			wifiConfig.Ssid = string.Format("\"{0}\"", networkSSID);
			wifiConfig.PreSharedKey = string.Format("\"{0}\"", networkPass);

			WifiManager wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);

			// Use ID
			int netId = wifiManager.AddNetwork(wifiConfig);
			wifiManager.Disconnect();
			wifiManager.EnableNetwork(netId, true);
			wifiManager.Reconnect();
		}

		public static void GetBackFileImage(string compoundNumber)
		{
			var listFilePath = GeneralBll.SetFhotoBackByCompoundNumber(compoundNumber);

			foreach (var strPath in listFilePath)
			{
				GlobalClass.FileImages.Add(new Java.IO.File(strPath));
			}
		}

		public static List<string> ListPermissions()
		{
			var listPermission = new List<string>();
			var androidVersion = GetBuildVersion();

			if (androidVersion < 23)
			{
				ShowToast("ANDROID BUILD VERSION: " + Build.VERSION.SdkInt.ToString());
				return listPermission;
			}

			if (androidVersion < Constants.AndroidVersion.Android11)
			{
				foreach (var permission in Constants.Permissions)
				{
					////20230222 hsyip : exclude Manifest.Permission.ManageExternalStorage checking below Android OS 11
					//if (!permission.Contains(Manifest.Permission.ManageExternalStorage))
					//{
					var result = ContextCompat.CheckSelfPermission(Android.App.Application.Context, permission);
					if (result != Permission.Granted)
					{
						listPermission.Add(permission);
					}
					//}
				}
			}
			else
			{
				if (androidVersion == Constants.AndroidVersion.Android11) //Android11
				{
					foreach (var permission in Constants.PermissionsAndroid11)
					{
						var result = ContextCompat.CheckSelfPermission(Android.App.Application.Context, permission);
						if (result != Permission.Granted)
						{
							listPermission.Add(permission);
						}
					}
				}
				else//Android12 and above
				{
					foreach (var permission in Constants.PermissionsAndroid12)
					{
						var result = ContextCompat.CheckSelfPermission(Android.App.Application.Context, permission);
						if (result != Permission.Granted)
						{
							listPermission.Add(permission);
						}
					}
				}

			}




			return listPermission;
		}
		public static int GetBuildVersion()
		{
			return (int)Build.VERSION.SdkInt;
		}

		public static string GetExternalStorageDirectory()
		{
			try
			{
				string result = string.Empty;
#if DebugSM29 || ReleaseSM29
                return GeneralAndroidClass.GetExternalStorageDirectory();
#else
				if (GetBuildVersion() >= Constants.AndroidVersion.Android11)
				{
					var storageManager = (StorageManager)Android.App.Application.Context.GetSystemService(Context.StorageService);
					var storageVolumes = storageManager.StorageVolumes;
					var storageVolume = storageVolumes.FirstOrDefault();
					if (storageVolume != null)
					{
						result = storageVolume.Directory.AbsolutePath;

					}

					return result;
				}
				else
				{
					return Android.OS.Environment.ExternalStorageDirectory.AbsolutePath; // GeneralAndroidClass.GetExternalStorageDirectory();
				}
#endif
			}
			catch (Exception ex)
			{

				return String.Empty;
			}

		}
		public static BluetoothAdapter GetBluetoothAdapter()
		{
			if (GetBuildVersion() >= AndroidVersion.Android11)
			{
				var bluetoothManager = (BluetoothManager)Android.App.Application.Context.GetSystemService(Context.BluetoothService);
				if (bluetoothManager != null)
					return bluetoothManager.Adapter;
				return null;

			}
			else
			{
				return BluetoothAdapter.DefaultAdapter;
			}


		}

		public static bool IsPrinterExist()
		{
			GlobalClass.BluetoothAndroid = new BluetoothAndroid();
			var openresult = GlobalClass.BluetoothAndroid.BluetoothOpen();

			if (!openresult.Succes)
			{
				return false;
			}

			var result = GlobalClass.BluetoothAndroid.BluetoothScan();
			return result != 0;
		}
		public static AlertDialog ShowProgressDialog(Context ctx, string message)
		{

			int llPadding = 30;
			LinearLayout ll = new LinearLayout(ctx);
			ll.Orientation = Orientation.Vertical;
			ll.SetPadding(llPadding, llPadding, llPadding, llPadding);
			ll.SetGravity(GravityFlags.Center);
			LinearLayout.LayoutParams llParam = new LinearLayout.LayoutParams(
				LinearLayout.LayoutParams.WrapContent,
				LinearLayout.LayoutParams.WrapContent);
			llParam.Gravity = GravityFlags.Center;
			ll.LayoutParameters = llParam;

			Android.Widget.ProgressBar progressBar = new Android.Widget.ProgressBar(ctx);
			progressBar.Indeterminate = true;
			progressBar.SetPadding(0, 0, llPadding, 10);
			progressBar.LayoutParameters = llParam;

			llParam = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
				ViewGroup.LayoutParams.WrapContent);
			llParam.Gravity = GravityFlags.Center;

			TextView tvText = new TextView(ctx);
			tvText.SetText(message, TextView.BufferType.Normal);
			tvText.SetTextColor(Android.Graphics. Color.ParseColor("#000000"));
			tvText.SetTextSize(ComplexUnitType.Dip, 14);
			tvText.LayoutParameters = llParam;

			ll.AddView(progressBar);
			ll.AddView(tvText);

			AlertDialog.Builder builder = new AlertDialog.Builder(ctx);
			builder.SetCancelable(true);
			builder.SetView(ll);

			var dialog = builder.Create();
			dialog.Show();
			Android.Views.Window window = dialog.Window;
			if (window != null)
			{
				var layoutParams = new WindowManagerLayoutParams();
				layoutParams.CopyFrom(dialog.Window.Attributes);
				layoutParams.Width = LinearLayout.LayoutParams.WrapContent;
				layoutParams.Height = LinearLayout.LayoutParams.WrapContent;
				dialog.Window.Attributes = layoutParams;
			}
			return dialog;
		}

		public static AlertDialog GetDialogCustom(Context context)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder(context, Resource.Style.CustomDialogAlert);
			AlertDialog ad = builder.Create();
			ad.SetTitle(Constants.AppName);
			ad.SetIcon(Resource.Drawable.iconalert);
			ad.SetCanceledOnTouchOutside(false);
			return ad;
		}
	}
}
