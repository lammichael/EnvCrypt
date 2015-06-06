using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.Verb.DecryptEntry
{
    public class EntriesDecrypterResult<TKey>
        where TKey : KeyBase
    {
        public CategoryEntryPair CategoryEntryPair { get; set; }
        public string DecryptedValue { get; set; }
        public TKey DecryptedUsingKey { get; set; }
    }
}
