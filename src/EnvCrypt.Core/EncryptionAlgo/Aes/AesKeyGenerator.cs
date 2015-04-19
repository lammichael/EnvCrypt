using System.Security.Cryptography;
using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.EncryptionAlgo.Aes
{
    public class AesKeyGenerator : IKeyGenerator<AesKey>
    {
        public AesKey GetNewKey()
        {
            var generated = new AesKey();
            using (var myAes = new AesManaged())
            {
                myAes.GenerateIV();
                generated.Key = myAes.Key;
                generated.Iv = myAes.IV;
            }
            return generated;
        }
    }
}
