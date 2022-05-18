using System.Runtime.CompilerServices;
using ADB.NET.Interfaces;

namespace WirelessAndroidDebuggingBridge.Utilities;

public class DispatchNotify
{
    public Task<byte[]> Oncall { get; set; }
    public ICommandParsable CommandType { get; }
    public DateTime RegisterTime { get; }
    
    public byte[]? Data { get; set; }

    public Guid id { get; set; }
    public  DispatchNotify(ICommandParsable commandType )
    {
        this.CommandType = commandType;
        this.RegisterTime = DateTime.Now;
        this.id = Guid.NewGuid();
        Oncall = new Task<byte[]>(() => Data);
    }
 
}