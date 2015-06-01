using System;
using System.IO.Ports;
using System.Reactive.Threading.Tasks;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace More.Net.Channels.Ports
{
    /// <summary>
    /// 
    /// </summary>
    public class SerialChannel : ISerialChannel
    {
        #region Constructors

        /// <summary>
        /// Creates a new serial port channel on the specified port.
        /// </summary>
        /// <param name="port"></param>
        public SerialChannel(String port)
        {
            serialPort = new SerialPort(port);
        }

        #endregion

        public static IObservable<SerialChannel> Discover()
        {
            return SerialPort
                .GetPortNames()
                .ToObservable()
                .Select(port => new SerialChannel(port));
        }

        #region ISerialChannel

        #region IChannel

        #region IDisposable

        ///// <summary>
        ///// 
        ///// </summary>
        //void IDisposable.Dispose()
        //{
        //    DisconnectAsync().Wait();
        //}

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public Boolean CanRead
        {
            get { return serialPort.BaseStream.CanRead; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean CanWrite
        {
            get { return serialPort.BaseStream.CanWrite; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean IsConnected
        {
            get { return serialPort.IsOpen; }
        }

        /// <summary>
        /// Asynchronously opens the serial port.  
        /// </summary>
        /// <returns>
        /// True if the port is successfully opened for use.
        /// </returns>
        /// <exception cref="System.UnauthorizedAccessException">
        /// Access is denied to the port.  The current process or another process on the system 
        /// already has the specified COM port open either by a System.IO.Ports.SerialPort 
        /// instance or in unmanaged code.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// One or more of the properties for this instance are invalid. For example, the Parity, 
        /// DataBits, or Handshake properties are not valid values;
        /// the BaudRate is less than or equal to zero; 
        /// the ReadTimeout or WriteTimeout property is less than zero and is not InfiniteTimeout.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The port name does not begin with "COM", or the file type of the port is not supported.
        /// </exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        public async Task<Boolean> ConnectAsync()
        {
            if (serialPort.IsOpen == false)
                serialPort.Open();
            return await Task
                .FromResult(serialPort.IsOpen == true)
                .ConfigureAwait(false);
        }

        public IObservable<System.Reactive.Unit> CloseAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Asynchronously closes the port.  Additionally disposes the underlying stream.
        /// </summary>
        /// <returns>
        /// True if the channel is successfully closed.
        /// </returns>
        /// <exception cref="System.IO.IOException"></exception>
        public async Task<Boolean> DisconnectAsync()
        {
            if (serialPort.IsOpen == true)
                serialPort.Close();
            return await Task
                .FromResult(serialPort.IsOpen == false)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// Buffer is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Offset or count is negative.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The sum of offset and count is larger than the buffer length.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// The stream is currently in use by a previous read operation.
        /// </exception>
        public async Task<Int32> ReadAsync(Byte[] buffer, Int32 offset, Int32 count)
        {
            //return await Observable
            //    .FromEventPattern<SerialDataReceivedEventHandler, DataRecievedEventArgs>(
            //        handler => serialPort.DataReceived += handler,
            //        handler => serialPort.DataReceived -= handler)
            //    .FirstAsync()
            //    .SelectMany(_ => serialPort.BaseStream.ReadAsync(buffer, offset, count));

            return await serialPort.BaseStream
                .ReadAsync(buffer, offset, count)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<Int32> WriteAsync(Byte[] buffer, Int32 offset, Int32 count)
        {
            await serialPort.BaseStream
                .WriteAsync(buffer, offset, count)
                .ConfigureAwait(false);
            return count;
        }

        #endregion

        /// <summary>
        /// Gets or sets the baud rate.
        /// </summary>
        public Int32 Baudrate
        {
            get { return serialPort.BaudRate; }
            set { serialPort.BaudRate = value; }
        }

        /// <summary>
        /// Gets or sets the number of bits in a byte.
        /// </summary>
        public Int32 DataBits
        {
            get { return serialPort.DataBits; }
            set { serialPort.DataBits = value; }
        }

        /// <summary>
        /// Gets the port the channel observes.
        /// </summary>
        public String Port
        {
            get { return serialPort.PortName; }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return Port;
        }

        #endregion

        #region Private Fields

        private readonly SerialPort serialPort;

        #endregion


        IObservable<IChannelStream> IChannel.ConnectAsync()
        {
            throw new NotImplementedException();
        }
    }
}
