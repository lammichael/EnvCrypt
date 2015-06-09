using EnvCrypt.Core.EncryptionAlgo;

namespace EnvCrypt.Core.Key.Rsa
{
    public class RsaKeyGenerationOptions : KeyGenerationOptions<RsaKey>
    {
        public int KeySize { get; set; }
        public bool UseOaepPadding { get; set; }
    }
}
