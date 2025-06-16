using System.ComponentModel;

namespace AndroidCompound5.AimforceUtils
{
	public class Enums
	{

		public enum LogType
		{
			Error = 1,
			Info = 2,
			Debug = 3
		}

		public enum FileLogType
		{
			Apps = 1,
			CompoundService = 2,
			GpsService = 3
		}

		public enum FormName
		{
			Communication,
			Login,
			OptionLocation,
			CompoundBll,
			NoteBll,
			InfoBll
		}

		public enum ActiveForm
		{
			None = 0,
			FindCategory = 1,
			FindModel = 2,
			FindStreet = 3,
			FindDelivery = 4,
			FormAkta = 5,
			FormNext = 6,
			FindEnforcer = 7,
			FindKesalahan = 8,
			FormNote = 9,
			FindOffend2 = 10,
			FindLot = 11,
			FindTempatJadi = 12,
			FindTujuan = 13,
			FindPerniagaan = 14,
			FindColor = 15
		}

		public enum ParkingStatus
		{
			[Description("Ada Parking Session !!")] //A
			Used = 1,
			[Description("Available")] //E
			Available = 2,
			[Description("Tak dapat hubungi server dapat status parking !!")]// ' '
			Error = 3,
			[Description("No Internet Connection")]// ' '
			ErrorConnection = 4,
			[Description("Semak Petak")]// ' '
			SemakPetak = 5,
		}


		public enum BarcodeSymbology : byte
		{
			Code39 = 0x04,
			CodeITF = 0x05,
			Code93 = 0x48,
			Code128 = 0x49,
		}

		public enum PrintStatus
		{
			BluetoothEnabled,
			Connected,
			InProgress1,
			InProgress2,
			InProgress3,
			WaitingClose,
			Completed,
			Error
		}

		public enum PrintType
		{
			CompoundType1 = 1,
			CompoundType2 = 2,
			CompoundType3 = 3,
			CompoundType5 = 5,
			TestPrint = 6
		}
	}
}
