using System.Text;

namespace ADB.NET.DataTypes.ABDpacket;

public class ADBdata 
{
    public byte[] data { get; }

    public ADBdata(byte[] data)
    {
        this.data = data;
    }
    public ADBdata(string data)
    {
        this.data = System.Text.Encoding.UTF8.GetBytes(data);
    }

    public ADBdata(Span<byte> slice)
    {
        this.data = slice.ToArray();
    }
}