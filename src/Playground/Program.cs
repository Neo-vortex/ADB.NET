using System.Buffers.Binary;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using ADB.NET;
using ADB.NET.DataTypes.ABDpacket;
using LibUsbDotNet;
using LibUsbDotNet.Info;
using LibUsbDotNet.LibUsb;
using LibUsbDotNet.Main;
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
        
        [DllImport(
            "/home/noob/CLionProjects/AndroidDebuggingBridgeLibUSB/cmake-build-debug/libAndroidDebuggingBridgeLibUSB.so",
             CharSet = CharSet.Unicode,
            SetLastError = true)]
        public static extern int Init();
        
        
        public static void Main()
        {



            int result = Init();
            

            
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


