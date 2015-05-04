using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.EncryptionAlgo.Rsa
{
    public static class RsaMaxEncryptSizeUtils
    {
        /*
        * if the optimal asymmetric encryption padding (OAEP) parameter is true:
        * ((KeySize - 384) / 8) + 7
        * Without OAEP:
        * ((KeySize - 384) / 8) + 37
         * Taken from http://stackoverflow.com/questions/1496793/rsa-encryption-getting-bad-length
        */
        public static int GetMaxBytesThatCanBeEncrypted(this RsaKey key)
        {
            Contract.Requires<ArgumentNullException>(key != null, "key");
            //
            var keySize = key.GetKeySize();
            if (key.UseOaepPadding)
            {
                return ((keySize - 384)/8) + 7;
            }
            else
            {
                return ((keySize - 384)/8) + 37;
            }
        }


        /// <summary>
        /// Gets the key size in bytes.
        /// </summary>
        public static int GetKeySize(this RsaKey key)
        {
            Contract.Requires<ArgumentNullException>(key != null, "key");
            Contract.Requires<EnvCryptAlgoException>(key.Key.Modulus != null,
                "getting key size with null modulus in RSA key");
            Contract.Requires<EnvCryptAlgoException>(key.Key.Modulus.Length != 0,
                "getting key size with empty modulus in RSA key");
            Contract.Ensures(Contract.Result<int>() > 0);
            //
            return key.Key.Modulus.Length * 8;
        }
    }
}
