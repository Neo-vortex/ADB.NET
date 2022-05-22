
#define SOCKET_KEEP_ALIVE 

using System.Net;
using System.Net.Sockets;
using ADB.NET.Classes.ADBcommandTypes;
using ADB.NET.DataTypes.ABDpacket;
using ADB.NET.DataTypes.ADBauthenticationMethod;
using ADB.NET.Enums;
using ADB.NET.Interfaces;
using WirelessAndroidDebuggingBridge.Interfaces;
using WirelessAndroidDebuggingBridge.Utilities;

namespace WirelessAndroidDebuggingBridge.DataTypes;

public class WirelessAndroidDebuggingfBridgeDevice : IWirelessAndroidDebuggingBridgeDevice
{
    public List<DispatchNotify> DispatchNotifies { get; set; } = new();
    public ADBConnectionStatus Status { get; set; }
    public  event  IADBdevice.NewADBpacket_delegate? NewADBpacket;
    public  event  IADBdevice.ADBDisconnected_delegate? ADBDisconnected;
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
            DeviceSocket.NoDelay = true;
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
            SendRawPacket(connectPacket);



           
            
            
            var authChallange = await waitHandler;
            //todo sign auth_challange?
           // var auth_response = ADBpacketFactory.CreateAuthBpacket(System.Text.Encoding.ASCII.GetBytes("4564698547"), new OFFER_PUBLIC_KEY());
            waitHandler = new DispatchNotify(new CNXN());
            DispatchNotifies.Add(waitHandler);

            var c4 = ADBpacket.FromByteArray(Convert.FromHexString(
                "415554480300000000000000db020000bbf50000beaaabb75141414141436b52552b6a6e3541736449374e71704935587557676c46476b53587442434f7158735869797a687a6867534f4532616964726a747831524f36747647634e4e73745648707047796546752b4e6a382b50334c7431696c5a746835505470326b384d6a5a764e376747705658655355764b30466a6f6550636935476e717930324d4131534278367671452f2b375a4259444d6e6f4a68725a326f76622b39746947676b4a5a714a7a467458656672472f576e767a4665354f447638704d6f734232787851424935436c556b6a7947695243652b3868506951676a327230506d31746c4233657575682b315579714157645066317848785865484f4d2b54576373384b58622f75776b4e6e6c5538367930613453386f4a724d633765512b326f613868306b7a6367776c2f466d674d7867324c61512b7a5473454e6e6d4465656c39546643465878534c6a72594131797a2b37325a4e4d4a763069746c6a657159455450423351744b314d6873696175365a4468417778716378734b6b4175462b2b4d4c6d797355646b747a347731574c34456e362b6b326d51696e62304d326a796b3334644d3458615a306c54696e3542367a633567784d31587657417a4f753557555531335470776e366f6e424158324d33754f696c3243662b31754c53687852654f6d694f7155707666692f724876436e4247305830766e784a344e327830684466506c76366b58317034787a38706f49434635456f41766b612b337861753833486a65494a5742556f3347564a52432b39475956654641744c4167794f5650527466373665395161795a30642b2f7139424163616641517963794b4a5a734a4d4b6b6175542f3654376b57516b776e384f5a73312b682b31717348626171496e7139395a5a4d4b49563234312b62343430556f4854773256354f4450693630316971436b5a78525337764376487745414151413d206e6f6f62406e6f6f622d53797374656d2d50726f647563742d4e616d6500"));

            var x3pro = ADBpacket.FromByteArray(Convert.FromHexString(
                "415554480300000000000000db020000bbf50000beaaabb75141414141436b52552b6a6e3541736449374e71704935587557676c46476b53587442434f7158735869797a687a6867534f4532616964726a747831524f36747647634e4e73745648707047796546752b4e6a382b50334c7431696c5a746835505470326b384d6a5a764e376747705658655355764b30466a6f6550636935476e717930324d4131534278367671452f2b375a4259444d6e6f4a68725a326f76622b39746947676b4a5a714a7a467458656672472f576e767a4665354f447638704d6f734232787851424935436c556b6a7947695243652b3868506951676a327230506d31746c4233657575682b315579714157645066317848785865484f4d2b54576373384b58622f75776b4e6e6c5538367930613453386f4a724d633765512b326f613868306b7a6367776c2f466d674d7867324c61512b7a5473454e6e6d4465656c39546643465878534c6a72594131797a2b37325a4e4d4a763069746c6a657159455450423351744b314d6873696175365a4468417778716378734b6b4175462b2b4d4c6d797355646b747a347731574c34456e362b6b326d51696e62304d326a796b3334644d3458615a306c54696e3542367a633567784d31587657417a4f753557555531335470776e366f6e424158324d33754f696c3243662b31754c53687852654f6d694f7155707666692f724876436e4247305830766e784a344e327830684466506c76366b58317034787a38706f49434635456f41766b612b337861753833486a65494a5742556f3347564a52432b39475956654641744c4167794f5650527466373665395161795a30642b2f7139424163616641517963794b4a5a734a4d4b6b6175542f3654376b57516b776e384f5a73312b682b31717348626171496e7139395a5a4d4b49563234312b62343430556f4854773256354f4450693630316971436b5a78525337764376487745414151413d206e6f6f62406e6f6f622d53797374656d2d50726f647563742d4e616d6500"));


            var fake_signed = ADBpacket.FromByteArray(Convert.FromHexString(
                "4155544802000000000000000001000028850000beaaabb716cdf2b4ed5e3cf9d8397960673852af4caa84320ef594d0890bd8610cb27b126197fa0326f6132a61cee62514b73d21f8ec8cf1071effd206f48ec78fae19d306e461bcae931fa3f66cdce4498be6cde96e085a82e090c6eed9d18108e4ddf14a5b3f70314ea663e12ecbf91fdf182f3b18d9117217e3acbeb70da8b6847553a70627525609f1d1fb0bab3e849891e8bf04b0676c7e43152874f53b1510fe8ddf9429594d6bb390d3d2e9e1ec2c738b6cb7c1f0b736a1e502c612f8581fbb2cded1ddcddfab3ab0674d3b29cf92136cbb267aec053ace7e631978c8fa68c5e752985e4619a6c30091dacfd8a3d5e41086e850c930688a953aea3f67f2dd4a1f"));
            
            SendRawPacket(fake_signed);
            var auth_response =
                ADBpacketFactory.CreateAuthBpacket(System.Text.Encoding.UTF8.GetBytes("4564698547"), new OFFER_PUBLIC_KEY());
            SendRawPacket(x3pro);
            var connectResult = await waitHandler;
            Console.WriteLine(System.Text.Encoding.UTF8.GetString(connectResult));
            int g = 5;
        });
    }

    public void SendRawPacket(ADBpacket packet)
    {
        lock (_send_lock)
        {
            var result = packet.ToSplittedByteArray();
            DeviceSocket?.Send(result.Item1);
            DeviceSocket?.Send(result.Item2);
        }
    }
}