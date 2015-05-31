using EnvCrypt.Core.Key.Rsa.Utils;

namespace EnvCrypt.Core.Key.Rsa
{
    class CanEncryptUsingRsaKeyChecker : ICanEncryptUsingKeyChecker<RsaKey>
    {
        public bool IsEncryptingKey(RsaKey key)
        {
            var keyType = key.GetKeyType();
            return keyType == KeyTypeEnum.Public || keyType == KeyTypeEnum.Private;
        }
    }
}