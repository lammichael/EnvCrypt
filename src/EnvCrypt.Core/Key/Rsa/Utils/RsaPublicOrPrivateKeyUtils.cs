using System.Diagnostics.Contracts;

namespace EnvCrypt.Core.Key.Rsa.Utils
{
    static class RsaPublicOrPrivateKeyUtils
    {
        /// <summary>
        /// To find out if the RSA key is a public or private key.
        /// Throws EnvCryptException if there is insufficient data for either.
        /// </summary>
        [Pure]
        public static KeyTypeEnum GetKeyType(this RsaKey forKey, bool throwExceptionWhenPublicKeyHasTooMuchInfo = true)
        {
            var key = forKey.Key;
            if ((key.D != null && key.D.Length > 0)
                && (key.DP != null && key.DP.Length > 0)
                && (key.DQ != null && key.DQ.Length > 0)
                && (key.Exponent != null && key.Exponent.Length > 0)
                && (key.InverseQ != null && key.InverseQ.Length > 0)
                && (key.Modulus != null && key.Modulus.Length > 0)
                && (key.P != null && key.P.Length > 0)
                && (key.Q != null && key.Q.Length > 0))
            {
                return KeyTypeEnum.Private;
            }

            if ((key.Exponent != null && key.Exponent.Length > 0)
                && (key.Modulus != null && key.Modulus.Length > 0))
            {
                if (throwExceptionWhenPublicKeyHasTooMuchInfo)
                {
                    if ((key.D != null && key.D.Length > 0)
                        || (key.DP != null && key.DP.Length > 0)
                        || (key.DQ != null && key.DQ.Length > 0)
                        || (key.InverseQ != null && key.InverseQ.Length > 0)
                        || (key.P != null && key.P.Length > 0)
                        || (key.Q != null && key.Q.Length > 0))
                    {
                        throw new EnvCryptException(
                            "RSA public key does not have enough data to be a private key but has information from the private key");
                    }
                }
                return KeyTypeEnum.Public;
            }
            throw new EnvCryptException("RSA key does not have enough data to be a public or private key");
        }
    }
}
