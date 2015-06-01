//using System;
//using System.Net;
//using System.Net.Sockets;
//using System.Reactive.Linq;
//using System.Reactive.Threading.Tasks;
//using System.Threading.Tasks;

//namespace More.Net.Channels.Net
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    internal class TcpChannel : ITcpChannel
//    {
//        #region Constructors

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="hostName"></param>
//        /// <param name="port"></param>
//        public TcpChannel(TcpClient tcpClient)
//        {
//            this.tcpClient = tcpClient;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="address"></param>
//        /// <returns></returns>
//        public static IObservable<TcpChannel> ListenOn(IPEndPoint address)
//        {
//            TcpListener listener = new TcpListener(address);
//            listener.Start();
//            return Observable
//                .Defer(() => listener.AcceptTcpClientAsync().ToObservable())
//                .Repeat()
//                .Select(tcpClient => new TcpChannel(tcpClient));
//        }

//        #endregion

//        #region IChannel

//        #region IDisposable

//        /// <summary>
//        /// 
//        /// </summary>
//        void IDisposable.Dispose()
//        {
//            CloseAsync().Wait();
//        }

//        #endregion


//        public bool CanRead
//        {
//            get { throw new NotImplementedException(); }
//        }

//        public bool CanWrite
//        {
//            get { throw new NotImplementedException(); }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public Boolean IsOpen
//        {
//            get { return tcpClient.Connected; }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public async Task<Boolean> OpenAsync()
//        {
//            if (tcpClient.Connected == false)
//                await tcpClient
//                    .ConnectAsync(hostName, port)
//                    .ConfigureAwait(false);
//            networkStream = tcpClient.GetStream();
//            return tcpClient.Connected == true;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public async Task<Boolean> CloseAsync()
//        {
//            tcpClient.Close();
//            return await Task
//                .FromResult(tcpClient.Connected == false)
//                .ConfigureAwait(false);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="buffer"></param>
//        /// <param name="offset"></param>
//        /// <param name="count"></param>
//        /// <returns></returns>
//        public async Task<Int32> ReadAsync(Byte[] buffer, Int32 offset, Int32 count)
//        {
//            Int32 bytesRead = 0;
//            if (tcpClient.Connected == true)
//                bytesRead = await networkStream
//                    .ReadAsync(buffer, offset, count)
//                    .ConfigureAwait(false);
//            return bytesRead;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="buffer"></param>
//        /// <param name="offset"></param>
//        /// <param name="count"></param>
//        /// <returns></returns>
//        public async Task<Int32> WriteAsync(Byte[] buffer, Int32 offset, Int32 count)
//        {
//            Int32 bytesWrittenAndFlushed = 0;
//            if (tcpClient.Connected == true)
//            {
//                await networkStream
//                    .WriteAsync(buffer, offset, count)
//                    .ConfigureAwait(false);
//                bytesWrittenAndFlushed = count;
//            }

//            return bytesWrittenAndFlushed;
//        }

//        #endregion

//        #region Private Fields

//        private NetworkStream networkStream;

//        private readonly String hostName;
//        private readonly Int32 port;
//        private readonly TcpClient tcpClient;

//        #endregion

//        public IIPChannelEndPoint RemoteEndPoint
//        {
//            get { throw new NotImplementedException(); }
//        }

//        public IIPChannelEndPoint LocalEndPoint
//        {
//            get { throw new NotImplementedException(); }
//        }
//    }
//}
