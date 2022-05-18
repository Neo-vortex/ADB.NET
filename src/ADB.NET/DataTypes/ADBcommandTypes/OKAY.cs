using ADB.NET.Interfaces;

namespace ADB.NET.Classes.ADBcommandTypes;

public class OKAY : ICommandParsable
{
    public string GetCommand()
    {
        return "OKAY";
    }
}