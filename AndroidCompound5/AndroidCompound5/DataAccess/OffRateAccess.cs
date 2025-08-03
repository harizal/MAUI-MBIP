using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.DataAccess;


namespace AndroidCompound5
{
    /// <summary>
    /// Offrate = DH08.FIL
    /// </summary>
    public static class OffrateAccess
    {
		[Obsolete("This method is deprecated. Please use NewMethod() instead.")]
		public static List<OffrateDto> GetOffRateAccess(string strFullFileName)
		{
			var objStream = new StreamReader(strFullFileName);

			var listOffrate = new List<OffrateDto>();

			string sLine = "";
			int len = 0;
			while ((sLine = objStream.ReadLine()) != null)
			{
				if (sLine.Length > 0)
				{
					var offrate = new OffrateDto();

					offrate.OfdCode = GeneralUtils.ValidateRecordByLine(sLine, 0, 10, ref len);
					offrate.ActCode = GeneralUtils.ValidateRecordByLine(sLine, 0, 10, ref len);
					offrate.CarCategory = GeneralUtils.ValidateRecordByLine(sLine, 0, 1, ref len);
					offrate.OffendAmt = GeneralUtils.ValidateRecordByLine(sLine, 0, 10, ref len);
					offrate.OffendAmt2 = GeneralUtils.ValidateRecordByLine(sLine, 0, 10, ref len);
					offrate.OffendAmt3 = GeneralUtils.ValidateRecordByLine(sLine, 0, 10, ref len);
					listOffrate.Add(offrate);
				}
			}

			objStream.Close();
			objStream.Dispose();

			return listOffrate;
		}
		public static List<OffrateDto> GetOffRateAccess()
        {
            return [.. DbContextProvider.Instance.Offrates];
        }
    }
}