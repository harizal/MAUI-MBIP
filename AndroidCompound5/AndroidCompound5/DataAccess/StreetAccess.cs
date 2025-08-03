using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.DataAccess;


namespace AndroidCompound5
{
    /// <summary>
    /// StreetFil = "DH06.FIL"
    /// </summary>
    public static class StreetAccess
    {
        public static List<StreetDto> GetStreetAccess()
        {
            return [.. DbContextProvider.Instance.Streets];
        }

		[Obsolete("This method is deprecated. Please use NewMethod() instead.")]
		public static List<StreetDto> GetStreetAccess(string strFullFileName)
		{
			var objStream = new StreamReader(strFullFileName);

			var listStreet = new List<StreetDto>();

			string sLine = "";
			int len = 0;
			while ((sLine = objStream.ReadLine()) != null)
			{
				if (sLine.Length > 0)
				{
					var street = new StreetDto();

					street.Code = GeneralUtils.ValidateRecordByLine(sLine, 0, 10, ref len);
					street.Zone = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len);
					street.LongDesc = GeneralUtils.ValidateRecordByLine(sLine, len, 40, ref len);
					street.ShortDesc = GeneralUtils.ValidateRecordByLine(sLine, len, 15, ref len);
					street.Mukim = GeneralUtils.ValidateRecordByLine(sLine, len, 6, ref len);

					listStreet.Add(street);
				}
			}

			objStream.Close();
			objStream.Dispose();

			return listStreet;
		}
	}
}