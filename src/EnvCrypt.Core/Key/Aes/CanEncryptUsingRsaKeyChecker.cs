using System.Linq;

namespace EnvCrypt.Core.Key.Aes
{
    class CanEncryptUsingAesKeyChecker : ICanEncryptUsingKeyChecker<AesKey>
    {
        public bool IsEncryptingKey(AesKey key)
        {
            return key.Iv != null && key.Iv.Any() && key.Key != null && key.Key.Any();
        }
    }
}