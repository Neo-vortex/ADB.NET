using ADB.NET.Classes.ADBcommandTypes;
using ADB.NET.Interfaces;

namespace ADB.NET.DataTypes.ABDpacket;

public static class ADBheaderFactory
{
    public static ADBheader CreateConnectHeader(uint version = 0x01)
    {
        return new ADBheader(new CNXN(), version, 256*1024);
    }

    public static ADBheader CreateAuthHeader(IAuthenticationMethodParsable method)
    {
        return new ADBheader(new AUTH(), method.GetAuthenticationMethod(), 0x00);
    }
}