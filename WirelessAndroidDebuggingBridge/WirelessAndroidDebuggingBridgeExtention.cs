using ADB.NET.DataTypes;
using WirelessAndroidDebuggingBridge.Interfaces;

namespace WirelessAndroidDebuggingBridge;

public static class WirelessAndroidDebuggingBridgeExtention
{
    public static void AddWirelessAndroidDebuggingBridge(this ManagedAndroidDebuggingBridge managedAndroidDebuggingBridge, IWirelessAndroidDebuggingBridgeDevice device )
    {
        managedAndroidDebuggingBridge.ManuallyAddDevice(device);
    }
}
