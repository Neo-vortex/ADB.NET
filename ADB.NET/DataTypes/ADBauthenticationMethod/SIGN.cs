using ADB.NET.Interfaces;

namespace ADB.NET.DataTypes.ADBauthenticationMethod;

public class SIGN : IAuthenticationMethodParsable
{
    public uint GetAuthenticationMethod()
    {
        return 2;
    }
}