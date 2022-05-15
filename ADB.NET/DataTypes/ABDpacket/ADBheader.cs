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