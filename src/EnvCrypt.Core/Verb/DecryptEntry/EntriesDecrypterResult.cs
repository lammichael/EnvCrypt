using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.Verb.DecryptEntry
{
    public class EntriesDecrypterResult
    {
        public CategoryEntryPair CategoryEntryPair { get; set; }
        public string DecryptedValue { get; set; }
    }


    public class EntriesDecrypterResult<TKey> : EntriesDecrypterResult
        where TKey : KeyBase
    {
        public TKey DecryptedUsingKey { get; set; }
    }
}
