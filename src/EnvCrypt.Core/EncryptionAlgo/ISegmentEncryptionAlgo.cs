using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.EncryptionAlgo
{
    [ContractClass(typeof(SegmentEncryptionAlgoContracts<>))]
    public interface ISegmentEncryptionAlgo<in TKey> where TKey : KeyBase
    {
        IList<byte[]> Encrypt(byte[] binaryData, TKey usingKey);
        byte[] Decrypt(IList<byte[]> segmentEncryptedData, TKey usingKey);
    }


    [ContractClassFor(typeof(ISegmentEncryptionAlgo<>))]
    internal abstract class SegmentEncryptionAlgoContracts<TKey> : ISegmentEncryptionAlgo<TKey>
        where TKey : KeyBase
    {
        public IList<byte[]> Encrypt(byte[] binaryData, TKey usingKey)
        {
            Contract.Requires<ArgumentNullException>(binaryData != null, "binaryData");
            Contract.Requires<ArgumentNullException>(usingKey != null, "usingKey");
            Contract.Ensures(Contract.Result<IList<byte[]>>() != null);
            Contract.Ensures(Contract.Result<IList<byte[]>>().Any());
            Contract.Ensures(Contract.ForAll(Contract.Result<IList<byte[]>>(), b => b.Any()));

            return default(IList<byte[]>);
        }


        public byte[] Decrypt(IList<byte[]> segmentEncryptedData, TKey usingKey)
        {
            Contract.Requires<ArgumentNullException>(segmentEncryptedData != null, "binaryData");
            Contract.Requires<ArgumentNullException>(usingKey != null, "usingKey");
            Contract.Ensures(Contract.Result<byte[]>() != null);

            return default(byte[]);
        }
    }
}