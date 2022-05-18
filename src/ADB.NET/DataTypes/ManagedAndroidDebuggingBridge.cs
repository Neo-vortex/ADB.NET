using ADB.NET.Interfaces;

namespace ADB.NET.DataTypes;

public class ManagedAndroidDebuggingBridge
{
    public List<IADBdevice> Devices { get; } = new ();
    public  void  ManuallyAddDevice(IADBdevice device)
    {
        if  (device == null)
        {
            throw  new  ArgumentNullException(nameof(device));
        }
        Devices.Add(device);
    }
}