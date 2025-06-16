using Android.App;
using Android.Content;
using Android.Graphics;
using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZXing;
using ZXing.Common;
using Application = Android.App.Application;
using Color = Android.Graphics.Color;
using Paint = Android.Graphics.Paint;
using Rect = Android.Graphics.Rect;

namespace AndroidCompound5
{
    public class PrintImageBll
    {
        private const int Text40 = 40;
        private const int Text35 = 35;
        private const int Text30 = 30;
        private const int Text28 = 28;
        private const int Text25 = 25;
        private const int Text24 = 24;
        private const int Text22 = 22;
        private const int Text20 = 20;
        private const int Text18 = 18;
        private const int Text16 = 16;
        private const int Text14 = 14;
        private const int Text12 = 12;

        private const int RobotoThin = 0;
        private const int RobotoThinItalic = 1;
        private const int RobotoLight = 2;
        private const int RobotoLightItalic = 3;
        private const int RobotoRegular = 4;
        private const int RobotoItalic = 5;
        private const int RobotoMedium = 6;
        private const int RobotoMediumItalic = 7;
        private const int RobotoBold = 8;
        private const int RobotoBoldItalic = 9;
        private const int RobotoBlack = 10;
        private const int RobotoBlackItalic = 11;
        private const int RobotoCondensed = 12;
        private const int RobotoCondensedItalic = 13;
        private const int RobotoCondensedBold = 14;
        private const int RobotoCondensedBoldItalic = 15;
        private const int CybertoothLight = 16;
        private const int dataunifon = 17;
        private const int datalatin = 18;
        private const int TimesNewRoman = 19;

        private const string FontArial = "sans-serif";

        private const int DefaultWidth = 800; //776;
        private const int BitmapDefaultWidth = 800;

        private int _fontSize = 12;
        private bool _isFontBold = false;
        private string _fontType = "sans-serif";
        private Color DefaultColor = Color.Black;

        private int _typeFaceType = -1;         //default standard font = -1
        int _positionX = 10;
        private int _oneLineSpace = 12;            //alter when setfontsize()
        private int MAXLENGTH = 45;            //alter when setfontsize()

        private Typeface GetTypeFace(int typefaceValue)
        {
            Typeface typeface;
            switch (typefaceValue)
            {
                case RobotoThin:
                    typeface = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/Roboto-Thin.ttf");
                    break;
                case RobotoThinItalic:
                    typeface = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/Roboto-ThinItalic.ttf");
                    break;
                case RobotoLight:
                    typeface = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/Roboto-Light.ttf");
                    break;
                case RobotoLightItalic:
                    typeface = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/Roboto-LightItalic.ttf");
                    break;
                case RobotoRegular:
                    typeface = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/Roboto-Regular.ttf");
                    break;
                case RobotoItalic:
                    typeface = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/Roboto-Italic.ttf");
                    break;
                case RobotoMedium:
                    typeface = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/Roboto-Medium.ttf");
                    break;
                case RobotoMediumItalic:
                    typeface = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/Roboto-MediumItalic.ttf");
                    break;
                case RobotoBold:
                    typeface = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/Roboto-Bold.ttf");
                    break;
                case RobotoBoldItalic:
                    typeface = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/Roboto-BoldItalic.ttf");
                    break;
                case RobotoBlack:
                    typeface = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/Roboto-Black.ttf");
                    break;
                case RobotoBlackItalic:
                    typeface = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/Roboto-BlackItalic.ttf");
                    break;
                case RobotoCondensed:
                    typeface = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/Roboto-Condensed.ttf");
                    break;
                case RobotoCondensedItalic:
                    typeface = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/Roboto-CondensedItalic.ttf");
                    break;
                case RobotoCondensedBold:
                    typeface = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/Roboto-BoldCondensed.ttf");
                    break;
                case RobotoCondensedBoldItalic:
                    typeface = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/Roboto-BoldCondensedItalic.ttf");
                    break;
                case CybertoothLight:
                    typeface = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/CybertoothLight.ttf");
                    break;
                case datalatin:
                    typeface = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/data-latin.ttf");
                    break;
                case dataunifon:
                    typeface = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/data-unifon.ttf");
                    break;
                case TimesNewRoman:
                    typeface = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/Times-New-Roman.ttf");
                    break;
                default:
                    return null;
            }
            return typeface;
        }


        public Paint SetTextPaint(Paint.Align align = null)
        {
            var textPaint = new Paint();

            textPaint.Color = Color.Black;
            textPaint.TextSize = _fontSize;
            textPaint.AntiAlias = true;
            textPaint.FakeBoldText = _isFontBold;

            if (_typeFaceType >= 0)
            {
                var typeFace = GetTypeFace(_typeFaceType);
                if (typeFace != null)
                {
                    textPaint.SetTypeface(typeFace);
                }
            }
            else
            {
                textPaint.FontFeatureSettings = _fontType;
            }
            return textPaint;
        }


        private static int GetApproxXToCenterText(string text, Typeface typeface,
           int fontSize, int widthToFitStringInto)
        {
            Paint p = new Paint();
            p.SetTypeface(typeface);
            p.TextSize = fontSize;
            float textWidth = p.MeasureText(text);
            int xOffset = (int)((widthToFitStringInto - textWidth) / 2f) - (int)(fontSize / 2f);
            return xOffset;
        }

        private PrintImageDto CreateTitle(string textData, int positionX, int positionY)
        {
            SetFontSize(Text30);

            Typeface typeface = GetTypeFace(RobotoItalic);
            var xOffSet = GetApproxXToCenterText(textData, typeface, Text30, DefaultWidth);
            var imageDto = CreateText(textData, xOffSet, positionY);
            SetFontNormal();
            return imageDto;
        }

        private PrintImageDto CreateTitle(string textData, int positionX, int positionY, int fontsize)
        {
            SetFontSize(fontsize);

            Typeface typeface = GetTypeFace(RobotoItalic);
            var xOffSet = GetApproxXToCenterText(textData, typeface, fontsize, DefaultWidth);
            var imageDto = CreateText(textData, xOffSet, positionY);
            SetFontNormal();
            return imageDto;
        }
        private string GetStringOrEmpty(string sValue)
        {
            return !string.IsNullOrEmpty(sValue) ? sValue : "";
        }

        private PrintImageDto SetDataPrint(string text, int positionX, int positionY, Paint textPaint)
        {
            return SetDataPrint(text, positionX, positionY, textPaint, 0, null);
        }
        private PrintImageDto SetDataPrint(string text, int positionX, int positionY, Paint textPaint, int maxWidthJustified, bool isJustified)
        {
            return SetDataPrint(text, positionX, positionY, textPaint, 0, null, maxWidthJustified, isJustified);
        }
        private PrintImageDto SetDataPrint(string text, int positionX, int positionY, Paint textPaint, int Width,
            Paint.Align Align, int maxWidthJustified = 0, bool isJustified = false)
        {
            var printImage = new PrintImageDto();
            printImage.Text = text ?? string.Empty;
            printImage.PositionX = positionX;
            printImage.PositionY = positionY;
            printImage.TextPaint = textPaint;
            printImage.Width = Width;
            printImage.Alignment = Align ?? Paint.Align.Left;
            printImage.IsJustified = isJustified;
            printImage.JustifiedMaxWidth = maxWidthJustified;
            return printImage;
        }


        private PrintImageDto CreateText(string text, int positionX, int positionY)
        {
            text = GetStringOrEmpty(text);

            var textPaint = SetTextPaint();
            return SetDataPrint(text, positionX, positionY, textPaint);
        }


        private PrintImageDto CreateText(string text, int positionX, int positionY, Paint.Align align, int Width)
        {
            var textPaint = SetTextPaint(align);
            return SetDataPrint(text, positionX, positionY, textPaint, Width, align);
        }

        private PrintImageDto CreateLine(int positionX, int positionY, int stopX, int stopY)
        {
            var paint = new Paint();
            paint.AntiAlias = true;
            paint.StrokeWidth = 2f;
            paint.Color = Color.Black;
            paint.SetStyle(Paint.Style.Stroke);
            paint.StrokeJoin = Paint.Join.Round;

            var printImage = new PrintImageDto();
            printImage.IsLine = true;
            printImage.PositionX = positionX;
            printImage.PositionY = positionY;
            printImage.StopX = stopX;
            printImage.StopY = stopY;
            printImage.TextPaint = paint;

            return printImage;

        }

        private Bitmap PrepareBitmap(int iLastPositionY)
        {
            var bmp = Bitmap.CreateBitmap(BitmapDefaultWidth, iLastPositionY, Bitmap.Config.Rgb565);
            bmp.EraseColor(Color.White);

            return bmp;
        }


        private void SetFontSize(int fontSize, string fontType = FontArial, int typeFaceType = TimesNewRoman)  //RobotoRegular)
        {
            _fontSize = fontSize;
            _fontType = fontType;
            _typeFaceType = typeFaceType;
            _oneLineSpace = (int)(fontSize * 1.2);
        }
        private void SetFontNormal()
        {
            _fontSize = Text22;
            _fontType = FontArial;
        }
        private void SetFontBold(bool value)
        {
            _isFontBold = value;
        }

        private int GetLastPositionY(List<PrintImageDto> listData)
        {
            var lastData = listData.LastOrDefault();
            if (lastData != null)
            {
                return lastData.PositionY;
            }
            return 0;
        }

        private Canvas CreateCanvas(Bitmap bmp)
        {
            var canvas = new Canvas(bmp);
            return canvas;
        }

        private static int GetTextAlignRight(string text, Typeface typeface, int fontSize, int maxWidth)
        {
            string output = SetTextMeasure(text, typeface, fontSize, maxWidth);
            Paint p = new Paint();
            p.SetTypeface(typeface);
            p.TextSize = fontSize;
            float textWidth = p.MeasureText(output);
            int xOffset = maxWidth - (int)textWidth;
            return xOffset;
        }

        private static int GetTextAlignCenter(string text, Typeface typeface, int fontSize, int maxWidth)
        {
            string output = SetTextMeasure(text, typeface, fontSize, maxWidth);
            Paint p = new Paint();
            p.SetTypeface(typeface);
            p.TextSize = fontSize;
            float textWidth = p.MeasureText(output);
            int xOffset = (maxWidth - (int)textWidth) / 2;
            return xOffset;
        }


        private static string SetTextMeasure(string text, Typeface typeface, int fontSize, int maxWidth)
        {
            string output = "";
            Paint p = new Paint();
            p.SetTypeface(typeface);
            p.TextSize = fontSize;
            float textWidth = p.MeasureText(text);
            if (textWidth > ((float)maxWidth))
            {
                //Truncate the excess replace with ... 
                string truncated = "...";
                float truncatedLength = (float)maxWidth - p.MeasureText(truncated);
                float y = 0;
                for (int x = 1; x < text.Length; x++)
                {
                    output += text.Substring(x, 1);
                    y = p.MeasureText(output);
                    if (y > truncatedLength)
                    {
                        output = text.Substring(1, x - 1) + truncated;
                        break;
                    }
                }
            }
            else
            {
                output = text;
            }

            return output;
        }

        private int GetTextMeasure(string text, Typeface typeface, int fontSize)
        {
            Paint p = new Paint();
            p.SetTypeface(typeface);
            p.TextSize = fontSize;
            float textWidth = p.MeasureText(text);
            return (int)textWidth;
        }


        private void DrawText(Canvas canvas, List<PrintImageDto> listData)
        {

            foreach (var printImageDto in listData)
            {
                if (printImageDto.IsLine)
                {
                    canvas.DrawLine(printImageDto.PositionX, printImageDto.PositionY, printImageDto.StopX,
                        printImageDto.StopY, printImageDto.TextPaint);
                }
                else if (printImageDto.IsRoundRectangle)
                {
                    canvas.DrawRoundRect(printImageDto.PositionLeft, printImageDto.PositionTop, printImageDto.PositionRight,
                        printImageDto.PositionBottom, 10, 10, printImageDto.TextPaint);
                }
                else if (printImageDto.IsRectangle)
                {
                    canvas.DrawRect(printImageDto.PositionLeft, printImageDto.PositionTop, printImageDto.PositionRight,
                        printImageDto.PositionBottom, printImageDto.TextPaint);
                }
                else if (printImageDto.IsLogo && printImageDto.Bitmap != null)
                {
                    if (printImageDto.SignKompaun)
                    {
                        var positionX = printImageDto.PositionX + 50;
                        var positionY = printImageDto.PositionY - 90;
                        //var bitmapWidth = printImageDto.Bitmap.Width;
                        var rect = new Rect(positionX, positionY, positionX + 200, positionY + 100);
                        canvas.DrawBitmap(printImageDto.Bitmap, null, rect, null);
                    }
                    else
                    {
                        if (printImageDto.Alignment == Paint.Align.Center)
                        {
                            //rect = new Rect(0, 0, 100, 100);
                            var bitmapWidth = printImageDto.Bitmap.Width;

                            var posX = (int)(DefaultWidth - bitmapWidth) / 2;
                            posX = 250;
                            var rect = new Rect(posX + 80, 10, posX + 280, 150);
                            canvas.DrawBitmap(printImageDto.Bitmap, null, rect, null);

                            //canvas.DrawBitmap(printImageDto.Bitmap, posX, 0, null);


                            //var rect = new Rect(0, 0, 100, 100);
                            //var rectDest = new Rect(0, 0, 100, 100);
                            //canvas.DrawBitmap(printImageDto.Bitmap, rect, rectDest, null);
                        }
                        else
                        {
                            if (printImageDto.IsJomPay)
                            {
                                var rect = new Rect(printImageDto.PositionX, printImageDto.PositionY, printImageDto.PositionX + printImageDto.Width, printImageDto.StopY);
                                canvas.DrawBitmap(printImageDto.Bitmap, null, rect, null);
                            }
                            else
                            {
                                if (printImageDto.PositionX > 0)
                                {
                                    var rect = new Rect(printImageDto.PositionLeft, printImageDto.PositionTop, printImageDto.PositionRight, printImageDto.PositionBottom);
                                    //var rect = new Rect(printImageDto.PositionX, printImageDto.PositionY, printImageDto.Width, printImageDto.PositionBottom);
                                    canvas.DrawBitmap(printImageDto.Bitmap, null, rect, null);
                                }
                                else
                                {
                                    Rect rect = new Rect(10, 10, 100, 100);
                                    canvas.DrawBitmap(printImageDto.Bitmap, null, rect, null);
                                }
                            }
                        }
                    }

                    printImageDto.Bitmap.Recycle();

                }
                else
                {
                    if (printImageDto.Width > 0)
                    {
                        //1. Measure the output text width. If output width > printImageDto.Width, truncate the output.
                        //2. Check the alignment
                        //3. Draw the output text.
                        string output = SetTextMeasure(printImageDto.Text, printImageDto.TextPaint.Typeface, (int)printImageDto.TextPaint.TextSize, printImageDto.Width);
                        int outputPosX = printImageDto.PositionX;
                        if (printImageDto.Alignment == Paint.Align.Right)
                        {
                            outputPosX += GetTextAlignRight(output, printImageDto.TextPaint.Typeface, (int)printImageDto.TextPaint.TextSize, printImageDto.Width);
                            canvas.DrawText(output, outputPosX, printImageDto.PositionY, printImageDto.TextPaint);

                        }
                        else if (printImageDto.Alignment == Paint.Align.Center)
                        {
                            outputPosX += GetTextAlignCenter(output, printImageDto.TextPaint.Typeface, (int)printImageDto.TextPaint.TextSize, printImageDto.Width);
                            canvas.DrawText(output, outputPosX, printImageDto.PositionY, printImageDto.TextPaint);
                        }
                        else     //default Align = LEFT
                        {
                            canvas.DrawText(output, outputPosX, printImageDto.PositionY, printImageDto.TextPaint);
                        }
                    }
                    else
                    {
                        var text = printImageDto.Text;

                        if (printImageDto.IsJustified)
                        {
                            var textWidth = GetTextMeasure(text, printImageDto.TextPaint.Typeface,
                                (int)printImageDto.TextPaint.TextSize);
                            if ((printImageDto.JustifiedMaxWidth - textWidth) <= 60)
                            {
                                text = AddTextSpace(printImageDto.Text, printImageDto.JustifiedMaxWidth);
                            }
                        }


                        canvas.DrawText(text, printImageDto.PositionX, printImageDto.PositionY,
                            printImageDto.TextPaint);
                    }
                }

            }
        }

        private string AddTextSpace(string text, int maxWidth)
        {
            var sData = text.Split(' ').ToList();
            for (int i = 0; i < sData.Count; i++)
            {
                sData[i] += " ";
                var tempData = string.Join(" ", sData);

                var paint = SetTextPaint();
                var textWidth = GetTextMeasure(tempData, paint.Typeface, (int)paint.TextSize);
                if (maxWidth - textWidth <= 5)
                {
                    break;
                }
            }
            return string.Join(" ", sData);
        }

        private PrintImageDto CreateLogoHeader(Context context, int logo)
        {
            var bitmap = BitmapFactory.DecodeResource(context.Resources, logo);

            var printImage = new PrintImageDto
            {
                IsLogo = true,
                Bitmap = bitmap,
                Alignment = Paint.Align.Center,
                IsJustified = true,
                Width = 200
            };

            return printImage;
        }

        private PrintImageDto CreateRectangle(int left, int top, int bottom, int right, bool fillColor)
        {
            var paint = new Paint();
            paint.AntiAlias = true;
            paint.StrokeWidth = 2f;
            paint.Color = Color.DarkGray;
            if (fillColor)
                paint.SetStyle(Paint.Style.Fill);
            else
                paint.SetStyle(Paint.Style.Stroke);

            var printImage = new PrintImageDto();
            printImage.IsRectangle = true;
            printImage.PositionLeft = left;
            printImage.PositionTop = top;
            printImage.PositionBottom = bottom;
            printImage.PositionRight = right;
            printImage.TextPaint = paint;

            printImage.PositionX = right;
            printImage.PositionY = bottom;

            return printImage;

        }

        private PrintImageDto CreateRectangle(int left, int top, int bottom, int right)
        {
            return CreateRectangle(left, top, bottom, right, false);
        }

        public Bitmap CreatePrintOutBitmap(Context context, int logo)
        {
            int positionX = 10;
            int position25 = 25;
            int addLine = 40;

            var listData = new List<PrintImageDto>
            {
                CreateLogoHeader(context, logo)
            };

            int lastPositionY = GetLastPositionY(listData);
            lastPositionY += 150;
            SetFontBold(true);
            listData.Add(CreateTitle("MAJLIS BANDARAYA PASIR GUDANG", 120, lastPositionY));
            lastPositionY += addLine;
            SetFontBold(false);
            listData.Add(CreateTitle("NOTIS KESALAHAN SERTA TAWARAN KOMPAUN", 120, lastPositionY));

            SetFontSize(18);
            lastPositionY += addLine + positionX;
            listData.Add(CreateText("KOD KAUNTER : 524", positionX, lastPositionY));
            listData.Add(CreateText("NO KOMPAUN : 314524200615001", 270, lastPositionY, Paint.Align.Right, 500));
            lastPositionY += positionX;
            listData.Add(CreateLine(positionX, lastPositionY, DefaultWidth, lastPositionY));

            lastPositionY += addLine;
            listData.Add(CreateText("NO KENDERAAN", positionX, lastPositionY));
            listData.Add(CreateText(": ", 250, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("NO CUKAI JALAN", positionX, lastPositionY));
            listData.Add(CreateText(": ", 250, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("JENAMA/MODEL", positionX, lastPositionY));
            listData.Add(CreateText(": ", 250, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("PETAK KENDERAAN", positionX, lastPositionY));
            listData.Add(CreateText(": ", 250, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("JENIS KENDERAAN", positionX, lastPositionY));
            listData.Add(CreateText(": ", 250, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("TARIKH & WAKTU", positionX, lastPositionY));
            listData.Add(CreateText(": ", 250, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("TEMPAT KESALAHAN", positionX, lastPositionY));
            listData.Add(CreateText(": ", 250, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("CARA PENYERAHAN", positionX, lastPositionY));
            listData.Add(CreateText(": ", 250, lastPositionY));

            lastPositionY += addLine;
            listData.Add(CreateText("KEPADA PEMUNYA/PEMANDU KENDERAAN TERSEBUT DI ATAS, TUAN/PUAN DIDAPATI", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("MELAKUKAN KESALAHAN SEPERTI BERIKUT :-", positionX, lastPositionY));
            lastPositionY += addLine;
            listData.Add(CreateText("BUTIR-BUTIR KESALAHAN :", positionX, lastPositionY));
            SetFontBold(true);
            lastPositionY += position25;
            listData.Add(CreateText("GAGAL MEMATUHI SYARAT YANG DINYATAKAN PADA KUPON LETAK KERETA IAITU TIDAK", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("MEMPAMERKAN KUPON YANG DIKELUARKAN OLEH MAJLIS SEMASA MELETAKAN KENDERAAN", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("BERMOTOR DI DALAM PETAK LETAK KERETA BERKUPON", positionX, lastPositionY));

            SetFontBold(false);
            lastPositionY += addLine;
            listData.Add(CreateText("PERUNTUKAN UNDANG-UNDANG :-", positionX, lastPositionY));
            SetFontBold(true);
            lastPositionY += position25;
            listData.Add(CreateText("Perengan 27(2), PERINTAH PENGANGKUTAN JALAN ( PERUNTUKAN TEMPAT LETAK", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("KERETA ) ( MAJLIS BANDARAYA PASIR GUDANG ) 2007", positionX, lastPositionY));

            SetFontBold(false);
            lastPositionY += addLine + positionX;
            listData.Add(CreateText("DIKELUARKAN OLEH", positionX, lastPositionY));
            listData.Add(CreateText(": ", 250, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("SAKSI", positionX, lastPositionY));
            listData.Add(CreateText(": ", 250, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("JABATAN", positionX, lastPositionY));
            listData.Add(CreateText(": ", 250, lastPositionY));

            SetFontBold(true);
            lastPositionY += addLine + positionX;
            listData.Add(CreateRectangle(20, lastPositionY, lastPositionY + 110, DefaultWidth, true));
            DefaultColor = Color.White;
            lastPositionY += addLine;
            listData.Add(CreateTitle("TAWARAN BAYARAN RM 10.00", 120, lastPositionY));
            lastPositionY += addLine;
            listData.Add(CreateTitle("DALAM TEMPOH 3 HARI", 120, lastPositionY));
            SetFontBold(false);
            DefaultColor = Color.Black;

            SetFontSize(18);
            SetFontBold(true);
            lastPositionY += addLine + addLine;
            listData.Add(CreateText("TAWARAN KOMPAUN :", positionX, lastPositionY));
            SetFontBold(false);
            lastPositionY += position25;
            listData.Add(CreateText("Saya bersedia mengkompaun kesalahan ini dengan Kadar Kompaun", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("RM30.00. Tawaran ini berkuat kuasa dalam tempoh 14 hari dari tarikh", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("notis ini. Kegagalan menjelaskan bayaran kompaun akan menyebabkan", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("TINDAKAN MAHKAMAH akan diteruskan.", positionX, lastPositionY));
            lastPositionY += addLine + positionX;
            listData.Add(CreateRectangle(390, lastPositionY - position25, lastPositionY + 7, 780, true));
            DefaultColor = Color.White;
            listData.Add(CreateText("TEMPOH", 400, lastPositionY));
            listData.Add(CreateText("KADAR (RM)", 500, lastPositionY));
            listData.Add(CreateText("KOMPAUN TAMAT", 620, lastPositionY));
            DefaultColor = Color.Black;

            listData.Add(CreateRectangle(391, lastPositionY + 5, lastPositionY + position25 + 5, 500, false));
            listData.Add(CreateRectangle(500, lastPositionY + 5, lastPositionY + position25 + 5, 620, false));
            listData.Add(CreateRectangle(620, lastPositionY + 5, lastPositionY + position25 + 5, 779, false));
            lastPositionY += position25;
            listData.Add(CreateText("1-3 HARI", 400, lastPositionY));
            listData.Add(CreateText("10.00", 510, lastPositionY));
            listData.Add(CreateText("17-06-2020", 630, lastPositionY));

            listData.Add(CreateRectangle(391, lastPositionY + 5, lastPositionY + position25 + 5, 500, false));
            listData.Add(CreateRectangle(500, lastPositionY + 5, lastPositionY + position25 + 5, 620, false));
            listData.Add(CreateRectangle(620, lastPositionY + 5, lastPositionY + position25 + 5, 779, false));
            lastPositionY += position25;
            //listData.Add(CreateRectangle(390, lastPositionY, lastPositionY, 780, false));
            listData.Add(CreateText("4-14 HARI", 400, lastPositionY));
            listData.Add(CreateText("30.00", 510, lastPositionY));
            listData.Add(CreateText("28-06-2020", 630, lastPositionY));

            SetFontBold(true);
            lastPositionY += position25 + positionX + positionX;
            listData.Add(CreateRectangle(450, lastPositionY - position25, lastPositionY + 70, 780));
            listData.Add(CreateText("Biller Code : 84277", 460, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("REF 1 : 314524200615001", 460, lastPositionY));
            listData.Add(CreateLine(positionX, lastPositionY, 380, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("REF 2 : No Tel Cth.0123456789", 460, lastPositionY));
            SetFontBold(false);
            listData.Add(CreateText("(PENGARAH UNDANG-UNDANG)", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("JABATAN UNDANG-UNDANG", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("b.p YANG DIPERTUA", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("MAJLIS BANDARAYA PASIR GUDANG", positionX, lastPositionY));

            lastPositionY += addLine + addLine + addLine;
            listData.Add(CreateText("SALINAN UNTUK ORANG KENA KOMPAUN", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateLine(positionX, lastPositionY, DefaultWidth, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("SALINAN UNTUK KEGUNAAN PEJABAT", positionX, lastPositionY));


            lastPositionY += addLine + addLine + addLine;
            listData.Add(new PrintImageDto
            {
                IsLogo = true,
                Bitmap = GenerateBarcode("314524200615001"),
                Alignment = Paint.Align.Left,
                IsJustified = true,
                PositionX = 1,
                PositionY = lastPositionY - addLine - addLine
            });
            listData.Add(CreateText("NO KOMPAUN : 314524200615001", positionX, lastPositionY));
            listData.Add(CreateText("KOD KAUNTER : 524", 600, lastPositionY));

            DefaultColor = Color.White;
            lastPositionY += addLine;
            listData.Add(CreateRectangle(positionX, lastPositionY - position25, lastPositionY + 40, 780, true));
            listData.Add(CreateText("UNTUK DIISI OLEH PEGAWAI", positionX, lastPositionY, Paint.Align.Center, 400));
            listData.Add(CreateText("UNTUK DIISI OLEH", 400, lastPositionY, Paint.Align.Center, 400));
            lastPositionY += position25;
            listData.Add(CreateText("MENGKOMPAUN", positionX, lastPositionY, Paint.Align.Center, 400));
            listData.Add(CreateText("ORANG KENA KOMPAUN", 400, lastPositionY, Paint.Align.Center, 400));
            DefaultColor = Color.Black;

            listData.Add(CreateRectangle(positionX + 1, lastPositionY + 10, lastPositionY + 250, 400, false));
            listData.Add(CreateRectangle(400, lastPositionY + 10, lastPositionY + 250, 779, false));

            var positionx1 = positionX + 3;
            var positionx2 = 400 + 3;
            lastPositionY += addLine;
            listData.Add(CreateText("Tawaran kompaun RM ..........................", positionx1, lastPositionY, Paint.Align.Left, 400));
            listData.Add(CreateText("Saya menerima tawaran mengkompaun suatu", positionx2, lastPositionY, Paint.Align.Left, 400));
            lastPositionY += position25;
            listData.Add(CreateText("Tarikh tamat kompaun ..........................", positionx1, lastPositionY, Paint.Align.Left, 400));
            listData.Add(CreateText("kesalahan bernomor 3145242006150001", positionx2, lastPositionY, Paint.Align.Left, 400));
            lastPositionY += position25;
            listData.Add(CreateText("Tandatangan & cop pegawai mengkompaun :", positionx1, lastPositionY, Paint.Align.Left, 400));

            lastPositionY += addLine + positionX;
            listData.Add(CreateText("Nama :", positionx2, lastPositionY, Paint.Align.Left, 400));
            lastPositionY += position25;
            listData.Add(CreateText("Tarikh :", positionx1, lastPositionY, Paint.Align.Left, 400));
            listData.Add(CreateText("No. Kad Pengenalan :", positionx2, lastPositionY, Paint.Align.Left, 400));
            lastPositionY += position25;
            listData.Add(CreateText("Waktu :", positionx1, lastPositionY, Paint.Align.Left, 400));
            listData.Add(CreateText("Alamat :", positionx2, lastPositionY, Paint.Align.Left, 400));

            lastPositionY += position25;

            for (int i = 0; i < 6; i++)
            {
                lastPositionY += 20;
                listData.Add(CreateText("", positionX, lastPositionY));
            }

            var bitmap = PrepareBitmap(lastPositionY);
            var canvas = CreateCanvas(bitmap);
            DrawText(canvas, listData);

            CreateFileBitmap(bitmap, "SampleBitmap");
            return bitmap;
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
        public Bitmap CreateKompaunType2Bitmap_2(Context context, int logo, int jompay, int Asign, CompoundDto compoundDto, OffendDto offendDto, ActDto actDto, EnforcerDto enforcerDto)
        {
            int nLine = 0, i = 0;
            string saksiName = "", scanDateTime = "", zoneName = "";

            string formatPrintAmtWord = SetAmount2Word(compoundDto.Compound2Type.CompAmt);
            string formatPrintAmt = FormatPrintAmount(compoundDto.Compound2Type.CompAmt);
            string formatPrintAmt2 = FormatPrintAmount(compoundDto.Compound2Type.CompAmt2);
            string formatPrintAmt3 = FormatPrintAmount(compoundDto.Compound2Type.CompAmt3);
            string formatPrintDate = GeneralBll.FormatPrintDate(compoundDto.CompDate);
            string formatPrintTime = GeneralBll.FormatPrintTime(compoundDto.CompTime);
            string amtkmp1 = FormatPrintAmount1(compoundDto.Compound2Type.CompAmt);
            string amtkmp2 = FormatPrintAmount1(compoundDto.Compound2Type.CompAmt2);
            string formatRayuanAmt = FormatPrintAmount1(offendDto.OffendAmt);
            string formatRayuanTandaAmt = FormatPrintAmount1(offendDto.OffendAmt);
            string formatTempohDate = AddDate(compoundDto.CompDate, 13);
            string formatTempohDate3Days = AddDate(compoundDto.CompDate, 2);
            string formatNotaTempohDate = GeneralBll.FormatPrintDate(compoundDto.TempohDate);

            var configDto = GeneralBll.GetConfig();
            if (configDto == null)
            {
                LogFile.WriteLogFile("Get Config null", Enums.LogType.Error);
            }

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

            var zone = TableFilBll.GetZoneByCodeAndMukim(compoundDto.Zone, compoundDto.Mukim);
            if (zone == null)
                zoneName = " ";
            else
                zoneName = zone.LongDesc;

            string strKodHasil = offendDto.IncomeCode.PadRight(8);
            string strNoKmp = compoundDto.CompNum;

            int positionX = 10;
            int position25 = 25;
            int addLine = 40;

            var listData = new List<PrintImageDto> { };
            int lastPositionY = 10;

            lastPositionY += addLine;
            SetFontBold(true);
            lastPositionY += position25;
            SetFontSize(22);

            listData.Add(CreateText("PADA menjalankan  kuasa-kuasa yang telah  diberikan di  bawah  seksyen 120(1)(e)", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("Akta Pengangkutan Jalan 1987 {Akta 333], dan  Perintah 39(1) dan (2) saya dengan", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("ini  menawar untuk  mengkompaun  kesalahan  ini dengan bayaran " + formatPrintAmt + " Jika", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("tuan/puan  menerima  tawaran ini, sila  jelaskan  bayaran  tersebut SECARA DALAM ", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("APLIKASI PARKMAX@JOHOR", positionX, lastPositionY));
            lastPositionY += position25;

            lastPositionY += addLine;
            listData.Add(CreateText("2.  Tawaran  mengkompaun  kesalahan ini  berkuat kuasa  selama  14  hari  sahaja", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("dari   tarikh   notis   ini   dikeluarkan   dan   jika  jawapan   tidak  diterima  dalam", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("tempoh, itu tindakan mahkamah akan diambil terhadap tuan/puan tanpa notis lagi.", positionX, lastPositionY));
            lastPositionY += position25;
            lastPositionY += addLine;

            string strURLImage = configDto.UrlImage + "council=MBIP&compound=" + compoundDto.CompNum;
            listData.Add(CreateBarcodeImageQRCode(10, lastPositionY + 5, strURLImage, 650, BarcodeAlign.Left));

            listData.Add(CreateRectangle(220, lastPositionY + 5, lastPositionY + 130, 800, false));
            SetFontSize(20);
            SetFontBold(true);
            listData.Add(CreateText("DISKAUN TERHADAP BAYARAN MENGIKUT TEMPOH HARI", positionX + 220, lastPositionY + addLine));
            listData.Add(CreateText("             1 - 14 HARI DISKAUN 50%         ", positionX + 330 , lastPositionY + addLine + position25));
            listData.Add(CreateText("            15 - 30 HARI DISKAUN 30%         ", positionX + 330 , lastPositionY + addLine + position25 + position25));

            var bitmapASign = BitmapFactory.DecodeResource(context.Resources, Asign);
            listData.Add(new PrintImageDto
            {
                IsLogo = true,
                SignKompaun = true,
                Bitmap = bitmapASign,
                Alignment = Paint.Align.Left,
                IsJustified = false,
                PositionX = 500,
                PositionY = lastPositionY+250,
                Width = 250
            });

            lastPositionY += addLine;
            lastPositionY += addLine;
            lastPositionY += addLine;
            lastPositionY += addLine;
            lastPositionY += addLine;
            lastPositionY += addLine;
            lastPositionY += addLine;
            listData.Add(CreateText("...........................................................................", 400, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("DATUK BANDAR", 520, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("MAJLIS BANDARAYA ISKANDAR PUTERI", 400, lastPositionY));

            //lastPositionY += addLine;
            //lastPositionY += addLine;
            //listData.Add(CreateLine(positionX, lastPositionY, DefaultWidth, lastPositionY));

            lastPositionY += addLine;
            lastPositionY += addLine;
            lastPositionY += addLine;

            //listData.Add(CreateLogoHeader(context, logo, positionX, lastPositionY- addLine, positionX + 130, lastPositionY + 80, Paint.Align.Left));

            //SetFontBold(true);
            //SetFontSize(21);
            //listData.Add(CreateTitle("MAJLIS BANDARAYA ISKANDAR PUTERI", 250, lastPositionY+10,Text22));
            //lastPositionY += position25;
            //listData.Add(CreateTitle("JABATAN PENGUATKUASA", 250, lastPositionY+10, Text22));
            //string strURLImage = configDto.UrlImage + "council=MBIP&compound=" + compoundDto.CompNum;
            //listData.Add(CreateBarcodeImageQRCode(650, lastPositionY- addLine - 10, strURLImage, 650, BarcodeAlign.Left));
            //lastPositionY += addLine;
            //lastPositionY += addLine;
            //lastPositionY += addLine;

            //SetFontSize(20);
            //listData.Add(CreateText("No Kenderaan", positionX, lastPositionY));
            //SetFontBold(true);
            //listData.Add(CreateText(": ", 180, lastPositionY));
            //listData.Add(CreateText(compoundDto.Compound1Type.CarNum.PadRight(15), 200, lastPositionY));
            //SetFontBold(false);

            //var listString1 = SeparateText(actDto.LongDesc, 5, 50);
            //SetFontSize(20);
            //lastPositionY += addLine;
            //listData.Add(CreateText("Peruntukan", positionX, lastPositionY));
            //SetFontBold(true);
            //listData.Add(CreateText(": ", 180, lastPositionY));
            //listData.Add(CreateText(listString1[0], 200, lastPositionY));
            //SetFontBold(false);
            //lastPositionY += position25;
            //listData.Add(CreateText("Undang-undang", positionX, lastPositionY));
            //SetFontBold(true);
            //listData.Add(CreateText(listString1[1], 200, lastPositionY));

            //if (listString1[2].TrimEnd() != "")
            //{
            //    lastPositionY += position25;
            //    listData.Add(CreateText(listString1[2], 200, lastPositionY));
            //}
            //if (listString1[3].TrimEnd() != "")
            //{
            //    lastPositionY += position25;
            //    listData.Add(CreateText(listString1[3], 200, lastPositionY));
            //}

            //SetFontBold(false);
            //lastPositionY += addLine;
            //listData.Add(CreateText("Perintah ", positionX, lastPositionY));
            //SetFontBold(true);
            //listData.Add(CreateText(": ", 180, lastPositionY));
            //listData.Add(CreateText(offendDto.PrnDesc, 200, lastPositionY));

            //lastPositionY += addLine;
            //listData.Add(CreateText("Tarikh & Waktu", positionX, lastPositionY));
            //SetFontBold(true);
            //listData.Add(CreateText(": ", 180, lastPositionY));
            //listData.Add(CreateText(formatPrintDate + " " + formatPrintTime, 200, lastPositionY));
            //SetFontBold(true);
            //lastPositionY += addLine;
            //listData.Add(CreateText("KERATAN UNTUK CATATAN PEMBAYARAN", 220, lastPositionY));
            //lastPositionY += position25;
            //listData.Add(CreateText("TERIMA KASIH", 350, lastPositionY));
            //SetFontBold(false);
            //lastPositionY += position25;
            //listData.Add(CreateText("KPD691184", 350, lastPositionY));


            for (i = 0; i < 6; i++)
            {
                lastPositionY += 20;
                listData.Add(CreateText("", positionX, lastPositionY));
            }

            var bitmap = PrepareBitmap(lastPositionY);
            var canvas = CreateCanvas(bitmap);
            DrawText(canvas, listData);

            CreateFileBitmap(bitmap, "SampleBitmap");
            return bitmap;
        }

        public Bitmap CreateKompaunType1Bitmap_1(Context context, int logo, int jompay, int Asign, CompoundDto compoundDto, OffendDto offendDto, ActDto actDto, EnforcerDto enforcerDto)
        {
            int nLine = 0, i = 0;
            string saksiName = "", scanDateTime = "", zoneName = "";

            string formatPrintAmtWord = SetAmount2Word(compoundDto.Compound1Type.CompAmt);
            string formatPrintAmt = FormatPrintAmount(compoundDto.Compound1Type.CompAmt);
            string formatPrintAmt2 = FormatPrintAmount(compoundDto.Compound1Type.CompAmt2);
            string formatPrintAmt3 = FormatPrintAmount(compoundDto.Compound1Type.CompAmt3);
            string formatPrintDate = GeneralBll.FormatPrintDate(compoundDto.CompDate);
            string formatPrintTime = GeneralBll.FormatPrintTime(compoundDto.CompTime);
            string amtkmp1 = FormatPrintAmount1(compoundDto.Compound1Type.CompAmt);
            string amtkmp2 = FormatPrintAmount1(compoundDto.Compound1Type.CompAmt2);
            string formatRayuanAmt = FormatPrintAmount1(offendDto.OffendAmt);
            string formatRayuanTandaAmt = FormatPrintAmount1(offendDto.OffendAmt);
            string formatTempohDate = AddDate(compoundDto.CompDate, 13);
            string formatTempohDate3Days = AddDate(compoundDto.CompDate, 2);
            string formatNotaTempohDate = GeneralBll.FormatPrintDate(compoundDto.TempohDate);

            var configDto = GeneralBll.GetConfig();
            if (configDto == null)
            {
                LogFile.WriteLogFile("Get Config null", Enums.LogType.Error);
            }

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

            var zone = TableFilBll.GetZoneByCodeAndMukim(compoundDto.Zone, compoundDto.Mukim);
            if (zone == null)
                zoneName = " ";
            else
                zoneName = zone.LongDesc;

            string strKodHasil = offendDto.IncomeCode.PadRight(8);
            string strNoKmp = compoundDto.CompNum;

            int positionX = 10;
            int position25 = 25;
            int addLine = 40;

            var listData = new List<PrintImageDto> { };
            int lastPositionY = 10;
            listData.Add(CreateLogoHeader(context, logo, positionX + 250, lastPositionY, positionX + 550, lastPositionY + 200, Paint.Align.Center));

            lastPositionY += addLine;
            lastPositionY += addLine;
            lastPositionY += addLine;
            lastPositionY += addLine;
            lastPositionY += addLine;
            SetFontBold(true);
            listData.Add(CreateTitle("MAJLIS BANDARAYA ISKANDAR PUTERI", 120, lastPositionY));
            lastPositionY += addLine;
            SetFontBold(false);
            listData.Add(CreateTitle("Perintah Lalulintas Jalan", 200, lastPositionY, Text28));
            lastPositionY += position25;
            listData.Add(CreateTitle("(Peruntukan Tempat Letak Kereta)", 180, lastPositionY, Text28));
            lastPositionY += position25;
            listData.Add(CreateTitle("Majlis Perbandaran Johor Bahru Tengah 2003", 150, lastPositionY, Text28));
            lastPositionY += position25;
            listData.Add(CreateTitle("(Perintah 39)", 240, lastPositionY, Text24));
            lastPositionY += addLine;
            listData.Add(CreateTitle("NOTIS KESALAHAN SERTA TAWARAN KOMPAUN", 50, lastPositionY, Text28));

            lastPositionY += addLine;
            listData.Add(CreateBarcodeImage(460, lastPositionY, strNoKmp, 300, BarcodeAlign.Left));
            lastPositionY += addLine;

            SetFontSize(22);
            lastPositionY += addLine + positionX;
            SetFontBold(false);
            listData.Add(CreateText("NO", 500, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(":", 520, lastPositionY));
            listData.Add(CreateText(strNoKmp, 540, lastPositionY));
            SetFontBold(false);


            lastPositionY += addLine;
            listData.Add(CreateText("Kepada Pemandu/Pemunya", positionX, lastPositionY));

            lastPositionY += addLine;
            listData.Add(CreateText("No Kenderaan", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(compoundDto.Compound1Type.CarNum.PadRight(15), 200, lastPositionY));
            SetFontBold(false);

            listData.Add(CreateText("Warna", 500, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 600, lastPositionY));
            listData.Add(CreateText(compoundDto.Compound1Type.CarColorDesc, 620, lastPositionY));
            SetFontBold(false);

            lastPositionY += addLine;
            listData.Add(CreateText("Jenama/Model", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(compoundDto.Compound1Type.CarTypeDesc, 200, lastPositionY));
            SetFontBold(false);

            lastPositionY += addLine;
            listData.Add(CreateText("Jenis", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(category.ShortDesc, 200, lastPositionY));
            SetFontBold(false);

            listData.Add(CreateText("No. Petak ", 500, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 600, lastPositionY));
            listData.Add(CreateText(compoundDto.Compound1Type.LotNo, 620, lastPositionY));
            SetFontBold(false);

            lastPositionY += addLine;
            SetFontBold(false);
            listData.Add(CreateText("Tarikh ", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(formatPrintDate, 200, lastPositionY));
            SetFontBold(false);

            listData.Add(CreateText("Waktu", 500, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 600, lastPositionY));
            listData.Add(CreateText(formatPrintTime, 620, lastPositionY));
            SetFontBold(false);


            string tempatjadi;
            tempatjadi = compoundDto.StreetDesc + "," + zoneName;

            var listStringStreet = SeparateText(tempatjadi, 6, 50);

            lastPositionY += addLine;
            listData.Add(CreateText("Tempat/Jalan", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(listStringStreet[0], 200, lastPositionY));
            if (listStringStreet[1].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringStreet[1], 200, lastPositionY));
            }
            if (listStringStreet[2].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringStreet[2], 200, lastPositionY));
            }
            if (listStringStreet[3].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringStreet[3], 200, lastPositionY));
            }
            if (listStringStreet[4].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringStreet[4], 200, lastPositionY));
            }


            scanDateTime = compoundDto.Compound1Type.ScanDateTime;
            if (scanDateTime.TrimEnd().Length > 10 && scanDateTime.TrimEnd() != "ERROR")
            {
                lastPositionY += addLine;
                //listData.Add(CreateText("TARIKH DAN MASA IMBAS SMART PARKING ", positionX, lastPositionY));
                listData.Add(CreateText("IMBAS SMART PARKING : YA", positionX, lastPositionY));
                //SetFontBold(true);
                //listData.Add(CreateText(": ", 500, lastPositionY));
                ////listData.Add(CreateText(GeneralBll.ConvertStringyyyymmddToddmmyyyyhhmmss(compoundDto.Compound1Type.ScanDateTime), 520, lastPositionY));
                //listData.Add(CreateText("YA", 520, lastPositionY));
                //SetFontBold(false);
            }
            //else
            //{
            //    if (scanDateTime.TrimEnd() == "ERROR" || scanDateTime.TrimEnd() == "")
            //    {
            //        lastPositionY += addLine;
            //        SetFontBold(true);
            //        listData.Add(CreateText("TAK DAPAT IMBAS SMART PARKING ", positionX, lastPositionY));
            //        SetFontBold(false);
            //    }
            //}

            SetFontSize(22);
            lastPositionY += addLine;
            listData.Add(CreateText("Dengan ini diberitahu kenderaan bermotor tuan/puan telah dilaporkan dengan kesalahan", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("seperti yang berikut:", positionX, lastPositionY));

            SetFontSize(22);
            var listString1 = SeparateText(actDto.LongDesc, 5, 45);
            lastPositionY += addLine;
            listData.Add(CreateText("Peruntukan", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(listString1[0], 200, lastPositionY));
            SetFontBold(false);
            lastPositionY += position25;
            listData.Add(CreateText("Undang-undang", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(listString1[1], 200, lastPositionY));

            if (listString1[2].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listString1[2], 200, lastPositionY));
            }
            if (listString1[3].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listString1[3], 200, lastPositionY));
            }

            SetFontBold(false);
            lastPositionY += addLine;
            listData.Add(CreateText("Perintah ", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(offendDto.PrnDesc, 200, lastPositionY));

            lastPositionY += addLine;
            listData.Add(CreateText("Kesalahan", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));

            string section = offendDto.LongDesc;
            var listStringSection = SeparateText(section, 5, 40);
            SetFontBold(true);
            listData.Add(CreateText(listStringSection[0], 200, lastPositionY));
            if (listStringSection[1].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringSection[1], 200, lastPositionY));
            }
            if (listStringSection[2].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringSection[2], 200, lastPositionY));
            }
            if (listStringSection[3].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringSection[3], 200, lastPositionY));
            }

            SetFontBold(false);
            lastPositionY += addLine;
            listData.Add(CreateText("Jumlah Kompaun", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(formatPrintAmt, 200, lastPositionY));


            SetFontBold(false);
            lastPositionY += addLine;
            listData.Add(CreateText("Dikeluarkan Oleh", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(enforcerDto.EnforcerId + "(" + enforcerDto.EnforcerName + ")", 200, lastPositionY));
            SetFontBold(true);
            lastPositionY += position25;
            listData.Add(CreateText(enforcerDto.Jabatan, 200, lastPositionY));

            SetFontBold(false);
            lastPositionY += addLine;
            listData.Add(CreateText("Tarikh", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(formatPrintDate, 200, lastPositionY));

            SetFontSize(21);
            lastPositionY += addLine;
            lastPositionY += addLine;
            lastPositionY += addLine;
            lastPositionY += addLine;
            var bitmapASign = BitmapFactory.DecodeResource(context.Resources, Asign);
            listData.Add(new PrintImageDto
            {
                IsLogo = true,
                SignKompaun = true,
                Bitmap = bitmapASign,
                Alignment = Paint.Align.Left,
                IsJustified = false,
                PositionX = 450,
                PositionY = lastPositionY-10,
                Width = 250
            });



            listData.Add(CreateText("..........................................................................", 400, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("DATUK BANDAR", 520, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("MAJLIS BANDARAYA ISKANDAR PUTERI", 400, lastPositionY));

            var bitmap = PrepareBitmap(lastPositionY);
            var canvas = CreateCanvas(bitmap);
            DrawText(canvas, listData);

            CreateFileBitmap(bitmap, "SampleBitmap");
            return bitmap;
        }

        public Bitmap CreateTestBitmap()
        {

            int positionX = 10;
            int position25 = 25;
            int addLine = 40;

            var listData = new List<PrintImageDto> { };
            int lastPositionY = 10;
            SetFontBold(true);
            listData.Add(CreateTitle("TEST PRINT", 120, lastPositionY));
            lastPositionY += addLine;
            SetFontBold(false);
            listData.Add(CreateTitle("Perintah Lalulintas Jalan", 200, lastPositionY, Text28));
            lastPositionY += position25;
            listData.Add(CreateTitle("(Peruntukan Tempat Letak Kereta)", 180, lastPositionY, Text28));
            lastPositionY += position25;
            listData.Add(CreateTitle("Majlis Perbandaran Johor Bahru Tengah 2003", 150, lastPositionY, Text28));
            lastPositionY += position25;
            listData.Add(CreateTitle("(Perintah 39)", 240, lastPositionY, Text24));
            lastPositionY += addLine;
            listData.Add(CreateTitle("NOTIS KESALAHAN SERTA TAWARAN KOMPAUN", 50, lastPositionY, Text28));

            var bitmap = PrepareBitmap(lastPositionY);
            var canvas = CreateCanvas(bitmap);
            DrawText(canvas, listData);

            CreateFileBitmap(bitmap, "SampleBitmap");
            return bitmap;
        }
        public Bitmap CreateKompaunType1BitmapOld(Context context, int logo, int jompay, int Asign, CompoundDto compoundDto, OffendDto offendDto, ActDto actDto, EnforcerDto enforcerDto)
        {
            int nLine = 0, i = 0;
            string saksiName = "", scanDateTime = "";

            string formatPrintAmtWord = SetAmount2Word(compoundDto.Compound1Type.CompAmt);
            string formatPrintAmt = FormatPrintAmount(compoundDto.Compound1Type.CompAmt);
            string formatPrintAmt2 = FormatPrintAmount(compoundDto.Compound1Type.CompAmt2);
            string formatPrintAmt3 = FormatPrintAmount(compoundDto.Compound1Type.CompAmt3);
            string formatPrintDate = GeneralBll.FormatPrintDate(compoundDto.CompDate);
            string formatPrintTime = GeneralBll.FormatPrintTime(compoundDto.CompTime);
            string amtkmp1 = FormatPrintAmount1(compoundDto.Compound1Type.CompAmt);
            string amtkmp2 = FormatPrintAmount1(compoundDto.Compound1Type.CompAmt2);
            string formatRayuanAmt = FormatPrintAmount1(offendDto.OffendAmt);
            string formatRayuanTandaAmt = FormatPrintAmount1(offendDto.OffendAmt);
            string formatTempohDate = AddDate(compoundDto.CompDate, 13);
            string formatTempohDate3Days = AddDate(compoundDto.CompDate, 2);
            string formatNotaTempohDate = GeneralBll.FormatPrintDate(compoundDto.TempohDate);

            var configDto = GeneralBll.GetConfig();
            if (configDto == null)
            {
                LogFile.WriteLogFile("Get Config null", Enums.LogType.Error);
            }

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

            string strKodHasil = offendDto.IncomeCode.PadRight(8);
            string strNoKmp = compoundDto.CompNum;

            int positionX = 10;
            int position25 = 25;
            int addLine = 40;

            var listData = new List<PrintImageDto> { };
            int lastPositionY = 10;
            listData.Add(CreateLogoHeader(context, logo, positionX + 250, lastPositionY, positionX + 550, lastPositionY + 200, Paint.Align.Center));

            lastPositionY += addLine;
            lastPositionY += addLine;
            lastPositionY += addLine;
            lastPositionY += addLine;
            lastPositionY += addLine;
            SetFontBold(true);
            listData.Add(CreateTitle("MAJLIS BANDARAYA ISKANDAR PUTERI", 120, lastPositionY));
            lastPositionY += addLine;
            SetFontBold(false);
            listData.Add(CreateTitle("Perintah Lalulintas Jalan", 150, lastPositionY, Text24));
            lastPositionY += position25;
            listData.Add(CreateTitle("(Peruntukan Tempat Letak Kereta)", 130, lastPositionY, Text24));
            lastPositionY += position25;
            listData.Add(CreateTitle("Majlis Perbandaran Johor Bahru Tengah 2023", 100, lastPositionY, Text24));
            lastPositionY += position25;
            listData.Add(CreateTitle("(Perintah 39)", 190, lastPositionY, Text22));
            lastPositionY += addLine;
            listData.Add(CreateTitle("NOTIS KESALAHAN SERTA TAWARAN KOMPAUN", 100, lastPositionY));

            lastPositionY += addLine;
            listData.Add(CreateBarcodeImage(460, lastPositionY, strNoKmp, 300, BarcodeAlign.Left));
            lastPositionY += addLine;

            SetFontSize(22);
            lastPositionY += addLine + positionX;
            SetFontBold(false);
            listData.Add(CreateText("NO", 500, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(":", 520, lastPositionY));
            listData.Add(CreateText(strNoKmp, 540, lastPositionY));
            SetFontBold(false);


            lastPositionY += addLine;
            listData.Add(CreateText("Kepada Pemandu/Pemunya", positionX, lastPositionY));

            lastPositionY += addLine;
            listData.Add(CreateText("No Kenderaan", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(compoundDto.Compound1Type.CarNum.PadRight(15), 200, lastPositionY));
            SetFontBold(false);

            lastPositionY += addLine;
            listData.Add(CreateText("Jenama/Model", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(compoundDto.Compound1Type.CarTypeDesc, 200, lastPositionY));
            SetFontBold(false);

            listData.Add(CreateText("Jenis", 550, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 650, lastPositionY));
            listData.Add(CreateText(category.ShortDesc, 670, lastPositionY));
            SetFontBold(false);

            lastPositionY += addLine;
            listData.Add(CreateText("Warna", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(compoundDto.Compound1Type.CarColorDesc, 200, lastPositionY));
            SetFontBold(false);

            listData.Add(CreateText("No. Petak ", 550, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 650, lastPositionY));
            listData.Add(CreateText(compoundDto.Compound1Type.LotNo, 670, lastPositionY));
            SetFontBold(false);

            string tempatjadi;
            tempatjadi = compoundDto.StreetDesc + "," + compoundDto.ZoneDesc;

            var listStringStreet = SeparateText(tempatjadi, 6, 50);

            lastPositionY += addLine;
            listData.Add(CreateText("Jalan/Tempat", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(listStringStreet[0], 200, lastPositionY));
            if (listStringStreet[1].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringStreet[1], 200, lastPositionY));
            }
            if (listStringStreet[2].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringStreet[2], 200, lastPositionY));
            }
            if (listStringStreet[3].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringStreet[3], 200, lastPositionY));
            }
            if (listStringStreet[4].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringStreet[4], 200, lastPositionY));
            }

            lastPositionY += addLine;
            listData.Add(CreateText("Tarikh & Waktu", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(formatPrintDate + " " + formatPrintTime, 200, lastPositionY));
            SetFontBold(false);

            scanDateTime = compoundDto.Compound1Type.ScanDateTime;
            if (scanDateTime.TrimEnd().Length > 10 && scanDateTime.TrimEnd() != "ERROR")
            {
                lastPositionY += addLine;
                listData.Add(CreateText("TARIKH DAN MASA IMBAS SMART PARKING ", positionX, lastPositionY));
                SetFontBold(true);
                listData.Add(CreateText(": ", 450, lastPositionY));
                listData.Add(CreateText(GeneralBll.ConvertStringyyyymmddToddmmyyyyhhmmss(compoundDto.Compound1Type.ScanDateTime), 470, lastPositionY));
                SetFontBold(false);
            }
            else
            {
                if (scanDateTime.TrimEnd() == "ERROR")
                {
                    lastPositionY += addLine;
                    SetFontBold(true);
                    listData.Add(CreateText("TAK DAPAT IMBAS SMART PARKING ", positionX, lastPositionY));
                    SetFontBold(false);
                }
            }

            SetFontSize(20);
            lastPositionY += addLine;
            listData.Add(CreateText("Dengan ini diberitahu kenderaan bermotor tuan/puan telah dilaporkan dengan kesalahan", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("seperti yang berikut:", positionX, lastPositionY));

            SetFontSize(20);
            var listString1 = SeparateText(actDto.LongDesc, 5, 55);
            lastPositionY += addLine;
            listData.Add(CreateText("Peruntukan", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(listString1[0], 200, lastPositionY));
            SetFontBold(false);
            lastPositionY += position25;
            listData.Add(CreateText("Undang-undang", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(listString1[1], 200, lastPositionY));

            if (listString1[2].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listString1[2], 200, lastPositionY));
            }
            if (listString1[3].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listString1[3], 200, lastPositionY));
            }

            SetFontBold(false);
            lastPositionY += addLine;
            listData.Add(CreateText("Perintah ", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(offendDto.PrnDesc, 200, lastPositionY));

            lastPositionY += addLine;
            listData.Add(CreateText("Kesalahan", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));

            string section = offendDto.LongDesc;
            var listStringSection = SeparateText(section, 5, 55);
            SetFontBold(true);
            listData.Add(CreateText(listStringSection[0], 200, lastPositionY));
            if (listStringSection[1].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringSection[1], 200, lastPositionY));
            }
            if (listStringSection[2].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringSection[2], 200, lastPositionY));
            }
            if (listStringSection[3].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringSection[3], 200, lastPositionY));
            }

            SetFontBold(false);
            lastPositionY += addLine;
            listData.Add(CreateText("Jumlah Kompaun", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(formatPrintAmt, 200, lastPositionY));


            SetFontBold(false);
            lastPositionY += addLine;
            listData.Add(CreateText("Dikeluarkan Oleh", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(enforcerDto.EnforcerId, 200, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(enforcerDto.Jabatan, 200, lastPositionY));

            SetFontBold(false);
            lastPositionY += addLine;
            listData.Add(CreateText("Tarikh", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(formatPrintDate, 200, lastPositionY));

            SetFontSize(21);
            lastPositionY += addLine;
            string string1 = "";
            string1 = "PADA  menjalankan  kuasa-kuasa  yang telah  diberikan di bawah seksyen 120(1)(e)";
            string1 += "Akta Pengangkutan Jalan 1987 {Akta 333], dan Perintah 39(1) dan (2) saya dengan";
            string1 += "ini menawar untuk mengkompaun kesalahan ini dengan bayaran RM ............";
            string1 += ". Jika tuan/puan menerima tawaran ini, sila jelaskan bayaran tersebut di ";
            string1 += "IBU PEJABAT MAJLIS BANDARAYA ISKANDAR PUTERI(MBIP), WISMA SKUDAI, CAWANGAN PERLING, UTC GALERIA(KOTA RAYA) ";
            string1 += "DAN ATAS TALIAN APLIKASI e-KHIDMAT, PBTPAY, APLIKASI PARKMAX@JOHOR";
            SetFontBold(true);
            lastPositionY += position25;

            listData.Add(CreateText("PADA menjalankan  kuasa-kuasa yang telah  diberikan di  bawah  seksyen 120(1)(e)", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("Akta Pengangkutan Jalan 1987 {Akta 333], dan  Perintah 39(1) dan (2) saya dengan", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("ini  menawar untuk  mengkompaun  kesalahan  ini dengan bayaran RM ............ Jika", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("tuan/puan  menerima  tawaran ini, sila  jelaskan  bayaran  tersebut di IBU  PEJABAT", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("MAJLIS  BANDARAYA   ISKANDAR   PUTERI(MBIP),   WISMA  SKUDAI,  CAWANGAN", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("PERLING,  UTC GALERIA(KOTA RAYA)  DAN   ATAS  TALIAN   e-KHIDMAT,  PBTPAY,", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("APLIKASI PARKMAX@JOHOR", positionX, lastPositionY));
            lastPositionY += position25;

            lastPositionY += addLine;
            listData.Add(CreateText("2.  Tawaran  mengkompaun  kesalahan  ini  berkuatkuasa  selama  14  hari  sahaja", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("daripada   tarikh   notis   ini  dikeluarkan   dan  jika  jawapan   tidak  diterima  dalam", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("tempoh, itu tindakan mahkamah akan diambil terhadap tuan/puan tanpa notis lagi.", positionX, lastPositionY));
            lastPositionY += position25;
            lastPositionY += addLine;
            lastPositionY += addLine;

            listData.Add(CreateRectangle(400, lastPositionY + 5, lastPositionY + 130, 800, false));
            SetFontSize(20);
            SetFontBold(true);
            listData.Add(CreateText("DISKAUN TERHADAP BAYARAN MENGIKUT TEMPOH HARI", positionX + 400, lastPositionY + addLine));
            listData.Add(CreateText("             1 - 14 HARI DISKAUN 50%         ", positionX + 400, lastPositionY + addLine + position25));
            listData.Add(CreateText("            15 - 30 HARI DISKAUN 30%         ", positionX + 400, lastPositionY + addLine + position25 + position25));

            lastPositionY += addLine;
            lastPositionY += addLine;
            listData.Add(CreateText("...................................................................", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("DATUK BANDAR", positionX + 120, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("MAJLIS BANDARAYA ISKANDAR PUTERI", positionX, lastPositionY));

            lastPositionY += addLine;
            lastPositionY += addLine;
            listData.Add(CreateLine(positionX, lastPositionY, DefaultWidth, lastPositionY));

            lastPositionY += addLine;
            lastPositionY += addLine;
            lastPositionY += addLine;

            listData.Add(CreateLogoHeader(context, logo, positionX, lastPositionY - addLine, positionX + 130, lastPositionY + 80, Paint.Align.Left));

            SetFontBold(true);
            SetFontSize(20);
            listData.Add(CreateTitle("MAJLIS BANDARAYA ISKANDAR PUTERI", 250, lastPositionY, Text22));
            lastPositionY += position25;
            listData.Add(CreateTitle("JABATAN PENGUATKUASA", 250, lastPositionY, Text22));
            string strURLImage = configDto.UrlImage + "council=MBIP&compound=" + compoundDto.CompNum;
            listData.Add(CreateBarcodeImageQRCode(650, lastPositionY - addLine - 10, strURLImage, 650, BarcodeAlign.Left));
            lastPositionY += addLine;
            lastPositionY += addLine;
            lastPositionY += addLine;

            SetFontSize(20);
            listData.Add(CreateText("No Kenderaan", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(compoundDto.Compound1Type.CarNum.PadRight(15), 200, lastPositionY));
            SetFontBold(false);

            SetFontSize(20);
            lastPositionY += addLine;
            listData.Add(CreateText("Peruntukan", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(listString1[0], 200, lastPositionY));
            SetFontBold(false);
            lastPositionY += position25;
            listData.Add(CreateText("Undang-undang", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(listString1[1], 200, lastPositionY));

            if (listString1[2].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listString1[2], 200, lastPositionY));
            }
            if (listString1[3].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listString1[3], 200, lastPositionY));
            }

            SetFontBold(false);
            lastPositionY += addLine;
            listData.Add(CreateText("Perintah ", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(offendDto.PrnDesc, 200, lastPositionY));

            lastPositionY += addLine;
            listData.Add(CreateText("Tarikh & Waktu", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(formatPrintDate + " " + formatPrintTime, 200, lastPositionY));
            SetFontBold(true);
            lastPositionY += addLine;
            listData.Add(CreateText("KERATAN UNTUK CATATAN PEMBAYARAN", 220, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("TERIMA KASIH", 350, lastPositionY));
            SetFontBold(false);
            lastPositionY += position25;
            listData.Add(CreateText("KPD691184", 350, lastPositionY));


            for (i = 0; i < 6; i++)
            {
                lastPositionY += 20;
                listData.Add(CreateText("", positionX, lastPositionY));
            }

            var bitmap = PrepareBitmap(lastPositionY);
            var canvas = CreateCanvas(bitmap);
            DrawText(canvas, listData);

            CreateFileBitmap(bitmap, "SampleBitmap");
            return bitmap;
        }

        public Bitmap CreateKompaunType2Bitmap_1(Context context, int logo, int jompay, int Asign, CompoundDto compoundDto, OffendDto offendDto, ActDto actDto, EnforcerDto enforcerDto)
        {
            int nLine = 0, i = 0;
            string saksiName = "", scanDateTime = "", zoneName = "";

            string formatPrintAmtWord = SetAmount2Word(compoundDto.Compound2Type.CompAmt);
            string formatPrintAmt = FormatPrintAmount(compoundDto.Compound2Type.CompAmt);
            string formatPrintAmt2 = FormatPrintAmount(compoundDto.Compound2Type.CompAmt2);
            string formatPrintAmt3 = FormatPrintAmount(compoundDto.Compound2Type.CompAmt3);
            string formatPrintDate = GeneralBll.FormatPrintDate(compoundDto.CompDate);
            string formatPrintTime = GeneralBll.FormatPrintTime(compoundDto.CompTime);
            string amtkmp1 = FormatPrintAmount1(compoundDto.Compound2Type.CompAmt);
            string amtkmp2 = FormatPrintAmount1(compoundDto.Compound2Type.CompAmt2);
            string formatRayuanAmt = FormatPrintAmount1(offendDto.OffendAmt);
            string formatRayuanTandaAmt = FormatPrintAmount1(offendDto.OffendAmt);
            string formatTempohDate = AddDate(compoundDto.CompDate, 13);
            string formatTempohDate3Days = AddDate(compoundDto.CompDate, 2);
            string formatNotaTempohDate = GeneralBll.FormatPrintDate(compoundDto.TempohDate);

            var configDto = GeneralBll.GetConfig();
            if (configDto == null)
            {
                LogFile.WriteLogFile("Get Config null", Enums.LogType.Error);
            }

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

            var zone = TableFilBll.GetZoneByCodeAndMukim(compoundDto.Zone, compoundDto.Mukim);
            if (zone == null)
                zoneName = " ";
            else
                zoneName = zone.LongDesc;

            string strKodHasil = offendDto.IncomeCode.PadRight(8);
            string strNoKmp = compoundDto.CompNum;

            int positionX = 10;
            int position25 = 25;
            int addLine = 40;

            var listData = new List<PrintImageDto> { };
            int lastPositionY = 10;
            listData.Add(CreateLogoHeader(context, logo, positionX + 250, lastPositionY, positionX + 550, lastPositionY + 200, Paint.Align.Center));

            lastPositionY += addLine;
            lastPositionY += addLine;
            lastPositionY += addLine;
            lastPositionY += addLine;
            lastPositionY += addLine;
            SetFontBold(true);
            listData.Add(CreateTitle("MAJLIS BANDARAYA ISKANDAR PUTERI", 120, lastPositionY));
            lastPositionY += addLine;
            SetFontBold(false);
            listData.Add(CreateTitle("Perintah Lalulintas Jalan", 200, lastPositionY, Text28));
            lastPositionY += position25;
            listData.Add(CreateTitle("(Peruntukan Tempat Letak Kereta)", 180, lastPositionY, Text28));
            lastPositionY += position25;
            listData.Add(CreateTitle("Majlis Perbandaran Johor Bahru Tengah 2003", 150, lastPositionY, Text28));
            lastPositionY += position25;
            listData.Add(CreateTitle("(Perintah 39)", 240, lastPositionY, Text24));
            lastPositionY += addLine;
            listData.Add(CreateTitle("NOTIS KESALAHAN SERTA TAWARAN KOMPAUN", 50, lastPositionY, Text28));

            lastPositionY += addLine;
            listData.Add(CreateBarcodeImage(460, lastPositionY, strNoKmp, 300, BarcodeAlign.Left));
            lastPositionY += addLine;

            SetFontSize(22);
            lastPositionY += addLine + positionX;
            SetFontBold(false);
            listData.Add(CreateText("NO", 500, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(":", 520, lastPositionY));
            listData.Add(CreateText(strNoKmp, 540, lastPositionY));
            SetFontBold(false);


            lastPositionY += addLine;
            listData.Add(CreateText("Kepada Pemandu/Pemunya", positionX, lastPositionY));

            lastPositionY += addLine;
            listData.Add(CreateText("No Kenderaan", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(compoundDto.Compound2Type.CarNum.PadRight(15), 200, lastPositionY));
            SetFontBold(false);

            listData.Add(CreateText("Warna", 500, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 600, lastPositionY));
            listData.Add(CreateText(compoundDto.Compound2Type.CarColorDesc, 620, lastPositionY));
            SetFontBold(false);

            lastPositionY += addLine;
            listData.Add(CreateText("Jenama/Model", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(compoundDto.Compound2Type.CarTypeDesc, 200, lastPositionY));
            SetFontBold(false);

            lastPositionY += addLine;
            listData.Add(CreateText("Jenis", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(category.ShortDesc, 200, lastPositionY));
            SetFontBold(false);

            lastPositionY += addLine;
            SetFontBold(false);
            listData.Add(CreateText("Tarikh ", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(formatPrintDate, 200, lastPositionY));
            SetFontBold(false);

            listData.Add(CreateText("Waktu", 500, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 600, lastPositionY));
            listData.Add(CreateText(formatPrintTime, 620, lastPositionY));
            SetFontBold(false);

            lastPositionY += addLine;
            SetFontBold(false);
            listData.Add(CreateText("Cara Serah ", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(delivery.ShortDesc, 200, lastPositionY));
            SetFontBold(false);

            string tempatjadi;
            tempatjadi = compoundDto.StreetDesc + "," + zoneName;

            var listStringStreet = SeparateText(tempatjadi, 6, 50);

            lastPositionY += addLine;
            listData.Add(CreateText("Tempat/Jalan", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(listStringStreet[0], 200, lastPositionY));
            if (listStringStreet[1].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringStreet[1], 200, lastPositionY));
            }
            if (listStringStreet[2].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringStreet[2], 200, lastPositionY));
            }
            if (listStringStreet[3].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringStreet[3], 200, lastPositionY));
            }
            if (listStringStreet[4].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringStreet[4], 200, lastPositionY));
            }


            SetFontSize(22);
            lastPositionY += addLine;
            listData.Add(CreateText("Dengan ini diberitahu kenderaan bermotor tuan/puan telah dilaporkan dengan kesalahan", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("seperti yang berikut:", positionX, lastPositionY));

            SetFontSize(22);
            var listString1 = SeparateText(actDto.LongDesc, 5, 45);
            lastPositionY += addLine;
            listData.Add(CreateText("Peruntukan", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(listString1[0], 200, lastPositionY));
            SetFontBold(false);
            lastPositionY += position25;
            listData.Add(CreateText("Undang-undang", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(listString1[1], 200, lastPositionY));

            if (listString1[2].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listString1[2], 200, lastPositionY));
            }
            if (listString1[3].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listString1[3], 200, lastPositionY));
            }

            SetFontBold(false);
            lastPositionY += addLine;
            listData.Add(CreateText("Perintah ", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(offendDto.PrnDesc, 200, lastPositionY));

            lastPositionY += addLine;
            listData.Add(CreateText("Kesalahan", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));

            string section = offendDto.LongDesc;
            var listStringSection = SeparateText(section, 5, 40);
            SetFontBold(true);
            listData.Add(CreateText(listStringSection[0], 200, lastPositionY));
            if (listStringSection[1].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringSection[1], 200, lastPositionY));
            }
            if (listStringSection[2].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringSection[2], 200, lastPositionY));
            }
            if (listStringSection[3].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringSection[3], 200, lastPositionY));
            }

            SetFontBold(false);
            lastPositionY += addLine;
            listData.Add(CreateText("Jumlah Kompaun", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(formatPrintAmt, 200, lastPositionY));


            SetFontBold(false);
            lastPositionY += addLine;
            listData.Add(CreateText("Dikeluarkan Oleh", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(enforcerDto.EnforcerId + "(" + enforcerDto.EnforcerName + ")", 200, lastPositionY));
            SetFontBold(true);
            lastPositionY += position25;
            listData.Add(CreateText(enforcerDto.Jabatan, 200, lastPositionY));

            SetFontBold(false);
            lastPositionY += addLine;
            listData.Add(CreateText("Tarikh", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 180, lastPositionY));
            listData.Add(CreateText(formatPrintDate, 200, lastPositionY));

            SetFontSize(21);
            lastPositionY += addLine;
            lastPositionY += addLine;
            lastPositionY += addLine;
            lastPositionY += addLine;
            var bitmapASign = BitmapFactory.DecodeResource(context.Resources, Asign);
            listData.Add(new PrintImageDto
            {
                IsLogo = true,
                SignKompaun = true,
                Bitmap = bitmapASign,
                Alignment = Paint.Align.Left,
                IsJustified = false,
                PositionX = 450,
                PositionY = lastPositionY - 10,
                Width = 250
            });



            listData.Add(CreateText("..........................................................................", 400, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("DATUK BANDAR", 520, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("MAJLIS BANDARAYA ISKANDAR PUTERI", 400, lastPositionY));

            var bitmap = PrepareBitmap(lastPositionY);
            var canvas = CreateCanvas(bitmap);
            DrawText(canvas, listData);

            CreateFileBitmap(bitmap, "SampleBitmap");
            return bitmap;
        }

        public Bitmap CreateKompaunType1Bitmap_2(Context context, int logo, int jompay, int Asign, CompoundDto compoundDto, OffendDto offendDto, ActDto actDto, EnforcerDto enforcerDto)
        {
            int nLine = 0, i = 0;
            string saksiName = "", scanDateTime = "";

            string formatPrintAmtWord = SetAmount2Word(compoundDto.Compound1Type.CompAmt);
            string formatPrintAmt = FormatPrintAmount(compoundDto.Compound1Type.CompAmt);
            string formatPrintAmt2 = FormatPrintAmount(compoundDto.Compound1Type.CompAmt2);
            string formatPrintAmt3 = FormatPrintAmount(compoundDto.Compound1Type.CompAmt3);
            string formatPrintDate = GeneralBll.FormatPrintDate(compoundDto.CompDate);
            string formatPrintTime = GeneralBll.FormatPrintTime(compoundDto.CompTime);
            string amtkmp1 = FormatPrintAmount1(compoundDto.Compound1Type.CompAmt);
            string amtkmp2 = FormatPrintAmount1(compoundDto.Compound1Type.CompAmt2);
            string formatRayuanAmt = FormatPrintAmount1(offendDto.OffendAmt);
            string formatRayuanTandaAmt = FormatPrintAmount1(offendDto.OffendAmt);
            string formatTempohDate = AddDate(compoundDto.CompDate, 13);
            string formatTempohDate3Days = AddDate(compoundDto.CompDate, 2);
            string formatNotaTempohDate = GeneralBll.FormatPrintDate(compoundDto.TempohDate);

            var configDto = GeneralBll.GetConfig();
            if (configDto == null)
            {
                LogFile.WriteLogFile("Get Config null", Enums.LogType.Error);
            }

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

            string strKodHasil = offendDto.IncomeCode.PadRight(8);
            string strNoKmp = compoundDto.CompNum;

            int positionX = 10;
            int position25 = 25;
            int addLine = 40;

            var listData = new List<PrintImageDto> { };
            int lastPositionY = 10;

            lastPositionY += addLine;
            SetFontBold(true);
            lastPositionY += position25;
            SetFontSize(22);

            listData.Add(CreateText("PADA menjalankan  kuasa-kuasa yang telah  diberikan di  bawah  seksyen 120(1)(e)", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("Akta Pengangkutan Jalan 1987 [Akta 333], dan  Perintah 39(1) dan (2) saya dengan", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("ini  menawar untuk  mengkompaun  kesalahan  ini dengan bayaran " + formatPrintAmt + " Jika", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("tuan/puan  menerima  tawaran ini, sila  jelaskan  bayaran  tersebut SECARA DALAM ", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("APLIKASI PARKMAX@JOHOR", positionX, lastPositionY));
            lastPositionY += position25;

            lastPositionY += addLine;
            listData.Add(CreateText("2.  Tawaran  mengkompaun  kesalahan ini  berkuat kuasa  selama  14  hari  sahaja", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("dari   tarikh   notis   ini   dikeluarkan   dan   jika  jawapan   tidak  diterima  dalam", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("tempoh, itu tindakan mahkamah akan diambil terhadap tuan/puan tanpa notis lagi.", positionX, lastPositionY));
            lastPositionY += position25;
            lastPositionY += addLine;

            string strURLImage = configDto.UrlImage + "council=MBIP&compound=" + compoundDto.CompNum;
            listData.Add(CreateBarcodeImageQRCode(10, lastPositionY + 5, strURLImage, 650, BarcodeAlign.Left));

            listData.Add(CreateRectangle(220, lastPositionY + 5, lastPositionY + 130, 800, false));
            SetFontSize(20);
            SetFontBold(true);
            listData.Add(CreateText("DISKAUN TERHADAP BAYARAN MENGIKUT TEMPOH HARI", positionX + 220, lastPositionY + addLine));
            listData.Add(CreateText("             1 - 14 HARI DISKAUN 50%         ", positionX + 330, lastPositionY + addLine + position25));
            listData.Add(CreateText("            15 - 30 HARI DISKAUN 30%         ", positionX + 330, lastPositionY + addLine + position25 + position25));

            var bitmapASign = BitmapFactory.DecodeResource(context.Resources, Asign);
            listData.Add(new PrintImageDto
            {
                IsLogo = true,
                SignKompaun = true,
                Bitmap = bitmapASign,
                Alignment = Paint.Align.Left,
                IsJustified = false,
                PositionX = 500,
                PositionY = lastPositionY + 250,
                Width = 250
            });

            lastPositionY += addLine;
            lastPositionY += addLine;
            lastPositionY += addLine;
            lastPositionY += addLine;
            lastPositionY += addLine;
            lastPositionY += addLine;
            lastPositionY += addLine;
            listData.Add(CreateText("...........................................................................", 400, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("DATUK BANDAR", 520, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("MAJLIS BANDARAYA ISKANDAR PUTERI", 400, lastPositionY));

            //lastPositionY += addLine;
            //lastPositionY += addLine;
            //listData.Add(CreateLine(positionX, lastPositionY, DefaultWidth, lastPositionY));

            lastPositionY += addLine;
            lastPositionY += addLine;
            lastPositionY += addLine;

            //listData.Add(CreateLogoHeader(context, logo, positionX, lastPositionY- addLine, positionX + 130, lastPositionY + 80, Paint.Align.Left));

            //SetFontBold(true);
            //SetFontSize(21);
            //listData.Add(CreateTitle("MAJLIS BANDARAYA ISKANDAR PUTERI", 250, lastPositionY+10,Text22));
            //lastPositionY += position25;
            //listData.Add(CreateTitle("JABATAN PENGUATKUASA", 250, lastPositionY+10, Text22));
            //string strURLImage = configDto.UrlImage + "council=MBIP&compound=" + compoundDto.CompNum;
            //listData.Add(CreateBarcodeImageQRCode(650, lastPositionY- addLine - 10, strURLImage, 650, BarcodeAlign.Left));
            //lastPositionY += addLine;
            //lastPositionY += addLine;
            //lastPositionY += addLine;

            //SetFontSize(20);
            //listData.Add(CreateText("No Kenderaan", positionX, lastPositionY));
            //SetFontBold(true);
            //listData.Add(CreateText(": ", 180, lastPositionY));
            //listData.Add(CreateText(compoundDto.Compound1Type.CarNum.PadRight(15), 200, lastPositionY));
            //SetFontBold(false);

            //var listString1 = SeparateText(actDto.LongDesc, 5, 50);
            //SetFontSize(20);
            //lastPositionY += addLine;
            //listData.Add(CreateText("Peruntukan", positionX, lastPositionY));
            //SetFontBold(true);
            //listData.Add(CreateText(": ", 180, lastPositionY));
            //listData.Add(CreateText(listString1[0], 200, lastPositionY));
            //SetFontBold(false);
            //lastPositionY += position25;
            //listData.Add(CreateText("Undang-undang", positionX, lastPositionY));
            //SetFontBold(true);
            //listData.Add(CreateText(listString1[1], 200, lastPositionY));

            //if (listString1[2].TrimEnd() != "")
            //{
            //    lastPositionY += position25;
            //    listData.Add(CreateText(listString1[2], 200, lastPositionY));
            //}
            //if (listString1[3].TrimEnd() != "")
            //{
            //    lastPositionY += position25;
            //    listData.Add(CreateText(listString1[3], 200, lastPositionY));
            //}

            //SetFontBold(false);
            //lastPositionY += addLine;
            //listData.Add(CreateText("Perintah ", positionX, lastPositionY));
            //SetFontBold(true);
            //listData.Add(CreateText(": ", 180, lastPositionY));
            //listData.Add(CreateText(offendDto.PrnDesc, 200, lastPositionY));

            //lastPositionY += addLine;
            //listData.Add(CreateText("Tarikh & Waktu", positionX, lastPositionY));
            //SetFontBold(true);
            //listData.Add(CreateText(": ", 180, lastPositionY));
            //listData.Add(CreateText(formatPrintDate + " " + formatPrintTime, 200, lastPositionY));
            //SetFontBold(true);
            //lastPositionY += addLine;
            //listData.Add(CreateText("KERATAN UNTUK CATATAN PEMBAYARAN", 220, lastPositionY));
            //lastPositionY += position25;
            //listData.Add(CreateText("TERIMA KASIH", 350, lastPositionY));
            //SetFontBold(false);
            //lastPositionY += position25;
            //listData.Add(CreateText("KPD691184", 350, lastPositionY));


            for (i = 0; i < 6; i++)
            {
                lastPositionY += 20;
                listData.Add(CreateText("", positionX, lastPositionY));
            }

            var bitmap = PrepareBitmap(lastPositionY);
            var canvas = CreateCanvas(bitmap);
            DrawText(canvas, listData);

            CreateFileBitmap(bitmap, "SampleBitmap");
            return bitmap;
        }

        public Bitmap CreateKompaunType2Bitmap(Context context, int logo, int jompay, int Asign, CompoundDto compoundDto, OffendDto offendDto, ActDto actDto, EnforcerDto enforcerDto)
        {
            int nLine = 0, i = 0;
            string saksiName = "", scanDateTime = "";

            string formatPrintAmtWord = SetAmount2Word(compoundDto.Compound2Type.CompAmt);
            string formatPrintAmt = FormatPrintAmount(compoundDto.Compound2Type.CompAmt);
            string formatPrintAmt2 = FormatPrintAmount(compoundDto.Compound2Type.CompAmt2);
            string formatPrintAmt3 = FormatPrintAmount(compoundDto.Compound2Type.CompAmt3);
            string formatPrintDate = GeneralBll.FormatPrintDate(compoundDto.CompDate);
            string formatPrintTime = GeneralBll.FormatPrintTime(compoundDto.CompTime);
            string amtkmp1 = FormatPrintAmount1(compoundDto.Compound2Type.CompAmt);
            string formatRayuanAmt = FormatPrintAmount1(offendDto.OffendAmt);

            string formatTempohDate = AddDate(compoundDto.CompDate, 13);
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

            string strKodHasil = offendDto.IncomeCode.PadRight(8);
            string strNoKmp = compoundDto.CompNum;

            int positionX = 10;
            int position25 = 25;
            int addLine = 40;

            var listData = new List<PrintImageDto> { };
            //var listData = new List<PrintImageDto>
            //{
            //    CreateLogoHeader(context, logo)
            //};

            //int lastPositionY = GetLastPositionY(listData);
            //lastPositionY += 150;
            int lastPositionY = 25;
            SetFontBold(true);
            listData.Add(CreateTitle("MAJLIS BANDARAYA PASIR GUDANG", 120, lastPositionY));
            lastPositionY += addLine;
            SetFontBold(false);
            listData.Add(CreateTitle("NOTIS KESALAHAN SERTA TAWARAN KOMPAUN", 10, lastPositionY));

            SetFontSize(20);
            lastPositionY += addLine + positionX;
            listData.Add(CreateText("KOD KAUNTER ", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 220, lastPositionY));
            listData.Add(CreateText(strKodHasil, 240, lastPositionY));
            SetFontBold(false);
            listData.Add(CreateText("NO KOMPAUN", 460, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(":", 590, lastPositionY));
            listData.Add(CreateText(strNoKmp, 600, lastPositionY));
            SetFontBold(false);

            lastPositionY += positionX;
            listData.Add(CreateLine(positionX, lastPositionY, DefaultWidth, lastPositionY));

            lastPositionY += addLine;
            listData.Add(CreateText("NO KENDERAAN", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 220, lastPositionY));
            listData.Add(CreateText(compoundDto.Compound2Type.CarNum.PadRight(15), 240, lastPositionY));
            SetFontBold(false);

            lastPositionY += addLine;
            listData.Add(CreateText("NO CUKAI JALAN", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 220, lastPositionY));
            listData.Add(CreateText(compoundDto.Compound2Type.RoadTax, 240, lastPositionY));
            SetFontBold(false);

            lastPositionY += addLine;
            listData.Add(CreateText("JENAMA/MODEL", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 220, lastPositionY));
            listData.Add(CreateText(compoundDto.Compound2Type.CarTypeDesc, 240, lastPositionY));
            SetFontBold(false);

            lastPositionY += addLine;
            listData.Add(CreateText("JENIS KENDERAAN", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 220, lastPositionY));
            listData.Add(CreateText(category.ShortDesc, 240, lastPositionY));
            SetFontBold(false);

            lastPositionY += addLine;
            listData.Add(CreateText("TARIKH & WAKTU", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 220, lastPositionY));
            listData.Add(CreateText(formatPrintDate + " " + formatPrintTime, 240, lastPositionY));
            SetFontBold(false);

            string tempatjadi;
            tempatjadi = compoundDto.StreetDesc;

            var listStringStreet = SeparateText(tempatjadi, 6, 50);

            lastPositionY += addLine;
            listData.Add(CreateText("TEMPAT KESALAHAN", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 220, lastPositionY));
            listData.Add(CreateText(listStringStreet[0], 240, lastPositionY));
            if (listStringStreet[1].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringStreet[1], 240, lastPositionY));
            }
            if (listStringStreet[2].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringStreet[2], 240, lastPositionY));
            }

            lastPositionY += addLine;
            listData.Add(CreateText("CARA PENYERAHAN", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 220, lastPositionY));
            listData.Add(CreateText(delivery.ShortDesc, 240, lastPositionY));
            SetFontBold(false);

            SetFontSize(18);
            lastPositionY += addLine;
            listData.Add(CreateText("KEPADA PEMUNYA/PEMANDU KENDERAAN TERSEBUT DI ATAS, TUAN/PUAN DIDAPATI", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("MELAKUKAN KESALAHAN SEPERTI BERIKUT :-", positionX, lastPositionY));

            SetFontSize(20);
            var listString1 = SeparateText(compoundDto.Compound2Type.CompDesc, 15, 45);
            lastPositionY += addLine;
            listData.Add(CreateText("BUTIR-BUTIR", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 220, lastPositionY));
            listData.Add(CreateText(listString1[0], 240, lastPositionY));
            SetFontBold(false);
            lastPositionY += position25;
            listData.Add(CreateText("KESALAHAN", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(listString1[1], 240, lastPositionY));

            if (listString1[2].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listString1[2], 240, lastPositionY));
            }
            if (listString1[3].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listString1[3], 240, lastPositionY));
            }
            if (listString1[4].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listString1[4], 240, lastPositionY));
            }
            if (listString1[5].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listString1[5], 240, lastPositionY));
            }

            SetFontBold(false);
            lastPositionY += addLine;
            listData.Add(CreateText("PERUNTUKAN UNDANG-UNDANG :-", positionX, lastPositionY));

            string section = offendDto.PrnDesc + "," + actDto.LongDesc;
            var listStringSection = SeparateText(section, 5, 70);
            SetFontBold(true);
            lastPositionY += position25;
            listData.Add(CreateText(listStringSection[0], positionX, lastPositionY));
            if (listStringSection[1].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringSection[1], positionX, lastPositionY));
            }
            if (listStringSection[2].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringSection[2], positionX, lastPositionY));
            }
            if (listStringSection[3].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringSection[3], positionX, lastPositionY));
            }

            var listString2 = SeparateText(enforcerDto.EnforcerId + "(" + enforcerDto.EnforcerName + ")", 2, 55);
            SetFontBold(false);
            lastPositionY += addLine;
            listData.Add(CreateText("DIKELUARKAN OLEH", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 220, lastPositionY));
            listData.Add(CreateText(listString2[0], 240, lastPositionY));
            if (listString2[1].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listString2[1], 240, lastPositionY));
            }

            lastPositionY += addLine;
            listData.Add(CreateText("SAKSI", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 220, lastPositionY));
            listData.Add(CreateText(saksiName, 240, lastPositionY));
            SetFontBold(false);

            lastPositionY += addLine;
            listData.Add(CreateText("JABATAN", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 220, lastPositionY));
            listData.Add(CreateText(enforcerDto.Jabatan, 240, lastPositionY));
            SetFontBold(false);

            if (offendDto.PrintFlag != "0")
            {
                SetFontBold(true);
                SetFontSize(32);
                lastPositionY += addLine + positionX;
                listData.Add(CreateRectangle(20, lastPositionY, lastPositionY + 110, DefaultWidth, true));
                DefaultColor = Color.White;
                lastPositionY += addLine;
                listData.Add(CreateTitle("TAWARAN BAYARAN " + formatRayuanAmt, 110, lastPositionY));
                lastPositionY += addLine;
                listData.Add(CreateTitle("DALAM TEMPOH " + offendDto.PrintFlag.TrimEnd() + " HARI", 110, lastPositionY));
                SetFontBold(false);
                DefaultColor = Color.Black;

                SetFontSize(22);
                SetFontBold(true);
                lastPositionY += addLine + addLine;
                listData.Add(CreateText("TAWARAN KOMPAUN :", positionX, lastPositionY));
                SetFontBold(false);
                lastPositionY += position25;
                listData.Add(CreateText("Saya bersedia mengkompaun kesalahan ini dengan Kadar Kompaun", positionX, lastPositionY));
                lastPositionY += position25;
                SetFontBold(true);
                listData.Add(CreateText(formatPrintAmt, positionX, lastPositionY));
                SetFontBold(false);
                listData.Add(CreateText(". Tawaran ini berkuat kuasa dalam tempoh 14 hari dari tarikh", positionX + 105, lastPositionY));
                lastPositionY += position25;
                listData.Add(CreateText("notis ini. Kegagalan menjelaskan bayaran kompaun akan menyebabkan", positionX, lastPositionY));
                lastPositionY += position25;
                SetFontBold(true);
                listData.Add(CreateText("TINDAKAN MAHKAMAH ", positionX, lastPositionY));
                SetFontBold(false);
                listData.Add(CreateText("akan diteruskan.", positionX + 250, lastPositionY));

                SetFontBold(true);
                SetFontSize(19);
                lastPositionY += addLine + positionX;
                listData.Add(CreateRectangle(390, lastPositionY - position25, lastPositionY + 7, 780, true));
                DefaultColor = Color.White;
                listData.Add(CreateText("TEMPOH", 400, lastPositionY));
                listData.Add(CreateText("KADAR (RM)", 500, lastPositionY));
                listData.Add(CreateText("KOMPAUN TAMAT", 620, lastPositionY));
                DefaultColor = Color.Black;

                if (offendDto.PrintFlag == "3")
                {
                    listData.Add(CreateRectangle(391, lastPositionY + 5, lastPositionY + position25 + 5, 500, false));
                    listData.Add(CreateRectangle(500, lastPositionY + 5, lastPositionY + position25 + 5, 620, false));
                    listData.Add(CreateRectangle(620, lastPositionY + 5, lastPositionY + position25 + 5, 779, false));
                    lastPositionY += position25;
                    listData.Add(CreateText("1-3 HARI", 400, lastPositionY));
                    listData.Add(CreateText(formatRayuanAmt, 510, lastPositionY));
                    listData.Add(CreateText(formatNotaTempohDate, 630, lastPositionY));

                    listData.Add(CreateRectangle(391, lastPositionY + 5, lastPositionY + position25 + 5, 500, false));
                    listData.Add(CreateRectangle(500, lastPositionY + 5, lastPositionY + position25 + 5, 620, false));
                    listData.Add(CreateRectangle(620, lastPositionY + 5, lastPositionY + position25 + 5, 779, false));
                    lastPositionY += position25;
                    listData.Add(CreateText("4-14 HARI", 400, lastPositionY));
                    listData.Add(CreateText(amtkmp1, 510, lastPositionY));
                    listData.Add(CreateText(formatTempohDate, 630, lastPositionY));
                }
                else
                {
                    listData.Add(CreateRectangle(391, lastPositionY + 5, lastPositionY + position25 + 5, 500, false));
                    listData.Add(CreateRectangle(500, lastPositionY + 5, lastPositionY + position25 + 5, 620, false));
                    listData.Add(CreateRectangle(620, lastPositionY + 5, lastPositionY + position25 + 5, 779, false));
                    lastPositionY += position25;
                    listData.Add(CreateText("  1 HARI", 400, lastPositionY));
                    listData.Add(CreateText(formatRayuanAmt, 510, lastPositionY));
                    listData.Add(CreateText(formatNotaTempohDate, 630, lastPositionY));

                    listData.Add(CreateRectangle(391, lastPositionY + 5, lastPositionY + position25 + 5, 500, false));
                    listData.Add(CreateRectangle(500, lastPositionY + 5, lastPositionY + position25 + 5, 620, false));
                    listData.Add(CreateRectangle(620, lastPositionY + 5, lastPositionY + position25 + 5, 779, false));
                    lastPositionY += position25;
                    listData.Add(CreateText("2-14 HARI", 400, lastPositionY));
                    listData.Add(CreateText(amtkmp1, 510, lastPositionY));
                    listData.Add(CreateText(formatTempohDate, 630, lastPositionY));
                }

                lastPositionY += position25;
                lastPositionY += position25;
                var bitmapASign = BitmapFactory.DecodeResource(context.Resources, Asign);
                listData.Add(new PrintImageDto
                {
                    IsLogo = true,
                    SignKompaun = true,
                    Bitmap = bitmapASign,
                    Alignment = Paint.Align.Left,
                    IsJustified = false,
                    PositionX = 10,
                    PositionY = lastPositionY,
                    Width = 250
                });

                lastPositionY += position25;
                listData.Add(CreateLine(positionX, lastPositionY, 380, lastPositionY));
                lastPositionY += position25;

                SetFontBold(true);
                SetFontSize(18);
                listData.Add(CreateText("(PENGARAH UNDANG-UNDANG)", positionX, lastPositionY));
                lastPositionY += position25;
                SetFontBold(false);
                listData.Add(CreateText("JABATAN UNDANG-UNDANG", positionX, lastPositionY));
                lastPositionY += position25;
                listData.Add(CreateText("b.p YANG DIPERTUA", positionX, lastPositionY));
                lastPositionY += position25;
                listData.Add(CreateText("MAJLIS BANDARAYA PASIR GUDANG", positionX, lastPositionY));
            }
            else
            {
                SetFontSize(22);
                SetFontBold(true);
                lastPositionY += addLine + addLine;
                listData.Add(CreateText("TAWARAN KOMPAUN :", positionX, lastPositionY));
                SetFontBold(false);
                lastPositionY += position25;
                listData.Add(CreateText("Saya bersedia mengkompaun kesalahan ini dengan Kadar Kompaun", positionX, lastPositionY));
                lastPositionY += position25;
                SetFontBold(true);
                listData.Add(CreateText(formatPrintAmt, positionX, lastPositionY));
                SetFontBold(false);
                listData.Add(CreateText(". Tawaran ini berkuat kuasa dalam tempoh 14 hari dari tarikh", positionX + 105, lastPositionY));
                lastPositionY += position25;
                listData.Add(CreateText("notis ini. Kegagalan menjelaskan bayaran kompaun akan menyebabkan", positionX, lastPositionY));
                lastPositionY += position25;
                SetFontBold(true);
                listData.Add(CreateText("TINDAKAN MAHKAMAH ", positionX, lastPositionY));
                SetFontBold(false);
                listData.Add(CreateText("akan diteruskan.", positionX + 250, lastPositionY));

                lastPositionY += addLine + positionX;
                listData.Add(CreateRectangle(390, lastPositionY - position25, lastPositionY + 4, 780, true));
                DefaultColor = Color.White;
                SetFontSize(18);
                listData.Add(CreateText("TEMPOH", 400, lastPositionY));
                listData.Add(CreateText("KADAR (RM)", 500, lastPositionY));
                listData.Add(CreateText("KOMPAUN TAMAT", 620, lastPositionY));
                DefaultColor = Color.Black;

                listData.Add(CreateRectangle(391, lastPositionY + 5, lastPositionY + position25 + 5, 500, false));
                listData.Add(CreateRectangle(500, lastPositionY + 5, lastPositionY + position25 + 5, 620, false));
                listData.Add(CreateRectangle(620, lastPositionY + 5, lastPositionY + position25 + 5, 779, false));
                lastPositionY += position25;
                listData.Add(CreateText("  14 HARI", 400, lastPositionY));
                listData.Add(CreateText(amtkmp1, 510, lastPositionY));
                listData.Add(CreateText(formatTempohDate, 630, lastPositionY));

                lastPositionY += position25;
                lastPositionY += position25;
                var bitmapASign = BitmapFactory.DecodeResource(context.Resources, Asign);
                listData.Add(new PrintImageDto
                {
                    IsLogo = true,
                    SignKompaun = true,
                    Bitmap = bitmapASign,
                    Alignment = Paint.Align.Left,
                    IsJustified = false,
                    PositionX = 10,
                    PositionY = lastPositionY,
                    Width = 250
                });

                lastPositionY += position25;
                listData.Add(CreateLine(positionX, lastPositionY, 380, lastPositionY));
                lastPositionY += position25;

                SetFontBold(true);
                listData.Add(CreateText("(PENGARAH UNDANG-UNDANG)", positionX, lastPositionY));
                lastPositionY += position25;
                SetFontBold(false);
                listData.Add(CreateText("JABATAN UNDANG-UNDANG", positionX, lastPositionY));
                lastPositionY += position25;
                listData.Add(CreateText("b.p YANG DIPERTUA", positionX, lastPositionY));
                lastPositionY += position25;
                listData.Add(CreateText("MAJLIS BANDARAYA PASIR GUDANG", positionX, lastPositionY));

            }

            SetFontSize(18);
            lastPositionY += addLine + addLine + addLine;
            listData.Add(CreateText("SALINAN UNTUK ORANG KENA KOMPAUN", positionX, lastPositionY));
            lastPositionY += position25;
            SetFontSize(36);
            SetFontBold(true);
            listData.Add(CreateText("....................................................................................................", positionX, lastPositionY));

            SetFontSize(18);
            lastPositionY += position25;
            listData.Add(CreateText("SALINAN UNTUK KEGUNAAN PEJABAT", positionX, lastPositionY));

            lastPositionY += addLine + addLine + addLine;
            listData.Add(new PrintImageDto
            {
                IsLogo = true,
                Bitmap = GenerateBarcode(strNoKmp),
                Alignment = Paint.Align.Left,
                IsJustified = true,
                PositionX = 1,
                PositionY = lastPositionY - addLine - addLine,
                Width = 301,
                PositionBottom = (lastPositionY - addLine - addLine) + 50
            });
            SetFontSize(20);
            listData.Add(CreateText("NO KOMPAUN : " + strNoKmp, positionX, lastPositionY));
            listData.Add(CreateText("KOD KAUNTER : " + strKodHasil, 600, lastPositionY));

            DefaultColor = Color.White;
            lastPositionY += addLine;
            listData.Add(CreateRectangle(positionX, lastPositionY - position25, lastPositionY + 40, 780, true));
            listData.Add(CreateText("UNTUK DIISI OLEH PEGAWAI", positionX, lastPositionY, Paint.Align.Center, 400));
            listData.Add(CreateText("UNTUK DIISI OLEH", 400, lastPositionY, Paint.Align.Center, 400));
            lastPositionY += position25;
            listData.Add(CreateText("MENGKOMPAUN", positionX, lastPositionY, Paint.Align.Center, 400));
            listData.Add(CreateText("ORANG KENA KOMPAUN", 400, lastPositionY, Paint.Align.Center, 400));
            DefaultColor = Color.Black;

            listData.Add(CreateRectangle(positionX + 1, lastPositionY + 10, lastPositionY + 280, 400, false));
            listData.Add(CreateRectangle(400, lastPositionY + 10, lastPositionY + 280, 779, false));

            SetFontSize(18);
            var positionx1 = positionX + 3;
            var positionx2 = 410 + 3;
            lastPositionY += addLine;
            listData.Add(CreateText("Tawaran kompaun RM ..........................", positionx1, lastPositionY, Paint.Align.Left, 400));
            listData.Add(CreateText("Saya menerima tawaran mengkompaun ", positionx2, lastPositionY, Paint.Align.Left, 400));
            lastPositionY += addLine;
            listData.Add(CreateText("Tarikh tamat kompaun ..........................", positionx1, lastPositionY, Paint.Align.Left, 400));
            listData.Add(CreateText("suatu kesalahan bernombor ", positionx2, lastPositionY, Paint.Align.Left, 400));
            lastPositionY += addLine;
            listData.Add(CreateText("Tandatangan & cop pegawai mengkompaun :", positionx1, lastPositionY, Paint.Align.Left, 400));
            listData.Add(CreateText(compoundDto.CompNum, positionx2, lastPositionY, Paint.Align.Left, 400));

            lastPositionY += addLine + positionX;
            listData.Add(CreateText("Nama :", positionx2, lastPositionY, Paint.Align.Left, 400));
            lastPositionY += addLine;
            listData.Add(CreateText("Tarikh :", positionx1, lastPositionY, Paint.Align.Left, 400));
            listData.Add(CreateText("No. Kad Pengenalan :", positionx2, lastPositionY, Paint.Align.Left, 400));
            lastPositionY += addLine;
            listData.Add(CreateText("Waktu :", positionx1, lastPositionY, Paint.Align.Left, 400));
            listData.Add(CreateText("Alamat :", positionx2, lastPositionY, Paint.Align.Left, 400));

            SetFontSize(18);
            lastPositionY += addLine + 40;
            listData.Add(CreateText("* RESIT INI DIAKUI SAH SETELAH DICETAK OLEH MESIN PENCETAK RESIT MPK", positionX, lastPositionY));

            for (i = 0; i < 6; i++)
            {
                lastPositionY += 20;
                listData.Add(CreateText("", positionX, lastPositionY));
            }

            var bitmap = PrepareBitmap(lastPositionY);
            var canvas = CreateCanvas(bitmap);
            DrawText(canvas, listData);

            CreateFileBitmap(bitmap, "SampleBitmap");
            return bitmap;
        }

        public Bitmap CreateKompaunType3Bitmap(Context context, int logo, int jompay, int Asign, CompoundDto compoundDto, OffendDto offendDto, ActDto actDto, EnforcerDto enforcerDto)
        {
            int i = 0;
            string saksiName = "";

            string formatPrintAmtWord = SetAmount2Word(compoundDto.Compound3Type.CompAmt);
            string formatPrintAmt = FormatPrintAmount(compoundDto.Compound3Type.CompAmt);
            string formatPrintAmt2 = FormatPrintAmount(compoundDto.Compound3Type.CompAmt2);
            string formatPrintAmt3 = FormatPrintAmount(compoundDto.Compound3Type.CompAmt3);
            string formatPrintDate = GeneralBll.FormatPrintDate(compoundDto.CompDate);
            string formatPrintTime = GeneralBll.FormatPrintTime(compoundDto.CompTime);
            string amtkmp1 = FormatPrintAmount1(compoundDto.Compound3Type.CompAmt);
            string formatRayuanAmt = FormatPrintAmount1(offendDto.OffendAmt);

            string formatTempohDate = AddDate(compoundDto.CompDate, 13);
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

            string strKodHasil = offendDto.IncomeCode.PadRight(8);
            string strNoKmp = compoundDto.CompNum;

            int positionX = 10;
            int position25 = 25;
            int addLine = 40;

            var listData = new List<PrintImageDto> { };
            //var listData = new List<PrintImageDto>
            //{
            //    CreateLogoHeader(context, logo)
            //};

            //int lastPositionY = GetLastPositionY(listData);
            //lastPositionY += 150;
            int lastPositionY = 25;
            SetFontBold(true);
            listData.Add(CreateTitle("MAJLIS BANDARAYA PASIR GUDANG", 120, lastPositionY));
            lastPositionY += addLine;
            SetFontBold(false);
            listData.Add(CreateTitle("NOTIS KESALAHAN SERTA TAWARAN KOMPAUN", 10, lastPositionY));

            SetFontSize(20);
            lastPositionY += addLine + positionX;
            listData.Add(CreateText("KOD KAUNTER ", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 220, lastPositionY));
            listData.Add(CreateText(strKodHasil, 240, lastPositionY));
            SetFontBold(false);
            listData.Add(CreateText("NO KOMPAUN", 460, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(":", 590, lastPositionY));
            listData.Add(CreateText(strNoKmp, 600, lastPositionY));
            SetFontBold(false);

            lastPositionY += positionX;
            listData.Add(CreateLine(positionX, lastPositionY, DefaultWidth, lastPositionY));

            if (compoundDto.Compound3Type.CompanyName.ToString().TrimEnd().Length > 0)
            {
                lastPositionY += addLine;
                listData.Add(CreateText("NAMA SYARIKAT", positionX, lastPositionY));
                SetFontBold(true);
                listData.Add(CreateText(": ", 220, lastPositionY));
                listData.Add(CreateText(compoundDto.Compound3Type.CompanyName, 240, lastPositionY));
                SetFontBold(false);
            }

            if (compoundDto.Compound3Type.Company.ToString().TrimEnd().Length > 0)
            {
                lastPositionY += addLine;
                listData.Add(CreateText("N0. SYARIKAT", positionX, lastPositionY));
                SetFontBold(true);
                listData.Add(CreateText(": ", 220, lastPositionY));
                listData.Add(CreateText(compoundDto.Compound3Type.Company, 240, lastPositionY));
                SetFontBold(false);
            }

            if (compoundDto.Compound3Type.OffenderName.ToString().TrimEnd().Length > 0)
            {
                lastPositionY += addLine;
                listData.Add(CreateText("PEMILIK", positionX, lastPositionY));
                SetFontBold(true);
                listData.Add(CreateText(": ", 220, lastPositionY));
                listData.Add(CreateText(compoundDto.Compound3Type.OffenderName, 240, lastPositionY));
                SetFontBold(false);
            }

            if (compoundDto.Compound3Type.OffenderIc.ToString().TrimEnd().Length > 0)
            {
                lastPositionY += addLine;
                listData.Add(CreateText("NO.KP", positionX, lastPositionY));
                SetFontBold(true);
                listData.Add(CreateText(": ", 220, lastPositionY));
                listData.Add(CreateText(compoundDto.Compound3Type.OffenderIc, 240, lastPositionY));
                SetFontBold(false);
            }

            string alamat;
            alamat = compoundDto.Compound3Type.Address1.Trim() + "," + compoundDto.Compound3Type.Address2.Trim() + "," + compoundDto.Compound3Type.Address3.Trim();
            var listStringAlamat = SeparateText(alamat, 6, 55);

            lastPositionY += addLine;
            listData.Add(CreateText("ALAMAT", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 220, lastPositionY));
            listData.Add(CreateText(listStringAlamat[0], 240, lastPositionY));
            if (listStringAlamat[1].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringAlamat[1], 240, lastPositionY));
            }
            if (listStringAlamat[2].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringAlamat[2], 240, lastPositionY));
            }

            lastPositionY += addLine;
            listData.Add(CreateText("TARIKH KESALAHAN", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 220, lastPositionY));
            listData.Add(CreateText(formatPrintDate, 240, lastPositionY));
            SetFontBold(false);

            lastPositionY += addLine;
            listData.Add(CreateText("MASA KESALAHAN", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 220, lastPositionY));
            listData.Add(CreateText(formatPrintTime, 240, lastPositionY));
            SetFontBold(false);

            string tempatjadi;
            tempatjadi = compoundDto.StreetDesc;

            var listStringStreet = SeparateText(tempatjadi, 6, 50);

            lastPositionY += addLine;
            listData.Add(CreateText("TEMPAT KESALAHAN", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 220, lastPositionY));
            listData.Add(CreateText(listStringStreet[0], 240, lastPositionY));
            if (listStringStreet[1].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringStreet[1], 240, lastPositionY));
            }
            if (listStringStreet[2].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringStreet[2], 240, lastPositionY));
            }

            lastPositionY += addLine;
            listData.Add(CreateText("CARA PENYERAHAN", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 220, lastPositionY));
            listData.Add(CreateText(delivery.ShortDesc, 240, lastPositionY));
            SetFontBold(false);

            SetFontSize(18);
            lastPositionY += addLine;
            listData.Add(CreateText("KEPADA PEMUNYA/PEMANDU KENDERAAN TERSEBUT DI ATAS, TUAN/PUAN DIDAPATI", positionX, lastPositionY));
            lastPositionY += position25;
            listData.Add(CreateText("MELAKUKAN KESALAHAN SEPERTI BERIKUT :-", positionX, lastPositionY));

            SetFontSize(20);
            var listString1 = SeparateText(compoundDto.Compound3Type.CompDesc, 15, 45);
            lastPositionY += addLine;
            listData.Add(CreateText("BUTIR-BUTIR", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 220, lastPositionY));
            listData.Add(CreateText(listString1[0], 240, lastPositionY));
            SetFontBold(false);
            lastPositionY += position25;
            listData.Add(CreateText("KESALAHAN", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(listString1[1], 240, lastPositionY));

            if (listString1[2].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listString1[2], 240, lastPositionY));
            }
            if (listString1[3].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listString1[3], 240, lastPositionY));
            }
            if (listString1[4].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listString1[4], 240, lastPositionY));
            }
            if (listString1[5].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listString1[5], 240, lastPositionY));
            }

            SetFontBold(false);
            lastPositionY += addLine;
            listData.Add(CreateText("PERUNTUKAN UNDANG-UNDANG :-", positionX, lastPositionY));

            string section = offendDto.PrnDesc + "," + actDto.LongDesc;
            var listStringSection = SeparateText(section, 5, 70);
            SetFontBold(true);
            lastPositionY += position25;
            listData.Add(CreateText(listStringSection[0], positionX, lastPositionY));
            if (listStringSection[1].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringSection[1], positionX, lastPositionY));
            }
            if (listStringSection[2].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringSection[2], positionX, lastPositionY));
            }
            if (listStringSection[3].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listStringSection[3], positionX, lastPositionY));
            }

            var listString2 = SeparateText(enforcerDto.EnforcerId + "(" + enforcerDto.EnforcerName + ")", 2, 55);
            SetFontBold(false);
            lastPositionY += addLine;
            listData.Add(CreateText("DIKELUARKAN OLEH", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 220, lastPositionY));
            listData.Add(CreateText(listString2[0], 240, lastPositionY));
            if (listString2[1].TrimEnd() != "")
            {
                lastPositionY += position25;
                listData.Add(CreateText(listString2[1], 240, lastPositionY));
            }

            lastPositionY += addLine;
            listData.Add(CreateText("SAKSI", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 220, lastPositionY));
            listData.Add(CreateText(saksiName, 240, lastPositionY));
            SetFontBold(false);

            lastPositionY += addLine;
            listData.Add(CreateText("JABATAN", positionX, lastPositionY));
            SetFontBold(true);
            listData.Add(CreateText(": ", 220, lastPositionY));
            listData.Add(CreateText(enforcerDto.Jabatan, 240, lastPositionY));
            SetFontBold(false);

            if (offendDto.PrintFlag != "0")
            {
                SetFontBold(true);
                SetFontSize(32);
                lastPositionY += addLine + positionX;
                listData.Add(CreateRectangle(20, lastPositionY, lastPositionY + 110, DefaultWidth, true));
                DefaultColor = Color.White;
                lastPositionY += addLine;
                listData.Add(CreateTitle("TAWARAN BAYARAN " + formatRayuanAmt, 110, lastPositionY));
                lastPositionY += addLine;
                listData.Add(CreateTitle("DALAM TEMPOH " + offendDto.PrintFlag.TrimEnd() + " HARI", 110, lastPositionY));
                SetFontBold(false);
                DefaultColor = Color.Black;

                SetFontSize(22);
                SetFontBold(true);
                lastPositionY += addLine + addLine;
                listData.Add(CreateText("TAWARAN KOMPAUN :", positionX, lastPositionY));
                SetFontBold(false);
                lastPositionY += position25;
                listData.Add(CreateText("Saya bersedia mengkompaun kesalahan ini dengan Kadar Kompaun", positionX, lastPositionY));
                lastPositionY += position25;
                SetFontBold(true);
                listData.Add(CreateText(formatPrintAmt, positionX, lastPositionY));
                SetFontBold(false);
                listData.Add(CreateText(". Tawaran ini berkuat kuasa dalam tempoh 14 hari dari tarikh", positionX + 105, lastPositionY));
                lastPositionY += position25;
                listData.Add(CreateText("notis ini. Kegagalan menjelaskan bayaran kompaun akan menyebabkan", positionX, lastPositionY));
                lastPositionY += position25;
                SetFontBold(true);
                listData.Add(CreateText("TINDAKAN MAHKAMAH ", positionX, lastPositionY));
                SetFontBold(false);
                listData.Add(CreateText("akan diteruskan.", positionX + 250, lastPositionY));

                SetFontBold(true);
                SetFontSize(19);
                lastPositionY += addLine + positionX;
                listData.Add(CreateRectangle(390, lastPositionY - position25, lastPositionY + 7, 780, true));
                DefaultColor = Color.White;
                listData.Add(CreateText("TEMPOH", 400, lastPositionY));
                listData.Add(CreateText("KADAR (RM)", 500, lastPositionY));
                listData.Add(CreateText("KOMPAUN TAMAT", 620, lastPositionY));
                DefaultColor = Color.Black;

                if (offendDto.PrintFlag == "3")
                {
                    listData.Add(CreateRectangle(391, lastPositionY + 5, lastPositionY + position25 + 5, 500, false));
                    listData.Add(CreateRectangle(500, lastPositionY + 5, lastPositionY + position25 + 5, 620, false));
                    listData.Add(CreateRectangle(620, lastPositionY + 5, lastPositionY + position25 + 5, 779, false));
                    lastPositionY += position25;
                    listData.Add(CreateText("1-3 HARI", 400, lastPositionY));
                    listData.Add(CreateText(formatRayuanAmt, 510, lastPositionY));
                    listData.Add(CreateText(formatNotaTempohDate, 630, lastPositionY));

                    listData.Add(CreateRectangle(391, lastPositionY + 5, lastPositionY + position25 + 5, 500, false));
                    listData.Add(CreateRectangle(500, lastPositionY + 5, lastPositionY + position25 + 5, 620, false));
                    listData.Add(CreateRectangle(620, lastPositionY + 5, lastPositionY + position25 + 5, 779, false));
                    lastPositionY += position25;
                    listData.Add(CreateText("4-14 HARI", 400, lastPositionY));
                    listData.Add(CreateText(amtkmp1, 510, lastPositionY));
                    listData.Add(CreateText(formatTempohDate, 630, lastPositionY));
                }
                else
                {
                    listData.Add(CreateRectangle(391, lastPositionY + 5, lastPositionY + position25 + 5, 500, false));
                    listData.Add(CreateRectangle(500, lastPositionY + 5, lastPositionY + position25 + 5, 620, false));
                    listData.Add(CreateRectangle(620, lastPositionY + 5, lastPositionY + position25 + 5, 779, false));
                    lastPositionY += position25;
                    listData.Add(CreateText("  1 HARI", 400, lastPositionY));
                    listData.Add(CreateText(formatRayuanAmt, 510, lastPositionY));
                    listData.Add(CreateText(formatNotaTempohDate, 630, lastPositionY));

                    listData.Add(CreateRectangle(391, lastPositionY + 5, lastPositionY + position25 + 5, 500, false));
                    listData.Add(CreateRectangle(500, lastPositionY + 5, lastPositionY + position25 + 5, 620, false));
                    listData.Add(CreateRectangle(620, lastPositionY + 5, lastPositionY + position25 + 5, 779, false));
                    lastPositionY += position25;
                    listData.Add(CreateText("2-14 HARI", 400, lastPositionY));
                    listData.Add(CreateText(amtkmp1, 510, lastPositionY));
                    listData.Add(CreateText(formatTempohDate, 630, lastPositionY));
                }

                lastPositionY += position25;
                lastPositionY += position25;
                var bitmapASign = BitmapFactory.DecodeResource(context.Resources, Asign);
                listData.Add(new PrintImageDto
                {
                    IsLogo = true,
                    SignKompaun = true,
                    Bitmap = bitmapASign,
                    Alignment = Paint.Align.Left,
                    IsJustified = false,
                    PositionX = 10,
                    PositionY = lastPositionY,
                    Width = 250
                });

                lastPositionY += position25;
                listData.Add(CreateLine(positionX, lastPositionY, 380, lastPositionY));
                lastPositionY += position25;

                SetFontSize(18);
                SetFontBold(true);
                listData.Add(CreateText("(PENGARAH UNDANG-UNDANG)", positionX, lastPositionY));
                lastPositionY += position25;
                SetFontBold(false);
                listData.Add(CreateText("JABATAN UNDANG-UNDANG", positionX, lastPositionY));
                lastPositionY += position25;
                listData.Add(CreateText("b.p YANG DIPERTUA", positionX, lastPositionY));
                lastPositionY += position25;
                listData.Add(CreateText("MAJLIS BANDARAYA PASIR GUDANG", positionX, lastPositionY));
            }
            else
            {
                SetFontSize(22);
                SetFontBold(true);
                lastPositionY += addLine + addLine;
                listData.Add(CreateText("TAWARAN KOMPAUN :", positionX, lastPositionY));
                SetFontBold(false);
                lastPositionY += position25;
                listData.Add(CreateText("Saya bersedia mengkompaun kesalahan ini dengan Kadar Kompaun", positionX, lastPositionY));
                lastPositionY += position25;
                SetFontBold(true);
                listData.Add(CreateText(formatPrintAmt, positionX, lastPositionY));
                SetFontBold(false);
                listData.Add(CreateText(". Tawaran ini berkuat kuasa dalam tempoh 14 hari dari tarikh", positionX + 105, lastPositionY));
                lastPositionY += position25;
                listData.Add(CreateText("notis ini. Kegagalan menjelaskan bayaran kompaun akan menyebabkan", positionX, lastPositionY));
                lastPositionY += position25;
                SetFontBold(true);
                listData.Add(CreateText("TINDAKAN MAHKAMAH ", positionX, lastPositionY));
                SetFontBold(false);
                listData.Add(CreateText("akan diteruskan.", positionX + 250, lastPositionY));

                lastPositionY += addLine + positionX;
                listData.Add(CreateRectangle(390, lastPositionY - position25, lastPositionY + 4, 780, true));
                DefaultColor = Color.White;
                SetFontSize(18);
                listData.Add(CreateText("TEMPOH", 400, lastPositionY));
                listData.Add(CreateText("KADAR (RM)", 500, lastPositionY));
                listData.Add(CreateText("KOMPAUN TAMAT", 620, lastPositionY));
                DefaultColor = Color.Black;

                listData.Add(CreateRectangle(391, lastPositionY + 5, lastPositionY + position25 + 5, 500, false));
                listData.Add(CreateRectangle(500, lastPositionY + 5, lastPositionY + position25 + 5, 620, false));
                listData.Add(CreateRectangle(620, lastPositionY + 5, lastPositionY + position25 + 5, 779, false));
                lastPositionY += position25;
                listData.Add(CreateText("  14 HARI", 400, lastPositionY));
                listData.Add(CreateText(amtkmp1, 510, lastPositionY));
                listData.Add(CreateText(formatTempohDate, 630, lastPositionY));


                lastPositionY += position25;
                lastPositionY += position25;
                var bitmapASign = BitmapFactory.DecodeResource(context.Resources, Asign);
                listData.Add(new PrintImageDto
                {
                    IsLogo = true,
                    SignKompaun = true,
                    Bitmap = bitmapASign,
                    Alignment = Paint.Align.Left,
                    IsJustified = false,
                    PositionX = 10,
                    PositionY = lastPositionY,
                    Width = 250
                });

                lastPositionY += position25;
                listData.Add(CreateLine(positionX, lastPositionY, 380, lastPositionY));
                lastPositionY += position25;

                SetFontBold(true);
                listData.Add(CreateText("(PENGARAH UNDANG-UNDANG)", positionX, lastPositionY));
                lastPositionY += position25;
                SetFontBold(false);
                listData.Add(CreateText("JABATAN UNDANG-UNDANG", positionX, lastPositionY));
                lastPositionY += position25;
                listData.Add(CreateText("b.p YANG DIPERTUA", positionX, lastPositionY));
                lastPositionY += position25;
                listData.Add(CreateText("MAJLIS BANDARAYA PASIR GUDANG", positionX, lastPositionY));

            }

            SetFontSize(18);
            lastPositionY += addLine + addLine + addLine;
            listData.Add(CreateText("SALINAN UNTUK ORANG KENA KOMPAUN", positionX, lastPositionY));
            lastPositionY += position25;
            SetFontSize(36);
            SetFontBold(true);
            listData.Add(CreateText("....................................................................................................", positionX, lastPositionY));

            SetFontSize(18);
            lastPositionY += position25;
            listData.Add(CreateText("SALINAN UNTUK KEGUNAAN PEJABAT", positionX, lastPositionY));

            lastPositionY += addLine + addLine + addLine;
            listData.Add(new PrintImageDto
            {
                IsLogo = true,
                Bitmap = GenerateBarcode(strNoKmp),
                Alignment = Paint.Align.Left,
                IsJustified = true,
                PositionX = 1,
                PositionY = lastPositionY - addLine - addLine,
                Width = 301,
                PositionBottom = (lastPositionY - addLine - addLine) + 50
            });
            SetFontSize(20);
            listData.Add(CreateText("NO KOMPAUN : " + strNoKmp, positionX, lastPositionY));
            listData.Add(CreateText("KOD KAUNTER : " + strKodHasil, 600, lastPositionY));

            DefaultColor = Color.White;
            lastPositionY += addLine;
            listData.Add(CreateRectangle(positionX, lastPositionY - position25, lastPositionY + 40, 780, true));
            listData.Add(CreateText("UNTUK DIISI OLEH PEGAWAI", positionX, lastPositionY, Paint.Align.Center, 400));
            listData.Add(CreateText("UNTUK DIISI OLEH", 400, lastPositionY, Paint.Align.Center, 400));
            lastPositionY += position25;
            listData.Add(CreateText("MENGKOMPAUN", positionX, lastPositionY, Paint.Align.Center, 400));
            listData.Add(CreateText("ORANG KENA KOMPAUN", 400, lastPositionY, Paint.Align.Center, 400));
            DefaultColor = Color.Black;

            listData.Add(CreateRectangle(positionX + 1, lastPositionY + 10, lastPositionY + 280, 400, false));
            listData.Add(CreateRectangle(400, lastPositionY + 10, lastPositionY + 280, 779, false));

            SetFontSize(18);
            var positionx1 = positionX + 3;
            var positionx2 = 410 + 3;
            lastPositionY += addLine;
            listData.Add(CreateText("Tawaran kompaun RM ..........................", positionx1, lastPositionY, Paint.Align.Left, 400));
            listData.Add(CreateText("Saya menerima tawaran mengkompaun ", positionx2, lastPositionY, Paint.Align.Left, 400));
            lastPositionY += addLine;
            listData.Add(CreateText("Tarikh tamat kompaun ..........................", positionx1, lastPositionY, Paint.Align.Left, 400));
            listData.Add(CreateText("suatu kesalahan bernombor ", positionx2, lastPositionY, Paint.Align.Left, 400));
            lastPositionY += addLine;
            listData.Add(CreateText("Tandatangan & cop pegawai mengkompaun :", positionx1, lastPositionY, Paint.Align.Left, 400));
            listData.Add(CreateText(compoundDto.CompNum, positionx2, lastPositionY, Paint.Align.Left, 400));

            lastPositionY += addLine + positionX;
            listData.Add(CreateText("Nama :", positionx2, lastPositionY, Paint.Align.Left, 400));
            lastPositionY += addLine;
            listData.Add(CreateText("Tarikh :", positionx1, lastPositionY, Paint.Align.Left, 400));
            listData.Add(CreateText("No. Kad Pengenalan :", positionx2, lastPositionY, Paint.Align.Left, 400));
            lastPositionY += addLine;
            listData.Add(CreateText("Waktu :", positionx1, lastPositionY, Paint.Align.Left, 400));
            listData.Add(CreateText("Alamat :", positionx2, lastPositionY, Paint.Align.Left, 400));

            SetFontSize(18);
            lastPositionY += addLine + 40;
            listData.Add(CreateText("* RESIT INI DIAKUI SAH SETELAH DICETAK OLEH MESIN PENCETAK RESIT MPK", positionX, lastPositionY));

            for (i = 0; i < 6; i++)
            {
                lastPositionY += 20;
                listData.Add(CreateText("", positionX, lastPositionY));
            }

            var bitmap = PrepareBitmap(lastPositionY);
            var canvas = CreateCanvas(bitmap);
            DrawText(canvas, listData);

            CreateFileBitmap(bitmap, "SampleBitmap");
            return bitmap;
        }

        private PrintImageDto CreateLogoHeader(Context context, int logo, int positionLeft, int positionTop, int positionRight, int positionBottom, Paint.Align align)
        {
            var bitmap = BitmapFactory.DecodeResource(context.Resources, logo);

            var printImage = new PrintImageDto
            {
                IsLogo = true,
                Bitmap = bitmap,
                Alignment = align,
                IsJustified = true,
                Width = 200,
                PositionX = positionLeft,
                PositionLeft = positionLeft,
                PositionTop = positionTop,
                PositionRight = positionRight,
                PositionBottom = positionBottom
            };

            return printImage;
        }
        private string CreateFileBitmap(Bitmap bmp, string fileName)
        {
            string databaseFolder = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath + Constants.ImgsPath;
            GeneralBll.CreateFolder(databaseFolder);

            fileName = $"{databaseFolder}/{fileName}.png";
            var stream = new FileStream(fileName, FileMode.Create);

            bmp.Compress(Bitmap.CompressFormat.Jpeg, 85, stream);
            stream.Close();
            return fileName;
        }

        public enum BarcodeAlign
        {
            Left = 0,
            Right = 1,
            Center = 2
        }

        private PrintImageDto CreateBarcodeImage(int positionX, int positionY, string sValue, int size, BarcodeAlign align = BarcodeAlign.Left)
        {
            var bitmap = BarcodeImage.CreateBitmapBarcode2(sValue, size);

            var printImage = new PrintImageDto
            {
                IsLogo = true,
                Bitmap = bitmap
            };

            switch (align)
            {
                case BarcodeAlign.Left:
                    printImage.PositionLeft = positionX;
                    printImage.PositionTop = positionY;
                    printImage.PositionBottom = positionY + bitmap.Height;     //Y + image Height
                    printImage.PositionRight = positionX + bitmap.Width;       //X + Image width 
                    break;
                case BarcodeAlign.Right:
                    printImage.PositionLeft = positionX + size - bitmap.Width;
                    printImage.PositionTop = positionY;
                    printImage.PositionRight = positionX + size;               //X + Image width 
                    printImage.PositionBottom = positionY + bitmap.Height;     //Y + image Height
                    break;
                case BarcodeAlign.Center:
                    printImage.PositionLeft = (positionX + size - bitmap.Width) / 2;
                    printImage.PositionTop = positionY;
                    printImage.PositionRight = printImage.PositionLeft + bitmap.Width;       //X + Image width 
                    printImage.PositionBottom = positionY + bitmap.Height;                   //Y + image Height
                    break;
            }
            printImage.PositionX = printImage.PositionRight;
            printImage.PositionY = printImage.PositionBottom;

            return printImage;
        }

        private PrintImageDto CreateBarcodeImagePDF417(int positionX, int positionY, string sValue, int size, BarcodeAlign align = BarcodeAlign.Left)
        {
            //            var bitmap = BarcodeImage.CreateBitmapBarcodePDF417(sValue, size, 200, 5);
            align = BarcodeAlign.Left;
            var bitmap = BarcodeImage.CreateBitmapBarcodePDF417(sValue, 2000, 200, 5);

            var printImage = new PrintImageDto
            {
                IsLogo = true,
                Bitmap = bitmap
            };

            switch (align)
            {
                case BarcodeAlign.Left:
                    printImage.PositionLeft = positionX;
                    printImage.PositionTop = positionY;
                    printImage.PositionBottom = positionY + bitmap.Height;     //Y + image Height
                    printImage.PositionRight = positionX + bitmap.Width;       //X + Image width 
                    break;
                case BarcodeAlign.Right:
                    printImage.PositionLeft = positionX + size - bitmap.Width;
                    printImage.PositionTop = positionY;
                    printImage.PositionRight = positionX + size;               //X + Image width 
                    printImage.PositionBottom = positionY + bitmap.Height;     //Y + image Height
                    break;
                case BarcodeAlign.Center:
                    printImage.PositionLeft = (positionX + size - bitmap.Width) / 2;
                    printImage.PositionTop = positionY;
                    printImage.PositionRight = printImage.PositionLeft + bitmap.Width;       //X + Image width 
                    printImage.PositionBottom = positionY + bitmap.Height;                   //Y + image Height
                    break;
            }
            printImage.PositionX = printImage.PositionRight;
            printImage.PositionY = printImage.PositionBottom;

            //#if DEBUG
            //            CreateFileBitmap(bitmap, $"Barcode_{sValue}(TopLeft {printImage.PositionTop.ToString()},{printImage.PositionLeft.ToString()} " +
            //                $"(BottomRight {printImage.PositionBottom.ToString()},{printImage.PositionRight.ToString()})");
            //#endif

            return printImage;
        }

        private PrintImageDto CreateBarcodeImageQRCode(int positionX, int positionY, string sValue, int size, BarcodeAlign align = BarcodeAlign.Left)
        {
            //var bitmap = BarcodeImage.CreateBitmapBarcodePDF417(sValue, size, 200, 5);
            align = BarcodeAlign.Left;
            var bitmap = BarcodeImage.CreateBitmapBarcodeQRCode(sValue, 125, 125, 5);

            var printImage = new PrintImageDto
            {
                IsLogo = true,
                Bitmap = bitmap
            };

            switch (align)
            {
                case BarcodeAlign.Left:
                    printImage.PositionLeft = positionX;
                    printImage.PositionTop = positionY;
                    printImage.PositionBottom = positionY + bitmap.Height;     //Y + image Height
                    printImage.PositionRight = positionX + bitmap.Width;       //X + Image width 
                    break;
                case BarcodeAlign.Right:
                    printImage.PositionLeft = positionX + size - bitmap.Width;
                    printImage.PositionTop = positionY;
                    printImage.PositionRight = positionX + size;               //X + Image width 
                    printImage.PositionBottom = positionY + bitmap.Height;     //Y + image Height
                    break;
                case BarcodeAlign.Center:
                    printImage.PositionLeft = (positionX + size - bitmap.Width) / 2;
                    printImage.PositionTop = positionY;
                    printImage.PositionRight = printImage.PositionLeft + bitmap.Width;       //X + Image width 
                    printImage.PositionBottom = positionY + bitmap.Height;                   //Y + image Height
                    break;
            }
            printImage.PositionX = printImage.PositionRight;
            printImage.PositionY = printImage.PositionBottom;

            return printImage;
        }
        private Bitmap GenerateBarcode(string noKompaun)
        {
            try
            {
                ZXing.OneD.Code128Writer code128Writer = new ZXing.OneD.Code128Writer();
                BitMatrix bitMatrix = new MultiFormatWriter().encode(noKompaun, BarcodeFormat.CODE_128, 660, 264); //code128Writer.encode(noKompaun, ZXing.BarcodeFormat.CODE_128, 1, 1);
                var width = bitMatrix.Width;
                var height = bitMatrix.Height;
                int[] pixelsImage = new int[width * height];
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (bitMatrix[j, i])
                            pixelsImage[i * width + j] = (int)Convert.ToInt64(0xff000000);
                        else
                            pixelsImage[i * width + j] = (int)Convert.ToInt64(0xffffffff);

                    }
                }
                Bitmap bitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
                bitmap.SetPixels(pixelsImage, 0, width, 0, 0, width, height);

                return bitmap;
            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }
}