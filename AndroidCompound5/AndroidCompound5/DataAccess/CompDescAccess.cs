using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using System.Collections.Generic;
using System.IO;

namespace AndroidCompound5
{
    public static class CompDescAccess
    {
        public static List<CompDescDto> GetCompDescAccess(string strFullFileName)
        {
            var objStream = new StreamReader(strFullFileName);

            var listCompDesc = new List<CompDescDto>();

            string sLine = "";
            int len = 0;


            while ((sLine = objStream.ReadLine()) != null)
            {
                if (sLine.Length > 0)
                {

                    var compDesc = new CompDescDto()
                    {
                        ActCode = GeneralUtils.ValidateRecordByLine(sLine, 0, 10, ref len).Trim(),
                        OfdCode = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim(),
                        ButirCode = GeneralUtils.ValidateRecordByLine(sLine, len, 2, ref len).Trim(),
                        ButirDesc = GeneralUtils.ValidateRecordByLine(sLine, len, 600, ref len).Trim()
                    };
                    listCompDesc.Add(compDesc);

                }
            }

            objStream.Close();
            objStream.Dispose();

            return listCompDesc;
        }
    }
}