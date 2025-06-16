using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using AndroidCompound5.BusinessObject.DTOs;
using Android.Graphics;
using System.Text.RegularExpressions;
using Java.Lang;
using Exception = System.Exception;
using System.Security.Cryptography;
using System.Text;
using AndroidCompound5.Classes;
using Newtonsoft.Json;
using AndroidCompound5.AimforceUtils;

namespace AndroidCompound5
{
    public static class GeneralBll
    {
        public static bool CopyFile(string source, string destination, bool isFullPath)
        {
            try
            {
                var pathFile = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath;

                if (!isFullPath)
                {
                    source = pathFile + source;
                    destination = pathFile + destination;
                }

                File.Copy(source, destination, true);

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("Error CopyFile : " + ex.Message, Enums.LogType.Error);
                return false;
            }
        }

        public static bool IsNumeric(string sValue)
        {
            try
            {
                Convert.ToDecimal(sValue);

                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }

        }

        public static void CreateFolder(string path)
        {
            var tmpFile = new Java.IO.File(path);
            if (!tmpFile.Exists())
                tmpFile.Mkdirs();
        }

        public static void CreateNewFile(string pathFile)
        {
            var file = new Java.IO.File(pathFile);
            if (!file.Exists())
                file.CreateNewFile();
        }
        public static void InitFolder()
        {
            string strFolder = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.DCIMPath + Constants.ImgsPath;
            CreateFolder(strFolder);
            strFolder = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath;
            CreateFolder(strFolder);
            strFolder = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath + Constants.SendOnlinePath + Constants.ImgsPath;
            CreateFolder(strFolder);

            var listFolder = new List<string>
                            {
                                Constants.MasterPath,
                                Constants.TransPath,
                                Constants.TempPath,
                                Constants.SignPath,
                                Constants.LogPath,
                                Constants.ImgsPath,
                                Constants.BackupPath,
                                Constants.SendOnlinePath,
                            };

            foreach (var folder in listFolder)
            {
                CreateFolder(strFolder + folder);
            }
        }

        public static void InitLog()
        {
            LogFile.WriteLogFile(Constants.AppVersion);

        }

        public static List<string> CheckMasterFile()
        {
            string strFolder = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath +
                               Constants.MasterPath;

            var listMasterFile = new List<string>()
            {
                Constants.InfoDat,
                Constants.TableFil,
                Constants.EnforcerFil,
                Constants.StreetFil,
                Constants.ConfigApp
            };
            var result = new List<string>();

            foreach (var masterFile in listMasterFile)
            {
                //LogFile.WriteLogFile(strFolder + masterFile,Enums.LogType.Debug);
                if (File.Exists(strFolder + masterFile)) continue;
                if (masterFile.Contains(Constants.InfoDat))
                    result.Add("Fail control tak dijumpai. " + masterFile + "\r\n");
                else
                    result.Add("Missing master file " + masterFile + "\r\n");

            }

            return result;
        }

        public static string GetLocalDate()
        {
            return GetLocalDateTime().ToString("dd/MM/yyyy");
        }

        public static string GetLocalTime()
        {
            return GetLocalDateTime().ToString("HH:mm:ss");
        }

        public static DateTime GetLocalDateTime()
        {
            return DateTime.Now;
            //return DateTimeBll.GetLocalDateTime();
        }

        public static string GetInternalImagePath()
        {
            //string path = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath + Constants.ImgsPath;
            string path = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.DCIMPath + Constants.ImgsPath;
            return path;
        }

        public static string GetSendonLinePath()
        {
            string path = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath +
                          Constants.SendOnlinePath ;
            return path;
        }

        public static string GetMasterPath()
        {
            string path = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath +
                          Constants.MasterPath;

            return path;
        }

        public static string GetTransPath()
        {
            string path = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath +
                          Constants.TransPath;

            return path;
        }

        public static string GetTempPath()
        {
            string path = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath +
                          Constants.TempPath;

            return path;
        }

        public static string GetSignatureBackupPath()
        {
            string path = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath +
                          Constants.BackupPath + "Files/";


            CreateFolder(path);

            return path;
        }

        public static string GetImageBackupPath()
        {
            string path = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath +
                          Constants.BackupPath + "IMGS/";

            //if (ExternalSdCardInfo.ExternalSdCardExists)
            //{
            //    path = System.IO.Path.Combine(ExternalSdCardInfo.Path, GlobalClass.TRANSBACKUPPATH + "IMGS/");
            //}

            CreateFolder(path);

            return path;
        }

        public static string GetTransBackupPath()
        {
            string path = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath +
                          Constants.BackupPath;

            //if (ExternalSdCardInfo.ExternalSdCardExists)
            //{
            //    path = System.IO.Path.Combine(ExternalSdCardInfo.Path, GlobalClass.TRANSBACKUPPATH);
            //}

            CreateFolder(path);

            return path;
        }


        public static bool IsFileExist(string fileName, bool isFullPath)
        {
            if (!isFullPath)
                fileName = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath +
                           fileName;
            return File.Exists(fileName);
        }

        public static ConfigAppDto GetConfig()
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.ConfigApp;

            if (!System.IO.File.Exists(strFullFileName))
                return null;

            return ConfigAccess.GetConfigAccess(strFullFileName);

        }

        public static string GetSignaturePath()
        {
            string path = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath +
                          Constants.SignPath;
            return path;
        }

        public static string RemoveUnicodeCharactersFromString(string inputValue)
        {
            return new string(inputValue.Where(c => c <= sbyte.MaxValue).ToArray());
        }

        public static bool IsUnicodeFound(string inputValue)
        {
            return inputValue.Any(c => c > sbyte.MaxValue);
        }

        public static void BitmapToFile(Bitmap bitmap, string sFileName)
        {


            using (var fs = new FileStream(sFileName, FileMode.OpenOrCreate))
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 100, fs);
            }

        }

        public static void DeleteFile(string strFullFileName)
        {
            File.Delete(strFullFileName);
        }

        public static void DeleteFile(string strFileName, bool isFullPath)
        {
            if (!isFullPath)
                strFileName = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath +
                              strFileName;

            File.Delete(strFileName);
        }

        public static List<string> GetListMasterFiles()
        {
            var listFiles = new List<string>()
            {
                Constants.TableFil,
                Constants.EnforcerFil,
                Constants.StreetFil,
                Constants.OffRateFil,
                Constants.UsedCouponFil,
                Constants.HandheldFil,
                Constants.PassFil,
                Constants.CompDesc,
                Constants.MessageFil,
                //Constants.ActTitleFil,
                //Constants.LesenFil

            };

            return listFiles;
        }

        public static List<string> GetListTransFiles()
        {
            var listFiles = new List<string>()
            {
               Constants.CompoundFil,
               Constants.NoteFil,
               Constants.SitaFil,
               Constants.SEMAKPASSFIL,
               Constants.GpsFil,
               Constants.NewCouponFil,
               Constants.ENQUIRYLOGFIL,

            };

            return listFiles;
        }

        public static void InitFileTrans()
        {

            var listTransFile = GetListTransFiles();

            var pathSource = GetTransPath();
            foreach (var transFile in listTransFile)
            {

                DeleteFile(pathSource + transFile, true);
                if (!IsFileExist(pathSource + transFile, true))
                    CreateNewFile(pathSource + transFile);
            }
        }

        public static void InitFileImages()
        {
            var pathSource = GeneralBll.GetInternalImagePath();

            //get all image in folder
            var di = new DirectoryInfo(pathSource);
            FileInfo[] rgFiles = di.GetFiles("*.*");

            foreach (FileInfo fi in rgFiles)
            {
                if (GeneralBll.IsFileExist(fi.FullName, true))
                    GeneralBll.DeleteFile(fi.FullName);
            }
        }

        public static void InitFileSendOnLine()
        {
            var pathSource = GeneralBll.GetSendonLinePath();

            //get all Send Online files in folder
            var di = new DirectoryInfo(pathSource);
            FileInfo[] rgFiles = di.GetFiles("*.*");

            foreach (FileInfo fi in rgFiles)
            {
                if (GeneralBll.IsFileExist(fi.FullName, true))
                    GeneralBll.DeleteFile(fi.FullName);
            }
        }

        public static List<string> GetListFileImageNameByCompoundNumber(string compoundNumber)
        {
            var listFileName = new List<string>();

            var pathImageSource = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.DCIMPath +
                           Constants.ImgsPath;
            var di = new DirectoryInfo(pathImageSource);
            FileInfo[] rgFiles = di.GetFiles(compoundNumber + "*.*");


            foreach (FileInfo fi in rgFiles)
            {
                if (File.Exists(fi.FullName))
                    listFileName.Add(fi.FullName);
            }

            return listFileName;
        }

        public static int GetTotalUnsendCompound()
        {
            var pathUnSendCompound = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath + Constants.SendOnlinePath ;
            var di = new DirectoryInfo(pathUnSendCompound);
            FileInfo[] rgFiles = di.GetFiles("*.txt");

            rgFiles.Count();


            return rgFiles.Count();
        }
        public static int GetTotalUnsendCompoundPhoto()
        {
            var pathUnSendCompoundPhoto = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath + Constants.SendOnlinePath + Constants.ImgsPath; ;
            var di = new DirectoryInfo(pathUnSendCompoundPhoto);
            FileInfo[] rgFiles = di.GetFiles("*.jpg");

            rgFiles.Count();


            return rgFiles.Count();
        }

        public static List<string> GetListFileImageNameOnLineByCompoundNumber(string compoundNumber)
        {
            var listFileName = new List<string>();

            var pathImageSource = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath + Constants.SendOnlinePath + Constants.ImgsPath;
            var di = new DirectoryInfo(pathImageSource);
            FileInfo[] rgFiles = di.GetFiles(compoundNumber + "*.jpg");


            foreach (FileInfo fi in rgFiles)
            {
                if (File.Exists(fi.FullName))
                    listFileName.Add(fi.FullName);
            }

            return listFileName;
        }

        private static bool isExistinList(List<string> aList, string strNokmp)
        {
            bool bRet = false;

            foreach (var item in aList)
            {
                if (item == strNokmp)
                {
                    bRet = true;
                    break;
                }
            }

            return bRet;
        }
        public static List<string> GetListAllUnSendImageNoCompOnLine()
        {
            var listNoKmp = new List<string>();
            string strNoKmp = "";

            var pathImageSource = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath + Constants.SendOnlinePath + Constants.ImgsPath;
            var di = new DirectoryInfo(pathImageSource);
            FileInfo[] rgFiles = di.GetFiles("*.jpg");


            foreach (FileInfo fi in rgFiles)
            {
                if (File.Exists(fi.FullName))
                {
                    strNoKmp = fi.Name.Substring(0, 14);
                    if (!isExistinList(listNoKmp, strNoKmp))
                        listNoKmp.Add(strNoKmp);
                }
            }

            return listNoKmp;
        }
        public static void CopyFinalImage2OnLineByCompoundNumber(string strFileName)
        {
            var pathImageSource = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.DCIMPath + Constants.ImgsPath;
            var pathImageDest = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath + Constants.SendOnlinePath + Constants.ImgsPath;

            if (File.Exists(pathImageSource + strFileName))
                File.Copy(pathImageSource + strFileName, pathImageDest + strFileName);
        }
        public static void CopyImage2OnLineByCompoundNumber(string compoundNumber)
        {
            var pathImageSource = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.DCIMPath + Constants.ImgsPath;
            var pathImageDest = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath + Constants.SendOnlinePath + Constants.ImgsPath;
            CreateFolder(pathImageDest);

            var di = new DirectoryInfo(pathImageSource);
            FileInfo[] rgFiles = di.GetFiles(compoundNumber + "*.*");


            foreach (FileInfo fi in rgFiles)
            {
                if (File.Exists(fi.FullName))
                    File.Copy(fi.FullName, pathImageDest + fi.Name);
            }

            return;
        }

        public static string GetMoneyFormat(string value1, string value2)
        {
            value1 = value1.PadLeft(4, '0');
            value2 = value2.PadLeft(2, '0');

            return value1 + value2;
        }

        public static string GetKeyAmountStringCompoundType5(string value)
        {
            if (IsNumeric(value))
                value = Convert.ToInt32(value).ToString();

            return value;
        }


        public static bool MoveFile(string source, string destination)
        {
            try
            {
                File.Copy(source, destination, true);
                File.Delete(source);
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("Error MoveFile : " + source, Enums.LogType.Error);
                LogFile.WriteLogFile("Message : " + ex.Message, Enums.LogType.Error);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input">yyyyMMdd</param>
        /// <returns></returns>
        public static DateTime? ConvertStringToDate(string input)
        {
            if (input.Length != 8)
                return null;

            try
            {
                var year = input.Substring(0, 4);
                var month = input.Substring(4, 2);
                var day = input.Substring(6, 2);
                var dtDate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));
                return dtDate;
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("Error GeneralBll ConvertStringToDate : " + ex.Message, Enums.LogType.Error);
                return null;
            }
            //string result = value.Substring(6, 2) + "-" + value.Substring(4, 2) + "-" + value.Substring(0, 4);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input">dd/MM/yyyy hh:mm:ss</param>
        /// <returns></returns>
        //                                  01234567890123
        public static DateTime ConvertStringddmmyyyyhhmmssToDateTime(string input)
        {
            if (input.Length != 19)
                input = GeneralBll.GetLocalDateTime().ToString("dd-MM-yyyy HH:mm:ss");

            try
            {
                var year = input.Substring(6, 4);
                var month = input.Substring(3, 2);
                var day = input.Substring(0, 2);
                var hour = input.Substring(11, 2);
                var minute = input.Substring(14, 2);
                var second = input.Substring(17, 2);

                var dtDate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day),
                                    Convert.ToInt32(hour), Convert.ToInt32(minute), Convert.ToInt32(second));
                return dtDate;
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("Error GeneralBll ConvertStringToDateTime(" + input + ")" + " : " + ex.Message, Enums.LogType.Error);
                return GeneralBll.GetLocalDateTime();
            }

        }
        public static DateTime ConvertStringToDateTime(string input)
        {
            if (input.Length != 14)
                return GeneralBll.GetLocalDateTime();

            try
            {
                var year = input.Substring(0, 4);
                var month = input.Substring(4, 2);
                var day = input.Substring(6, 2);
                var hour = input.Substring(8, 2);
                var minute = input.Substring(10, 2);
                var second = input.Substring(12, 2);

                var dtDate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day),
                                    Convert.ToInt32(hour), Convert.ToInt32(minute), Convert.ToInt32(second));
                return dtDate;
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("Error GeneralBll ConvertStringToDateTime(" + input + ")" + " : " + ex.Message, Enums.LogType.Error);
                return GeneralBll.GetLocalDateTime();
            }

        }
        public static string ConvertAmountOffend(string sValue)
        {
            try
            {
                return (Convert.ToDecimal(sValue) / 100).ToString("f");
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

        public static int ConvertStringToInt(string value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch (Exception)
            {

                return 0;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sValue">yyyyMMdd</param>
        /// <returns>dd-MM-yyyy</returns>
        public static string FormatPrintDate(string sValue)
        {
            try
            {
                if (sValue.Length == 8)
                {
                    //yyyyMMdd to dd-MM-yyyy
                    sValue = sValue.Substring(6, 2) + "-" + sValue.Substring(4, 2) + "-" + sValue.Substring(0, 4);
                }

                return sValue;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return sValue;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">HHmm</param>
        /// <returns>hh:mm tt</returns>
        public static string FormatPrintTime(string value)
        {
            if (value.Length != 6) return value;

            int hour = ConvertStringToInt(value.Substring(0, 2));
            int minute = ConvertStringToInt(value.Substring(2, 2));
            int second = ConvertStringToInt(value.Substring(4, 2));
            string result = "";
            if (hour >= 12)
            {
                if (hour == 12)
                    result = "12:" + minute.ToString("00") + ":" + second.ToString("00") + " P.M.";
                else
                    result = (hour - 12).ToString("00") + ":" + minute.ToString("00") + ":"+ second.ToString("00") + " P.M.";
            }
            else
                result = (hour).ToString("00") + ":" + minute.ToString("00") + ":" + second.ToString("00") + " A.M.";

            return result;
        }

        /// <summary>
        /// 
        /// </summary>          0123456789012345678
        /// <param name="input">dd/mm/yyyy hh:mm:ss</param>
        /// <returns>= ÿyyy-mm-dd hh:mm:ss
        public static string ConvertStringddmmyyyyhhmmssToyyyymmddhhmmss(string input)
        {
            string sReturn = "";
            //if (input.Length != 19)
            //    return GeneralBll.GetLocalDateTime().ToString();

            string year = input.Substring(6, 4);
            string month = input.Substring(3, 2);
            string day = input.Substring(0, 2);
            string hour = input.Substring(11, 2);
            string minute = input.Substring(14, 2);
            string second = input.Substring(17, 2);
            sReturn = year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + second;
            return sReturn;

        }

        /// <summary>
        /// 
        /// </summary>          0123456789012345678
        /// <param name="input">yyyy-mm-dd hh:mm:ss</param>
        /// <returns>= dd/mm/yyyy hh:mm:ss
        public static string ConvertStringyyyymmddToddmmyyyyhhmmss(string input)
        {
            string sReturn = "";
            if (input.Length != 19)
                return GeneralBll.GetLocalDateTime().ToString();

            string year = input.Substring(0, 4);
            string month = input.Substring(5, 2);
            string day = input.Substring(8, 2);
            string hour = input.Substring(11, 2);
            string minute = input.Substring(14, 2);
            string second = input.Substring(17, 2);
            sReturn = day + "-" + month + "-" + year + " " + hour + ":" + minute + ":" + second;
            return sReturn;

        }


        /// <summary>
        /// 
        /// </summary>          0123456789012345678
        /// <param name="input">dd/MM/yyyy hh:mm:ss</param>
        /// <returns>= ÿyyymmdd
        public static string GetDateDDMMYYYY(string input)
        {
            string sReturn = "";

            string year = input.Substring(6, 4);
            string month = input.Substring(3, 2);
            string day = input.Substring(0, 2);
            sReturn = year + month + day;
            return sReturn;

        }

        public static string GetLocalDateyyyyMMdd()
        {
            return GetLocalDateTime().ToString("yyyyMMdd");
        }

        public static string GetLocalTimeHhmmss()
        {
            return GetLocalDateTime().ToString("HHmmss");
        }

        public static string GetLocalTimeHhmm()
        {
            return GetLocalDateTime().ToString("HHmm");
        }
        public static double ConvertStringToDouble(string value)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch (Exception)
            {

                return 0;
            }
        }

        public static string FormatPrintAmount(string value)
        {
            var result = ConvertStringToDouble(value);

            if (result > 0)
            {
                result = result / 100;
                return "RM" + result.ToString("f2");
            }

            return "RM";
        }

        public static string FormatAmountInCents(string value)
        {
            var result = ConvertStringToDouble(value);
            string returnstr = "";

            if (result > 0)
            {
                result = result * 100;
                returnstr = result.ToString();
                returnstr = "0000000" + returnstr.TrimEnd();
                return returnstr.Substring(returnstr.Length - 7);
            }

            return "0000000";
        }

        public static string FormatViewAmount(string value)
        {
            var result = ConvertStringToDouble(value);

            if (result > 0)
            {
                result = result / 100;
                return "RM" + result.ToString("f0");
            }

            return "RM";
        }

        public static void LogGpsData(GpsDto gpsDto, string serviceUrl, string serviceUser, string servicePassword)
        {
            try
            {
                string strFolder = GeneralAndroidClass.GetExternalStorageDirectory();
                strFolder += Constants.ProgramPath + Constants.TransPath;

                string strFullFileName = strFolder + Constants.GpsFil;
                if (!File.Exists(strFullFileName))
                {
                    //LogFile.WriteLogFile("File Not Exist : " + strFullFileName, Enums.LogType.Debug);
                    CreateNewFile(strFullFileName);
                }

                gpsDto.Issend = "N";
                gpsDto.ActivityDate = GetLocalDateTime().ToString("yyyyMMdd");
                gpsDto.ActivityTime = GetLocalDateTime().ToString("HHmm");

                var svc = new WebService(serviceUrl, serviceUser, servicePassword);
                var message = svc.SendGpsInfo(gpsDto);
                if (message == Constants.SendSuccess)
                    gpsDto.Issend = "S";

                GpsAccess.AddGpsAccess(strFullFileName, gpsDto);

            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("GeneralBll LogGpsData Err : " + ex.Message, Enums.LogType.Error, Enums.FileLogType.GpsService);
            }


        }

        public static string GetVideoPath()
        {
            string path = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath +
                          Constants.VideoPath;

            CreateFolder(path);


            return path;
        }

        public static bool IsAlphaNumericAndSpaceCommaDotPercentMinus(string inputValue)
        {
            if (string.IsNullOrEmpty(inputValue)) return true;
            Regex r = new Regex(@"^[a-zA-Z0-9\ \,\.\%\-\/\\\(\)\&\@]*$");
            return r.IsMatch(inputValue);
        }
        public static bool IsAlphaNumericAndDotCommaSpaceMinusSlashBraket(string inputValue)
        {
            if (string.IsNullOrEmpty(inputValue)) return true;
            Regex r = new Regex(@"^[a-zA-Z0-9\.\,\ \-\/\(\)]*$");
            return r.IsMatch(inputValue);
        }

        public static bool IsAlphaNumericAndDotCommaSpaceMinusSlash(string inputValue)
        {
            if (string.IsNullOrEmpty(inputValue)) return true;
            Regex r = new Regex(@"^[a-zA-Z0-9\.\,\ \-\/]*$");
            return r.IsMatch(inputValue);
        }
        public static bool IsAlphaNumericAndDotCommaMinusSlash(string inputValue)
        {
            if (string.IsNullOrEmpty(inputValue)) return true;
            Regex r = new Regex(@"^[a-zA-Z0-9\.\,\-\/]*$");
            return r.IsMatch(inputValue);
        }
        public static bool IsAlphaNumericAndDotComma(string inputValue)
        {
            if (string.IsNullOrEmpty(inputValue)) return true;
            Regex r = new Regex(@"^[a-zA-Z0-9\.\,]*$");
            return r.IsMatch(inputValue);
        }
        public static bool IsAlphaNumericAndMinusSlash(string inputValue)
        {
            if (string.IsNullOrEmpty(inputValue)) return true;
            Regex r = new Regex(@"^[a-zA-Z0-9\-\/]*$");
            return r.IsMatch(inputValue);
        }
        public static bool IsAlphaNumericAndMinus(string inputValue)
        {
            if (string.IsNullOrEmpty(inputValue)) return true;
            Regex r = new Regex(@"^[a-zA-Z0-9\-]*$");
            return r.IsMatch(inputValue);
        }

        public static bool IsAlphaNumericAndSpace(string inputValue)
        {
            if (string.IsNullOrEmpty(inputValue)) return true;
            Regex r = new Regex(@"^[a-zA-Z0-9\ ]*$");
            return r.IsMatch(inputValue);
        }
        public static bool IsAlphaNumericAndSpaceDotComma(string inputValue)
        {
            if (string.IsNullOrEmpty(inputValue)) return true;
            Regex r = new Regex(@"^[a-zA-Z0-9\ \.\,]*$");
            return r.IsMatch(inputValue);
        }

        public static bool IsAlphaNumeric(string inputValue)
        {
            if (string.IsNullOrEmpty(inputValue)) return true;
            Regex r = new Regex(@"^[a-zA-Z0-9]*$");
            return r.IsMatch(inputValue);
        }

        public static bool IsAlphabetAndSpace(string inputValue)
        {
            if (string.IsNullOrEmpty(inputValue)) return true;
            Regex r = new Regex(@"^[a-zA-Z\ ]*$");
            return r.IsMatch(inputValue);
        }

        public static bool IsAlphaNumericAndOthers1(string inputValue)
        {
            //-Allow alphanumeric, (, ), /, hyphen(-), comma(,), dot(.), @, and &
            if (string.IsNullOrEmpty(inputValue)) return true;
            Regex r = new Regex(@"^[a-zA-Z0-9\(\)\.\,\-\/\@\&\ \:]*$");
            return r.IsMatch(inputValue);
        }

        public static bool IsAlphaNumericAndOthers2(string inputValue)
        {

            if (string.IsNullOrEmpty(inputValue)) return true;
            Regex r = new Regex(@"^[a-zA-Z0-9\(\)\.\,\-\/\ \:\@\&]*$");
            return r.IsMatch(inputValue);
        }

        public static bool IsAlphaNumericAndOthers3(string inputValue)
        {
            //slash( / ), hyphen( - ), comma( , ), dot ( . ), @, space( ), & and doubel dot( : ) 
            if (string.IsNullOrEmpty(inputValue)) return true;
            Regex r = new Regex(@"^[a-zA-Z0-9\.\/\-\,\@\ \&\:]*$");
            return r.IsMatch(inputValue);
        }

        public static string GetUpdatePath()
        {
            string path = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath +
                          Constants.MasterPath;

            return path;
        }

        #region SeparateText
        public static List<string> SeparateText(string sValueText, int lenList, int MaxLength)
        {
            List<string> listString = new List<string>();
            //int MaxLengthPaper = 80;
            int nCurrPos = 0;
            int nPos = 0;

            for (int i = 0; i < lenList; i++)
            {
                var result = "";
                if (i == 0)
                {
                    nPos = FormatOffDesc(ref result, sValueText, MaxLength);

                }
                else
                    nPos = FormatOffDesc(ref result, sValueText.Substring(nCurrPos, sValueText.Length - nCurrPos), MaxLength);

                listString.Add(result);

                if (nPos <= 0)
                    break;
                nCurrPos = nCurrPos + nPos + 1;
            }

            for (int j = listString.Count; j < lenList; j++)
            {
                listString.Add("");
            }

            return listString;
        }

        private static int FormatOffDesc(ref string sdest, string sSource, int maxLen)
        {
            int nPos, nLen, nPrePos;

            nLen = sSource.Length;
            if (nLen > maxLen)
                nLen = maxLen;
            nPos = FindString(sSource, " ", nLen);
            if (nPos > 0)
                sdest = sSource.Substring(0, nPos);
            else
                sdest = sSource;

            nLen = sdest.Trim().Length;

            if (nLen > maxLen)
            {
                nPrePos = nLen;
                nLen = maxLen;
                //nPos = GetNextChar(sSource, nLen, nPrePos);
                nPos = GetPrevChar(sSource, nPrePos);
                if (nPos <= 0)
                    nPos = nLen;
                sdest = sSource.Substring(0, nPos);
            }

            return nPos;
        }

        private static int FindString(string source, string target, int startPos)
        {
            int srcLen = source.Length;

            int iResult = -1;
            if (startPos < srcLen)
            {
                for (int i = startPos; i < srcLen; i++)
                {
                    if (source.Substring(i, 1) == target)
                    {
                        iResult = i;
                        break;
                    }
                }
            }

            return iResult;
        }

        private static int GetNextChar(string sSource, int nLen, int nPrePos)
        {
            int nPos = 0;

            nPos = FindString(sSource, "/", nLen);
            if (nPos <= 0 || nPos > nPrePos)
            {
                nPos = FindString(sSource, ",", nLen);
                if (nPos <= 0 || nPos > nPrePos)
                {
                    nPos = FindString(sSource, ".", nLen);
                    if (nPos <= 0 || nPos > nPrePos)
                    {
                        nPos = FindString(sSource, "\\", nLen);
                        if (nPos <= 0 || nPos > nPrePos)
                        {
                            nPos = FindString(sSource, "|", nLen);
                            if (nPos <= 0 || nPos > nPrePos)
                            {
                                nPos = FindString(sSource, "-", nLen);
                                if (nPos <= 0 || nPos > nPrePos)
                                {
                                    nPos = FindString(sSource, " ", nLen);
                                    if (nPos <= 0 || nPos > nPrePos)
                                        nPos = GetPrevChar(sSource, nPrePos);
                                }
                            }
                        }
                    }
                }
            }

            return nPos;
        }
        private static int FindReverseString(string source, string target, int startPos)
        {
            int srcLen = source.Length;

            int iResult = -1;
            if (startPos <= srcLen)
            {
                //for (int i = startPos; i < 0; i--)
                for (int i = startPos - 1; i > 0; i--)
                {
                    if (source.Substring(i, 1) == target)
                    {
                        iResult = i;
                        break;
                    }
                }
            }

            return iResult;
        }

        private static int GetPrevChar(string sSource, int nPrePos)
        {
            int nPos = 0;

            nPos = FindReverseString(sSource, " ", nPrePos);
            if (nPos <= 0 || nPos > nPrePos)
            {
                nPos = FindReverseString(sSource, ",", nPrePos);
                if (nPos <= 0 || nPos > nPrePos)
                {
                    nPos = FindReverseString(sSource, ".", nPrePos);
                    if (nPos <= 0 || nPos > nPrePos)
                    {
                        nPos = FindReverseString(sSource, "\\", nPrePos);
                        if (nPos <= 0 || nPos > nPrePos)
                        {
                            nPos = FindReverseString(sSource, "|", nPrePos);
                            if (nPos <= 0 || nPos > nPrePos)
                            {
                                nPos = FindReverseString(sSource, "-", nPrePos);
                                if (nPos <= 0 || nPos > nPrePos)
                                {
                                    nPos = FindReverseString(sSource, "/", nPrePos);
                                    if (nPos <= 0 || nPos > nPrePos)
                                        nPos = nPrePos;
                                }
                            }
                        }
                    }
                }
            }

            return nPos;
        }


        #endregion

        public static CardInfoBean ReadMyKadInfoNew(string stringJsonFormat)
        {
            var cardInfo = new CardInfoBean();
            try
            {
                cardInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<CardInfoBean>(stringJsonFormat);
                cardInfo.IsSuccessRead = true;
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("ReadMyKadInfo Error : " + ex.Message);
                cardInfo.IsSuccessRead = false;
                cardInfo.Message = "Error Read MyKad, Error : " + ex.Message;
            }

            return cardInfo;
        }

        public static CardInfoDto ReadMyKadInfo(string stringJsonFormat)
        {
            var cardInfo = new CardInfoDto();
            try
            {
                cardInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<CardInfoDto>(stringJsonFormat);
                cardInfo.IsSuccessRead = true;
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("ReadMyKadInfo Error : " + ex.Message);
                cardInfo.IsSuccessRead = false;
                cardInfo.Message = "Error Read MyKad, Error : " + ex.Message;
            }

            return cardInfo;
        }

        public static CardInfoDto SetMyCardAddress(CardInfoDto cardDto)
        {
            var listAddress = new List<string>();
            string address1 = cardDto.address1;
            string address2 = cardDto.address2;
            string address3 = cardDto.address3;

            var address = string.Format("{0} {1}", cardDto.address1, cardDto.address2);
            var addressPostCodeCity = string.Format("{0} {1} {2}", cardDto.postcode, cardDto.city, cardDto.state);


            if (string.IsNullOrEmpty(cardDto.address3))
            {
                listAddress = GeneralBll.SeparateText(address, 2, Constants.MaxLengthCompound3Address);
                address1 = listAddress[0];
                if (string.IsNullOrEmpty(listAddress[1]))
                    address2 = addressPostCodeCity;
                else
                {
                    address2 = listAddress[1];
                    address3 = addressPostCodeCity;
                }
            }
            else
            {
                if (address.Length <= 80)
                {
                    address1 = address;
                    address2 = cardDto.address3;
                    address3 = addressPostCodeCity;
                }
                else
                {
                    address = string.Format("{0} {1} {2}", cardDto.address1, cardDto.address2, cardDto.address3);

                    var listString = GeneralBll.SeparateText(address, 2, Constants.MaxLengthCompound3Address);
                    address1 = listString[0].Trim();
                    address2 = listString[1].Trim();
                    address3 = addressPostCodeCity;
                }

            }

            cardDto.address1 = address1;
            cardDto.address2 = address2;
            cardDto.address3 = address3;
            return cardDto;
        }

        public static List<string> SetFhotoBackByCompoundNumber(string compoundNumber)
        {

            var listFileImage = new List<string>();

            if (!string.IsNullOrEmpty(compoundNumber))
            {
                for (int i = 1; i <= Constants.MaxPhoto; i++)
                {
                    string fname = compoundNumber + i.ToString("0") + ".jpg";


                    if (IsFileExist(GetInternalImagePath() + fname, true))
                        listFileImage.Add(GetInternalImagePath() + fname);
                    else
                        break;

                }
            }

            return listFileImage;
        }

        public static SplitDto GetSplitData(string data, char separator)
        {
            var output = new SplitDto();
            output.Code = data;

            var list = data.Split(separator).ToList();
            //if (list.Count >= 2)
            //{
            //    output.Code = list[0];

            //    output.Description = list[1];
            //}
            output.Description = "";

            for (int i = 0; i <= list.Count - 1; i++)
            {
                if (i == 0)
                    output.Code = list[i];
                else if (i == list.Count)
                    output.Description += list[i];
                else
                    output.Description += list[i] + " ";
            }
            return output;
        }

        public static List<CompDescDto> GetCompDescByActAndOfdCode(string act, string ofdCode)
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.CompDesc;

            var listCompDesc = new List<CompDescDto>();
            if (!System.IO.File.Exists(strFullFileName))
                return listCompDesc;

            return
                CompDescAccess.GetCompDescAccess(strFullFileName)
                    .Where(c => c.ActCode == act && c.OfdCode == ofdCode)
                    .ToList();
        }

        public static void InitFileSplashScreen()
        {

            var listTransFile = GetListTransFiles();

            var pathSource = GetTransPath();
            foreach (var transFile in listTransFile)
            {
                if (!IsFileExist(pathSource + transFile, true))
                    CreateNewFile(pathSource + transFile);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input">dd/MM/yyyy</param>
        /// <returns></returns>
        public static string ConvertDateToYyyyMmDdFormat(string input)
        {
            if (input.Length != 10)
                return input;
            //mm/dd/yyyy
            return input.Substring(6, 4) + input.Substring(3, 2) + input.Substring(0, 2);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input">mm/dd/yyyy</param>
        /// <returns></returns>
        public static string ConvertDateToddMMyyyyFormat(string input)
        {
            if (input.Length != 10)
                return input;

            return input.Substring(3, 2) + input.Substring(0, 2) + input.Substring(6, 4);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input">hh:MM tt</param>
        /// <returns></returns>
        public static string ConvertTimeToHhMmFormat(string input)
        {
            if (input.Length >= 5)
            {
                if (input.Contains("PM"))
                {
                    var hour = Convert.ToInt32(input.Substring(0, 2)) + 12;
                    var menit = input.Substring(3, 2);
                    return hour.ToString("00") + menit;
                }

                return input.Substring(0, 5).Replace(":", "");
            }

            return input;
        }

        public static DateTime AddDaysExcludeWeekEnd(DateTime input, int nNumDay)
        {
            /*
            int i = 1;
            char cDate[9], buf[20];
            int nDayofweek, iYear, iMonth, iDay;
            CStr str;

            while (i < numdays)
            {
                AddDate(datestr, 1, cDate);
                iYear = atoi(str.field(buf, cDate, 4));
                iMonth = atoi(str.field(buf, cDate + 4, 2));
                iDay = atoi(str.field(buf, cDate + 6, 2));
                nDayofweek = Dayofweek(iDay, iMonth, iYear);
                if (nDayofweek != 0 && nDayofweek != 6)
                    i++;

                strcpy(datestr, cDate);
            }
            return (datestr);
*/

            int i = 1;
            string DayOfWeek = "";

            while (i < nNumDay)
            {
                input = input.AddDays(1);
                DayOfWeek = input.DayOfWeek.ToString();
                if (DayOfWeek != "Sunday" && DayOfWeek != "Saturday")
                    i++;
            }

            return input;
        }

        public static bool IsOnline()
        {

            Runtime runtime = Runtime.GetRuntime();
            try
            {
                Java.Lang.Process ipProcess = runtime.Exec("/system/bin/ping -c 1 8.8.8.8");
                int exitValue = ipProcess.WaitFor();
                return (exitValue == 0);
            }
            catch (Java.IO.IOException e)
            {
                LogFile.WriteLogFile("IOException General IsOnline : " + e.StackTrace);
                //e.PrintStackTrace();
            }
            catch (InterruptedException e)
            {
                LogFile.WriteLogFile("InterruptedException General IsOnline : " + e.StackTrace);
                e.PrintStackTrace();
            }

            return false;

        }


        //public static bool IsInternetAvailable()
        //{
        //    var current = Connectivity.NetworkAccess;
        //    return current == NetworkAccess.Internet;
        //}
        public static ActiveParkingDto GetActiveParking(string carNumber, InfoDto info)
        {
            //var info = InfoBll.GetInfoDto();


            //format
            //<car number>|<handheld id>|<enforcer id>|<council>
            string input = carNumber + "|" + info.DolphinId + "|" + info.EnforcerId + "|MBIP";

            var result = new ActiveParkingDto();
            result.ScanDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            if (!IsOnline())
            {
                result.ReturnResponse = Enums.ParkingStatus.ErrorConnection;
                LogFile.WriteLogFile("GetActiveParking : No Internet Connection, CarNumber : " + carNumber);
                return result;
            }

            var configDto = GetConfig();
            if (configDto == null)
            {
                throw new Exception("Config Not Found");
            }

            var insService = new WebService(configDto.ServiceUrl, configDto.ServiceUser, configDto.ServicePassword);

            for (int i = 1; i <= Constants.MaxRetry; i++)
            {

                result = insService.GetActiveParking(carNumber, input, configDto.ServiceAppletUrl);
                if (result.ReturnResponse == Enums.ParkingStatus.Error)
                {
                    //FileSystemAndroid.WriteLogFile("GetActiveParking : start sleep");
                    Java.Lang.Thread.Sleep(Constants.SleepRetryActiveParking);
                    //FileSystemAndroid.WriteLogFile("GetActiveParking : finish sleep");
                }
                else if (result.ReturnResponse == Enums.ParkingStatus.ErrorConnection)
                {
                    result.ReturnResponse = Enums.ParkingStatus.ErrorConnection;
                    break; ;

                }
                else
                    break;
            }
            insService = null;

            return result;//insService.GetActiveParking(carNumber,input);
        }

        public static ActiveParkingDto GetActiveParking_WebService(string carNumber, InfoDto info)
        {
            string input = carNumber + "|" + info.DolphinId + "|" + info.EnforcerId + "|" + info.Council;

            var result = new ActiveParkingDto();

            if (!IsOnline())
            {
                result.ReturnResponse = Enums.ParkingStatus.ErrorConnection;
                LogFile.WriteLogFile("GetActiveParking : No Internet Connection, CarNumber : " + carNumber);
                return result;
            }

            var configDto = GetConfig();
            if (configDto == null)
            {
                throw new Exception("Config Not Found");
            }

            //var insService = new WebService(configDto.ServiceUrl, configDto.ServiceUser, configDto.ServicePassword);
            for (int i = 1; i <= Constants.MaxRetry; i++)
            {

                //result = insService.GetActiveParking(carNumber, input, configDto.ServiceAppletUrl);
                result = System.Threading.Tasks.Task.Run(async () => await HttpClientService.GetActiveParking(configDto.ServiceAppletUrl, info.EnforcerId, carNumber)).Result;
                if (result.ReturnResponse == Enums.ParkingStatus.Error)
                {
                    Java.Lang.Thread.Sleep(Constants.SleepRetryActiveParking);
                }
                else if (result.ReturnResponse == Enums.ParkingStatus.ErrorConnection)
                {
                    result.ReturnResponse = Enums.ParkingStatus.Error;
                    return result;

                }
                else
                    return result;
            }
            return result;
        }

        public static UPSBActiveParkingDto GetActiveParking_UPSB(string carNumber)
        {
            var result = new UPSBActiveParkingDto();

            if (!IsOnline())
            {
                result.ReturnResponse = Enums.ParkingStatus.ErrorConnection;
                LogFile.WriteLogFile("GetActiveParking_UPSB : No Internet Connection, CarNumber : " + carNumber);
                return result;
            }

            var configDto = GetConfig();
            if (configDto == null)
            {
                throw new Exception("Config Not Found");
            }

            UPSBInput input = new UPSBInput();
            input.AppId = configDto.AppID;
            input.CompanyId = configDto.CompanyID;
            input.ParkCode = configDto.ParkCode;
            input.VplNumber = carNumber;

            try
            {
                for (int i = 1; i <= Constants.MaxRetry; i++)
                {
                    result = System.Threading.Tasks.Task.Run(async () => await HttpClientService.GetActiveParkingUPSB(configDto.ServiceUPSBUrl, input)).Result;
                    if (result.ReturnResponse == Enums.ParkingStatus.Error)
                    {
                        Java.Lang.Thread.Sleep(Constants.SleepRetryActiveParking);
                    }
                    else if (result.ReturnResponse == Enums.ParkingStatus.ErrorConnection)
                    {
                        result.ReturnResponse = Enums.ParkingStatus.Error;
                        return result;

                    }
                    else
                        return result;
                }
            }

            catch (Exception ex)
            {
                LogFile.WriteLogFile("GetActiveParking_UPSB : error : " + ex.Message);

            }
            return result;
        }
        public static int ConvertTimeToMinutes(string input)
        {
            int iReturn = 0;

            int hours = Convert.ToInt16(input.Substring(0, 2));
            int minutes = Convert.ToInt16(input.Substring(2, 2));
            iReturn = (hours * 12) + minutes;
            return iReturn;

        }

        /// <summary>
        /// 
        /// </summary>          0123456789012345678
        /// <param name="input">dd/mm/yyyy hh:mm:ss</param>
        /// <returns>= hhmmss
        public static string GetTimeHHMMSS(string input)
        {
            string sReturn = "";

            if (input.TrimEnd() != "")
            {
                string hour = input.Substring(11, 2);
                string minute = input.Substring(14, 2);
                string second = input.Substring(17, 2);
                sReturn = hour + minute + second;
            }
            return sReturn;

        }

        public static void SaveCompressImage(string path)
        {
            //rename for copy file
            string pathCopy = path.Replace(".jpg", "Copy");
            pathCopy += ".jpg";

            //copy first 
            File.Copy(path, pathCopy, true);

            try
            {
                //compress copy file
                //Bitmap resizedBitmap = pathCopy.LoadAndResizeBitmap(480, 640);

                //Bitmap OriBitmap = pathCopy.LoadBitmap();
                //Bitmap OriBitmap = pathCopy.LoadAndResizeBitmap(480, 640);
                Bitmap OriBitmap = BitmapFactory.DecodeFile(pathCopy);
                Bitmap resizedBitmap = Bitmap.CreateScaledBitmap(OriBitmap, (int)(OriBitmap.Width / 2), (int)(OriBitmap.Height / 2), false);
                var stream = new FileStream(pathCopy, FileMode.Create);
                resizedBitmap.Compress(Bitmap.CompressFormat.Jpeg, 25, stream);
                stream.Close();
                stream.Dispose();
                GC.Collect();
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("err compress : " + ex.Message);
                LogFile.WriteLogFile("path : " + path);
            }

            //check file size
            var length = new System.IO.FileInfo(pathCopy).Length;
            if (length > 0)
            {
                //copy back original
                File.Copy(pathCopy, path, true);
            }

            //remove copy file
            if (File.Exists(pathCopy))
            {
                File.Delete(pathCopy);
            }
        }

        public static void PausedServiceSendCompound()
        {
            SharedPreferences.SaveString(SharedPreferencesKeys.SendCompoundService, Constants.PauseCompoundService);
        }

        public static void ResumeServiceSendCompound()
        {
            SharedPreferences.SaveString(SharedPreferencesKeys.SendCompoundService, string.Empty);
        }

        public static bool IsSendServiceSendCompoundAllow()
        {
            return SharedPreferences.GetString(SharedPreferencesKeys.SendCompoundService) !=
                   Constants.PauseCompoundService;
        }

        public static bool IsPrintSignature()
        {
            return SharedPreferences.GetString(SharedPreferencesKeys.PrintSignature) !=
                   Constants.PrintSignature;
        }

        public static void SetPrintSignature(bool bFlag)
        {
            if (bFlag)
                SharedPreferences.SaveString(SharedPreferencesKeys.PrintSignature, Constants.PrintSignature);
            else
                SharedPreferences.SaveString(SharedPreferencesKeys.PrintSignature, string.Empty);

        }
        public static void SaveImageStream(Stream streamFile, string fileName)
        {
            try
            {
                var bitmap = BitmapFactory.DecodeStream(streamFile);

                var stream = new FileStream(fileName, FileMode.Create);
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                stream.Close();
                stream.Dispose();
            }

            catch (Exception ex)
            {
                LogFile.WriteLogFile("SaveImageStream : " + ex.Message);
                LogFile.WriteLogFile("Filename  : " + fileName);
            }
        }

        public static string GenerateKey(string value)
        {
            HMACSHA256 hmac = new HMACSHA256(Encoding.ASCII.GetBytes(Constants.Key));
            byte[] resultByte = hmac.ComputeHash(Encoding.ASCII.GetBytes(value));
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < resultByte.Length; i++)
            {
                sb.Append(resultByte[i].ToString("X2"));
            }
            return sb.ToString().ToLower();
        }

        public static int getPhotoCount(string strcompno)
        {
            int nCount = 0;
            string fname;

            for (int i = 0; i <= Constants.MaxPhoto; i++)
            {
                fname = strcompno + i.ToString("0") + ".jpg";
                //if (GeneralBll.IsFileExist(Constants.ImgsPath + fname, false))
                if (GeneralBll.IsFileExist(GeneralAndroidClass.GetExternalStorageDirectory() + Constants.DCIMPath + Constants.ImgsPath + fname, true))
                        nCount += 1;
            }
            fname = strcompno + "final0.jpg";
            //if (GeneralBll.IsFileExist(Constants.ImgsPath + fname, false))
            if (GeneralBll.IsFileExist(GeneralAndroidClass.GetExternalStorageDirectory() + Constants.DCIMPath + Constants.ImgsPath + fname, true))
                nCount += 1;

            return nCount;
        }

        public static int getTotalPhotoCount()
        {
            int nCount = 0;
            //var pathImageSource = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath + Constants.ImgsPath;
            var pathImageSource = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.DCIMPath + Constants.ImgsPath;
            var di = new DirectoryInfo(pathImageSource);
            FileInfo[] rgFiles = di.GetFiles("*.jpg");


            foreach (FileInfo fi in rgFiles)
            {
                if (File.Exists(fi.FullName))
                    nCount++;            }

            return nCount;
        }

        public static int GetVersionIntFromString(string fileName)
        {
            var versionNumber = 0;
            try
            {
                if (!string.IsNullOrEmpty(fileName))
                {
#if PM90
                    fileName = fileName.ToLower().Replace("mbipandroid.mbipandroid_", string.Empty).Replace(".apk", string.Empty);
#else
                    fileName = fileName.ToLower().Replace("mbipandroidhik.mbipandroidhik_", string.Empty).Replace(".apk", string.Empty);
#endif
                }
                return Convert.ToInt32(fileName);
            }
            catch (Exception)
            {
            }

            return versionNumber;
        }
        public static string ProcessRcvData(byte[] buffer)
        {
            string s1 = "";
            char c;
            int i = 0, asciival = 0;

            for (i = 0; i < buffer.Length; i++)
            {
                c = (char)buffer[i];
                asciival = (int)c;
                if ((asciival >= 48 && asciival <= 57) || (asciival >= 65 && asciival <= 90) || (asciival >= 97 && asciival <= 122) || asciival == 32)
                    s1 = s1 + c.ToString();
            }

            //            string s = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);

            return s1;
        }

        public static ActiveReserveParkingDto GetReserveParking(string streetid, string zoneid, string lotno)
        {
            var result = new ActiveReserveParkingDto();

            if (!IsOnline())
            {
                result.ReturnResponse = Enums.ParkingStatus.ErrorConnection;
                LogFile.WriteLogFile("GetReserveParking : No Internet Connection, Street : " + streetid);
                return result;
            }

            var configDto = GetConfig();
            if (configDto == null)
            {
                throw new Exception("Config Not Found");
            }

            var insService = new WebService(configDto.ServiceUrl, configDto.ServiceUser, configDto.ServicePassword);

            for (int i = 1; i <= Constants.MaxRetry; i++)
            {

                result = insService.GetSeasonParkList(zoneid, streetid, lotno, configDto.ServiceAppletUrl);
                if (result.status == "N")
                    Java.Lang.Thread.Sleep(Constants.SleepRetryActiveParking);
                else
                    return result;
            }

            return result;

        }
        public static bool IsRegisterPrinter(string strAddress)
        {
            string strPrinterAddress = "";
            bool bReturn = false;

            if (!GeneralBll.IsFileExist(Constants.LASTTRANS, false))
                return bReturn;

            string fullpathfileName = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath + Constants.LASTTRANS;

            var objInfo = new StreamReader(fullpathfileName);

            try
            {
                string sLine;
                int len = 0;

                while ((sLine = objInfo.ReadLine()) != null)
                {
                    if (sLine.Length > 0)
                    {
                        strPrinterAddress = GeneralUtils.ValidateRecordByLine(sLine, 0, 18, ref len).Trim();
                        if (strPrinterAddress == strAddress)
                        {
                            bReturn = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("Error IsRegisterPrinter : " + ex.Message);
            }
            finally
            {

                objInfo.Close();
                objInfo.Dispose();
            }

            return bReturn;
        }

        public static void RegisterPrinter(string strAddress)
        {

            string sLine = "";
            string strFullFileName;

            sLine = GeneralUtils.SetLine(sLine, strAddress, 18);

            sLine += "\r\n";

            strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath + Constants.LASTTRANS;

            var objInfo = new StreamWriter(strFullFileName, true);

            objInfo.Write(sLine);
            objInfo.Close();
            objInfo.Dispose();
        }
        public static string SetPrintDataToString(PrintDataDto input)
        {
            string json = string.Empty;

            if (input != null && input.DataItems.Any())
            {
                json = JsonConvert.SerializeObject(input);

            }

            return json;
        }

        public static PrintDataDto GetListPrintDataString(string printData)
        {
            var result = new PrintDataDto();

            if (!string.IsNullOrEmpty(printData))
            {
                result = JsonConvert.DeserializeObject<PrintDataDto>(printData);
            }


            return result;
        }
        public static void CleanRefListDataPrint()
        {
            //clean preferences
            SharedPreferences.SaveString(SharedPreferencesKeys.ListPrintData, "");
        }
    }
}