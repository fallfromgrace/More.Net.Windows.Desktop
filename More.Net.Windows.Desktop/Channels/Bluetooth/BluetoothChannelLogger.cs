using More.Net.Linq;
using More.Net.Logging;
using InTheHand.Net.Sockets;
using log4net;
using System;
using System.Linq;

namespace More.Net.Channels.Bluetooth
{
    internal class BluetoothChannelLogger : LoggerBase
    {
        public BluetoothChannelLogger() :
            base(LogManager.GetLogger(typeof(BluetoothChannel)))
        {

        }

        public void OnAuthenticationBegin(String pin, BluetoothDeviceInfo deviceInfo)
        {
            if (Logger.IsInfoEnabled)
            {
                Logger.InfoFormat(
                    "Authenticating bluetooth device.{3}    Name: {0}{3}    Pin: {1}{3}    Address: {2}",
                    deviceInfo.DeviceName,
                    pin,
                    deviceInfo.DeviceAddress,
                    Environment.NewLine);
            }
        }

        public void OnAuthenticationCompleted(String pin, BluetoothDeviceInfo deviceInfo)
        {
            if (Logger.IsInfoEnabled)
            {
                if (deviceInfo.Authenticated)
                    Logger.InfoFormat(
                        "Successfully authenticated bluetooth device.{3}    Name: {0}{3}    Pin: {1}{3}    Address: {2}",
                        deviceInfo.DeviceName,
                        pin,
                        deviceInfo.DeviceAddress,
                        Environment.NewLine);
                else
                    Logger.InfoFormat(
                        "Failed to authenticate bluetooth device.{3}    Name: {0}{3}    Pin: {1}{3}    Address: {2}",
                        deviceInfo.DeviceName,
                        pin,
                        deviceInfo.DeviceAddress,
                        Environment.NewLine);
            }
        }

        public void OnConnectionBegin(BluetoothDeviceInfo deviceInfo)
        {
            if (Logger.IsInfoEnabled)
            {
                Logger.InfoFormat(
                    "Connecting to bluetooth device.{2}    Name: {0}{2}    Address: {1}",
                    deviceInfo.DeviceName,
                    deviceInfo.DeviceAddress,
                    Environment.NewLine);
            }
        }

        public void OnConnectionCompleted(BluetoothDeviceInfo deviceInfo)
        {
            if (Logger.IsInfoEnabled)
            {
                if (deviceInfo.Connected)
                    Logger.InfoFormat(
                        "Successfully connected to bluetooth device.{2}    Name: {0}{2}    Address: {1}",
                        deviceInfo.DeviceName,
                        deviceInfo.DeviceAddress,
                        Environment.NewLine);
                else
                    Logger.InfoFormat(
                        "Failed to connect to bluetooth device.{2}    Name: {0}{2}    Address: {1}",
                        deviceInfo.DeviceName,
                        deviceInfo.DeviceAddress,
                        Environment.NewLine);
            }
        }

        public void OnConnectionCompleted(BluetoothDeviceInfo deviceInfo, Exception ex)
        {
            if (Logger.IsErrorEnabled)
            {
                if (deviceInfo.Connected)
                    Logger.Error(
                        String.Format(
                            "Successfully connected to bluetooth device.{2}    Name: {0}{2}    Address: {1}",
                            deviceInfo.DeviceName,
                            deviceInfo.DeviceAddress,
                            Environment.NewLine),
                         ex);
                else
                    Logger.Error(
                        String.Format(
                            "Failed to connect to bluetooth device.{2}    Name: {0}{2}    Address: {1}",
                            deviceInfo.DeviceName,
                            deviceInfo.DeviceAddress,
                            Environment.NewLine),
                         ex);
            }
        }

        public void OnReadBegin(BluetoothDeviceInfo deviceInfo)
        {
            if (Logger.IsInfoEnabled == true)
            {
                Logger.InfoFormat(
                    "Reading bytes.{2}    Name: {0}{2}    Address: {1}{2}",
                    deviceInfo.DeviceName,
                    deviceInfo.DeviceAddress,
                    Environment.NewLine);
            }
        }

        public void OnReadCompleted(BluetoothDeviceInfo deviceInfo, Byte[] buffer, Int32 offset, Int32 count)
        {
            if (Logger.IsDebugEnabled == true)
            {
                Logger.DebugFormat(
                    "Sucessfully read bytes.{4}    Name: {0}{4}    Address: {1}{4}    Count: {2}{4}    Data: {3}{4}",
                    deviceInfo.DeviceName,
                    deviceInfo.DeviceAddress,
                    count,
                    buffer.Skip(offset).Take(count).Select(b => String.Format("0x{0:X2}", b)).Concat(" "),
                    Environment.NewLine);

            }
            else if (Logger.IsInfoEnabled == true)
            {
                Logger.InfoFormat(
                    "Sucessfully read bytes.{3}    Name: {0}{3}    Address: {1}{3}    Count: {2}{3}",
                    deviceInfo.DeviceName,
                    deviceInfo.DeviceAddress,
                    count,
                    Environment.NewLine);
            }
        }

        public void OnReadCompleted(BluetoothDeviceInfo deviceInfo, Exception ex)
        {
            if (Logger.IsErrorEnabled)
            {
                Logger.Error(
                    String.Format(
                        "Failed to read next set of bytes.{3}    Name: {0}{3}    Address: {1}{3}    Connected: {2}",
                        deviceInfo.DeviceName,
                        deviceInfo.DeviceAddress,
                        deviceInfo.Connected,
                        Environment.NewLine),
                    ex);
            }
        }
    }
}
