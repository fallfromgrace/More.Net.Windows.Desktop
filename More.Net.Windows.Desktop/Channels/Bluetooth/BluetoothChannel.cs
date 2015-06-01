using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using log4net;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace More.Net.Channels.Bluetooth
{
    /// <summary>
    /// 
    /// </summary>
    public class BluetoothChannel// : IBluetoothChannel
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        static BluetoothChannel()
        {
            Logger = new BluetoothChannelLogger();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="pin"></param>
        public BluetoothChannel(BluetoothAddress address) : 
            this(new BluetoothDeviceInfo(address.ToInTheHandBluetoothAddress()))
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="pin"></param>
        internal BluetoothChannel(BluetoothDeviceInfo deviceInfo)
        {
            this.canRead = false;
            this.canWrite = false;
            this.deviceInfo = deviceInfo;
            this.endPoint = new BluetoothEndPoint(
                deviceInfo.DeviceAddress,
                BluetoothService.SerialPort);
        }

        #endregion

        #region IBluetoothChannel

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
        public Boolean IsConnected
        {
            get { return client != null ? client.Connected : false; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean CanRead
        {
            get { return canRead; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean CanWrite
        {
            get { return canWrite; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<Boolean> ConnectAsync()
        {
            Boolean connected = false;

            try
            {
                Logger.OnConnectionBegin(deviceInfo);
                this.client = new BluetoothClient();
                await client
                    .ConnectAsync(endPoint)
                    .ConfigureAwait(false);
                connected = client.Connected;

                if (connected)
                {
                    this.networkStream = client.GetStream();
                    this.canRead = this.networkStream.CanRead;
                    this.canWrite = this.networkStream.CanWrite;
                }
                else
                {
                    this.client.Dispose();
                }
                Logger.OnConnectionCompleted(deviceInfo);
            }
            catch (Exception ex)
            {
                Logger.OnConnectionCompleted(deviceInfo, ex);
                this.client.Dispose();
                throw;
            }

            return connected;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<Boolean> DisconnectAsync()
        {
            if (client.Connected)
                client.Close();
            return await Task
                .FromResult(client.Connected == false)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<Int32> ReadAsync(Byte[] buffer, Int32 offset, Int32 count)
        {
            Int32 bytesRead = 0;
            try
            {
                Logger.OnReadBegin(deviceInfo);
                if (this.canRead == false)
                    throw new InvalidOperationException();

                this.canRead = false;
                bytesRead = await networkStream
                    .ReadAsync(buffer, offset, count)
                    .ConfigureAwait(false);
                this.canRead = true;
                Logger.OnReadCompleted(deviceInfo, buffer, offset, bytesRead);
            }
            catch (Exception ex)
            {
                Logger.OnReadCompleted(deviceInfo, ex);
                throw;
            }
            return bytesRead;
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
            if (this.canWrite == false)
                throw new InvalidOperationException();

            this.canWrite = false;
            await networkStream
                .WriteAsync(buffer, offset, count)
                .ConfigureAwait(false);
            this.canWrite = true;
            return count;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public BluetoothAddress Address
        {
            get { return deviceInfo.DeviceAddress.ToBluetoothAddress(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean IsAuthenticated
        {
            get { return deviceInfo.Authenticated; }
        }

        /// <summary>
        /// 
        /// </summary>
        public String Name
        {
            get { return deviceInfo.DeviceName; }
        }

        /// <summary>
        /// Asynchronously attempts to pair the local device with the remote device.
        /// </summary>
        /// <param name="pin"></param>
        /// <returns></returns>
        /// <remarks>
        /// This is not actually asynchronous - The pair request is created using Task.Run().  
        /// This is exposed as an asynchronous method, however, to keep symmetry with the other 
        /// methods.
        /// </remarks>
        public async Task<Boolean> AuthenticateAsync(String pin)
        {
            deviceInfo.Refresh();
            Logger.OnAuthenticationBegin(pin, deviceInfo);
            if (deviceInfo.Authenticated == false)
                await Task
                    .Run(() => BluetoothSecurity.PairRequest(endPoint.Address, pin))
                    .ConfigureAwait(false);
            deviceInfo.Refresh();
            Logger.OnAuthenticationCompleted(pin, deviceInfo);
            return deviceInfo.Authenticated;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<Double> GetRssiAsync()
        {
            throw new NotSupportedException();
        }

        #endregion

        #region Overrides

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return Address.ToString();
        }

        #endregion

        #region Private Fields

        private Boolean canRead;
        private Boolean canWrite;
        private BluetoothClient client;
        private NetworkStream networkStream;
        private readonly BluetoothDeviceInfo deviceInfo;
        private readonly BluetoothEndPoint endPoint;
        private static readonly BluetoothChannelLogger Logger;

        #endregion
    }
}
