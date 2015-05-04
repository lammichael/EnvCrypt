using System.Security.Cryptography;

namespace EnvCrypt.Core.EncryptionAlgo.Aes
{
    public class AesKeyGenerator : IKeyGenerator<AesKey, AesGenerationOptions>
    {
        public AesKey GetNewKey(AesGenerationOptions options)
        {
            var generated = new AesKey();
            using (var myAes = new AesManaged())
            {
                myAes.KeySize = options.KeySize;
                myAes.GenerateIV();
                generated.Key = myAes.Key;
                generated.Iv = myAes.IV;
            }
            return generated;
        }
    }
}
