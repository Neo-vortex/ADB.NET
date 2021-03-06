using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace AndroidDebuggingBridgeKeyring;

public class AutoGeneratedInMemoryKeyring : IKeyring
{
    private AsymmetricCipherKeyPair _asymetricKey;

    public AutoGeneratedInMemoryKeyring()
    {
        var result = ADBCrypto.CryptoLib.GenerateKeyPair(1024);
        PrivateKey = ADBCrypto.CryptoLib.GetBase64Key(result.Public);
        PrivateKey = ADBCrypto.CryptoLib.GetBase64Key(result.Public);
        _asymetricKey = result;
    }


    public string PrivateKey { get; set; }
    public string PublicKey { get; set; }

    public byte[] Sign(byte[] data)
    {
        // Converting bouncy castle key to native csp.
        var rsaParam =  ADBCrypto.CryptoLib.ToRsaParameters(_asymetricKey.Private as RsaPrivateCrtKeyParameters ?? throw new InvalidOperationException());

        using var rsa = new RSACryptoServiceProvider();
        rsa.ImportParameters(rsaParam);

        // Signing data.
        return rsa.SignHash(data, CryptoConfig.MapNameToOID("SHA1"));
    }

    public bool Verify(byte[] data, byte[] msg, byte[] signature)
    {
        try
        {
            var msgBytes = msg;
            var sigBytes = signature;
            var signer = SignerUtilities.GetSigner("SHA-256withRSA");
            signer.Init(false, _asymetricKey.Public);
            signer.BlockUpdate(msgBytes, 0, msgBytes.Length);
            return signer.VerifySignature(sigBytes);
        }
        catch (Exception exc)
        {
            return false;
        }
    }
}