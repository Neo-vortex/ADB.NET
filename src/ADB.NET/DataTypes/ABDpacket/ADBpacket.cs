
#define CHECK_CRC2
#define CHECK_PACKET_LENGH
using System.Buffers.Binary;
#if CHECK_CRC2
using System.Security;
#endif



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

    public ADBpacket(ADBheader header)
    {
        Header = header ?? throw new ArgumentNullException(nameof(header));
    }

    public ADBheader Header { get; set; } = new();
    public ADBdata Data { get; set; }

    private uint CalculateCrc32()
    {
        return Data.data.Aggregate<byte, uint>(0, (current, t) => (current + t) & 0xFFFFFFFF);
    }

    public static ADBpacket FromByteArray(byte[] buffer)
    {
        if (buffer.Length < 24) throw new ArgumentException("Buffer is too small");
        var packet = new ADBpacket
        {
            Header =
            {
                command = BinaryPrimitives.ReadUInt32LittleEndian(buffer),
                arg0 = BinaryPrimitives.ReadUInt32LittleEndian(buffer.AsSpan()[4..]),
                arg1 = BinaryPrimitives.ReadUInt32LittleEndian(buffer.AsSpan()[8..]),
                data_length = BinaryPrimitives.ReadUInt32LittleEndian(buffer.AsSpan()[12..]),
                data_crc32 = BinaryPrimitives.ReadUInt32LittleEndian(buffer.AsSpan()[16..]),
                magic = BinaryPrimitives.ReadUInt32LittleEndian(buffer.AsSpan()[20..])
            }
        };
        if (packet.Header.data_length == 0 ) return packet;
        packet.Data = new ADBdata(buffer.AsSpan()[24..]);
        #if CHECK_CRC2
        if (packet.CalculateCrc32() != packet.Header.data_crc32)
        {
            throw new SecurityException("CRC32 not matched");
        }
        #endif
        #if CHECK_PACKET_LENGH
        if (packet.Data.data.Length != packet.Header.data_length)
        {
            throw new Exception("data lenght is not correct");
        }    
        #endif
   
        return packet;
    }

    public override string ToString()
    {
        return $"{nameof(Header)}: {Header}, {nameof(Data)}: {Data}";
    }


    public unsafe Tuple<byte[], byte[]> ToSplittedByteArray()
    {
        Span<byte> buffer1 = stackalloc byte[24 ];
        Span<byte> buffer2 = stackalloc byte[(int)Header.data_length];
        BinaryPrimitives.WriteUInt32LittleEndian(buffer1, Header.command);
        BinaryPrimitives.WriteUInt32LittleEndian(buffer1[4..],
            Header.arg0);
        BinaryPrimitives.WriteUInt32LittleEndian(buffer1[8..],
            Header.arg1);
        BinaryPrimitives.WriteUInt32LittleEndian(buffer1[12..],
            Header.data_length);
        BinaryPrimitives.WriteUInt32LittleEndian(buffer1[16..],
            Header.data_crc32);
        BinaryPrimitives.WriteUInt32LittleEndian(buffer1[20..],
            Header.magic);
        if (Header.data_length > 0) Data.data.CopyTo(buffer2);
        return new Tuple<byte[], byte[]>(buffer1.ToArray(), buffer2.ToArray());
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