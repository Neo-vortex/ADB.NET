using ADB.NET.Enums;

namespace ADB.NET.Interfaces;

public interface IADBdevice
{
    public Task Init();
    
    public  ADBConnectionStatus Status { get; set; }
}