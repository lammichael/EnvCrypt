using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Utils;

namespace EnvCrypt.Core.Key.Aes
{
    public class AesKey : KeyBase, ISymmetricKeyMarker
    {
        public override EnvCryptAlgoEnum Algorithm
        {
            get { return EnvCryptAlgoEnum.Aes; }
        }

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
                return ((Key != null ? ByteArrayHashUtils.GetHashCode(Key) : 0) * 397) ^ 
                    (Iv != null ? ByteArrayHashUtils.GetHashCode(Iv) : 0);
            }
        }
    }
}
