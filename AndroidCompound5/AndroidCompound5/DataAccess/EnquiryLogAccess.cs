using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.DataAccess;


namespace AndroidCompound5
{
    public static class EnquiryLogAccess
    {
		[Obsolete("This method is deprecated. Please use NewMethod() instead.")]
		public static List<LogServerDto> GetEnquiryLogAccess(string strFullFileName)
		{
			var objStream = new StreamReader(strFullFileName);

			var listEnquiryLog = new List<LogServerDto>();

			string sLine = "";
			int len = 0;
			while ((sLine = objStream.ReadLine()) != null)
			{
				if (sLine.Length > 0)
				{

					var enquirylog = new LogServerDto();

					enquirylog.CarNo = GeneralUtils.ValidateRecordByLine(sLine, 0, 15, ref len);
					enquirylog.Date = GeneralUtils.ValidateRecordByLine(sLine, len, 8, ref len);
					enquirylog.Time = GeneralUtils.ValidateRecordByLine(sLine, len, 6, ref len);
					enquirylog.DolphinId = GeneralUtils.ValidateRecordByLine(sLine, len, 2, ref len);
					enquirylog.EnforcerId = GeneralUtils.ValidateRecordByLine(sLine, len, 7, ref len);
					enquirylog.Zone = GeneralUtils.ValidateRecordByLine(sLine, len, 6, ref len);
					enquirylog.Sector = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len);
					enquirylog.Street = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len);
					enquirylog.Status = GeneralUtils.ValidateRecordByLine(sLine, len, 1, ref len);
					enquirylog.End = GeneralUtils.ValidateRecordByLine(sLine, len, 2, ref len);
					enquirylog.Latitude = GeneralUtils.ValidateRecordByLine(sLine, len, 15, ref len);
					enquirylog.Longitude = GeneralUtils.ValidateRecordByLine(sLine, len, 15, ref len);

					listEnquiryLog.Add(enquirylog);
				}
			}

			objStream.Close();
			objStream.Dispose();

			return listEnquiryLog;
		}

		public static void AddEnquiryLogFil(LogServerDto EnquiryLog)
        {
            DbContextProvider.Instance.LogServers.Add(EnquiryLog);
        }

        public static List<LogServerDto> GetEnquiryLogAccess()
        {
            return [.. DbContextProvider.Instance.LogServers];
        }
    }
}