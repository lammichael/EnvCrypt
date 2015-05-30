using System;
using System.Diagnostics.Contracts;

namespace EnvCrypt.Core.Key
{
    [ContractClass(typeof(CanEncryptUsingKeyCheckerContracts<>))]
    public interface ICanEncryptUsingKeyChecker<in TKey>
        where TKey : KeyBase
    {
        bool IsEncryptingKey(TKey key);
    }


    [ContractClassFor(typeof(ICanEncryptUsingKeyChecker<>))]
    internal abstract class CanEncryptUsingKeyCheckerContracts<TKey> : ICanEncryptUsingKeyChecker<TKey> where TKey : KeyBase
    {
        public bool IsEncryptingKey(TKey key)
        {
            Contract.Requires<ArgumentNullException>(key != null, "key");

            return default(bool);
        }
    }
}