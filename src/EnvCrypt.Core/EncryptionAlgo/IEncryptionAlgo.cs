using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.EncryptionAlgo
{
    public interface IEncryptionAlgo<in T> where T : KeyBase
    {
        byte[] Encrypt(byte[] stringToEncrypt, T usingKey);
        byte[] Decrypt(byte[] toDecrypt, T usingKey);
    }
}