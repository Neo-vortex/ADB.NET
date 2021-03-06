using ADB.NET.Classes.ADBcommandTypes;
using ADB.NET.Interfaces;

namespace ADB.NET.DataTypes.ABDpacket;

public static class ADBheaderFactory
{
    public static ADBheader CreateConnectHeader(uint version = 0x01 , uint maxDataLength =0x40000)
    {
        return new ADBheader(new CNXN(), version, maxDataLength);
    }

    public static ADBheader CreateAuthHeader(IAuthenticationMethodParsable method)
    {
        return new ADBheader(new AUTH(), method.GetAuthenticationMethod(), 0x00);
    }
}