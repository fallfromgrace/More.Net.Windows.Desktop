using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InTheHand.Net.Bluetooth;
using System.Reactive.Linq;
using InTheHand.Net.Sockets;
using log4net;

namespace More.Net.Channels.Bluetooth
{
    /// <summary>
    /// 
    /// </summary>
    public class BluetoothChannelDiscovery// : IBluetoothChannelDiscovery
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public BluetoothChannelDiscovery()
        {
            bluetoothComponent = new BluetoothComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        static BluetoothChannelDiscovery()
        {
            Logger = LogManager.GetLogger(typeof(BluetoothChannelDiscovery));
        }
        private static readonly ILog Logger;

        #endregion

        #region IBluetoothChannelFactory

        #region IDisposable

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            bluetoothComponent.Dispose();
        }

        #endregion

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public IObservable<IBluetoothChannel> Discover()
        //{
        //    return bluetoothComponent
        //        .WhenDeviceDiscovered(false, false, false, true)
        //        .Do(deviceInfo => Logger.InfoFormat(
        //            "Bluetooth device discovered.{4}  Name: {0}{4}  Address: {1}{4}  Authenticated: {2}{4}  Connected: {3}", 
        //            deviceInfo.DeviceName, 
        //            deviceInfo.DeviceAddress, 
        //            deviceInfo.Authenticated,
        //            deviceInfo.Connected,
        //            Environment.NewLine))
        //        .Select(deviceInfo => new BluetoothChannel(deviceInfo));
        //}

        #endregion

        #region Private Fields

        private readonly BluetoothComponent bluetoothComponent;

        #endregion
    }
}
