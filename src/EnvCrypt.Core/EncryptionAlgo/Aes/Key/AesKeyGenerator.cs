using System.Diagnostics.Contracts;
using System.Security.Cryptography;

namespace EnvCrypt.Core.EncryptionAlgo.Aes.Key
{
    class AesKeyGenerator : IKeyGenerator<AesKey, AesKeyGenerationOptions>
    {
        public AesKey GetNewKey(AesKeyGenerationOptions options)
        {
            Contract.Ensures(Contract.Result<AesKey>().Iv != null);
            Contract.Ensures(Contract.Result<AesKey>().Iv.Length > 0);
            Contract.Ensures(Contract.Result<AesKey>().Key != null);
            Contract.Ensures(Contract.Result<AesKey>().Key.Length > 0);
            //
            /*
             * Exception when key size is invalid:
             * 'System.Security.Cryptography.CryptographicException' Specified key is not a valid size for this algorithm.
             * Check placed here because all Contract.Requires must be at the interface level.
             */
            if(!(options.KeySize >= 128))
            {
                throw new EnvCryptAlgoException("AES key size must be >= 128");
            }
            if(!(options.KeySize <= 256))
            {
                throw new EnvCryptAlgoException("AES key size must be <= 256");
            }

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
