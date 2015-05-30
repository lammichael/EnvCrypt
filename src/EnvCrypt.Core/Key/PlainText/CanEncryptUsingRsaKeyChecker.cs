namespace EnvCrypt.Core.Key.PlainText
{
    class CanEncryptUsingPlainTextKeyChecker : ICanEncryptUsingKeyChecker<PlainTextKey>
    {
        public bool IsEncryptingKey(PlainTextKey key)
        {
            return true;
        }
    }
}