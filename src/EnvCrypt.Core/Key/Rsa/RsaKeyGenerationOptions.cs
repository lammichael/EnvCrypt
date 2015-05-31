using EnvCrypt.Core.EncryptionAlgo;

namespace EnvCrypt.Core.Key.Rsa
{
    class RsaKeyGenerationOptions : KeyGenerationOptions<RsaKey>
    {
        public int KeySize { get; set; }
        public bool UseOaepPadding { get; set; }
    }
}
