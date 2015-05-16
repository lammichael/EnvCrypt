using System;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using EnvCrypt.Core.EncryptionAlgo.Poco;

namespace EnvCrypt.Core.EncryptionAlgo.Rsa.Key
{
    /// <summary>
    /// RSA key data and metadata.
    /// </summary>
    public class RsaKey : KeyBase
    {
        public RSAParameters Key { get; private set; }

        /// <summary>
        /// True for OAEP padding (PKCS #1 v2), false for PKCS #1 type 2 padding
        /// </summary>
        public bool UseOaepPadding { get; private set; }


        public RsaKey(RSAParameters key, bool useOaepPadding)
        {
            UseOaepPadding = useOaepPadding;
            Key = key;
        }


        protected bool Equals(RsaKey other)
        {
            Contract.Requires<ArgumentNullException>(other != null, "other");
            //
            return Key.Equals(other.Key) && UseOaepPadding.Equals(other.UseOaepPadding);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RsaKey) obj);
        }

        /// <summary>
        /// Hash code only uses Modulus & Exponent because then the hash will be identical
        /// for the private and public key.
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                return
                    ((Key.Modulus != null ? Key.Modulus.GetHashCode() : 0) * 397) ^ 
                    ((Key.Exponent != null ? Key.Exponent.GetHashCode() : 0) * 397) ^ 
                    UseOaepPadding.GetHashCode();
            }
        }
    }
}
