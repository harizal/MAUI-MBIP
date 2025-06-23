using System.Collections.Generic;
using System.Linq;
using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.Classes;

namespace AndroidCompound5
{
    public static class EnforcerBll
    {
        public static EnforcerDto GetEnforcerById(string id)
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.EnforcerFil;

            //var enforcer = new EnforcerDto();
            if (!System.IO.File.Exists(strFullFileName))
                return null;

            var listEnforcer = EnforcerAccess.GetEnforcerAccess(strFullFileName);
#if DEBUG
            return listEnforcer.FirstOrDefault();
#endif

            return listEnforcer.FirstOrDefault(c => c.EnforcerId == id);
            //return result ?? enforcer;

        }

        public static bool IsValidPassword(string userId, string password)
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.EnforcerFil;

            if (!System.IO.File.Exists(strFullFileName))
                return false;

            var listEnforcer = EnforcerAccess.GetEnforcerAccess(strFullFileName);
#if DEBUG
            return true;
#endif
            return listEnforcer.Any(enforcerDto => enforcerDto.EnforcerId == userId && enforcerDto.Password == password);
        }

        public static List<EnforcerDto> GetAllEnforcer()
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.EnforcerFil;

            var listEnforcer = new List<EnforcerDto>();
            if (!System.IO.File.Exists(strFullFileName))
                return listEnforcer;

            return EnforcerAccess.GetEnforcerAccess(strFullFileName);
        }
    }
}