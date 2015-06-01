using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using More.Net.Reactive.Linq;

namespace More.Net.Channels.Experimental
{
    /// <summary>
    /// Adapter for a System.Net.Sockets.NetworkStream.
    /// </summary>
    public class NetworkStreamChannelStream : IChannelStream
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of NetworkStreamChannelStream with the specified 
        /// System.Net.Sockets.NetworkStream.
        /// </summary>
        /// <param name="networkStream"></param>
        public NetworkStreamChannelStream(
            NetworkStream networkStream) : 
            this(networkStream, 4096)
        {

        }

        /// <summary>
        /// Initializes a new instance of NetworkStreamChannelStream with the specified 
        /// System.Net.Sockets.NetworkStream and read buffer size.
        /// </summary>
        /// <param name="networkStream"></param>
        /// <param name="readBufferSize"></param>
        public NetworkStreamChannelStream(
            NetworkStream networkStream, 
            Int32 readBufferSize)
        {
            this.networkStream = networkStream;
            this.readBuffer = new Byte[readBufferSize];
        }

        #endregion

        #region IReadStream

        /// <summary>
        /// 
        /// </summary>
        public Boolean CanRead
        {
            get { return networkStream.CanRead; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 ReadBufferSize
        {
            get { return readBuffer.Length; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// The result does not copy the contents from the internal buffer.  Therefore, if another 
        /// read occurs before consuming the result, the result will silently become invalid.
        /// </remarks>
        public IObservable<IReadOnlyList<Byte>> ReadAsync()
        {
            return networkStream
                .ReadAsync(readBuffer, 0, readBuffer.Length)
                .ToObservable()
                .Select(count => readBuffer.GetView(0, count));
        }

        #endregion

        #region IWriteStream

        /// <summary>
        /// 
        /// </summary>
        public bool CanWrite
        {
            get { return networkStream.CanWrite; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public IObservable<Int32> WriteAsync(IReadOnlyList<Byte> source)
        {
            Byte[] buffer = source.ToArray();
            return networkStream
                .WriteAsync(buffer, 0, buffer.Length)
                .ToObservable()
                .Select(_ => buffer.Length);
        }

        #endregion

        public void Dispose()
        {
            networkStream.Dispose();
        }

        private readonly Byte[] readBuffer;
        private readonly NetworkStream networkStream;


        IObservable<ArrayView<byte>> IReadStream.ReadAsync()
        {
            throw new NotImplementedException();
        }
    }
}
