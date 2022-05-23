namespace AndroidDebuggingBridgeKeyring;

public interface IKeyring
{
    public string PrivateKey { get; set; }
    public string PublicKey { get; set; }
    public byte[] Sign(byte[] data);
    public bool Verify(byte[] data, byte[] mesg , byte[] signature);
}