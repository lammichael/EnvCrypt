using System;
using System.Diagnostics.Contracts;

namespace EnvCrypt.Core.EncryptionAlgo.Rsa.Key
{
    class RsaKeyGenerationOptions : KeyGenerationOptions<RsaKey>
    {
        public int KeySize { get; private set; }
        public bool UseOaepPadding { get; private set; }


        public RsaKeyGenerationOptions(int keySize, bool useOaepPadding)
        {
            Contract.Requires<ArgumentException>(keySize >= 384);
            //
            KeySize = keySize;
            UseOaepPadding = useOaepPadding;
        }
    }
}
