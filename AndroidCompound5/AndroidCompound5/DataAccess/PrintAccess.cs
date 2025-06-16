using System;
using System.IO;

namespace AndroidCompound5
{
    public static class PrintAccess
    {
        public static bool PrintToFile(string strFullFileName, string sLine)
        {
            try
            {
                if (!File.Exists(strFullFileName))
                {
                    LogFile.WriteLogFile("File Not Exist : " + strFullFileName, Enums.LogType.Debug);
                    var file = new Java.IO.File(strFullFileName);
                    if (!file.Exists())
                        file.CreateNewFile();
                }

                var objPrint = new StreamWriter(strFullFileName, true);

                objPrint.Write(sLine);
                objPrint.Close();
                objPrint.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("Error PrintToFile Message : " + ex.Message, Enums.LogType.Error);
                return false;
            }

        }

    }
}