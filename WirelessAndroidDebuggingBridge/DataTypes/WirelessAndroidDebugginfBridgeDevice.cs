using System.Net;
using System.Net.Sockets;
using ADB.NET.DataTypes.ABDpacket;
using ADB.NET.Enums;
using WirelessAndroidDebuggingBridge.Interfaces;

namespace WirelessAndroidDebuggingBridge.DataTypes;

public class WirelessAndroidDebugginfBridgeDevice : IWirelessAndroidDebuggingBridgeDevice 
{
    public ADBConnectionStatus Status { get; set; }
    public  delegate void NewADBpacket_delegate(ADBpacket packet);
    public  delegate void ADBDisconnected_delegate(IWirelessAndroidDebuggingBridgeDevice packet);
    public  event  NewADBpacket_delegate NewADBpacket;
    public  event  ADBDisconnected_delegate ADBDisconnected;
    public  IPEndPoint DeviceIPEndPoint { get; set; }
    public  Socket DeviceSocket { get; set; }
    
    public WirelessAndroidDebugginfBridgeDevice(IPEndPoint deviceIPEndPoint)
    {
        DeviceIPEndPoint = deviceIPEndPoint ?? throw new  ArgumentNullException("deviceIPEndPoint");
    }

    private void packet_reader()
    {
        while (Status == ADBConnectionStatus.CONNECTED)
        {
        }
    }

    private void socket_check()
    {
        while (Status == ADBConnectionStatus.CONNECTED)
        {
            if (! Utilities.SocketUtilities.SocketConnected(DeviceSocket))
            {
                Status = ADBConnectionStatus.DISCONNECTED;
                ADBDisconnected?.Invoke(this);
            }
            Thread.Sleep(Utilities.Consts._SLEEP_TIME);
        }
    }
    
    public Task Init()
    {
        return Task.Run(() =>
        {
            Status = ADBConnectionStatus.PENDING;
            DeviceSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            DeviceSocket.Connect(DeviceIPEndPoint);
            DeviceSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            DeviceSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, 5);
            DeviceSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, 5);
            DeviceSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveRetryCount, 5);
            Status = ADBConnectionStatus.CONNECTED;
            
            
            
            
            
            
            new Thread(socket_check).Start();
            new Thread(packet_reader).Start();
        });
    }


}