using Android.Content.PM;
using Android.Content;
using Android.Net.Wifi;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidCompound5.Classes
{
	public static class AndroidUtils
	{

		public static void ShowToast(Context context, string sMessage)
		{
			Toast.MakeText(context, sMessage, ToastLength.Long).Show();
		}

		public static void ShowToastInConstruction(Context context)
		{
			Toast.MakeText(context, "In Construction", ToastLength.Long).Show();
		}

		public static void SetWifi(Context context, bool blValue)
		{
			var wifiManager = (WifiManager)context.GetSystemService(Context.WifiService);
			wifiManager.SetWifiEnabled(blValue);
		}

		//public static void OnDialogLogin(string message, Context context, bool isUseDefaultMessage)
		//{
		//	if (isUseDefaultMessage)
		//		message = "Fail control tak dijumpai. Sila hubung pejabat.";
		//
		//	AlertDialog.Builder builder = new AlertDialog.Builder(context, Resource.Style.CustomDialogAlert);
		//	AlertDialog ad = builder.Create();
		//	ad.SetTitle(Constants.AppName);
		//	ad.SetIcon(Resource.Drawable.iconalert);
		//	ad.SetMessage(message);
		//	ad.DismissEvent += (s, e) =>
		//	{
		//		var intent = new Intent(context, typeof(LoginNew));
		//		intent.AddFlags(ActivityFlags.ReorderToFront);
		//		context.StartActivity(intent);
		//		((Activity)context).Finish();
		//	};
		//	// Positive
		//	ad.SetButton2("OK", (s, e) => { });
		//
		//	ad.Show();
		//}
		//
		//public static AlertDialog GetDialogCustom(Context context)
		//{
		//	AlertDialog.Builder builder = new AlertDialog.Builder(context, Resource.Style.CustomDialogAlert);
		//	AlertDialog ad = builder.Create();
		//	ad.SetTitle(Constants.AppName);
		//	ad.SetIcon(Resource.Drawable.iconalert);
		//	return ad;
		//}

		//public static void StartActivityLoginAfterPrint(Context context)
		//{
		//    var login = new Intent(context, typeof(LoginAfterPrint));
		//    context.StartActivity(login);
		//}

		//public static void OnDialogStay(Context context, string sMessage)
		//{
		//	var ad = GetDialogCustom(context);
		//	ad.SetMessage(sMessage);
		//	// Positive
		//	ad.SetButton2("OK", (s, e) => { });
		//	ad.Show();
		//}

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
			try
			{

				Java.IO.File file = new Java.IO.File(fullFileName);

				//Uri apkURI = Uri.FromFile(file); //for Build.VERSION.SDK_INT <= 24
				//if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
				//{
				//    apkURI = FileProvider.GetUriForFile(context, context.PackageName + ".provider", file);
				//}
				//apkURI = FileProvider.GetUriForFile(context, context.PackageName + ".provider", file);
				var apkURI = FileProvider.GetUriForFile(context, context.PackageName + ".fileprovider", file);

				Intent intent = new Intent(Intent.ActionView);
				intent.PutExtra(Intent.ExtraNotUnknownSource, true);
				intent.SetDataAndType(apkURI, "application/vnd.android" + ".package-archive");
				intent.SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
				intent.AddFlags(ActivityFlags.GrantReadUriPermission);
				context.StartActivity(intent);

			}
			catch (Exception e)
			{
				Console.WriteLine(e);

			}
		}

		public static string AddDate(string date, int days)
		{
			string target = "";
			int iyear = 0;
			int imonth = 0;
			int idate = 0;
			int daysinmonth = 0;

			iyear = Convert.ToInt32(date.Substring(0, 4));
			imonth = Convert.ToInt32(date.Substring(4, 2));
			idate = Convert.ToInt32(date.Substring(6, 2));

			idate = idate + days;

			switch (imonth)
			{
				case 1:
				case 3:
				case 5:
				case 7:
				case 8:
				case 10:
				case 12:
					daysinmonth = 31;
					break;
				case 4:
				case 6:
				case 9:
				case 11:
					daysinmonth = 30;
					break;
				case 2:
					if (iyear % 4 == 0)
					{
						daysinmonth = 29;
					}
					else
					{
						daysinmonth = 28;
					}
					break;
			}

			if (idate > daysinmonth)
			{
				idate = idate - daysinmonth;
				++imonth;

				if (imonth > 12)
				{
					imonth = imonth - 12;
					++iyear;
				}
			}

			target = idate.ToString("00") + "-" + imonth.ToString("00") + "-" + iyear.ToString();
			return target;
		}



	}
}