using System;
using System.Collections.Generic;
using EnvCrypt.Core.EncryptionAlgo.Rsa.Key;
using EnvCrypt.Core.EncryptionAlgo.Rsa.Utils;

namespace EnvCrypt.Core.EncryptionAlgo.Rsa
{
    class RsaSegmentEncrypt : SegmentEncrypt<RsaKey>
    {
        private readonly IRsaMaxEncryptionCalc _maxEncryptionCalc;

        public RsaSegmentEncrypt(IEncryptionAlgo<RsaKey> encryptionAlgo, IRsaMaxEncryptionCalc maxEncryptionCalc)
            : base(encryptionAlgo)
        {
            _maxEncryptionCalc = maxEncryptionCalc;
        }


        public override IList<byte[]> Encrypt(byte[] binaryData, RsaKey usingKey)
        {
            // Get maximum bytes allowed and encrypt segments of binaryData based on this
            var maxBytestoEncrypt = _maxEncryptionCalc.GetMaxBytesThatCanBeEncrypted(usingKey);
            var ret = new List<byte[]>((int)Math.Max(1, Math.Ceiling((double)binaryData.Length / maxBytestoEncrypt)));

            for (int i = 0; i < binaryData.Length; i = i + maxBytestoEncrypt)
            {
                var currSegmentSize = Math.Min(binaryData.Length - i, maxBytestoEncrypt);
                var segmentToEncrypt = new byte[currSegmentSize];
                Buffer.BlockCopy(binaryData, i, segmentToEncrypt, 0, currSegmentSize);

                ret.Add(EncryptionAlgo.Encrypt(segmentToEncrypt, usingKey));
            }

            return ret;
        }
    }
}
