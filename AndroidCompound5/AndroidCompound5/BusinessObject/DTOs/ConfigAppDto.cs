
namespace AndroidCompound5.BusinessObject.DTOs
{
    public class ConfigAppDto
    {
        public string ServiceUrl { get; set; }
        public string ServiceAppletUrl { get; set; }
        public string ServiceKey { get; set; }
        public string FtpHost { get; set; }
        public string FtpUser { get; set; }
        public string FtpPassword { get; set; }
        public string FtpUnload { get; set; }
        public string FtpDownload { get; set; }
        public string FtpControl { get; set; }
        public bool GpsLog { get; set; }
        public int GpsInterval { get; set; }
        public int SendCompoundInterval { get; set; }
        public int ImgsWidth { get; set; }
        public int ImgsHeight { get; set; }
        public string ServiceUser { get; set; }
        public string ServicePassword { get; set; }
        public long VideoMaxDuration { get; set; }
        public long VideoMaxFileSize { get; set; }
        public string ApkFileName { get; set; }
        public string UrlPhoto { get; set; } = "http://1.9.46.170:8081/mpnswsparkingphoto/ws-servlet/WSModule/uploadPhoto";
        public string UrlImage { get; set; } = "http://1.9.46.172:38082/home/search?";
        public string ApkFtpUrl { get; set; } = "ftp://1.9.46.170:521/";

        public string ServiceUPSBUrl { get; set; }
        public string CompanyID { get; set; } 
        public string ParkCode { get; set; } 
        public string AppID{ get; set; } 
    }
}
