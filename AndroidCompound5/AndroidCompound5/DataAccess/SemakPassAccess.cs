using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.DataAccess;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AndroidCompound5
{
    public static class SemakPassAccess
    {
        public static void UpdateSemakPassAccess(SemakPassDto semakPassInfo)
        {
            DbContextProvider.Instance.SemakPasses.Update(semakPassInfo);
        }

        public static void AddSemakPassAccess(SemakPassDto semakPassInfo)
        {
            DbContextProvider.Instance.SemakPasses.Add(semakPassInfo);
        }
        
        public static List<SemakPassDto> GetSemakPassAccess()
        {
            return [.. DbContextProvider.Instance.SemakPasses];
        }

		[Obsolete("This method is deprecated. Please use NewMethod() instead.")]
		public static List<SemakPassDto> GetSemakPassAccess(string strFullFileName)
		{
			var objStream = new StreamReader(strFullFileName);

			var listSemakPass = new List<SemakPassDto>();

			string sLine = "";
			int len = 0;
			while ((sLine = objStream.ReadLine()) != null)
			{
				if (sLine.Length > 0)
				{
					var semakpass = new SemakPassDto();

					semakpass.SemakNo = GeneralUtils.ValidateRecordByLine(sLine, 0, 12, ref len);
					semakpass.Zone = GeneralUtils.ValidateRecordByLine(sLine, len, 6, ref len);
					semakpass.Street = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim();
					semakpass.StreetDesc = GeneralUtils.ValidateRecordByLine(sLine, len, 100, ref len);
					semakpass.NoPetak = GeneralUtils.ValidateRecordByLine(sLine, len, 6, ref len);
					semakpass.NamaPemohon = GeneralUtils.ValidateRecordByLine(sLine, len, 50, ref len).Trim();
					semakpass.StartDate = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim();
					semakpass.EndDate = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim();
					semakpass.Remark = GeneralUtils.ValidateRecordByLine(sLine, len, 50, ref len).Trim();
					semakpass.Date = GeneralUtils.ValidateRecordByLine(sLine, len, 8, ref len).Trim();
					semakpass.Time = GeneralUtils.ValidateRecordByLine(sLine, len, 4, ref len).Trim();
					semakpass.Pic1 = GeneralUtils.ValidateRecordByLine(sLine, len, 20, ref len).Trim();
					semakpass.EnfId = GeneralUtils.ValidateRecordByLine(sLine, len, 4, ref len).Trim();

					listSemakPass.Add(semakpass);
				}
			}

			objStream.Close();
			objStream.Dispose();

			return listSemakPass;
		}
	}
}