namespace ADB.NET.Utilities;

public static class Consts
{
    public const uint _MAGIC_CONST = 0XFFFFFFFF;
    public static byte[] _RSA_PUBKEY = { 34, 35, 36, 34, 36, 39, 38, 35, 34, 37 };
    public  static  string _HOST_NAME = "host::features=stat_v2,cmd,shell_v2";
    public const int _PACKET_HEADER_SIZE = 24;
}