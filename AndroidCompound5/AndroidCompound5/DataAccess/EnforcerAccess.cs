
using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using System;
using System.Collections.Generic;
using System.IO;


namespace AndroidCompound5
{
    /// <summary>
    /// PasswordFil = DH05.FIL
    /// </summary>
    public static class EnforcerAccess
    {
        public static List<EnforcerDto> GetEnforcerAccess(string strFullFileName)
        {
            var objStream = new StreamReader(strFullFileName);

            var listEnforcer = new List<EnforcerDto>();

            string sLine = "";
            int len = 0;
            while ((sLine = objStream.ReadLine()) != null)
            {
                if (sLine.Length > 0)
                {
                    var enforcer = new EnforcerDto();

                    enforcer.EnforcerId= GeneralUtils.ValidateRecordByLine(sLine, 0, 7, ref len);
                    enforcer.EnforcerName = GeneralUtils.ValidateRecordByLine(sLine, len, 60, ref len);
                    enforcer.EnforcerIc = GeneralUtils.ValidateRecordByLine(sLine, len, 14, ref len);
                    enforcer.Password = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len);
                    enforcer.Level = GeneralUtils.ValidateRecordByLine(sLine, len, 1, ref len);
                    enforcer.EnforcerUnit = GeneralUtils.ValidateRecordByLine(sLine, len, 50, ref len);
                    enforcer.KodJabatan = GeneralUtils.ValidateRecordByLine(sLine, len, 3, ref len);
                    enforcer.Jabatan = GeneralUtils.ValidateRecordByLine(sLine, len, 50, ref len);
                    enforcer.KodKaunter = GeneralUtils.ValidateRecordByLine(sLine, len, 8, ref len);
                    enforcer.KodCetak = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len);

                    listEnforcer.Add(enforcer);
                }
            }

            objStream.Close();
            objStream.Dispose();

            return listEnforcer;
        }
    }
}