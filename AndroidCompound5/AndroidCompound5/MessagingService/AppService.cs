using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AndroidCompound5
{
    public class AppService
    {
        public async Task<string> PostAsync(List<CompoundImageDto> param, string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(param);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var result = await client.PostAsync(string.IsNullOrEmpty(url) ? "http://1.9.46.170:8081/mpnswsparkingphoto/ws-servlet/WSModule/uploadPhoto" : url, content);
                    // on error throw a exception  
                    result.EnsureSuccessStatusCode();

                    // handling the answer  
                    return await result.Content.ReadAsStringAsync();
                }
            }
            catch (System.Exception ex)
            {
                LogFile.WriteLogFile("uploadPhoto URL : " + url + "  Error : " + ex.Message);
                return string.Empty;
            }
        }
    }
}