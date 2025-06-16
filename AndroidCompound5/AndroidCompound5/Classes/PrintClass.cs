using Android.Bluetooth;
using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using Java.IO;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidCompound5.Classes
{
	public class PrintClass
	{
		private BluetoothSocket socket;
		private BluetoothAdapter adapter;
		private PrintWriter oStream;
		public List<BluetoothDevice> _listDevice;

		//private const int MaxLengthPaper = 82;
		private const int MaxLengthPaper = 60;

		private const string PrintAddedConstant = "";

		public PrintClass()
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
					if (socket.IsConnected)
						socket.Close();

					socket.Connect();

					oStream = new PrintWriter(socket.OutputStream, true);

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

		public void PrintText(string text)
		{
			oStream.Write(PrintAddedConstant + text);
			oStream.Flush();
		}

		public void PrintTextNoAdded(string text)
		{
			oStream.Write(text);
			oStream.Flush();
		}

		public void PrintChar(byte[] buffer)
		{

			for (int i = 0; i < buffer.Length; i++)
			{
				oStream.Write(buffer[i]);
			}

		}

		public void PrintFormFeed()
		{
			Byte[] PrintLineFeed = new Byte[4] { 27, 122, 27, 60 };

			for (int i = 0; i< PrintLineFeed.Length; i++)
			{
				oStream.Write(PrintLineFeed[i]);
			}

		}


		public void PrintLine()
		{
			oStream.Write("\r\n");
			oStream.Flush();
		}

		public void PrintLine(int iterate)
		{
			for (int i = 0; i < iterate; i++)
			{
				oStream.Write("\r\n");
				oStream.Flush();
			}

		}

		public void PrintClose()
		{
			oStream.Close();
			oStream.Dispose();

			if (socket.IsConnected)
			{
				socket.Close();
			}
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

		//public void PrintCompound(string compoundNumber)
		//{

		//    switch (GlobalClass.Compound.GetCompType())
		//    {
		//        case GlobalClass.CompType1:
		//            GlobalClass.Compound = null;
		//            GlobalClass.Compound = new Compound1Class();
		//            GlobalClass.Compound.GetCompoundRecord(compoundNumber);
		//            PrintCompound1();
		//            break;
		//        case GlobalClass.CompType2:
		//            GlobalClass.Compound = null;
		//            GlobalClass.Compound = new Compound2Class();
		//            GlobalClass.Compound.GetCompoundRecord(compoundNumber);
		//            PrintCompound2();
		//            break;
		//        case GlobalClass.CompType3:
		//            GlobalClass.Compound = null;
		//            GlobalClass.Compound = new Compound3Class();
		//            GlobalClass.Compound.GetCompoundRecord(compoundNumber);
		//            PrintCompound3();
		//            break;
		//        case GlobalClass.CompType4:
		//            GlobalClass.Compound = null;
		//            GlobalClass.Compound = new Compound4Class();
		//            GlobalClass.Compound.GetCompoundRecord(compoundNumber);
		//            PrintCompound4();
		//            break;
		//        case GlobalClass.CompType5:
		//            GlobalClass.Compound = null;
		//            GlobalClass.Compound = new Compound5Class();
		//            GlobalClass.Compound.GetCompoundRecord(compoundNumber);
		//            PrintCompound5();
		//            break;
		//    }

		//    PrintClose();
		//}


		//        private void PrintCompound5()
		//        {
		///*            var amt1 = GeneralBll.FormatPrintAmount(compoundDto.Compound5Type.UnlockAmt);
		//            var amt2 = GeneralBll.FormatPrintAmount(compoundDto.Compound5Type.TowAmt);

		//            var offendDesc = offendDto.PrnDesc + " " + offendDto.ActCode;


		//            string formatPrintDate = GeneralBll.FormatPrintDate(compoundDto.CompDate);
		//            string formatPrintTime = GeneralBll.FormatPrintTime(compoundDto.CompTime);


		//            // Start of compound layout
		//            PrintText("\x1B\x40\n\n");
		//            PrintText("\x1B\x45\x5A\n");
		//            PrintText("{print:@1,20:ALOGO,hmult2,vmult2|");
		//            PrintText("@36,180:MF107|MAJLIS BANDARAYA PETALING JAYA|");
		//            PrintText("@60,100:MF204|                JALAN YONG SHOOK LIN, 46675 PETALING JAYA|");

		//            PrintText("@108,100:MF204|                             NOTIS KESALAHAN|");
		//            PrintText("@132,180:MF107|  JANGAN ALIHKAN KENDERAAN INI|");
		//            PrintText("@156,90:MF107|(Pengalihan akan menyebabkan kerosakan)|");

		//            PrintText("@204,100:MF204|      PERINTAH LALULINTAS (PERATURAN MENGENAI TEMPAT LETAK KERETA)|");
		//            PrintText("@228,100:MF204|                           PETALING JAYA 1992|}");
		//            PrintText("{ahead:24}");
		//            PrintText("{LP}");
		//            PrintText("\x1B\x40\x1B\x77\x21");

		//            PrintText("\n\n");
		//            PrintText("BIL/BAYARAN YANG PERLU DIJELASKAN:\n");
		//            PrintText("1. Caj Membuka Kunci      : \x0E" + amt1.PadLeft(9) + "\x0F  No. Kompaun: \x0E" + compoundDto.CompNum + "\x0F\n");
		//            PrintText("2. Caj Mengalih Kenderaan : " + amt2.PadLeft(9) + "           No Kenderaan : \x0E" + compoundDto.Compound5Type.CarNum.PadRight(15) + "\x0F\n");
		//            PrintText("3. Bayaran Simpanan RM5 x : RM\n");
		//            PrintText("4. Lain-lain Bayaran Yang : RM\n");
		//            PrintText("   ditanggung oleh Majlis                            --------------------------\n");
		//            PrintText("                                                        Tandatangan Pengawai\n");
		//            PrintText("   Jumlah                 : RM                   Nama  :\n");
		//            PrintText("                                                 Tarikh:\n\n");

		//            PrintText("\x1B\x45\x5A\n");
		//            PrintText("{ahead:14}");
		//            PrintText("{print:");
		//            PrintText("@1,5:BC128,HIGH 9,WIDE 2|" + compoundDto.CompNum + "|");
		//            PrintText("@1,565:BC128,HIGH 9,WIDE 2|" + offendDto.IncomeCode.PadRight(8) + "|}");
		//            PrintText("{ahead:14}");
		//            PrintText("{LP}");
		//            PrintText("\x1B\x40\x1B\x77\x21");

		//            PrintText("\n\n");
		//            PrintText("-----------------------------------------------------------------------------------\n");
		//            PrintText("\n\n\n");

		//            PrintText("\x1B\x40\n\n");
		//            PrintText("\x1B\x45\x5A\n");
		//            PrintText("{print:@1,20:ALOGO,hmult2,vmult2|");
		//            PrintText("@36,180:MF107|MAJLIS BANDARAYA PETALING JAYA|");
		//            PrintText("@60,100:MF204|                JALAN YONG SHOOK LIN, 46675 PETALING JAYA|");

		//            PrintText("@108,100:MF204|                             NOTIS KESALAHAN|");
		//            PrintText("@132,180:MF107|  JANGAN ALIHKAN KENDERAAN INI|");
		//            PrintText("@156,90:MF107|(Pengalihan akan menyebabkan kerosakan)|");

		//            PrintText("@204,100:MF204|      PERINTAH LALULINTAS (PERATURAN MENGENAI TEMPAT LETAK KERETA)|");
		//            PrintText("@228,100:MF204|                           PETALING JAYA 1992|}");
		//            PrintText("{ahead:24}");
		//            PrintText("{LP}");
		//            PrintText("\x1B\x40\x1B\x77\x21");

		//            PrintText("\x1B\x45\x5A\n");
		//            PrintText("{ahead:12}");
		//            PrintText("{print:");
		//            PrintText("@12,1:MF204|No Kompaun       :|");
		//            PrintText("@12,185:MF107|" + compoundDto.CompNum + "|");
		//            PrintText("@1,575:BC128,HIGH 9,WIDE 2|" + compoundDto.CompNum + "|}");
		//            PrintText("{ahead:14}");
		//            PrintText("{LP}");
		//            PrintText("\x1B\x40\x1B\x77\x21");

		//            PrintText("No Kenderaan     : \x0E" + compoundDto.Compound5Type.CarNum.PadRight(15) + "\x0F    Kod Hasil:" + offendDto.IncomeCode.PadRight(8) + "\n\n");

		//            PrintText("\x1B\x45\x5A\n");
		//            PrintText("{ahead:12}");
		//            PrintText("{print:");
		//            PrintText("@12,1:MF204|No Cukai Jalan   :|");
		//            PrintText("@12,185:MF204|" + compoundDto.Compound5Type.RoadTax + "|");
		//            PrintText("@1,575:BC128,HIGH 9,WIDE 2|" + offendDto.IncomeCode.PadRight(8) + "|}");
		//            PrintText("{ahead:14}");
		//            PrintText("{LP}");
		//            PrintText("\x1B\x40\x1B\x77\x21");

		//            PrintText("Jenis Kenderaan  : " + compoundDto.Compound5Type.CarTypeDesc.PadRight(28) + "Model Kenderaan : " + compoundDto.Compound5Type.VehicleTypeDesc.PadRight(18) + "\n\n");
		//            PrintText("Warna            : " + compoundDto.Compound5Type.CarColorDesc.PadRight(40) + "\n\n");
		//            PrintText("Tarikh           : " + formatPrintDate + "                  Waktu Dikunci  : " + formatPrintTime + "\n\n");
		//            PrintText("No. Kunci        : " + compoundDto.Compound5Type.LockKey.PadRight(5) + "\n\n");

		//            //            PrintTextAlignment("Tempat           :" + compoundDto.StreetDesc);
		//            PrintTextAlignment(compoundDto.StreetDesc, "Tempat           :", 45);
		//            //            PrintTextAlignment("Kesalahan        :" + offendDto.LongDesc);
		//            PrintTextAlignment(offendDto.LongDesc, "Kesalahan        :", 45);

		//            PrintText("BIL/BAYARAN YANG PERLU DIJELASKAN:\n\n");

		//            PrintText("1. Caj Membuka Kunci      : \x0E" + amt1 + "\x0F\n\n");
		//            PrintText("2. Caj Mengalih Kenderaan : " + amt2 + "\n\n");
		//            PrintText("3. Bayaran Simpanan RM5 x : RM\n\n");
		//            PrintText("4. Lain-lain Bayaran Yang : RM                          --------------------------\n");
		//            PrintText("   ditanggung oleh Majlis                                  Tandatangan Pengawai\n");
		//            PrintText("                                                    Nama  :\n");
		//            PrintText("   Jumlah                 : RM                      Tarikh:\n\n");

		//            PrintText("Dengan ini adalah diberi Notis bahawa kenderaan tuan ditahan daripada bergerak dan/\n");
		//            PrintText("atau dialih kerana kesalahan seperti yang dinyatakan di atas.\n\n");

		//            PrintText("Pemunya-pemunya kenderaan yang melakukan mana-mana kesalahan tersebut boleh diambil\n");
		//            PrintText("tindakan dialih dan/atau ditahan daripada bergerak kenderaan mereka.\n\n");

		//            PrintText("Tuan  adalah  dikehendaki  supaya  hadir  di kaunter  penguatkuasa  di Majlis\n");
		//            PrintText("Bandaraya Petaling Jaya, Jalan Yong Shook Lin, 46675 Petaling Jaya No. Telefon \n");
		//            PrintText("79563544 samb. 148/149\x0F untuk menyelesaikan  hal ini pada  waktu\n");
		//            PrintText("pejabat seperti berikut:-\n");
		//            PrintText("Isnin-Jumaat    8.30 Pagi - 6.00 Petang\n");
		//            PrintText("Sabtu           9.00 Pagi - 2.00 Petang\n");
		//            PrintText("Dan pada tarikh kesalahan yang tercatat di atas dengan membawa Notis ini.\n\n");

		//            PrintText("Sekiranya Tuan gagal untuk menghadirkan diri seperti yang dikehendaki pada masa dan\n");
		//            PrintText("tarikh yang disebutkan di atas,  Majlis akan mengalihkan  kenderaan Tuan ke bengkel\n");
		//            PrintText("baru  Jalan  SS8/2,  Petaling  Jaya  untuk  ditahan  sehingga  segala  bayaran  dan\n");
		//            PrintText("perbelanjaan  mengenainya  dijelaskan. Bayaran  hanya diterima pada  waktu  pejabat\n");
		//            PrintText("seperti tersebut  di atas dalam bentuk tunai dan setelah  dibuktikan hak milik yang\n");
		//            PrintText("sah sahaja.\n\n");

		//            PrintText("Majlis tidak akan bertanggungjawab di atas  kehilangan dan kerosakan pada kenderaan\n");
		//            PrintText("Tuan  semasa ianya  ditahan atau  dialihkan  sama ada  kerosakan dan  kecurian  itu\n");
		//            PrintText("berlaku semasa kenderaan Tuan ditahan daripada bergerak atau dialihkan.\n\n");

		//            PrintText("Sila ambil  perhatian, tidak sesiapa  kecuali dibenarkan oleh Majlis  untuk berbuat\n");
		//            PrintText("demikian boleh  membuka atau  merosakkan alat m engunci tayar yang  digunakan untuk\n");
		//            PrintText("menahan kenderaan ini dan dilarang  mengalih atau menggerakkan  kenderaan ini tanpa\n");
		//            PrintText("kebenaran daripada Majlis.\n\n");

		//            PrintText("Dikeluarkan oleh : " + enforcerDto.EnforcerName.PadRight(60) + "\n");

		//            PrintText("\x1B\x45\x5A\n");
		//            PrintText("{print:");
		//            PrintText("@1,1:MF204|No. K/Pengenalan : " + enforcerDto.EnforcerIc.PadRight(20) + "|");
		//            PrintText("@25,1:MF204|Tarikh: " + formatPrintDate + "|");
		//            PrintText("@1,590:ASIGN|}");
		//            PrintText("{LP}");
		//            PrintText("\x1B\x40\x1B\x77\x21");

		//            PrintText("                                                  --------------------------------\n");
		//            PrintText("                                                       MOHD AZIZI BIN MOHD ZAIN\n");
		//            PrintText("                                                           Datuk Bandar\n");
		//            PrintText("---------------------------------                  Majlis Bandaraya Petaling Jaya\n");
		//            PrintText("Tandatangan Pegawai Yang Mengunci                 \n");

		//            PrintText("\n\n\n");
		//*/
		//        }

		//        private void PrintCompound4()
		//        {
		//            //var deliveryDto = TableFilBll.GetDeliveryByCode(compoundDto.Compound2Type.DeliveryCode);
		//            //if (deliveryDto == null)
		//            //    deliveryDto = new DeliveryDto();
		///*
		//            var amt = compoundDto.Compound4Type.CompAmt;
		//            var amt1 = compoundDto.Compound4Type.StorageAmt;
		//            var amt2 = compoundDto.Compound4Type.TransportAmt;
		//            var amt3 = Convert.ToDouble(amt) + Convert.ToDouble(amt1) + Convert.ToDouble(amt2);
		//            amt3 = amt3 / 100;
		//            string formatPrintAmt = amt3.ToString("f2");
		//            amt = GeneralBll.FormatPrintAmount(amt);
		//            amt1 = GeneralBll.FormatPrintAmount(amt1);
		//            amt2 = GeneralBll.FormatPrintAmount(amt2);

		//            string formatPrintDate = GeneralBll.FormatPrintDate(compoundDto.CompDate);
		//            string formatPrintTime = GeneralBll.FormatPrintTime(compoundDto.CompTime);
		//            var offendDesc = offendDto.PrnDesc + " " + offendDto.ActCode;

		//            // Start of compound layout
		//            PrintText("\x1B\x40\n\n");
		//            PrintText("\x1B\x45\x5A\n");
		//            PrintText("{print:@1,20:ALOGO,hmult2,vmult2|");
		//            PrintText("@12,180:MF107|MAJLIS BANDARAYA PETALING JAYA|");

		//            PrintText("@36,180:MF204|                BAHAGIAN P/KUASA &KESELAMATAN|");
		//            PrintText("@60,180:MF204|                  JABATAN KHIDMAT PENGURUSAN|");

		//            PrintText("@84,180:MF204|             TINGKAT 1, MENARA MPPJ, JALAN TENGAH|");
		//            PrintText("@108,180:MF204|              46200 PETALING JAYA. Tel: 79588085|");

		//            PrintText("@156,180:MF204|          SENARAI BARANG-BARANG YANG DIPINDAH/DISITA|}");
		//            PrintText("{ahead:24}");
		//            PrintText("{LP}");
		//            PrintText("\x1B\x40\x1B\x77\x21");

		//            PrintText("\n\n");
		//            PrintText("BAYARAN YANG PERLU DIJELASKAN:\n\n");

		//            PrintText("1. Kompaun          : " + amt.PadLeft(9) + "             Waktu Bayaran\n");
		//            PrintText("                                            Isnin-Jumaat     : 8.00 pg. - 6.00 ptg\n");
		//            PrintText("2. Caj Menyimpan    : " + amt1.PadLeft(9) + "             Sabtu            : 9.00 pg. - 2.00 ptg\n");
		//            PrintText("\n");
		//            PrintText("3. Caj Pengangkutan : " + amt2.PadLeft(9) + "\n\n");

		//            PrintText("   Jumlah           : " + formatPrintAmt.PadLeft(9) + "\n\n");

		//            PrintText("\x1B\x45\x5A\n");
		//            PrintText("{ahead:14}");
		//            PrintText("{print:");
		//            PrintText("@1,5:BC128,HIGH 9,WIDE 2|" + compoundDto.CompNum + "|");
		//            PrintText("@1,565:BC128,HIGH 9,WIDE 2|" + offendDto.IncomeCode.PadRight(8) + "|}");
		//            PrintText("{ahead:14}");
		//            PrintText("{LP}");
		//            PrintText("\x1B\x40\x1B\x77\x21");

		//            PrintText("Rayuan Kompaun:\n\n");

		//            PrintText("Kompaun dikurangkan kepada RM_______________\n\n");
		//            PrintText("Tarikh:________________\n\n");
		//            PrintText("Tandatangan Pegawai Berkuasa:_____________________\n\n");

		//            PrintText("\n\n\n");
		//            PrintText("-----------------------------------------------------------------------------------\n");
		//            PrintText("\n\n\n");

		//            PrintText("\x1B\x40\n\n");
		//            PrintText("\x1B\x45\x5A\n");
		//            PrintText("{print:@1,20:ALOGO,hmult2,vmult2|");
		//            PrintText("@12,180:MF107|MAJLIS BANDARAYA PETALING JAYA|");

		//            PrintText("@36,180:MF204|                BAHAGIAN P/KUASA &KESELAMATAN|");
		//            PrintText("@60,180:MF204|                  JABATAN KHIDMAT PENGURUSAN|");

		//            PrintText("@84,180:MF204|             TINGKAT 1, MENARA MPPJ, JALAN TENGAH|");
		//            PrintText("@108,180:MF204|              46200 PETALING JAYA. Tel: 79588085|");

		//            PrintText("@156,180:MF204|          SENARAI BARANG-BARANG YANG DIPINDAH/DISITA|}");
		//            PrintText("{ahead:24}");
		//            PrintText("{LP}");
		//            PrintText("\x1B\x40\x1B\x77\x21");


		//            PrintText("\x1B\x45\x5A\n");
		//            PrintText("{ahead:12}");
		//            PrintText("{print:");
		//            PrintText("@12,1:MF204|No Siri Sitaan :|");
		//            PrintText("@12,185:MF107|" + compoundDto.CompNum + "|");
		//            PrintText("@1,575:BC128,HIGH 9,WIDE 2|" + compoundDto.CompNum + "|}");
		//            PrintText("{ahead:14}");
		//            PrintText("{LP}");
		//            PrintText("\x1B\x40\x1B\x77\x21");

		//            PrintText("AMBIL PERHATIAN barang-barang berikut adalah dipindah/disitakan pada\n\n");

		//            PrintText("Tarikh    : " + formatPrintDate + "          Masa : " + formatPrintTime + "\n\n");

		//            PrintText("\x1B\x45\x5A\n");
		//            PrintText("{ahead:12}");
		//            PrintText("{print:");
		//            PrintText("@12,1:MF204|Kod Hasil :|");
		//            PrintText("@12,185:MF107|" + offendDto.IncomeCode.PadRight(8) + "|");
		//            PrintText("@1,575:BC128,HIGH 9,WIDE 2|" + offendDto.IncomeCode.PadRight(8) + "|}");
		//            PrintText("{ahead:14}");
		//            PrintText("{LP}");
		//            PrintText("\x1B\x40\x1B\x77\x21");

		//            //            PrintTextAlignment("Tempat :" + compoundDto.StreetDesc);
		//            PrintTextAlignment(compoundDto.StreetDesc, "Tempat :", 45);

		//            PrintText("bagi menjalankan kuasa-kuasa di bawah:\n\n");


		//            PrintText("Seksyen   : " + offendDesc + "\n\n");

		//            int i = 0;
		//            var listSita = CompoundBll.GetListSitaByCompoundNumber(compoundDto.CompNum);
		//            var listSitaS = listSita.Where(c => c.Flag == "S").ToList();
		//            var listSitaN = listSita.Where(c => c.Flag == "N").ToList();



		//            if (listSitaS.Count > 0)
		//            {
		//                PrintText("SENARAI BARANG-BARANG SENANG MUSNAH\n\n");

		//                foreach (var sitaDto in listSitaS)
		//                {
		//                    i++;
		//                    PrintText(i.ToString("00") + " " + sitaDto.Description + "\n");
		//                }

		//                PrintText("\n");

		//            }
		//            if (listSitaN.Count > 0)
		//            {
		//                PrintText("SENARAI BARANG-BARANG TIDAK SENANG MUSNAH\n\n");

		//                foreach (var sitaDto in listSitaN)
		//                {
		//                    i++;
		//                    PrintText(i.ToString("00") + " " + sitaDto.Description + "\n");
		//                }
		//                PrintText("\n");

		//            }

		//            PrintText("BUTIR-BUTIR PEMILIK / AGEN / WAKIL\n\n");
		//            PrintText("Nama    : " + compoundDto.Compound4Type.OffenderName.PadRight(60) + "\n\n");
		//            PrintText("No. K/P : " + compoundDto.Compound4Type.OffenderIc.PadRight(15) + "\n\n");

		//            string buf1 = compoundDto.Compound4Type.OffenderAddr;
		//            if (buf1.Length <= 35)
		//            {
		//                PrintText("        " + buf1.PadRight(35) + "\n");
		//                PrintText("        \n");
		//                PrintText("        \n");
		//            }
		//            else if (buf1.Length <= 70)
		//            {
		//                PrintText("        " + buf1.Substring(0, 35) + "\n");
		//                PrintText("        " + buf1.Substring(35, buf1.Length - 35).PadRight(35) + "\n");
		//                PrintText("        \n");
		//            }
		//            else
		//            {
		//                PrintText("        " + buf1.Substring(0, 35) + "\n");
		//                PrintText("        " + buf1.Substring(35, 35) + "\n");
		//                PrintText("        " + buf1.Substring(70, buf1.Length - 70).PadRight(30) + "\n");
		//            }



		//            PrintText("\n");
		//            PrintText("Saya mengesahkan barang-barang yang dipindah/disitakan adalah seperti dalam\n");
		//            PrintText("senarai ini.\n\n\n");

		//            PrintText("                                                         --------------------------\n");
		//            PrintText("                                                                Tandatangan\n");
		//            PrintText("                                                            (Pemilik/Agen/Wakil)\n");

		//            PrintText("-----------------------------------------------------------------------------------\n");
		//            PrintText("Nama Pegawai Bertugas: " + enforcerDto.EnforcerName.PadRight(60) + "\n");
		//            PrintText("No. K/Pengenalan: " + enforcerDto.EnforcerIc.PadRight(20) + "\n");
		//            PrintText("Tarikh: " + formatPrintDate + "\n");
		//            PrintText("Catatan:-                                                --------------------------\n");
		//            PrintText("                                                             T.Tangan Penyampai\n\n");

		//            var witness = EnforcerBll.GetEnforcerById(compoundDto.WitnessId);
		//            if (witness == null)
		//                witness = new EnforcerDto();

		//            PrintText("Nama Saksi: " + witness.EnforcerName.PadRight(60) + "\n");
		//            PrintText("No. K/Pengenalan: " + witness.EnforcerIc.PadRight(20) + "\n");
		//            PrintText("Tarikh: " + formatPrintDate + "                                       --------------------------\n");
		//            PrintText("                                                               T.Tangan Saksi\n\n");


		//            PrintText("\n\n\n");
		//*/

		//        }

		//        private void PrintCompound3()
		//        {
		//            int nLine = 0, i = 0 ;

		//            StructClass.offend_t offend = new StructClass.offend_t();
		//            StructClass.carcategory_t carcategory = new StructClass.carcategory_t();
		//            EnforcerClass Enforcer = new EnforcerClass();
		//            StructClass.zone_t zone = new StructClass.zone_t();
		//            StructClass.act_t Act = new StructClass.act_t();
		//            StructClass.delivery_t Delivery = new StructClass.delivery_t();

		//            // Get this compound's offend record
		//            GlobalClass.TableClass.GetOffendRecord(ref offend, GlobalClass.Compound.GetActCode(),
		//                                                   GlobalClass.Compound.GetOfdCode());

		//            GlobalClass.TableClass.GetActRecord(ref Act, offend.ActCode);

		//            GlobalClass.TableClass.GetCarCategoryRecord(ref carcategory, GlobalClass.Compound.GetCarCategory());

		//            GlobalClass.TableClass.GetZoneRecord(ref zone, GlobalClass.Compound.GetZone(),
		//                                                 GlobalClass.Compound.GetMukim());

		//            // Get issued this compound enforcer record
		//            Enforcer.GetEnforcerRecord(GlobalClass.Compound.GetEnforcerId());


		//            string formatPrintAmt = FormatPrintAmount(GlobalClass.Compound.GetCompAmt());
		//            string formatPrintAmt2 = FormatPrintAmount(GlobalClass.Compound.GetCompAmt2());
		//            string formatPrintAmt3 = FormatPrintAmount(GlobalClass.Compound.GetCompAmt3());
		//            string formatPrintDate = GeneralClass.GetDateFormatPrintCompund(GlobalClass.Compound.GetCompDate());
		//            string formatPrintTime = GeneralClass.GetTimeFormatCompound(GlobalClass.Compound.GetCompTime());

		//            var offendDesc = offend.PrnDesc + " " + offend.ShortDesc;


		//            GlobalClass.TableClass.GetDeliveryRecord(ref Delivery, GlobalClass.Compound.GetDeliveryCode());

		//            InitialisePrinter();
		//            SetFontSize(0);
		//            SetLeftMargin();
		//            SetLineSpacing(10);

		//            string strKodHasil = offend.IncomeCode.PadRight(8);
		//            string strNoKmp = GlobalClass.Compound.GetCompoundNumber();

		//            PrintText("\n");
		//            PrintText("\n");
		//            PrintText("\n");
		//            PrintText("\n");
		//            PrintText("\n");
		//            PrintText("\n");
		//            PrintText("\n");
		//            SetBold(true);
		//            PrintText("Kod Hasil : ");
		//            SetBold(false);
		//            PrintText(strKodHasil);
		//            PrintText("              ");
		//            SetBold(true);
		//            PrintText("NO KOMPAUN : ");
		//            SetBold(false);
		//            PrintText(strNoKmp);
		//            SetLineSpacing(10);
		//            PrintText("\n\n");

		//            SetBarCodeHight(40);
		//            PrintBarcode(strKodHasil);
		//            PrintText("                ");
		//            PrintBarcode(strNoKmp);
		//            PrintText("\n\n");
		//            SetLineSpacing(30);


		//            var listStringName = SeparateText(GlobalClass.Compound.GetOffenderName(), 4, 45);
		//            var listStringCompanyName = SeparateText(GlobalClass.Compound.GetCompanyName(), 4, 45);
		//            SetBold(true);
		//            PrintText("KEPADA            \n\n");
		//            nLine = 12;
		//            PrintText("NAMA/SYARIKAT     : ");
		//            SetBold(false);
		//            if (listStringName[0].Trim().Length > 0 && listStringCompanyName[0].Trim().Length > 0)
		//            {
		//                PrintText(listStringName[0] + "\n");
		//                nLine++;
		//                if (listStringName[1].Trim().Length > 0)
		//                {
		//                    PrintText("                    " + listStringName[1] + "\n");
		//                    nLine++;
		//                }

		//                if (listStringCompanyName[0].Trim().Length > 0)
		//                {
		//                    PrintText(listStringCompanyName[0] + "\n");
		//                    nLine++;
		//                }

		//                if (listStringCompanyName[1].Trim().Length > 0)
		//                {
		//                    PrintText("                    " + listStringCompanyName[1] + "\n");
		//                    nLine++;
		//                }
		//            }
		//            else if (listStringName[0].Trim().Length > 0)
		//            {
		//                PrintText(listStringName[0] + "\n");
		//                nLine++;
		//                if (listStringName[1].Trim().Length > 0)
		//                {
		//                    PrintText("                    " + listStringName[1] + "\n");
		//                    nLine++;
		//                }
		//            }
		//            else if (listStringCompanyName[0].Trim().Length > 0)
		//            {
		//                PrintText(listStringCompanyName[0] + "\n");
		//                nLine++;
		//                if (listStringCompanyName[1].Trim().Length > 0)
		//                {
		//                    PrintText("                    " + listStringCompanyName[1] + "\n");
		//                    nLine++;
		//                }
		//            }

		//            SetBold(true);
		//            PrintText("\nNO.KP/NO.SYARIKAT : ");
		//            nLine++;
		//            SetBold(false);

		//            if (GlobalClass.Compound.GetOffenderIc().Trim().Length > 0 && GlobalClass.Compound.GetCompany().Trim().Length > 0)
		//            {
		//                PrintText(GlobalClass.Compound.GetOffenderIc() + "/" + GlobalClass.Compound.GetCompany() +"\n");
		//                nLine++;
		//            }
		//            else if (GlobalClass.Compound.GetOffenderIc().Trim().Length > 0)
		//            {
		//                PrintText(GlobalClass.Compound.GetOffenderIc() + "\n");
		//                nLine++;
		//            }
		//            else if (GlobalClass.Compound.GetCompany().Trim().Length > 0)
		//            {
		//                PrintText(GlobalClass.Compound.GetCompany() + "\n");
		//                nLine++;
		//            }

		//            string alamat;

		//            alamat = GlobalClass.Compound.GetAlamat1().Trim() + "," + GlobalClass.Compound.GetAlamat2().Trim() + "," + GlobalClass.Compound.GetAlamat3().Trim();
		//            var listStringAlamat = SeparateText(alamat, 6, 45);

		//            SetBold(true);
		//            PrintText("\nALAMAT            : ");
		//            nLine++;
		//            SetBold(false);
		//            PrintText(listStringAlamat[0] + "\n");
		//            nLine++;
		//            SetBold(false);
		//            if (listStringAlamat[1].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listStringAlamat[1] + "\n");
		//                nLine++;
		//            }
		//            if (listStringAlamat[2].Trim().Length > 0)
		//            { 
		//                PrintText("                    " + listStringAlamat[2] + "\n");
		//                nLine++;
		//            }

		//            string tempatjadi ;
		//            if (string.Compare(GlobalClass.Compound.GetTempatjadi(), "SEPERTI DI ATAS") == 0)
		//                tempatjadi = GlobalClass.Compound.GetTempatjadi();
		//            else
		//                tempatjadi = GlobalClass.Compound.GetTempatjadi() + "," + GlobalClass.Compound.GetStreetDesc();

		//            var listStringStreet = SeparateText(tempatjadi, 6, 45);
		//            SetBold(true);
		//            PrintText("\nTEMPAT KESALAHAN  : ");
		//            nLine++;
		//            SetBold(false);
		//            PrintText(listStringStreet[0] + "\n");
		//            nLine++;
		//            SetBold(false);
		//            if (listStringStreet[1].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listStringStreet[1] + "\n");
		//                nLine++;
		//            }
		//            if (listStringStreet[2].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listStringStreet[2] + "\n");
		//                nLine++;
		//            }
		//            if (listStringStreet[3].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listStringStreet[3] + "\n");
		//                nLine++;
		//            }
		//            if (listStringStreet[4].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listStringStreet[4] + "\n");
		//                nLine++;
		//            }

		//            SetBold(true);
		//            nLine++;
		//            PrintText("\nTARIKH            : ");
		//            SetBold(false);
		//            PrintText(formatPrintDate + "\n");
		//            nLine++;

		//            SetBold(true);
		//            PrintText("\nMASA              : ");
		//            nLine++;
		//            SetBold(false);
		//            PrintText(formatPrintTime + "\n");
		//            nLine++;

		//            var listString0 = SeparateText(Act.LongDesc, 4, 45);
		//            SetBold(true);
		//            PrintText("\nAKTA/UUK          : ");
		//            nLine++;
		//            SetBold(false);
		//            PrintText(listString0[0] + "\n");
		//            nLine++;
		//            SetBold(false);
		//            if (listString0[1].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listString0[1] + "\n");
		//                nLine++;
		//            }
		//            if (listString0[2].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listString0[2] + "\n");
		//                nLine++;
		//            }

		//            var listString = SeparateText(offend.PrnDesc + " - " + offend.LongDesc, 8, 45);
		//            SetBold(true);
		//            PrintText("\nPERUNTUKAN        : ");
		//            nLine++;
		//            SetBold(false);
		//            PrintText(listString[0] +"\n");
		//            SetBold(true);
		//            nLine++;
		//            PrintText("PERUNDANGAN         ");
		//            SetBold(false);
		//            PrintText(listString[1] + "\n");
		//            nLine++;

		//            if (listString[2].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listString[2] + "\n");
		//                nLine++;
		//            }
		//            if (listString[3].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listString[3] + "\n");
		//                nLine++;
		//            }
		//            if (listString[4].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listString[4] + "\n");
		//                nLine++;
		//            }
		//            if (listString[5].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listString[5] + "\n");
		//                nLine++;
		//            }
		//            if (listString[6].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listString[6] + "\n");
		//                nLine++;
		//            }

		//            var listString1 = SeparateText(GlobalClass.Compound.GetCompDesc(), 8, 45);
		//            SetBold(true);
		//            PrintText("\nBUTIR-BUTIR       : ");
		//            SetBold(false);
		//            nLine++;
		//            PrintText(listString1[0] + "\n");
		//            nLine++;
		//            SetBold(true);
		//            PrintText("KESALAHAN           ");
		//            SetBold(false);
		//            PrintText(listString1[1] + "\n");
		//            nLine++;

		//            if (listString1[2].Trim().Length > 0)
		//            { 
		//                PrintText("                    " + listString1[2] + "\n");
		//                nLine++;
		//            }
		//            if (listString1[3].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listString1[3] + "\n");
		//                nLine++;
		//            }
		//            if (listString1[4].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listString1[4] + "\n");
		//                nLine++;
		//            }
		//            if (listString1[5].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listString1[5] + "\n");
		//                nLine++;
		//            }
		//            if (listString1[6].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listString1[6] + "\n");
		//                nLine++;
		//            }

		//            SetBold(true);
		//            PrintText("\nDIKELUARKAN OLEH  : ");
		//            nLine++;
		//            SetBold(false);
		////            PrintText(Enforcer.GetEnforcerName() + "\n") ;
		//            PrintText(Enforcer.GetEnforcerID() + "\n");
		//            nLine++;
		////            PrintText("Total Line : " + nLine);

		//            for (i = nLine; i < 43; i++)
		//            {
		//                PrintLine();
		//                nLine++;
		//            }

		//            SetBold(true);
		//            PrintText("\n\nBAYARAN DALAM             BAYARAN DALAM            BAYARAN DALAM\n");
		//            PrintText("TEMPOH 7 HARI             TEMPOH 14 HARI           TEMPOH 30 HARi\n");
		//            nLine = nLine + 5;
		//            SetFontSize(1);
		////            SetFontDouble();
		//            PrintText(formatPrintAmt + "      " + formatPrintAmt2 + "    " + formatPrintAmt3 + "\n\n\n");
		//            SetBold(false);
		//            SetFontSize(0);
		//            nLine = nLine + 3;

		//          //  PrintText("Total Line : " + nLine);

		//            PrintText("\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
		//            SetBold(true);
		//            PrintText("Kod Hasil : ");
		//            SetBold(false);
		//            PrintText(strKodHasil);
		//            PrintText("             ");
		//            SetBold(true);
		//            PrintText("NO KOMPAUN : ");
		//            SetBold(false);
		//            PrintText(strNoKmp);
		//            PrintText("\n\n\n");

		//            SetBold(true);
		//            PrintText("TARIKH            : ");
		//            SetBold(false);
		//            PrintText(formatPrintDate);

		//            SetBold(true);
		//            PrintText("\n\nMASA              : ");
		//            SetBold(false);
		//            PrintText(formatPrintTime);

		//            SetBold(true);
		//            PrintText("\n\nDIKELUARKAN OLEH  : ");
		//            SetBold(false);
		////          PrintText(Enforcer.GetEnforcerName());
		//            PrintText(Enforcer.GetEnforcerID());
		//            PrintFormFeed();


		//        }

		//        private void PrintCompound2()
		//        {
		//            int nLine = 0, i = 0;

		//            StructClass.offend_t offend = new StructClass.offend_t();
		//            StructClass.carcategory_t carcategory = new StructClass.carcategory_t();
		//            EnforcerClass Enforcer = new EnforcerClass();
		//            StructClass.zone_t zone = new StructClass.zone_t();
		//            StructClass.act_t Act = new StructClass.act_t();
		//            StructClass.delivery_t Delivery = new StructClass.delivery_t();

		//            // Get this compound's offend record
		//            GlobalClass.TableClass.GetOffendRecord(ref offend, GlobalClass.Compound.GetActCode(),
		//                                                   GlobalClass.Compound.GetOfdCode());

		//            GlobalClass.TableClass.GetActRecord(ref Act, offend.ActCode);

		//            GlobalClass.TableClass.GetCarCategoryRecord(ref carcategory, GlobalClass.Compound.GetCarCategory());

		//            GlobalClass.TableClass.GetZoneRecord(ref zone, GlobalClass.Compound.GetZone(),
		//                                                 GlobalClass.Compound.GetMukim());

		//            GlobalClass.TableClass.GetCarCategoryRecord(ref carcategory, GlobalClass.Compound.GetCarCategory());

		//            // Get issued this compound enforcer record
		//            Enforcer.GetEnforcerRecord(GlobalClass.Compound.GetEnforcerId());


		//            string formatPrintAmt = FormatPrintAmount(GlobalClass.Compound.GetCompAmt());
		//            string formatPrintAmt2 = FormatPrintAmount(GlobalClass.Compound.GetCompAmt2());
		//            string formatPrintAmt3 = FormatPrintAmount(GlobalClass.Compound.GetCompAmt3());
		//            string formatPrintDate = GeneralClass.GetDateFormatPrintCompund(GlobalClass.Compound.GetCompDate());
		//            string formatPrintTime = GeneralClass.GetTimeFormatCompound(GlobalClass.Compound.GetCompTime());

		//            var offendDesc = offend.PrnDesc + " " + offend.ShortDesc;


		//            GlobalClass.TableClass.GetDeliveryRecord(ref Delivery, GlobalClass.Compound.GetDeliveryCode());

		//            InitialisePrinter();
		//            SetFontSize(0);
		//            SetLeftMargin();
		//            SetLineSpacing(10);

		//            string strKodHasil = offend.IncomeCode.PadRight(8);
		//            string strNoKmp = GlobalClass.Compound.GetCompoundNumber();

		//            PrintText("\n");
		//            PrintText("\n");
		//            PrintText("\n");
		//            PrintText("\n");
		//            PrintText("\n");
		//            PrintText("\n");
		//            PrintText("\n");
		//            SetBold(true);
		//            PrintText("Kod Hasil : ");
		//            SetBold(false);
		//            PrintText(strKodHasil);
		//            PrintText("              ");
		//            SetBold(true);
		//            PrintText("NO KOMPAUN : ");
		//            SetBold(false);
		//            PrintText(strNoKmp);
		//            SetLineSpacing(10);
		//            PrintText("\n\n");

		//            SetBarCodeHight(40);
		//            PrintBarcode(strKodHasil);
		//            PrintText("                ");
		//            PrintBarcode(strNoKmp);
		//            PrintText("\n\n");
		//            SetLineSpacing(30);

		//            SetBold(true);
		//            PrintText("KEPADA             : PEMILIK KENDERAAN\n");
		//            nLine = 12;
		//            nLine++;
		//            PrintLine();
		//            nLine++;
		//            PrintText("NO KENDERAAN       : ");
		//            SetBold(false);
		//            PrintText(GlobalClass.Compound.GetCarNumber().PadRight(15) + "\n");
		//            nLine++;
		//            SetBold(true);
		//            PrintText("\nNO CUKAI JALAN     : " );
		//            SetBold(false);
		//            PrintText(GlobalClass.Compound.GetRoadTax() + "\n");
		//            nLine++;
		//            SetBold(true);
		//            PrintText("\nJENIS KENDERAAN    : ");
		//            SetBold(false);
		//            PrintText(carcategory.ShortDesc.PadRight(28) + "\n");
		//            nLine++;
		//            SetBold(true);
		//            PrintText("\nMODEL KENDERAAN    : " );
		//            SetBold(false);
		//            PrintText(GlobalClass.Compound.GetCarTypeDesc().PadRight(18) + "\n");
		//            nLine++;
		//            SetBold(true);
		//            PrintText("\nWARNA              : " );
		//            SetBold(false);
		//            PrintText(GlobalClass.Compound.GetCarColorDesc().PadRight(40) + "\n");
		//            nLine++;

		//            string tempatjadi;
		//            tempatjadi = GlobalClass.Compound.GetStreetDesc();

		//            var listStringStreet = SeparateText(tempatjadi, 6, 45);
		//            SetBold(true);
		//            PrintText("\nTEMPAT KESALAHAN  : ");
		//            nLine++;
		//            SetBold(false);
		//            PrintText(listStringStreet[0] + "\n");
		//            nLine++;
		//            SetBold(false);
		//            if (listStringStreet[1].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listStringStreet[1] + "\n");
		//                nLine++;
		//            }
		//            if (listStringStreet[2].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listStringStreet[2] + "\n");
		//                nLine++;
		//            }
		//            if (listStringStreet[3].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listStringStreet[3] + "\n");
		//                nLine++;
		//            }
		//            if (listStringStreet[4].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listStringStreet[4] + "\n");
		//                nLine++;
		//            }

		//            SetBold(true);
		//            nLine++;
		//            PrintText("\nTARIKH            : ");
		//            SetBold(false);
		//            PrintText(formatPrintDate + "\n");
		//            nLine++;

		//            SetBold(true);
		//            PrintText("\nMASA              : ");
		//            nLine++;
		//            SetBold(false);
		//            PrintText(formatPrintTime + "\n");
		//            nLine++;

		//            var listString0 = SeparateText(Act.LongDesc, 4, 45);
		//            SetBold(true);
		//            PrintText("\nAKTA/UUK          : ");
		//            nLine++;
		//            SetBold(false);
		//            PrintText(listString0[0] + "\n");
		//            nLine++;
		//            SetBold(false);
		//            if (listString0[1].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listString0[1] + "\n");
		//                nLine++;
		//            }
		//            if (listString0[2].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listString0[2] + "\n");
		//                nLine++;
		//            }

		//            var listString = SeparateText(offend.PrnDesc + " - " + offend.LongDesc, 8, 45);
		//            SetBold(true);
		//            PrintText("\nPERUNTUKAN        : ");
		//            nLine++;
		//            SetBold(false);
		//            PrintText(listString[0] + "\n");
		//            SetBold(true);
		//            nLine++;
		//            PrintText("PERUNDANGAN         ");
		//            SetBold(false);
		//            PrintText(listString[1] + "\n");
		//            nLine++;

		//            if (listString[2].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listString[2] + "\n");
		//                nLine++;
		//            }
		//            if (listString[3].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listString[3] + "\n");
		//                nLine++;
		//            }
		//            if (listString[4].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listString[4] + "\n");
		//                nLine++;
		//            }
		//            if (listString[5].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listString[5] + "\n");
		//                nLine++;
		//            }
		//            if (listString[6].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listString[6] + "\n");
		//                nLine++;
		//            }

		//            var listString1 = SeparateText(GlobalClass.Compound.GetCompDesc(), 8, 45);
		//            SetBold(true);
		//            PrintText("\nBUTIR-BUTIR       : ");
		//            SetBold(false);
		//            nLine++;
		//            PrintText(listString1[0] + "\n");
		//            nLine++;
		//            SetBold(true);
		//            PrintText("KESALAHAN           ");
		//            SetBold(false);
		//            PrintText(listString1[1] + "\n");
		//            nLine++;

		//            if (listString1[2].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listString1[2] + "\n");
		//                nLine++;
		//            }
		//            if (listString1[3].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listString1[3] + "\n");
		//                nLine++;
		//            }
		//            if (listString1[4].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listString1[4] + "\n");
		//                nLine++;
		//            }
		//            if (listString1[5].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listString1[5] + "\n");
		//                nLine++;
		//            }
		//            if (listString1[6].Trim().Length > 0)
		//            {
		//                PrintText("                    " + listString1[6] + "\n");
		//                nLine++;
		//            }

		//            SetBold(true);
		//            PrintText("\nDIKELUARKAN OLEH  : ");
		//            nLine++;
		//            SetBold(false);
		//            //PrintText(Enforcer.GetEnforcerName() + "\n");
		//            PrintText(Enforcer.GetEnforcerID() + "\n");
		//            nLine++;
		//            //            PrintText("Total Line : " + nLine);

		//            for (i = nLine; i < 42; i++)
		//            {
		//                PrintLine();
		//                nLine++;
		//            }

		//            SetBold(true);
		//            PrintText("\n\nBAYARAN DALAM             BAYARAN DALAM            BAYARAN DALAM\n");
		//            PrintText("TEMPOH 7 HARI             TEMPOH 14 HARI           TEMPOH 30 HARi\n");
		//            nLine = nLine + 5;
		//            SetFontSize(1);
		//            //            SetFontDouble();
		//            PrintText(formatPrintAmt + "      " + formatPrintAmt2 + "    " + formatPrintAmt3 + "\n\n\n");
		//            SetBold(false);
		//            SetFontSize(0);
		//            nLine = nLine + 3;

		//            //  PrintText("Total Line : " + nLine);

		//            PrintText("\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
		//            SetBold(true);
		//            PrintText("Kod Hasil : ");
		//            SetBold(false);
		//            PrintText(strKodHasil);
		//            PrintText("             ");
		//            SetBold(true);
		//            PrintText("NO KOMPAUN : ");
		//            SetBold(false);
		//            PrintText(strNoKmp);
		//            PrintText("\n\n\n");

		//            SetBold(true);
		//            PrintText("TARIKH            : ");
		//            SetBold(false);
		//            PrintText(formatPrintDate);

		//            SetBold(true);
		//            PrintText("\n\nMASA              : ");
		//            SetBold(false);
		//            PrintText(formatPrintTime);

		//            SetBold(true);
		//            PrintText("\nDIKELUARKAN OLEH  : ");
		//            nLine++;
		//            SetBold(false);
		//            //PrintText(Enforcer.GetEnforcerName() + "\n");
		//            PrintText(Enforcer.GetEnforcerID() + "\n");
		//            PrintFormFeed();


		//        }
		//        private void PrintCompoundOld2()
		//        {

		//            StructClass.offend_t offend = new StructClass.offend_t();
		//            StructClass.carcategory_t carcategory = new StructClass.carcategory_t();
		//            EnforcerClass Enforcer = new EnforcerClass();
		//            StructClass.zone_t zone = new StructClass.zone_t();
		//            StructClass.act_t Act = new StructClass.act_t();
		//            StructClass.delivery_t Delivery = new StructClass.delivery_t();


		//            // Get this compound's offend record
		//            GlobalClass.TableClass.GetOffendRecord(ref offend, GlobalClass.Compound.GetActCode(),
		//                                                   GlobalClass.Compound.GetOfdCode());

		//            GlobalClass.TableClass.GetActRecord(ref Act, offend.ActCode);

		//            GlobalClass.TableClass.GetCarCategoryRecord(ref carcategory, GlobalClass.Compound.GetCarCategory());

		//            GlobalClass.TableClass.GetZoneRecord(ref zone, GlobalClass.Compound.GetZone(),
		//                                                 GlobalClass.Compound.GetMukim());

		//            GlobalClass.TableClass.GetDeliveryRecord(ref Delivery, GlobalClass.Compound.GetDeliveryCode());

		//            // Get issued this compound enforcer record
		//            Enforcer.GetEnforcerRecord(GlobalClass.Compound.GetEnforcerId());


		//            string formatPrintAmt = FormatPrintAmount(GlobalClass.Compound.GetCompAmt());
		//            string formatPrintDate = GeneralClass.GetDateFormatPrintCompund(GlobalClass.Compound.GetCompDate());
		//            string formatPrintTime = GeneralClass.GetTimeFormatCompound(GlobalClass.Compound.GetCompTime());

		//            var offendDesc = offend.PrnDesc + " " + offend.ShortDesc;

		//            SetFontSize(0);
		//            SetLeftMargin();
		//            SetLineSpacing(30);
		//            PrintImage(1);

		//            PrintText("Nama              :\n");
		//            PrintText("No. Kad Pengenalan:\n");
		//            PrintText("Alamat            :\n");
		//            PrintText("(Alamat rumah/pejabat. BUKAN NOMBOR PETI SURAT.)\n");
		//            PrintText("Tarikh            :\n");

		//            PrintText("DB No.            : " + GlobalClass.Compound.GetCompoundNumber() + "\n");
		//            PrintText("No Kenderaan      : " + GlobalClass.Compound.GetCarNumber().PadRight(15) + "\n");
		//            PrintText("Bertarikh pada    : " + formatPrintDate + "\n\n\n\n");

		//            PrintText("_______________________\r\n");
		//            PrintText("Tandatangan\r\n");

		//            PrintText("Bayaran yang dikenakan: " + formatPrintAmt + "\n");

		//            PrintText("Bayaran dalam bentuk tunai, deraf bank dan wang kiriman POS sahaja\n");
		//            PrintText("\n");
		//            PrintText("------------------------------------------------------------------\n");
		//            PrintText("\n");

		//            PrintImage(2);

		//            PrintText("Kepada: Pemilik Kenderaan\n");

		//            PrintText("Tuan/Puan\n");

		//            PrintText("Menurut maklumat/aduan yang diterima, saya dapati tuan/puan telah\n");
		//            PrintText("melakukan kesalahan yang berikut:\n");

		//            PrintText("Kesalahan        : " + offendDesc + "\n");

		//            PrintText("Kompaun DB No.   : " + GlobalClass.Compound.GetCompoundNumber() + "\n");
		//            PrintBarcode(GlobalClass.Compound.GetCompoundNumber());
		//            PrintLine();
		//            PrintText("No Kenderaan     : " + GlobalClass.Compound.GetCarNumber().PadRight(15) + "\n");
		//            PrintText("No Cukai Jalan   : " + GlobalClass.Compound.GetRoadTax() + "\n");
		//            PrintText("Jenis Kenderaan  : " + carcategory.ShortDesc.PadRight(28) + "\n");
		//            PrintText("Model Kenderaan  : " + GlobalClass.Compound.GetCarTypeDesc().PadRight(18) + "\n");
		//            PrintText("Warna            : " + GlobalClass.Compound.GetCarColorDesc().PadRight(40) + "\n");
		//            //PrintTextAlignment("Tempat           : " + compoundDto.StreetDesc);
		//            PrintTextAlignment(GlobalClass.Compound.GetStreetDesc(), "Tempat           : ", 45);
		//            PrintText("Tarikh           : " + formatPrintDate + "\n");
		//            PrintText("Waktu            : " + formatPrintTime + "\n");
		//            PrintText("Kod Hasil        : " + offend.IncomeCode.PadRight(8) + "\n");
		//            PrintBarcode(offend.IncomeCode.PadRight(8));
		//            PrintLine(2);
		//            //PrintTextAlignment("Keterangan       :" + offendDto.LongDesc);
		//            PrintTextAlignment(offend.LongDesc, "Keterangan       : ", 50);
		//            PrintLine();
		//            //            PrintTextAlignment("Butir-butir Kesalahan :" + compoundDto.Compound2Type.CompDesc);
		//            PrintTextAlignment(GlobalClass.Compound.GetCompDesc(), "Butir-butir Kesalahan :", 45);
		//            PrintLine();
		//            PrintText("Bayaran yang dikenakan: " + formatPrintAmt + "\n\n");

		//            PrintText("---------------------------------------------------------------------\n");
		//            PrintText("Dengan ini tuan/puan  adalah  dimaklumkan  bahawa menurut kuasa  yang\n");
		//            PrintText("diberi kepada saya  oleh Undang-undang  kecil (Mengkompaun Kesalahan-\n");
		//            PrintText("kesalahan)  (Kawasan  Majlis Bandaraya Petaling Jaya)  *Jalan,  Parit\n");
		//            PrintText("dan Bangunan 1974/Kerajaan Tempatan 1976, saya  bersedia  dan  dengan\n");
		//            PrintText("ini menawarkan untuk mengkompaun kesalahan itu mengikut kadar kompaun\n");
		//            PrintText("yang  telah  ditetapkan  mengikut  jenis kesalahan yang dinyatakan di\n");
		//            PrintText("atas kompaun ini.\n\n");

		//            PrintText("Jika tawaran ini diterima, pembayaran boleh dibuat melalui laman  web\n");
		//            PrintText("www.mbpj.gov.my klik ePay@MBPJ atau mestilah dibuat dengan wang tunai\n");
		//            PrintText("atau kiriman wang pos, pesanan juruwang, pesanan bank atau deraf bank\n");
		//            PrintText("yang  dibuat  untuk  dibayar  kepada  Datuk  Bandar, Majlis Bandaraya\n");
		//            PrintText("Petaling Jaya yang dipalangkan dengan perkataan Akaun Penerima Sahaja\n");
		//            PrintText("boleh diserahkan dengan sendiri kepada saya di alamat Pejabat  Majlis\n");
		//            PrintText("Bandaraya Petaling Jaya, Jalan Yong Shook Lin,  46675  Petaling Jaya.\n");
		//            PrintText("Jika  terdapat  sebarang  kemusykilan,  tuan/puan  boleh  menghubungi\n");
		//            PrintText("pegawai bertugas di talian 03-79602657, 79602658, 79588085  dan  Faks\n");
		//            PrintText("603-79607141.\n\n");

		//            PrintText("Suatu  resit  rasmi  akan  dikeluarkan di atas pembayaran tersebut.\n\n");

		//            PrintText("Tawaran ini akan habis tempoh pada 14 hari sahaja. Jika bayaran penuh\n");
		//            PrintText("jumlah  wang  yang  dinyatakan  di  atas,  diterima pada atau sebelum\n");
		//            PrintText("penutupan  urusan   pada  tarikh  tersebut,   tiada  apa-apa  langkah\n");
		//            PrintText("perbicaraan selanjutnya akan  diambil  terhadap  tuan/puan  berhubung\n");
		//            PrintText("dengan  kesalahan  itu. Jika  tidak langkah-langkah akan diambil bagi\n");
		//            PrintText("menjalankan pendakwaan tanpa notis selanjutnya.\n\n");

		//            PrintText("Bertarikh pada: " + formatPrintDate + "\n\n");
		//            PrintText("                                ");
		//            PrintImage(0);

		//            PrintText("                                         ----------------------------\n");
		//            PrintText("                                           MOHD AZIZI BIN MOHD ZAIN  \n");
		//            PrintText("                                                 Datuk Bandar        \n");
		//            PrintText("                                       Majlis Bandaraya Petaling Jaya\n\n");


		//            PrintText("Kod Badan : " + Enforcer.GetEnforcerIC().PadRight(10) + "\n");
		//            PrintText("Tarikh Penyerahan: " + formatPrintDate + "\n");
		//            PrintText("Cara Penyerahan: " + Delivery.ShortDesc.PadRight(17) + "        ----------------------\n");
		//            PrintText("                                             T.Tangan Penyampai\n");
		//            PrintText("Nama Penerima:\n");
		//            PrintText("No. K/Pengenalan:\n");
		//            PrintText("Alamat:                                  ----------------------\n");
		//            PrintText("                                              T.Tangan Penerima\n");

		//            PrintText("\n\n\n\n");


		//        }

		//        private string FormatPrintAmount(string amount)
		//        {
		//            decimal decTemp = 0;
		//            string sResult = "";

		//            try
		//            {
		//                decTemp = Convert.ToDecimal(amount);
		//            }
		//            catch { }

		//            sResult = decTemp > 0 ? string.Format("RM{0}", (decTemp / 100).ToString("f")) : "RM";

		//            return sResult;
		//        }

		//        private void PrintCompound1()
		//        {

		//            StructClass.offend_t offend = new StructClass.offend_t();
		//            StructClass.carcategory_t carcategory = new StructClass.carcategory_t();
		//            EnforcerClass Enforcer = new EnforcerClass();
		//            StructClass.zone_t zone = new StructClass.zone_t();
		//            StructClass.act_t Act = new StructClass.act_t();

		//            // Get this compound's offend record
		//            GlobalClass.TableClass.GetOffendRecord(ref offend, GlobalClass.Compound.GetActCode(),
		//                                                   GlobalClass.Compound.GetOfdCode());

		//            GlobalClass.TableClass.GetActRecord(ref Act, offend.ActCode);

		//            GlobalClass.TableClass.GetCarCategoryRecord(ref carcategory, GlobalClass.Compound.GetCarCategory());

		//            GlobalClass.TableClass.GetZoneRecord(ref zone, GlobalClass.Compound.GetZone(),
		//                                                 GlobalClass.Compound.GetMukim());

		//            // Get issued this compound enforcer record
		//            Enforcer.GetEnforcerRecord(GlobalClass.Compound.GetEnforcerId());


		//            string formatPrintAmt = FormatPrintAmount(GlobalClass.Compound.GetCompAmt());
		//            string formatPrintDate = GeneralClass.GetDateFormatPrintCompund(GlobalClass.Compound.GetCompDate());
		//            string formatPrintTime = GeneralClass.GetTimeFormatCompound(GlobalClass.Compound.GetCompTime());

		//            var offendDesc = offend.PrnDesc + " " + offend.ShortDesc;

		//            SetFontSize(0);
		//            SetLeftMargin();
		//            SetLineSpacing(30);
		//            PrintImage(3);

		//            PrintText("Bayaran yang dikenakan : " + formatPrintAmt + "\n");
		//            PrintText("Tarikh                 : " + formatPrintDate + "\n");
		//            PrintText("No Kenderaan           : " + GlobalClass.Compound.GetCarNumber().PadRight(15) + "\n");
		//            PrintText("Kompaun PJP No         : " + GlobalClass.Compound.GetCompoundNumber() + "\n");

		//            PrintText("\r\n\n_____________________________________\r\n");
		//            PrintText("T /Tangan Pemandu atau Pemilik Kereta Motor\r\n\n");

		//            PrintText("Bayaran dalam bentuk tunai, deraf bank dan wang kiriman POS sahaja\n");
		//            PrintText("\r_________________________________________________________________\r\n\n");

		//            PrintText("Kepada :\r\n");
		//            PrintText("     PEMILIK KENDERAAN\r\n\n");
		//            PrintText("Tuan/Puan\r\n");

		//            PrintText("Menurut maklumat/aduan yang diterima, saya dapati tuan/puan telah\n");
		//            PrintText("melakukan kesalahan yang berikut:\n\n");

		//            PrintText("Peruntukan Undang-undang berkaitan: Perintah 5 Perintah Pengangkutan\n");
		//            PrintText("Jalan (Peruntukan Tempat Letak Kereta) Majlis Bandaraya Petaling\n");
		//            PrintText("Jaya 2007\r\n\n");

		//            PrintText("Jenis Kompaun    : " + offend.IncomeCode.PadRight(8) + "\n");
		//            PrintBarcode(offend.IncomeCode.PadRight(8));
		//            PrintLine(2);
		//            PrintText("Kompaun PJP No.  : " + GlobalClass.Compound.GetCompoundNumber() + "\n");
		//            PrintBarcode(GlobalClass.Compound.GetCompoundNumber());

		//            PrintLine(2);
		//            PrintText("Tarikh           : " + formatPrintDate + "           Waktu    : " + formatPrintTime + "\n");
		//            PrintText("No Kenderaan     : " + GlobalClass.Compound.GetCarNumber().PadRight(15) + "      Warna    : " + GlobalClass.Compound.GetCarColorDesc().TrimEnd() + "\n");
		//            PrintText("No Cukai Jalan   : " + GlobalClass.Compound.GetRoadTax() + "             No Petak : " + GlobalClass.Compound.GetParkingLot() + "\n");
		//            PrintText("Jenis Kenderaan  : " + GlobalClass.Compound.GetCarTypeDesc().PadRight(28) + "\n");
		//            PrintTextAlignment(GlobalClass.Compound.GetStreetDesc(), "Jalan            : ", 45);
		//            PrintTextAlignment(offend.LongDesc, "Keterangan       : ", 50);
		//            PrintLine();

		//            string sTemp = "";


		//            PrintTextAlignment(sTemp, "Butir-butir Kesalahan  : ", 45);
		//            PrintLine();

		//            PrintText("Bayaran yang dikenakan : " + formatPrintAmt + "\n\n");

		//            PrintText("Dikeluarkan Oleh       : " + Enforcer.GetEnforcerID() + "\n");

		//            PrintText("---------------------------------------------------------------------\n");
		//            PrintText("Dengan ini tuan/puan adalah dimaklumkan  bahawa  menurut  kuasa  yang\n");
		//            PrintText("diberi kepada saya oleh Sek 120(e) Akta Pengangkutan jalan 1987  saya\n");
		//            PrintText("bersedia  dan dengan ini menawarkan untuk mengkompaun  kesalahan  itu\n");
		//            PrintText("mengikut kadar kompaun yang telah ditetapkan mengikut jenis kesalahan\n");
		//            PrintText("yang ditandakan dibelakang kompaun ini.  Kadar  kompaun  yang  telah \n");
		//            PrintText("ditetapkan mengikut jenis kesalahan yang dinyatakan di  atas  kompaun\n.");
		//            PrintText("ini.\n\n");

		//            PrintText("Jika tawaran ini diterima, pembayaran boleh dibuat melalui laman  web\n");
		//            PrintText("www.mbpj.gov.my klik ePay@MBPJ atau mestilah dibuat dengan wang tunai\n");
		//            PrintText("atau kiriman wang pos, pesanan juruwang, pesanan bank atau deraf bank\n");
		//            PrintText("yang  dibuat  untuk  dibayar  kepada  Datuk  Bandar, Majlis Bandaraya\n");
		//            PrintText("Petaling Jaya yang dipalangkan dengan perkataan Akaun Penerima Sahaja\n");
		//            PrintText("boleh diserahkan dengan sendiri kepada saya di alamat Pejabat  Majlis\n");
		//            PrintText("Bandaraya Petaling Jaya, Jalan Yong Shook Lin,  46675  Petaling Jaya.\n");
		//            PrintText("Jika  terdapat  sebarang  kemusykilan,  tuan/puan  boleh  menghubungi\n");
		//            PrintText("pegawai bertugas di talian 03-79602657, 79602658, 79588085  dan  Faks\n");
		//            PrintText("603-79607141.\n\n");

		//            PrintText("Suatu  resit  rasmi  akan  dikeluarkan di atas pembayaran tersebut.\n\n");

		//            PrintText("Tawaran ini akan habis tempoh pada 14 hari sahaja. Jika bayaran penuh\n");
		//            PrintText("jumlah  wang  yang  dinyatakan  di  atas,  diterima pada atau sebelum\n");
		//            PrintText("penutupan  urusan   pada  tarikh  tersebut,   tiada  apa-apa  langkah\n");
		//            PrintText("perbicaraan selanjutnya akan  diambil  terhadap  tuan/puan  berhubung\n");
		//            PrintText("dengan  kesalahan  itu. Jika  tidak langkah-langkah akan diambil bagi\n");
		//            PrintText("menjalankan pendakwaan tanpa notis selanjutnya.\n\n");

		//            PrintText("Bertarikh pada: " + formatPrintDate + "\n\n");

		//            PrintImage(0);

		//            PrintText("                                         ----------------------------\n");
		//            PrintText("                                                Pegawai Berkuasa     \n");
		//            PrintText("                                       Majlis Bandaraya Petaling Jaya\n");

		//            PrintText("\n\n\n");

		//        }


		private void SetHeightBarcode()
		{
			Byte[] buffer = new Byte[] { 29, 104, 40 };
			for (int i = 0; i < buffer.Length; i++)
			{
				oStream.Write(buffer[i]);
			}
		}

		private void SendPrinterStatus()
		{
			Byte[] buffer = new Byte[] { 27, 118 };
			for (int i = 0; i < buffer.Length; i++)
			{
				oStream.Write(buffer[i]);
			}
			System.Threading.Thread.Sleep(2000);
		}

		private void InitialisePrinter()
		{
			Byte[] buffer = new Byte[] { 27, 64 };
			for (int i = 0; i < buffer.Length; i++)
			{
				oStream.Write(buffer[i]);
			}
			System.Threading.Thread.Sleep(2000);
		}
		private void SetWidthBarcode()
		{
			Byte[] buffer = new Byte[] { 29, 119, 4 };
			for (int i = 0; i < buffer.Length; i++)
			{
				oStream.Write(buffer[i]);
			}
		}

		private void SetFontDouble()
		{
			Byte[] buffer = new Byte[] { 29, 33, 16 };
			for (int i = 0; i < buffer.Length; i++)
			{
				oStream.Write(buffer[i]);
			}
		}

		private void SetDefaultLineSpacing()
		{
			Byte[] buffer = new Byte[] { 27, 50 };
			for (int i = 0; i < buffer.Length; i++)
			{
				oStream.Write(buffer[i]);
			}
		}

		private void SetLineSpacing(int space)
		{
			Byte[] buffer = new Byte[] { 27, 51, 1 };
			if (space > 0 && space < 255)
				buffer[2] = Convert.ToByte(space);

			for (int i = 0; i < buffer.Length; i++)
			{
				oStream.Write(buffer[i]);
			}
		}

		private void SetBarCodeHight(int nHeight)
		{
			Byte[] buffer = new Byte[] { 29, 104, 60 };
			if (nHeight > 0 && nHeight < 255)
				buffer[2] = Convert.ToByte(nHeight);

			for (int i = 0; i < buffer.Length; i++)
			{
				oStream.Write(buffer[i]);
			}
		}

		/// <summary>
		/// set font size , range 0 - 7
		/// 0 = normal
		/// </summary>
		private void SetFontSize(int size)
		{
			Byte[] buffer = new Byte[] { 29, 33, 0 };
			if (size < 0 || size > 7)
				size = 0;

			buffer[2] = Convert.ToByte(size);

			for (int i = 0; i < buffer.Length; i++)
			{
				oStream.Write(buffer[i]);
			}
		}

		private void SetPrintArea()
		{
			Byte[] buffer = new Byte[] { 29, 87, 0, 2 };

			for (int i = 0; i < buffer.Length; i++)
			{
				oStream.Write(buffer[i]);
			}
		}

		private void SetLeftMargin()
		{
			Byte[] buffer = new Byte[] { 29, 76, 0, 0 };

			for (int i = 0; i < buffer.Length; i++)
			{
				oStream.Write(buffer[i]);
			}
		}

		/// <summary>
		/// set font bold on/off
		/// true = on
		/// </summary>
		/// <param name="value"></param>
		private void SetBold(bool value)
		{

			Byte[] buffer = new Byte[] { 27, 69, 0 };

			if (value)
				buffer[2] = Convert.ToByte(1);

			for (int i = 0; i < buffer.Length; i++)
			{
				oStream.Write(buffer[i]);
			}
		}
		private void SetReadableOff()
		{
			Byte[] buffer = new Byte[] { 29, 72, 0 };
			for (int i = 0; i < buffer.Length; i++)
			{
				oStream.Write(buffer[i]);
			}
		}

		public void PrintTest()
		{
			SetFontSize(0);
			PrintText("abcdef ghijklmno Font Size 0");
			PrintLine();
			SetFontSize(1);
			PrintText("abcdef ghijklmno Font Size 1");
			PrintLine();
			SetFontSize(2);
			PrintText("abcdef ghijklmno Font Size 2");
			PrintLine();
			SetFontSize(3);
			PrintText("abcdef ghijklmno Font Size 3");
			PrintLine();
			//SetFontNormal(4);
			//PrintText("abcdef ghijklmno Font Size 4\n");
			PrintClose();
			//SetFontNormal(5);
			//PrintText("abcdef ghijklmno Font Size 5");
			//SetFontNormal(6);
			//PrintText("abcdef ghijklmno Font Size 6");
			//SetFontNormal(7);
			//PrintText("abcdef ghijklmno Font Size 7");
		}

		public void PrintTest2()
		{
			SetFontSize(0);
			SetBold(true);
			PrintText("abcdef ghijklmno Font Size 0");
			SetBold(false);
			PrintLine();
			PrintText("abcdef ghijklmno Font Size 0");
			PrintLine();
			PrintClose();


		}

		//public void PrintTestBarcode()
		//{
		//    PrintText("Start Print" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "\n\n");
		//    PrintLine(2);
		//    Print2DBarcode("043KK1B0037TestBarcode");
		//    PrintLine(2);
		//    PrintText("Finish");

		//    PrintClose();
		//}
		//public void PrintBarcode(string compoundNo)
		//{

		//    SetReadableOff();
		//    //SetHeightBarcode();

		//    Byte[] buffer = new Byte[] { 29, 107, 73, 13 };

		//    string data = compoundNo;//"043KK1B003750";
		//    byte[] bytesValue;
		//    bytesValue = Encoding.ASCII.GetBytes(data);

		//    var byteData = new Byte[buffer.Length + bytesValue.Length];
		//    Buffer.BlockCopy(buffer, 0, byteData, 0, buffer.Length);
		//    Buffer.BlockCopy(bytesValue, 0, byteData, buffer.Length, bytesValue.Length);

		//    for (int i = 0; i < byteData.Length; i++)
		//    {
		//        oStream.Write(byteData[i]);
		//    }


		//}

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

			for (int i = 0; i < byteData.Length; i++)
			{
				oStream.Write(byteData[i]);
			}


		}

		#region FormatOffDesc

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

		#endregion

		private void PrintImage(int imageNumber)
		{
			Byte[] PageMode = { 0x1b, 0x4c };
			Byte[] SetArea0 = { 0x1b, 0x57, 0x32, 0x02, 0x00, 0x00, 0x64, 0x02, 0x55, 0x00 };
			Byte[] SetArea1 = { 0x1b, 0x57, 0x64, 0x00, 0x00, 0x00, 0x64, 0x02, 0x00, 0x01 };
			Byte[] SetArea2 = { 0x1b, 0x57, 0x64, 0x00, 0x00, 0x00, 0x64, 0x02, 0x78, 0x00 };
			Byte[] SetArea3 = { 0x1b, 0x57, 0x64, 0x00, 0x00, 0x00, 0x64, 0x02, 0x00, 0x01 };
			Byte[] SetArea4 = { 0x1b, 0x57, 0x00, 0x00, 0x00, 0x00, 0x64, 0x02, 0x00, 0x01 };
			Byte[] firstImage = { 0x1b, 0x66, 0x00 };
			Byte[] secondImage = { 0x1b, 0x66, 0x01 };
			Byte[] thirdImage = { 0x1b, 0x66, 0x02 };
			Byte[] fourthImage = { 0x1b, 0x66, 0x03 };
			Byte[] fifthImage = { 0x1b, 0x66, 0x04 };

			Byte[] ModeChange = { 0x0c };

			PrintChar(PageMode);
			switch (imageNumber)
			{
				case 0:
					PrintChar(SetArea0);
					PrintChar(firstImage);
					break;
				case 1:
					PrintChar(SetArea1);
					PrintChar(secondImage);
					break;
				case 2:
					PrintChar(SetArea2);
					PrintChar(thirdImage);
					break;
				case 3:
					PrintChar(SetArea3);
					PrintChar(fourthImage);
					break;
				case 4:
					PrintChar(SetArea4);
					PrintChar(fifthImage);
					break;
			}
			PrintChar(ModeChange);

		}
	}
}