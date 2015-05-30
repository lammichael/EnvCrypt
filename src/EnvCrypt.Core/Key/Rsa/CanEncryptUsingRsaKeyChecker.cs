using EnvCrypt.Core.Key.Rsa.Utils;

namespace EnvCrypt.Core.Key.Rsa
{
    class CanEncryptUsingRsaKeyChecker : ICanEncryptUsingKeyChecker<RsaKey>
    {
        public bool IsEncryptingKey(RsaKey key)
        {
            return key.GetKeyType() == KeyTypeEnum.Private;
        }
    }
}