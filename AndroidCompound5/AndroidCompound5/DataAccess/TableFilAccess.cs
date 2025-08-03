using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.DataAccess;
using System.Collections.Generic;
using System.IO;


namespace AndroidCompound5
{
    /// <summary>
    /// TableFil = "DH04.FIL"
    /// </summary>
    public class TableFilAccess
    {
        public static List<CarCategoryDto> GetCarCategoryAccess()
        {
            return [.. DbContextProvider.Instance.CarCategories];			
        }

        public static List<CarTypeDto> GetCarTypeAccess()
        {
            return [.. DbContextProvider.Instance.CarTypes];
        }

        public static List<CarColorDto> GetCarColorAccess()
        {
			return [.. DbContextProvider.Instance.CarColors];
        }

        public static List<OffendDto> GetOffendAccess()
        {
            return [.. DbContextProvider.Instance.Offends];
        }

        public static List<ZoneDto> GetZoneAccess()
        {
            return [.. DbContextProvider.Instance.Zones];
        }

        public static List<ActDto> GetActAccess()
        {
			return [.. DbContextProvider.Instance.Acts];
        }

        public static List<DeliveryDto> GetDeliveryAccess()
        {
            return [.. DbContextProvider.Instance.Deliveries];
        }    

        public static List<MukimDto> GetMukimAccess()
        {
            return [.. DbContextProvider.Instance.Mukims];
        }

        public static List<TempatJadiDto> GetTempatJadiAccess()
        {
            return [.. DbContextProvider.Instance.TempatJadis];
        }

		[Obsolete("This method is deprecated. Please use NewMethod() instead.")]
		public static List<CarCategoryDto> GetCarCategoryAccess(string strFullFileName)
		{
			var objStream = new StreamReader(strFullFileName);

			var listCarCategory = new List<CarCategoryDto>();

			string sLine = "";
			int len = 0;
			int section = 0;

			while ((sLine = objStream.ReadLine()) != null)
			{
				if (sLine.Length > 0)
				{
					if (sLine.Contains("*****"))
						section++;
					else
					{
						if (section == 0) // CarCategory
						{
							var carCategory = new CarCategoryDto()
							{
								Carcategory = GeneralUtils.ValidateRecordByLine(sLine, 0, 1, ref len).Trim(),
								ShortDesc = GeneralUtils.ValidateRecordByLine(sLine, len, 40, ref len).Trim(),

							};
							listCarCategory.Add(carCategory);
						}
					}
				}
			}

			objStream.Close();
			objStream.Dispose();

			return listCarCategory;
		}
		[Obsolete("This method is deprecated. Please use NewMethod() instead.")]
		public static List<CarTypeDto> GetCarTypeAccess(string strFullFileName)
		{
			var objStream = new StreamReader(strFullFileName);

			var listCarType = new List<CarTypeDto>();

			string sLine = "";
			int len = 0;
			int section = 0;

			while ((sLine = objStream.ReadLine()) != null)
			{
				if (sLine.Length > 0)
				{
					if (sLine.Contains("*****"))
						section++;
					else
					{
						if (section == 1) // CarType
						{
							var carType = new CarTypeDto()
							{
								Code = GeneralUtils.ValidateRecordByLine(sLine, 0, 3, ref len).Trim(),
								CarcategoryCode = GeneralUtils.ValidateRecordByLine(sLine, len, 1, ref len).Trim(),
								LongDescCode = GeneralUtils.ValidateRecordByLine(sLine, len, 40, ref len).Trim(),
								ShortDescCode = GeneralUtils.ValidateRecordByLine(sLine, len, 17, ref len).Trim()
							};
							listCarType.Add(carType);
						}
					}
				}
			}

			objStream.Close();
			objStream.Dispose();

			return listCarType;
		}
		[Obsolete("This method is deprecated. Please use NewMethod() instead.")]
		public static List<CarColorDto> GetCarColorAccess(string strFullFileName)
		{
			var objStream = new StreamReader(strFullFileName);

			var listCarColor = new List<CarColorDto>();

			string sLine = "";
			int len = 0;
			int section = 0;

			while ((sLine = objStream.ReadLine()) != null)
			{
				if (sLine.Length > 0)
				{
					if (sLine.Contains("*****"))
						section++;
					else
					{
						if (section == 2) // CarColor
						{
							var carColor = new CarColorDto()
							{
								Code = GeneralUtils.ValidateRecordByLine(sLine, 0, 2, ref len).Trim(),
								LongDesc = GeneralUtils.ValidateRecordByLine(sLine, len, 40, ref len).Trim(),
								ShortDesc = GeneralUtils.ValidateRecordByLine(sLine, len, 17, ref len).Trim()
							};
							listCarColor.Add(carColor);
						}
					}
				}
			}

			objStream.Close();
			objStream.Dispose();

			return listCarColor;
		}
		[Obsolete("This method is deprecated. Please use NewMethod() instead.")]
		public static List<OffendDto> GetOffendAccess(string strFullFileName)
		{
			var objStream = new StreamReader(strFullFileName);

			var listOffend = new List<OffendDto>();

			string sLine = "";
			int len = 0;
			int section = 0;

			while ((sLine = objStream.ReadLine()) != null)
			{
				if (sLine.Length > 0)
				{
					if (sLine.Contains("*****"))
						section++;
					else
					{
						if (section == 3) // offend
						{
							var offend = new OffendDto()
							{
								ActCode = GeneralUtils.ValidateRecordByLine(sLine, 0, 10, ref len).Trim(),
								OfdCode = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim(),
								IncomeCode = GeneralUtils.ValidateRecordByLine(sLine, len, 8, ref len).Trim(),
								ShortDesc = GeneralUtils.ValidateRecordByLine(sLine, len, 15, ref len).Trim(),
								LongDesc = GeneralUtils.ValidateRecordByLine(sLine, len, 1000, ref len).Trim(),
								OffendAmt = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim(),
								OffendAmt2 = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim(),
								OffendAmt3 = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim(),
								PrnDesc = GeneralUtils.ValidateRecordByLine(sLine, len, 20, ref len).Trim(),
								CompType = GeneralUtils.ValidateRecordByLine(sLine, len, 1, ref len).Trim(),
								NoticeTitle1 = GeneralUtils.ValidateRecordByLine(sLine, len, 43, ref len).Trim(),
								NoticeTitle2 = GeneralUtils.ValidateRecordByLine(sLine, len, 43, ref len).Trim(),
								NoticeDenda = GeneralUtils.ValidateRecordByLine(sLine, len, 650, ref len).Trim(),
								Action = GeneralUtils.ValidateRecordByLine(sLine, len, 350, ref len).Trim(),
								PrintFlag = GeneralUtils.ValidateRecordByLine(sLine, len, 1, ref len).Trim(),
							};
							listOffend.Add(offend);

						}
					}
				}
			}

			objStream.Close();
			objStream.Dispose();

			return listOffend;
		}
		[Obsolete("This method is deprecated. Please use NewMethod() instead.")]
		public static List<ZoneDto> GetZoneAccess(string strFullFileName)
		{
			var objStream = new StreamReader(strFullFileName);

			var listZone = new List<ZoneDto>();

			string sLine = "";
			int len = 0;
			int section = 0;

			while ((sLine = objStream.ReadLine()) != null)
			{
				if (sLine.Length > 0)
				{
					if (sLine.Contains("*****"))
						section++;
					else
					{
						if (section == 4) // zone
						{
							var zone = new ZoneDto
							{
								Code = GeneralUtils.ValidateRecordByLine(sLine, 0, 10, ref len).Trim(),
								Mukim = GeneralUtils.ValidateRecordByLine(sLine, len, 6, ref len).Trim(),
								LongDesc = GeneralUtils.ValidateRecordByLine(sLine, len, 40, ref len).Trim(),
								ShortDesc = GeneralUtils.ValidateRecordByLine(sLine, len, 15, ref len).Trim()
							};
							listZone.Add(zone);
						}
					}
				}
			}

			objStream.Close();
			objStream.Dispose();

			return listZone;
		}
		[Obsolete("This method is deprecated. Please use NewMethod() instead.")]
		public static List<ActDto> GetActAccess(string strFullFileName)
		{
			var objStream = new StreamReader(strFullFileName);

			var listAct = new List<ActDto>();

			string sLine = "";
			int len = 0;
			int section = 0;

			while ((sLine = objStream.ReadLine()) != null)
			{
				if (sLine.Length > 0)
				{
					if (sLine.Contains("*****"))
						section++;
					else
					{
						if (section == 5) // act
						{
							var act = new ActDto()
							{
								Code = GeneralUtils.ValidateRecordByLine(sLine, 0, 10, ref len).Trim(),
								ShortDesc = GeneralUtils.ValidateRecordByLine(sLine, len, 40, ref len).Trim(),
								LongDesc = GeneralUtils.ValidateRecordByLine(sLine, len, 255, ref len).Trim(),
								Title1 = GeneralUtils.ValidateRecordByLine(sLine, len, 70, ref len).Trim(),
								Title2 = GeneralUtils.ValidateRecordByLine(sLine, len, 70, ref len).Trim(),
								Title3 = GeneralUtils.ValidateRecordByLine(sLine, len, 70, ref len).Trim(),
								Title4 = GeneralUtils.ValidateRecordByLine(sLine, len, 70, ref len).Trim()

							};
							listAct.Add(act);
						}
					}
				}
			}

			objStream.Close();
			objStream.Dispose();

			return listAct;
		}
		[Obsolete("This method is deprecated. Please use NewMethod() instead.")]
		public static List<DeliveryDto> GetDeliveryAccess(string strFullFileName)
		{
			var objStream = new StreamReader(strFullFileName);

			var listDelivery = new List<DeliveryDto>();

			string sLine = "";
			int len = 0;
			int section = 0;

			while ((sLine = objStream.ReadLine()) != null)
			{
				if (sLine.Length > 0)
				{
					if (sLine.Contains("*****"))
						section++;
					else
					{
						if (section == 6) // DeliveryDto
						{

							var delivery = new DeliveryDto()
							{
								Code = GeneralUtils.ValidateRecordByLine(sLine, 0, 2, ref len).Trim(),
								ShortDesc = GeneralUtils.ValidateRecordByLine(sLine, len, 50, ref len).Trim()
							};

							listDelivery.Add(delivery);
						}
					}
				}
			}

			objStream.Close();
			objStream.Dispose();

			return listDelivery;
		}
		[Obsolete("This method is deprecated. Please use NewMethod() instead.")]
		public static List<MukimDto> GetMukimAccess(string strFullFileName)
		{
			var objStream = new StreamReader(strFullFileName);

			var listMukim = new List<MukimDto>();

			string sLine = "";
			int len = 0;
			int section = 0;

			while ((sLine = objStream.ReadLine()) != null)
			{
				if (sLine.Length > 0)
				{
					if (sLine.Contains("*****"))
						section++;
					else
					{
						if (section == 8) // Mukim
						{
							var mukim = new MukimDto
							{
								Code = GeneralUtils.ValidateRecordByLine(sLine, 0, 6, ref len).Trim(),
								LongDesc = GeneralUtils.ValidateRecordByLine(sLine, len, 40, ref len).Trim()
							};
							listMukim.Add(mukim);
						}
					}
				}
			}

			objStream.Close();
			objStream.Dispose();

			return listMukim;
		}

		[Obsolete("This method is deprecated. Please use NewMethod() instead.")]
		public static List<TempatJadiDto> GetTempatJadiAccess(string strFullFileName)
		{
			var objStream = new StreamReader(strFullFileName);

			var listTempatJadi = new List<TempatJadiDto>();

			string sLine = "";
			int len = 0;
			int section = 0;

			while ((sLine = objStream.ReadLine()) != null)
			{
				if (sLine.Length > 0)
				{
					if (sLine.Contains("*****"))
						section++;
					else
					{
						if (section == 9) // Tempat jadi
						{
							var tempatJadi = new TempatJadiDto
							{
								Code = GeneralUtils.ValidateRecordByLine(sLine, 0, 4, ref len).Trim(),
								Description = GeneralUtils.ValidateRecordByLine(sLine, len, 100, ref len).Trim()
							};
							listTempatJadi.Add(tempatJadi);
						}
					}
				}
			}

			objStream.Close();
			objStream.Dispose();

			return listTempatJadi;
		}
	}
}