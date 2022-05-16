using System.Net.Sockets;

namespace WirelessAndroidDebuggingBridge.Utilities;

public class SocketUtilities
{
   public static bool SocketConnected(Socket? s)
    {
        return s != null && (!(s.Poll(1000, SelectMode.SelectRead)) || !( (s.Available == 0) && !(s.Connected)));
    }
   public  static byte[] ReceiveExactly(Socket handler, int length)
   {
       var buffer = new byte[length];
       var receivedLength = 0;
       while(receivedLength < length)
       {
           var nextLength = handler.Receive(buffer,receivedLength,length-receivedLength, SocketFlags.None);
           if(nextLength==0)
           {
               return Array.Empty<byte>();
           }
           receivedLength += nextLength;
       }
       return buffer;
   }
}