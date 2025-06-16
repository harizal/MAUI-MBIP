using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.Classes;
using System.Collections.Generic;
using System.IO;


namespace AndroidCompound5
{
    public static class EnquiryLogAccess
    {
        public static void AddEnquiryLogFil(LogServerDto EnquiryLog, InfoDto infoDto, string Latitude, string Longitude)
        {
            string sLine = "";

            sLine = GeneralUtils.SetLine(sLine, EnquiryLog.CarNo, 15);
            sLine = GeneralUtils.SetLine(sLine, EnquiryLog.Date, 8);
            sLine = GeneralUtils.SetLine(sLine, EnquiryLog.Time, 6);
            sLine = GeneralUtils.SetLine(sLine, EnquiryLog.DolphinId, 2);
            sLine = GeneralUtils.SetLine(sLine, EnquiryLog.EnforcerId, 7);
            sLine = GeneralUtils.SetLine(sLine, EnquiryLog.Zone, 6);
            sLine = GeneralUtils.SetLine(sLine, EnquiryLog.Sector, 10);
            sLine = GeneralUtils.SetLine(sLine, EnquiryLog.Street, 10);
            sLine = GeneralUtils.SetLine(sLine, EnquiryLog.Status, 1);
            sLine = GeneralUtils.SetLine(sLine, EnquiryLog.End, 2);
            sLine = GeneralUtils.SetLine(sLine, "     ", 6);
            sLine = GeneralUtils.SetLine(sLine, "         ", 10);

            sLine = GeneralUtils.SetLine(sLine, Latitude, 15);
            sLine = GeneralUtils.SetLine(sLine, Longitude, 15);


            sLine += Constants.NewLine;
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.TransPath + Constants.ENQUIRYLOGFIL;

            var objNote = new StreamWriter(strFullFileName, true);

            objNote.Write(sLine);
            objNote.Close();
            objNote.Dispose();

        }

        public static List<LogServerDto> GetEnquiryLogAccess(string strFullFileName)
        {
            var objStream = new StreamReader(strFullFileName);

            var listEnquiryLog = new List<LogServerDto>();

            string sLine = "";
            int len = 0;
            while ((sLine = objStream.ReadLine()) != null)
            {
                if (sLine.Length > 0)
                {

                    var enquirylog = new LogServerDto();

                    enquirylog.CarNo = GeneralUtils.ValidateRecordByLine(sLine, 0, 15, ref len);
                    enquirylog.Date = GeneralUtils.ValidateRecordByLine(sLine, len, 8, ref len);
                    enquirylog.Time = GeneralUtils.ValidateRecordByLine(sLine, len, 6, ref len);
                    enquirylog.DolphinId = GeneralUtils.ValidateRecordByLine(sLine, len, 2, ref len);
                    enquirylog.EnforcerId = GeneralUtils.ValidateRecordByLine(sLine, len, 7, ref len);
                    enquirylog.Zone = GeneralUtils.ValidateRecordByLine(sLine, len, 6, ref len);
                    enquirylog.Sector = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len);
                    enquirylog.Street = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len);
                    enquirylog.Status = GeneralUtils.ValidateRecordByLine(sLine, len, 1, ref len);
                    enquirylog.End = GeneralUtils.ValidateRecordByLine(sLine, len, 2, ref len);
                    enquirylog.Latitude = GeneralUtils.ValidateRecordByLine(sLine, len, 15, ref len);
                    enquirylog.Longitude = GeneralUtils.ValidateRecordByLine(sLine, len, 15, ref len);

                    listEnquiryLog.Add(enquirylog);
                }
            }

            objStream.Close();
            objStream.Dispose();

            return listEnquiryLog;
        }
    }
}