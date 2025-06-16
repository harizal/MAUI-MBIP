using System;
using System.IO;
using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;

namespace AndroidCompound5
{
    /// <summary>
    /// ConfigApp = "ConfigAppMps.xml"
    /// </summary>
    public static class ConfigAccess
    {
       
        public static ConfigAppDto GetConfigAccess(string strFullFileName)
        {
            var objStream = new StreamReader(strFullFileName);

            var configDto = new ConfigAppDto();

            try
            {
                string sLine;
                int len = 0;

                configDto.ServiceUrl = "Default Service";//"http://1.9.46.170/mbjbwebservice/ServiceCompound.asmx";
                configDto.ServiceAppletUrl = "Default Service";// "http://1.9.46.170:84/smartparking/module-servlet";
                configDto.UrlPhoto = "Default Service";// "http://1.9.46.170:8081/mpnswsparkingphoto/ws-servlet/WSModule/uploadPhoto";
                configDto.UrlImage = "Default Service";// "http://1.9.46.172:38082/home/search?";
                configDto.ServiceKey = "Default Service";
                configDto.FtpHost = "Default Service";
                configDto.FtpUser = "";
                configDto.FtpPassword = "";
                configDto.FtpUnload = "";
                configDto.FtpDownload = "";
                configDto.FtpControl = "";
                configDto.GpsLog = false;

                configDto.GpsInterval = 5;//in minute
                configDto.ImgsWidth = 300;
                configDto.ImgsHeight = 300;
                configDto.SendCompoundInterval = 2; //in minute
                configDto.ServiceUser = "aimforce";
                configDto.ServicePassword = "1234";
                configDto.VideoMaxDuration = 100000;//in second 100 second
                configDto.VideoMaxFileSize = 100000; //in byte 100 kb
                configDto.ApkFileName = "MpknAndroidCompound.MpknAndroidCompound.apk";
                

                while ((sLine = objStream.ReadLine()) != null)
                {
                    //Console.WriteLine(sLine);
                    if (sLine.Length > 0)
                    {
                        if (sLine.Contains("<ServiceUrl>"))
                            configDto.ServiceUrl = RemoveString(sLine, "ServiceUrl");
                        else if (sLine.Contains("<ServiceUPSBUrl>"))
                            configDto.ServiceUPSBUrl = RemoveString(sLine, "ServiceUPSBUrl");
                        else if (sLine.Contains("<ServiceAppletUrl>"))
                            configDto.ServiceAppletUrl = RemoveString(sLine, "ServiceAppletUrl");
                        else if (sLine.Contains("<UrlImage>"))
                            configDto.UrlImage = RemoveString(sLine, "UrlImage");
                        else if (sLine.Contains("<UrlPhoto>"))
                            configDto.UrlPhoto = RemoveString(sLine, "UrlPhoto");
                        else if (sLine.Contains("<ServiceKey>"))
                            configDto.ServiceKey = RemoveString(sLine, "ServiceKey");
                        else if (sLine.Contains("<FtpHost>"))
                            configDto.FtpHost = RemoveString(sLine, "FtpHost");
                        else if (sLine.Contains("<FtpUser>"))
                            configDto.FtpUser = RemoveString(sLine, "FtpUser");
                        else if (sLine.Contains("<FtpPassword>"))
                            configDto.FtpPassword = RemoveString(sLine, "FtpPassword");
                        else if (sLine.Contains("<FtpUnload>"))
                            configDto.FtpUnload = RemoveString(sLine, "FtpUnload");
                        else if (sLine.Contains("<FtpDownload>"))
                            configDto.FtpDownload = RemoveString(sLine, "FtpDownload");
                        else if (sLine.Contains("<ftpControl>"))
                            configDto.FtpControl = RemoveString(sLine, "ftpControl");
                        else if (sLine.Contains("<ServiceUser>"))
                            configDto.ServiceUser = RemoveString(sLine, "ServiceUser");
                        else if (sLine.Contains("<ServicePassword>"))
                            configDto.ServicePassword = RemoveString(sLine, "ServicePassword");
                        else if (sLine.Contains("<GpsLog>"))
                        {

                            if (RemoveString(sLine, "GpsLog") == "1")
                                configDto.GpsLog = true;
                        }
                        else if (sLine.Contains("<GpsInterval>"))
                            configDto.GpsInterval =
                                Convert.ToInt32(RemoveString(sLine, "GpsInterval"));
                        else if (sLine.Contains("<SendCompoundInterval>"))
                        {
                            configDto.SendCompoundInterval = Convert.ToInt32(RemoveString(sLine, "SendCompoundInterval"));
                        }
                        else if (sLine.Contains("<ImgsWidth>"))
                        {
                            configDto.ImgsWidth = Convert.ToInt32(RemoveString(sLine, "ImgsWidth"));
                        }
                        else if (sLine.Contains("<ImgsHeight>"))
                        {
                            configDto.ImgsHeight = Convert.ToInt32(RemoveString(sLine, "ImgsHeight"));
                        }
                        else if (sLine.Contains("<VideoMaxDuration>"))
                            configDto.VideoMaxDuration = Convert.ToInt64(RemoveString(sLine, "VideoMaxDuration"));
                        else if (sLine.Contains("<VideoMaxFileSize>"))
                            configDto.VideoMaxFileSize = Convert.ToInt64(RemoveString(sLine, "VideoMaxFileSize"));
                        else if (sLine.Contains("<ApkFileName>"))
                            configDto.ApkFileName = RemoveString(sLine, "ApkFileName");
                        else if (sLine.Contains("<UploadImage>"))
                            configDto.UrlPhoto = RemoveString(sLine, "UploadImage");
                        else if (sLine.Contains("<CompanyID>"))
                            configDto.CompanyID= RemoveString(sLine, "CompanyID");
                        else if (sLine.Contains("<ParkCode>"))
                            configDto.ParkCode = RemoveString(sLine, "ParkCode");
                        else if (sLine.Contains("<AppID>"))
                            configDto.AppID = RemoveString(sLine, "AppID");


                    }//end if
                }//end while
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("File Name " + strFullFileName, ex.Message, Enums.LogType.Error);
                LogFile.WriteLogFile("GetConfigApp", ex.Message, Enums.LogType.Error);
                LogFile.WriteLogFile("StackTrace : ", ex.StackTrace, Enums.LogType.Error);
            }
            
            finally
            {

                objStream.Close();
                objStream.Dispose();
            }

            return configDto;
        }

        private static string RemoveString(string sValue, string removeString)
        {
            string removeString1 = "<" + removeString + ">";
            string result = sValue.Replace(removeString1, "");
            removeString1 = "</" + removeString + ">";
            result = result.Replace(removeString1, "");
            result = result.Replace("\r", "");
            result = result.Replace(Constants.NewLine, "");
            
            return result.Trim();
        }

    }
}