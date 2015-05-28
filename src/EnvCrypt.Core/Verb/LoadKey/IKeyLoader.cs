using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.Verb.LoadKey
{
    [ContractClass(typeof(KeyLoaderContracts<>))]
    public interface IKeyLoader<out TKey>
        where TKey : KeyBase
    {
        TKey Run(string ecKeyFilePath);
    }


    [ContractClassFor(typeof(IKeyLoader<>))]
    internal abstract class KeyLoaderContracts<TKey> : IKeyLoader<TKey> 
        where TKey : KeyBase
    {
        public TKey Run(string ecKeyFilePath)
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrWhiteSpace(ecKeyFilePath), "ecKeyFilePath");
            Contract.Ensures(Contract.Result<TKey>() != null);
            Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<TKey>().Name));
            return default(TKey);
        }
    }
}
