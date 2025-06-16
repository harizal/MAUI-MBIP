using System;
using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.Outputs;

namespace AndroidCompound5
{
    public static class DateTimeBll
    {
        public static OutputDateTime GetServerDateTime()
        {
            var output = new OutputDateTime();

            try
            {
               
                var configDto = GeneralBll.GetConfig();
                if (configDto == null)
                {
                    LogFile.WriteLogFile("Get Config null", Enums.LogType.Error);
                    return output;
                }

                
                var webService = new WebService(configDto.ServiceUrl, configDto.ServiceUser, configDto.ServicePassword);
                var result = webService.GetServerDateTime(configDto.ServiceKey);
                
                if (string.IsNullOrEmpty(result))
                {
                    output.Result = false;
                    output.Message = "Got Empty from server";
                    output.dtTime = DateTime.Now;

                }
                else
                {
                    LogFile.WriteLogFile("result DateTime Server : " + result);
                    output.Result = true;
                    output.dtTime = Convert.ToDateTime(result);
                }
                return output;


            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("BLL Err GetServerDateTime : " + ex.Message, Enums.LogType.Error);
                output.Result = false;
                output.Message = ex.Message;
                return output;
            }

        }
    }
}