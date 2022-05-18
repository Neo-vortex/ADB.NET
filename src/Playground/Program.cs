using System.Buffers.Binary;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using ADB.NET;
using ADB.NET.DataTypes.ABDpacket;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using WirelessAndroidDebuggingBridge.DataTypes;


namespace  main
{
    class MyClass
    {
        
        
        public static void Main()
        {

            
            
            var t = GenerateKeyPair(256);
           var c =    GetBase64Key(t.Public);
            
            var result   =   SignDataSHA1(new byte[] { 1, 2, 3, 4, 5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20 }, null);
            /*
            var x = ADBpacket.FromByteArray(Convert.FromHexString("415554480300000000000000db020000bbf50000beaaabb75141414141436b52552b6a6e3541736449374e71704935587557676c46476b53587442434f7158735869797a687a6867534f4532616964726a747831524f36747647634e4e73745648707047796546752b4e6a382b50334c7431696c5a746835505470326b384d6a5a764e376747705658655355764b30466a6f6550636935476e717930324d4131534278367671452f2b375a4259444d6e6f4a68725a326f76622b39746947676b4a5a714a7a467458656672472f576e767a4665354f447638704d6f734232787851424935436c556b6a7947695243652b3868506951676a327230506d31746c4233657575682b315579714157645066317848785865484f4d2b54576373384b58622f75776b4e6e6c5538367930613453386f4a724d633765512b326f613868306b7a6367776c2f466d674d7867324c61512b7a5473454e6e6d4465656c39546643465878534c6a72594131797a2b37325a4e4d4a763069746c6a657159455450423351744b314d6873696175365a4468417778716378734b6b4175462b2b4d4c6d797355646b747a347731574c34456e362b6b326d51696e62304d326a796b3334644d3458615a306c54696e3542367a633567784d31587657417a4f753557555531335470776e366f6e424158324d33754f696c3243662b31754c53687852654f6d694f7155707666692f724876436e4247305830766e784a344e327830684466506c76366b58317034787a38706f49434635456f41766b612b337861753833486a65494a5742556f3347564a52432b39475956654641744c4167794f5650527466373665395161795a30642b2f7139424163616641517963794b4a5a734a4d4b6b6175542f3654376b57516b776e384f5a73312b682b31717348626171496e7139395a5a4d4b49563234312b62343430556f4854773256354f4450693630316971436b5a78525337764376487745414151413d206e6f6f62406e6f6f622d53797374656d2d50726f647563742d4e616d6500"));
                Console.WriteLine(  System.Text.Encoding.UTF8.GetString(x.Data.data) );
                */
            var device = new WirelessAndroidDebuggingfBridgeDevice(new IPEndPoint(IPAddress.Parse("192.168.2.165"), 5555));
            
            /*/*var device = new WirelessAndroidDebuggingfBridgeDevice(new IPEndPoint(IPAddress.Parse("192.168.249.82"), 5555));#1#
            var device = new WirelessAndroidDebuggingfBridgeDevice(new IPEndPoint(IPAddress.Parse("192.168.249.125"), 5555));*/
            device.Init().Wait();
         

            /*192.168.249.82:5555*/


            /*var _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(new IPEndPoint(IPAddress.Parse("192.168.249.82") , 5555));
            var _CONNECT = new ADBpacket(ADBcommandType.CNXN, 0x01000000, 256*1024, "host::C#_ADB");
            _socket.Send(_CONNECT.ToByteArray());
            var data = new byte[256*1024];
            var cnt= _socket.Receive(data, SocketFlags.None);
            var _result = ADBpacket.FromByteArray(data[..cnt]);
            var rr = _result.GetHumanReadableCommandType();


            /*var _ath_moc = new ADBpacket(ADBcommandType.AUTH, 3, 0, "4564698547");#1#
            var _ath_moc = new ADBpacket(ADBcommandType.AUTH, 3, 0, "4564698547");
            _socket.Send(_ath_moc.ToByteArray());
            data = new byte[256*1024];
             cnt= _socket.Receive(data, SocketFlags.None);
             Console.WriteLine(System.Text.Encoding.UTF8.GetString(data[..19]));
             Console.WriteLine(System.Text.Encoding.UTF8.GetString(data[20..cnt]));
             try
             {   
                 _result = ADBpacket.FromByteArray(data[..cnt]);
             }
             catch (Exception e)
             {
                 data = new byte[256*1024];
                 cnt= _socket.Receive(data, SocketFlags.None);
             }
             _result = ADBpacket.FromByteArray(data[..cnt]);
             
             Console.WriteLine(System.Text.Encoding.UTF8.GetString(_result.data));
             
        }*/
        }
    }

}


