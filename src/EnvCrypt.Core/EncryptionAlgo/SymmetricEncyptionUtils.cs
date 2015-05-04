using System;
using System.IO;
using System.Security.Cryptography;

namespace EnvCrypt.Core.EncryptionAlgo
{
    internal class SymmetricEncyptionUtils
    {
        public static byte[] EncryptBytes(SymmetricAlgorithm alg, byte[] message)
        {
            if ((message == null) || (message.Length == 0))
            {
                return message;
            }

            if (alg == null)
            {
                throw new ArgumentNullException("alg");
            }

            using (var stream = new MemoryStream())
            using (var encryptor = alg.CreateEncryptor())
            using (var encrypt = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
            {
                encrypt.Write(message, 0, message.Length);
                encrypt.FlushFinalBlock();
                return stream.ToArray();
            }
        }


        public static byte[] DecryptBytes(SymmetricAlgorithm alg, byte[] message)
        {
            if ((message == null) || (message.Length == 0))
            {
                return message;
            }

            if (alg == null)
            {
                throw new ArgumentNullException("alg");
            }

            using (var stream = new MemoryStream())
            using (var decryptor = alg.CreateDecryptor())
            using (var encrypt = new CryptoStream(stream, decryptor, CryptoStreamMode.Write))
            {
                encrypt.Write(message, 0, message.Length);
                encrypt.FlushFinalBlock();
                return stream.ToArray();
            }
        }
    }
}
