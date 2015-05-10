using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncryptionAlgo.Rsa.Key;

namespace EnvCrypt.Core.EncryptionAlgo.Rsa.Utils
{
    public static class RsaKeySizeUtils
    {
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
