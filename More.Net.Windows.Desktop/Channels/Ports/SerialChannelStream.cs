using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace More.Net.Channels.Ports
{
    public class SerialChannelStream : IChannelStream
    {
        public SerialChannelStream(Stream stream)
        {

        }

        public bool CanRead
        {
            get { throw new NotImplementedException(); }
        }

        public int ReadBufferSize
        {
            get { throw new NotImplementedException(); }
        }

        public IObservable<ArrayView<byte>> ReadAsync()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool CanWrite
        {
            get { throw new NotImplementedException(); }
        }

        public IObservable<int> WriteAsync(IReadOnlyList<byte> buffer)
        {
            throw new NotImplementedException();
        }

        private readonly Stream stream;
    }
}
