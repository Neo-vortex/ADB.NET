using System.Buffers.Binary;

namespace ADB.NET.Interfaces;

public interface ICommandParsable
{
    public string GetCommand();

    public uint GetCommandAsUint()
    {
        return BitConverter.ToUInt32(System.Text.Encoding.UTF8.GetBytes(GetCommand()));
    }

}