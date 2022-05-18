using ADB.NET.DataTypes.ABDpacket;
using ADB.NET.Enums;

namespace ADB.NET.Interfaces;

public interface IADBdevice
{
    public Task Init();
    
    public  ADBConnectionStatus Status { get; set; }

    public void SendRawPacket(ADBpacket packet);
    public  delegate void NewADBpacket_delegate(ADBpacket packet);
    public  delegate void ADBDisconnected_delegate(IADBdevice device);
    public  event  NewADBpacket_delegate? NewADBpacket;
    public  event  ADBDisconnected_delegate? ADBDisconnected;
}