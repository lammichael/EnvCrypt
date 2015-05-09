using System;
using System.Diagnostics.Contracts;

namespace EnvCrypt.Core.EncryptionAlgo.Aes
{
    public class AesKey : KeyBase
    {
        public byte[] Key { get; set; }
        public byte[] Iv { get; set; }


        protected bool Equals(AesKey other)
        {
            Contract.Requires<ArgumentNullException>(other != null, "other");
            //
            return Equals(Key, other.Key) && Equals(Iv, other.Iv);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AesKey) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Key != null ? Key.GetHashCode() : 0)*397) ^ (Iv != null ? Iv.GetHashCode() : 0);
            }
        }
    }
}
