using System.Diagnostics.Contracts;
using System.Security.Cryptography;

namespace EnvCrypt.Core.EncryptionAlgo.Aes
{
    public class AesKeyGenerator : IKeyGenerator<AesKey, AesGenerationOptions>
    {
        public AesKey GetNewKey(AesGenerationOptions options)
        {
            Contract.Requires<EnvCryptAlgoException>(options.KeySize >= 128,
                "AES key size must be >= 128");
            Contract.Requires<EnvCryptAlgoException>(options.KeySize <= 256,
                "AES key size must be <= 128");
            Contract.Ensures(Contract.Result<AesKey>().Iv != null);
            Contract.Ensures(Contract.Result<AesKey>().Iv.Length > 0);
            Contract.Ensures(Contract.Result<AesKey>().Key != null);
            Contract.Ensures(Contract.Result<AesKey>().Key.Length > 0);
            //
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
