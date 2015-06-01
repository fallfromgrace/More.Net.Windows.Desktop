using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InTheHand.Net;
using InTheHand.Net.Sockets;

namespace More.Net.Channels.Experimental.Bluetooth
{
    /// <summary>
    /// 
    /// </summary>
    internal static class InTheHandBluetoothClientExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public static async Task ConnectAsync(this BluetoothClient client, BluetoothEndPoint endPoint)
        {
            await Task.Factory
                .FromAsync(client.BeginConnect, client.EndConnect, endPoint, client)
                .ConfigureAwait(false);
        }
    }
}
