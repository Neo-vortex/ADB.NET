﻿using System.Buffers.Binary;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Security;

namespace ADBCrypto;

public class Class1
{
    private static byte[] convertRsaPublicKeyToAdbFormat(AsymmetricKeyParameter key , int keyLenght) {
        var KEY_LENGTH_WORDS = keyLenght / 4;
            
            var publicKey = (RsaKeyParameters)key;

            var r32 = BigInteger.Zero.SetBit(32);
            var n = publicKey.Modulus;
            var r = BigInteger.Zero.SetBit(KEY_LENGTH_WORDS * 32);
            var rr = r.ModPow(BigInteger.ValueOf(2), n);
            var rem = n.Remainder(r32);
            var n0inv = rem.ModInverse(r32);

            var myN = new int[KEY_LENGTH_WORDS];
            var myRr = new int[KEY_LENGTH_WORDS];
            BigInteger[] res;
            for (var i = 0; i < KEY_LENGTH_WORDS; i++) {
                res = rr.DivideAndRemainder(r32);
                rr = res[0];
                rem = res[1];
                myRr[i] = rem.IntValue;
                res = n.DivideAndRemainder(r32);
                n = res[0];
                rem = res[1];
                myN[i] = rem.IntValue;
            }

            /* ------------------------------------------------------------------------------------------- */

            /*ByteBuffer bbuf = ByteBuffer.allocate(524).order(ByteOrder.LITTLE_ENDIAN);*/
            var bbuf = new byte[524];

            BinaryPrimitives.WriteInt32LittleEndian( bbuf.AsSpan(0 ..4) , KEY_LENGTH_WORDS);
            BinaryPrimitives.WriteInt32LittleEndian(bbuf.AsSpan(4 .. 8) , n0inv.Negate().IntValue);
            var counter = 0;
            foreach (var variable in myN)
            {   
                BinaryPrimitives.WriteInt32LittleEndian(bbuf.AsSpan( new Range( 8+ (4*counter) , 12 + (4* counter))), variable);
                counter++;
            }
            counter = 0;
            foreach (var variable in myRr)
            {
                BinaryPrimitives.WriteInt32LittleEndian(bbuf.AsSpan(new Range(12 + (4 * KEY_LENGTH_WORDS) + (4 * counter) , 16 + (4 * KEY_LENGTH_WORDS) + (4 * counter))), variable);
                counter++;
            }
            BinaryPrimitives.WriteInt32LittleEndian(bbuf.AsSpan(16 + (4 * KEY_LENGTH_WORDS) + (4 * KEY_LENGTH_WORDS) .. 20 + (4 * KEY_LENGTH_WORDS) + (4 * KEY_LENGTH_WORDS)), publicKey.Exponent.IntValue);
            return bbuf;
        }
  
        public static string GetBase64Key(AsymmetricKeyParameter key)
        {
            var publicKey = (RsaKeyParameters)key;

            using (MemoryStream ms = new MemoryStream())
            {
                byte[] buff = publicKey.Exponent.ToByteArrayUnsigned();
                ms.Write(buff, 0, buff.Length);

                buff = publicKey.Modulus.ToByteArrayUnsigned();
                ms.Write(buff, 0, buff.Length);

                ms.Flush();
                return Convert.ToBase64String(ms.ToArray());
            }
        }
        public static AsymmetricCipherKeyPair GenerateKeyPair(int keySize)
        {
            DigestRandomGenerator randomGenerator = new DigestRandomGenerator(new MD5Digest());
            SecureRandom secureRandom = new SecureRandom(randomGenerator);
            var keyGenerationParameters = new KeyGenerationParameters(secureRandom, keySize);
            var keyPairGenerator = new RsaKeyPairGenerator();
            keyPairGenerator.Init(keyGenerationParameters);
            return keyPairGenerator.GenerateKeyPair();
        }
        public static byte[] SignDataSHA1(byte[] data, AsymmetricKeyParameter privateKey)
        {
            
            RSA rsak = RSA.Create(1024);  
            RSAParameters rsaKeyInfo = rsak.ExportParameters(true);  
            // Converting bouncy castle key to native csp.

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaKeyInfo);

                // Signing data.
                return rsa.SignHash(data, CryptoConfig.MapNameToOID("SHA1"));
            }
        }
}