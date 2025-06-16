using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AndroidCompound5.Classes;
using Android.Runtime;
using Android.Bluetooth;
using AndroidCompound5.BusinessObject.DTOs;
using Java.Util;
using System.Threading;
using AndroidCompound5.AimforceUtils;

namespace AndroidCompound5
{
    public class PrintBll
    {
        private BluetoothSocket socket;
        private BluetoothAdapter adapter;
        private Stream oStream;
        private Stream iStream;
        public List<BluetoothDevice> _listDevice;
        private byte[] _printerResponse = new byte[1024];

        //private const int MaxLengthPaper = 82;
        private const int MaxLengthPaper = 60;

        private const string PrintAddedConstant = "";

        private int iFontSize = 24, nLine = 0 ;
        private string strTTFName = "";

        public PrintBll()
        {
            oStream = null;
            adapter = BluetoothAdapter.DefaultAdapter;

            if (!adapter.IsEnabled)
            {
                adapter.Enable();
            }
        }

        private bool CheckAdapterIsNull(BluetoothAdapter adapter)
        {
            return adapter == null;
        }

        public ResponseBluetoothDevices BluetoothConnect(BluetoothDevice bluetoothDevice)
        {
            var response = new ResponseBluetoothDevices();

            int retries = 0;
            bool isConnected = false;
            string message = "";

            do
            {
                try
                {

                    // Check adapter

                    if (CheckAdapterIsNull(adapter))
                    {
                        // If the adapter is null.
                        response.Succes = false;
                        response.Message = "No bluetooth adapter available.";
                        return response;
                    }

                    socket = bluetoothDevice.CreateRfcommSocketToServiceRecord(UUID.FromString(bluetoothDevice.GetUuids().ElementAt(0).ToString()));
                    //socket.Close();
                    if (socket.IsConnected)
                        socket.Close();
                    socket.Connect();
                    if (!socket.IsConnected)
                    {
                        response.Succes = false;
                        response.Message = "Cannot connect to socket";
                        LogFile.WriteLogFile("Cannot connect to socket");
                        return response;
                    }

                    oStream = socket.OutputStream;
                    iStream = socket.InputStream;
                    LogFile.WriteLogFile("oStream Assigned");
                    
                    response.Succes = true;
                    response.Message = string.Format("Bluetooth conected to {0}", bluetoothDevice.Name);

                    isConnected = true;
                }

                catch (Exception ex)
                {
                    message = ex.Message;
                    retries++;
                    Thread.Sleep(100);
                }

            } while (isConnected == false && retries < Constants.MaxPrintRetry);

            if (isConnected == false)
            {
                response.Succes = false;
                response.Message = "Can not connect to device, please check your device. ";
                if (message.Length > 0)
                    response.Message += message;
                return response;

            }

            return response;


        }
        public static void WritePrintFile(string strPrintLine)
        {
            string strFile = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath + "SAMPLE.DAT";

            var objWrite = new StreamWriter(strFile, true);
            objWrite.WriteLine(strPrintLine + "\r");
            objWrite.Close();
            objWrite.Dispose();
        }


        private string SetCenterText(string sValue)
        {
            try
            {
                int spaces = MaxLengthPaper - sValue.Length;
                int padLeft = spaces / 2 + sValue.Length;
                return sValue.PadLeft(padLeft).PadRight(MaxLengthPaper);

            }
            catch (Exception ex)
            {

                return sValue;
            }

        }

        private int PrintTextAlignment(string sValueText, String ConstantText, int MaxLength)
        {
            String strSpace = "                                                        ";
            bool bFirst = true;

            var sbPrint = new StringBuilder();

            if (MaxLength > MaxLengthPaper)
                MaxLength = MaxLengthPaper;

            if (sValueText.Length <= MaxLength)
            {
                sbPrint.Append(sValueText + "\r\n");
                PrintTextNoAdded(sbPrint.ToString());
                return 1;
            }
            int iLengtLine = 0;
            string[] sTemp = sValueText.Split(' ');

            int i = 0;
            int iCountLine = 0;
            int iLoop = 0;

            string sPrintTemp1 = "";

            while (i <= sTemp.GetUpperBound(0))
            {

                if (sTemp[i].Length >= MaxLength)
                {
                    sPrintTemp1 += sTemp[i] + "\r\n";

                    if (bFirst)
                        sbPrint.Append(sPrintTemp1);
                    else
                        sbPrint.Append(ConstantText + sPrintTemp1);
                    bFirst = false;
                    sPrintTemp1 = "";


                    int iDiv = sTemp[i].Length / MaxLength;
                    int iMod = sTemp[i].Length % MaxLength;
                    if (iMod > 0)
                        iDiv += 1;
                    iCountLine += iDiv;
                    i += 1;
                }
                else
                {
                    iLengtLine += sTemp[i].Length + 1;

                    if (iLengtLine <= MaxLength)
                    {
                        sPrintTemp1 += sTemp[i] + " ";
                        i += 1;
                        if (i > sTemp.GetUpperBound(0))
                        {
                            sPrintTemp1 += "\r\n";

                            sbPrint.Append(ConstantText + sPrintTemp1);
                            ConstantText = strSpace.Substring(1, ConstantText.Length);
                            iCountLine += 1;
                        }
                    }
                    else
                    {
                        sPrintTemp1 += "\r\n";
                        iLengtLine = 0;
                        iCountLine += 1;
                        sbPrint.Append(ConstantText + sPrintTemp1);
                        ConstantText = strSpace.Substring(1, ConstantText.Length);

                        sPrintTemp1 = "";
                    }
                }

                iLoop += 1;

                if (iLoop > 100) //somethink happen, continuous looping
                    break;

            }

            PrintTextNoAdded(sbPrint.ToString());
            return iCountLine;
        }

        private List<string> SeparateText(string sValueText, int lenList, int MaxLengthPaper)
        {
            List<string> listString = new List<string>();
            var sbPrint = new StringBuilder();

            if (sValueText.Length <= MaxLengthPaper)
            {
                listString.Add(sValueText);
                for (int j = listString.Count; j < lenList; j++)
                {
                    listString.Add("");
                }
                return listString;
            }
            int iLengtLine = 0;
            string[] sTemp = sValueText.Split(' ');

            int i = 0;
            int iCountLine = 0;
            int iLoop = 0;

            string sPrintTemp1 = "";

            while (i <= sTemp.GetUpperBound(0))
            {
                if (sTemp[i].Length >= MaxLengthPaper)
                {
                    listString.Add(sPrintTemp1);

                    sPrintTemp1 = "";

                    int iDiv = sTemp[i].Length / MaxLengthPaper;
                    int iMod = sTemp[i].Length % MaxLengthPaper;
                    if (iMod > 0)
                        iDiv += 1;
                    iCountLine += iDiv;
                    i += 1;
                }
                else
                {
                    iLengtLine += sTemp[i].Length + 1;

                    if (iLengtLine <= MaxLengthPaper)
                    {
                        sPrintTemp1 += sTemp[i] + " ";
                        i += 1;
                        if (i > sTemp.GetUpperBound(0))
                        {
                            listString.Add(sPrintTemp1);
                            iCountLine += 1;
                        }
                    }
                    else
                    {
                        iLengtLine = 0;
                        iCountLine += 1;
                        listString.Add(sPrintTemp1);
                        sPrintTemp1 = "";
                    }
                }


                iLoop += 1;

                if (iLoop > 100) //somethink happen, continuous looping
                    break;

            }
            for (int j = listString.Count; j < lenList; j++)
            {
                listString.Add("");
            }

            return listString;
        }

        private string FormatPrintAmount(string amount)
        {
            decimal decTemp = 0;
            string sResult = "";

            try
            {
                decTemp = Convert.ToDecimal(amount);
            }
            catch { }

            sResult = decTemp > 0 ? string.Format("RM{0}", (decTemp / 100).ToString("f")) : "RM";

            return sResult;
        }

        private string FormatPrintAmount1(string amount)
        {
            decimal decTemp = 0;
            string sResult = "";

            try
            {
                decTemp = Convert.ToDecimal(amount);
            }
            catch { }

            sResult = decTemp > 0 ? string.Format("{0}", (decTemp / 100).ToString("f")) : "";

            return sResult;
        }

        public void PrintCompound(string compoundNumber)
        {
            try
            {
                LogFile.WriteLogFile("PrintCompound : start");
                var compoundDto = CompoundBll.GetCompoundByCompoundNumber(compoundNumber);

                var offendDto = TableFilBll.GetOffendByCodeAndAct(compoundDto.OfdCode, compoundDto.ActCode);

                var actDto = TableFilBll.GetActByCode(compoundDto.ActCode);

                var enforcerDto = EnforcerBll.GetEnforcerById(compoundDto.EnforcerId);

                string strFile = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath + "SAMPLE.DAT";
                GeneralBll.DeleteFile(strFile, true);

                switch (compoundDto.CompType)
                {
                    case Constants.CompType1:
                        PrintCompound1(compoundDto, offendDto, actDto, enforcerDto);
                        break;
                    case Constants.CompType2:
                        PrintCompound2(compoundDto, offendDto, actDto, enforcerDto);
  
                        break;
                    case Constants.CompType3:
                        PrintCompound3(compoundDto, offendDto, actDto, enforcerDto);
                        break;
                    case Constants.CompType5:
                        PrintCompound5(compoundDto, offendDto, actDto, enforcerDto);
                        break;
                    default:
                        throw new Exception("No Compound Not Found.");

                }

                Thread.Sleep(1000);
                SensorCheck();
                PrintClose();
                LogFile.WriteLogFile("PrintCompound : End");
            }

            catch (Exception ex)
            {
                LogFile.WriteLogFile("PrintCompound :" + ex.Message);
                PrintClose();
            }
        }

        private void PrintCompound3(CompoundDto compoundDto, OffendDto offendDto, ActDto actDto, EnforcerDto enforcerDto)
        {

            int nLine = 0, i = 0;
            string saksiName = "";

            string formatPrintAmtWord = SetAmount2Word(compoundDto.Compound3Type.CompAmt);
            string formatPrintAmt = FormatPrintAmount(compoundDto.Compound3Type.CompAmt);
            string formatPrintAmt2 = FormatPrintAmount(compoundDto.Compound3Type.CompAmt2);
            string formatPrintAmt3 = FormatPrintAmount(compoundDto.Compound3Type.CompAmt3);
            string formatPrintDate = GeneralBll.FormatPrintDate(compoundDto.CompDate);
            string formatPrintTime = GeneralBll.FormatPrintTime(compoundDto.CompTime);

            string formatTempohDate;
            if (offendDto.PrintFlag == "S")
                formatTempohDate = AddDate(compoundDto.CompDate, 30);
            else
                formatTempohDate = AddDate(compoundDto.CompDate, 14);

            string formatTempohDate3Days = AddDate(compoundDto.CompDate, 2);
            string formatNotaTempohDate = GeneralBll.FormatPrintDate(compoundDto.TempohDate);

            var offendDesc = offendDto.PrnDesc + " " + offendDto.ShortDesc;

            var delivery = TableFilBll.GetDeliveryByCode(compoundDto.Compound3Type.DeliveryCode);
            if (delivery == null)
            {
                delivery = new DeliveryDto();
                delivery.ShortDesc = "Missing Code(" + compoundDto.Compound3Type.DeliveryCode + ")";
            }

            var saksi = EnforcerBll.GetEnforcerById(compoundDto.WitnessId);
            if (saksi == null)
                saksiName = " ";
            else
                saksiName = saksi.EnforcerName;

            InitialisePrinter();
            SetLeftMargin();
            SetLineSpacing(10);

            string strKodHasil = offendDto.IncomeCode.PadRight(10);
            string strNoKmp = compoundDto.CompNum;

            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            nLine = 8;
            SetBold(false);
            SetFontSize(15);
            SetBarCodeHight(45);
            PrintBarcode(strKodHasil);
            PrintText("                                                                      ");
            PrintBarcode(strNoKmp);
            SetLineSpacing(15);
            PrintLine();
            PrintLine();
            nLine = nLine + 2;

            SetBold(false);
            SetFontSize(30);
            PrintText("KOD HASIL : ");
            SetBold(true);
            PrintText(strKodHasil);
            PrintText("                     ");
            SetBold(false);
            PrintText("NO KOMPAUN : ");
            SetBold(true);
            PrintText(strNoKmp);
            PrintLine();
            PrintLine();
            nLine = nLine + 2;

            SetLineSpacing(5);
            SetBold(false);
            SetFontSize(26);
            PrintText("TARIKH : ");
            SetBold(true);
            SetBold(true);
            PrintText(formatPrintDate);
            PrintText("        ");
            SetBold(false);
            SetFontSize(26);
            PrintText("WAKTU  : ");
            SetBold(true);
            PrintText(formatPrintTime);
            PrintLine();
            PrintLine();
            nLine = nLine + 2;

            string strCompany = "";
            string strOffender = "";
            if (compoundDto.Compound3Type.CompanyName.TrimEnd() != "")
                strCompany = compoundDto.Compound3Type.CompanyName;

            if (compoundDto.Compound3Type.Company.TrimEnd() != "")
                strCompany = strCompany + "(" + compoundDto.Compound3Type.Company + ")";

            if (compoundDto.Compound3Type.Rujukan.TrimEnd() != "")
                strCompany = strCompany + "(" + compoundDto.Compound3Type.Rujukan + ")";

            if (compoundDto.Compound3Type.OffenderName.TrimEnd() != "")
                strOffender = compoundDto.Compound3Type.OffenderName;

            SetBold(false);
            SetFontSize(26);
            PrintText("KEPADA : ");
            SetBold(true);
            if (strCompany.TrimEnd() != "")
            {
                PrintText(strCompany);
                PrintLine();
                PrintLine();
                nLine = nLine + 2;

                PrintText("         " + strOffender);
                PrintLine();
                PrintLine();
                nLine = nLine + 2;
            }
            else
            {
                PrintText(strOffender);
                PrintLine();
                PrintLine();
                nLine = nLine + 2;
            }

            if (compoundDto.Compound3Type.OffenderIc.TrimEnd() != "")
            {
                PrintText("         " + compoundDto.Compound3Type.OffenderIc);
                PrintLine();
                nLine++;
            }

            PrintText("         " + compoundDto.Compound3Type.Address1 );
            PrintLine();
            nLine++;
            PrintText("         " + compoundDto.Compound3Type.Address2 );
            PrintLine();
            nLine++;
            PrintText("         " + compoundDto.Compound3Type.Address3 );
            PrintLine();
            nLine++;

            string tempatjadi;
            tempatjadi = compoundDto.Tempatjadi;

            var listStringStreet = SeparateText(tempatjadi, 6, 50);
            SetBold(false);
            PrintText("TEMPAT/LOKASI       : ");
            SetBold(true);
            PrintText(listStringStreet[0]);
            PrintLine();
            nLine++;
            if (listStringStreet[1].TrimEnd() != "")
            {
                PrintText("                   " + listStringStreet[1]);
                PrintLine();
                nLine++;
            }
            if (listStringStreet[2].TrimEnd() != "")
            {
                PrintText("                   " + listStringStreet[2]);
                PrintLine();
                nLine++;
            }
            PrintLine();
            nLine++;

            SetBold(false);
            PrintText("PERUNTUKAN UNDANG-UNDANG YANG BERKAITAN : ");
            PrintLine();
            nLine++;
            SetBold(true);
            string section = actDto.LongDesc;
            var listStringSection = SeparateText(section, 5, 60);
            PrintText(listStringSection[0]);
            PrintLine();
            nLine++;
            if (listStringSection[1].TrimEnd() != "")
            {
                PrintText(listStringSection[1]);
                PrintLine();
                nLine++;
            }
            if (listStringSection[2].TrimEnd() != "")
            {
                PrintText(listStringSection[2]);
                PrintLine();
                nLine++;
            }
            if (listStringSection[3].TrimEnd() != "")
            {
                PrintText(listStringSection[3]);
                PrintLine();
                nLine++;
            }

            SetBold(false);
            PrintLine();
            nLine++;
            PrintText("SEKSYEN              KETERANGAN ");
            PrintLine();
            nLine++;
            PrintText("-------              ---------- ");
            PrintLine();
            nLine++;
            SetBold(true);
            var listStringOffend = SeparateText(offendDto.LongDesc, 5, 45);
            PrintText(offendDto.PrnDesc.PadRight(20));
            PrintText("     " + listStringOffend[0]);
            PrintLine();
            if (listStringOffend[1].TrimEnd() != "")
            {
                PrintText("                               " + listStringOffend[1]);
                PrintLine();
                nLine++;
            }
            if (listStringOffend[2].TrimEnd() != "")
            {
                PrintText("                               " + listStringOffend[2]);
                PrintLine();
                nLine++;
            }
            if (listStringOffend[3].TrimEnd() != "")
            {
                PrintText("                               " + listStringOffend[3]);
                PrintLine();
                nLine++;
            }
            if (listStringOffend[4].TrimEnd() != "")
            {
                PrintText("                               " + listStringOffend[4]);
                PrintLine();
                nLine++;
            }

            PrintLine();
            nLine++;
            var listString1 = SeparateText(compoundDto.Compound3Type.CompDesc, 15, 40);
            SetBold(false);
            PrintText("BUTIR-BUTIR :  ");
            SetBold(true);
            PrintText(listString1[0]);
            PrintLine();
            nLine++;
            SetBold(false);
            PrintText("KESALAHAN         ");
            SetBold(true);
            PrintText(listString1[1]);
            PrintLine();
            nLine++;
            if (listString1[2].TrimEnd() != "")
            {
                PrintText("                              " + listString1[2]);
                PrintLine();
                nLine++;
            }
            if (listString1[3].TrimEnd() != "")
            {
                PrintText("                              " + listString1[3]);
                PrintLine();
                nLine++;
            }
            if (listString1[4].TrimEnd() != "")
            {
                PrintText("                              " + listString1[4]);
                PrintLine();
                nLine++;
            }
            if (listString1[5].TrimEnd() != "")
            {
                PrintText("                              " + listString1[5]);
                PrintLine();
                nLine++;
            }

            SetBold(false);
            PrintLine();
            nLine++;
            PrintText("DIKELUARKAN OLEH : ");
            SetBold(true);
            PrintText(compoundDto.EnforcerId);
            SetBold(false);
            PrintText("                          KOD SAKSI : ");
            SetBold(true);
            PrintText(compoundDto.WitnessId);
            PrintLine();
            PrintLine();
            nLine = nLine + 2;

            SetBold(false);
            PrintLine();
            nLine++;
            PrintText("CARA PENYERAHAN  : ");
            SetBold(true);
            PrintText(delivery.ShortDesc);
            PrintLine();
            PrintLine();
            nLine = nLine + 2;


            if (offendDto.PrintFlag == "Y")
            {
                SetBold(true);
                SetFontSize(40);
                PrintText("KADAR BAYARAN KOMPAUN");
                PrintLine();
                PrintLine();
                nLine = nLine + 2;

                SetFontSize(22);
                SetBold(false);
                PrintText("Bayaran dalam           Bayaran selepas      Bayaran selepas 30");
                PrintLine();
                nLine++;
                PrintText("tempoh 14 hari          tempoh 14 hari        hari sebelum tindakan");
                PrintLine();
                nLine++;
                PrintText("                                                                                undang-undang");
                PrintLine();
                nLine++;

                SetBold(true);
                SetFontSize(30);
                PrintText(formatPrintAmt + "             " + formatPrintAmt2 + "             " + formatPrintAmt3);
                SetBold(false);
                PrintLine();
                nLine++;

            }
            else if (offendDto.PrintFlag == "S")
            {
                SetBold(true);
                SetFontSize(40);
                PrintText("KADAR BAYARAN KOMPAUN");
                PrintLine();
                PrintLine();
                nLine = nLine + 2;

                SetFontSize(22);
                SetBold(false);
                PrintText("Bayaran dalam           Bayaran selepas 30 hari sebelum        ");
                PrintLine();
                nLine++;
                PrintText("tempoh 30 hari          tindakan undang-undang                     ");
                PrintLine();
                nLine++;

                SetBold(true);
                SetFontSize(30);
                PrintText(formatPrintAmt + "                " + formatPrintAmt3);
                SetBold(false);
                PrintLine();
                nLine++;
            }
            else
            {
                SetFontSize(40);
                SetBold(true);
                PrintText("KADAR BAYARAN KOMPAUN : " + formatPrintAmt3);
                PrintLine();
                nLine++;
            }

            SetBold(false);
            PrintLine();
            nLine++;
            SetBold(true);
            SetFontSize(38);
            PrintText("TEMPOH TAMAT   : " + formatTempohDate);
            PrintLine();
            PrintLine();
            nLine = nLine + 2;
            SetBold(false);
            SetFontSize(22);
            PrintText("Tempoh bayaran dikira dari tarikh kesalahan " + nLine.ToString());
            PrintLine();
            nLine++;
            PrintText("dilakukan, termasuk Hari Ahad dan hari");
            PrintLine();
            nLine++;
            PrintText("kelepasan Am.");
            PrintLine();
            nLine++;

            //if (GeneralBll.IsPrintSignature())
            //{
            //    SetPageMode();
            //    SetPrintArea(5, 7);
            //    SetPrintPos(50, 1);
            //    PrintImage(0);
            //    SetExitPageMode();
            //}

            for (i = nLine; i < 58; i++)
                PrintLine();

            SetFontSize(24);
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            nLine = nLine + 6;
            SetBold(true);
            PrintText("No Kompaun : " + strNoKmp.PadRight(35) + "No. Kenderaan   :  ") ;
            PrintLine();
            PrintLine();
            nLine = nLine + 2;
            PrintText("Jumlah Yang Dikenakan  :                        Tarikh Kompaun :  " + formatPrintDate);
            PrintLine();
            PrintLine();
            PrintText("No Cek :                                                        Seksyen Kesalahan : " + offendDto.PrnDesc);
            PrintLine();
            PrintLine();
            PrintText("Kod Hasil : " + offendDto.IncomeCode.PadRight(38) + "Kod Jabatan : L");
            PrintLine();
            PrintLine();
            PrintText("Tandatangan              ");
            PrintLine();
            PrintLine();
            SetFontSize(20);
            PrintLine();
            PrintText("RESIT INI DIAKUI SAH SETELAH DICETAK OLEH MESIN PENERIMAAN MAJLIS.");
            PrintFormFeed();


        }

        private void PrintCompound5(CompoundDto compoundDto, OffendDto offendDto, ActDto actDto, EnforcerDto enforcerDto)
        {

            int nLine = 0, i = 0;
            string saksiName = "";

            string formatPrintAmtWord = SetAmount2Word(compoundDto.Compound5Type.CompAmt);
            string formatPrintAmt = FormatPrintAmount(compoundDto.Compound5Type.CompAmt);
            string formatPrintAmt2 = FormatPrintAmount(compoundDto.Compound5Type.CompAmt2);
            string formatPrintAmt3 = FormatPrintAmount(compoundDto.Compound5Type.CompAmt3);
            string formatPrintDate = GeneralBll.FormatPrintDate(compoundDto.CompDate);
            string formatPrintTime = GeneralBll.FormatPrintTime(compoundDto.CompTime);

            string formatTempohDate = AddDate(compoundDto.CompDate, 13);
            string formatTempohDate3Days = AddDate(compoundDto.CompDate, 2);
            string formatNotaTempohDate = GeneralBll.FormatPrintDate(compoundDto.TempohDate);

            var offendDesc = offendDto.PrnDesc + " " + offendDto.ShortDesc;
            var category = TableFilBll.GetCarCategory(compoundDto.Compound5Type.Category);
            if (category == null)
            {
                category = new CarCategoryDto();
                category.ShortDesc = "Missing Code(" + compoundDto.Compound5Type.Category + ")";
            }

            var delivery = TableFilBll.GetDeliveryByCode(compoundDto.Compound5Type.DeliveryCode);
            if (delivery == null)
            {
                delivery = new DeliveryDto();
                delivery.ShortDesc = "Missing Code(" + compoundDto.Compound5Type.DeliveryCode + ")";
            }

            var message = TableFilBll.GetMessage();
            if (message == null)
            {
                message = new MessageDto();
                message.TelNo  = "";
            }

            var saksi = EnforcerBll.GetEnforcerById(compoundDto.WitnessId);
            if (saksi == null)
                saksiName = " ";
            else
                saksiName = saksi.EnforcerName;

            InitialisePrinter();
            SetLeftMargin();
            SetLineSpacing(10);

            string strKodHasil = offendDto.IncomeCode.PadRight(10);
            string strNoKmp = compoundDto.CompNum;

            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintText("       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
            PrintLine();
            PrintLine();
            PrintLine();
            nLine = 8;
            SetBold(false);
            SetFontSize(15);
            SetBarCodeHight(45);
            PrintBarcode(strKodHasil);
            PrintText("                                                                      ");
            PrintBarcode(strNoKmp);
            SetLineSpacing(15);
            PrintLine();
            PrintLine();
            nLine = nLine + 2;

            SetBold(false);
            SetFontSize(30);
            PrintText("KOD HASIL : ");
            SetBold(true);
            PrintText(strKodHasil);
            PrintText("                     ");
            SetBold(false);
            PrintText("NO NOTIS   : ");
            SetBold(true);
            PrintText(strNoKmp);
            PrintLine();
            PrintLine();
            nLine = nLine + 2;

            PrintText("                       " + actDto.ShortDesc);
            PrintLine();
            nLine++;
            PrintText("                            NOTIS DI BAWAH SEKSYEN");
            PrintLine();
            nLine++;
            PrintText("                   " + offendDto.PrnDesc + " " + actDto.ShortDesc);
            PrintLine();
            nLine++;
            PrintLine();
            nLine++;

            SetLineSpacing(4);
            SetBold(false);
            SetFontSize(26);
            PrintText("TARIKH                       :  ");
            SetBold(true);
            SetBold(true);
            PrintText(formatPrintDate);
            PrintText("        ");
            SetBold(false);
            SetFontSize(26);
            PrintText("WAKTU  : ");
            SetBold(true);
            PrintText(formatPrintTime);
            PrintLine();
            nLine++;

            SetBold(false);
            SetFontSize(26);
            PrintText("Kepada                        :  ");
            SetBold(true);
            PrintText("Pemilik/Pemandu Kenderaan");
            PrintLine();
            nLine++; 

            SetBold(false);
            SetFontSize(26);
            PrintText("NO.KENDERAAN          :  ");
            SetBold(true);
            SetFontSize(30);
            PrintText(compoundDto.Compound5Type.CarNum.PadRight(15));
            PrintLine();
            nLine++;

            SetFontSize(26);
            SetBold(false);
            PrintText("JENIS KENDERAAN    : ");
            SetBold(true);
            PrintText(category.ShortDesc.PadRight(40));
            PrintLine();
            nLine++; 

            SetBold(false);
            PrintText("MODEL KENDERAAN    : ");
            SetBold(true);
            PrintText(compoundDto.Compound5Type.CarTypeDesc);
            PrintLine();
            nLine++;



            SetBold(false);
            PrintText("JALAN                          : ");
            SetBold(true);
            PrintText(compoundDto.StreetDesc);
            PrintLine();
            nLine++;

            string tempatjadi;
            tempatjadi = compoundDto.Tempatjadi;

            var listStringStreet = SeparateText(tempatjadi, 6, 50);
            SetBold(false);
            PrintText("TEMPAT/LOKASI       : ");
            SetBold(true);
            PrintText(listStringStreet[0]);
            PrintLine();
            nLine++;
            if (listStringStreet[1].TrimEnd() != "")
            {
                PrintText("                   " + listStringStreet[1]);
                PrintLine();
                nLine++;
            }
            if (listStringStreet[2].TrimEnd() != "")
            {
                PrintText("                   " + listStringStreet[2]);
                PrintLine();
                nLine++;
            }
            PrintLine();
            nLine++;

            SetFontSize(18);
            SetBold(false);

            PrintText("Adalah tuan telah didapati melakukan kesalahan di bawah Seksyen " + offendDto.PrnDesc + " Akta Pengangkutan Jalan") ;
            PrintLine();
            nLine++;
            PrintText("1987 iaitu menyebabkan atau membenarkan kenderaan motor diberhentikan di mana-mana jalan ") ;
            PrintLine();
            nLine++;
            PrintText("dalam apa-apa kedudukan atau apa-apa keadaan atau apa-apa hal yang mungkin menyebabkan   ") ;
            PrintLine();
            nLine++;
            PrintText("bahaya, galangan atau kesusahan tidak berpatutan kepada pengguna jalan yang lain atau    ") ;
            PrintLine();
            nLine++;
            PrintText("kepada lalulintas.") ;
            PrintLine();
            nLine++;
            PrintLine();
            nLine++;

            PrintText("Maka dengan ini pihak Majlis mengambil tindakan mengapit roda kenderaan motor dan tuan  ") ;
            PrintLine();
            nLine++;
            PrintText("dikehendaki untuk hadir ke Jabatan Penguatkuasaan dan Kesalamatan Majlis Perbandaraan ") ;
            PrintLine();
            nLine++;
            PrintText("Kuantan bagi membuat tuntutan ke  atas kenderaan motor tersebut dalam tempoh empat(4) jam ") ;
            PrintLine();
            nLine++;
            PrintText("dari waktu notis ini. Kegagalan tuan untuk  berbuat demikian akan menyebabkan kenderaan ") ;
            PrintLine();
            nLine++;
            PrintText("motor tersebut dialihkan ke tempat simpanan Majlis dan ditahan tanpa sebarang notis lagi.") ;
            PrintLine();
            nLine++;

            SetBold(false);
            SetFontSize(26);
            PrintLine();
            nLine++;
            PrintText("DIKELUARKAN OLEH : ");
            SetBold(true);
            PrintText(compoundDto.EnforcerId);
            SetBold(false);
            PrintText("                          KOD SAKSI : ");
            SetBold(true);
            PrintText(compoundDto.WitnessId);
            PrintLine();
            nLine++; 

            SetBold(false);
            PrintLine();
            nLine++;
            PrintText("CARA PENYERAHAN  : ");
            SetBold(true);
            PrintText(delivery.ShortDesc);
            PrintLine();
            nLine++;
            PrintLine();
            nLine++;

            SetFontSize(24);
            PrintText("Kadar fi yang dikenakan mengikut jenis kenderaan:") ;
            PrintLine();
            nLine++;
            SetFontSize(20);
            PrintText("                                                                                              Pengapitan   Pengalihan   Penahanan") ;
            PrintLine();
            nLine++;
            PrintText("                                                                                                                                                 (sehari)") ;
            PrintLine();
            nLine++;
            PrintText("Motosikal/trisikal") ;
            PrintText("                                                                RM  20.00    RM  30.00    RM  50.00") ;
            PrintLine();
            nLine++;
            PrintText("Kereta motor");
            PrintText("                                                                              RM  50.00    RM 100.00    RM 100.00");
            PrintLine();
            nLine++;
            PrintText("Bas/Lori berat tanpa muatan tidak melebihi                RM 100.00    RM 300.00    RM 200.00");
            PrintLine();
            nLine++;
            PrintText("3000 kg                                 ");
            PrintLine();
            nLine++;
            PrintText("Lori tangki/trelar/lain-lain");
            PrintText("                                          RM 150.00    RM 400.00    RM 300.00");
            PrintLine();
            nLine++;

            PrintText("kenderaan berat tanpa muatan melebihi 3000 kg");
            PrintLine();
            nLine++;
            PrintLine();
            nLine++;
            //            PrintText("Sila Hubungi 0169540761/0199040160") ;
            PrintText("Sila Hubungi " + message.TelNo);
            PrintLine();
            nLine++;
            PrintText("Untuk tindakan buka apit roda") ;
            PrintLine();
            nLine++;

            for (i = nLine; i < 63; i++)
                PrintLine();

            SetFontSize(24);
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            nLine = nLine + 6;
            SetBold(true);
            PrintText("No Kompaun : " + strNoKmp.PadRight(35) + "No. Kenderaan   :  " + compoundDto.Compound5Type.CarNum);
            PrintLine();
            PrintLine();
            nLine = nLine + 2;
            PrintText("Jumlah Yang Dikenakan  :                        Tarikh Kompaun :  " + formatPrintDate);
            PrintLine();
            PrintLine();
            PrintText("No Cek :                                                        Seksyen Kesalahan : " + offendDto.PrnDesc);
            PrintLine();
            PrintLine();
            PrintText("Kod Hasil : " + offendDto.IncomeCode.PadRight(38) + "Kod Jabatan : L");
            PrintLine();
            PrintLine();
            PrintText("Tandatangan              ");
            PrintLine();
            PrintLine();
            SetFontSize(20);
            PrintLine();
            PrintText("RESIT INI DIAKUI SAH SETELAH DICETAK OLEH MESIN PENERIMAAN MAJLIS.");
            PrintFormFeed();

        }


        //        void CPrintDlg::SetCompoundLayout5()
        //        {
        //            char buf[1001];
        //            char cdate[11], ctime[13];
        //            char amt[10], amt14[10], amt28[10];
        //            char roadtax[11], cartype[41], incomecode[11];
        //            char section[16], tempohDate[11], actDesc[256];
        //            char streetDesc[101];
        //            char sLongDesc[8][60] ;
        //	int i, nCurrPos, nPos, nLine = 0;
        //        offend_t offend;
        //        carcategory_t carcategory;
        //        CEnforcer Enforcer;
        //        CMessage Message;
        //        act_t act;
        //        CStr str;
        //        GetActRecord(&act, Compound->GetActCode());
        //        sprintf(actDesc, "%.255s", act.LongDesc);
        //        str.rtrim(actDesc) ;

        //	delivery_t delivery;

        //        GetDeliveryRecord(&delivery, Compound->GetDeliveryCode());

        //        GetOffendRecord(&offend, Compound->GetActCode(), Compound->GetOffendCode());

        //        Message.GetMessageRecord(0) ;

        //	Enforcer.GetEnforcerRecord(Compound->GetEnforcerID()) ;

        //	GetCarCategoryRecord(&carcategory, Compound->GetCarCategory());

        //        sprintf(streetDesc, "%.100s", Compound->GetStreetDesc());

        //        // Prepare Date and time info
        //        FormatPrintDate(cdate, Compound->GetCompoundDate());
        //        FormatPrintTime(ctime, Compound->GetCompoundTime());

        //        // Prepare Date add 14 days
        //        AddDate(cdate, 14, tempohDate);

        //        // Prepare amount in desire format
        //        FormatPrintAmount(amt, Compound->GetCompoundAmount());
        //        FormatPrintAmount(amt14, Compound->GetCompoundAmount2());
        //        FormatPrintAmount(amt28, Compound->GetCompoundAmount3());

        //        // Prepare Roadtax info
        //        memset(roadtax, '\0', 11);
        //        strncpy(roadtax, Compound->GetRoadTax(), 10);
        //        str.rtrim(roadtax) ;

        //	// Prepare offence's section
        //	str.field(section, offend.ShortDesc, 15) ;
        //	str.rtrim(section) ;

        //	// Prepare Car Type info
        //	sprintf(cartype, "%.17s", Compound->GetCarTypeDesc());
        //        str.rtrim(cartype) ;

        //	if (bMatchHandheld==false)
        //		bPrintSignature = false ;

        //	/************** Start of compound layout *****************/
        //	Printer.Print2Buffer(buf, "\x1B\x45\x5A");
        //	Printer.Print2Buffer(buf, "{PRINT,QSTOP45,STOP2100:") ;

        //	sprintf(incomecode, "%.10s", offend.IncomeCode);
        //        //sprintf(incomecode, "705") ;  // HARD Code for Parking
        //        str.rtrim(incomecode) ;

        //    Printer.Print2Buffer(buf, "@165,10:MF107|XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX|\n") ;
        //	nLine = 215 ;
        //    Printer.Print2Buffer(buf, "@%d,1:BC128,HIGH 10,WIDE 2|%s|\n", nLine, incomecode) ;
        //    Printer.Print2Buffer(buf, "@%d,580:BC128,HIGH 10,WIDE 2|%.10s|\n", nLine, Compound->GetCompoundNumber()) ;

        //    if (Compound->IsPrinted()==true) 
        //       Printer.Print2Buffer(buf, "@%d,350:FC12G|SALINAN|\n", nLine + 5) ;

        //	nLine = nLine + 80 ;
        //    Printer.Print2Buffer(buf, "@%d,1:FC12G|KOD HASIL   :|\n", nLine) ;
        //    Printer.Print2Buffer(buf, "@%d,400:FC12G|NO NOTIS  :|\n", nLine ) ;
        //	Printer.Print2Buffer(buf, "@%d,245:FC12G|%s|\n", nLine, incomecode) ;
        //    Printer.Print2Buffer(buf, "@%d,580:FC12G|%.10s|\n", nLine, Compound->GetCompoundNumber()) ;

        //	nLine = nLine + 50 ;
        //    Printer.Print2Buffer(buf, "@%d,1:MF107|       %s|\n", nLine, actDesc) ;
        //	nLine = nLine + 30 ;
        //	Printer.Print2Buffer(buf, "@%d,1:MF107|          NOTIS DI BAWAH SEKSYEN|\n", nLine ) ;
        //	nLine = nLine + 30 ;
        //	Printer.Print2Buffer(buf, "@%d,1:MF107|       %s %s|\n", nLine,section, actDesc ) ;

        //	nLine = nLine + 40 ;
        //    Printer.Print2Buffer(buf, "@%d,1:MF156|TARIKH         :|\n", nLine) ;
        //    Printer.Print2Buffer(buf, "@%d,220:FC12G|%s|\n", nLine , cdate) ;
        //    Printer.Print2Buffer(buf, "@%d,480:MF156|WAKTU    :|\n", nLine) ;
        //    Printer.Print2Buffer(buf, "@%d,615:FC12G|%s|\n", nLine, ctime) ;

        //	nLine = nLine + 30 ;
        //    Printer.Print2Buffer(buf, "@%d,1:MF156|KEPADA         :|\n", nLine) ;
        //    Printer.Print2Buffer(buf, "@%d,220:FC12G|Pemilik/Pemandu Kenderaan|\n", nLine) ;

        //	nLine = nLine + 30 ;
        //    Printer.Print2Buffer(buf, "@%d,1:MF156|NO.KENDERAAN   :|\n", nLine) ;
        //    Printer.Print2Buffer(buf, "@%d,220:MF107,WIDE 2|%.15s|\n", nLine, Compound->GetCarNumber()) ;

        //	nLine = nLine + 30 ;
        //    Printer.Print2Buffer(buf, "@%d,1:MF156|JENIS KENDERAAN:|\n", nLine) ;
        //    Printer.Print2Buffer(buf, "@%d,220:FC12G|%.40s|\n", nLine, carcategory.ShortDesc) ;

        //	nLine = nLine + 30 ;
        //    Printer.Print2Buffer(buf, "@%d,1:MF156|MODEL KENDERAAN:|\n", nLine) ;
        //    Printer.Print2Buffer(buf, "@%d,220:FC12G|%.17s|\n", nLine, Compound->GetCarTypeDesc()) ;

        //	nLine = nLine + 30 ;
        //    Printer.Print2Buffer(buf, "@%d,1:MF156|JALAN          :|\n", nLine) ;
        //    Printer.Print2Buffer(buf, "@%d,220:FC12G|%.40s|\n", nLine, streetDesc) ;

        //	nLine = nLine + 30 ;
        //    Printer.Print2Buffer(buf, "@%d,1:MF156|TEMPAT/LOKASI  :|\n", nLine) ;
        //	for(i=0; i<8; i++)
        //		sLongDesc[i][0] = '\0' ;

        //	str.field(buf, Compound->GetTempatJadi(), 150) ;
        //	str.rtrim(buf) ;
        //	nCurrPos = 0 ;

        //	for(i=0; i<8; i++)
        //	{
        //		nPos = FormatOffDesc(sLongDesc[i], &buf[nCurrPos], 30);
        //		if (nPos<=0)
        //			break ;
        //		nCurrPos = nCurrPos + nPos + 1 ;
        //	}
        //    Printer.Print2Buffer(buf, "@%d,220:FC12G|%s|\n", nLine, sLongDesc[0]) ;
        //	if (sLongDesc[1][0] != '\0')
        //	{
        //		nLine = nLine + 25 ;
        //		Printer.Print2Buffer(buf, "@%d,220:FC12G|%s|\n", nLine, sLongDesc[1]) ;
        //	}
        //	if (sLongDesc[2][0] != '\0')
        //	{
        //   		nLine = nLine + 25 ;
        //		Printer.Print2Buffer(buf, "@%d,220:FC12G|%s|\n", nLine, sLongDesc[2]) ;
        //	}

        //   	nLine = nLine + 40 ;
        //	Printer.Print2Buffer(buf, "@%d,5:MF226|Adalah tuan telah didapati melakukan kesalahan di bawah Seksyen %s Akta Pengangkutan Jalan|", nLine, section) ;
        //   	nLine = nLine + 25 ;
        //	Printer.Print2Buffer(buf, "@%d,5:MF226|1987 iaitu menyebabkan atau membenarkan kenderaan motor diberhentikan di mana-mana jalan |", nLine) ;
        //   	nLine = nLine + 25 ;
        //	Printer.Print2Buffer(buf, "@%d,5:MF226|dalam apa-apa kedudukan atau apa-apa keadaan atau apa-apa hal yang mungkin menyebabkan   |", nLine) ;
        //   	nLine = nLine + 25 ;
        //	Printer.Print2Buffer(buf, "@%d,5:MF226|bahaya, galangan atau kesusahan tidak berpatutan kepada pengguna jalan yang lain atau |", nLine) ;
        //   	nLine = nLine + 25 ;
        //	Printer.Print2Buffer(buf, "@%d,5:MF226|kepada lalulintas.|", nLine) ;

        //	nLine = nLine + 35 ;
        //	Printer.Print2Buffer(buf, "@%d,5:MF226|Maka dengan ini pihak Majlis mengambil tindakan mengapit roda kenderaan motor dan tuan |", nLine) ;
        //   	nLine = nLine + 25 ;
        //	Printer.Print2Buffer(buf, "@%d,5:MF226|dikehendaki untuk hadir ke Jabatan Penguatkuasaan dan Kesalamatan Majlis Perbandaraan Kuantan|", nLine) ;
        //   	nLine = nLine + 25 ;
        //	Printer.Print2Buffer(buf, "@%d,5:MF226|bagi membuat tuntutan ke  atas kenderaan motor tersebut dalam tempoh empat(4) jam dari waktu|", nLine) ;
        //   	nLine = nLine + 25 ;
        //	Printer.Print2Buffer(buf, "@%d,5:MF226|notis ini. Kegagalan tuan untuk  berbuat demikian akan menyebabkan kenderaan motor tersebut|", nLine) ;
        //   	nLine = nLine + 25 ;
        //	Printer.Print2Buffer(buf, "@%d,5:MF226|dialihkan ke tempat simpanan Majlis dan ditahan tanpa sebarang notis lagi|", nLine) ;

        //   	nLine = nLine + 35 ;
        //    Printer.Print2Buffer(buf, "@%d,1:MF156|DIKELUARKAN OLEH : |\n",nLine) ;
        //    Printer.Print2Buffer(buf, "@%d,480:MF156|KOD SAKSI : |\n",nLine) ;
        //    Printer.Print2Buffer(buf, "@%d,240:FC12G|%.4s|\n",nLine ,Enforcer.GetEnforcerID()) ;
        //    Printer.Print2Buffer(buf, "@%d,630:FC12G|%.4s|\n",nLine,Compound->GetWitnessID()) ;

        //   	nLine = nLine + 35 ;
        //    Printer.Print2Buffer(buf, "@%d,1:MF156|CARA PENYERAHAN  : |\n",nLine) ;
        //   	Printer.Print2Buffer(buf, "@%d,240:FC12G|%.17s|",nLine, delivery.ShortDesc) ;	

        //   	nLine = nLine + 40 ;
        //	Printer.Print2Buffer(buf, "@%d,1:FC12G|Kadar fi yang dikenakan mengikut jenis kenderaan:|\n", nLine) ;
        //   	nLine = nLine + 25 ;
        //	Printer.Print2Buffer(buf, "@%d,410:MF156|Pengapitan|\n", nLine) ;
        //	Printer.Print2Buffer(buf, "@%d,550:MF156|Pengalihan|\n", nLine) ;
        //	Printer.Print2Buffer(buf, "@%d,690:MF156|Penahanan|\n", nLine) ;

        //   	nLine = nLine + 25 ;
        //	Printer.Print2Buffer(buf, "@%d,680:MF156|(sehari)|\n", nLine) ;
        //   	nLine = nLine + 25 ;
        //	Printer.Print2Buffer(buf, "@%d,1:MF226|Motosikal/trisikal|\n",nLine) ;
        //	Printer.Print2Buffer(buf, "@%d,410:MF156|RM  20.00|\n",nLine) ;
        //	Printer.Print2Buffer(buf, "@%d,550:MF156|RM  30.00|\n",nLine) ;
        //	Printer.Print2Buffer(buf, "@%d,690:MF156|RM  50.00|\n",nLine) ;

        //	nLine = nLine + 25 ;
        //	Printer.Print2Buffer(buf, "@%d,1:MF226|Kereta motor|\n",nLine) ;
        //	Printer.Print2Buffer(buf, "@%d,410:MF156|RM  50.00|\n",nLine) ;
        //	Printer.Print2Buffer(buf, "@%d,550:MF156|RM 100.00|\n",nLine) ;
        //	Printer.Print2Buffer(buf, "@%d,690:MF156|RM 100.00|\n",nLine) ;

        //	nLine = nLine + 25 ;
        //	Printer.Print2Buffer(buf, "@%d,1:MF226|Bas/Lori berat tanpa muatan tidak melebihi|\n",nLine) ;
        //	Printer.Print2Buffer(buf, "@%d,410:MF156|RM 100.00|\n",nLine) ;
        //	Printer.Print2Buffer(buf, "@%d,550:MF156|RM 300.00|\n",nLine) ;
        //	Printer.Print2Buffer(buf, "@%d,690:MF156|RM 200.00|\n",nLine) ;
        //	nLine = nLine + 25 ;
        //	Printer.Print2Buffer(buf, "@%d,1:MF226|3000 kg|\n",nLine) ;

        //	nLine = nLine + 25 ;
        //	Printer.Print2Buffer(buf, "@%d,1:MF226|Lori tangki/trelar/lain-lain|\n",nLine) ;
        //	Printer.Print2Buffer(buf, "@%d,410:MF156|RM 150.00|\n",nLine) ;
        //	Printer.Print2Buffer(buf, "@%d,550:MF156|RM 400.00|\n",nLine) ;
        //	Printer.Print2Buffer(buf, "@%d,690:MF156|RM 300.00|\n",nLine) ;
        //	nLine = nLine + 25 ;
        //	Printer.Print2Buffer(buf, "@%d,1:MF226|kenderaan berat tanpa muatan melebihi 3000 kg|\n",nLine) ;

        //	nLine = nLine + 50 ;
        //	Printer.Print2Buffer(buf, "@%d,1:MF226|Sila Hubungi %.50s|\n",nLine, Message.GetTelNo()) ;
        //	nLine = nLine + 25 ;
        //	Printer.Print2Buffer(buf, "@%d,1:MF226|Untuk tindakan buka apit roda|\n",nLine) ;

        //	nLine = 1662 ;
        //	Printer.Print2Buffer(buf, "@%d,135:MF107|:%.10s|\n", nLine, Compound->GetCompoundNumber()) ;
        //    Printer.Print2Buffer(buf, "@%d,625:MF107|:%.15s|\n", nLine, Compound->GetCarNumber()) ;

        //   	nLine = nLine + 63 ;
        //    Printer.Print2Buffer(buf, "@%d,643:FC12G|: %s|\n", nLine, cdate) ;

        //   	nLine = nLine + 55 ;
        //    Printer.Print2Buffer(buf, "@%d,680:FC12G|: %.10s|\n", nLine, offend.PrnDesc) ;

        //   	nLine = nLine + 55 ;
        //	Printer.Print2Buffer(buf, "@%d,105:FC12G|: %s|\n", nLine,incomecode) ;
        //	Printer.Print2Buffer(buf, "@%d,680:MF107|: L|\n", nLine) ;

        //    Printer.Print2Buffer(buf, "}") ;
        //}





        private void PrintCompoundOld3(CompoundDto compoundDto, OffendDto offendDto, ActDto actDto, EnforcerDto enforcerDto)
        {
            string saksiName = "";

            string formatPrintAmtWord = SetAmount2Word(compoundDto.Compound3Type.CompAmt);
            string formatPrintAmt = FormatPrintAmount(compoundDto.Compound3Type.CompAmt);
            string formatPrintAmt2 = FormatPrintAmount(compoundDto.Compound3Type.CompAmt2);
            string formatPrintAmt3 = FormatPrintAmount(compoundDto.Compound3Type.CompAmt3);
            string formatPrintDate = GeneralBll.FormatPrintDate(compoundDto.CompDate);
            string formatPrintTime = GeneralBll.FormatPrintTime(compoundDto.CompTime);

            string formatTempohDate = AddDate(compoundDto.CompDate, 14);
            string formatNotaTempohDate = GeneralBll.FormatPrintDate(compoundDto.TempohDate);

            var offendDesc = offendDto.PrnDesc + " " + offendDto.ShortDesc;

            var delivery = TableFilBll.GetDeliveryByCode(compoundDto.Compound3Type.DeliveryCode);
            if (delivery == null)
                delivery = new DeliveryDto();

            var saksi = EnforcerBll.GetEnforcerById(compoundDto.WitnessId);
            if (saksi == null)
                saksiName = " ";
            else
                saksiName = saksi.EnforcerName;

            string strKodHasil = offendDto.IncomeCode.PadRight(8);
            string strNoKmp = compoundDto.CompNum;

            /************** Start of compound layout *****************/
            PrintText("\x1B\x45\x5A");
            PrintText("{PRINT,QSTOP50,STOP2500:");

            PrintText("@215,1:BC128,HIGH 10,WIDE 3|" + offendDto.IncomeCode + "|\n");
            PrintText("@215,400:BC128,HIGH 10,WIDE 3|" + compoundDto.CompNum + "|\n");

            if (Convert.ToInt32(compoundDto.PrintCnt) > 0)
                PrintText("@220,250:FC12G|SALINAN|\n");

            PrintText("@290,1:FC12G|KOD HASIL     :|\n");
            PrintText("@290,400:FC12G|NO KOMPAUN:|\n");
            PrintText("@290,245:FC12G|" + offendDto.IncomeCode + "|\n");
            PrintText("@290,580:FC12G|" + compoundDto.CompNum + " |\n");

            PrintText("@340,1:MF156|TARIKH         :|\n");
            PrintText("@340,220:FC12G|" + formatPrintDate + "|\n");
            PrintText("@340,480:MF156|WAKTU    :|\n");
            PrintText("@340,615:FC12G|" + formatPrintTime + " |\n");

            string strCompany = "";
            string strOffender = "";
            if (compoundDto.Compound3Type.CompanyName.TrimEnd() != "")
                strCompany = compoundDto.Compound3Type.CompanyName;

            if (compoundDto.Compound3Type.Company.TrimEnd() != "")
                strCompany = strCompany + "(" + compoundDto.Compound3Type.Company + ")"; 

            if (compoundDto.Compound3Type.Rujukan.TrimEnd() != "")
                strCompany = strCompany + "(" + compoundDto.Compound3Type.Rujukan + ")";

            if (compoundDto.Compound3Type.OffenderName.TrimEnd() != "")
                strOffender = compoundDto.Compound3Type.OffenderName;

            PrintText("@380,120:FC12G|"+ strCompany + "|");
            PrintText("@405,120:FC12G|"+ strOffender + "|");

            if (compoundDto.Compound3Type.OffenderIc.TrimEnd() != "")
                PrintText("@430,120:FC12G|(NO K/P: "+ compoundDto.Compound3Type.OffenderIc + ")|");

            PrintText("@455,120:FC12G|"+ compoundDto.Compound3Type.Address1 + "|");
            PrintText("@480,120:FC12G|" + compoundDto.Compound3Type.Address2 + "|");
            PrintText("@515,120:FC12G|" + compoundDto.Compound3Type.Address3 + "|");

            var listStringTempatjadi = SeparateText(compoundDto.Tempatjadi, 6, 45);
            PrintText("@540,1:MF156|TEMPAT/LOKASI :|\n");
            PrintText("@540,265:FC12G|" + listStringTempatjadi[0] + "|\n");
            PrintText("@565,265:FC12G|" + listStringTempatjadi[1] + "|\n");
            PrintText("@590,265:FC12G|" + listStringTempatjadi[2] + "|\n");

            PrintText("@620,5:FC12G|PERUNTUKAN UNDANG-UNDANG YANG BERKAITAN:|");

            var listStringAct = SeparateText(actDto.LongDesc, 4, 50);
            PrintText("@645,5:FC12G|" + listStringAct[0] + "|\n");
            PrintText("@670,5:FC12G|" + listStringAct[1] + "|\n");
            PrintText("@695,5:FC12G|" + listStringAct[2] + "|\n");

            PrintText("@715,1:FC12G|SEKSYEN|\n");
            PrintText("@715,200:FC12G|KETERANGAN|\n");
            PrintText("@740,1:FC12G|-------|\n");
            PrintText("@740,200:FC12G|----------|\n");
            PrintText("@765,1:FC12G|" + offendDto.PrnDesc + "|\n");

            var listStringOffend = SeparateText(offendDto.LongDesc, 8, 50);
            PrintText("@765,200:FC12G|" + listStringOffend[0] + "|\n");
            PrintText("@790,200:FC12G|" + listStringOffend[0] + "|\n");
            PrintText("@815,200:FC12G|" + listStringOffend[0] + "|\n");
            PrintText("@840,200:FC12G|" + listStringOffend[0] + "|\n");

            var listStringButir = SeparateText(compoundDto.Compound1Type.CompDesc, 15, 40);
            PrintText("@860,1:MF185|BUTIR-BUTIR :|\n");
            PrintText("@885,1:MF185|KESALAHAN|\n");
            PrintText("@860,265:FC12G|" + listStringButir[0] + "|\n");
            PrintText("@885,265:FC12G|" + listStringButir[1] + "|\n");
            PrintText("@910,265:FC12G|" + listStringButir[2] + "|\n");
            PrintText("@935,265:FC12G|" + listStringButir[3] + "|\n");
       
            PrintText("@965,1:MF156|DIKELUARKAN OLEH : |\n");
            PrintText("@965,480:MF156|KOD SAKSI : |\n");
            PrintText("@965,265:FC12G|" + enforcerDto.EnforcerId + "|\n");
            PrintText("@965,630:FC12G|" + compoundDto.WitnessId + "|\n");

            PrintText("@995,1:MF156|CARA PENYERAHAN  : |\n");
            PrintText("@995,265:FC12G|" + delivery.ShortDesc + "|");

            if (compoundDto.Kadar == "N")
            {
                PrintText("@1040,1:MF107|KADAR BAYARAN KOMPAUN|\n");
                PrintText("@1065,1:MF156|Bayaran dalam   Bayaran selepas  Bayaran selepas 30|\n");
                PrintText("@1090,1:MF156|tempoh 14 hari  tempoh 14 hari   hari sebelum tindakan|\n");
                PrintText("@1115,1:MF156|                                 undang-undang        |\n");

                PrintText("@1145,1:MF107|" + formatPrintAmt + "|\n");
                PrintText("@1145,210:MF107|" + formatPrintAmt2 + "|\n");
                PrintText("@1145,427:MF107|" + formatPrintAmt3 + "|\n");
            }
            else
            {
                PrintText("@1070,1:MF107|KADAR BAYARAN KOMPAUN : " + formatPrintAmt3 + "|\n");
            }
            PrintText("@1185,1:MF107|TEMPOH TAMAT : " + formatTempohDate + "|\n");
            if (GeneralBll.IsPrintSignature())
                PrintText("@1100,500:S" + compoundDto.EnforcerId + ",HMULT2,VMULT2|");

            PrintText("@1215,1:MF156|Tempoh bayaran dikira dari tarikh kesalahan|\n");
            PrintText("@1240,1:MF156|dilakukan, termasuk Hari Ahad dan hari|\n");
            PrintText("@1265,1:MF156|kelepasan Am.|\n");

            PrintText("@1662,135:MF107|" + compoundDto.CompNum + "|\n");

            PrintText("@1725,643:FC12G|: " + formatPrintDate + "|\n");
            PrintText("@1780,680:FC12G|: " + offendDto.PrnDesc + "|\n");

            PrintText("@1835,105:FC12G|: " + offendDto.IncomeCode + "|\n");
            PrintText("@1835,680:MF107|: L|\n");

            PrintText("}");

        }

        private void PrintCompound2(CompoundDto compoundDto, OffendDto offendDto, ActDto actDto, EnforcerDto enforcerDto)
        {

            int nLine = 0, i = 0;
            string saksiName = "";

            string formatPrintAmtWord = SetAmount2Word(compoundDto.Compound2Type.CompAmt);
            string formatPrintAmt = FormatPrintAmount(compoundDto.Compound2Type.CompAmt);
            string formatPrintAmt2 = FormatPrintAmount(compoundDto.Compound2Type.CompAmt2);
            string formatPrintAmt3 = FormatPrintAmount(compoundDto.Compound2Type.CompAmt3);
            string formatPrintDate = GeneralBll.FormatPrintDate(compoundDto.CompDate);
            string formatPrintTime = GeneralBll.FormatPrintTime(compoundDto.CompTime);

            string formatTempohDate;
            if (offendDto.PrintFlag == "S")
                formatTempohDate = AddDate(compoundDto.CompDate, 30);
            else
                formatTempohDate = AddDate(compoundDto.CompDate, 14);
            string formatTempohDate3Days = AddDate(compoundDto.CompDate, 2);
            string formatNotaTempohDate = GeneralBll.FormatPrintDate(compoundDto.TempohDate);

            var offendDesc = offendDto.PrnDesc + " " + offendDto.ShortDesc;
            var category = TableFilBll.GetCarCategory(compoundDto.Compound2Type.Category);
            if (category == null)
            {
                category = new CarCategoryDto();
                category.ShortDesc = "Missing Code(" + compoundDto.Compound2Type.Category + ")";
            }

            var delivery = TableFilBll.GetDeliveryByCode(compoundDto.Compound2Type.DeliveryCode);
            if (delivery == null)
            {
                delivery = new DeliveryDto();
                delivery.ShortDesc = "Missing Code(" + compoundDto.Compound2Type.DeliveryCode + ")";
            }

            var saksi = EnforcerBll.GetEnforcerById(compoundDto.WitnessId);
            if (saksi == null)
                saksiName = " ";
            else
                saksiName = saksi.EnforcerName;

            InitialisePrinter();
            SetLeftMargin();
            SetLineSpacing(10);

            string strKodHasil = offendDto.IncomeCode.PadRight(10);
            string strNoKmp = compoundDto.CompNum;

            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            nLine = 8;
            SetBold(false);
            SetFontSize(15);
            SetBarCodeHight(45);
            PrintBarcode(strKodHasil);
            PrintText("                                                                      ");
            PrintBarcode(strNoKmp);
            SetLineSpacing(15);
            PrintLine();
            PrintLine();
            nLine = nLine + 2;

            SetBold(false);
            SetFontSize(30);
            PrintText("KOD HASIL : ");
            SetBold(true);
            PrintText(strKodHasil);
            PrintText("                     ");
            SetBold(false);
            PrintText("NO KOMPAUN : ");
            SetBold(true);
            PrintText(strNoKmp);
            PrintLine();
            PrintLine();
            nLine = nLine + 2;

            SetLineSpacing(5);
            SetBold(false);
            SetFontSize(26);
            PrintText("TARIKH                :  ");
            SetBold(true);
            SetBold(true);
            PrintText(formatPrintDate);
            PrintText("        ");
            SetBold(false);
            SetFontSize(26);
            PrintText("WAKTU  : ");
            SetBold(true);
            PrintText(formatPrintTime);
            PrintLine();
            PrintLine();
            nLine = nLine + 2;

            SetBold(false);
            SetFontSize(26);
            PrintText("Kepada                :  ");
            SetBold(true);
            PrintText("Pemilik/Pemandu Kenderaan");
            PrintLine();
            PrintLine();
            nLine = nLine + 2;

            SetBold(false);
            SetFontSize(26);
            PrintText("NO.KENDERAAN          :  ");
            SetBold(true);
            SetFontSize(30);
            PrintText(compoundDto.Compound2Type.CarNum.PadRight(15));
            PrintLine();
            PrintLine();
            nLine = nLine + 2;

            SetFontSize(26);
            SetBold(false);
            PrintText("JENIS KENDERAAN    : ");
            SetBold(true);
            PrintText(category.ShortDesc.PadRight(40));
            PrintLine();
            PrintLine();
            nLine = nLine + 2;

            SetBold(false);
            PrintText("MODEL KENDERAAN    : ");
            SetBold(true);
            PrintText(compoundDto.Compound2Type.CarTypeDesc);
            PrintLine();
            PrintLine();
            nLine = nLine + 2;



            SetBold(false);
            PrintText("JALAN                          : ");
            SetBold(true);
            PrintText(compoundDto.StreetDesc);
            PrintLine();
            PrintLine();
            nLine = nLine + 2;

            string tempatjadi;
            tempatjadi = compoundDto.Tempatjadi;

            var listStringStreet = SeparateText(tempatjadi, 6, 50);
            SetBold(false);
            PrintText("TEMPAT/LOKASI       : ");
            SetBold(true);
            PrintText(listStringStreet[0]);
            PrintLine();
            nLine++;
            if (listStringStreet[1].TrimEnd() != "")
            {
                PrintText("                   " + listStringStreet[1]);
                PrintLine();
                nLine++;
            }
            if (listStringStreet[2].TrimEnd() != "")
            {
                PrintText("                   " + listStringStreet[2]);
                PrintLine();
                nLine++;
            }
            PrintLine();
            nLine++;

            SetBold(false);
            PrintText("PERUNTUKAN UNDANG-UNDANG YANG BERKAITAN : ");
            PrintLine();
            nLine++;
            SetBold(true);
            string section = actDto.LongDesc;
            var listStringSection = SeparateText(section, 5, 60);
            PrintText(listStringSection[0]);
            PrintLine();
            nLine++;
            if (listStringSection[1].TrimEnd() != "")
            {
                PrintText(listStringSection[1]);
                PrintLine();
                nLine++;
            }
            if (listStringSection[2].TrimEnd() != "")
            {
                PrintText(listStringSection[2]);
                PrintLine();
                nLine++;
            }
            if (listStringSection[3].TrimEnd() != "")
            {
                PrintText(listStringSection[3]);
                PrintLine();
                nLine++;
            }

            SetBold(false);
            PrintLine();
            nLine++;
            PrintText("SEKSYEN              KETERANGAN ");
            PrintLine();
            nLine++;
            PrintText("-------              ---------- ");
            PrintLine();
            nLine++;
            SetBold(true);
            var listStringOffend = SeparateText(offendDto.LongDesc, 5, 45);
            PrintText(offendDto.PrnDesc.PadRight(20));
            PrintText("     " + listStringOffend[0]);
            PrintLine();
            if (listStringOffend[1].TrimEnd() != "")
            {
                PrintText("                               " + listStringOffend[1]);
                PrintLine();
                nLine++;
            }
            if (listStringOffend[2].TrimEnd() != "")
            {
                PrintText("                               " + listStringOffend[2]);
                PrintLine();
                nLine++;
            }
            if (listStringOffend[3].TrimEnd() != "")
            {
                PrintText("                               " + listStringOffend[3]);
                PrintLine();
                nLine++;
            }
            if (listStringOffend[4].TrimEnd() != "")
            {
                PrintText("                               " + listStringOffend[4]);
                PrintLine();
                nLine++;
            }

            PrintLine();
            nLine++;
            var listString1 = SeparateText(compoundDto.Compound2Type.CompDesc, 15, 40);
            SetBold(false);
            PrintText("BUTIR-BUTIR :  ");
            SetBold(true);
            PrintText(listString1[0]);
            PrintLine();
            nLine++;
            SetBold(false);
            PrintText("KESALAHAN         ");
            SetBold(true);
            PrintText(listString1[1]);
            PrintLine();
            nLine++;
            if (listString1[2].TrimEnd() != "")
            {
                PrintText("                              " + listString1[2]);
                PrintLine();
                nLine++;
            }
            if (listString1[3].TrimEnd() != "")
            {
                PrintText("                              " + listString1[3]);
                PrintLine();
                nLine++;
            }
            if (listString1[4].TrimEnd() != "")
            {
                PrintText("                              " + listString1[4]);
                PrintLine();
                nLine++;
            }
            if (listString1[5].TrimEnd() != "")
            {
                PrintText("                              " + listString1[5]);
                PrintLine();
                nLine++;
            }

            SetBold(false);
            PrintLine();
            nLine++;
            PrintText("DIKELUARKAN OLEH : ");
            SetBold(true);
            PrintText(compoundDto.EnforcerId);
            SetBold(false);
            PrintText("                          KOD SAKSI : ");
            SetBold(true);
            PrintText(compoundDto.WitnessId);
            PrintLine();
            PrintLine();
            nLine = nLine + 2;

            SetBold(false);
            PrintLine();
            nLine++;
            PrintText("CARA PENYERAHAN  : ");
            SetBold(true);
            PrintText(delivery.ShortDesc);
            PrintLine();
            PrintLine();
            nLine = nLine + 2;

            if (offendDto.PrintFlag == "Y")
            {
                SetBold(true);
                SetFontSize(40);
                PrintText("KADAR BAYARAN KOMPAUN");
                PrintLine();
                PrintLine();
                nLine = nLine + 2;

                SetFontSize(22);
                SetBold(false);
                PrintText("Bayaran dalam           Bayaran selepas      Bayaran selepas 30");
                PrintLine();
                nLine++;
                PrintText("tempoh 14 hari          tempoh 14 hari        hari sebelum tindakan");
                PrintLine();
                nLine++;
                PrintText("                                                                                undang-undang");
                PrintLine();
                nLine++;

                SetBold(true);
                SetFontSize(30);
                PrintText(formatPrintAmt + "             " + formatPrintAmt2 + "             " + formatPrintAmt3);
                SetBold(false);
                PrintLine();
                nLine++;

            }
            else if (offendDto.PrintFlag == "S")
            {
                SetBold(true);
                SetFontSize(40);
                PrintText("KADAR BAYARAN KOMPAUN");
                PrintLine();
                PrintLine();
                nLine = nLine + 2;

                SetFontSize(22);
                SetBold(false);
                PrintText("Bayaran dalam           Bayaran selepas 30 hari sebelum        ");
                PrintLine();
                nLine++;
                PrintText("tempoh 30 hari          tindakan undang-undang                     ");
                PrintLine();
                nLine++;

                SetBold(true);
                SetFontSize(30);
                PrintText(formatPrintAmt + "                " + formatPrintAmt3);
                SetBold(false);
                PrintLine();
                nLine++;
            }
            else
            {
                SetFontSize(22);
                SetBold(false);
                PrintText("Sila Rujuk Bahagian Perundangan dan Pendakwaan");
                PrintLine();
                nLine++;
                PrintText("Majlis Perbandaraan Kuantan, Jalan Tanah Putih");
                PrintLine();
                nLine++;
                PrintText("25150, Kuantan(No Telefon:09-5121555)");
                PrintLine();
                nLine++;
                PrintText("dalam tempoh 14 hari");
                PrintLine();
                nLine++;
            }


            SetBold(false);
            PrintLine();
            nLine++;
            SetBold(true);
            SetFontSize(38);
            PrintText("TEMPOH TAMAT   : " + formatTempohDate);
            PrintLine();
            PrintLine();
            nLine = nLine + 2;

            //if (GeneralBll.IsPrintSignature())
            //{
            //    SetPageMode();
            //    SetPrintArea(5, 7);
            //    SetPrintPos(50, 1);
            //    PrintImage(0);
            //    SetExitPageMode();
            //}

            var passbulan = PassBulanBll.GetPassBulanByCarNum(compoundDto.Compound2Type.CarNum);
            if (passbulan != null)
            {
                string cPassType = passbulan.PassType;
                if (cPassType == "S" || cPassType == "V" || cPassType == "M" || cPassType == "L" || cPassType == "K" || cPassType == "X" || cPassType == "Y")
                {
                    SetFontSize(35);
                    SetBold(true);
                    PrintText("     (PENGECUALIAN)");
                    SetBold(false);
                    PrintLine();
                    nLine++;
                }
            }

            for (i = nLine; i < 58; i++)
                PrintLine();

            SetFontSize(24);
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            nLine = nLine + 6;
            SetBold(true);
            PrintText("No Kompaun : " + strNoKmp.PadRight(35) + "No. Kenderaan   :  " + compoundDto.Compound2Type.CarNum);
            PrintLine();
            PrintLine();
            nLine = nLine + 2;
            PrintText("Jumlah Yang Dikenakan  :                        Tarikh Kompaun :  " + formatPrintDate);
            PrintLine();
            PrintLine();
            PrintText("No Cek :                                                        Seksyen Kesalahan : " + offendDto.PrnDesc);
            PrintLine();
            PrintLine();
            PrintText("Kod Hasil : " + offendDto.IncomeCode.PadRight(38) + "Kod Jabatan : L");
            PrintLine();
            PrintLine();
            PrintText("Tandatangan              ");
            PrintLine();
            PrintLine();
            SetFontSize(20);
            PrintLine();
            PrintText("RESIT INI DIAKUI SAH SETELAH DICETAK OLEH MESIN PENERIMAAN MAJLIS.");
            PrintFormFeed();

        }

        private void PrintCompoundOld2(CompoundDto compoundDto, OffendDto offendDto, ActDto actDto, EnforcerDto enforcerDto)
        {

            string saksiName = "";

            string formatPrintAmtWord = SetAmount2Word(compoundDto.Compound2Type.CompAmt);
            string formatPrintAmt = FormatPrintAmount(compoundDto.Compound2Type.CompAmt);
            string formatPrintAmt2 = FormatPrintAmount(compoundDto.Compound2Type.CompAmt2);
            string formatPrintAmt3 = FormatPrintAmount(compoundDto.Compound2Type.CompAmt3);
            string formatPrintDate = GeneralBll.FormatPrintDate(compoundDto.CompDate);
            string formatPrintTime = GeneralBll.FormatPrintTime(compoundDto.CompTime);

            string formatTempohDate = AddDate(compoundDto.CompDate, 14);
            string formatNotaTempohDate = GeneralBll.FormatPrintDate(compoundDto.TempohDate);

            var offendDesc = offendDto.PrnDesc + " " + offendDto.ShortDesc;
            var category = TableFilBll.GetCarCategory(compoundDto.Compound2Type.Category);
            if (category == null)
                category = new CarCategoryDto();

            var delivery = TableFilBll.GetDeliveryByCode(compoundDto.Compound2Type.DeliveryCode);
            if (delivery == null)
                delivery = new DeliveryDto();

            var saksi = EnforcerBll.GetEnforcerById(compoundDto.WitnessId);
            if (saksi == null)
                saksiName = " ";
            else
                saksiName = saksi.EnforcerName;

            string strKodHasil = offendDto.IncomeCode.PadRight(8);
            string strNoKmp = compoundDto.CompNum;

            /************** Start of compound layout *****************/
            PrintText("\x1B\x45\x5A");
            PrintText("{PRINT,QSTOP50,STOP2500:");

            PrintText("@215,1:BC128,HIGH 10,WIDE 3|" + offendDto.IncomeCode + "|\n");
            PrintText("@215,400:BC128,HIGH 10,WIDE 3|" + compoundDto.CompNum + "|\n");

            if (Convert.ToInt32(compoundDto.PrintCnt) > 0)
                PrintText("@220,250:FC12G|SALINAN|\n");

            PrintText("@290,1:FC12G|KOD HASIL     :|\n");
            PrintText("@290,400:FC12G|NO KOMPAUN:|\n");
            PrintText("@290,245:FC12G|" + offendDto.IncomeCode + "|\n");
            PrintText("@290,580:FC12G|" + compoundDto.CompNum + " |\n");

            PrintText("@340,1:MF156|TARIKH         :|\n");
            PrintText("@340,220:FC12G|" + formatPrintDate + "|\n");
            PrintText("@340,480:MF156|WAKTU    :|\n");
            PrintText("@340,615:FC12G|" + formatPrintTime + " |\n");

            PrintText("@380,1:MF156|Kepada         :|\n");
            PrintText("@380,220:FC12G|Pemilik/Pemandu Kenderaan|\n");

            PrintText("@420,1:MF156|NO.KENDERAAN   :|\n");
            PrintText("@420,220:MF107,WIDE 2||" + compoundDto.Compound2Type.CarNum + "|\n");

            PrintText("@460,1:MF156|JENIS KENDERAAN:|\n");
            PrintText("@460,220:FC12G|" + category.ShortDesc + " |\n");

            PrintText("@505,1:MF156|MODEL KENDERAAN:|\n");
            PrintText("@505,220:FC12G|" + compoundDto.Compound2Type.CarTypeDesc + " |\n");

            var listStringStreet = SeparateText(compoundDto.StreetDesc, 6, 45);
            PrintText("@545,1:MF156|JALAN          :|\n");
            PrintText("@545,220:FC12G|" + listStringStreet[0] + " |\n");

            var listStringTempatjadi = SeparateText(compoundDto.Tempatjadi, 6, 45);
            PrintText("@585,1:MF156|TEMPAT/LOKASI :|\n");
            PrintText("@585,220:FC12G|" + listStringTempatjadi[0] + "|\n");
            PrintText("@605,220:FC12G|" + listStringTempatjadi[1] + "|\n");
            PrintText("@625,220:FC12G|" + listStringTempatjadi[2] + "|\n");

            PrintText("@640,5:FC12G|PERUNTUKAN UNDANG-UNDANG YANG BERKAITAN:|");

            var listStringAct = SeparateText(actDto.LongDesc, 4, 50);
            PrintText("@665,5:FC12G|" + listStringAct[0] + "|\n");
            PrintText("@690,5:FC12G|" + listStringAct[1] + "|\n");
            PrintText("@715,5:FC12G|" + listStringAct[2] + "|\n");

            PrintText("@740,1:FC12G|SEKSYEN|\n");
            PrintText("@740,220:FC12G|KETERANGAN|\n");
            PrintText("@760,1:FC12G|-------|\n");
            PrintText("@760,220:FC12G|----------|\n");
            PrintText("@785,1:FC12G|" + offendDto.PrnDesc + "|\n");

            var listStringOffend = SeparateText(offendDto.LongDesc, 8, 50);
            PrintText("@785,220:FC12G|" + listStringOffend[0] + "|\n");
            PrintText("@810,220:FC12G|" + listStringOffend[0] + "|\n");
            PrintText("@835,220:FC12G|" + listStringOffend[0] + "|\n");
            PrintText("@860,220:FC12G|" + listStringOffend[0] + "|\n");

            var listStringButir = SeparateText(compoundDto.Compound1Type.CompDesc, 15, 40);
            PrintText("@890,1:MF185|BUTIR-BUTIR :|\n");
            PrintText("@915,1:MF185|KESALAHAN|\n");
            PrintText("@890,220:FC12G|" + listStringButir[0] + "|\n");
            PrintText("@910,220:FC12G|" + listStringButir[1] + "|\n");
            PrintText("@945,220:FC12G|" + listStringButir[2] + "|\n");
            PrintText("@965,220:FC12G|" + listStringButir[3] + "|\n");

            PrintText("@995,1:MF156|DIKELUARKAN OLEH : |\n");
            PrintText("@995,480:MF156|KOD SAKSI : |\n");
            PrintText("@995,265:FC12G|" + enforcerDto.EnforcerId + "|\n");
            PrintText("@995,630:FC12G|" + compoundDto.WitnessId + "|\n");

            PrintText("@1025,1:MF156|CARA PENYERAHAN  : |\n");
            PrintText("@1025,240:FC12G|"+ delivery.ShortDesc + "|");

            if (compoundDto.Kadar == "Y")
            {
                PrintText("@1070,1:MF107|KADAR BAYARAN KOMPAUN|\n");
                PrintText("@1100,1:MF156|Bayaran dalam   Bayaran selepas  Bayaran selepas 30|\n");
                PrintText("@1125,1:MF156|tempoh 14 hari  tempoh 14 hari   hari sebelum tindakan|\n");
                PrintText("@1150,1:MF156|                                 undang-undang        |\n");

                PrintText("@1180,1:MF107|" + formatPrintAmt + "|\n");
                PrintText("@1180,210:MF107|" + formatPrintAmt2 + "|\n");
                PrintText("@1180,427:MF107|" + formatPrintAmt3 + "|\n");
            }
            else
            {
                PrintText("@1075,1:MF156|Sila Rujuk Bahagian Perundangan dan Pendakwaan|\n");
                PrintText("@1100,1:MF156|Majlis Perbandaraan Kuantan, Jalan Tanah Putih|\n");
                PrintText("@1125,1:MF156|25150, Kuantan(No Telefon:09-5121555)|\n");
                PrintText("@1150,1:MF156|dalam tempoh 14 hari|\n");
            }
            PrintText("@1210,1:MF107|TEMPOH TAMAT : " + formatTempohDate + "|\n");
            if (GeneralBll.IsPrintSignature())
                PrintText("@1100,500:S" + compoundDto.EnforcerId + ",HMULT2,VMULT2|");

            var passbulan = PassBulanBll.GetPassBulanByCarNum(compoundDto.Compound1Type.CarNum);
            if (passbulan != null)
            {
                string cPassType = passbulan.PassType;
                if (cPassType == "S" || cPassType == "V" || cPassType == "M" || cPassType == "L" || cPassType == "K" || cPassType == "X" || cPassType == "Y")
                    PrintText("@1450,300:FC12G|(PENGECUALIAN)|\n");
            }

            PrintText("@1662,135:MF107|" + compoundDto.CompNum + "|\n");
            PrintText("@1662,625:MF107|" + compoundDto.Compound1Type.CarNum + "|\n");

            PrintText("@1725,643:FC12G|: " + formatPrintDate + "|\n");
            PrintText("@1780,680:FC12G|: " + offendDto.PrnDesc + "|\n");

            PrintText("@1835,105:FC12G|: " + offendDto.IncomeCode + "|\n");
            PrintText("@1835,680:MF107|: L|\n");

            PrintText("}");
        }

        private void PrintCompound1(CompoundDto compoundDto, OffendDto offendDto, ActDto actDto, EnforcerDto enforcerDto)
        {

            int i = 0;
            string saksiName = "", scanDateTime = "";

            string formatPrintAmtWord = SetAmount2Word(compoundDto.Compound1Type.CompAmt);
            string formatPrintAmt = FormatPrintAmount(compoundDto.Compound1Type.CompAmt);
            string formatPrintAmt2 = FormatPrintAmount(compoundDto.Compound1Type.CompAmt2);
            string formatPrintAmt3 = FormatPrintAmount(compoundDto.Compound1Type.CompAmt3);
            string formatPrintDate = GeneralBll.FormatPrintDate(compoundDto.CompDate);
            string formatPrintTime = GeneralBll.FormatPrintTime(compoundDto.CompTime);
            string formatCouponDate = GeneralBll.FormatPrintDate(compoundDto.Compound1Type.CouponDate);
            string formatCouponTime = GeneralBll.FormatPrintTime(compoundDto.Compound1Type.CouponTime + "00");

            //string formatTempohDate = AddDate(compoundDto.CompDate, 14);
            string formatTempohDate;
            if (offendDto.PrintFlag == "S")
                formatTempohDate = AddDate(compoundDto.CompDate, 30);
            else
                formatTempohDate = AddDate(compoundDto.CompDate, 14);

            string formatTempohDate3Days = AddDate(compoundDto.CompDate, 2);
            string formatNotaTempohDate = GeneralBll.FormatPrintDate(compoundDto.TempohDate);

            var offendDesc = offendDto.PrnDesc + " " + offendDto.ShortDesc;
            var category = TableFilBll.GetCarCategory(compoundDto.Compound1Type.Category);
            if (category == null)
            {
                category = new CarCategoryDto();
                category.ShortDesc = "Missing Code(" + compoundDto.Compound1Type.Category + ")";
            }

            var delivery = TableFilBll.GetDeliveryByCode(compoundDto.Compound1Type.DeliveryCode);
            if (delivery == null)
            {
                delivery = new DeliveryDto();
                delivery.ShortDesc = "Missing Code(" + compoundDto.Compound1Type.DeliveryCode + ")";
            }

            var saksi = EnforcerBll.GetEnforcerById(compoundDto.WitnessId);
            if (saksi == null)
                saksiName = " ";
            else
                saksiName = saksi.EnforcerName;

            InitialisePrinter();
            SetLeftMargin();
            SetLineSpacing(10);

            string strKodHasil = offendDto.IncomeCode.PadRight(10);
            string strNoKmp = compoundDto.CompNum;

            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            SetBold(false);
            SetFontSize(15);
            SetBarCodeHight(45);
            PrintBarcode(strKodHasil);
            PrintText("                                                                              ");
            PrintBarcode(strNoKmp);
            SetLineSpacing(15);
            PrintLine();

            SetBold(false);
            SetFontSize(26);
            PrintText("KOD HASIL    : ");
            SetBold(true);
            PrintText(strKodHasil);
            PrintText("                            ");
            SetBold(false);
            PrintText("NO KOMPAUN : ");
            SetBold(true);
            PrintText(strNoKmp);
            PrintLine();
            PrintLine();

            SetLineSpacing(30);
            SetBold(false);
            SetFontSize(26);
            PrintText("TARIKH                      : ");
            SetBold(true);
            PrintText(formatPrintDate);
            SetBold(false);
            PrintText("          ");
            PrintText("WAKTU           : ");
            SetBold(true);
            PrintText(formatPrintTime);
            PrintLine();

            SetBold(false);
            SetFontSize(26);
            PrintText("NO.KERETA               :  ");
            SetBold(true);
            SetFontSize(30);
            PrintText(compoundDto.Compound1Type.CarNum.PadRight(15));
            SetBold(false);
            PrintLine();

            SetFontSize(26);
            SetBold(false);
            PrintText("NO. CUKAI                 : ");
            SetBold(true);
            PrintText(compoundDto.Compound1Type.RoadTax);
            PrintLine();

            SetBold(false);
            PrintText("JENIS KENDERAAN  : ");
            SetBold(true);
            PrintText(category.ShortDesc.PadRight(30));
            PrintLine();

            SetBold(false);
            PrintText("MODEL KENDERAAN  : ");
            SetBold(true);
            PrintText(compoundDto.Compound1Type.CarTypeDesc.PadRight(30));
            PrintLine();

            SetBold(false);
            PrintText("WARNA                        : ");
            SetBold(true);
            PrintText(compoundDto.Compound1Type.CarColorDesc);
            PrintLine();

            SetBold(false);
            PrintText("TEMPAT                      : ");
            SetBold(true);
            PrintText(compoundDto.StreetDesc);
            PrintLine();

            SetBold(false);
            PrintText("NO PETAK                   : ");
            SetBold(true);
            PrintText(compoundDto.Compound1Type.LotNo);
            PrintLine();

            scanDateTime = compoundDto.Compound1Type.ScanDateTime;
            if (scanDateTime.TrimEnd().Length > 10 && scanDateTime.TrimEnd() != "ERROR")
            {
                SetBold(true);
                PrintText("TARIKH DAN MASA IMBAS UP PARKING : " + compoundDto.Compound1Type.ScanDateTime);
                PrintLine();
                PrintLine();
                SetBold(false);
            }
            else
            {
                if (scanDateTime.TrimEnd() == "ERROR")
                {
                    SetBold(true);
                    PrintText("TAK DAPAT IMBAS UP PARKING ");
                    PrintLine();
                    PrintLine();
                    SetBold(false);
                }
                else
                {
                    PrintLine();
                    PrintLine();
                }
            }

            SetBold(false);
            SetLineSpacing(25);
            PrintLine();
            PrintText("KOD KESALAHAN  :  ");
            PrintText(offendDto.PrnDesc.PadRight(20));
            PrintLine();
            var listStringOffend = SeparateText(offendDto.LongDesc, 5, 55);
            PrintText(listStringOffend[0]);
            PrintLine();
            PrintText(listStringOffend[1]);
            PrintLine();
            PrintText(listStringOffend[2]);
            PrintLine();
            PrintText(listStringOffend[3]);
            PrintLine();
            PrintText(listStringOffend[4]);
            PrintLine();

            SetBold(false);
            PrintText("DIKELUARKAN OLEH : ");
            SetBold(true);
            PrintText(compoundDto.EnforcerId);
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();

            SetBold(true);
            SetLineSpacing(15);
            SetFontSize(40);
            PrintText("             KADAR BAYARAN KOMPAUN");
            PrintLine();
            PrintLine();

            SetFontSize(23);
            SetBold(false);
            PrintText("Bayaran dalam           Bayaran selepas      Bayaran selepas 28");
            PrintLine();
            nLine++;
            PrintText("tempoh 14 hari          tempoh 14 hari          hari sebelum tindakan");
            PrintLine();
            PrintText("                                                                                    undang-undang");
            PrintLine();
            SetBold(true);
            SetFontSize(30);
            PrintText(formatPrintAmt + "               " + formatPrintAmt2 + "              " + formatPrintAmt3);
            SetBold(false); 
            PrintLine();
            PrintLine();
            SetFontSize(22);
            SetBold(false);
            PrintText("Dikeluarkan oleh                                         ");
            //SetFontSize(24);
            //PrintText("*ANDA DIKEHENDAKI MEMBUAT");
            //PrintLine();
            //PrintText("                                                                 BAYARAN DI KAUNTER UPSB ATAU DI   ");
            //PrintLine();
            //PrintText("                                                                 APLIKASI UP PARKING(TEL:097867775)");
            //PrintLine();
            SetFontSize(22);

            //PrintText("Dikeluarkan oleh                                           ");
            //SetFontSize(24);
            //PrintText("*Jelaskan bayaran di kaunter UPSB");
            //PrintLine();
            //PrintText("                                                                   di alamat:-PT2275, Tingkat Bawah,");
            //PrintLine();
            //PrintText("                                                                   Jln P.Puteh sentral,1 / 4, ");
            //PrintLine();
            //PrintText("                                                                   16800 P.Puteh, Kelantan");
            //PrintLine();
            //SetFontSize(22);
            PrintText("............................................");
            PrintLine();
            PrintText("(Tandatangan Pegawai Yang Mengeluarkan Notis)");
            PrintLine();
            PrintText("b/p Datuk Bandar                ");
            PrintLine();
            PrintText("Majlis Bandaraya Iskandar Puteri        ");
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();

            //            *ANDA DIKEHENDAKI MEMBUAT PEMBAYARAN / PERTANYAAN DI KAUNTER PEJABAT USAHA PERANTI SDN. BHD.
            PrintFormFeed();

        }

        private void PrintCompoundOld1(CompoundDto compoundDto, OffendDto offendDto, ActDto actDto, EnforcerDto enforcerDto)
        {

            string saksiName = "";

            string formatPrintAmtWord = SetAmount2Word(compoundDto.Compound1Type.CompAmt);
            string formatPrintAmt = FormatPrintAmount(compoundDto.Compound1Type.CompAmt);
            string formatPrintAmt2 = FormatPrintAmount(compoundDto.Compound1Type.CompAmt2);
            string formatPrintAmt3 = FormatPrintAmount(compoundDto.Compound1Type.CompAmt3);
            string formatPrintDate = GeneralBll.FormatPrintDate(compoundDto.CompDate);
            string formatPrintTime = GeneralBll.FormatPrintTime(compoundDto.CompTime);

            string formatTempohDate = AddDate(compoundDto.CompDate, 14);
            string formatNotaTempohDate = GeneralBll.FormatPrintDate(compoundDto.TempohDate);

            var offendDesc = offendDto.PrnDesc + " " + offendDto.ShortDesc;
            var category = TableFilBll.GetCarCategory(compoundDto.Compound1Type.Category);
            if (category == null)
                category = new CarCategoryDto();

            var delivery = TableFilBll.GetDeliveryByCode(compoundDto.Compound1Type.DeliveryCode);
            if (delivery == null)
                delivery = new DeliveryDto();

            var saksi = EnforcerBll.GetEnforcerById(compoundDto.WitnessId);
            if (saksi == null)
                saksiName = " ";
            else
                saksiName = saksi.EnforcerName;

            // Prepare coupon information
            string couponNumber = compoundDto.Compound1Type.CouponNumber.TrimEnd();
            string couponDate = GeneralBll.FormatPrintDate(compoundDto.Compound1Type.CouponDate);
            string couponTime = GeneralBll.FormatPrintTime(compoundDto.Compound1Type.CouponTime);

            string strKodHasil = offendDto.IncomeCode.PadRight(8);
            string strNoKmp = compoundDto.CompNum;

            /************** Start of compound layout *****************/
            PrintText("\x1B\x45\x5A");
            PrintText("{PRINT,QSTOP50,STOP2500:");

            PrintText("@215,1:BC128,HIGH 10,WIDE 3|"+ offendDto.IncomeCode + "|\n");
            PrintText("@215,400:BC128,HIGH 10,WIDE 3|" + compoundDto.CompNum  + "|\n");

            if (Convert.ToInt32(compoundDto.PrintCnt) > 0)
                PrintText("@220,250:FC12G|SALINAN|\n");

            PrintText("@290,1:FC12G|KOD HASIL     :|\n");
            PrintText("@290,400:FC12G|NO KOMPAUN:|\n");
            PrintText("@290,245:FC12G|" + offendDto.IncomeCode + "|\n");
            PrintText("@290,580:FC12G|" + compoundDto.CompNum  + " |\n");
            PrintText("@340,1:MF156|NO.KENDERAAN   :|\n");
            PrintText("@340,245:MF072|" + compoundDto.Compound1Type.CarNum + "|\n");
            PrintText("@340,480:MF156|TARIKH   :|\n");
            PrintText("@340,615:FC12G|"+ formatPrintDate + "|\n");

            PrintText("@380,1:MF156|JENIS KENDERAAN:|\n");
            PrintText("@380,245:FC12G|" + category.ShortDesc + " |\n");
            PrintText("@380,480:MF156|WAKTU    :|\n");
            PrintText("@380,615:FC12G|" + formatPrintTime + " |\n");

            PrintText("@425,1:MF156|MODEL KENDERAAN:|\n");
            PrintText("@425,245:FC12G|" + compoundDto.Compound1Type.CarTypeDesc + " |\n");

            PrintText("@465,1:MF156|NO PETAK       :|\n");
            PrintText("@465,245:FC12G|" + compoundDto.Compound1Type.LotNo + "|\n");

            var listStringStreet = SeparateText(compoundDto.StreetDesc, 6, 45);
            PrintText("@505,1:MF156|JALAN          :|\n");
            PrintText("@505,215:FC12G|" + listStringStreet[0] + "|\n");

            var listStringTempatjadi = SeparateText(compoundDto.Tempatjadi, 6, 45);
            PrintText("@545,1:MF156|TEMPAT/LOKASI :|\n");

            PrintText("@545,245:FC12G|" + listStringTempatjadi[0] + "|\n");
            PrintText("@585,245:FC12G|" + listStringTempatjadi[1] + "|\n");
            PrintText("@625,245:FC12G|" + listStringTempatjadi[2] + "|\n");

            PrintText("@640,5:FC12G|PERUNTUKAN UNDANG-UNDANG YANG BERKAITAN:|");

            var listStringAct = SeparateText(actDto.LongDesc, 4, 50);
            PrintText("@665,5:FC12G|" + listStringAct[0] + "|\n");
            PrintText("@690,5:FC12G|" + listStringAct[1] + "|\n");
            PrintText("@715,5:FC12G|" + listStringAct[2] + "|\n");

            PrintText("@745,1:FC12G|SEKSYEN|\n");
            PrintText("@745,185:FC12G|KETERANGAN|\n");
            PrintText("@770,1:FC12G|-------|\n");
            PrintText("@770,185:FC12G|----------|\n");
            PrintText("@795,1:FC12G|" + offendDto.PrnDesc + "|\n");

            var listStringOffend = SeparateText(offendDto.LongDesc, 8, 50);
            PrintText("@795,185:FC12G|" + listStringOffend[0] + "|\n");
            PrintText("@820,185:FC12G|" + listStringOffend[0] + "|\n");
            PrintText("@845,185:FC12G|" + listStringOffend[0] + "|\n");
            PrintText("@870,185:FC12G|" + listStringOffend[0] + "|\n");

            var listStringButir = SeparateText(compoundDto.Compound1Type.CompDesc, 15, 40);
            PrintText("@900,1:MF185|BUTIR-BUTIR :|\n");
            PrintText("@925,1:MF185|KESALAHAN|\n");
            PrintText("@900,185:FC12G|" + listStringButir[0] + "|\n");
            PrintText("@920,185:FC12G|" + listStringButir[1] + "|\n");
            PrintText("@955,185:FC12G|" + listStringButir[2] + "|\n");
            PrintText("@975,185:FC12G|" + listStringButir[3] + "|\n");

            if (couponNumber != "")
            {
                PrintText("@1000,265:MF156|NO KUPON   : |\n");
                PrintText("@1000,500:FC12G|"+ couponNumber + "|");
                PrintText("@1030,265:MF156|TARIKH/MASA:|\n");
                if (couponTime != "")
                    PrintText("@1030,500:FC12G|" + couponDate + " " +  couponTime) ;
                else
                    PrintText("@1030,500:FC12G|" + couponDate) ;
            }

            PrintText("@1060,1:MF156|DIKELUARKAN OLEH : |\n");
            PrintText("@1060,480:MF156|KOD SAKSI : |\n");
            PrintText("@1060,265:FC12G|" + enforcerDto.EnforcerId + "|\n");
            PrintText("@1060,630:FC12G|" + compoundDto.WitnessId + "|\n");

            if (compoundDto.Kadar == "Y")
            {
                PrintText("@1100,1:MF107|KADAR BAYARAN KOMPAUN|\n");
                PrintText("@1130,1:MF156|Bayaran dalam   Bayaran selepas  Bayaran selepas 30|\n");
                PrintText("@1160,1:MF156|tempoh 14 hari  tempoh 14 hari   hari sebelum tindakan|\n");
                PrintText("@1185,1:MF156|                                 undang-undang        |\n");

                PrintText("@1210,1:MF107|" + formatPrintAmt + "|\n");
                PrintText("@1210,210:MF107|" + formatPrintAmt2 + "|\n");
                PrintText("@1210,427:MF107|" + formatPrintAmt3 + "|\n");
            }
            else
            {
                PrintText("@1100,1:MF156|Sila Rujuk Bahagian Perundangan dan Pendakwaan|\n");
                PrintText("@1130,1:MF156|Majlis Perbandaraan Kuantan, Jalan Tanah Putih|\n");
                PrintText("@1160,1:MF156|25150, Kuantan(No Telefon:09-5121555)|\n");
                PrintText("@1185,1:MF156|dalam tempoh 14 hari|\n");
            }
            PrintText("@1255,1:MF107|TEMPOH TAMAT : "+ formatTempohDate  + "|\n");
            if (GeneralBll.IsPrintSignature())
                PrintText("@1100,500:S"+ compoundDto.EnforcerId + ",HMULT2,VMULT2|");

            PrintText("@1295,1:MF156|Tempoh bayaran dikira dari tarikh kesalahan|\n");
            PrintText("@1320,1:MF156|dilakukan, termasuk Hari Ahad dan hari|\n");
            PrintText("@1345,1:MF156|kelepasan Am.|\n");

            var passbulan = PassBulanBll.GetPassBulanByCarNum(compoundDto.Compound1Type.CarNum);
            if (passbulan != null)
            {
                string cPassType = passbulan.PassType;
                if (cPassType == "S" || cPassType == "V" || cPassType == "M" || cPassType == "L" || cPassType == "K" || cPassType == "X" || cPassType == "Y")
                    PrintText("@1450,300:FC12G|(PENGECUALIAN)|\n");
            }

            PrintText("@1672,135:MF107|" + compoundDto.CompNum + "|\n");
            PrintText("@1672,625:MF107|"+ compoundDto.Compound1Type.CarNum + "|\n");

            PrintText("@1735,643:FC12G|: "+ formatPrintDate + "|\n");
            PrintText("@1790,680:FC12G|: "+ offendDto.PrnDesc + "|\n");

            PrintText("@1845,105:FC12G|: "+ offendDto.IncomeCode + "|\n");
            PrintText("@1845,680:MF107|: L|\n");

            PrintText("}");

        }
        public void PrintTest()
        {
            InitialisePrinter();
            SetLeftMargin();
            SetLineSpacing(10);

            SetPageMode();
            PrintImage(0);
            SetExitPageMode();
            SetLineSpacing(20);
            SetFontSize(36);
            SetBold(true);
            PrintText("                      MAJLIS PERBANDARAN KLANG");
            PrintLine();

            SetBold(false);
            SetFontSize(17);
            PrintText("BANGUNAN SULTAN ALAM SHAH, JALAN PERBANDARAN, 41675 KLANG BANDAR DIRAJA, SELANGOR DARUL EHSAN.");
            PrintLine();
            PrintText("NOMBOR TELEFON:03-3371 6044/33716141/3375 5555/3375 8026 NOMBOR FAKSIMILE:03-3372 0344/3374 8459");
            PrintLine();
            PrintText("LAMAN SESAWANG: www.mpklang.gov.my E-MEL:aduan@mpklang.gov.my TALIAN BEBAS TOL: 1 800 88 23826");
            PrintLine();
            PrintLine();

            SetBold(false);
            SetFontSize(15);
            SetBarCodeHight(40);
            PrintBarcode("524");
            PrintText("                                                        ");
            PrintBarcode("240524171213001");
            SetLineSpacing(15);
            PrintLine();

            SetBold(false);
            SetFontSize(26);
            PrintText("KOD KAUNTER : ");
            SetBold(true);
            PrintText("524");
            PrintText("                            ");
            SetBold(false);
            PrintText("NO KOMPAUN : ");
            SetBold(true);
            PrintText("240524171213001");
            PrintLine();
            PrintLine();
            SetLineSpacing(10);

            PrintLine();
            SetLineSpacing(25);
            PrintText("240524171213001");
            PrintText("PERINTAH PENGANGKUTAN JALAN (PERUNTUKAN TEMPAT LETAK");
            PrintLine();
            PrintText("KERETA)(MAJLIS PERBANDARAN KLANG) 2007");
            PrintLine();
            SetBold(true);
            SetFontSize(36);
            PrintText("         TAWARAN MENGKOMPAUN SUATU KESALAHAN");
            PrintLine();
            PrintLine();

            SetBold(false);
            SetFontSize(26);
            PrintText("Kepada : ");
            SetBold(true);
            PrintText("PEMUNYA BERDAFTAR KENDERAAN MOTOR");
            PrintLine();
            SetBold(false);
            PrintLine();

            PrintText("Tuan/Puan,");
            PrintLine();
            PrintText("Rujukan : ");
            SetBold(true);
            PrintText("240524171213001");
            PrintLine();
            PrintLine();

            SetBold(false);
            SetFontSize(20);
            PrintText("Menurut maklumat / aduan yang diterima, saya berpendapat bahawa tuan / puan telah  ");
            PrintLine();
            PrintText("melakukan kesalahan yang berikut:");
            PrintLine();
            PrintLine();
            SetFontSize(26);
            PrintText("PERUNTUKAN UNDANG-UNDANG BERKAITAN : ");
            PrintLine();
            SetBold(true);
            PrintText("PERENGGAN (36(2)(g),PERINTAH PENGANGKUTAN JALAN");
            PrintLine();
            PrintText("(PERUNTUKAN TEMPAT LETAK KERETA) (MPK) 2007 ");
            PrintLine();

            SetBold(false);
            PrintText("MANA-MANA PEGAWAI YSNG DIBERI KUASA OLEH MAJLIS");
            PrintLine();
            PrintText("ANA-MANA ATENDEN LETAK KERETA ATAU MANA-MANA PEGAWAI");
            PrintLine();
            PrintText("POLIS BOLEH MENUNDA, MENGALIH DAN MENAHAN MANA-MANA");
            PrintLine();
            PrintText("KENDERAAN MOTOR, APA-APA BARANG ATAU BENDA YANG");
            PrintLine();
            PrintText("MENGHALANG ATAU MENYEBABKAN TERHALANG MANA-MANA PILI");
            PrintLine();
            PrintText("BOMBA DI TEMPAT ATAU PETAK LETAK KERETA ");
            PrintLine();

            PrintLine();
            SetBold(false);
            PrintText("NO.PENDAFTARAN       : ");
            SetBold(true);
            PrintText("WJY1233");
            SetBold(false);
            PrintText("           LESEN KEN : ");
            SetBold(true);
            PrintText("R102377773");
            PrintLine();
            PrintLine();

            SetBold(false);
            PrintText("BUATAN/MODEL           : ");
            SetBold(true);
            PrintText("PROTON SAGA");
            PrintLine();
            PrintLine();

            SetBold(false);
            PrintText("JENIS                            : ");
            SetBold(true);
            PrintText("KERETA");
            PrintLine();
            PrintLine();

            SetBold(false);
            PrintText("TARIKH KESALAHAN   : ");
            SetBold(true);
            PrintText("01-12-2017");
            SetBold(false);
            PrintText("             WAKTU        : ");
            SetBold(true);
            PrintText("11:39");
            PrintLine();
            PrintLine();

            SetBold(false);
            PrintText("TEMPAT KESALAHAN  : ");
            SetBold(true);
            PrintText("LINTANG SUNGAI KERAMAT 10A");
            PrintLine();
            PrintLine();

            SetBold(false);
            PrintText("PETAK KENDERAAN    : ");
            SetBold(true);
            PrintText("OKU");
            PrintLine();
            PrintLine();

            SetBold(false);
            PrintText("BUTIR-BUTIR             : ");
            SetBold(true);
            PrintText("DILETAKKAN DI TANAH");
            PrintLine();
            SetBold(false);
            PrintText("KESALAHAN                   ");
            SetBold(true);
            PrintText("LAIN DARIPADA TEMPAT");
            PrintLine();
            PrintText("LETAK KERETA YANG DISEDIAKAN OLEH");
            PrintLine();
            PrintText("MAJLIS");
            PrintLine();
            PrintLine();

            SetBold(false);
            PrintText("SERAHAN DOKUMEN    : ");
            SetBold(true);
            PrintText("DENGAN TANGAN");
            PrintLine();
            PrintLine();

            SetBold(false);
            PrintText("NAMA PENGADU           : ");
            SetBold(true);
            PrintText("U301 Mohammed Shairuel Fahmi Bin Ahmad");
            PrintLine();
            PrintText("                                         Suhaimi");
            PrintLine();
            PrintLine();

            SetBold(false);
            PrintText("JAWATAN PENGADU    : ");
            SetBold(true);
            PrintText("KEW101 - Pembantu Penguatkuasa (N17)");
            PrintLine();
            PrintLine();

            SetBold(false);
            PrintText("SAKSI                           : ");
            SetBold(true);
            PrintText("Ahmad Haeriri Bin Ahmad Suhaimi");
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();

            SetBold(false);
            SetFontSize(20);
            PrintText("       TANDATANGAN PENGADU : ");
            SetFontSize(30);
            PrintText("...........................................");
            SetFontSize(20);
            PrintLine();
            PrintText("                                                                 b/p YANG DIPERTUA     ");
            PrintLine();
            PrintText("                                             MAJLIS PERBANDARAN KLANG, SELANGOR DARUL EHSAN");
            PrintLine();
            PrintLine();
            PrintLine();

            SetBold(false);
            SetFontSize(18);
            SetLineSpacing(18);
            PrintText("(2)    Tuan / puan adalah dengan ini dimaklumkan bahawa menurut kuasa yang diberi");
            PrintLine();
            PrintText("kepada saya oleh PERINTAH PENGANGKUTAN JALAN (PERUNTUKAN TEMPAT LETAK KERETA)(MAJLIS");
            PrintLine();
            PrintText("PERBANDARAN KLANG) 2007 ,saya bersedia dan dengan ini menawarkan untuk mengkompaun");
            PrintLine();
            PrintText("kesalahan itu dengan bayaran wang sebanyak");
            PrintLine();

            PrintLine();
            SetBold(true);
            SetFontSize(24);
            PrintText("      RM300.00(RINGGIT MALAYSIA  TIGA RATUS  SAHAJA)");

            PrintLine();
            PrintLine();

            SetBold(true);
            SetFontSize(20);
            PrintText("Nota:");
            PrintLine();
            PrintText("TAWARAN BAYARAN RM10.00 (RINGGIT MALAYSIA SEPULUH SAHAJA) DALAM TEMPOH TIGA (3)");
            PrintLine();
            PrintText("HARI. Tawaran ini akan habis tempoh pada  15-12-2017" );
            PrintLine();
            PrintLine();

            SetBold(false);
            SetFontSize(18);
            SetLineSpacing(28);
            PrintText("Jika tawaran ini diterima, pembayaran mestilah dibuat dengan wang tunai atau dengan kiriman");
            PrintLine();
            PrintText("wang, wang pos, pesanan juruwang, pesanan  bank, atau bank draf yang  dibuat untuk  dibayar");
            PrintLine();
            PrintText("kepada  Yang  Dipertua  Majlis  Perbandaran  Klang  dan  dipalang  'Akaun Penerima Sahaja'.");
            PrintLine();
            PrintText("Pembayaran melalui pos mestilah dialamatkan kepada Yang Dipertua, Majlis Perbandaran Klang.");
            PrintLine();
            PrintText("Suatu  resit  rasmi akan dikeluarkan apabila kompaun dibayar. Pembayaran boleh juga  dibuat");
            PrintLine();
            PrintText("melalui apa-apa cara lain yang ditetapkan  oleh  Majlis  dari  semasa  ke  semasa. Sebarang");
            PrintLine();
            PrintText("pertanyaan dan semakan berkenaan Tawaran  Mengkompaun  Suatu  Kesalahan  boleh  menghubungi");
            PrintLine();
            PrintText("Jabatan Undang-Undang, Tingkat 2, Pejabat Cawangan MPK, Lot 175, Jalan Tengku Kelana, 41000");
            PrintLine();
            PrintText("Klang Bandar Diraja, Selangor  Darul  Ehsan di talian telefon 03-3375 8033/ 3375 8026  atau");
            PrintLine();
            PrintText("faksimile 03-3374 8459. ");


            PrintLine();
            PrintLine();
            PrintText("(3)    Tawaran ini akan habis tempoh pada  ");
            SetBold(true);
            SetFontSize(26);
            PrintText("15-12-2017 . ");
            SetBold(false);
            SetFontSize(18);
            PrintText("Jika pembayaran penuh jumlah wang ");
            PrintLine();
            PrintText("yang dinyatakan di atas diterima pada atau sebelum penutupan urusan pada  tarikh  tersebut,");
            PrintLine();
            PrintText("tiada apa-apa langkah perbicaraan  selanjutnya akan diambil  terhadap  tuan/puan  berhubung");
            PrintLine();
            PrintText("dengan  kesalahan  itu. Jika  tidak, pendakwaan  akan  dijalankan tanpa  notis selanjutnya.");
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();

            SetPageMode();
          //  SetPrintPosition(1);
            PrintImage(1);
            SetExitPageMode();
            SetFontSize(18);
            PrintText("     TANDATANGAN PEGAWAI MENGKOMPAUN : ");
            SetFontSize(30);
            PrintText(".....................................");
            PrintLine();
            SetFontSize(18);
            PrintText("                                                                                    DATIN FADZILAH BINTI ABDUL AZIZ");
            PrintLine();
            PrintText("                                                                                    PENGARAH UNDANG-UNDANG");
            PrintLine();
            PrintText("                                                                                    B/P YANG DIPERTUA MAJLIS PERBANDARAN KLANG");
            PrintLine();
            PrintText("                                                                                    SELANGOR DARUL EHSAN");
            PrintLine();
            PrintLine();

            SetFontSize(18);
            PrintText("Pembayaran boleh dibuat disemua kauter bayaran MPK mengikut waktu yang ditetapkan. ");
            PrintLine();
            PrintText("Sebarang aduan berkenaan Tawaran Mengkompaun Suatu Kesalahan,sila berurusan dengan ");
            PrintLine();
            PrintText("Jabatan Komunikasi dan Pengaduan Awam, MPK.");
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            SetFontSize(20);
            PrintText("Cetakan no resit.:                                              ");
            PrintLine();
            SetFontSize(30);
            PrintText("...........................................................................");
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            SetBold(true);
            SetFontSize(36);
            PrintText("                   MENERIMA TAWARAN MENGKOMPAUN");
            PrintLine();
            PrintLine();
            SetBold(false);
            SetFontSize(24);
            PrintText("Yang Dipertua");
            PrintLine();
            PrintText("Majlis Perbandaran Klang,");
            PrintLine();
            PrintText("Selangor Darul Ehsan");
            PrintLine();
            PrintLine();
            SetFontSize(20);
            PrintText("Saya merujuk kepada tawaran mengkompaun suatu kesalahan di bawah rujukan  ");
            PrintLine();
            SetBold(true);
            SetFontSize(24);
            PrintText("240524171213001");
            SetBold(false);
            PrintText("dan bertarikh  ");
            SetBold(true);
            PrintText("01-12-2017");
            SetBold(false);
            SetFontSize(20);
            PrintText("Saya terima tawaran itu ");
            PrintLine();
            PrintText("dan sertakan bersama-sama ini wang tunai/kiriman wang pos/pesanan juruwang/");
            PrintLine();
            PrintText("pesanan bank/bank draf bank no. * ....................... bagi sebanyak ");
            PrintLine();
            PrintText("RM .........................(RINGGIT MALAYSIA ..............................) ");
            PrintLine();
            PrintText("bagi menjelaskan bayaran kompaun ini.");
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            SetFontSize(24);
            SetBold(false);
            PrintText("TANDATANGAN            :  ");
            SetFontSize(30);
            PrintText("....................................");
            PrintLine();
            PrintLine();
            SetFontSize(26);
            SetBold(false);
            PrintText("NO.PENDAFTARAN    : ");
            SetBold(true);
            PrintText("WJY1233");
            SetBold(false);
            PrintText("        TARIKH    : ");
            SetFontSize(30);
            PrintText("...../...../.........");
            SetFontSize(24);
            PrintLine();
            PrintLine();
            PrintText("NAMA(Huruf Besar)  : ");
            SetFontSize(30);
            PrintText("................................. ");
            PrintLine();
            PrintLine();
            SetFontSize(24);
            PrintText("NO.KAD PENGENALAN : ");
            SetFontSize(30);
            PrintText(".................................");
            PrintLine();
            PrintLine();
            SetFontSize(24);
            PrintText("ALAMAT                       : ");
            SetFontSize(30);
            PrintText(".................................");
            PrintLine();
            PrintLine();
            PrintText("                                  ");
            PrintText(".................................");
            PrintLine();
            PrintLine();

            SetBold(false);
            SetFontSize(15);
            SetBarCodeHight(40);
            PrintBarcode("405");
            PrintText("                                                        ");
            PrintBarcode("240524171213001");
            SetLineSpacing(15);
            PrintLine();

            SetBold(false);
            SetFontSize(26);
            PrintText("KOD KAUNTER : ");
            SetBold(true);
            PrintText("405");
            PrintText("                            ");
            SetBold(false);
            PrintText("NO KOMPAUN : ");
            SetBold(true);
            PrintText("240524171213001");
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintLine();
            PrintFormFeed();

            PrintClose();

        }

        private int FormatOffDesc(ref string sdest, string sSource, int maxLen)
        {
            int nPos, nLen, nPrePos;

            nLen = sSource.Length;
            if (nLen > maxLen)
                nLen = maxLen;
            nPos = FindString(sSource, " ", nLen);
            if (nPos > 0)
                sdest = sSource.Substring(0, nPos);
            else
                sdest = sSource;

            nLen = sdest.Trim().Length;

            if (nLen > maxLen + 5)
            {
                nPrePos = nLen;
                nLen = maxLen;
                nPos = GetNextChar(sSource, nLen, nPrePos);

                if (nPos <= 0)
                    nPos = nLen;
                sdest = sSource.Substring(0, nPos);
            }

            return nPos;
        }

        private int FindString(string source, string target, int startPos)
        {
            int srcLen = source.Length;

            int iResult = -1;
            if (startPos < srcLen)
            {
                for (int i = startPos; i < srcLen; i++)
                {
                    if (source.Substring(i, 1) == target)
                    {
                        iResult = i;
                        break;
                    }
                }
            }

            return iResult;
        }

        private int GetNextChar(string sSource, int nLen, int nPrePos)
        {
            int nPos = 0;

            nPos = FindString(sSource, "/", nLen);
            if (nPos <= 0 || nPos > nPrePos)
            {
                nPos = FindString(sSource, ",", nLen);
                if (nPos <= 0 || nPos > nPrePos)
                {
                    nPos = FindString(sSource, ".", nLen);
                    if (nPos <= 0 || nPos > nPrePos)
                    {
                        nPos = FindString(sSource, "\\", nLen);
                        if (nPos <= 0 || nPos > nPrePos)
                        {
                            nPos = FindString(sSource, "|", nLen);
                            if (nPos <= 0 || nPos > nPrePos)
                            {
                                nPos = FindString(sSource, "-", nLen);
                                if (nPos <= 0 || nPos > nPrePos)
                                {
                                    nPos = FindString(sSource, " ", nLen);
                                    if (nPos <= 0 || nPos > nPrePos)
                                        nPos = GetPrevChar(sSource, nPrePos);
                                }
                            }
                        }
                    }
                }
            }

            return nPos;
        }
        private int FindReverseString(string source, string target, int startPos)
        {
            int srcLen = source.Length;

            int iResult = -1;
            if (startPos < srcLen)
            {
                for (int i = startPos; i < 0; i--)
                {
                    if (source.Substring(i, 1) == target)
                    {
                        iResult = i;
                        break;
                    }
                }
            }

            return iResult;
        }

        private int GetPrevChar(string sSource, int nPrePos)
        {
            int nPos = 0;

            nPos = FindReverseString(sSource, " ", nPrePos);
            if (nPos <= 0 || nPos > nPrePos)
            {
                nPos = FindReverseString(sSource, ",", nPrePos);
                if (nPos <= 0 || nPos > nPrePos)
                {
                    nPos = FindReverseString(sSource, ".", nPrePos);
                    if (nPos <= 0 || nPos > nPrePos)
                    {
                        nPos = FindReverseString(sSource, "\\", nPrePos);
                        if (nPos <= 0 || nPos > nPrePos)
                        {
                            nPos = FindReverseString(sSource, "|", nPrePos);
                            if (nPos <= 0 || nPos > nPrePos)
                            {
                                nPos = FindReverseString(sSource, "-", nPrePos);
                                if (nPos <= 0 || nPos > nPrePos)
                                {
                                    nPos = FindReverseString(sSource, "/", nPrePos);
                                    if (nPos <= 0 || nPos > nPrePos)
                                        nPos = nPrePos;
                                }
                            }
                        }
                    }
                }
            }

            return nPos;
        }

        public void PrintBarcode(string compoundNo)
        {

            SetReadableOff();

            Byte[] buffer = new Byte[] { 29, 107, 73, 13 };

            buffer[3] = Convert.ToByte(compoundNo.Length);

            string data = compoundNo;//"043KK1B003750";
            byte[] bytesValue = Encoding.ASCII.GetBytes(data);

            var byteData = new Byte[buffer.Length + bytesValue.Length];
            Buffer.BlockCopy(buffer, 0, byteData, 0, buffer.Length);
            Buffer.BlockCopy(bytesValue, 0, byteData, buffer.Length, bytesValue.Length);

            WriteComm(byteData);

        }


        private string amtinWord(long num)
        {
            string[] amtword = {"KOSONG", "SE" , "DUA ", "TIGA ", "EMPAT ", "LIMA ",  "ENAM ", "TUJUH ", "LAPAN ", "SEMBILAN "};
            string sValue = "";

            sValue = amtword[num];

	        return (sValue) ;

        }


    private string SetAmount2Word(string amount)
        {
            long puluhribu, ratusribu, ribu, ratus, belas, amtlessthan10;
            long amt;
            string[] lessthan20 = { "KOSONG", "SATU", "DUA", "TIGA", "EMPAT", "LIMA", "ENAM", "TUJUH", "LAPAN", "SEMBILAN", "SEPULUH", "SEBELAS", "DUA BELAS", "TIGA BELAS", "EMPAT BELAS", "LIMA BELAS", "ENAM BELAS", "TUJUH BELAS", "LAPAN BELAS", "SEMBILAN BELAS" };
            string sRibuWord ="", sRatusWord="", sBelasWord="", sPuluhRibuWord="", sBuf="", sLessthan10 = "" , sValue = "";
            long decTemp = 0;

            decTemp = Convert.ToInt64(amount)/100;

            amt = decTemp;
            ribu = amt / 1000;
            amt = amt % 1000;

            ratus = amt / 100;
            amt = amt % 100;

            belas = amt;

            if (ribu > 0)
            {
                if (ribu < 20)
                    sRibuWord = lessthan20[ribu] + " RIBU";
                else
                {
                    if (ribu < 100)
                    {
                        puluhribu = ribu % 10;
                        sRibuWord = amtinWord((ribu / 10)) + "PULUH " + amtinWord(puluhribu) + " RIBU";
                    }
                    else
                    {
                        if (ribu > 100 && ribu < 1000)
                        {
                            ratusribu = ribu / 100;
                            puluhribu = ribu % 100;

                            if (puluhribu % 10 > 0)
                                sRibuWord = amtinWord(ribu/100) + " RATUS" + amtinWord((puluhribu / 10)) + " PULUH" + amtinWord((puluhribu % 10)) + " RIBU";
                            else
                                sRibuWord = amtinWord(ribu / 100) + " RATUS" + amtinWord((puluhribu / 10)) + " PULUH" ;
                        }
                    }
                }
            }

            if (ratus > 0)
                sRatusWord = amtinWord(ratus) + "RATUS";

            if (belas >= 20)
            {
                belas = belas / 10;
                amtlessthan10 = belas % 10;

                sBelasWord = amtinWord(belas) + " PULUH ";
                if (amtlessthan10 > 0)
                    sLessthan10 = amtinWord(amtlessthan10);
            }
            else
            {
                if (belas > 0)
                    sBelasWord = lessthan20[belas];
            }

            sValue = "RINGGIT MALAYSIA " + sRibuWord + " " + sRatusWord + " " + sBelasWord + " SAHAJA";
            return sValue;
        }


        private void SetPageMode()
        {
            Byte[] PageMode = { 0x1b, 0x4c };

            PrintChar(PageMode);

        }

        private void SetExitPageMode()
        {
            Byte[] ModeChange = { 0x0c };

            PrintChar(ModeChange);

        }

        private void SetPrintArea(int width, int height)
        {
            Byte[] buffer = new Byte[] { 27, 87, 0, 0, 0, 0, 0, 0, 0, 0 };
            int nxL, nxH, nyL, nyH;

            if (width > 0)
            {
                width = width * 10;
                nxL = 64;
                nxH = 3; // width / 256;
                buffer[6] = Convert.ToByte(nxL);
                buffer[7] = Convert.ToByte(nxH);
            }

            if (height > 0)
            {
                height = height * 25;
                nyL = height % 256;
                nyH = height / 256;
                buffer[8] = Convert.ToByte(nyL);
                buffer[9] = Convert.ToByte(nyH);
            }

            WriteComm(buffer);
        }

        private void SetPrintPos(double x, double y)
        {
            int nxL = 0, nxH = 0, nyL = 0, nyH = 0;
            Byte[] bufferXY = { 27, 79, 0, 0, 0, 0 };

            x =  x * 9;
            y =  y * 20;
            if (x > 0)
            {
                nxL = (int) x % 256;
                nxH = (int) x / 256;
            }

            if (y > 0)
            {
                nyL = (int) y % 256;
                nyH = (int) y / 256;
            }

            bufferXY[2] = Convert.ToByte(nxL);
            bufferXY[3] = Convert.ToByte(nxH);
            bufferXY[4] = Convert.ToByte(nyL);
            bufferXY[5] = Convert.ToByte(nyH);
            WriteComm(bufferXY);

        }

        private void PrintImage(int imageNumber)
        {
            Byte[] ImageBuffer = { 0x1b, 0x66, 0x00 };
            ImageBuffer[2] = System.Convert.ToByte(imageNumber);

            PrintChar(ImageBuffer);
        }

        //Woosim Commnad Method
        //1. Select TTF file
        private void selectTTF(string ttfName)
        {
            Byte[] sendTTFSelectCmd = new Byte[3] { 0x1B, 0x67, 0x46 };
            Byte[] endCmd = new Byte[1] { 0x00 };
            string selectTTF = ttfName;

            Byte[] selectedTTFName = Encoding.Default.GetBytes(selectTTF);

            WriteComm(sendTTFSelectCmd);

            WriteComm(selectedTTFName);

            WriteComm(endCmd);

        }

        //Woosim Commnad Method
        //2. Draw Text as width X dot, height Y dot.
        private void printTextUsingTTF(string text, int iXSize, int iYSize)
        {
            Byte[] sendTTFDataCmd = new Byte[5] { 0x1B, 0x67, 0x55, 0x00, 0x00 };
            sendTTFDataCmd[3] = System.Convert.ToByte(iXSize);
            sendTTFDataCmd[4] = System.Convert.ToByte(iYSize);

            string printText = text;
            int i = 0;
            Byte[] printTextDataHex = Encoding.BigEndianUnicode.GetBytes(printText);
            Byte[] endCmd = new Byte[2] { 0x00, 0x00 };

            try
            {
                WriteComm(sendTTFDataCmd);

                WriteComm(printTextDataHex);

                WriteComm(endCmd);
               // oStream.FlushAsync();

            }

            catch (Exception ex)
            {
                LogFile.WriteLogFile(ex.Message + ":" + text);
            }

        }


        //Woosim Commnad Method
        //2. Draw Text as width X dot, height Y dot.
        private void printTextUsingTTF(string text)
        {
            Byte[] sendTTFDataCmd = new Byte[5] { 0x1B, 0x67, 0x55, 0x00, 0x00 };
            sendTTFDataCmd[3] = System.Convert.ToByte(iFontSize);
            sendTTFDataCmd[4] = System.Convert.ToByte(iFontSize);

            string printText = text;
            int i = 0;
            Byte[] printTextDataHex = Encoding.BigEndianUnicode.GetBytes(printText);
            Byte[] endCmd = new Byte[2] { 0x00, 0x00 };

            try
            {
                WriteComm(sendTTFDataCmd);

                WriteComm(printTextDataHex);

                WriteComm(endCmd);
               // oStream.FlushAsync();
            }

            catch (Exception ex)
            {
                LogFile.WriteLogFile(ex.Message + ":" + text);
            }
        }

        public static string AddDate(string date, int days)
        {
            string target = "";
            int iyear = 0;
            int imonth = 0;
            int idate = 0;
            int daysinmonth = 0;

            iyear = Convert.ToInt32(date.Substring(0, 4));
            imonth = Convert.ToInt32(date.Substring(4, 2));
            idate = Convert.ToInt32(date.Substring(6, 2));

            idate = idate + days;

            switch (imonth)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    daysinmonth = 31;
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    daysinmonth = 30;
                    break;
                case 2:
                    if (iyear % 4 == 0)
                    {
                        daysinmonth = 29;
                    }
                    else
                    {
                        daysinmonth = 28;
                    }
                    break;
            }

            if (idate > daysinmonth)
            {
                idate = idate - daysinmonth;
                ++imonth;

                if (imonth > 12)
                {
                    imonth = imonth - 12;
                    ++iyear;
                }
            }

            target = idate.ToString("00") + "-" + imonth.ToString("00") + "-" + iyear.ToString();
            return target;
        }

        private void SetBox(int xL, int xH, int yL, int yH, int nThickness)
        {
            Byte[] Box = { 29, 105, 0, 0, 0, 0, 0 };

            Box[2] = Convert.ToByte(xL);
            Box[3] = Convert.ToByte(xH);
            Box[4] = Convert.ToByte(yL);
            Box[5] = Convert.ToByte(yH);
            Box[6] = Convert.ToByte(nThickness);

            PrintChar(Box);

        }

        public void SensorCheck()
        {
            Byte[] PrinterSensor = new Byte[2] { 27, 118 };
            WriteComm(PrinterSensor);
        }

            async public void WriteComm(byte[] data)
        {
            await oStream.WriteAsync(data, 0, data.Length);
            // Thread.Sleep(10);
            //oStream.Write(data, 0, data.Length);

        }

        //public void WriteComm(byte[] data)
        //{
        //    //   await oStream.WriteAsync(data, 0, data.Length);
        //    oStream.Write(data, 0, data.Length);

        //}

        public void PrintText(string text)
        {
            printTextUsingTTF(text);
            oStream.Flush();
            WritePrintFile(text);
        }
        public void PrintNormalText(string text)
        {
            byte[] convertedTextData = Encoding.Default.GetBytes(text);
            WriteComm(convertedTextData);

            //oStream.FlushAsync();
            WritePrintFile(text);
        }

        public void PrintTextNoAdded(string text)
        {
            byte[] convertedTextData = Encoding.Default.GetBytes(text);
            WriteComm(convertedTextData);
        }

        public void PrintChar(byte[] buffer)
        {
            WriteComm(buffer);
        }

        public void PrintFormFeed()
        {
//            Byte[] PrintLineFeed = new Byte[4] { 27, 122, 27, 60 };
            Byte[] PrintLineFeed = new Byte[4] { 27, 122, 27, 121 };
            WriteComm(PrintLineFeed);
        }


        public void PrintLine()
        {
            Byte[] buffer = new Byte[1] { 0x0A };
            WriteComm(buffer);
            nLine++;
        }

        public void PrintClose()
        {
            int iTry = 0;
            int status = 0;

            for (iTry = 0; iTry <= 3; iTry++)
            {
                status = PrinterQuery();
                if (status == 0)
                    break;
            }

            oStream.Close();
            oStream.Dispose();

            if (socket.IsConnected)
            {
                socket.Close();
            }

            //Printer no response
            //if (status == -1)
            //{
            //    LogFile.WriteLogFile($"PrintBll.PrintClose({GlobalClassAndroid.BluetoothService.Name}) Printer Status: {_printerStatus} - {_printerMessage}.");
            //}

            return;


            ////oStream.Close();
            ////oStream.Dispose();
            //byte[] myReadBuffer = new byte[1024];
            //string myString = "";
            //int numberOfBytesRead = 0, iDelay = 0;
            //do
            //{
            //    numberOfBytesRead = iStream.Read(myReadBuffer, 0, myReadBuffer.Length);
            //    if (numberOfBytesRead > 0)
            //    {
            //        myString = Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead);
            //        LogFile.WriteLogFile(myString);
            //    }
            //}
            //while (iStream.IsDataAvailable());

            //do
            //{
            //    while (iDelay < 20000)
            //        iDelay++;
            //}
            //while (oStream.IsDataAvailable());

            //if (socket.IsConnected)
            //{
            //    oStream.FlushAsync();
            //    oStream.Close();
            //    oStream.Dispose();
            //    socket.Close();
            //    socket = null;
            //    oStream = null;
            //    LogFile.WriteLogFile("oStream Flush");

            //}
        }

        private void SetHeightBarcode()
        {
            Byte[] buffer = new Byte[] { 29, 104, 40 };
            WriteComm(buffer);
        }

        private void SendPrinterStatus()
        {
            Byte[] buffer = new Byte[] { 27, 118 };
            WriteComm(buffer);
        }

        private void InitialisePrinter()
        {
            Byte[] buffer = new Byte[] { 27, 64 };
            WriteComm(buffer);
        }
        private void SetWidthBarcode()
        {
            Byte[] buffer = new Byte[] { 29, 119, 4 };
            WriteComm(buffer);
        }

        private void SetFontTriple()
        {
            Byte[] buffer = new Byte[] { 29, 33, 2 };
            WriteComm(buffer);
        }

        private void SetFontDouble()
        {
            Byte[] buffer = new Byte[] { 29, 33, 1 };
            WriteComm(buffer);
        }

        private void SetFontNormal()
        {
            Byte[] buffer = new Byte[] { 29, 33, 0 };
            WriteComm(buffer);
        }
        private void SetDefaultLineSpacing()
        {
            Byte[] buffer = new Byte[] { 27, 50 };
            WriteComm(buffer);
        }

        private void SetLineSpacing(int space)
        {
            Byte[] buffer = new Byte[] { 27, 51, 1 };
            if (space > 0 && space < 255)
                buffer[2] = Convert.ToByte(space);

            WriteComm(buffer);
        }

        private void SetBarCodeHight(int nHeight)
        {
            Byte[] buffer = new Byte[] { 29, 104, 60 };
            if (nHeight > 0 && nHeight < 255)
                buffer[2] = Convert.ToByte(nHeight);

            WriteComm(buffer);
        }

        /// <summary>
        /// set font size , range 0 - 7
        /// 0 = normal
        /// </summary>
        private void SetFontSize(int size)
        {
            iFontSize = size;
        }

        private void SetLeftMargin()
        {
            Byte[] buffer = new Byte[] { 29, 76, 0, 0 };

            WriteComm(buffer);
        }

        /// <summary>
        /// set font bold on/off
        /// true = on
        /// </summary>
        /// <param name="value"></param>
        private void SetBold(bool value)
        {

            if (value == true)
                selectTTF(Constants.PRIMESANSBOLD);
            else
                selectTTF(Constants.PRIMESANS);

        }
        private void SetReadableOff()
        {
            Byte[] buffer = new Byte[] { 29, 72, 0 };
            WriteComm(buffer);
        }

        private void SetReverse(bool bOn)
        {
            Byte[] buffer = new Byte[] { 29, 66, 1 };

            if (bOn)
                buffer[2] = Convert.ToByte(1);
            else
                buffer[2] = Convert.ToByte(0);

            WriteComm(buffer);

        }
        private void SetEmphasiz(bool bOn)
        {
            Byte[] buffer = new Byte[] { 27, 69, 0 };

            if (bOn)
                buffer[2] = Convert.ToByte(1);
            else
                buffer[2] = Convert.ToByte(0);

            WriteComm(buffer);

        }

        public int ReadChar(int Delay)
        {
            byte[] buffer = new byte[1024];
            int bytes = 0, retries = 0;
            _printerResponse = new byte[1024];  //clean previous response data.
            do
            {
                try
                {
                    //Unblock READ() from InputStream
                    using (var ist = socket.InputStream)
                    {
                        var _ist = (ist as InputStreamInvoker).BaseInputStream;
                        var aa = 0;
                        if ((aa = _ist.Available()) > 0)
                        {
                            bytes = _ist.Read(buffer, 0, aa);
                            System.Array.Resize(ref buffer, bytes);
                        }
                    }
                    // Blocked READ() from the InputStream
                    //bytes = socket.InputStream.Read(buffer, 0, buffer.Length);

                    if (bytes > 0)
                    {
                        // buffer can be over-written by next input stream data, so it should be copied
                        _printerResponse = Arrays.CopyOf(buffer, bytes);
                        break;
                    }

                    Thread.Sleep(Delay);
                    retries++;

                    //// Send the obtained bytes to the UI Activity
                    //service.handler.ObtainMessage(MainActivity.MESSAGE_READ, bytes, -1, rcvData).SendToTarget();
                }
                catch (Java.IO.IOException e)
                {
                    LogFile.WriteLogFile("PrintBll", "ReadChar()", e.Message, Enums.LogType.Error);
                    break;
                }

            } while (bytes == 0 && retries <= 10);

            if (bytes == 0)
            {
            }
            return bytes;
        }

        public byte[] ReadCharData()
        {
            return _printerResponse;
        }

        private string _printerMessage = "";
        private int _printerStatus = 0;
        private bool isBlackMarkOn = false;                 //later put this into printer setting control parameters
        private string bluetoothDeviceName = "Woosim";      //later put this into printer setting control parameters

        public string GetBluetoothDeviceName()
        {
            if (!adapter.IsEnabled)
            {
                return "";
            }

            return (adapter.Name);
        }

        public string GetPrinterMessage()
        {
            return _printerMessage;
        }

        public int PrinterQuery()
        {
            //initialized printer status
            _printerMessage = "";
            _printerStatus = 0;

            //query printer status
            PrintChar(new Byte[2] { 27, 118 });
            int bytes = ReadChar(200);
            var resp = ReadCharData();
            if (bytes > 0)
            {
                //Woosim printer response  (Citizen printer return either 0 or any error always no-response)
                if ((resp[0] & 0x01) == 0x01)       //BIT 0  = 2^0
                {
                    _printerMessage = "No Paper";
                    _printerStatus = 1;
                }
                if ((resp[0] & 0x02) == 0x02)       //BIT 1  = 2^1
                {
                    if (_printerMessage.Length > 0)
                        _printerMessage += ",";
                    _printerMessage += "Cover is opened";
                    _printerStatus = 1;
                }
                //No blackmark sensor detection required
                if (isBlackMarkOn && (resp[0] & 0x04) == 0x4)        //BIT 2  = 2^2
                {
                    if (_printerMessage.Length > 0)
                        _printerMessage += ",";
                    _printerMessage += "No BlackMark Found";
                    _printerStatus = 1;
                }
                if (_printerStatus == 0)
                {
                    _printerMessage = "Printer OK";
                }
            }
            else
            {
                _printerMessage += "Printer no response";
                _printerStatus = -1;
            }
            return _printerStatus;
        }


    }
}

