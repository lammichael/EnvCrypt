using System;
using System.Diagnostics.Contracts;

namespace EnvCrypt.Core.Key
{
    [ContractClass(typeof(KeySuitabilityCheckerContracts<>))]
    public interface IKeySuitabilityChecker<in TKey>
        where TKey : KeyBase
    {
        bool IsEncryptingKey(TKey key);
        bool IsDecryptingKey(TKey key);
    }


    [ContractClassFor(typeof(IKeySuitabilityChecker<>))]
    internal abstract class KeySuitabilityCheckerContracts<TKey> : IKeySuitabilityChecker<TKey> where TKey : KeyBase
    {
        public bool IsEncryptingKey(TKey key)
        {
            Contract.Requires<ArgumentNullException>(key != null, "key");

            return default(bool);
        }

        public bool IsDecryptingKey(TKey key)
        {
            Contract.Requires<ArgumentNullException>(key != null, "key");

            return default(bool);
        }
    }
}