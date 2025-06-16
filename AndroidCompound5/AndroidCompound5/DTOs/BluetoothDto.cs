using System.Collections.Generic;
using Android.Bluetooth;

namespace AndroidCompound5.DTOs
{
    public class BluetoothDto
    {
    }

    public class ResponseBluetoothAndroid
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ResponseBluetoothAndroid"/> is succes.
        /// </summary>
        /// <value>
        ///   <c>true</c> if succes; otherwise, <c>false</c>.
        /// </value>
        public bool Succes { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseBluetoothAndroid"/> class.
        /// </summary>
        public ResponseBluetoothAndroid()
        {
            Succes = true;
            Message = "Success";
        }
    }

    public class ResponseBluetoothDevices : ResponseBluetoothAndroid
    {
        /// <summary>
        /// Gets or sets the devices.
        /// </summary>
        /// <value>
        /// The devices.
        /// </value>
        public List<BluetoothDevice> Devices { get; set; }

        public ResponseBluetoothDevices()
        {
            Devices = new List<BluetoothDevice>();
        }
    }
}