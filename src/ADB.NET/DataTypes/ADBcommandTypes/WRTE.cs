using ADB.NET.Interfaces;

namespace ADB.NET.Classes.ADBcommandTypes;

public class WRTE : ICommandParsable
{
    public string GetCommand()
    {
        return "WRTE";
    }
}