using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AndroidCompound5
{
    public static class SemakPassAccess
    {
        public static void UpdateSemakPassAccess(string strFullFileName, SemakPassDto semakPassInfo)
        {
            bool blUpdate = false;

            string sLine = "";

            sLine = GeneralUtils.SetLine(sLine, semakPassInfo.SemakNo, 12);
            sLine = GeneralUtils.SetLine(sLine, semakPassInfo.Zone, 6);
            sLine = GeneralUtils.SetLine(sLine, semakPassInfo.Street, 10);
            sLine = GeneralUtils.SetLine(sLine, semakPassInfo.StreetDesc, 100);
            sLine = GeneralUtils.SetLine(sLine, semakPassInfo.NoPetak, 6);
            sLine = GeneralUtils.SetLine(sLine, semakPassInfo.NamaPemohon, 50);
            sLine = GeneralUtils.SetLine(sLine, semakPassInfo.StartDate, 10);
            sLine = GeneralUtils.SetLine(sLine, semakPassInfo.EndDate, 10);
            sLine = GeneralUtils.SetLine(sLine, semakPassInfo.Remark, 50);
            sLine = GeneralUtils.SetLine(sLine, semakPassInfo.Date, 8);
            sLine = GeneralUtils.SetLine(sLine, semakPassInfo.Time, 4);
            sLine = GeneralUtils.SetLine(sLine, semakPassInfo.Pic1, 20);
            sLine = GeneralUtils.SetLine(sLine, semakPassInfo.EnfId, 4);
            sLine += Constants.NewLine;

            long length = new System.IO.FileInfo(strFullFileName).Length;
            LogFile.WriteLogFile("Length Note fil " + length);
            if (length == 0)
            {
                var objInfo = new StreamWriter(strFullFileName, false);

                objInfo.Write(sLine);
                objInfo.Close();
                objInfo.Dispose();
            }
            else
            {
                StreamReader objNote = new StreamReader(strFullFileName);
                var sUpdate = new StringBuilder();

                while ((sLine = objNote.ReadLine()) != null)
                {
                    if (sLine.Length > 0)
                    {

                        if (sLine.Substring(1, 12) == semakPassInfo.SemakNo)
                        {
                            blUpdate = true;
                            sUpdate.Append(sLine);
                        }
                        else
                        {
                            sUpdate.Append(sLine + Constants.NewLine);
                        }
                    }
                }
                objNote.Close();
                objNote.Dispose();

                //add new note record
                if (!blUpdate)
                    sUpdate.Append(sLine);

                File.Delete(strFullFileName);

                var objWrite = new StreamWriter(strFullFileName);

                objWrite.Write(sUpdate);
                objWrite.Close();
                objWrite.Dispose();
            }

           
        }

        public static void AddSemakPassAccess(string strFullFileName, SemakPassDto semakPassInfo)
        {
            string sLine = "";

            sLine = GeneralUtils.SetLine(sLine, semakPassInfo.SemakNo, 12);
            sLine = GeneralUtils.SetLine(sLine, semakPassInfo.Zone, 6);
            sLine = GeneralUtils.SetLine(sLine, semakPassInfo.Street, 10);
            sLine = GeneralUtils.SetLine(sLine, semakPassInfo.StreetDesc, 100);
            sLine = GeneralUtils.SetLine(sLine, semakPassInfo.NoPetak, 6);
            sLine = GeneralUtils.SetLine(sLine, semakPassInfo.NamaPemohon, 50);
            sLine = GeneralUtils.SetLine(sLine, semakPassInfo.StartDate, 10);
            sLine = GeneralUtils.SetLine(sLine, semakPassInfo.EndDate, 10);
            sLine = GeneralUtils.SetLine(sLine, semakPassInfo.Remark, 50);
            sLine = GeneralUtils.SetLine(sLine, semakPassInfo.Date, 8);
            sLine = GeneralUtils.SetLine(sLine, semakPassInfo.Time, 4);
            sLine = GeneralUtils.SetLine(sLine, semakPassInfo.Pic1, 20);
            sLine = GeneralUtils.SetLine(sLine, semakPassInfo.EnfId, 4);
            sLine += Constants.NewLine;

            var objNote = new StreamWriter(strFullFileName, true);

            objNote.Write(sLine);
            objNote.Close();
            objNote.Dispose();

        }
        
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