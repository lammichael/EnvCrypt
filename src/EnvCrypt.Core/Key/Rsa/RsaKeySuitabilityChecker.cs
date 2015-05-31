using EnvCrypt.Core.Key.Rsa.Utils;

namespace EnvCrypt.Core.Key.Rsa
{
    class RsaKeySuitabilityChecker : IKeySuitabilityChecker<RsaKey>
    {
        public bool IsEncryptingKey(RsaKey key)
        {
            var keyType = key.GetKeyType();
            return keyType == KeyTypeEnum.Public || keyType == KeyTypeEnum.Private;
        }

        public bool IsDecryptingKey(RsaKey key)
        {
            var keyType = key.GetKeyType();
            return keyType == KeyTypeEnum.Private;
        }
    }
}