using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace More.Net.Channels.Experimental.Bluetooth
{
    public class BluetoothChannel
    {
        public BluetoothChannel()
        {
            Task.Factory.FromAsync(client.BeginConnect, client.EndConnect, endpoint, client);
        }

        //public IObservable<Boolean> ConnectAsync()
        //{
        //    client.ConnectAsync(endpoint)
        //        .ToObservable()
        //        .Do(_ => client.GetStream())
        //        .Select(_ => true);
        //}

        public IObservable<Boolean> AuthenticateAsync()
        {
            throw new NotImplementedException();
        }

        public IObservable<IReadOnlyList<Byte>> WhenDataRead()
        {
            throw new NotImplementedException();
        }

        public IObservable<Boolean> WhenConnectionChanged()
        {
            throw new NotImplementedException();
        }

        InTheHand.Net.BluetoothEndPoint endpoint = null;
        InTheHand.Net.Sockets.BluetoothClient client = null;
        private IChannelStream stream;
    }
}
