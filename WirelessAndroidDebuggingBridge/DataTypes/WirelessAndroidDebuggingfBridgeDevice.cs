
#define SOCKET_KEEP_ALIVE 

using System.Net;
using System.Net.Sockets;
using ADB.NET.Classes.ADBcommandTypes;
using ADB.NET.DataTypes.ABDpacket;
using ADB.NET.DataTypes.ADBauthenticationMethod;
using ADB.NET.Enums;
using WirelessAndroidDebuggingBridge.Interfaces;
using WirelessAndroidDebuggingBridge.Utilities;

namespace WirelessAndroidDebuggingBridge.DataTypes;

public class WirelessAndroidDebuggingfBridgeDevice : IWirelessAndroidDebuggingBridgeDevice
{
    public List<DispatchNotify> DispatchNotifies { get; set; } = new();
    public ADBConnectionStatus Status { get; set; }
    public  delegate void NewADBpacket_delegate(ADBpacket packet);
    public  delegate void ADBDisconnected_delegate(IWirelessAndroidDebuggingBridgeDevice packet);
    public  event  NewADBpacket_delegate? NewADBpacket;
    public  event  ADBDisconnected_delegate? ADBDisconnected;
    public  IPEndPoint DeviceIPEndPoint { get; set; }
    public  Socket? DeviceSocket { get; set; }

    private object _send_lock = new();
    
    public WirelessAndroidDebuggingfBridgeDevice(IPEndPoint deviceIpEndPoint)
    {
        DeviceIPEndPoint = deviceIpEndPoint ?? throw new  ArgumentNullException("deviceIpEndPoint");
    }

    private void packet_reader()
    {
        while (Status == ADBConnectionStatus.CONNECTED)
        {
            ADBpacket packet;
            var header = ADBheader.FromByteArray(Utilities.SocketUtilities.ReceiveExactly(this.DeviceSocket!,
                ADB.NET.Utilities.Consts._PACKET_HEADER_SIZE));
            if (header.data_length > 0)
            {
                var data = Utilities.SocketUtilities.ReceiveExactly(this.DeviceSocket!,(int) header.data_length);
                 packet = new ADBpacket(header, new ADBdata(data) );
            }
            else
            {
                packet = new ADBpacket(header);
            }

            foreach (var dispatch in DispatchNotifies.Where(dispatch => dispatch.CommandType.GetCommandAsUint() == packet.Header.command))
            {
                dispatch.Data = packet.Data.data;
                dispatch.Oncall.Start();
                dispatch.Oncall.Wait();
                DispatchNotifies.Remove(dispatch);
                break;
            }
            NewADBpacket?.Invoke(packet);
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
        return Task.Run( async () =>
        {
            Status = ADBConnectionStatus.PENDING;
            DeviceSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await DeviceSocket.ConnectAsync(DeviceIPEndPoint);
            #if SOCKET_KEEP_ALIVE
            DeviceSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            DeviceSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, 5);
            DeviceSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, 5);
            DeviceSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveRetryCount, 5);
            #endif
            Status = ADBConnectionStatus.CONNECTED;
            new Thread(socket_check).Start();
            new Thread(packet_reader).Start();
            var connectPacket = ADBpacketFactory.CreateConnectPacket(ADB.NET.Utilities.Consts._HOST_NAME);
            var waitHandler = new DispatchNotify(new AUTH());
            DispatchNotifies.Add(waitHandler);
            SendPacket(connectPacket);
            var auth_challange = await waitHandler;
            //todo sign auth_challange?
           // var auth_response = ADBpacketFactory.CreateAuthBpacket(System.Text.Encoding.ASCII.GetBytes("4564698547"), new OFFER_PUBLIC_KEY());
            waitHandler = new DispatchNotify(new AUTH());
            DispatchNotifies.Add(waitHandler);


            var _packet = ADBpacket.FromByteArray(Convert.FromHexString(
                "41555448020000000000000000010000eb760000beaaabb79db1c8125889d3f640c439bd85b657ba157625d9cf2664535671b36e5c67ae93d54efabf7938df877a694752570b49fe6035400ccd9bc7c145208abd28b1808adf5b2c4ae66b76b9a9f3d5bed058812e712064c6bd42be37d5cbf53e7e59377b834ad5fc1522af5986cb6137c87fe360991ec963df54a39fb718c522586369360584e0b134e00b79a6f25d3851976cfc9530bcbf23256709183c67a6c97df3b052fe50a04b8d0bf4ce478701e524c00916820a9a12ac8c0d2b6d004e55c1282f0ce70911800d37d1393934f779b5f11c920d101c79b0ee1a1c0cb56c1fd6d81e26161f5508fe2e5a22a1e326da7a61a2b2d1652dba7990bc029e57b9311e5274"));


            byte[]  fake_bytes =   Enumerable.Repeat(5, 64).ToArray().SelectMany(BitConverter.GetBytes).ToArray();

            var auth_response =
                ADBpacketFactory.CreateAuthBpacket(fake_bytes, new SIGN());
            
            SendPacket(auth_response);
            var connectResult = await waitHandler;
            Console.WriteLine(System.Text.Encoding.UTF8.GetString(connectResult));
            int g = 5;
        });
    }

    private void SendPacket(ADBpacket packet)
    {
        lock (_send_lock)
        {
            DeviceSocket?.Send(packet.ToByteArray());
        }
    }
}