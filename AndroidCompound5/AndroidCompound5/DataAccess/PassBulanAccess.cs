using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;


namespace AndroidCompound5
{
    /// <summary>
    /// PasswordFil = DH05.FIL
    /// </summary>
    public static class PassBulanAccess
    {
		[Obsolete("This method is deprecated. Please use NewMethod() instead.")]
		public static List<PassBulanDto> GetPassBulanAccess(string strFullFileName)
		{
			var objStream = new StreamReader(strFullFileName);

			var listPassBulan = new List<PassBulanDto>();

			string sLine = "";
			int len = 0;
			while ((sLine = objStream.ReadLine()) != null)
			{
				if (sLine.Length > 0)
				{
					var passbulan = new PassBulanDto();

					passbulan.PassType = GeneralUtils.ValidateRecordByLine(sLine, 0, 1, ref len);
					passbulan.SerialNum = GeneralUtils.ValidateRecordByLine(sLine, 1, 8, ref len);
					passbulan.CarNum = GeneralUtils.ValidateRecordByLine(sLine, 9, 15, ref len);
					passbulan.StartDate = GeneralUtils.ValidateRecordByLine(sLine, 24, 8, ref len);
					passbulan.EndDate = GeneralUtils.ValidateRecordByLine(sLine, 32, 8, ref len);

					listPassBulan.Add(passbulan);
				}
			}

			objStream.Close();
			objStream.Dispose();

			return listPassBulan;
		}

		public static List<PassBulanDto> GetPassBulanAccess()
        {
            return [.. DbContextProvider.Instance.PassBulans];
        }
    }
}