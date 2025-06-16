using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using System;
using System.IO;

namespace AndroidCompound5
{
    /// <summary>
    /// InfoDat = DH01.DAT
    /// </summary>
    public static class InfoAccess
    {
        public static InfoDto GetInfoAccess(string strFullFileName)
        {
            var objInfo = new StreamReader(strFullFileName);

            var info = new InfoDto();

            try
            {
                string sLine;
                int len = 0;

                while ((sLine = objInfo.ReadLine()) != null)
                {
                    //Console.WriteLine(sLine);
                    if (sLine.Length > 0)
                    {
                        info.DolphinId = GeneralUtils.ValidateRecordByLine(sLine, 0, 2, ref len);
                        info.Council = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len);
                        info.AssignZone = GeneralUtils.ValidateRecordByLine(sLine, len, 20, ref len);
                        info.BroadMsg = GeneralUtils.ValidateRecordByLine(sLine, len, 60, ref len);
                        info.StartCmp = GeneralUtils.ValidateRecordByLine(sLine, len, 20, ref len);
                        info.StartSita = GeneralUtils.ValidateRecordByLine(sLine, len, 20, ref len);
                        info.LogDate = GeneralUtils.ValidateRecordByLine(sLine, len, 8, ref len);
                        info.LogTime = GeneralUtils.ValidateRecordByLine(sLine, len, 4, ref len);
                        info.EnforcerId = GeneralUtils.ValidateRecordByLine(sLine, len, 7, ref len);
                        info.CurrMukim = GeneralUtils.ValidateRecordByLine(sLine, len, 2, ref len);
                        info.CurrZone = GeneralUtils.ValidateRecordByLine(sLine, len, 4, ref len);
                        info.CurrDate = GeneralUtils.ValidateRecordByLine(sLine, len, 8, ref len);

                        string sTemp = GeneralUtils.ValidateRecordByLine(sLine, len, 8, ref len);
                        info.CurrComp = GeneralUtils.IsNumeric(sTemp) ? Convert.ToInt32(sTemp) : 0;

                        sTemp = GeneralUtils.ValidateRecordByLine(sLine, len, 8, ref len);
                        info.CurrSita = GeneralUtils.IsNumeric(sTemp) ? Convert.ToInt32(sTemp) : 0;

                        sTemp = GeneralUtils.ValidateRecordByLine(sLine, len, 4, ref len);
                        info.CompCnt = GeneralUtils.IsNumeric(sTemp) ? Convert.ToInt32(sTemp) : 0;

                        sTemp = GeneralUtils.ValidateRecordByLine(sLine, len, 4, ref len);
                        info.SitaCnt = GeneralUtils.IsNumeric(sTemp) ? Convert.ToInt32(sTemp) : 0;

                        sTemp = GeneralUtils.ValidateRecordByLine(sLine, len, 4, ref len);
                        info.NoticeCnt = GeneralUtils.IsNumeric(sTemp) ? Convert.ToInt32(sTemp) : 0;

                        sTemp = GeneralUtils.ValidateRecordByLine(sLine, len, 4, ref len);
                        info.PhotoCnt = GeneralUtils.IsNumeric(sTemp) ? Convert.ToInt32(sTemp) : 0;

                        sTemp = GeneralUtils.ValidateRecordByLine(sLine, len, 4, ref len);
                        info.NoteSize = GeneralUtils.IsNumeric(sTemp) ? Convert.ToInt32(sTemp) : 0;

                        sTemp = GeneralUtils.ValidateRecordByLine(sLine, len, 4, ref len);
                        info.NoteCnt = GeneralUtils.IsNumeric(sTemp) ? Convert.ToInt32(sTemp) : 0;

                        sTemp = GeneralUtils.ValidateRecordByLine(sLine, len, 8, ref len);
                        info.CurrRcpNum = GeneralUtils.IsNumeric(sTemp) ? Convert.ToInt32(sTemp) : 0;

                        sTemp = GeneralUtils.ValidateRecordByLine(sLine, len, 4, ref len);
                        info.RcpCnt = GeneralUtils.IsNumeric(sTemp) ? Convert.ToInt32(sTemp) : 0;

                    }//end if
                }//end while
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            finally
            {

                objInfo.Close();
                objInfo.Dispose();
            }

            return info;
        }

        public static void UpdateInfoAccess(string strFullFileName, InfoDto infoDto, Enums.FormName formName)
        {
            string sLine = "";

            sLine = GeneralUtils.SetLine(sLine, infoDto.DolphinId, 2);
            sLine = GeneralUtils.SetLine(sLine, infoDto.Council, 10);
            sLine = GeneralUtils.SetLine(sLine, infoDto.AssignZone, 20);
            sLine = GeneralUtils.SetLine(sLine, infoDto.BroadMsg, 60);
            sLine = GeneralUtils.SetLine(sLine, infoDto.StartCmp, 20);
            sLine = GeneralUtils.SetLine(sLine, infoDto.StartSita, 20);
            sLine = GeneralUtils.SetLine(sLine, infoDto.LogDate, 8);
            sLine = GeneralUtils.SetLine(sLine, infoDto.LogTime, 4);
            sLine = GeneralUtils.SetLine(sLine, infoDto.EnforcerId, 7);
            sLine = GeneralUtils.SetLine(sLine, infoDto.CurrMukim, 2);
            sLine = GeneralUtils.SetLine(sLine, infoDto.CurrZone, 4);
            sLine = GeneralUtils.SetLine(sLine, infoDto.CurrDate, 8);

            sLine = GeneralUtils.SetLine(sLine, infoDto.CurrComp.ToString("00000000"), 8);
            sLine = GeneralUtils.SetLine(sLine, infoDto.CurrSita.ToString("00000000"), 8);
            sLine = GeneralUtils.SetLine(sLine, infoDto.CompCnt.ToString("0000"), 4);
            sLine = GeneralUtils.SetLine(sLine, infoDto.SitaCnt.ToString("0000"), 4);
            sLine = GeneralUtils.SetLine(sLine, infoDto.NoticeCnt.ToString("0000"), 4);
            sLine = GeneralUtils.SetLine(sLine, infoDto.PhotoCnt.ToString("0000"), 4);
            sLine = GeneralUtils.SetLine(sLine, infoDto.NoteSize.ToString("0000"), 4);
            sLine = GeneralUtils.SetLine(sLine, infoDto.NoteCnt.ToString("0000"), 4);
            sLine = GeneralUtils.SetLine(sLine, infoDto.CurrRcpNum.ToString("00000000"), 8);
            sLine = GeneralUtils.SetLine(sLine, infoDto.RcpCnt.ToString("0000"), 4);
            var fileStream = new FileStream(strFullFileName, FileMode.Create, FileAccess.Write, FileShare.None);
            var objInfo = new StreamWriter(fileStream);
            objInfo.Write(sLine);
            objInfo.Flush();
            fileStream.Flush(true); // true ensures the OS buffer is flushed (requires .NET Core or newer Xamarin)
            fileStream.Close();

            LogFile.WriteLogFile("Size DH01 " + sLine.Trim().Length + ", FormName : " + formName.ToString());

            //todo 
            //copy to bak file
        }
    }
}