using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ServiceModel;
using Java.Lang;
using Exception = System.Exception;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.BusinessObject.DTOs.Responses;
using AndroidCompound5.AimforceUtils;

namespace AndroidCompound5
{

    public class WebService
    {
        private ServiceCompoundSoap _serviceCompound;
        private UserCredentials _userCredentials;
          
        //private string _serviceKey;
        public WebService(string serviceUrl, string serviceUser, string serviecPassword)
        {
           // _serviceKey = serviceKey;

            _serviceCompound = null;

            ServicePointManager.DefaultConnectionLimit = 10;
            ServicePointManager.MaxServicePoints = 5;

            var binding = new BasicHttpBinding();
            binding.Name = "MyServicesSoap";
            binding.CloseTimeout = TimeSpan.FromSeconds(10);
            binding.OpenTimeout = TimeSpan.FromSeconds(10);
            binding.ReceiveTimeout = TimeSpan.FromSeconds(10);
            binding.SendTimeout = TimeSpan.FromSeconds(10);
           
            //binding.AllowCookies = false;
            binding.BypassProxyOnLocal = false;
            //binding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            binding.MaxBufferSize = 2147483647;
            binding.MaxReceivedMessageSize = 2147483647;
            binding.MaxBufferPoolSize = 2147483647;
            binding.MessageEncoding = WSMessageEncoding.Text;
            binding.TextEncoding = System.Text.Encoding.UTF8;
            binding.TransferMode = TransferMode.Buffered;
            binding.UseDefaultWebProxy = true;
            
            binding.Security.Mode = BasicHttpSecurityMode.None;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;

            var endpoint = new EndpointAddress(serviceUrl);

            _serviceCompound = new ServiceCompoundSoapClient(binding, endpoint);

            _userCredentials = new UserCredentials();
            _userCredentials.userid = Encrypt(serviceUser);
            _userCredentials.password = Encrypt(serviecPassword);
        }

        public static string Encrypt(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }


        public static string Decrypt(string encryptedText)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(encryptedText);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes, 0, base64EncodedBytes.Length);
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
                LogFile.WriteLogFile("IOException General IsOnline : " + e.StackTrace, Enums.LogType.Error);
                //e.PrintStackTrace();
            }
            catch (InterruptedException e)
            {
                LogFile.WriteLogFile("InterruptedException General IsOnline : " + e.StackTrace, Enums.LogType.Error);
                e.PrintStackTrace();
            }

            return false;

        }

        private ENQUIRYLOGInfo SetEnquirylogInfo(LogServerDto logserverDto)
        {
            var enquiryloginfo = new ENQUIRYLOGInfo();
            enquiryloginfo.NoDaftar = logserverDto.CarNo;
            enquiryloginfo.LogDate = logserverDto.Date;
            enquiryloginfo.LogTime = logserverDto.Time;
            enquiryloginfo.DHID = logserverDto.DolphinId ;
            enquiryloginfo.KodPguatkuasa = logserverDto.EnforcerId ;
            enquiryloginfo.KodZon = logserverDto.Zone;
            enquiryloginfo.KodKaw = logserverDto.Sector;
            enquiryloginfo.KodJalan = logserverDto.Street ;
            enquiryloginfo.Status = logserverDto.Status ;
            enquiryloginfo.Latitude = logserverDto.Latitude;
            enquiryloginfo.Longitude = logserverDto.Longitude;



            return enquiryloginfo;
        }

        private GPSInfo SetGpsInfo(GpsDto gpsDto)
        {
            var gpsInfo = new GPSInfo();
            gpsInfo.ActivityDate = gpsDto.ActivityDate;
            gpsInfo.ActivityTime = gpsDto.ActivityTime;
            gpsInfo.Batterylife = gpsDto.BatteryLife;
            gpsInfo.EnforcerID = gpsDto.Kodpguatkuasa;
            gpsInfo.NoDolphin = gpsDto.DhId;
            gpsInfo.Latitude = gpsDto.GpsX;
            gpsInfo.Longitude = gpsDto.GpsY;

            return gpsInfo;
        }

        public string SendGpsInfo(GpsDto gpsDto)
        {
            string result = "";

            if (!IsOnline())
            {
                LogFile.WriteLogFile("SendGpsInfo : No Internet Connection", Enums.LogType.Info, Enums.FileLogType.GpsService);
                return result;
            }
            try
            {
                var gpsInfo = SetGpsInfo(gpsDto);

               result = _serviceCompound.InsertEnforceractivity(gpsInfo, _userCredentials);

                return result;
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("Error SendGpsInfo() : " + ex.Message, Enums.LogType.Error, Enums.FileLogType.GpsService);
                return ex.Message;
            }

        }

        public string SendEnquiryLogInfo(LogServerDto enquiryDto)
        {
            string result = "";

            if (!IsOnline())
            {
                LogFile.WriteLogFile("SendEnquiryLogInfo : No Internet Connection", Enums.LogType.Info, Enums.FileLogType.GpsService);
                return result;
            }
            try
            {
                var enquirylogInfo = SetEnquirylogInfo(enquiryDto);

                result = _serviceCompound.InsertEnquirylog(enquirylogInfo, _userCredentials);

                return result;
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("Error SendEnquiryLog() : " + ex.Message, Enums.LogType.Error, Enums.FileLogType.GpsService);
                return ex.Message;
            }

        }

        public string SendInsertCompound(CompoundDto compoundDto)
        {
            string result = "";

            if (!IsOnline())
            {
                LogFile.WriteLogFile("SendInsertCompound : No Internet Connection", Enums.LogType.Info, Enums.FileLogType.CompoundService);
                return result;
            }
            try
            {
                var compoundInfo = SetCompoundInfoService(compoundDto);

                if (compoundInfo == null)
                    return string.Empty;

                result = _serviceCompound.InsertCompoundDetails(compoundInfo, _userCredentials);

                return result;
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("Error SendInsertCompound() : " + ex.Message, Enums.LogType.Error, Enums.FileLogType.CompoundService);
                return ex.Message;
            }

        }

        private CompoundInfo SetCompoundInfoService(CompoundDto compoundDto)
        {
            try
            {
                var compoundInfo = new CompoundInfo();

                compoundInfo.CompoundNumber = compoundDto.CompNum;
                compoundInfo.CompoundType = compoundDto.CompType;
                compoundInfo.Actcode = compoundDto.ActCode;
                compoundInfo.offense = compoundDto.OfdCode;
                compoundInfo.Mukim = compoundDto.Mukim;
                compoundInfo.Zone = compoundDto.Zone;
                compoundInfo.street = compoundDto.StreetCode;
                compoundInfo.streetDescr = compoundDto.StreetDesc;
                compoundInfo.compDate = compoundDto.CompDate;
                compoundInfo.compTime = compoundDto.CompTime;
                compoundInfo.enforcer = compoundDto.EnforcerId;
                compoundInfo.witness = compoundDto.WitnessId;
                compoundInfo.PubWitness = compoundDto.PubWitness;
                compoundInfo.PrintCnt = compoundDto.PrintCnt;
                compoundInfo.Tempatjadi = compoundDto.Tempatjadi;
                compoundInfo.tujuan= compoundDto.Tujuan;
                compoundInfo.perniagaan= compoundDto.Perniagaan;
                compoundInfo.Kadar = compoundDto.Kadar;
                compoundInfo.Latitude = compoundDto.GpsX;
                compoundInfo.Longitude = compoundDto.GpsY;
                compoundInfo.tempohdate = compoundDto.TempohDate ;
                compoundInfo.kodSalah = compoundDto.OfdCode;

                if (compoundDto.CompType == Constants.CompType1)
                {
                    compoundInfo.noDaftar = compoundDto.Compound1Type.CarNum;
                    compoundInfo.vehicleNo = compoundDto.Compound1Type.CarNum;
                    compoundInfo.jenken= compoundDto.Compound1Type.Category ;
                    compoundInfo.CarType = compoundDto.Compound1Type.CarType;
                    compoundInfo.CarTypeDesc = compoundDto.Compound1Type.CarTypeDesc;
                    compoundInfo.CarColor = compoundDto.Compound1Type.CarColor;
                    compoundInfo.CarColorDesc = compoundDto.Compound1Type.CarColorDesc;
                    compoundInfo.parkingLot = compoundDto.Compound1Type.LotNo ;
                    compoundInfo.RoadTax = compoundDto.Compound1Type.RoadTax;
                    compoundInfo.amnkmp = compoundDto.Compound1Type.CompAmt;
                    compoundInfo.amnkmp2= compoundDto.Compound1Type.CompAmt2;
                    compoundInfo.amnkmp3 = compoundDto.Compound1Type.CompAmt3;
                    compoundInfo.pdNo = compoundDto.Compound1Type.CouponNumber;
                    compoundInfo.PdDate= compoundDto.Compound1Type.CouponDate;
                    compoundInfo.pdTime = compoundDto.Compound1Type.CouponTime ;
                    compoundInfo.DeliveryCode = compoundDto.Compound1Type.DeliveryCode ;
                    compoundInfo.compDescr = compoundDto.Compound1Type.CompDesc;
                   // compoundInfo.vendorhhd = "A";
                }
                else if (compoundDto.CompType == Constants.CompType2)
                {
                    compoundInfo.vehicleNo = compoundDto.Compound2Type.CarNum;
                    compoundInfo.jenken = compoundDto.Compound1Type.Category;
                    compoundInfo.CarType = compoundDto.Compound2Type.CarType;
                    compoundInfo.CarTypeDesc = compoundDto.Compound2Type.CarTypeDesc;
                    compoundInfo.CarColor = compoundDto.Compound2Type.CarColor;
                    compoundInfo.CarColorDesc = compoundDto.Compound2Type.CarColorDesc;
                    compoundInfo.RoadTax = compoundDto.Compound2Type.RoadTax;
                    compoundInfo.amnkmp = compoundDto.Compound2Type.CompAmt;
                    compoundInfo.amnkmp2 = compoundDto.Compound2Type.CompAmt2;
                    compoundInfo.amnkmp3 = compoundDto.Compound2Type.CompAmt3;
                    compoundInfo.DeliveryCode = compoundDto.Compound2Type.DeliveryCode;
                    compoundInfo.muatan = compoundDto.Compound2Type.Muatan ;
                    compoundInfo.compDescr = compoundDto.Compound2Type.CompDesc;
                    compoundInfo.noDaftar = compoundDto.Compound2Type.CarNum;
                }
                else if (compoundDto.CompType == Constants.CompType3)
                {
                    compoundInfo.noRujukan = compoundDto.Compound3Type.Rujukan;
                    compoundInfo.Company = compoundDto.Compound3Type.Company;
                    compoundInfo.CompanyName = compoundDto.Compound3Type.CompanyName;
                    compoundInfo.OffenderIc = compoundDto.Compound3Type.OffenderIc;
                    compoundInfo.OffenderName = compoundDto.Compound3Type.OffenderName;
                    compoundInfo.Alamat1 = compoundDto.Compound3Type.Address1;
                    compoundInfo.alamat2 = compoundDto.Compound3Type.Address2;
                    compoundInfo.alamat3 = compoundDto.Compound3Type.Address3;

                    //compoundInfo.Taman = compoundDto.Compound3Type.Taman;
                    //compoundInfo.PostCode = compoundDto.Compound3Type.PostCode;
                    //compoundInfo.City = compoundDto.Compound3Type.City;
                    //compoundInfo.State = compoundDto.Compound3Type.State;
                    compoundInfo.amnkmp = compoundDto.Compound3Type.CompAmt;
                    compoundInfo.amnkmp2 = compoundDto.Compound3Type.CompAmt2;
                    compoundInfo.amnkmp3 = compoundDto.Compound3Type.CompAmt3;
                    compoundInfo.DeliveryCode = compoundDto.Compound3Type.DeliveryCode;
                    compoundInfo.compDescr = compoundDto.Compound3Type.CompDesc;

                    //compoundInfo.UnitNo = compoundDto.Compound3Type.No;
                    //compoundInfo.Building = compoundDto.Compound3Type.Building;
                    //compoundInfo.Jalan = compoundDto.Compound3Type.Jalan;
                }
                else if (compoundDto.CompType == Constants.CompType4)
                {
                    compoundInfo.noRujukan = compoundDto.Compound4Type.Rujukan;
                    compoundInfo.OffenderIc = compoundDto.Compound4Type.OffenderIc;
                    compoundInfo.OffenderName = compoundDto.Compound4Type.OffenderName;
                    
                    compoundInfo.Taman = compoundDto.Compound4Type.Taman;
                    compoundInfo.PostCode = compoundDto.Compound4Type.PostCode;
                    compoundInfo.City = compoundDto.Compound4Type.City;
                    compoundInfo.State = compoundDto.Compound4Type.State;
                    compoundInfo.amnkmp = compoundDto.Compound4Type.CompAmt;
                   // compoundInfo.s = compoundDto.Compound4Type.StorageAmt;
                    //compoundInfo.tr = compoundDto.Compound4Type.TransportAmt;
                    compoundInfo.catatan = compoundDto.Compound4Type.Remark;
                }
                else if (compoundDto.CompType == Constants.CompType5)
                {
                    compoundInfo.KodKen = compoundDto.Compound5Type.CarNum;
                    compoundInfo.CarType = compoundDto.Compound5Type.CarType;
                    compoundInfo.CarTypeDesc = compoundDto.Compound5Type.CarTypeDesc;
                    compoundInfo.CarColor = compoundDto.Compound5Type.CarColor;
                    compoundInfo.CarColorDesc = compoundDto.Compound5Type.CarColorDesc;
                    compoundInfo.RoadTax = compoundDto.Compound5Type.RoadTax;
                    //compoundInfo.lock = compoundDto.Compound5Type.LockTime;
                    //compoundInfo.loc = compoundDto.Compound5Type.LockKey;
                    //compoundInfo.un = compoundDto.Compound5Type.UnlockAmt;
                    //compoundInfo.to = compoundDto.Compound5Type.TowAmt;
                }

                return compoundInfo;
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("Error SetCompoundInfoService() : " + ex.Message, Enums.LogType.Error, Enums.FileLogType.CompoundService);
                return null;
            }
        }

        public string GetServerDateTime(string servicekey)
        {
            string result = "";

            try
            {
                result = _serviceCompound.GetServerDatetime(servicekey);
            }
            catch (Exception ex)
            {

                LogFile.WriteLogFile("Error GetServerDateTime : " + ex.Message);
            }
            return result;
        }

        public string IsCompoundExist(string carnum, string servicekey)
        {
            string result = "";

            try
            {
                result = _serviceCompound.IsCompoundexist(carnum,servicekey);
            }
            catch (Exception ex)
            {

                LogFile.WriteLogFile("Error IsCompoundExist : " + ex.Message);
                result = null;
            }
            return result;
        }

        public ActiveParkingDto GetActiveParking(string carNumber, string input, string serviceAppletUrl)
        {
          

            var result = new ActiveParkingDto();
            result.ReturnResponse = Enums.ParkingStatus.Error;
            result.ScanDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            try
            {

                ServicePointManager.DefaultConnectionLimit = 10;
                if (!string.IsNullOrEmpty(input))
                {
                   
                    var encoding = new System.Text.UTF8Encoding();
                    string postData = "";
                    postData = "_module=HandheldModule&_method=getActiveParkingList_PARKMAX&vehicleId=" + input.Trim();

                    byte[] data = encoding.GetBytes(postData);
                    string webServiceUrl = serviceAppletUrl;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    var myRequest = (HttpWebRequest)WebRequest.Create(webServiceUrl);
                    myRequest.Method = "POST";
                    myRequest.ContentLength = data.Length;
                    myRequest.UserAgent =
                        "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36";
                    myRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                    myRequest.Accept = "application/json";
                    myRequest.Headers.Set("X-Auth-Module", "true");
                    myRequest.Timeout = 5000; //5 second
                    myRequest.ReadWriteTimeout = 5000;
                    myRequest.KeepAlive = true;
                    Stream newStream = myRequest.GetRequestStream();
                    // Send the data.
                    newStream.Write(data, 0, data.Length);
                    newStream.Close();
                    HttpWebResponse hwr = null;

                    //myRequest.Proxy = null;
                    //using (hwr = (HttpWebResponse)myRequest.GetResponse())
                    //{
                    //}
                    hwr = (HttpWebResponse)myRequest.GetResponse();



                    var reader = new StreamReader(hwr.GetResponseStream());
                    string res = reader.ReadToEnd();
                    LogFile.WriteLogFile("CallGetActiveParking : " + res);
                    hwr.Close();
                    reader.Close();

                    var listInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VehicleResponse>>(res);

                    if (res == "[]")
                        result.ReturnResponse = Enums.ParkingStatus.Available;

                    foreach (var vehicleResponse in listInfo)
                    {
                        if (vehicleResponse.Vehicleid.ToLower() == carNumber.ToLower())
                        {
                            if (vehicleResponse.StartTime != vehicleResponse.EndTime)
                            {
                                //return Enums.ParkingStatus.Used;
                                result.ReturnResponse = Enums.ParkingStatus.Used;
                                result.CarNumber = carNumber;
                                DateTime dt = Convert.ToDateTime(vehicleResponse.StartTime);
                                DateTime dt1 = Convert.ToDateTime(vehicleResponse.EndTime);

                                //DateTime dt2 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dt, "Singapore");

                                result.StartDate = dt.ToString("yyyy-MM-dd HH:mm:ss");
                                result.EndDate = dt1.ToString("yyyy-MM-dd HH:mm:ss");

                                break;
                            }
                        }
                        //return Enums.ParkingStatus.Available;
                        result.ReturnResponse = Enums.ParkingStatus.Available;
                        break;
                    }

                }
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("error CallGetActiveParking : " + ex.Message + ", Car Number : " + carNumber);
            }

            return result;

        }

        public ActiveReserveParkingDto GetSeasonParkList(string strZone, string strJalan, string strNoPetak, string serviceAppletUrl)
        {
            var result = new ActiveReserveParkingDto();
            result.status = "N";

            try
            {

                ServicePointManager.DefaultConnectionLimit = 10;
                if (!string.IsNullOrEmpty(strNoPetak))
                {
                    var encoding = new System.Text.UTF8Encoding();
                    string postData = "";
                    postData = "_module=HandheldModule&_method=getSeasonParkList&" +
                               "streetId=" + strJalan.Trim() +
                               "&zoneId=" + strZone.Trim() +
                               "&noLot=" + strNoPetak.Trim();

                    byte[] data = encoding.GetBytes(postData);
                    string webServiceUrl = serviceAppletUrl;

                    var myRequest = (HttpWebRequest)WebRequest.Create(webServiceUrl);
                    myRequest.ServerCertificateValidationCallback = delegate { return true; };
                    myRequest.Method = "POST";

                    myRequest.ContentLength = data.Length;
                    myRequest.UserAgent =
                        "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36";
                    myRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                    myRequest.Accept = "application/json";
                    myRequest.Headers.Set("X-Auth-Module", "true");
                    myRequest.Timeout = 5000; //5 second
                    myRequest.ReadWriteTimeout = 10000;
                    myRequest.KeepAlive = true;
                    Stream newStream = myRequest.GetRequestStream();
                    // Send the data.
                    newStream.Write(data, 0, data.Length);
                    newStream.Close();
                    newStream.Dispose();
                    HttpWebResponse hwr = null;

                    hwr = (HttpWebResponse)myRequest.GetResponse();

                    var reader = new StreamReader(hwr.GetResponseStream());
                    string res = reader.ReadToEnd();

                    LogFile.WriteLogFile("Response string for Lot Number(" + strNoPetak + ") : " + res);

                    reader.Close();
                    reader.Dispose();
                    DateTime dtstartdate, dtEndDate;

                    var listInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ReserveParkingResponse>>(res);
                    if (listInfo.Count > 0)
                    {
                        foreach (var vehicleResponse in listInfo)
                        {
                            result.Name = vehicleResponse.Name;
                            dtstartdate = DateTime.Parse(vehicleResponse.StartDate);
                            dtEndDate = DateTime.Parse(vehicleResponse.EndDate);
                            result.StartDate = dtstartdate.ToString("dd/MM/yyyy");
                            result.EndDate = dtEndDate.ToString("dd/MM/yyyy");
                            result.Amt = vehicleResponse.Amount;
                            result.Lotstatus = vehicleResponse.Status;
                            result.Note = vehicleResponse.Note;
                            result.status = "Y";
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("error CallGetReserveParkingList : " + ex.Message + ", Lot Number : " + strNoPetak);
            }

            return result;

        }

    }
}
