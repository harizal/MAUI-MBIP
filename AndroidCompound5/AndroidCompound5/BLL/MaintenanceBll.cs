using System.Linq;
using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.Classes;

namespace AndroidCompound5
{
    public class MaintenanceBll
    {

        public static void UpdateSemakPassInfo(InfoDto info, SemakPassDto semakPassInfo, string strCurrZone, string strCurrStreet, string strCurrStreetDesc)
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.TransPath + Constants.SEMAKPASSFIL;

            semakPassInfo.Zone = strCurrZone;
            semakPassInfo.Street = strCurrStreet;
            semakPassInfo.StreetDesc = strCurrStreetDesc;

            semakPassInfo.Date = GeneralBll.GetLocalDateyyyyMMdd();
            semakPassInfo.Time = GeneralBll.GetLocalTimeHhmm();

            semakPassInfo.SemakNo = GetSemakPassNumber(info.DolphinId);

            SemakPassAccess.UpdateSemakPassAccess(strFullFileName,  semakPassInfo);

        }

        public static string GetSemakPassNumber(string dolphinId)
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.TransPath + Constants.SEMAKPASSFIL;
            string todaydate = GeneralBll.GetLocalDateTime().ToString("yyMMdd"); ;
            var listSemakPass = SemakPassAccess.GetSemakPassAccess(strFullFileName);

            int rec = listSemakPass.Count;

            string SemakPassNo = "P" + dolphinId + todaydate + (rec + 1).ToString("00");

            if (IsSemakPassNumberExist(SemakPassNo))
                SemakPassNo = "";

            return SemakPassNo;
        }

        public static bool IsSemakPassNumberExist(string passnumber)
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.TransPath + Constants.SEMAKPASSFIL;

            var listSemakPass = SemakPassAccess.GetSemakPassAccess(strFullFileName);
            return listSemakPass.Any(SemakPassDto => SemakPassDto.SemakNo == passnumber);

        }


    }
}