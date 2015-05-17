using System.Dynamic;
using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.EncryptionAlgo
{
    abstract class KeyGenerationOptions<TKey> where TKey : KeyBase
    {
        public string NewKeyName { get; set; }
    }
}
