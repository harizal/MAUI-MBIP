using System;
using System.Collections.Generic;
using System.Text;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.Classes;
using Com.Woosim.Printer;
using Android.Graphics;
using AndroidCompound5.AimforceUtils;

namespace AndroidCompound5
{
    public class PrintDataBll
    {
        private const string ClassName = "PrintDataBll";
       
        //private const int MaxLengthPaper = 82;
        private const int MaxLengthPaper = 60;

        private readonly Byte[] PrintLineFeed = new Byte[4] { 27, 122, 27, 60 };
        private const string PrintAddedConstant = "   ";

        //from vivo mobile
        //maintain for compatible issue
        //ESC   K    1 
        Byte[] ExtechFont0; // = new Byte[3] { 27, 107, 49 };   //36 columns                X

        //ESC   K    2
        Byte[] ExtechFont1; // = new Byte[3] { 27, 33, 0 }; //48 columns                0+

        Byte[] ExtechFont2; // = new Byte[3] { 27, 33, 1 };   //57 columns                0-
        Byte[] ExtechFont3; // = new Byte[3] { 27, 107, 52 };   //64 columns                X
        Byte[] ExtechFont4; // = new Byte[3] { 27, 33, 1 };   //72 columns                0-
        Byte[] ExtechFont5; // = new Byte[3] { 27, 107, 54 };   //28 columns - monospace    X

        Byte[] FontBOLDON; // = new Byte[3] { 27, 69, 1 };
        Byte[] FontBOLDOFF; // = new Byte[3] { 27, 69, 0 };
        Byte[] FontDoubleHigh; // = new Byte[3] { 27, 33, 16 };
        Byte[] FontNormal = new Byte[3] { 27, 33, 0 }; //48 columns                0+
        Byte[] Contrast; // = new Byte[3] { 27, 33, 8 };
        Byte[] Peak_Power; // = new Byte[3] { 27, 80, 57 };               //         X
        Byte[] ResetBuffer; // = new Byte[1] { 24 };    
        Byte[] InitializePrinter = new Byte[2] { 27, 64 };    //Esc @

        private int iFontSize = 24;
        private string strTTFName = "";

        //end from

        private PrintDataDto _listPrintData;


        public PrintDataBll(PrintDataDto listData = null)
        {
            if (listData == null)
            {
                _listPrintData = new PrintDataDto();
            }
            else
            {
                _listPrintData = listData;
            }

        }

        public PrintDataDto GetListPrintData()
        {
            return _listPrintData;
        }

        public void UpdatePrintStat(bool updatePrintData, string tableName, int valueData, string documentNumber, int seqNo)
        {

            try
            {
                _listPrintData.FreqItems.Add(new PrintFreqItem()
                {
                    UpdateStat = updatePrintData,
                    TableName = tableName,
                    PrintFreqValue = valueData,
                    DocumentNumber = documentNumber,
                    SeqNo = seqNo
                });               

            }
            catch (Exception exc)
            {
                LogFile.WriteLogFile(ClassName, "UpdatePrintStat",exc.Message, Enums.LogType.Error);                
            }
        }

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
                PrintChar(sendTTFDataCmd);

                PrintChar(printTextDataHex);

                PrintChar(endCmd);
                // oStream.FlushAsync();
            }

            catch (Exception ex)
            {
                LogFile.WriteLogFile(ex.Message + ":" + text);
            }
        }

        public void PrintText(string text)
        {

            printTextUsingTTF(text);

            ////int li_out = 0;
            //int li_DataCount = 0;
            ////text = text + "\n";

            ////Skip printing after error detected, avoid program keep waiting to print & looked like hanged.
            ////if (mb_IsPrintError == true)
            ////    return;

            //if (text.Length > 0)
            //    li_DataCount = text.Length;

            //if (li_DataCount > 0)
            //{
            //    try
            //    {

            //        _listPrintData.DataItems.Add(new PrintDataItem()
            //        {
            //            Text = text,                       
            //        });


            //    }
            //    catch (Exception exc)
            //    {
            //        LogFile.WriteLogFile(ClassName, "PrintText",
            //                         exc.Message, Enums.LogType.Error);

            //        //PrintException(exc);
            //    }
            //}
        }

      
        public void PrintChar(byte[] buffer)
        {
            //set default delay to 200 milliseconds
            PrintChar(buffer, 100);
        }

        public void PrintChar(byte[] buffer, int Delay)
        {

            //int li_out = 0;
            int li_DataCount = 0;

         
            //if (buffer != null && buffer.Length > 0)
            if (buffer != null)
                li_DataCount = buffer.Length;

            if (li_DataCount > 0)
            {
                try
                {
                    _listPrintData.DataItems.Add(new PrintDataItem()
                    {
                        Byte = buffer,
                        IsByte = true
                    });

                }
                catch (Exception exc)
                {
                    //PrintException(exc);
                    LogFile.WriteLogFile(ClassName, "PrintChar",
                                     exc.Message, Enums.LogType.Info);
                }
            }
        }

        private string CenteringText(string as_Title, int countPerLine)
        {
            string sResult = "";
            int textLen = as_Title.Length;
            int iResult;
            int leftSpacing;

            if (textLen < countPerLine)
            {
                iResult = countPerLine - textLen;
                leftSpacing = iResult / 2;
            }
            else
                leftSpacing = 0;

            sResult = as_Title.PadLeft(textLen + leftSpacing);

            return sResult;
        }

        private void PrintTitle(string as_Title)
        {
            int li_len = 48; //PRINTER EXTECH
            string ls_Result = "";
            PrintChar(ExtechFont1);
            //PrintChar(Extech_Contrast);//contrast

            ls_Result = CenteringText(as_Title, li_len);

            PrintChar(FontDoubleHigh);
            PrintText(ls_Result + "\n");
            PrintChar(FontNormal);

        }

        public void PrintCompoundBitmap(Bitmap bitmap1)
        {
            PrintChar(WoosimCmd.InitPrinter());
            PrintChar(WoosimCmd.SetPageMode());
            PrintChar(WoosimImage.PrintCompressedBitmap(0, 0, 0, 0, bitmap1));
            PrintChar(WoosimCmd.PM_setStdMode());

            
        }
        public void PrintCompoundBitmap(Bitmap bitmap1, Bitmap bitmap2)
        {
            PrintChar(WoosimCmd.InitPrinter());
            PrintChar(WoosimCmd.SetPageMode());
            PrintChar(WoosimImage.PrintCompressedBitmap(0, 0, 0, 0, bitmap1));
            PrintChar(WoosimCmd.PM_setStdMode());

            PrintChar(WoosimCmd.InitPrinter());
            PrintChar(WoosimCmd.SetPageMode());
            PrintChar(WoosimImage.PrintCompressedBitmap(0, 0, 0, 0, bitmap2));
            PrintChar(WoosimCmd.PM_setStdMode());

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

                //switch (compoundDto.CompType)
                //{
                //    case Constants.CompType1:
                //        PrintCompound1(compoundDto, offendDto, actDto, enforcerDto);
                //        break;
                //    case Constants.CompType2:
                //        PrintCompound2(compoundDto, offendDto, actDto, enforcerDto);

                //        break;
                //    case Constants.CompType3:
                //        PrintCompound3(compoundDto, offendDto, actDto, enforcerDto);
                //        break;
                //    case Constants.CompType5:
                //        PrintCompound5(compoundDto, offendDto, actDto, enforcerDto);
                //        break;
                //    default:
                //        throw new Exception("No Compound Not Found.");

                //}

                LogFile.WriteLogFile("PrintCompound : End");
            }
            catch (Exception ex)
            {
                LogFile.WriteLogFile("UnExpected Error PrintCompound() : " + ex.Message, Enums.LogType.Error);
            }
        }

        public void PrintFormFeed()
        {
            Byte[] PrintLineFeed = new Byte[4] { 27, 122, 27, 121 };
            PrintChar(PrintLineFeed);
        }


        public void PrintLine()
        {
            Byte[] buffer = new Byte[1] { 0x0A };
            PrintChar(buffer);
        }
        private void SetLineSpacing(int space)
        {
            Byte[] buffer = new Byte[] { 27, 51, 1 };
            if (space > 0 && space < 255)
                buffer[2] = Convert.ToByte(space);

            PrintChar(buffer);
        }
        private void PrintBarcode1D_Page1(Enums.BarcodeSymbology Symbol, string Data)
        {
            //GS k m d1…dk NUL
            Byte[] BarcodeCode = new Byte[3] { 0x1d, 0x6b, (byte)Symbol };
            Byte[] BarcodeSubfix = new Byte[2] { 0x00, 0x0a };

            //ITF must have even data count
            if (Symbol.CompareTo(Enums.BarcodeSymbology.CodeITF) == 0)
                if ((Data.Length % 2) != 0)
                    Data = "0" + Data;

            byte[] BarcodeData = Encoding.ASCII.GetBytes(Data);
            PrintChar(BarcodeCode);
            PrintChar(BarcodeData);
            PrintChar(BarcodeSubfix);
            //PrintText(Data + "\n");   //PTP III Printer
            PrintText("\n");            //Woosim Printer
        }

        private void PrintBarcode1D_Page2(Enums.BarcodeSymbology Symbol, string Data)
        {
            //GS k m n d1…dn
            Byte[] BarcodeCode = new Byte[4] { 0x1d, 0x6b, (byte)Symbol, 0 };
            BarcodeCode[3] = Convert.ToByte(Data.Length);
            byte[] BarcodeData = Encoding.ASCII.GetBytes(Data);
            PrintChar(BarcodeCode);
            PrintChar(BarcodeData);
            //PrintText(Data + "\n");   //PTP III Printer
            PrintText("\n");            //Woosim Printer
        }


        private string amtinWord(long num)
        {
            string[] amtword = { "KOSONG", "SE", "DUA ", "TIGA ", "EMPAT ", "LIMA ", "ENAM ", "TUJUH ", "LAPAN ", "SEMBILAN " };
            string sValue = "";

            sValue = amtword[num];

            return (sValue);

        }


        private string SetAmount2Word(string amount)
        {
            long puluhribu, ratusribu, ribu, ratus, belas, amtlessthan10;
            long amt;
            string[] lessthan20 = { "KOSONG", "SATU", "DUA", "TIGA", "EMPAT", "LIMA", "ENAM", "TUJUH", "LAPAN", "SEMBILAN", "SEPULUH", "SEBELAS", "DUA BELAS", "TIGA BELAS", "EMPAT BELAS", "LIMA BELAS", "ENAM BELAS", "TUJUH BELAS", "LAPAN BELAS", "SEMBILAN BELAS" };
            string sRibuWord = "", sRatusWord = "", sBelasWord = "", sPuluhRibuWord = "", sBuf = "", sLessthan10 = "", sValue = "";
            long decTemp = 0;

            decTemp = Convert.ToInt64(amount) / 100;

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
                                sRibuWord = amtinWord(ribu / 100) + " RATUS" + amtinWord((puluhribu / 10)) + " PULUH" + amtinWord((puluhribu % 10)) + " RIBU";
                            else
                                sRibuWord = amtinWord(ribu / 100) + " RATUS" + amtinWord((puluhribu / 10)) + " PULUH";
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

            PrintChar(byteData);

        }

        public void Print2DBarcode(string buffer2D)
        {
            string data = buffer2D;
            byte[] bytesValue = Encoding.ASCII.GetBytes(data);

            byte[] byteData = WoosimBarcode.Create2DBarcodePDF417(2, 12, 4, 3, false, bytesValue);
            PrintChar(byteData);

        }

        public void PrintQRBarcode(string buffer2D)
        {
            string data = buffer2D;
            byte[] bytesValue = Encoding.ASCII.GetBytes(data);

            byte[] byteData = WoosimBarcode.Create2DBarcodeQRCode(0, 0x48, 3, bytesValue);
            PrintChar(byteData);


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

        private void SetHeightBarcode()
        {
            Byte[] buffer = new Byte[] { 29, 104, 40 };
            PrintChar(buffer);
        }

        private void SendPrinterStatus()
        {
            Byte[] buffer = new Byte[] { 27, 118 };
            PrintChar(buffer);
        }

        private void InitialisePrinter()
        {
            Byte[] buffer = new Byte[] { 27, 64 };
            PrintChar(buffer);
        }
        private void SetWidthBarcode()
        {
            Byte[] buffer = new Byte[] { 29, 119, 4 };
            PrintChar(buffer);
        }

        private void SetFontTriple()
        {
            Byte[] buffer = new Byte[] { 29, 33, 2 };
            PrintChar(buffer);
        }

        private void SetFontDouble()
        {
            Byte[] buffer = new Byte[] { 29, 33, 1 };
            PrintChar(buffer);
        }

        private void SetFontNormal()
        {
            Byte[] buffer = new Byte[] { 29, 33, 0 };
            PrintChar(buffer);
        }
        private void SetDefaultLineSpacing()
        {
            Byte[] buffer = new Byte[] { 27, 50 };
            PrintChar(buffer);
        }

        private void SetBarCodeHight(int nHeight)
        {
            Byte[] buffer = new Byte[] { 29, 104, 60 };
            if (nHeight > 0 && nHeight < 255)
                buffer[2] = Convert.ToByte(nHeight);

            PrintChar(buffer);
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

            PrintChar(buffer);
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
            PrintChar(buffer);
        }

        private void SetReverse(bool bOn)
        {
            Byte[] buffer = new Byte[] { 29, 66, 1 };

            if (bOn)
                buffer[2] = Convert.ToByte(1);
            else
                buffer[2] = Convert.ToByte(0);

            PrintChar(buffer);

        }
        private void SetEmphasiz(bool bOn)
        {
            Byte[] buffer = new Byte[] { 27, 69, 0 };

            if (bOn)
                buffer[2] = Convert.ToByte(1);
            else
                buffer[2] = Convert.ToByte(0);

            PrintChar(buffer);

        }

        //Woosim Commnad Method
        //1. Select TTF file
        private void selectTTF(string ttfName)
        {
            //Byte[] sendTTFSelectCmd = new Byte[3] { 0x1B, 0x67, 0x46 };
            //Byte[] endCmd = new Byte[1] { 0x00 };
            //string selectTTF = ttfName;

            //Byte[] selectedTTFName = Encoding.Default.GetBytes(selectTTF);


            //PrintChar(sendTTFSelectCmd);

            //PrintChar(selectedTTFName);

            //PrintChar(endCmd);
            PrintChar(WoosimCmd.SelectTTF(ttfName));

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
                PrintChar(sendTTFDataCmd);

                PrintChar(printTextDataHex);

                PrintChar(endCmd);
                // oStream.FlushAsync();

            }

            catch (Exception ex)
            {
                LogFile.WriteLogFile(ex.Message + ":" + text);
            }

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

            PrintChar(buffer);
        }

        private void SetPrintPos(double x, double y)
        {
            int nxL = 0, nxH = 0, nyL = 0, nyH = 0;
            Byte[] bufferXY = { 27, 79, 0, 0, 0, 0 };

            x = x * 9;
            y = y * 20;
            if (x > 0)
            {
                nxL = (int)x % 256;
                nxH = (int)x / 256;
            }

            if (y > 0)
            {
                nyL = (int)y % 256;
                nyH = (int)y / 256;
            }

            bufferXY[2] = Convert.ToByte(nxL);
            bufferXY[3] = Convert.ToByte(nxH);
            bufferXY[4] = Convert.ToByte(nyL);
            bufferXY[5] = Convert.ToByte(nyH);
            PrintChar(bufferXY);


        }

       
    }
}