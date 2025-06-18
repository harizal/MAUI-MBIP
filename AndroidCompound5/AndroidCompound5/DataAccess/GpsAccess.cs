using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using System.Collections.Generic;
using System.IO;

namespace AndroidCompound5
{
    public static class GpsAccess
    {
        public static void AddGpsAccess(string strFullFileName, GpsDto gpsDto)
        {
            string sLine = "";

            sLine = GeneralUtils.SetLine(sLine, gpsDto.Issend, 1);
            sLine = GeneralUtils.SetLine(sLine, gpsDto.ActivityDate, 8);
            sLine = GeneralUtils.SetLine(sLine, gpsDto.ActivityTime, 4);
            sLine = GeneralUtils.SetLine(sLine, gpsDto.GpsX, 15);
            sLine = GeneralUtils.SetLine(sLine, gpsDto.GpsY, 15);
            sLine = GeneralUtils.SetLine(sLine, gpsDto.Kodpguatkuasa, 7);
            sLine = GeneralUtils.SetLine(sLine, gpsDto.BatteryLife, 3);
            sLine = GeneralUtils.SetLine(sLine, gpsDto.DhId, 2);
            sLine = sLine + Constants.NewLine;

            var fileStream = new FileStream(strFullFileName, FileMode.Append, FileAccess.Write, FileShare.None);
            var objNote = new StreamWriter(fileStream);
            objNote.Write(sLine);
            objNote.Flush();
            fileStream.Flush(true); // true ensures the OS buffer is flushed (requires .NET Core or newer Xamarin)
            fileStream.Close();

        }
        public static List<GpsDto> GetGPSAccess(string strFullFileName)
        {
            var objStream = new StreamReader(strFullFileName);

            var listGps = new List<GpsDto>();

            string sLine = "";
            int len = 0;
            while ((sLine = objStream.ReadLine()) != null)
            {
                if (sLine.Length > 0)
                {

                    var gps = new GpsDto();

                    gps.Issend = GeneralUtils.ValidateRecordByLine(sLine, 0, 1, ref len);
                    gps.ActivityDate = GeneralUtils.ValidateRecordByLine(sLine, len, 8, ref len);
                    gps.ActivityTime = GeneralUtils.ValidateRecordByLine(sLine, len, 4, ref len);
                    gps.GpsX = GeneralUtils.ValidateRecordByLine(sLine, len, 15, ref len);
                    gps.GpsY = GeneralUtils.ValidateRecordByLine(sLine, len, 15, ref len);
                    gps.Kodpguatkuasa = GeneralUtils.ValidateRecordByLine(sLine, len, 4, ref len);
                    gps.BatteryLife = GeneralUtils.ValidateRecordByLine(sLine, len, 3, ref len);
                    gps.DhId = GeneralUtils.ValidateRecordByLine(sLine, len, 3, ref len);

                    if (gps.Issend == "N")
                        listGps.Add(gps);
                }
            }

            objStream.Close();
            objStream.Dispose();

            return listGps;
        }

    }
}