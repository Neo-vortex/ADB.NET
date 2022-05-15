using System.Buffers.Binary;

namespace ADB.NET.DataTypes.ABDpacket;

public class ADBpacket
{
    public ADBpacket(ADBheader header, ADBdata data)
    {
        Header = header ?? throw new ArgumentNullException(nameof(header));
        Data = data ?? throw new ArgumentNullException(nameof(data));
        Header.data_length = (uint)data.data.Length;
        Header.data_crc32 = CalculateCrc32();
    }

    public ADBpacket()
    {
    }

    public ADBheader Header { get; set; }
    public ADBdata Data { get; set; }

    private uint CalculateCrc32()
    {
        return Data?.data?.Aggregate<byte, uint>(0, (current, t) => (current + t) & Utilities.Consts._MAGIC_CONST) ??
               throw new ArgumentNullException("Data is null");
    }

    public static ADBpacket FromByteArray(byte[] buffer)
    {
        if (buffer.Length < 24) throw new ArgumentException("Buffer is too small");
        var packet = new ADBpacket
        {
            Header =
            {
                command = BinaryPrimitives.ReadUInt32LittleEndian(buffer),
                arg0 = BinaryPrimitives.ReadUInt32LittleEndian(buffer[4..]),
                arg1 = BinaryPrimitives.ReadUInt32LittleEndian(buffer[8..]),
                data_length = BinaryPrimitives.ReadUInt32LittleEndian(buffer[12..]),
                data_crc32 = BinaryPrimitives.ReadUInt32LittleEndian(buffer[16..]),
                magic = BinaryPrimitives.ReadUInt32LittleEndian(buffer[20..])
            }
        };
        if (packet.Header.data_length > 0) packet.Data = new ADBdata(buffer[24..].ToArray());
        return packet;
    }

    public override string ToString()
    {
        return $"{nameof(Header)}: {Header}, {nameof(Data)}: {Data}";
    }

    public unsafe byte[] ToByteArray()
    {
        Span<byte> buffer = stackalloc byte[24 + (int)Header.data_length];

        BinaryPrimitives.WriteUInt32LittleEndian(buffer, Header.command);
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[4..],
            Header.arg0);
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[8..],
            Header.arg1);
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[12..],
            Header.data_length);
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[16..],
            Header.data_crc32);
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[20..],
            Header.magic);
        if (Header.data_length > 0) Data.data.CopyTo(buffer[24..]);
        return buffer.ToArray();
    }
}