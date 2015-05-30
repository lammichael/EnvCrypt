using EnvCrypt.Core.EncryptionAlgo;

namespace EnvCrypt.Core.Key.Aes
{
    class AesKeyGenerationOptions : KeyGenerationOptions<AesKey>
    {
        public int KeySize { get; set; }
    }
}
