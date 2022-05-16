using System.Buffers.Binary;
using System.Text;
using ADB.NET.Interfaces;

namespace ADB.NET.DataTypes.ABDpacket;

public class ADBheader
{
    public ADBheader(ICommandParsable command, uint arg0, uint arg1)
    {
        this.command = BitConverter.ToUInt32(Encoding.ASCII.GetBytes(command.GetCommand()));
        this.arg0 = arg0;
        this.arg1 = arg1;
        magic = Utilities.Consts._MAGIC_CONST - this.command;
    }
    public ADBheader(string command, uint arg0, uint arg1)
    {
        this.command = BitConverter.ToUInt32(Encoding.ASCII.GetBytes(command));
        this.arg0 = arg0;
        this.arg1 = arg1;
        magic = Utilities.Consts._MAGIC_CONST - this.command;
    }
    
    public  byte[] ToByteArray()
    {
        Span<byte> buffer = stackalloc byte[24 + (int)this.data_length];

        BinaryPrimitives.WriteUInt32LittleEndian(buffer, this.command);
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[4..],
            this.arg0);
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[8..],
            this.arg1);
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[12..],
            this.data_length);
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[16..],
            this.data_crc32);
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[20..],
            this.magic);
        return buffer.ToArray();
    }
    
    public static ADBheader FromByteArray(byte[] buffer )
    {
        var header =  new ADBheader();
        header.command = BinaryPrimitives.ReadUInt32LittleEndian(buffer);
        header.arg0 = BinaryPrimitives.ReadUInt32LittleEndian(buffer[4..]);
        header.arg1 = BinaryPrimitives.ReadUInt32LittleEndian(buffer[8..]);
        header.data_length = BinaryPrimitives.ReadUInt32LittleEndian(buffer[12..]);
        header.data_crc32 = BinaryPrimitives.ReadUInt32LittleEndian(buffer[16..]);
        header.magic = BinaryPrimitives.ReadUInt32LittleEndian(buffer[20..]);
        return header;
    }
    public ADBheader()
    {
    }

    public uint command { get; set; } /* command identifier constant (A_CNXN, ...) */
    public uint arg0 { get; set; } /* first argument                            */
    public uint arg1 { get; set; } /* second argument                           */
    public uint data_length { get; set; } = 0; /* length of payload (0 is allowed)          */
    public uint data_crc32 { get; set; } = 0; /* crc32 of data payload                     */
    public uint magic { get; set; } /* command ^ 0xffffffff                      */
}