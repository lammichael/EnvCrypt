using EnvCrypt.Core.EncryptionAlgo;

namespace EnvCrypt.Core.Key.Aes
{
    public class AesKeyGenerationOptions : KeyGenerationOptions<AesKey>
    {
        public int KeySize { get; set; }
    }
}
