using Android.Bluetooth;
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
	public class BluetoothAndroid
	{
		private BluetoothSocket socket;
		private BluetoothAdapter adapter;
		private PrintWriter oStream;
		public List<BluetoothDevice> _listDevice;

		public BluetoothAndroid()
		{
			_listDevice = new List<BluetoothDevice>();
		}

		/// <summary>
		/// Checks the adapter.
		/// </summary>
		/// <param name="adapter">The adapter.</param>
		/// <returns></returns>
		private bool CheckAdapterIsNull(BluetoothAdapter adapter)
		{
			return adapter == null;
		}

		/// <summary>
		/// Bluetoothes the open.
		/// </summary>
		public ResponseBluetoothAndroid BluetoothOpen()
		{
			var response = new ResponseBluetoothAndroid();

			// Check if the bletooth adapter is null
			adapter = BluetoothAdapter.DefaultAdapter;
			if (CheckAdapterIsNull(adapter))
			{
				response.Succes = false;
				response.Message = "No bluetooth adapter available.";
			}

			if (!adapter.IsEnabled)
			{
				adapter.Enable();
			}

			return response;
		}

		/// <summary>
		/// Bluetoothes the scan.
		/// </summary>
		/// <returns></returns>
		public int BluetoothScan()
		{
			_listDevice = new List<BluetoothDevice>();

			// Check adapter
			if (CheckAdapterIsNull(adapter))
			{
				return 0;
			}

			// Find object or Bluetooth device are paired with the local adapter
			var devices = adapter.BondedDevices;
			if (!devices.Any())
			{
				return 0;
			}

			foreach (var device in devices)
			{
				_listDevice.Add(device);
			}

			return _listDevice.Count();
		}

		/// <summary>
		/// Bluetoothes the connect.
		/// </summary>
		/// <param name="bluetoothDevice">The bluetooth device.</param>
		public ResponseBluetoothDevices BluetoothConnect(BluetoothDevice bluetoothDevice)
		{
			var response = new ResponseBluetoothDevices();

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

				response.Succes = true;
				response.Message = string.Format("Bluetooth conected to {0}", bluetoothDevice.Name);
			}
			catch (Exception ex)
			{
				response.Succes = false;
				response.Message = string.Format("Can not connect to device, please check your device. {0}", ex.Message);
				return response;
			}

			return response;
		}

		/// <summary>
		/// Bluetoothes the send.
		/// </summary>
		/// <param name="bytes">The bytes.</param>
		public ResponseBluetoothAndroid BluetoothSendChar(byte[] buffer)
		{
			var response = new ResponseBluetoothAndroid();
			try
			{
				if (socket == null)
				{
					response.Succes = false;
					response.Message = "Please Connect to device & retry again.";
					return response;
				}

				oStream = new PrintWriter(socket.OutputStream, true);
				for (int i = 0; i < buffer.Length; i++)
				{
					oStream.Write(buffer[i]);
				}
				;
			}
			catch (Exception ex)
			{
				response.Succes = false;
				response.Message = string.Format("Please check you printer, printer error reported: {0}", ex.Message);
				return response;
			}
			return response;
		}

		/// <summary>
		/// Bluetoothes the send.
		/// </summary>
		/// <param name="text">The text.</param>
		public ResponseBluetoothAndroid BluetoothSendText(string text)
		{
			var response = new ResponseBluetoothAndroid();
			try
			{
				if (socket == null)
				{
					response.Succes = false;
					response.Message = "Please Connect to device & retry again.";
					return response;
				}
				oStream = new PrintWriter(socket.OutputStream, true);
				oStream.Write(text);
				oStream.Flush();

				// diconect
				BluetoothDisconnect();
			}
			catch (Exception ex)
			{
				response.Succes = false;
				response.Message = string.Format("Please check you printer, printer error reported: {0}", ex.Message);
				return response;
			}
			return response;
		}

		/// <summary>
		/// Bluetoothes the disconnect.
		/// </summary>
		public void BluetoothDisconnect()
		{
			oStream.Close();
			oStream.Dispose();

			if (socket.IsConnected)
			{
				socket.Close();
			}
		}

		/// <summary>
		/// Bluetoothes the close.
		/// </summary>
		public void BluetoothClose()
		{
			if (socket.IsConnected)
			{
				BluetoothDisconnect();
				socket.Dispose();
			}
		}
	}
}