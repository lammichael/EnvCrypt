using EnvCrypt.Core.EncryptionAlgo;

namespace EnvCrypt.Core.Key
{
    public abstract class KeyBase
    {
        public abstract EnvCryptAlgoEnum Algorithm { get; }

        /// <summary>
        /// Human readable name of the key.
        /// </summary>
        public string Name { get; set; }
    }
}
