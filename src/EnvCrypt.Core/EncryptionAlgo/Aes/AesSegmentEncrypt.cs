using System.Collections.Generic;
using EnvCrypt.Core.EncryptionAlgo.Aes.Key;

namespace EnvCrypt.Core.EncryptionAlgo.Aes
{
    class AesSegmentEncrypt : SegmentEncrypt<AesKey>
    {
        public AesSegmentEncrypt(IEncryptionAlgo<AesKey> encryptionAlgo) : base(encryptionAlgo)
        {}

        public override IList<byte[]> Encrypt(byte[] binaryData, AesKey usingKey)
        {
            return new List<byte[]>()
            {
                EncryptionAlgo.Encrypt(binaryData, usingKey)
            };
        }
    }
}
