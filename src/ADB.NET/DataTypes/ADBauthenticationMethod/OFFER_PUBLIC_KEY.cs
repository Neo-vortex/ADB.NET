using ADB.NET.Interfaces;

namespace ADB.NET.DataTypes.ADBauthenticationMethod;

public class OFFER_PUBLIC_KEY : IAuthenticationMethodParsable
{
    public uint GetAuthenticationMethod()
    {
        return 3;
    }
}