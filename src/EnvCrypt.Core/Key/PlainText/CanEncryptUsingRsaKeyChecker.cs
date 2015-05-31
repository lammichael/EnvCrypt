namespace EnvCrypt.Core.Key.PlainText
{
    class PlainTextKeySuitabilityChecker : IKeySuitabilityChecker<PlainTextKey>
    {
        public bool IsEncryptingKey(PlainTextKey key)
        {
            return true;
        }

        public bool IsDecryptingKey(PlainTextKey key)
        {
            return true;
        }
    }
}