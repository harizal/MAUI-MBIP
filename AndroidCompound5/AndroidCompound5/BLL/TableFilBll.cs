using AndroidCompound5.BusinessObject.DTOs;

namespace AndroidCompound5
{
    public static class TableFilBll
    {

        //public static List<BarangSitaDto> GetBarangSitaByFlag(string flag)
        //{
        //    string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
        //    strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.TableFil;

        //    var listBarangSita = new List<BarangSitaDto>();
        //    if (!System.IO.File.Exists(strFullFileName))
        //        return listBarangSita;

        //    return TableFilAccess.GetBarangSitaAccess(strFullFileName).Where(c => c.Flag == flag).ToList();
        //}

        public static CarCategoryDto GetCarCategory(string code)
        {
             return TableFilAccess.GetCarCategoryAccess().FirstOrDefault(c => c.Carcategory == code);
        }

        public static List<CarCategoryDto> GetAllCarCategory()
        {
            return TableFilAccess.GetCarCategoryAccess();
        }

        public static List<DeliveryDto> GetAllDelivery()
        {
            return TableFilAccess.GetDeliveryAccess();
        }

        public static DeliveryDto GetDeliveryByCode(string code)
        {
           return TableFilAccess.GetDeliveryAccess().FirstOrDefault(c => c.Code == code);
        }

        public static List<CarTypeDto> GetAllCarType()
        {
            return TableFilAccess.GetCarTypeAccess();
        }

        public static CarTypeDto GetCarTypeByCode(string code, string category)
        {
            return TableFilAccess.GetCarTypeAccess().FirstOrDefault(c => c.Code == code && c.CarcategoryCode == category);
        }

        public static List<CarColorDto> GetAllCarColor()
        {
            return TableFilAccess.GetCarColorAccess();
        }

        public static CarColorDto GetCarColorById(string id)
        {
            return TableFilAccess.GetCarColorAccess().FirstOrDefault(c => c.Code == id);
        }

        public static List<MukimDto> GetAllMukim()
        {
            return TableFilAccess.GetMukimAccess();
        }

        public static MukimDto GetMukimByCode(string mukim)
        {
          var listMukim = TableFilAccess.GetMukimAccess();
            return listMukim.FirstOrDefault(c => c.Code == mukim);
        }

        public static List<ZoneDto> GetZoneByMukim(string mukim)
        {
           return TableFilAccess.GetZoneAccess().Where(c => c.Mukim == mukim).ToList();
        }

        public static ZoneDto GetZoneByCodeAndMukim(string code, string mukim)
        {
            return TableFilAccess.GetZoneAccess().FirstOrDefault(c => c.Code == code && c.Mukim == mukim);
        }

        public static List<ActDto> GetAllAct()
        {
            return TableFilAccess.GetActAccess();
        }

        public static ActDto GetActByCode(string code)
        {
            return TableFilAccess.GetActAccess().FirstOrDefault(c => c.Code == code);
        }

        public static List<OffendDto> GetOffendByActCode(string actCode)
        {
            return TableFilAccess.GetOffendAccess().Where(c => c.ActCode == actCode).ToList();
        }

        public static List<OffendDto> GetOffendByOffCodeActCode(string offCode, string actCode)
        {
            return TableFilAccess.GetOffendAccess().Where(c => c.ActCode == actCode && c.OfdCode == offCode).ToList();
        }

        public static OffendDto GetOffendByCodeAndAct(string code, string actCode)
        {
            return TableFilAccess.GetOffendAccess().FirstOrDefault(c => c.OfdCode == code && c.ActCode == actCode);
        }


        public static List<OffendDto> GetAllOffend()
        {
            return TableFilAccess.GetOffendAccess();
        }

        public static string GetOffendDesc(string offendCode, string actCode, List<OffendDto> listOffend)
        {
            var offend = listOffend.FirstOrDefault(c => c.OfdCode == offendCode && c.ActCode == actCode);
            if (offend == null) return string.Empty;
            return offend.PrnDesc;
        }


        public static List<TempatJadiDto> GetAllTempatJadi()
        {
                        return TableFilAccess.GetTempatJadiAccess();
        }

        public static TempatJadiDto GetTempatJadi(string code)
        {
                        return TableFilAccess.GetTempatJadiAccess().FirstOrDefault(c=>c.Code == code);
        }

        public static HandheldDto GetHandheldByCode(string enfid, string handheldid)
        {
            return HandheldAccess.GetHandheldAccess().FirstOrDefault(c => c.HandheldID == handheldid && c.EnfID == enfid);
        }
        public static OffrateDto GetOffrateByCode(string ofdcode, string actcode, string catcategory)
        {
            return OffrateAccess.GetOffRateAccess().FirstOrDefault(c => c.ActCode == actcode && c.OfdCode == ofdcode && c.CarCategory == catcategory);
        }

        public static MessageDto GetMessage()
        {
            return MessageAccess.GetMessageAccess();
        }
    }
}