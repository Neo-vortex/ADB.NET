using System.Net.Sockets;

namespace WirelessAndroidDebuggingBridge.Utilities;

public class SocketUtilities
{
   public static bool SocketConnected(Socket s)
    {
        var part1 = s.Poll(1000, SelectMode.SelectRead);
        var part2 = (s.Available == 0);
        return !part1 || !part2;
    }
}