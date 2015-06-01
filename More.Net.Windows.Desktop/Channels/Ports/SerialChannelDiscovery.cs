using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using More.Net.Linq;
using log4net;

namespace More.Net.Channels.Ports
{
    /// <summary>
    /// 
    /// </summary>
    public class SerialChannelDiscovery// : IChannelDiscovery<ISerialChannel>
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public SerialChannelDiscovery()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        static SerialChannelDiscovery()
        {
            Logger = LogManager.GetLogger(typeof(SerialChannelDiscovery));
        }
        private static readonly ILog Logger;

        #endregion

        #region ISerialChannelFactory

        #region IDisposable

        public void Dispose()
        {
            
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IObservable<ISerialChannel> Discover()
        {
            return System.IO.Ports.SerialPort
                .GetPortNames()
                .ToObservable()
                .Select(port => new SerialChannel(port))
                .Do(port => Logger.InfoFormat(
                    "Serial Port Discovered.{3}    Port: {0}{3}    Open: {1}",
                    port.Port,
                    port.IsConnected,
                    port.Baudrate,
                    Environment.NewLine));
        }

        #endregion
    }
}
