namespace AndroidDebuggingBridgeKeyring;

public class InMemoryKeyring : IKeyring
{
    public string PrivateKey()
    {
        throw new NotImplementedException();
    }

    public string PublicKey()
    {
        throw new NotImplementedException();
    }

    public byte[] Sign(byte[] data)
    {
        throw new NotImplementedException();
    }

    public bool Verify(byte[] data, byte[] signature)
    {
        throw new NotImplementedException();
    }
}