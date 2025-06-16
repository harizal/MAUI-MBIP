using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.BusinessObject.DTOs.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AndroidCompound5.Classes;
using static Android.Content.ClipData;
using AndroidCompound5.AimforceUtils;

namespace AndroidCompound5
{
    public class CompoundBll
    {
        public static OutputDto GenerateCompoundNumber(InfoDto infoDto)
        {
            var output = new OutputDto();

            if (infoDto.CurrComp > 999)
            {
                output.Result = false;
                output.Message = "Running number overflow";
                return output;
            }

            string todaydate = GeneralBll.GetLocalDateTime().ToString("yyyyMMdd"); ;
            if (todaydate.CompareTo(infoDto.CurrDate) > 0)
            {
                if (ManageTransAfterUpload(infoDto.DolphinId, infoDto.CurrDate))
                {
                    LogFile.WriteLogFile("Start InitTrans", Enums.LogType.Info);
                    GeneralBll.InitFileTrans();
                    LogFile.WriteLogFile("Done InitTrans", Enums.LogType.Info);

                    if (ManageImageFolderAfterUpload(infoDto.CurrDate))
                    {
                    }//end if ManageImageFolderAfterUpload
                }
                infoDto.CurrComp = 0;
                infoDto.CurrDate = todaydate;
                infoDto.CompCnt = 0;
                infoDto.NoteCnt = 0;
                infoDto.NoteSize = 0;
                infoDto.PhotoCnt = 0;
                InfoBll.UpdateInfo(infoDto, Enums.FormName.CompoundBll);
            }

            string sTemp = (infoDto.CurrComp + 1).ToString("000");
            //            string compNo = "L"+ infoDto.DolphinId +  sTemp;
            string compNo = Constants.DevicePrefix + infoDto.DolphinId + infoDto.CurrDate + sTemp;

            if (IsCompoundNumberExist(compNo))
            {
                output.Message = "Duplicate Compound Number!";
                return output;
            }

            output.Result = true;
            output.Message = compNo;

            return output;
        }

        public static bool ResetCompoundFile(InfoDto infoDto)
        {
            bool bFlag = false;
            if (infoDto.CurrComp > 999)
            {
                LogFile.WriteLogFile("ResetCopoundFIles : Counter Overflow", Enums.LogType.Info,Enums.FileLogType.CompoundService);
                return bFlag;
            }

            string todaydate = GeneralBll.GetLocalDateTime().ToString("yyyyMMdd"); ;
            if (todaydate.CompareTo(infoDto.CurrDate) > 0)
            {
                if (ManageTransAfterUpload(infoDto.DolphinId, infoDto.CurrDate))
                {
                    LogFile.WriteLogFile("Start InitTrans", Enums.LogType.Info);
                    GeneralBll.InitFileTrans();
                    LogFile.WriteLogFile("Done InitTrans", Enums.LogType.Info);

                    if (ManageImageFolderAfterUpload(infoDto.CurrDate))
                    {
                    }//end if ManageImageFolderAfterUpload
                }
                infoDto.CurrComp = 0;
                infoDto.CurrDate = todaydate;
                infoDto.CompCnt = 0;
                infoDto.NoteCnt = 0;
                infoDto.NoteSize = 0;
                infoDto.PhotoCnt = 0;
                InfoBll.UpdateInfo(infoDto, Enums.FormName.CompoundBll);
                LogFile.WriteLogFile("ResetCopoundFIles : Done", Enums.LogType.Info, Enums.FileLogType.CompoundService);
                bFlag = true;
            }

            return bFlag;
        }
        public static void ClearEscapeCompound(string compoundNumber)
        {
            //clear sign
            string sFileName = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath +
                               Constants.SignPath + compoundNumber + Constants.SignName + ".png";

            if (File.Exists(sFileName))
            {
                File.Delete(sFileName);
            }

            //clear photo
            var di = new DirectoryInfo(GeneralBll.GetInternalImagePath());
            FileInfo[] rgFiles = di.GetFiles(compoundNumber + "*.*");


            foreach (FileInfo fi in rgFiles)
            {
                if (File.Exists(fi.FullName))
                    File.Delete(fi.FullName);
            }

        }

        public static bool ProcessCompoundOnlineService()
        {
            bool  bReturn = false;

            if (!GeneralBll.IsSendServiceSendCompoundAllow())
                return bReturn;

            //GeneralBll.PausedServiceSendCompound();

            var listFileCompoundOnline = ListFileCompoundOnline();

            var configDto = GeneralBll.GetConfig();
            if (configDto == null)
            {
                LogFile.WriteLogFile("Get Config null", Enums.LogType.Info);
                GeneralBll.ResumeServiceSendCompound();
                return bReturn;
            }

            try
            {
                var svc = new WebService(configDto.ServiceUrl, configDto.ServiceUser, configDto.ServicePassword);

                bReturn = true;

                foreach (var fileCompoundOnline in listFileCompoundOnline)
                {
                    if (GeneralBll.IsSendServiceSendCompoundAllow())
                    {
                        if (GeneralBll.IsFileExist(fileCompoundOnline, true))
                        {
                            var compoundDto = CompoundAccess.GetCompoundAccess(fileCompoundOnline).FirstOrDefault();
                            if (compoundDto != null)
                            {

                                LogFile.WriteLogFile("Send Compound Online : " + compoundDto.CompNum, Enums.LogType.Info,
                                    Enums.FileLogType.CompoundService);
                                //send it web service
                                var message = svc.SendInsertCompound(compoundDto);

                                if (message == Constants.SendSuccess)
                                {
                                    if (SendImage2(compoundDto.CompNum, configDto.UrlPhoto))
                                        DeleteSendImagesFile(compoundDto.CompNum);

                                    LogFile.WriteLogFile("Send Compound Online Success : ", Enums.LogType.Info,
                                        Enums.FileLogType.CompoundService);
                                    //delete the file
                                    File.Delete(fileCompoundOnline);
                                }
                                else
                                {
                                    LogFile.WriteLogFile("Send Compound Online Failed, message : " + message,
                                        Enums.LogType.Info, Enums.FileLogType.CompoundService);
                                    bReturn = false;
                                }

                            }
                        }
                    }
                    else
                    {
                        GeneralBll.ResumeServiceSendCompound();
                        return bReturn;
                    }
                }
                return bReturn;
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("Send Compound Online Failed, message : " + ex.Message,Enums.LogType.Info, Enums.FileLogType.CompoundService);
                bReturn = false;
                return bReturn;
            }
        }

        public static bool ProcessGPSOnlineService(string serviceUrl, string serviceUser, string servicePassword)
        {
            int nCnt = 0;
            bool bReturn = true;
            string strFolder = GeneralAndroidClass.GetExternalStorageDirectory();
            strFolder += Constants.ProgramPath + Constants.TransPath;

            string strFullFileName = strFolder + Constants.GpsFil;

            var listGPS = GpsAccess.GetGPSAccess(strFullFileName).ToList();
            if (listGPS.Count <= 0)
                return bReturn;

            try
            {
                var svc = new WebService(serviceUrl, serviceUser, servicePassword);

                foreach (var gpsdto in listGPS)
                {
                    nCnt++;
                    var message = svc.SendGpsInfo(gpsdto);
                    LogFile.WriteLogFile("Send GPS Online : " + nCnt.ToString() + " " + gpsdto.ActivityDate + " " + gpsdto.ActivityTime);
                }
                return bReturn;
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("Send GPS Online Failed, message : " + ex.Message, Enums.LogType.Info, Enums.FileLogType.CompoundService);
                bReturn = false;
                return bReturn;
            }
        }
        public static bool ProcessEnquiryLogOnlineService(string serviceUrl, string serviceUser, string servicePassword)
        {
            int nCnt = 0;
            bool bReturn = true;
            string strFolder = GeneralAndroidClass.GetExternalStorageDirectory();
            strFolder += Constants.ProgramPath + Constants.TransPath;

            string strFullFileName = strFolder + Constants.ENQUIRYLOGFIL;

            var listEnquiryLog = EnquiryLogAccess.GetEnquiryLogAccess(strFullFileName).ToList();
            if (listEnquiryLog.Count <= 0)
                return bReturn;


            try 
            { 
                var svc = new WebService(serviceUrl, serviceUser, servicePassword);

                foreach (var enquirydto in listEnquiryLog)
                {
                    nCnt++;
                    var message = svc.SendEnquiryLogInfo(enquirydto);
                    LogFile.WriteLogFile("Send Enquiry Log Online : " + nCnt.ToString() + " Car Number : " + enquirydto.CarNo + " message : " + message.ToString() + " dateTime : " + enquirydto.Date + " " + enquirydto.Time);
                }
                return bReturn;
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("Send Enquiry Log Online Failed, message : " + ex.Message, Enums.LogType.Info, Enums.FileLogType.CompoundService);
                bReturn = false;
                return bReturn;
            }
        }
        private static void DeleteSendImagesFile(string strNokmp)
        {
            var pathImageSource = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath + Constants.SendOnlinePath + Constants.ImgsPath;
            var di = new DirectoryInfo(pathImageSource);
            FileInfo[] rgFiles = di.GetFiles(strNokmp + "*.*");

            foreach (FileInfo fi in rgFiles)
            {
                if (File.Exists(fi.FullName))
                {
                    File.Delete(fi.FullName);
                    LogFile.WriteLogFile("Delete Image File : " + fi.FullName);
                }
            }

            return;
        }



        private static void SendImage(CompoundDto compoundDto, string url)
        {
            try
            {
                var compoundImages = new List<CompoundImageDto>();
                var imageList = GeneralBll.GetListFileImageNameOnLineByCompoundNumber(compoundDto.CompNum);

                var seq = 1;
                foreach (var item in imageList)
                {
                    byte[] imageArray = File.ReadAllBytes(item);
                    var fileSize = new FileInfo(item).Length.ToString();
                    compoundImages.Add(new CompoundImageDto
                    {
                        councilid = "MBIP",
                        compoundNo = compoundDto.CompNum,
                        compoundDate = compoundDto.CompDate,
                        compoundTime = compoundDto.CompTime,
                        seq = seq.ToString(),
                        filename = Path.GetFileName(item),
                        filesize = fileSize,
                        image = Convert.ToBase64String(imageArray),
                        secretkey = "Mp5N#@2O!8"
                    });
                    seq++;
                }

                var result = Task.Run(async () => await new AppService().PostAsync(compoundImages, url)).Result;

                if (result != string.Empty)
                {

                }
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("SendImage Error : " + ex.Message);

            }
        }

        private static bool SendImage2(string strnokmp, string url)
        {
            bool bFlag = false;

            try
            {
                var compoundImages = new List<CompoundImageDto>();
                var imageList = GeneralBll.GetListFileImageNameOnLineByCompoundNumber(strnokmp);

                var seq = 1;
                foreach (var item in imageList)
                {
                    byte[] imageArray = File.ReadAllBytes(item);
                    var fileSize = new FileInfo(item).Length.ToString();

                    compoundImages.Add(new CompoundImageDto
                    {
                        councilid = "MBIP",
                        compoundNo = strnokmp,
                        compoundDate = GeneralBll.GetLocalDateTime().ToString("yyyyMMdd"),
                        compoundTime = GeneralBll.GetLocalDateTime().ToString("hhmmss"),
                        seq = seq.ToString(),
                        filename = Path.GetFileName(item),
                        filesize = fileSize,
                        image = Convert.ToBase64String(imageArray),
                        secretkey = "Mp5N#@2O!8"
                    });
                    seq++;
                }

                var result = Task.Run(async () => await new AppService().PostAsync(compoundImages, url)).Result;
                if (!string.IsNullOrEmpty(result))
                    bFlag = true;
                return bFlag;

            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("SendImage2 Error : " + ex.Message);
                bFlag = false;
                return bFlag;

            }
        }
        public static bool SendAllUnsendImage(string url)
        {
            bool bFlag = false;

            try
            {
                var nokmpist = GeneralBll.GetListAllUnSendImageNoCompOnLine();

                foreach (var item in nokmpist)
                {
                    if (SendImage2(item, url))
                        DeleteSendImagesFile(item);
                }

                return bFlag;

            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("SendAllUnsendImage Error : " + ex.Message);
                bFlag = false;
                return bFlag;

            }
        }
        public static List<string> ListFileCompoundOnline()
        {
            var listFile = new List<string>();

            try
            {
                //collect from send compound online folder
                var dirCompoundOnline = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath +
                                   Constants.SendOnlinePath;
                var di = new DirectoryInfo(dirCompoundOnline);

                //add existing .db
                var rgFiles = di.GetFiles("*.*");
                listFile.AddRange(rgFiles.Select(fi => fi.FullName));
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("Error ListFileCompoundOnline : " + ex.Message, Enums.LogType.Error, Enums.FileLogType.CompoundService);
            }
            return listFile;
        }
        public static List<string> ListCompoundNumberOnline()
        {
            var listFile = new List<string>();

            try
            {
                //collect from send compound online folder
                var dirCompoundOnline = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath +
                                   Constants.SendOnlinePath;
                var di = new DirectoryInfo(dirCompoundOnline);

                var rgFiles = di.GetFiles("*.*");
                listFile.AddRange(rgFiles.Select(fi => fi.Name.Replace(".txt", "")));

            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("Error ListCompoundNumberOnline : " + ex.Message, Enums.LogType.Error, Enums.FileLogType.CompoundService);
            }
            return listFile;
        }

        public static bool IsCompoundNumberExist(string compoundNumber)
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.TransPath + Constants.CompoundFil;

            var listCompound = CompoundAccess.GetCompoundAccess(strFullFileName);
            return listCompound.Any(compoundDto => compoundDto.CompNum == compoundNumber);

        }
        //private static StructClass.compound_t ReadCompoundRecord(string fileName)
        //{
        //    if (!FileClass.IsFileExist(fileName, true))
        //        return new StructClass.compound_t();

        //    var objInfo = new StreamReader(fileName);

        //    try
        //    {
        //        string sLine;

        //        var readCompound = new StructClass.compound_t();

        //        while ((sLine = objInfo.ReadLine()) != null)
        //        {
        //            if (sLine.Length > 0)
        //            {
        //                //readCompound = ReadCompoundIntoDto(sLine);
        //                GeneralClass.ReadDataCompound(ref readCompound, sLine);
        //                break;

        //            }//end if
        //        }//end while

        //        return readCompound;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogBll.WriteLogFile(Enums.LogType.CompoundService, "Error ReadCompoundRecord : " + ex.Message);
        //        return new StructClass.compound_t();
        //    }
        //    finally
        //    {
        //        objInfo.Close();
        //        objInfo.Dispose();
        //    }
        //}

        //private static bool SendCompoundToWebService(StructClass.compound_t compoundData)
        //{
        //    try
        //    {
        //        var compoundInfo = new CompoundInfo();

        //        //general
        //        compoundInfo.CompoundNumber = compoundData.CompNum;
        //        compoundInfo.CompoundType = compoundData.CompType;
        //        compoundInfo.Actcode = compoundData.ActCode;
        //        compoundInfo.Kodakta = compoundData.OfdCode;
        //        compoundInfo.Mukim = compoundData.Mukim;
        //        compoundInfo.Zone = compoundData.Zone;
        //        compoundInfo.street = compoundData.StreetCode;
        //        compoundInfo.streetDescr = compoundData.StreetDesc;
        //        compoundInfo.enforcer = compoundData.EnforcerId;
        //        compoundInfo.witness = compoundData.WitnessId1;
        //        compoundInfo.PubWitness = compoundData.PubWitness;
        //        compoundInfo.PrintCnt = compoundData.PrintCnt;
        //        compoundInfo.Tempatjadi = compoundData.Tempatjadi;
        //        compoundInfo.IssueDate = compoundData.IssueDate;
        //        compoundInfo.IssueTime = compoundData.IssueTime;
        //        compoundInfo.Kadar = compoundData.Kadar;
        //        compoundInfo.NamaPenerima = compoundData.NamaPenerima;
        //        compoundInfo.IcPenerima = compoundData.IcPenerima;
        //        compoundInfo.compDate = compoundData.CompDate;
        //        compoundInfo.compTime = compoundData.CompTime;
        //        compoundInfo.kodSalah = compoundData.OfdCode;

        //        switch (compoundData.CompType)
        //        {
        //            case GlobalClass.COMP_TYPE1:
        //                compoundInfo.CarType = compoundData.Type1.CarType;
        //                compoundInfo.CarTypeDesc = compoundData.Type1.CarTypeDesc;
        //                compoundInfo.CarColor = compoundData.Type1.CarColor;
        //                compoundInfo.CarColorDesc = compoundData.Type1.CarColorDesc;
        //                compoundInfo.RoadTax = compoundData.Type1.RoadTax;
        //                compoundInfo.parkingLot = compoundData.Type1.ParkingLot;
        //                compoundInfo.amnkmp = compoundData.Type1.CompAmt;
        //                compoundInfo.amnkmp2 = compoundData.Type1.CompAmt2;
        //                compoundInfo.amnkmp3 = compoundData.Type1.CompAmt3;
        //                compoundInfo.compDescr = compoundData.Type1.CompDesc;
        //                break;
        //            case GlobalClass.COMP_TYPE2:
        //                compoundInfo.CarType = compoundData.Type2.CarType;
        //                compoundInfo.CarTypeDesc = compoundData.Type2.CarTypeDesc;
        //                compoundInfo.CarColor = compoundData.Type2.CarColor;
        //                compoundInfo.CarColorDesc = compoundData.Type2.CarColorDesc;
        //                compoundInfo.RoadTax = compoundData.Type2.RoadTax;
        //                compoundInfo.amnkmp = compoundData.Type2.CompAmt;
        //                compoundInfo.amnkmp2 = compoundData.Type2.CompAmt2;
        //                compoundInfo.amnkmp3 = compoundData.Type2.CompAmt3;
        //                compoundInfo.DeliveryCode = compoundData.Type2.DeliveryCode;
        //                compoundInfo.compDescr = compoundData.Type2.CompDesc;
        //                compoundInfo.jenken = compoundData.Type2.Category;
        //                compoundInfo.noDaftar = compoundData.Type2.CarNum;
        //                break;
        //            case GlobalClass.COMP_TYPE3:
        //                compoundInfo.noRujukan = compoundData.Type3.Rujukan;
        //                compoundInfo.Company = compoundData.Type3.Company;
        //                compoundInfo.CompanyName = compoundData.Type3.CompanyName;
        //                compoundInfo.OffenderIc = compoundData.Type3.OffenderIc;
        //                compoundInfo.OffenderName = compoundData.Type3.OffenderName;
        //                compoundInfo.Alamat1 = compoundData.Type3.Alamat1;
        //                compoundInfo.alamat2 = compoundData.Type3.Alamat2;
        //                compoundInfo.alamat3 = compoundData.Type3.Alamat3;
        //                compoundInfo.amnkmp = compoundData.Type3.CompAmt;
        //                compoundInfo.amnkmp2 = compoundData.Type3.CompAmt2;
        //                compoundInfo.amnkmp3 = compoundData.Type3.CompAmt3;
        //                compoundInfo.DeliveryCode = compoundData.Type3.DeliveryCode;
        //                compoundInfo.compDescr = compoundData.Type3.CompDesc;
        //                break;
        //        }

        //        compoundInfo.Latitude = compoundData.GpsX;
        //        compoundInfo.Longitude = compoundData.GpsY;

        //        var webService = new WebServiceClass();
        //        //var result = webService.SendCompoundToService(compoundInfo) == GlobalClass.SendSuccess;

        //        var result = webService.SendCompoundToService(compoundInfo);

        //        return result.Response;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogBll.WriteLogFile(LogType.CompoundService, "Error SendCompoundToWebService1() : " + ex.Message);
        //        return false;
        //    }

        //}


        //public static void RemoveAllFilesCompoundOnline()
        //{

        //    var listFileCompoundOnline = ListFileCompoundOnline();

        //    foreach (var fileCompoundOnline in listFileCompoundOnline)
        //    {
        //        FileClass.DeleteFile(fileCompoundOnline, true);
        //    }

        //}

        private static bool ValidateFileCompound()
        {
            string strFolder = GeneralAndroidClass.GetExternalStorageDirectory();
            strFolder += Constants.ProgramPath + Constants.TransPath;

            string strFullFileName = strFolder + Constants.CompoundFil;
            if (!File.Exists(strFullFileName))
            {
                LogFile.WriteLogFile("File Not Exist : " + strFullFileName, Enums.LogType.Debug);
                return false;
            }

            //strFullFileName = strFolder + Constants.NoteFil;

            //if (!File.Exists(strFullFileName))
            //{
            //    LogFile.WriteLogFile("File Not Exist : " + strFullFileName, Enums.LogType.Debug);
            //    return false;
            //}

            //strFullFileName = strFolder + Constants.SitaFil;

            //if (!File.Exists(strFullFileName))
            //{
            //    LogFile.WriteLogFile("File Not Exist : " + strFullFileName, Enums.LogType.Debug);
            //    return false;
            //}

            return true;
        }

        public static void SaveNote(string noteValue, string compoundNumber, string enforcerId, string noteCode)
        {
            try
            {
                string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
                strFullFileName += Constants.ProgramPath + Constants.TransPath + Constants.NoteFil;

                var dateTime = GeneralBll.GetLocalDateTime();

                var noteDto = new NoteDto();
                noteDto.Deleted = Constants.RecActive;
                noteDto.NoteCode = noteCode;
                noteDto.CompNum = compoundNumber;
                noteDto.NoteDate = dateTime.ToString("yyyyMMdd");
                noteDto.NoteTime = dateTime.ToString("HHmm");
                noteDto.EnforcerId = enforcerId;
                noteDto.NoteDesc = noteValue;

                NoteAccess.AddNoteAccess(strFullFileName, noteDto);

                //update info note contain
                var info = InfoBll.GetInfo();
                info.NoteCnt += 1;
                InfoBll.UpdateInfo(info, Enums.FormName.CompoundBll);

            }
            catch (Exception ex)
            {

                LogFile.WriteLogFile("Error SaveNote : " + ex.Message, Enums.LogType.Error);
                LogFile.WriteLogFile("Stack Trace : " + ex.StackTrace, Enums.LogType.Error);
            }


        }

        private static bool IsDuplicateCompoundNumber(string strFullFileName, string compoundNumber)
        {
            var listCompound = CompoundAccess.GetCompoundAccess(strFullFileName);
            return listCompound.Any(compoundDto => compoundDto.CompNum == compoundNumber);
        }

        public static int SaveCompound(CompoundDto compoundDto)
        {
            if (!ValidateFileCompound()) return Constants.Failed;


            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.TransPath + Constants.CompoundFil;

            if (IsDuplicateCompoundNumber(strFullFileName, compoundDto.CompNum))
                return Constants.DuplicateCompoundNumber;

            string sLine = ReadCompoundDtoIntoString(compoundDto);

            bool isSaved = false;

            for (int i = 1; i <= Constants.MaxRetryCompound; i++)
            {

                isSaved = CompoundAccess.AddCompoundAccess(strFullFileName, sLine, compoundDto.CompNum);
                if (isSaved)
                    break;

                Thread.Sleep(Constants.SleepRetryCompound);
            }

            if (!isSaved) return Constants.Failed;


            //save note
            if (!string.IsNullOrEmpty(compoundDto.NoteDesc))
            {
                string noteCode = "  ";
                SaveNote(compoundDto.NoteDesc, compoundDto.CompNum, compoundDto.EnforcerId, noteCode);
            }

            // update info
            var info = InfoBll.GetInfo();

            info.CurrComp += 1;
            //if (compoundDto.CompType == Constants.CompType4)
            //{
            //    info.SitaCnt += 1;
            //    //save sita
            //    SaveSita(compoundDto.Compound4Type.ListSita);
            //}
            //else
            info.CompCnt += 1;

            info.PhotoCnt += compoundDto.TotalPhoto;
            InfoBll.UpdateInfo(info, Enums.FormName.CompoundBll);


            //save for compound online
            strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.SendOnlinePath + compoundDto.CompNum + ".txt";
            CompoundAccess.AddCompoundAccess(strFullFileName, sLine, compoundDto.CompNum);


            return Constants.Success;

        }

        private static string ReadCompoundDtoIntoString(CompoundDto compoundDto)
        {
            string sLine = "";
            compoundDto.TempohDate = "        ";
            sLine = GeneralUtils.SetLine(sLine, compoundDto.Deleted, 1);
            sLine = GeneralUtils.SetLine(sLine, compoundDto.CompNum, 20);
            sLine = GeneralUtils.SetLine(sLine, compoundDto.CompType, 1);
            sLine = GeneralUtils.SetLine(sLine, compoundDto.ActCode, 10);
            sLine = GeneralUtils.SetLine(sLine, compoundDto.OfdCode, 10);
            sLine = GeneralUtils.SetLine(sLine, compoundDto.Mukim, 6);
            sLine = GeneralUtils.SetLine(sLine, compoundDto.Zone, 10);
            sLine = GeneralUtils.SetLine(sLine, compoundDto.StreetCode, 10);
            sLine = GeneralUtils.SetLine(sLine, compoundDto.StreetDesc, 100);
            sLine = GeneralUtils.SetLine(sLine, compoundDto.CompDate, 8);
            sLine = GeneralUtils.SetLine(sLine, compoundDto.CompTime, 6);
            sLine = GeneralUtils.SetLine(sLine, compoundDto.EnforcerId, 7);
            sLine = GeneralUtils.SetLine(sLine, compoundDto.WitnessId, 7);
            sLine = GeneralUtils.SetLine(sLine, compoundDto.PubWitness, 60);
            sLine = GeneralUtils.SetLine(sLine, compoundDto.PrintCnt, 1);
            sLine = GeneralUtils.SetLine(sLine, compoundDto.Tempatjadi, 150);

            sLine = GeneralUtils.SetLine(sLine, compoundDto.Tujuan, 100);
            sLine = GeneralUtils.SetLine(sLine, compoundDto.Perniagaan, 100);
            sLine = GeneralUtils.SetLine(sLine, compoundDto.Kadar, 1);
            sLine = GeneralUtils.SetLine(sLine, compoundDto.GpsX, 15);
            sLine = GeneralUtils.SetLine(sLine, compoundDto.GpsY, 15);
            sLine = GeneralUtils.SetLine(sLine, compoundDto.TempohDate, 8);

            if (compoundDto.CompType == Constants.CompType1)
            {
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound1Type.CarNum, 15);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound1Type.Category, 2);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound1Type.CarType, 3);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound1Type.CarTypeDesc, 40);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound1Type.CarColor, 2);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound1Type.CarColorDesc, 40);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound1Type.LotNo, 15);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound1Type.RoadTax, 10);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound1Type.RoadTaxDate, 8);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound1Type.CompAmt, 10);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound1Type.CompAmt2, 10);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound1Type.CompAmt3, 10);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound1Type.CouponNumber, 15);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound1Type.CouponDate, 8);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound1Type.CouponTime, 4);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound1Type.ScanDateTime, 19); //yyyy-mm-dd hh:mm:ss
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound1Type.DeliveryCode, 2);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound1Type.CompDesc, 600);
            }
            else if (compoundDto.CompType == Constants.CompType2)
            {
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound2Type.CarNum, 15);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound2Type.Category, 2);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound2Type.CarType, 3);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound2Type.CarTypeDesc, 40);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound2Type.CarColor, 2);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound2Type.CarColorDesc, 40);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound2Type.RoadTax, 10);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound2Type.CompAmt, 10);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound2Type.CompAmt2, 10);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound2Type.CompAmt3, 10);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound2Type.DeliveryCode, 2);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound2Type.Muatan, 20);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound2Type.CompDesc, 600);
            }
            else if (compoundDto.CompType == Constants.CompType3)
            {
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound3Type.Rujukan, 15);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound3Type.Company, 20);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound3Type.CompanyName, 60);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound3Type.OffenderIc, 16);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound3Type.OffenderName, 60);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound3Type.Address1, 80);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound3Type.Address2, 80);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound3Type.Address3, 170);

                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound3Type.CompAmt, 10);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound3Type.CompAmt2, 10);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound3Type.CompAmt3, 10);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound3Type.DeliveryCode, 2);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound3Type.CompDesc, 600);
            }
            else if (compoundDto.CompType == Constants.CompType4)
            {
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound4Type.Rujukan, 15);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound4Type.OffenderIc, 16);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound4Type.OffenderName, 60);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound4Type.No, 15);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound4Type.Building, 30);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound4Type.Jalan, 30);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound4Type.Taman, 30);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound4Type.PostCode, 5);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound4Type.City, 20);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound4Type.State, 20);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound4Type.CompAmt, 10);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound4Type.CompAmt2, 10);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound4Type.CompAmt3, 10);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound4Type.StorageAmt, 6);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound4Type.TransportAmt, 6);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound4Type.Remark, 60);
            }
            else if (compoundDto.CompType == Constants.CompType5)
            {
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound5Type.CarNum, 15);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound5Type.Category, 2);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound5Type.CarType, 3);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound5Type.CarTypeDesc, 40);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound5Type.CarColor, 2);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound5Type.CarColorDesc, 40);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound5Type.RoadTax, 10);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound5Type.CompAmt, 10);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound5Type.CompAmt2, 10);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound5Type.CompAmt3, 10);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound5Type.DeliveryCode, 2);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound5Type.Muatan, 20);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound5Type.CompDesc, 600);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound5Type.LockTime, 4);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound5Type.LockKey, 5);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound5Type.UnlockAmt, 7);
                sLine = GeneralUtils.SetLine(sLine, compoundDto.Compound5Type.TowAmt, 7);
            }

            sLine = GeneralUtils.SetLine(sLine, Constants.NewLine, 2);

            return sLine;
        }

        public static bool UpdatePrintCompound(string compoundNumber)
        {
            string strFolder = GeneralAndroidClass.GetExternalStorageDirectory();
            strFolder += Constants.ProgramPath + Constants.TransPath;

            string strFullFileName = strFolder + Constants.CompoundFil;
            if (!File.Exists(strFullFileName))
            {
                LogFile.WriteLogFile("File Not Exist : " + strFullFileName, Enums.LogType.Debug);
                return false;
            }

            var listCompound = CompoundAccess.GetCompoundAccess(strFullFileName);
            var compoundDto = listCompound.FirstOrDefault(c => c.CompNum == compoundNumber);
            if (compoundDto != null)
            {
                var strPrint = compoundDto.PrintCnt;
                if (!GeneralBll.IsNumeric(strPrint))
                    return false;

                int printCnt = Convert.ToInt32(strPrint) + 1;
                if (printCnt > 9)
                    return false;

                compoundDto.PrintCnt = printCnt.ToString();

                //backup compound first
                var strBackupName = GeneralBll.GetLocalDateTime().ToString("yyyyMMddHHmmss") + Constants.CompoundFil;
                strBackupName = strFolder + strBackupName;
                if (!GeneralBll.CopyFile(strFullFileName, strBackupName, true))
                    return false;
                try
                {
                    string sLine = "";
                    var compoundLine = new StringBuilder();
                    foreach (var dto in listCompound)
                    {

                        if (dto.CompNum == compoundDto.CompNum)
                            sLine = ReadCompoundDtoIntoString(compoundDto);
                        else
                            sLine = ReadCompoundDtoIntoString(dto);

                        //sLine += Constants.NewLine;
                        compoundLine.Append(sLine);
                    }
                    //delete file first
                    File.Delete(strFullFileName);
                    //then Create first
                    GeneralBll.CreateNewFile(strFullFileName);
                    //write the update
                    CompoundAccess.AddCompoundAccess(strFullFileName, compoundLine, compoundDto.CompNum);
                    //remove the backup
                    File.Delete(strBackupName);
                    return true;
                }
                catch (Exception ex)
                {
                    LogFile.WriteLogFile("Error Update Compound , Message : " + ex.Message, Enums.LogType.Error);
                    //copy back the backup to original
                    GeneralBll.CopyFile(strBackupName, strFullFileName, true);
                    return false;
                }

            }

            return false;
        }

        public static bool ProcessCompoundOnlineServiceManual(string compoundNumber)
        {
            try
            {
                var dirCompoundOnline = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath +
                                                  Constants.SendOnlinePath;
                var strFullFileName = dirCompoundOnline + compoundNumber + ".txt";
                if (!File.Exists(strFullFileName))
                {
                    LogFile.WriteLogFile("File Not Exist " + strFullFileName, Enums.LogType.Info);
                    LogFile.WriteLogFile("Already send by services", Enums.LogType.Info);
                    return true; //maybe service have send it
                }
                var configDto = GeneralBll.GetConfig();
                if (configDto == null)
                {
                    LogFile.WriteLogFile("Get Config null", Enums.LogType.Info);
                    return false;
                }
                var svc = new WebService(configDto.ServiceUrl, configDto.ServiceUser, configDto.ServicePassword);


                var compoundDto = CompoundAccess.GetCompoundAccess(strFullFileName).FirstOrDefault();
                if (compoundDto != null)
                {
                    //send it web service
                    var message = svc.SendInsertCompound(compoundDto);
                    if (message == Constants.SendSuccess)
                    {
                        LogFile.WriteLogFile("Send Compound Online Success : ", Enums.LogType.Info);
                        //delete the file
                        File.Delete(strFullFileName);

                        if (SendImage2(compoundDto.CompNum, configDto.UrlPhoto))
                            DeleteSendImagesFile(compoundDto.CompNum);

                        return true;
                    }
                    else
                    {
                        LogFile.WriteLogFile("Send Compound Online Failed, message : " + message, Enums.LogType.Info);
                        return false;
                    }
                }
                LogFile.WriteLogFile("compoundDto null", Enums.LogType.Info);
                return false;
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("ProcessCompoundOnlineServiceManual " + ex.Message, Enums.LogType.Error);
                return false;
            }

        }

        public static CompoundDto GetCompoundByCompoundNumber(string compoundNumber)
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.TransPath + Constants.CompoundFil;


            if (!System.IO.File.Exists(strFullFileName))
            {
                LogFile.WriteLogFile("File Not Exist : " + strFullFileName, Enums.LogType.Debug);
                return null;
            }

            var listCompound = CompoundAccess.GetCompoundAccess(strFullFileName);

            return listCompound.FirstOrDefault(c => c.CompNum == compoundNumber);

        }

        public static List<CompoundDto> FindViewCompoundList(int iKey, string sSearchKey)
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.TransPath + Constants.CompoundFil;

            var result = new List<CompoundDto>();
            if (!System.IO.File.Exists(strFullFileName))
            {
                LogFile.WriteLogFile("File Not Exist : " + strFullFileName, Enums.LogType.Debug);
                return result;
            }

            var listOffend = TableFilBll.GetAllOffend();

            var listCompound =
                CompoundAccess.GetCompoundAccess(strFullFileName).Where(c => c.Deleted != Constants.RecDeleted).ToList();


            var len = string.IsNullOrEmpty(sSearchKey) ? 0 : sSearchKey.Length;
            foreach (var compoundDto in listCompound)
            {
                string offendDesc = TableFilBll.GetOffendDesc(compoundDto.OfdCode, compoundDto.ActCode, listOffend);
                bool blMatch = false;
                switch (iKey)
                {
                    case 0:

                        string sCompare = compoundDto.CompNum.Substring(0, len);
                        if (sCompare == sSearchKey) blMatch = true;
                        break;
                    case 1:
                        if (compoundDto.CompType == Constants.CompType1 ||
                            compoundDto.CompType == Constants.CompType2 ||
                            compoundDto.CompType == Constants.CompType5)
                        {
                            blMatch = GetCarNumber(compoundDto).Substring(0, len) == sSearchKey;
                        }
                        break;
                    case 2:
                        if (offendDesc.Length >= len)
                            blMatch = offendDesc.Substring(0, len) == sSearchKey;
                        break;
                    case 3:
                        if (compoundDto.CompType == Constants.CompType1 ||
                            compoundDto.CompType == Constants.CompType2 ||
                            compoundDto.CompType == Constants.CompType5)
                        {
                            string carTypeDesc = GetCarTypeDesc(compoundDto);
                            if (carTypeDesc.Length >= len)
                                blMatch = carTypeDesc.Substring(0, len) == sSearchKey;

                        }

                        break;
                    case 4:
                        if (compoundDto.CompTime.Length >= len)
                            blMatch = (compoundDto.CompTime.Substring(0, len) == sSearchKey);
                        break;
                }

                if (blMatch)
                {
                    compoundDto.OffendDesc = offendDesc;
                    result.Add(compoundDto);
                }

            }

            return result;
        }

        public static string GetCarTypeDesc(CompoundDto compoundDto)
        {
            switch (compoundDto.CompType)
            {
                case Constants.CompType1: return compoundDto.Compound1Type.CarTypeDesc;
                case Constants.CompType2: return compoundDto.Compound2Type.CarTypeDesc;
                case Constants.CompType5: return compoundDto.Compound5Type.CarTypeDesc;
            }
            return "";
        }

        private static string GetCarNumber(CompoundDto compoundDto)
        {
            switch (compoundDto.CompType)
            {
                case Constants.CompType1: return compoundDto.Compound1Type.CarNum;
                case Constants.CompType2: return compoundDto.Compound2Type.CarNum;
                case Constants.CompType5: return compoundDto.Compound5Type.CarNum;
            }
            return "";
        }

        public static void RemoveAllFilesCompoundOnline()
        {
            var listFileCompoundOnline = ListFileCompoundOnline();

            foreach (var fileCompoundOnline in listFileCompoundOnline)
            {
                GeneralBll.DeleteFile(fileCompoundOnline, true);
            }


        }

        public static NoteDto GetNoteByCompoundNumber(string compoundNumber)
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.TransPath + Constants.NoteFil;


            if (!System.IO.File.Exists(strFullFileName))
                return null;


            return NoteAccess.GetNoteAccess(strFullFileName).FirstOrDefault(c => c.CompNum == compoundNumber);

        }
        // status : 'E'- No parking session, 'A'- Got parking session, 'E'- Server Error, 'N'- No internet connection, 'C'- Cannot connect Kompaun server, 'D'- got compound issue, 'P' - got pass bulanan
        public static void EnquiryLogServer(string carNo, string status, InfoDto info, string Latitude, string Longitude, string strStreetCode, string scanDateTime)
        {
            var logServer = new LogServerDto();
            logServer.CarNo = carNo;
            logServer.Date = GeneralBll.GetDateDDMMYYYY(scanDateTime);
            logServer.Time = GeneralBll.GetTimeHHMMSS(scanDateTime);
            logServer.DolphinId = info.DolphinId;
            logServer.EnforcerId = info.EnforcerId;
            logServer.Zone = info.CurrZone;
            logServer.Sector = info.CurrMukim;
            logServer.Street = strStreetCode;
            logServer.Status = status;
            logServer.End = "10";

            EnquiryLogAccess.AddEnquiryLogFil(logServer, info, Latitude, Longitude);
        }



        public static List<CompoundDetailDto> GetCompoundDetails(bool isNoIC, string value)
        {
            var result = new List<CompoundDetailDto>();
            if (!GeneralBll.IsOnline())
            {
                LogFile.WriteLogFile("GetCompoundDetails : No Internet Connection");
                return result;
            }


            var searchType = isNoIC ? 3 : 4;
            var request = new GetCompoundDetail() {
                councilid = Constants.CouncilidKey,
                searchtype = searchType,
                searchvalue = value,
                refsource = Constants.RefsourceKey,
                secretkey = GeneralBll.GenerateKey($"{Constants.CouncilidKey}^{searchType}^{value}^{Constants.RefsourceKey}")
            };



            var configDto = GeneralBll.GetConfig();
            if (configDto == null)
            {
                throw new Exception("Config Not Found");
            }

            for (int i = 1; i <= Constants.MaxRetry; i++)
            {
                result = Task.Run(async () => await HttpClientService.GetCompoundDetails(request, configDto.ServiceUrl)).Result;

                if (result == null || !result.Any())
                {
                    Thread.Sleep(Constants.SleepRetryActiveParking);
                }
                else
                    return result;
            }

            return result;
        }

        public static ResultCompoundExistDto GetCompoundByCarNum(string carnum, WebService myWebservice)
        {
            string strExist = "null";
            var resultCompoundExistDto = new ResultCompoundExistDto();
            try
            {

                var configDto = GeneralBll.GetConfig();
                if (configDto == null)
                {
                    LogFile.WriteLogFile("Get Config null", Enums.LogType.Error);
                    resultCompoundExistDto.ReturnStatus = "null";
                    return resultCompoundExistDto;
                }


                ////var webService = new WebService(configDto.ServiceUrl, configDto.ServiceUser, configDto.ServicePassword);
                var result = myWebservice.IsCompoundExist(carnum, configDto.ServiceKey);

                if (string.IsNullOrEmpty(result))
                {
                    if (result == "")
                        strExist = "false";
                    else
                        strExist = "null";
                    resultCompoundExistDto.ReturnStatus = strExist ;
                }
                else
                {
                    strExist = "true";
                    resultCompoundExistDto.ReturnStatus = strExist;
                    var compoundexistDto = new CompoundExistDto();
                    compoundexistDto = JsonConvert.DeserializeObject<CompoundExistDto>(result);
                    if (compoundexistDto.Discname != "")
                    {
                        resultCompoundExistDto.ReturnStatus = "pass";
                        resultCompoundExistDto.vStartDate = compoundexistDto.vStartDate;
                        resultCompoundExistDto.vEndDate = compoundexistDto.vEndDate;
                        resultCompoundExistDto.Discname = compoundexistDto.Discname;
                    }
                }
                return resultCompoundExistDto;
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("BLL Err GetCompoundByCarNum : " + ex.Message, Enums.LogType.Error);
                resultCompoundExistDto.ReturnStatus = "null";
                return resultCompoundExistDto;
            }
        }

        private static bool ManageTransAfterUpload(string dolphinId, string strdate)
        {
            try
            {
                LogFile.WriteLogFile("Start Move Files TRANS For Backup", Enums.LogType.Info);

                var pathSource = GeneralBll.GetTransPath();

                var destSource = GeneralBll.GetTransBackupPath();

                var listFiles = GeneralBll.GetListTransFiles();



                var strPrefix = dolphinId + strdate + GeneralBll.GetLocalDateTime().ToString("HHmm");

                foreach (var listFile in listFiles)
                {
                    if (GeneralBll.IsFileExist(pathSource + listFile, true))
                        GeneralBll.MoveFile(pathSource + listFile, destSource + strPrefix + listFile);
                }


                ManageTransFileBackup();
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("Error ManageTransAfterUpload : " + ex.Message, Enums.LogType.Error);
                return false;
            }

        }

        private static void ManageTransFileBackup()
        {


            const int maxFile = 60;

            LogFile.WriteLogFile("Start ManageTransFileBackup", Enums.LogType.Info);

            var pathBackup = GeneralBll.GetTransBackupPath();

            var di = new DirectoryInfo(pathBackup);

            var listTransFile = GeneralBll.GetListTransFiles();

            foreach (var transFile in listTransFile)
            {

                FileInfo[] rgFiles = di.GetFiles("*" + transFile);

                if (rgFiles.Count() > maxFile)
                {
                    var listFiles = rgFiles.OrderByDescending(c => c.Name).Select(x => x.FullName).ToList();

                    var listToDelete = listFiles.Skip(maxFile).Take(listFiles.Count - maxFile).ToList();

                    foreach (var fileName in listToDelete)
                    {
                        GeneralBll.DeleteFile(fileName, true);
                    }
                }

            }

            LogFile.WriteLogFile("Done ManageTransFileBackup", Enums.LogType.Info);

        }

        private static bool ManageImageFolderAfterUpload(string strCurrdate)
        {
            try
            {
                LogFile.WriteLogFile("Start ManageImageFolderAfterUpload", Enums.LogType.Info);

                var pathSource = GeneralBll.GetInternalImagePath();


                var pathDest = GeneralBll.GetImageBackupPath();

                var strDate = strCurrdate;

                //get all image in folder
                var di = new DirectoryInfo(pathSource);
                FileInfo[] rgFiles = di.GetFiles("*.*");

                foreach (FileInfo fi in rgFiles)
                {
                    if (GeneralBll.IsFileExist(fi.FullName, true))
                        GeneralBll.MoveFile(fi.FullName, pathDest + strDate + fi.Name);
                }

                ManageImageBackup(pathDest);
                ManageSignFolderAfterUpload();

                LogFile.WriteLogFile("Done ManageImageFolderAfterUpload", Enums.LogType.Info);
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("Error ManageImageFolderAfterUpload : " + ex.Message, Enums.LogType.Error);
                return false;
            }

        }

        private static void ManageImageBackup(string pathBackupImage)
        {
            //remove all image that more than 3 days
            //var pathBackupImage = GeneralAndroidClass.GetExternalStorageDirectory() + GlobalClass.PROG_PATH +
            //             GlobalClass.TRANSBACKUPPATH + "IMGS/";

            //get all image in folder
            var di = new DirectoryInfo(pathBackupImage);
            FileInfo[] rgFiles = di.GetFiles("*.*");

            foreach (FileInfo fi in rgFiles)
            {

                var fileName = fi.Name;
                var strDate = fileName.Substring(0, 8);//format yyyyMMdd

                var dtDate = GeneralBll.ConvertStringToDate(strDate);
                if (dtDate == null) continue;

                var dateTimeServer = GeneralBll.GetLocalDateTime();
                var dateLocal = new DateTime(dateTimeServer.Year, dateTimeServer.Month, dateTimeServer.Day);
                var difference = dateLocal - dtDate.Value;

                //FileSystemAndroid.WriteLogFile("difference : " + difference.TotalDays);
                //keep for 3 days
                if (difference.TotalDays > 14)
                    GeneralBll.DeleteFile(fi.FullName, true);

            }

        }

        private static bool ManageSignFolderAfterUpload()
        {
            try
            {
                LogFile.WriteLogFile("Start ManageSignFolderAfterUpload", Enums.LogType.Info);

                var pathSource = GeneralBll.GetSignaturePath();


                var pathDest = GeneralBll.GetSignatureBackupPath();

                var strDate = GeneralBll.GetLocalDateTime().ToString("yyyyMMdd");

                //get all Signature in folder
                var di = new DirectoryInfo(pathSource);
                FileInfo[] rgFiles = di.GetFiles("*.*");

                foreach (FileInfo fi in rgFiles)
                {
                    if (GeneralBll.IsFileExist(fi.FullName, true))
                        GeneralBll.MoveFile(fi.FullName, pathDest + strDate + fi.Name);
                }

                ManageSignBackup(pathDest);

                LogFile.WriteLogFile("Done ManageImageFolderAfterUpload", Enums.LogType.Info);
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("Error ManageImageFolderAfterUpload : " + ex.Message, Enums.LogType.Error);
                return false;
            }

        }

        private static void ManageSignBackup(string pathBackupSign)
        {

            var di = new DirectoryInfo(pathBackupSign);
            FileInfo[] rgFiles = di.GetFiles("*.*");

            foreach (FileInfo fi in rgFiles)
            {

                var fileName = fi.Name;
                var strDate = fileName.Substring(0, 8);//format yyyyMMdd

                var dtDate = GeneralBll.ConvertStringToDate(strDate);
                if (dtDate == null) continue;

                var dateTimeServer = GeneralBll.GetLocalDateTime();
                var dateLocal = new DateTime(dateTimeServer.Year, dateTimeServer.Month, dateTimeServer.Day);
                var difference = dateLocal - dtDate.Value;

                //FileSystemAndroid.WriteLogFile("difference : " + difference.TotalDays);
                //keep for 3 days
                if (difference.TotalDays > 3)
                    GeneralBll.DeleteFile(fi.FullName, true);

            }

        }


    }
}