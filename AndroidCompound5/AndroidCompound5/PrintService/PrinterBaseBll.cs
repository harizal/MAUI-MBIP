using Android.Bluetooth;
using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.Classes;
using Java.Util;
using System.Text;

namespace AndroidCompound5.PrintService
{
	public class PrinterBaseBll
	{
		private const string ClassName = "PrinterBaseBll";

		public BluetoothSocket socket;
		public BluetoothAdapter adapter;
		public List<BluetoothDevice> _listDevice;
		private byte[] _printerResponse = new byte[1024];

		Byte[] FontNormal = new Byte[3] { 27, 33, 0 }; //48 columns                0+

		//not applicable to VS2022
		//public PrintWriter oStream;

		public PrinterBaseBll()
		{

			//oStream = null;
			//adapter = BluetoothAdapter.DefaultAdapter;
			adapter = GeneralAndroidClass.GetBluetoothAdapter();

			//Auto assign valid bluetooth socket for printing
			if (GlobalClass.BluetoothSocket != null)
			{
				if (GlobalClass.BluetoothSocket.IsConnected)
					socket = GlobalClass.BluetoothSocket;
				else
					GlobalClass.BluetoothSocket = null;
			}

			if (!adapter.IsEnabled)
			{
				adapter.Enable();
			}

#if Printer
            Log.WritePrintIntoFile("PrintBll() - Enable Bluetooth Adapter.");
#endif
		}

		public bool CheckAdapterIsNull(BluetoothAdapter adapter)
		{
			return adapter == null;
		}

		public ResponseBluetoothDevices BluetoothConnect(BluetoothDevice bluetoothDevice)
		{
			var response = new ResponseBluetoothDevices();
#if PrintFile
            response.Succes = true;
            return response;
#endif
			int retries = 0;
			bool isConnected = false;
			string message = "";

			do
			{
				try
				{
					//Check buletooth device 
					if (bluetoothDevice == null)
					{
						response.Succes = false;
						response.Message = "No bluetooth device";
						return response;
					}

					// Check adapter
					if (CheckAdapterIsNull(adapter))
					{
						// If the adapter is null.
						response.Succes = false;
						response.Message = "No bluetooth adapter available.";
						return response;
					}

					if (socket != null && socket.IsConnected)
					{
						response.Succes = true;
						response.Message = string.Format("Bluetooth conected to {0}", bluetoothDevice.Name);
						return response;
					}

#if Printer
                    Log.WritePrintIntoFile($"PrintBll.BluetoothConnect() - Responsed Device {bluetoothDevice.Name}: {response.ToString()}, Retries: {retries}");
#endif
					socket = bluetoothDevice.CreateRfcommSocketToServiceRecord(
						UUID.FromString(bluetoothDevice.GetUuids().ElementAt(0).ToString()));
					if (socket.IsConnected)
						socket.Close();

					//Android 12 required slight delay before connect
					Thread.Sleep(200);
#if Printer
                    DateTime startConnect = DateTime.Now;
#endif
					//make Tx/Rx streaming socket
					socket.Connect();

#if Printer
                    DateTime endConnect = DateTime.Now;
                    TimeSpan timeDiff = endConnect - startConnect;
                    Log.WritePrintIntoFile($"PrintBll.BluetoothConnect() - Device {bluetoothDevice.Name} RFcomm Socket Connected: {socket.ToString()}");
                    Log.WritePrintIntoFile($"PrintBll.BluetoothConnect() - Device {bluetoothDevice.Name} Socket.Connected Time Taken: {timeDiff.TotalMilliseconds.ToString() + "ms"} Retries: {retries}");
#endif
					//not applicable to VS2022
					//oStream = new PrintWriter(socket.OutputStream, true);

#if Printer
                    Log.WritePrintIntoFile($"PrintBll.BluetoothConnect() - Device {bluetoothDevice.Name} Text input/output streams ready, Retries: {retries}");
#endif

					response.Succes = true;
					response.Message = string.Format("Bluetooth conected to {0}", bluetoothDevice.Name);

					isConnected = true;
					GlobalClass.BluetoothSocket = socket;
				}
				catch (Exception ex)
				{
#if Printer
                    Log.WritePrintIntoFile($"PrintBll.BluetoothConnect() - Device {bluetoothDevice.Name} unable to connect. Retries: {retries}");

#endif
					message = ex.Message;
					retries++;
					//Thread.Sleep(100);
				}

			} while (isConnected == false && retries < Constants.MaxPrintRetry);

			if (isConnected == false)
			{
#if Printer
                Log.WritePrintIntoFile($"PrintBll.BluetoothConnect() - Device {bluetoothDevice.Name} connection failed after {Constants.MaxPrintRetry} retries");
#endif
				response.Succes = false;
				response.Message = "Can not connect to device, please check your printer.";
				if (message.Length > 0)
					response.Message += "\n" + message;

				//May selected wrong Bluetooth device, reset to null in order to allow reselect other printer
				GlobalClass.BluetoothDevice = null;
				return response;
			}

			return response;
		}

		public bool PrintTextData(string text)
		{
			bool isDone = false;

#if PrintFile //todo remove when prod
            //default WritePrintIntoFile() auto insert lineFeed(LF) into the log record.
            //remove the extra linefeed
            if (text.Length > 0 && text.Substring(text.Length - 1, 1) == "\n")
                text = text.Substring(0, text.Length - 1);
            Log.WritePrintIntoFile(text);
            return true;
#endif
			if (socket.IsConnected == false)
			{
				throw new Exception("PrintTextData() Socket disconnected");
			}

			try
			{
				if (text.Length > 0)
				{
					var buffer = Encoding.UTF8.GetBytes(text);
					socket.OutputStream.Write(buffer, 0, buffer.Length);
					socket.OutputStream.Flush();
					//oStream.Write(text);
					//oStream.Flush();
					isDone = true;
				}
				else
				{
#if Printer
                    Log.WritePrintIntoFile($"PrintBll.PrintTextData warning: null/empty text to print.");
#endif
				}
			}
			catch (Exception ex)
			{
				_printerMessage = ex.Message;
#if Printer
                Log.WritePrintIntoFile($"PrintBll.PrintTextData error: {_printerMessage}");
#endif
				LogFile.WriteLogFile(ClassName, "PrintTextData", _printerMessage, Enums.LogType.Error);
			}
			return isDone;
		}

		public bool PrintChar(byte[] buffer)
		{
			//set default delay to 100 milliseconds
			return PrintChar(buffer, 50);
		}

		public bool PrintChar(byte[] buffer, int Delay)
		{
			bool isDone = false;
#if Printer
            if (buffer == null)
                Log.WritePrintIntoFile($"PrintBll.PrintChar(bytes[]) - buffer is [NULL].");
            else
                Log.WritePrintIntoFile($"PrintBll.PrintChar(bytes[]) - {buffer.Length} bytes sent.");
#endif

			//int li_out = 0;
			int li_DataCount = 0;

			//Skip printing after error detected, avoid program keep waiting to print & looked like hanged.
			//if (mb_IsPrintError == true)
			//    return 0;

			//if (buffer != null && buffer.Length > 0)
			if (buffer != null)
				li_DataCount = buffer.Length;

			if (li_DataCount > 0)
			{
				try
				{
					isDone = PrintCharData(li_DataCount, buffer, Delay);
				}
				catch (Exception exc)
				{
					//PrintException(exc);
					LogFile.WriteLogFile(ClassName, "PrintChar",
									 exc.Message, Enums.LogType.Info);
				}
			}
			return isDone;
		}

		public bool PrintCharData(int li_DataCount, byte[] buffer, int Delay)
		{
			bool isDone = false;

#if PrintFile //todo remove when prod

            return true;
#endif
			int i = 0, Len = 0;
			int size = li_DataCount;
			try
			{
				//loop for LF & print
				for (int j = i; j < li_DataCount; j++)
				{
					if (buffer[j] == 0x0A)
					{
						Len = (j - i) + 1;
						socket.OutputStream.Write(buffer, i, Len);
						size -= Len;
						i = j + 1;
						Thread.Sleep(Delay);
					}
				}
				//Print the balance buffer if no LF found
				if (size > 0)
				{
					socket.OutputStream.Write(buffer, i, size);
					size = 0;
				}
				isDone = true;
			}
			catch (Exception ex)
			{
				_printerMessage = ex.Message;

#if Printer
                Log.WritePrintIntoFile($"PrintBll.PrintCharData error: {_printerMessage}");
#endif
				LogFile.WriteLogFile(ClassName, "PrintCharData", _printerMessage, Enums.LogType.Error);
			}
			return isDone;
		}

		protected int ReadChar(int Delay)
		{
#if Printer
            Log.WritePrintIntoFile($"PrintBll.ReadChar() looping for incoming data from printer.");
#endif

			byte[] buffer = new byte[1024];
			int bytes = 0, retries = 0;
			_printerResponse = new byte[1024];  //clean previous response data.
			do
			{
				try
				{
					#region Codes working fine in VS2017 but caused socket closed after compiled with VS2022
					//Unblock READ() from InputStream
					//using (var ist = socket.InputStream)
					//{
					//    var _ist = (ist as InputStreamInvoker).BaseInputStream;
					//    var aa = 0;
					//    if ((aa = _ist.Available()) > 0)
					//    {
					//        bytes = _ist.Read(buffer, 0, aa);
					//        System.Array.Resize(ref buffer, bytes);
					//    }
					//}
					// Blocked READ() from the InputStream
					#endregion Codes working fine in VS2017 but caused socket closed after compiled with VS2022

					#region new codes for VS2022
					if (socket.InputStream.IsDataAvailable())
					{
						//int lenData = (int) socket.InputStream.Length;
						bytes = socket.InputStream.Read(buffer, 0, buffer.Length - 1);
					}
					#endregion new codes for VS2022

					if (bytes > 0)
					{
						// buffer can be over-written by next input stream data, so it should be copied
						_printerResponse = Arrays.CopyOf(buffer, bytes);
#if Printer
                        Log.WritePrintIntoFile($"PrintBll.ReadChar() Input Stream received data. Data Length: {bytes} after {retries} retries.");
                        Log.WritePrintIntoFile(_printerResponse);
#endif
						break;
					}

					Thread.Sleep(Delay);
					retries++;

					//// Send the obtained bytes to the UI Activity
					//service.handler.ObtainMessage(MainActivity.MESSAGE_READ, bytes, -1, rcvData).SendToTarget();
				}
				catch (Java.IO.IOException e)
				{
#if Printer
                    Log.WritePrintIntoFile($"PrintBll.ReadChar() Java.IO.IOException: { e.Message}");
#endif
					LogFile.WriteLogFile(ClassName, "ReadChar()", e.Message, Enums.LogType.Error);
					break;
				}
				catch (Exception ex)
				{
#if Printer
                    Log.WritePrintIntoFile($"PrintBll.ReadChar() Exception: {ex.Message}");
#endif
					LogFile.WriteLogFile(ClassName, "ReadChar()", ex.Message, Enums.LogType.Error);
					break;
				}

			} while (bytes == 0 && retries <= 10);

			if (bytes == 0)
			{
#if Printer
                Log.WritePrintIntoFile($"PrintBll.ReadChar() no data from InputStream after {retries} retries {(retries * Delay).ToString()}ms .");
#endif
			}
			return bytes;
		}

		public byte[] ReadCharData()
		{
			return _printerResponse;
		}

		protected string _printerMessage = "";
		protected int _printerStatus = 0;
		protected bool isBlackMarkOn = false;                 //later put this into printer setting control parameters

		public string GetBluetoothDeviceName()
		{
			if (!adapter.IsEnabled)
			{
#if Printer
                Log.WritePrintIntoFile("PrintBll.GetBluetoothDeviceName() - Bluetooth Adapter is disabled.");
#endif
				return "";
			}

#if Printer
            Log.WritePrintIntoFile($"PrintBll.GetBluetoothDeviceName() - Bluetooth Adapter name: {adapter.Name}.");
#endif

			return (adapter.Name);
		}

		public string GetPrinterMessage()
		{
			return _printerMessage;
		}

		public virtual void PrintInitialise()
		{
			//Woosim printer initialize codes
			Byte[] prnInitialize = new Byte[3] { 27, 64, 10 }; //Printer reset + LF
			PrintChar(prnInitialize);
		}

		public virtual int PrinterQuery()
		{
#if PrintFile
            return 0;
#endif
			//initialized printer status
			_printerMessage = "";
			_printerStatus = 0;

			//query printer status
			PrintChar(new Byte[2] { 27, 118 });
			//Thread.Sleep(1000);
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
						_printerMessage += "\n";
					_printerMessage += "Cover is opened";
					_printerStatus = 1;
				}
				//No blackmark sensor detection required
				if (isBlackMarkOn && (resp[0] & 0x04) == 0x4)        //BIT 2  = 2^2
				{
					if (_printerMessage.Length > 0)
						_printerMessage += "\n";
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
				if (_printerMessage.Length > 0)
					_printerMessage += "\n";
				_printerMessage += "Printer no response";
				_printerStatus = -1;
			}
			return _printerStatus;
		}

		public virtual int PrinterFirmware()
		{
			//initialized printer status
			_printerMessage = "";
			_printerStatus = 0;

			//query printer firmware
			byte[] cmd = { 0x1B, 0x00, 0x02, 0x02 };
			PrintChar(cmd);
			int bytes = ReadChar(200);
			var resp = ReadCharData();
			if (bytes > 0)
			{
				GlobalClass.FwCode = GeneralBll.ProcessRcvData(resp);
				_printerStatus = 1;
			}
			else
			{
				GlobalClass.FwCode = "";
			}
			return _printerStatus;
		}
		public int PrintClose()
		{
#if PrintFile
            return 0;
#endif
			//int status = PrinterQuery();
			int status = 0;

#if Printer
            Log.WritePrintIntoFile($"PrintBll.PrintClose() Printer Status: {_printerStatus} - {_printerMessage}.");
            Log.WritePrintIntoFile($"PrintBll.PrintClose() Output Stream Closed, Socket Closed.");
#endif

			//not applicable to VS2022
			//oStream.Close();
			//oStream.Dispose();

			//if (socket.IsConnected)
			//{
			//    socket.Close();
			//}

			//Printer no response
			if (status == -1)
			{
				LogFile.WriteLogFile($"{ClassName}.PrintClose({GlobalClass.BluetoothDevice.Name}) Printer Status: {_printerStatus} - {_printerMessage}.");
			}

			return status;
		}

		public bool PrintData(PrintDataItem data)
		{
			bool isOK = false;
			try
			{
				if (data.IsByte)
				{
					var lengthBuffer = data.Byte.Length;
					isOK = PrintCharData(lengthBuffer, data.Byte, 50);
				}
				else
				{
					isOK = PrintTextData(data.Text);
				}
			}
			catch (Exception exc)
			{
				LogFile.WriteLogFile(ClassName, "PrintBufferData",
									 exc.Message, Enums.LogType.Error);
			}
			return isOK;
		}
	}
}