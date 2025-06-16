using AndroidCompound5.Classes;
using System.Linq;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.AimforceUtils;


namespace AndroidCompound5
{
    public static class PassBulanBll
    {
        public static PassBulanDto GetPassBulanByCarNum(string carnum)
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.PassFil;

            if (!System.IO.File.Exists(strFullFileName))
                return null;

            var listPassBulan = PassBulanAccess.GetPassBulanAccess(strFullFileName);

            return listPassBulan.FirstOrDefault(c => c.CarNum == carnum);

        }

    }
}