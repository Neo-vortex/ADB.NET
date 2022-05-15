using ADB.NET.Interfaces;

namespace ADB.NET.Classes.ADBcommandTypes;

public class CNXN : ICommandParsable
{
    public string GetCommand()
    {
        return "CNXN";
    }
}