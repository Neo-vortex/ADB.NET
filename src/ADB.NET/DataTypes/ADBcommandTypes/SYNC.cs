using ADB.NET.Interfaces;

namespace ADB.NET.Classes.ADBcommandTypes;

public class Sync : ICommandParsable
{
    public string GetCommand()
    {
        return "SYNC";
    }
}