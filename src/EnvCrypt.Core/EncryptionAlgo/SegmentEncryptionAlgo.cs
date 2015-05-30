using System;
using System.Collections.Generic;
using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.EncryptionAlgo
{
    abstract class SegmentEncryptionAlgo<T> : ISegmentEncryptionAlgo<T>
        where T : KeyBase
    {
        protected IEncryptionAlgo<T> EncryptionAlgo;

        protected SegmentEncryptionAlgo(IEncryptionAlgo<T> encryptionAlgo)
        {
            EncryptionAlgo = encryptionAlgo;
        }

        
        public abstract IList<byte[]> Encrypt(byte[] binaryData, T usingKey);


        public byte[] Decrypt(IList<byte[]> segmentEncryptedData, T usingKey)
        {
            // Decrypt each array and store in array of arrays
            var decryptedBytes = new byte[segmentEncryptedData.Count][];
            var retLength = 0;
            for (var i = 0; i < segmentEncryptedData.Count; i++)
            {
                var currDecrypted = EncryptionAlgo.Decrypt(segmentEncryptedData[i], usingKey);
                decryptedBytes[i] = currDecrypted;
                retLength += currDecrypted.Length;
            }

            // Copy the byte arrays into one byte array
            var ret = new byte[retLength];
            var retIndex = 0;
            for (var i = 0; i < decryptedBytes.GetLength(0); i++)
            {
                var arrayToAdd = decryptedBytes[i];
                Buffer.BlockCopy(arrayToAdd, 0, ret, retIndex, arrayToAdd.Length);
                retIndex += arrayToAdd.Length;
            }

            return ret;
        }
    }
}
