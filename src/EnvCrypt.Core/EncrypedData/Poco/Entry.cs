using System.Collections.Generic;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.EncrypedData.Poco
{
    /// <summary>
    /// Contains the encrypted value and the encryption key's uniquely identifying information.
    /// <code>EncryptionAlgorithm</code> and <code>KeyHash</code> properties
    /// could be derived from the key object but this means we require all the keys that
    /// have been used to encrypt the all the entries in the DAT file to write
    /// the DAT file back.
    /// </summary>
    public class Entry
    {
        public string Name { get; set; }

        /// <summary>
        /// A list is used to cater to encrypt data that exceeds the maximum 
        /// allowed for the algorithm.
        /// </summary>
        public IList<byte[]> EncryptedValue { get; set; }
        
        public string KeyName { get; set; }
        public EnvCryptAlgoEnum EncryptionAlgorithm { get; set; }
        public int KeyHash { get; set; }
    }
}