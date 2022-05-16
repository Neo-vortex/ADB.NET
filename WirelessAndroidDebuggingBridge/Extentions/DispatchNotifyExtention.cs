using System.Runtime.CompilerServices;

namespace WirelessAndroidDebuggingBridge.Utilities;

public static class DispatchNotifyExtention
{
    public static  TaskAwaiter<byte[]> GetAwaiter(this DispatchNotify dispatch)
    {
        return  dispatch.Oncall.GetAwaiter();
    }
}