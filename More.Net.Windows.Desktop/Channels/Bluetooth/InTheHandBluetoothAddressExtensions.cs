using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InTheHandBluetoothAddress = InTheHand.Net.BluetoothAddress;

namespace More.Net.Channels.Bluetooth
{
    /// <summary>
    /// 
    /// </summary>
    internal static class InTheHandBluetoothAddressExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bluetoothAddress"></param>
        /// <returns></returns>
        public static BluetoothAddress ToBluetoothAddress(
            this InTheHandBluetoothAddress bluetoothAddress)
        {
            return BluetoothAddress.FromBytes(bluetoothAddress.ToByteArray().Take(6).Reverse());
        }
    }
}
