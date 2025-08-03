using System.Collections.Generic;
using System.Linq;
using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.Classes;

namespace AndroidCompound5
{
    public static class StreetBll
    {
        public static List<StreetDto> GetStreetByZone(string zone, string mukim)
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.StreetFil;

            var listStreet = new List<StreetDto>();
            if (!System.IO.File.Exists(strFullFileName))
                return listStreet;

            return StreetAccess.GetStreetAccess().Where(c => c.Zone == zone && c.Mukim == mukim).ToList();
        }

        public static StreetDto? GetStreetByCodeAndZone(string code, string zone, string mukim)
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.StreetFil;

            if (!System.IO.File.Exists(strFullFileName))
                return null;

            return StreetAccess.GetStreetAccess().FirstOrDefault(c => c.Code == code && c.Zone == zone && c.Mukim == mukim) ?? null;
        }

    }
}