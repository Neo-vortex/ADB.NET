using ADB.NET.Interfaces;

namespace ADB.NET.Classes.ADBcommandTypes;

public class CLSE : ICommandParsable
{
    public string GetCommand()
    {
        return "CLSE";
    }
}