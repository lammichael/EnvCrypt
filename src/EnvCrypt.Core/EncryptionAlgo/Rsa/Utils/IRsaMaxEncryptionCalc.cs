using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncryptionAlgo.Rsa.Key;

namespace EnvCrypt.Core.EncryptionAlgo.Rsa.Utils
{
    [ContractClass(typeof(RsaMaxEncryptionCalcContracts))]
    internal interface IRsaMaxEncryptionCalc
    {
        int GetMaxBytesThatCanBeEncrypted(RsaKey key);
    }


    [ContractClassFor(typeof(IRsaMaxEncryptionCalc))]
    internal abstract class RsaMaxEncryptionCalcContracts : IRsaMaxEncryptionCalc
    {
        public int GetMaxBytesThatCanBeEncrypted(RsaKey key)
        {
            Contract.Requires<ArgumentNullException>(key != null, "key");
            Contract.Ensures(Contract.Result<int>() > 0);

            return default(int);
        }
    }
}