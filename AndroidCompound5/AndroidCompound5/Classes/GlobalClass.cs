using Android.Bluetooth;
using AndroidCompound5.BusinessObject.DTOs;
using Java.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidCompound5.Classes
{
	public class GlobalClass : Application
	{

		public static string ReturnCodeFind { get; set; }
		public static bool FindResult { get; set; }

		public static List<string> FileImages = new List<string>();

		public static BluetoothAndroid BluetoothAndroid { get; set; }
		public static BluetoothSocket BluetoothSocket { get; set; }
		public static BluetoothDevice BluetoothDevice { get; set; }

		public const string TRANSINIT = "TRANSINIT.TMP";
		public const string LASTTRANS = "LASTTRANS.txt";

		public static string Longitude { get; set; }
		public static string Latitude { get; set; }

		public static CompoundDto SetGpsData(CompoundDto compoundDto)
		{

			if (Longitude != "-" && Latitude != "-")
			{
				compoundDto.GpsX = Latitude;
				compoundDto.GpsY = Longitude;
			}
			return compoundDto;
		}
		public static string FwCode { get; set; }
		public static BluetoothPrintService printService { get; set; }

	}
}
