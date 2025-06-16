using AndroidCompound5.BusinessObject.DTOs;
using System.Collections.Generic;
using AndroidCompound5.Classes;
using System.Linq;
using AndroidCompound5.AimforceUtils;

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
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.TableFil;

            if (!System.IO.File.Exists(strFullFileName))
                return null;

            return TableFilAccess.GetCarCategoryAccess(strFullFileName).FirstOrDefault(c => c.Carcategory == code);
        }

        public static List<CarCategoryDto> GetAllCarCategory()
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.TableFil;

            if (!System.IO.File.Exists(strFullFileName))
                return null;

            return TableFilAccess.GetCarCategoryAccess(strFullFileName);
        }

        public static List<DeliveryDto> GetAllDelivery()
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.TableFil;

            var listDelivery = new List<DeliveryDto>();
            if (!System.IO.File.Exists(strFullFileName))
                return listDelivery;

            return TableFilAccess.GetDeliveryAccess(strFullFileName);
        }

        public static DeliveryDto GetDeliveryByCode(string code)
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.TableFil;

            if (!System.IO.File.Exists(strFullFileName))
                return null;

            return TableFilAccess.GetDeliveryAccess(strFullFileName).FirstOrDefault(c => c.Code == code);
        }

        public static List<CarTypeDto> GetAllCarType()
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.TableFil;

            var listCarType = new List<CarTypeDto>();
            if (!System.IO.File.Exists(strFullFileName))
                return listCarType;

            return TableFilAccess.GetCarTypeAccess(strFullFileName);
        }

        public static CarTypeDto GetCarTypeByCode(string code, string category)
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.TableFil;

            if (!System.IO.File.Exists(strFullFileName))
                return null;

            return TableFilAccess.GetCarTypeAccess(strFullFileName).FirstOrDefault(c => c.Code == code && c.CarcategoryCode == category);
        }

        public static List<CarColorDto> GetAllCarColor()
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.TableFil;

            var listCarColor = new List<CarColorDto>();
            if (!System.IO.File.Exists(strFullFileName))
                return listCarColor;

            return TableFilAccess.GetCarColorAccess(strFullFileName);
        }

        public static CarColorDto GetCarColorById(string id)
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.TableFil;

            if (!System.IO.File.Exists(strFullFileName))
                return null;

            return TableFilAccess.GetCarColorAccess(strFullFileName).FirstOrDefault(c => c.Code == id);
        }

        public static List<MukimDto> GetAllMukim()
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.TableFil;

            var listMukim = new List<MukimDto>();
            if (!System.IO.File.Exists(strFullFileName))
                return listMukim;

            return TableFilAccess.GetMukimAccess(strFullFileName);
        }

        public static MukimDto GetMukimByCode(string mukim)
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.TableFil;


            if (!System.IO.File.Exists(strFullFileName))
                return null;

            var listMukim = TableFilAccess.GetMukimAccess(strFullFileName);
            return listMukim.FirstOrDefault(c => c.Code == mukim);
        }

        public static List<ZoneDto> GetZoneByMukim(string mukim)
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.TableFil;

            var listZone = new List<ZoneDto>();
            if (!System.IO.File.Exists(strFullFileName))
                return listZone;

            return TableFilAccess.GetZoneAccess(strFullFileName).Where(c => c.Mukim == mukim).ToList();
        }

        public static ZoneDto GetZoneByCodeAndMukim(string code, string mukim)
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.TableFil;


            if (!System.IO.File.Exists(strFullFileName))
                return null;

            return TableFilAccess.GetZoneAccess(strFullFileName).FirstOrDefault(c => c.Code == code && c.Mukim == mukim);
        }

        public static List<ActDto> GetAllAct()
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.TableFil;

            var listAct = new List<ActDto>();
            if (!System.IO.File.Exists(strFullFileName))
                return listAct;

            return TableFilAccess.GetActAccess(strFullFileName);
        }

        public static ActDto GetActByCode(string code)
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.TableFil;

            if (!System.IO.File.Exists(strFullFileName))
                return null;

            return TableFilAccess.GetActAccess(strFullFileName).FirstOrDefault(c => c.Code == code);
        }

        public static List<OffendDto> GetOffendByActCode(string actCode)
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.TableFil;

            var listOffend = new List<OffendDto>();
            if (!System.IO.File.Exists(strFullFileName))
                return listOffend;

            return TableFilAccess.GetOffendAccess(strFullFileName).Where(c => c.ActCode == actCode).ToList();
        }

        public static List<OffendDto> GetOffendByOffCodeActCode(string offCode, string actCode)
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.TableFil;

            var listOffend = new List<OffendDto>();
            if (!System.IO.File.Exists(strFullFileName))
                return listOffend;

            return TableFilAccess.GetOffendAccess(strFullFileName).Where(c => c.ActCode == actCode && c.OfdCode == offCode).ToList();
        }

        public static OffendDto GetOffendByCodeAndAct(string code, string actCode)
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.TableFil;


            if (!System.IO.File.Exists(strFullFileName))
                return null;

            return TableFilAccess.GetOffendAccess(strFullFileName).FirstOrDefault(c => c.OfdCode == code && c.ActCode == actCode);
        }


        public static List<OffendDto> GetAllOffend()
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.TableFil;

            var listOffend = new List<OffendDto>();
            if (!System.IO.File.Exists(strFullFileName))
                return listOffend;

            return TableFilAccess.GetOffendAccess(strFullFileName);
        }

        public static string GetOffendDesc(string offendCode, string actCode, List<OffendDto> listOffend)
        {

            var offend = listOffend.FirstOrDefault(c => c.OfdCode == offendCode && c.ActCode == actCode);
            if (offend == null) return string.Empty;
            return offend.PrnDesc;
        }


        public static List<TempatJadiDto> GetAllTempatJadi()
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.TableFil;

            var listTempatJadi = new List<TempatJadiDto>();
            if (!System.IO.File.Exists(strFullFileName))
                return listTempatJadi;

            return TableFilAccess.GetTempatJadiAccess(strFullFileName);
        }

        public static TempatJadiDto GetTempatJadi(string code)
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.TableFil;

            if (!System.IO.File.Exists(strFullFileName))
                return null;

            return TableFilAccess.GetTempatJadiAccess(strFullFileName).FirstOrDefault(c=>c.Code == code);
        }

        public static HandheldDto GetHandheldByCode(string enfid, string handheldid)
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.HandheldFil;

            if (!System.IO.File.Exists(strFullFileName))
                return null;

            return HandheldAccess.GetHandheldAccess(strFullFileName).FirstOrDefault(c => c.HandheldID == handheldid && c.EnfID == enfid);
        }
        public static OffrateDto GetOffrateByCode(string ofdcode, string actcode, string catcategory)
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.OffRateFil ;

            if (!System.IO.File.Exists(strFullFileName))
                return null;

            return OffrateAccess.GetOffRateAccess(strFullFileName).FirstOrDefault(c => c.ActCode == actcode && c.OfdCode == ofdcode && c.CarCategory == catcategory);
        }

        public static MessageDto GetMessage()
        {
            string strFullFileName = GeneralAndroidClass.GetExternalStorageDirectory();
            strFullFileName += Constants.ProgramPath + Constants.MasterPath + Constants.MessageFil;

            if (!System.IO.File.Exists(strFullFileName))
                return null;

            return MessageAccess.GetMessageAccess(strFullFileName);
        }
    }
}