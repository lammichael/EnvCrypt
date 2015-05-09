using EnvCrypt.Core.EncryptionAlgo.Aes;
using EnvCrypt.Core.EncryptionAlgo.Rsa;

namespace EnvCrypt.Core.Key
{
    public class Key
    {
        public RsaKey RsaKey { get; set; }
        public AesKey AesKey { get; set; }
    }
}
