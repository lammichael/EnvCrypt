using System;
using System.Diagnostics.Contracts;

namespace EnvCrypt.Core.EncryptionAlgo.Rsa
{
    public class RsaGenerationOptions : IKeyGenerationOptions
    {
        public int KeySize { get; private set; }
        public bool UseOaepPadding { get; private set; }


        public RsaGenerationOptions(int keySize, bool useOaepPadding)
        {
            Contract.Requires<ArgumentException>(keySize >= 384);
            //
            KeySize = keySize;
            UseOaepPadding = useOaepPadding;
        }
    }
}
