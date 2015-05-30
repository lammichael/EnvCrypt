using System.Collections.Generic;
using EnvCrypt.Core.Key.PlainText;

namespace EnvCrypt.Core.EncryptionAlgo.PlainText
{
    class PlainTextSegmentEncryptionAlgo : ISegmentEncryptionAlgo<PlainTextKey>
    {
        public IList<byte[]> Encrypt(byte[] binaryData, PlainTextKey usingKey)
        {
            return new[] {binaryData};
        }


        public byte[] Decrypt(IList<byte[]> segmentEncryptedData, PlainTextKey usingKey)
        {
            return segmentEncryptedData[0];
        }
    }
}