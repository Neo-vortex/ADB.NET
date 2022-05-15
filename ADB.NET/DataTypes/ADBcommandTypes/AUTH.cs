using ADB.NET.Interfaces;

namespace ADB.NET.Classes.ADBcommandTypes;

public class AUTH : ICommandParsable
{
    public string GetCommand()
    {
        return "AUTH";
    }
}