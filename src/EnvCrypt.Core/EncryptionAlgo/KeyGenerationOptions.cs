using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.EncryptionAlgo
{
    public abstract class KeyGenerationOptions<TKey> where TKey : KeyBase
    {
        public string NewKeyName { get; set; }
    }
}
