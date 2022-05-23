namespace AndroidDebuggingBridgeKeyring;

public interface IKeyring
{
    public string PrivateKey();
    public string PublicKey();
    public byte[] Sign(byte[] data);
    public bool Verify(byte[] data, byte[] signature);
}