using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.Classes;

namespace AndroidCompound5
{
    public class InfoBll
    {
        public static InfoDto GetInfo()
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.InfoDat;

            if (!System.IO.File.Exists(strFullFileName))
                return null;

            var info = InfoAccess.GetInfoAccess(strFullFileName);
            if (string.IsNullOrEmpty(info.DolphinId) ||
                info.DolphinId == "  " ||
                info.DolphinId == "00")
                return null;
            return info;
        }

        public static void UpdateInfo(InfoDto infoDto, Enums.FormName formName)
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.InfoDat;

            InfoAccess.UpdateInfoAccess(strFullFileName, infoDto, formName);
        }

        public static InfoDto ReInitializeCounter()
        {

            var localdate = GeneralBll.GetLocalDateTime().ToString("yyyyMMdd");

            var infoDto = GetInfo();
            if (infoDto == null)
                return null;

            infoDto.CurrDate = localdate;
            //            var nokmp = "H"+ infoDto.DolphinId + localdate.Substring(2, 6) + "001     ";
            //var nokmp = "L"+ infoDto.DolphinId + "000001 ";
            var nokmp = Constants.DevicePrefix + infoDto.DolphinId + infoDto.CurrDate + "001";

            infoDto.StartCmp = nokmp ;
            
            infoDto.LogDate = "        ";
            infoDto.LogTime = "    ";
            infoDto.EnforcerId = "      ";
            infoDto.CurrMukim = "  ";
            infoDto.CurrZone = "    ";
            infoDto.Council = "          ";
            infoDto.AssignZone = "                    ";
            infoDto.BroadMsg = "                                                            ";
            infoDto.StartSita = "                    ";   
            
            infoDto.CurrComp = 0;
            infoDto.CurrSita = 900000;
            infoDto.CompCnt = 0;			// Total compound issued
            infoDto.NoteSize = 0;			// Total note count in bytes
            infoDto.NoteCnt = 0;			// Total note 
            infoDto.SitaCnt = 0;			// Total Sitaan issued
            infoDto.NoticeCnt = 0;		// Total Notice issued
            infoDto.PhotoCnt = 0;			// Total Photo captured
            infoDto.NoteSize = 0;			// Total note count in bytes
            infoDto.NoteCnt = 0;            // Total note 
            infoDto.CurrRcpNum = 0;
            infoDto.RcpCnt = 0;
            UpdateInfo(infoDto, Enums.FormName.InfoBll);

            // Clear the transfiles, images and send on line folder
            GeneralBll.InitFileTrans();

            GeneralBll.InitFileImages();

            GeneralBll.InitFileSendOnLine();

            return infoDto;
        }

    }
}