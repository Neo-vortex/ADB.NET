using System.Buffers.Binary;

namespace ADB.NET;

public class ADBpacket
{
    public uint command { get; set; } = 0;     /* command identifier constant (A_CNXN, ...) */
    public uint arg0 { get; set; } = 0;       /* first argument                            */
    public uint arg1 { get; set; } = 0;      /* second argument                           */
    public uint data_length { get; set; } = 0; /* length of payload (0 is allowed)          */
    public uint data_crc32 { get; set; } = 0;  /* crc32 of data payload                     */
    public uint magic { get; set; } = 0;
    public  byte[] data { get; set; }


    public ADBcommandType GetHumanReadableCommandType()
    {
      return  Enum.Parse<ADBcommandType>(System.Text.Encoding.ASCII.GetString(BitConverter.GetBytes(command)));
    }
    public ADBpacket(ADBcommandType command, uint arg0, uint arg1, string data)
    {
        this.command =  BitConverter.ToUInt32( System.Text.Encoding.ASCII.GetBytes(command.ToString()));
        this.arg0 = arg0;
        this.arg1 = arg1;
        if (string.IsNullOrWhiteSpace(data))
        {
            throw new ArgumentNullException(nameof(data));
        }
        var buffer = new byte[System.Text.Encoding.UTF8.GetByteCount(data) + 1];
        System.Text.Encoding.UTF8.GetBytes(data).CopyTo(buffer.AsSpan());
        buffer[^1] = 0;
        this.data = buffer;
        magic = 0XFFFFFFFF - this.command;
        data_length = (uint)data.Length + 1;
        data_crc32 = CalculateCrc32();
    }
    public ADBpacket(){}
    public ADBpacket(ADBcommandType command, uint arg0, uint arg1, byte[] data)
    {
        this.command =  BitConverter.ToUInt32( System.Text.Encoding.ASCII.GetBytes(command.ToString()));
        this.arg0 = arg0;
        this.arg1 = arg1;
        this.data = data ?? throw new ArgumentNullException(nameof(data));
        magic = 0XFFFFFFFF - this.command;
        data_length = (uint)data.Length;
        data_crc32 = CalculateCrc32();
    }
    private uint CalculateCrc32()
    {
        return data?.Aggregate<byte, uint>(0, (current, t) => (current + t) & 0xFFFFFFFF) ?? throw new ArgumentNullException($"Data is null");
    }
    public unsafe  byte[] ToByteArray()
    {
        Span<byte> buffer = stackalloc byte[24 + (int)data_length ];
        
        BinaryPrimitives.WriteUInt32LittleEndian(buffer,command );
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[4..],
            arg0);
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[8..],
            arg1);
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[12..],
            data_length);
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[16..],
            data_crc32);
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[20..],
            magic);
        if (data_length > 0)
        {
            data.CopyTo(buffer[24..]);
        }
        return buffer.ToArray();
    }
    public  static ADBpacket FromByteArray(byte[] buffer)
    {
        if (buffer.Length < 24)
        {
            throw new ArgumentException($"Buffer is too small");
        }
        var packet = new ADBpacket();
        packet.command = BinaryPrimitives.ReadUInt32LittleEndian(buffer);
        packet.arg0 = BinaryPrimitives.ReadUInt32LittleEndian(buffer[4..]);
        packet.arg1 = BinaryPrimitives.ReadUInt32LittleEndian(buffer[8..]);
        packet.data_length = BinaryPrimitives.ReadUInt32LittleEndian(buffer[12..]);
        packet.data_crc32 = BinaryPrimitives.ReadUInt32LittleEndian(buffer[16..]);
        packet.magic = BinaryPrimitives.ReadUInt32LittleEndian(buffer[20..]);
        if (packet.data_length > 0)
        {
            packet.data = buffer[24..].ToArray();
        }
        return packet;
    }
}