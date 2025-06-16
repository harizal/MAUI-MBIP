using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AndroidCompound5
{
    public static class CompoundAccess
    {
        public static bool AddCompoundAccess(string strFullFileName, string compoundLine, string compoundNumber)
        {
            try
            {
                var fileStream = new FileStream(strFullFileName, FileMode.Append, FileAccess.Write, FileShare.None);
                var objCompound = new StreamWriter(fileStream);
                objCompound.Write(compoundLine);
                objCompound.Flush();

                fileStream.Flush(true); // true ensures the OS buffer is flushed (requires .NET Core or newer Xamarin)
                fileStream.Close();
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("Error WriteCompound CompNum : " + compoundNumber, Enums.LogType.Error);
                LogFile.WriteLogFile("Error WriteCompound Message : " + ex.Message, Enums.LogType.Error);
                return false;
            }
        }

        public static List<CompoundDto> GetCompoundAccess(string strFullFileName)
        {
            var objStream = new StreamReader(strFullFileName);

            var listCompound = new List<CompoundDto>();

            string sLine = "";
            int len = 0;

            while ((sLine = objStream.ReadLine()) != null)
            {
                if (sLine.Length > 0)
                {

                    var compound = new CompoundDto();

                    compound.Deleted = GeneralUtils.ValidateRecordByLine(sLine, 0, 1, ref len);
                    compound.CompNum = GeneralUtils.ValidateRecordByLine(sLine, len, 20, ref len).Trim();
                    compound.CompType = GeneralUtils.ValidateRecordByLine(sLine, len, 1, ref len).Trim();
                    compound.ActCode = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim();
                    compound.OfdCode = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim();
                    compound.Mukim = GeneralUtils.ValidateRecordByLine(sLine, len, 6, ref len).Trim();
                    compound.Zone = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim();
                    compound.StreetCode = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim();
                    compound.StreetDesc = GeneralUtils.ValidateRecordByLine(sLine, len, 100, ref len).Trim();
                    compound.CompDate = GeneralUtils.ValidateRecordByLine(sLine, len, 8, ref len).Trim();
                    compound.CompTime = GeneralUtils.ValidateRecordByLine(sLine, len, 6, ref len).Trim();
                    compound.EnforcerId = GeneralUtils.ValidateRecordByLine(sLine, len, 7, ref len).Trim();
                    compound.WitnessId = GeneralUtils.ValidateRecordByLine(sLine, len, 7, ref len).Trim();
                    compound.PubWitness = GeneralUtils.ValidateRecordByLine(sLine, len, 60, ref len).Trim();
                    compound.PrintCnt = GeneralUtils.ValidateRecordByLine(sLine, len, 1, ref len).Trim();
                    compound.Tempatjadi = GeneralUtils.ValidateRecordByLine(sLine, len, 150, ref len).Trim();
                    compound.Tujuan = GeneralUtils.ValidateRecordByLine(sLine, len, 100, ref len).Trim();
                    compound.Perniagaan = GeneralUtils.ValidateRecordByLine(sLine, len, 100, ref len).Trim();
                    compound.Kadar = GeneralUtils.ValidateRecordByLine(sLine, len, 1, ref len).Trim();
                    compound.GpsX = GeneralUtils.ValidateRecordByLine(sLine, len, 15, ref len).Trim();
                    compound.GpsY = GeneralUtils.ValidateRecordByLine(sLine, len, 15, ref len).Trim();
                    compound.TempohDate = GeneralUtils.ValidateRecordByLine(sLine, len, 8, ref len).Trim();

                    if (compound.CompType == Constants.CompType1)
                    {
                        compound.Compound1Type.CarNum = GeneralUtils.ValidateRecordByLine(sLine, len, 15, ref len).Trim();
                        compound.Compound1Type.Category = GeneralUtils.ValidateRecordByLine(sLine, len, 2, ref len).Trim();
                        compound.Compound1Type.CarType = GeneralUtils.ValidateRecordByLine(sLine, len, 3, ref len).Trim();
                        compound.Compound1Type.CarTypeDesc = GeneralUtils.ValidateRecordByLine(sLine, len, 40, ref len).Trim();
                        compound.Compound1Type.CarColor = GeneralUtils.ValidateRecordByLine(sLine, len, 2, ref len).Trim();
                        compound.Compound1Type.CarColorDesc = GeneralUtils.ValidateRecordByLine(sLine, len, 40, ref len).Trim();
                        compound.Compound1Type.LotNo = GeneralUtils.ValidateRecordByLine(sLine, len, 15, ref len).Trim();

                        compound.Compound1Type.RoadTax = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim();
                        compound.Compound1Type.RoadTaxDate = GeneralUtils.ValidateRecordByLine(sLine, len, 8, ref len).Trim();

                        compound.Compound1Type.CompAmt = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim();
                        compound.Compound1Type.CompAmt2 = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim();
                        compound.Compound1Type.CompAmt3 = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim();
                        compound.Compound1Type.CouponNumber = GeneralUtils.ValidateRecordByLine(sLine, len, 15, ref len).Trim();
                        compound.Compound1Type.CouponDate = GeneralUtils.ValidateRecordByLine(sLine, len, 8, ref len).Trim();
                        compound.Compound1Type.CouponTime = GeneralUtils.ValidateRecordByLine(sLine, len, 4, ref len).Trim();
                        compound.Compound1Type.ScanDateTime = GeneralUtils.ValidateRecordByLine(sLine, len, 19, ref len).Trim();
                        compound.Compound1Type.DeliveryCode = GeneralUtils.ValidateRecordByLine(sLine, len, 2, ref len).Trim();
                        compound.Compound1Type.CompDesc = GeneralUtils.ValidateRecordByLine(sLine, len, 600, ref len).Trim();
                    }
                    else if (compound.CompType == Constants.CompType2)
                    {
                        compound.Compound2Type.CarNum = GeneralUtils.ValidateRecordByLine(sLine, len, 15, ref len).Trim();
                        compound.Compound2Type.Category = GeneralUtils.ValidateRecordByLine(sLine, len, 2, ref len).Trim();
                        compound.Compound2Type.CarType = GeneralUtils.ValidateRecordByLine(sLine, len, 3, ref len).Trim();
                        compound.Compound2Type.CarTypeDesc = GeneralUtils.ValidateRecordByLine(sLine, len, 40, ref len).Trim();
                        compound.Compound2Type.CarColor = GeneralUtils.ValidateRecordByLine(sLine, len, 2, ref len).Trim();
                        compound.Compound2Type.CarColorDesc = GeneralUtils.ValidateRecordByLine(sLine, len, 40, ref len).Trim();
                        compound.Compound2Type.RoadTax = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim();
                        compound.Compound2Type.CompAmt = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim();
                        compound.Compound2Type.CompAmt2 = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim();
                        compound.Compound2Type.CompAmt3 = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim();

                        compound.Compound2Type.DeliveryCode = GeneralUtils.ValidateRecordByLine(sLine, len, 2, ref len).Trim();
                        compound.Compound2Type.Muatan = GeneralUtils.ValidateRecordByLine(sLine, len, 20, ref len).Trim();
                        compound.Compound2Type.CompDesc = GeneralUtils.ValidateRecordByLine(sLine, len, 600, ref len).Trim();
                    }
                    else if (compound.CompType == Constants.CompType3)
                    {
                        compound.Compound3Type.Rujukan = GeneralUtils.ValidateRecordByLine(sLine, len, 15, ref len).Trim();
                        compound.Compound3Type.Company = GeneralUtils.ValidateRecordByLine(sLine, len, 20, ref len).Trim();
                        compound.Compound3Type.CompanyName = GeneralUtils.ValidateRecordByLine(sLine, len, 60, ref len).Trim();
                        compound.Compound3Type.OffenderIc = GeneralUtils.ValidateRecordByLine(sLine, len, 16, ref len).Trim();
                        compound.Compound3Type.OffenderName = GeneralUtils.ValidateRecordByLine(sLine, len, 60, ref len).Trim();
                        compound.Compound3Type.Address1 = GeneralUtils.ValidateRecordByLine(sLine, len, 80, ref len).Trim();
                        compound.Compound3Type.Address2 = GeneralUtils.ValidateRecordByLine(sLine, len, 80, ref len).Trim();
                        compound.Compound3Type.Address3 = GeneralUtils.ValidateRecordByLine(sLine, len, 170, ref len).Trim();


                        compound.Compound3Type.CompAmt = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim();
                        compound.Compound3Type.CompAmt2 = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim();
                        compound.Compound3Type.CompAmt3 = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim();

                        compound.Compound3Type.DeliveryCode = GeneralUtils.ValidateRecordByLine(sLine, len, 2, ref len).Trim();
                        compound.Compound3Type.CompDesc = GeneralUtils.ValidateRecordByLine(sLine, len, 600, ref len).Trim();
                    }
                    else if (compound.CompType == Constants.CompType4)
                    {
                        compound.Compound4Type.Rujukan = GeneralUtils.ValidateRecordByLine(sLine, len, 15, ref len).Trim();
                        compound.Compound4Type.OffenderIc = GeneralUtils.ValidateRecordByLine(sLine, len, 16, ref len).Trim();
                        compound.Compound4Type.OffenderName = GeneralUtils.ValidateRecordByLine(sLine, len, 60, ref len).Trim();
                        compound.Compound4Type.No = GeneralUtils.ValidateRecordByLine(sLine, len, 15, ref len).Trim();
                        compound.Compound4Type.Building = GeneralUtils.ValidateRecordByLine(sLine, len, 30, ref len).Trim();
                        compound.Compound4Type.Jalan = GeneralUtils.ValidateRecordByLine(sLine, len, 30, ref len).Trim();
                        compound.Compound4Type.Taman = GeneralUtils.ValidateRecordByLine(sLine, len, 30, ref len).Trim();
                        compound.Compound4Type.PostCode = GeneralUtils.ValidateRecordByLine(sLine, len, 5, ref len).Trim();
                        compound.Compound4Type.City = GeneralUtils.ValidateRecordByLine(sLine, len, 20, ref len).Trim();
                        compound.Compound4Type.State = GeneralUtils.ValidateRecordByLine(sLine, len, 20, ref len).Trim();
                        compound.Compound4Type.CompAmt = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim();
                        compound.Compound4Type.CompAmt2 = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim();
                        compound.Compound4Type.CompAmt3 = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim();

                        compound.Compound4Type.StorageAmt = GeneralUtils.ValidateRecordByLine(sLine, len, 6, ref len).Trim();
                        compound.Compound4Type.TransportAmt = GeneralUtils.ValidateRecordByLine(sLine, len, 6, ref len).Trim();
                        compound.Compound4Type.Remark = GeneralUtils.ValidateRecordByLine(sLine, len, 60, ref len).Trim();
                    }
                    else if (compound.CompType == Constants.CompType5)
                    {
                        compound.Compound5Type.CarNum = GeneralUtils.ValidateRecordByLine(sLine, len, 15, ref len).Trim();
                        compound.Compound5Type.Category = GeneralUtils.ValidateRecordByLine(sLine, len, 2, ref len).Trim();
                        compound.Compound5Type.CarType = GeneralUtils.ValidateRecordByLine(sLine, len, 3, ref len).Trim();
                        compound.Compound5Type.CarTypeDesc = GeneralUtils.ValidateRecordByLine(sLine, len, 40, ref len).Trim();
                        compound.Compound5Type.CarColor = GeneralUtils.ValidateRecordByLine(sLine, len, 2, ref len).Trim();
                        compound.Compound5Type.CarColorDesc = GeneralUtils.ValidateRecordByLine(sLine, len, 40, ref len).Trim();
                        compound.Compound5Type.RoadTax = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim();
                        compound.Compound5Type.CompAmt = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim();
                        compound.Compound5Type.CompAmt2 = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim();
                        compound.Compound5Type.CompAmt3 = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len).Trim();
                        compound.Compound5Type.DeliveryCode = GeneralUtils.ValidateRecordByLine(sLine, len, 2, ref len).Trim();
                        compound.Compound5Type.Muatan = GeneralUtils.ValidateRecordByLine(sLine, len, 20, ref len).Trim();
                        compound.Compound5Type.CompDesc = GeneralUtils.ValidateRecordByLine(sLine, len, 600, ref len).Trim();
                        compound.Compound5Type.LockTime = GeneralUtils.ValidateRecordByLine(sLine, len, 4, ref len).Trim();
                        compound.Compound5Type.LockKey = GeneralUtils.ValidateRecordByLine(sLine, len, 5, ref len).Trim();
                        compound.Compound5Type.UnlockAmt = GeneralUtils.ValidateRecordByLine(sLine, len, 7, ref len).Trim();
                        compound.Compound5Type.TowAmt = GeneralUtils.ValidateRecordByLine(sLine, len, 7, ref len).Trim();
                    }

                    listCompound.Add(compound);

                }
            }

            objStream.Close();
            objStream.Dispose();

            return listCompound;
        }


        public static bool AddCompoundAccess(string strFullFileName, StringBuilder compoundLine, string compoundNumber)
        {
            try
            {
                var fileStream = new FileStream(strFullFileName, FileMode.Append, FileAccess.Write, FileShare.None);
                var objCompound = new StreamWriter(fileStream);
                objCompound.Write(compoundLine);
                objCompound.Flush();
                fileStream.Flush(true); // true ensures the OS buffer is flushed (requires .NET Core or newer Xamarin)
                fileStream.Close();
                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("Error WriteCompound CompNum : " + compoundNumber, Enums.LogType.Error);
                LogFile.WriteLogFile("Error WriteCompound Message : " + ex.Message, Enums.LogType.Error);
                return false;
            }
        }
    }
}