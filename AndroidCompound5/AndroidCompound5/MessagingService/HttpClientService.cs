using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.BusinessObject.DTOs.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AndroidCompound5
{
    public class HttpClientService
    {
        public HttpClientService()
        {

        }

        public static async Task<ActiveParkingDto> GetActiveParking(string serviceAppletUrl, string id, string carNumber)
        {
            var resultActiveParkingDto = new ActiveParkingDto
            {
                ReturnResponse = Enums.ParkingStatus.Error,
                ScanDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")
            };

            try
            {

                using (HttpClient client = new HttpClient())
                {
                    var result = new List<ResultApiDto>();

                    var url = $"{serviceAppletUrl}?pbt=mpkknk&id={id}&search={carNumber}&source=mpkn";
                    var req = new HttpRequestMessage(HttpMethod.Post, url);
                    var response = await client.SendAsync(req);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        resultActiveParkingDto.ReturnResponse = Enums.ParkingStatus.Available;

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var _datas = JsonConvert.DeserializeObject<RootObject>(stringResult);
                    if (_datas != null)
                    {
                        foreach (var item in _datas.Rows)
                        {
                            result.Add(item);
                        }
                    }

                    if (result.Any())
                    {
                        //var hasParkir = result.SingleOrDefault(m => m.expired == "0");
                        var hasParkir = result.FirstOrDefault(m => m.expired == "0");
                        if (hasParkir != null)
                        {
                            resultActiveParkingDto.ReturnResponse = Enums.ParkingStatus.Used;
                            resultActiveParkingDto.CarNumber = carNumber;

                            CultureInfo provider = CultureInfo.InvariantCulture;
                            DateTime StartDate = DateTime.ParseExact($"{hasParkir?.date} {hasParkir?.start}", "dd/MM/yyyy hh:mmtt", provider);
                            DateTime EndDate = DateTime.ParseExact($"{hasParkir?.date} {hasParkir?.end}", "dd/MM/yyyy hh:mmtt", provider);

                            resultActiveParkingDto.StartDate = StartDate.ToString("yyyy-MM-dd HH:mm:ss");
                            resultActiveParkingDto.EndDate = EndDate.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                            resultActiveParkingDto.ReturnResponse = Enums.ParkingStatus.Available;
                    }

                    return resultActiveParkingDto;
                }
            }
            catch (System.Exception ex)
            {
                return resultActiveParkingDto;
            }
        }

        public static async Task<ActiveParkingDto> GetActiveParkingV2(string serviceAppletUrl, string id, string carNumber)
        {
            var resultActiveParkingDto = new ActiveParkingDto
            {
                ReturnResponse = Enums.ParkingStatus.Error,
                ScanDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")
            };

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "5VG3eaMazplirRpLTxC2KXaxfNIIBJcq");
                //var url = $"http://staging.gox-api.phgo.xyz/api/parking_validity?registration_number=PKV1212";//WER1234
                var url = $"{serviceAppletUrl}?registration_number={carNumber}";

                LogFile.WriteLogFile("ÜRL : " + url);
                var req = new HttpRequestMessage(HttpMethod.Get, url);
                //req.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

                var response = await client.SendAsync(req);

                if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    resultActiveParkingDto.ReturnResponse = Enums.ParkingStatus.Available;

                    var stringResult = await response.Content.ReadAsStringAsync();
                    LogFile.WriteLogFile("stringResult" + stringResult);
                    var dto = JsonConvert.DeserializeObject<ResponseDto>(stringResult);
                    if (dto.Success)
                    {
                        LogFile.WriteLogFile("dto success");

                        resultActiveParkingDto.ReturnResponse = Enums.ParkingStatus.Used;
                        resultActiveParkingDto.CarNumber = carNumber;

                        resultActiveParkingDto.StartDate = dto.Data?.Start_date;
                        resultActiveParkingDto.EndDate = dto.Data?.End_date;
                    }
                    else
                        resultActiveParkingDto.ReturnResponse = Enums.ParkingStatus.Available;
                }
            }

            return resultActiveParkingDto;
        }


        public static async Task<List<CompoundDetailDto>> GetCompoundDetails(GetCompoundDetail request, string serviceAppletUrl)
        {
            var result = new List<CompoundDetailDto>();
            try
            {
                var body = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(request));

                using (HttpClient client = new HttpClient())
                {
                    var url = $"{serviceAppletUrl}/GetCompDetailToPay";

                    LogFile.WriteLogFile("URL : " + url);
                    var req = new HttpRequestMessage(HttpMethod.Post, url)
                    {
                        Content = new FormUrlEncodedContent(body)
                    };
                    var response = await client.SendAsync(req);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var stringResult = await response.Content.ReadAsStringAsync();
                        LogFile.WriteLogFile("stringResult" + stringResult);
                        result = JsonConvert.DeserializeObject<List<CompoundDetailDto>>(stringResult);
                    }
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile(ex.Message);
            }

            return result;
        }

        public static async Task<UPSBActiveParkingDto> GetActiveParkingUPSB(string serviceUPSBtUrl, UPSBInput input)
        {
            DateTime dtStartTime, dtEndTime;

            var UPSBActiveParkingDto = new UPSBActiveParkingDto
            {
                ReturnResponse = Enums.ParkingStatus.Error,
                ScanDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")
            };

            var resultUPSBParkingDto = new UPSBParkingDto();

            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("company_id", input.CompanyId),
                new KeyValuePair<string, string>("park_code", input.ParkCode),
                new KeyValuePair<string, string>("vpl_number", input.VplNumber),
                new KeyValuePair<string, string>("sign", input.Sign),
                new KeyValuePair<string, string>("appid", input.AppId)
            });

            try
            {

                using (HttpClient client = new HttpClient())
                {
                    var response = await client.PostAsync(serviceUPSBtUrl, formContent);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        var stringJson = await response.Content.ReadAsStringAsync();
                        LogFile.WriteLogFile("stringResult" + stringJson);
                        resultUPSBParkingDto = JsonConvert.DeserializeObject<UPSBParkingDto>(stringJson);
                        UPSBActiveParkingDto.ReturnResponse = Enums.ParkingStatus.Available;
                        UPSBActiveParkingDto.Carnum = resultUPSBParkingDto.Vpl_Number;
                        UPSBActiveParkingDto.Start_Time = resultUPSBParkingDto.In_Time;
                        //UPSBActiveParkingDto.End_Time = EndDate.ToString("yyyy-MM-dd HH:mm:ss");
                        UPSBActiveParkingDto.End_Time = resultUPSBParkingDto.Out_Time;
                        UPSBActiveParkingDto.Duration = resultUPSBParkingDto.Duration;
                        UPSBActiveParkingDto.Amount_paid = resultUPSBParkingDto.Amount_paid;

                        if (CalculateTime(resultUPSBParkingDto.In_Time, resultUPSBParkingDto.Out_Time) > 0)
                            UPSBActiveParkingDto.ReturnResponse = Enums.ParkingStatus.Used;
                    }
                    else
                        UPSBActiveParkingDto.ReturnResponse = Enums.ParkingStatus.Available;
                }
            }

            catch (Exception ex)
            {
                LogFile.WriteLogFile("error" + ex.Message);
            }

            return UPSBActiveParkingDto;
        }

        private static double CalculateTime(string starttime, string endtime)
        {
            TimeSpan timespan;
            double nElapse = 0;
            string strElapse = "";
            DateTime dtStartTime, dtEndTime, dtCurrTime;

            if (starttime == "" || endtime == "")
                nElapse = 0;
            else
            {
                dtStartTime = ConvertStringToDateTime(starttime);
                dtEndTime = ConvertStringToDateTime(endtime);
                dtCurrTime = GetLocalDateTime();

                //            if (dtCurrTime >= dtStartTime && dtCurrTime <= dtEndTime)
                if (dtCurrTime <= dtEndTime)
                {
                    timespan = dtEndTime.Subtract(dtCurrTime);
                    strElapse = timespan.TotalMilliseconds.ToString();
                    nElapse = Convert.ToDouble(strElapse);
                }
            }
            return nElapse;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input">yyyy-MM-dd hh:mm:ss.s</param>
        /// <returns></returns>
        private static DateTime ConvertStringToDateTime(string input)
        {
            //if (input.Length != 21)
            //    return GeneralBll.GetLocalDateTime();
            DateTime dtDate = DateTime.Now; ;
            try
            {
                var year = input.Substring(0, 4);
                var month = input.Substring(5, 2);
                var day = input.Substring(8, 2);
                var hour = input.Substring(11, 2);
                var minute = input.Substring(14, 2);
                var second = input.Substring(17, 2);

                dtDate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day),
                                    Convert.ToInt32(hour), Convert.ToInt32(minute), Convert.ToInt32(second));
                return dtDate;
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("Error GeneralBll ConvertStringToDateTime : " + ex.Message, Enums.LogType.Error);
                //return GeneralBll.GetLocalDateTime();
                return dtDate;
            }

        }

        public static DateTime GetLocalDateTime()
        {
            return DateTime.Now;
        }


    }
}
