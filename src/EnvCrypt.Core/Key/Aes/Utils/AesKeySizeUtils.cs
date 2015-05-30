using System;
using System.Diagnostics.Contracts;

namespace EnvCrypt.Core.Key.Aes.Utils
{
    public static class AesKeySizeUtils
    {
        /// <summary>
        /// Gets the key size of an AES key.
        /// </summary>
        public static int GetKeySize(this AesKey key)
        {
            Contract.Requires<ArgumentNullException>(key != null, "key");
            Contract.Requires<ArgumentNullException>(key.Key != null, "key");
            Contract.Ensures(Contract.Result<int>() > 0);
            //
            return key.Key.Length*8;
        }
    }
}
