using EnvCrypt.Core.EncryptionAlgo;

namespace EnvCrypt.Core.Utils
{
    /// <summary>
    /// Defines how to convert from text to binary and vice versa.
    /// Requirement to pass in <code>EnvCryptAlgoEnum</code> is that
    /// we want the plaintext entires to be readable in the EC DAT.
    /// </summary>
    interface IEncryptedDetailsPersistConverter
    {
        string Encode(byte[] dataToPersist, EnvCryptAlgoEnum algorithm);
        byte[] Decode(string persistedStr, EnvCryptAlgoEnum algorithm);
    }
}