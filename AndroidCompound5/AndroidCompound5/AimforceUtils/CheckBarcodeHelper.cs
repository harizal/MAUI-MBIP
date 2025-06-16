using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidCompound5.AimforceUtils
{
	public static class CheckBarcodeHelper
	{

		public static int GetCheckDigit(string strSeq)
		{
			double dSeq = Convert.ToDouble(strSeq);
			double dHalf = dSeq / 2;

			double sinX = 0;
			double cosX = 0;
			double dTanValue = 0;

			int i1stDigit, i3rdDigit, i7thDigit;
			int i1stCheckDigit, i2ndCheckDigit;
			int iCheckDigit;


			sinX = Math.Sin(dHalf);
			cosX = Math.Cos(dHalf);

			// tan(), sin(), and cos() functions from math.h library
			// has problem with big value of radians, so change
			// the way of calculation by using this function.
			dTanValue = (sinX * cosX * 2) / (2 * cosX * cosX - 1);

			//string strTanValue = dTanValue.ToString("f20");
			string strTanValue = dTanValue.ToString();
			var temp = strTanValue.Split('.');
			var result = temp[1];

			strTanValue = result.PadRight(7, '0');

			i1stDigit = ConvertStringToInt(strTanValue.Substring(0, 1));
			i3rdDigit = ConvertStringToInt(strTanValue.Substring(2, 1));
			i7thDigit = ConvertStringToInt(strTanValue.Substring(6, 1));

			i1stCheckDigit = (i1stDigit + i3rdDigit) % 10;
			i2ndCheckDigit = (i3rdDigit + i7thDigit) % 10;
			iCheckDigit = (i1stCheckDigit * 10) + i2ndCheckDigit;

			return iCheckDigit;

		}

		public static int GetCheckDigit_2(string strSeq)
		{
			if (strSeq.Length <= 9) return -1;

			string strTanValue;
			string tmpstr;
			string strOdd, strEven;
			double dOddTanValue, dEvenTanValue;
			double dOddNo, dEvenNo;
			int i1stDigit, i2ndDigit, i3rdDigit;
			int i1stCheckDigit, i2ndCheckDigit;
			int iOddCheckDigit, iEvenCheckDigit; ;
			int iCheckDigit;
			string str;

			strOdd = strSeq.Substring(0, 1) + strSeq.Substring(2, 1) + strSeq.Substring(4, 1) + strSeq.Substring(6, 1) +
					 strSeq.Substring(8, 1);
			strEven = strSeq.Substring(1, 1) + strSeq.Substring(3, 1) + strSeq.Substring(5, 1) + strSeq.Substring(7, 1);

			dOddNo = Convert.ToDouble(strOdd);
			dEvenNo = Convert.ToDouble(strEven);

			dOddTanValue = Math.Tan(dOddNo);
			dEvenTanValue = Math.Tan(dEvenNo);

			//tmpstr = dOddTanValue.ToString("f10");
			tmpstr = dOddTanValue.ToString();

			var temp = tmpstr.Split('.');
			var result = temp[1];
			strTanValue = result.PadRight(7, '0');

			i1stDigit = ConvertStringToInt(strTanValue.Substring(0, 1));
			i2ndDigit = ConvertStringToInt(strTanValue.Substring(1, 1));
			i3rdDigit = ConvertStringToInt(strTanValue.Substring(2, 1));

			i1stCheckDigit = (i1stDigit + i2ndDigit) % 10;
			i2ndCheckDigit = (i2ndDigit + i3rdDigit) % 10;

			iOddCheckDigit = (i1stCheckDigit * 10) + i2ndCheckDigit;


			tmpstr = dEvenTanValue.ToString();
			temp = tmpstr.Split('.');
			result = temp[1];
			strTanValue = result.PadRight(7, '0');

			i1stDigit = ConvertStringToInt(strTanValue.Substring(0, 1));
			i2ndDigit = ConvertStringToInt(strTanValue.Substring(1, 1));
			i3rdDigit = ConvertStringToInt(strTanValue.Substring(2, 1));

			i1stCheckDigit = (i1stDigit + i2ndDigit) % 10;
			i2ndCheckDigit = (i2ndDigit + i3rdDigit) % 10;

			iEvenCheckDigit = (i1stCheckDigit * 10) + i2ndCheckDigit;

			iCheckDigit = iOddCheckDigit + iEvenCheckDigit;

			return iCheckDigit;
		}


		public static int GetCheckDigit_3(string strprefix, string strSeq)
		{
			double dTanValue;
			double sinX, cosX;

			string strTanValue, strSinValue, strCosValue;

			int i1stDigit, i2ndDigit, i3rdDigit;
			int i1stCheckDigit, i2ndCheckDigit;

			int iCheckDigit;


			i1stCheckDigit = 0;
			i2ndCheckDigit = 0;

			double dSeqNo = Convert.ToDouble(strSeq);

			sinX = Math.Sin(dSeqNo);
			//strSinValue = sinX.ToString("f20");
			strSinValue = sinX.ToString();
			var temp = strSinValue.Split('.');
			var result = temp[1];
			strSinValue = result.PadRight(7, '0');

			cosX = Math.Cos(dSeqNo);
			strCosValue = cosX.ToString();
			temp = strCosValue.Split('.');
			result = temp[1];
			strCosValue = result.PadRight(7, '0');


			dTanValue = Math.Tan(dSeqNo);
			strTanValue = dTanValue.ToString();
			temp = strTanValue.Split('.');
			result = temp[1];
			strTanValue = result.PadRight(7, '0');

			string cPrefix = strprefix.Substring(0, 1);

			switch (cPrefix.ToUpper())
			{
				case "A":
					i1stDigit = ConvertStringToInt(strSinValue.Substring(0, 1));// (int)str.wtof(strSinValue.Mid(0, 1)) ;
					i2ndDigit = ConvertStringToInt(strSinValue.Substring(2, 1));//(int)str.wtof(strSinValue.Mid(2, 1)) ;
					i3rdDigit = ConvertStringToInt(strTanValue.Substring(1, 1));//(int)str.wtof(strTanValue.Mid(1, 1)) ;
					i1stCheckDigit = (i1stDigit + i2ndDigit + i3rdDigit + 3) % 10;

					i1stDigit = ConvertStringToInt(strTanValue.Substring(3, 1));//(int)str.wtof(strTanValue.Mid(3, 1)) ;
					i2ndDigit = ConvertStringToInt(strCosValue.Substring(0, 1));//(int)str.wtof(strCosValue.Mid(0, 1)) ;
					i3rdDigit = ConvertStringToInt(strSinValue.Substring(2, 1));//(int)str.wtof(strSinValue.Mid(2, 1)) ;
					i2ndCheckDigit = (i1stDigit + i2ndDigit + i3rdDigit + 2) % 10;
					break;

				case "B":
					i1stDigit = ConvertStringToInt(strCosValue.Substring(2, 1));
					i2ndDigit = ConvertStringToInt(strSinValue.Substring(0, 1));
					i3rdDigit = ConvertStringToInt(strTanValue.Substring(0, 1));
					i1stCheckDigit = (i1stDigit + i2ndDigit + i3rdDigit + 2) % 10;

					i1stDigit = ConvertStringToInt(strSinValue.Substring(0, 1));
					i2ndDigit = ConvertStringToInt(strTanValue.Substring(1, 1));
					i3rdDigit = ConvertStringToInt(strSinValue.Substring(1, 1));
					i2ndCheckDigit = (i1stDigit + i2ndDigit + i3rdDigit + 3) % 10;
					break;

				case "C":
					i1stDigit = ConvertStringToInt(strSinValue.Substring(2, 1));
					i2ndDigit = ConvertStringToInt(strCosValue.Substring(0, 1));
					i3rdDigit = ConvertStringToInt(strSinValue.Substring(0, 1));
					i1stCheckDigit = Math.Abs(i1stDigit + i2ndDigit + i3rdDigit - 1) % 10;

					i1stDigit = ConvertStringToInt(strTanValue.Substring(1, 1));
					i2ndDigit = ConvertStringToInt(strTanValue.Substring(1, 1));
					i3rdDigit = ConvertStringToInt(strCosValue.Substring(0, 1));
					i2ndCheckDigit = (i1stDigit + i2ndDigit + i3rdDigit + 1) % 10;
					break;

				case "D":
					i1stDigit = ConvertStringToInt(strCosValue.Substring(3, 1));
					i2ndDigit = ConvertStringToInt(strCosValue.Substring(0, 1));
					i3rdDigit = ConvertStringToInt(strSinValue.Substring(0, 1));
					i1stCheckDigit = (i1stDigit + i2ndDigit + i3rdDigit + 3) % 10;

					i1stDigit = ConvertStringToInt(strTanValue.Substring(1, 1));
					i2ndDigit = ConvertStringToInt(strTanValue.Substring(1, 1));
					i3rdDigit = ConvertStringToInt(strCosValue.Substring(1, 1));
					i2ndCheckDigit = Math.Abs(i1stDigit + i2ndDigit + i3rdDigit - 5) % 10;
					break;

				case "E":
					i1stDigit = ConvertStringToInt(strTanValue.Substring(1, 1));
					i2ndDigit = ConvertStringToInt(strCosValue.Substring(1, 1));
					i3rdDigit = ConvertStringToInt(strCosValue.Substring(3, 1));
					i1stCheckDigit = (i1stDigit + i2ndDigit + i3rdDigit + 5) % 10;

					i1stDigit = ConvertStringToInt(strTanValue.Substring(1, 1));
					i2ndDigit = ConvertStringToInt(strSinValue.Substring(2, 1));
					i3rdDigit = ConvertStringToInt(strSinValue.Substring(0, 1));
					i2ndCheckDigit = Math.Abs(i1stDigit + i2ndDigit + i3rdDigit - 2) % 10;
					break;

				case "F":
					i1stDigit = ConvertStringToInt(strTanValue.Substring(0, 1));
					i2ndDigit = ConvertStringToInt(strSinValue.Substring(0, 1));
					i3rdDigit = ConvertStringToInt(strSinValue.Substring(2, 1));
					i1stCheckDigit = (i1stDigit + i2ndDigit + i3rdDigit + 6) % 10;

					i1stDigit = ConvertStringToInt(strCosValue.Substring(0, 1));
					i2ndDigit = ConvertStringToInt(strSinValue.Substring(0, 1));
					i3rdDigit = ConvertStringToInt(strSinValue.Substring(3, 1));
					i2ndCheckDigit = Math.Abs(i1stDigit + i2ndDigit + i3rdDigit - 3) % 10;
					break;



			}
			iCheckDigit = (i1stCheckDigit * 10) + i2ndCheckDigit;



			return iCheckDigit;
		}

		public static int GetCheckBias(string strSeq)
		{
			int iResult = 1;
			int icheckBias;
			int i;
			double dSeqNo;
			double dcheckBias;
			bool Is7zero = true;
			string str;

			if (strSeq.Length >= 8)
			{
				for (i = 1; i <= 8; i++)
				{
					if (strSeq.Substring(strSeq.Length - i, 1) != "0")
					{
						Is7zero = false;
						break;
					}
				}

				dSeqNo = Convert.ToDouble(strSeq);
				dcheckBias = (dSeqNo / 10000000);
				icheckBias = (int)dcheckBias;

				if (Is7zero)
				{
					iResult = iResult + icheckBias - 1;
				}
				else
				{
					iResult = iResult + icheckBias;
				}
			}

			return iResult;
		}

		public static string CBarcode2Seq(string strBarcode)
		{
			string sResult = "Invalid";
			int iLen = strBarcode.Length;

			switch (iLen)
			{
				case 9:
				case 11:
					string seqNo = strBarcode.Substring(1, iLen - 2);
					double dseqNo = Convert.ToInt32(seqNo);
					int iCheckDigit = Convert.ToInt32((strBarcode.Substring(strBarcode.Length - 1, 1) + strBarcode.Substring(0, 1)));

					int icheckBias = GetCheckBias(seqNo);

					int iResultA;
					if (dseqNo >= 20800001)
					{
						iResultA = GetCheckDigit_2(seqNo) + icheckBias;
					}
					else
						iResultA = GetCheckDigit(seqNo) + icheckBias;


					if (iResultA > 99)
					{
						string tmpstr = iResultA.ToString();
						iResultA = Convert.ToInt32(tmpstr.Substring(tmpstr.Length - 2, 2));
					}

					int iResultB;
					if (dseqNo >= 20800001)
					{
						iResultB = GetCheckDigit_2(seqNo) - icheckBias;
					}
					else
						iResultB = GetCheckDigit(seqNo) - icheckBias;


					if (iResultB < 0)
					{
						//tmpstr = iResultB.ToString();
						//tmpstr.Format(_T("%d"), iResultB) ;
						iResultB = 100 + iResultB;

					}
					else if (iResultB > 99)
					{
						iResultB = iResultB % 100;
					}

					string str1;
					if (iCheckDigit == iResultA)
					{
						str1 = "A";
					}
					else if (iCheckDigit == iResultB)
					{
						str1 = "B";
					}
					else
					{
						sResult = "Invalid";
						break;
					}

					sResult = str1 + seqNo.PadLeft(9, '0'); //PadL(seqNo, 9, "0");

					break;
				case 10:
					sResult = strBarcode.Substring(2, 7);
					sResult = sResult.PadLeft(10, '0');//PadL(strBarcode.Substring(2, 7), 10, "0");

					break;
				case 12:
					str1 = strBarcode.Substring(0, 1);
					seqNo = strBarcode.Substring(3, iLen - 3);
					iCheckDigit = Convert.ToInt32((strBarcode.Substring(1, 2)));
					int iResult = GetCheckDigit_3(str1, seqNo);

					if (iCheckDigit != iResult)
						sResult = "Invalid";
					else
						sResult = str1.ToUpper() + seqNo.PadLeft(9, '0');
					break;
			}

			return sResult;
		}

		public static int ConvertStringToInt(string value)
		{
			try
			{
				return Convert.ToInt32(value);
			}
			catch (Exception)
			{

				return 0;
			}
		}
	}


}
