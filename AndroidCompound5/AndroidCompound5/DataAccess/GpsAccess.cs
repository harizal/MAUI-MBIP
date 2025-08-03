using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.DataAccess;

namespace AndroidCompound5
{
    public static class GpsAccess
    {
        public static void AddGpsAccess(GpsDto gpsDto)
        {
			DbContextProvider.Instance.Gps.Add(gpsDto);
        }
        public static List<GpsDto> GetGPSAccess(string strFullFileName)
        {
			var listGps = new List<GpsDto>();
			foreach (var gps in DbContextProvider.Instance.Gps)
			{
				if (gps.Issend == "N")
					listGps.Add(gps);
			}
            return listGps;
		}

    }
}