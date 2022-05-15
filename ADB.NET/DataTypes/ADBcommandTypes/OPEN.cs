using ADB.NET.Interfaces;

namespace ADB.NET.Classes.ADBcommandTypes;

public class Open : ICommandParsable
{
    public string GetCommand()
    {
        return "OPEN";
    }
}