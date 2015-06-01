using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace More.Net.Channels.Bluetooth
{
    /// <summary>
    /// 
    /// </summary>
    internal static class BluetoothComponentExtensions
    {
        /// <summary>
        /// Asynchronously discovers bluetooth devices by projecting the results into an observable 
        /// sequence.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        internal static IObservable<BluetoothDeviceInfo> WhenDeviceDiscovered(
            this BluetoothComponent component, 
            Boolean authenticated, 
            Boolean remembered, 
            Boolean unknown, 
            Boolean discoverableOnly)
        {
            return Observable
                .Defer(() => component
                    .DiscoverDevices(authenticated, remembered, unknown, discoverableOnly))
                .Publish()
                .RefCount();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="component"></param>
        /// <param name="authenticated"></param>
        /// <param name="remembered"></param>
        /// <param name="unknown"></param>
        /// <param name="discoverableOnly"></param>
        /// <returns></returns>
        private static IObservable<BluetoothDeviceInfo> DiscoverDevices(
            this BluetoothComponent component,
            Boolean authenticated,
            Boolean remembered,
            Boolean unknown,
            Boolean discoverableOnly)
        {
            IObservable<DiscoverDevicesEventArgs> discoverDevicesComplete = component
                .WhenDeviceDiscoveryCompleted();

            IObservable<BluetoothDeviceInfo> discoverDevicesAsync = component
                .WhenDevicesDiscovered()
                .Merge(discoverDevicesComplete)
                .TakeUntil(discoverDevicesComplete)
                .SelectMany(e => e.Devices);

            component.DiscoverDevicesAsync(
                Int32.MaxValue, authenticated, remembered,
                unknown, discoverableOnly, component);

            return discoverDevicesAsync;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        private static IObservable<DiscoverDevicesEventArgs> WhenDevicesDiscovered(
            this BluetoothComponent component)
        {
            return Observable
                .FromEventPattern<DiscoverDevicesEventArgs>(
                    handler => component.DiscoverDevicesProgress += handler,
                    handler => component.DiscoverDevicesProgress -= handler)
                .Select(ep => ep.EventArgs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        private static IObservable<DiscoverDevicesEventArgs> WhenDeviceDiscoveryCompleted(
            this BluetoothComponent component)
        {
            return Observable
                .FromEventPattern<DiscoverDevicesEventArgs>(
                    handler => component.DiscoverDevicesComplete += handler,
                    handler => component.DiscoverDevicesComplete -= handler)
                .Select(ep => ep.EventArgs);
        }
    }
}
