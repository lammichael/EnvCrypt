using System.Security.Cryptography;
using EnvCrypt.Core.Key.Aes;

namespace EnvCrypt.Core.EncryptionAlgo.Aes
{
    public class AesAlgo : IEncryptionAlgo<AesKey>
    {
        public byte[] Encrypt(byte[] binaryData, AesKey usingKey)
        {
            using (var myAes = new AesManaged())
            {
                myAes.IV = usingKey.Iv;
                myAes.Key = usingKey.Key;
                return SymmetricEncyptionUtils.EncryptBytes(myAes, binaryData);
            }
        }


        public byte[] Decrypt(byte[] binaryData, AesKey usingKey)
        {
            using (var myAes = new AesManaged())
            {
                myAes.IV = usingKey.Iv;
                myAes.Key = usingKey.Key;
                return SymmetricEncyptionUtils.DecryptBytes(myAes, binaryData);
            }
        }
    }
}
