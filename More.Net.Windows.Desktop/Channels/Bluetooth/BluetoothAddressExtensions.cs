using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InTheHandBluetoothAddress = InTheHand.Net.BluetoothAddress;

namespace More.Net.Channels.Bluetooth
{
    internal static class BluetoothAddressExtensions
    {
        public static InTheHandBluetoothAddress ToInTheHandBluetoothAddress(
            this BluetoothAddress bluetoothAddress)
        {
            return new InTheHandBluetoothAddress(bluetoothAddress.ToByteArray());
        }
    }
}
