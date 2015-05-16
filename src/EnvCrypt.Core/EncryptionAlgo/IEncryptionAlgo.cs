using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncryptionAlgo.Poco;
using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.EncryptionAlgo
{
    [ContractClass(typeof(EncryptionAlgoContracts<>))]
    public interface IEncryptionAlgo<in T> where T : KeyBase
    {
        byte[] Encrypt(byte[] binaryData, T usingKey);
        byte[] Decrypt(byte[] binaryData, T usingKey);
    }


    [ContractClassFor(typeof(IEncryptionAlgo<>))]
    internal abstract class EncryptionAlgoContracts<T> : IEncryptionAlgo<T> where T : KeyBase
    {
        public byte[] Encrypt(byte[] binaryData, T usingKey)
        {
            Contract.Requires<ArgumentNullException>(binaryData != null, "binaryData");
            Contract.Requires<ArgumentNullException>(usingKey != null, "usingKey");
            Contract.Ensures(Contract.Result<byte[]>() != null, "decrypted value cannot null");

            return default(byte[]);
        }

        public byte[] Decrypt(byte[] binaryData, T usingKey)
        {
            Contract.Requires<ArgumentNullException>(binaryData != null, "binaryData");
            Contract.Requires<ArgumentNullException>(usingKey != null, "usingKey");
            Contract.Ensures(Contract.Result<byte[]>() != null, "decrypted value cannot be null");

            return default(byte[]);
        }
    }
}