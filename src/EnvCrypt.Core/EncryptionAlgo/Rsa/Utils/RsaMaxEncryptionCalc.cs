using EnvCrypt.Core.Key.Rsa;
using EnvCrypt.Core.Key.Rsa.Utils;

namespace EnvCrypt.Core.EncryptionAlgo.Rsa.Utils
{
    class RsaMaxEncryptionCalc : IRsaMaxEncryptionCalc
    {
        /*
        * if the optimal asymmetric encryption padding (OAEP) parameter is true:
        * ((KeySize - 384) / 8) + 7
        * Without OAEP:
        * ((KeySize - 384) / 8) + 37
         * Taken from http://stackoverflow.com/questions/1496793/rsa-encryption-getting-bad-length
        */
        public int GetMaxBytesThatCanBeEncrypted(RsaKey key)
        {
            var keySize = key.GetKeySize();
            if (key.UseOaepPadding)
            {
                return ((keySize - 384) / 8) + 7;
            }
            else
            {
                return ((keySize - 384) / 8) + 37;
            }
        }
    }
}
