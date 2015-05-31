using System.Linq;

namespace EnvCrypt.Core.Key.Aes
{
    class AesKeySuitabilityChecker : IKeySuitabilityChecker<AesKey>
    {
        public bool IsEncryptingKey(AesKey key)
        {
            return key.Iv != null && key.Iv.Any() && key.Key != null && key.Key.Any();
        }

        public bool IsDecryptingKey(AesKey key)
        {
            return this.IsEncryptingKey(key);
        }
    }
}