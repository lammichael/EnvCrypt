using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Key.PlainText;

namespace EnvCrypt.Core.Verb.LoadKey
{
    /// <summary>
    /// Loads the RSA key. Could be a private or public key.
    /// </summary>
    [ContractClass(typeof(KeyLoaderContracts<,>))]
    public interface IKeyLoader<out TKey, in TLoadDetails>
        where TKey : KeyBase
    {
        TKey Load(TLoadDetails keyLoadDetails);
    }


    [ContractClassFor(typeof(IKeyLoader<,>))]
    internal abstract class KeyLoaderContracts<TKey, TLoadDetails> : IKeyLoader<TKey, TLoadDetails> 
        where TKey : KeyBase
    {
        public TKey Load(TLoadDetails ecKeyLoadDetails)
        {
            Contract.Ensures(Contract.Result<TKey>() != null);
            Contract.Ensures(typeof(TKey) == typeof(PlainTextKey) || 
                !String.IsNullOrEmpty(Contract.Result<TKey>().Name));
            return default(TKey);
        }
    }
}
