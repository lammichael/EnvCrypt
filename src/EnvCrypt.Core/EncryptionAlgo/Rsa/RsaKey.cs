using System.Security.Cryptography;

namespace EnvCrypt.Core.EncryptionAlgo.Rsa
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
        /// Hash code uses <code>GetHashCode()</code> for the RSAParameters class.
        /// This returns the same hash code even when the arrays are different values.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Key.GetHashCode()*397) ^ UseOaepPadding.GetHashCode();
            }
        }
    }
}
