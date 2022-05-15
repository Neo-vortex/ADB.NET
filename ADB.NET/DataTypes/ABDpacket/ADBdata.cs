using System.Text;

namespace ADB.NET.DataTypes.ABDpacket;

public record ADBdata(byte[] data)
{
    public ADBdata(string data) : this(Encoding.UTF8.GetBytes(data))
    {
    }

    public byte[] data { get; } = data;
}