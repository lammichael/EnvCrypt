using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Key.PlainText;

namespace EnvCrypt.Core.Verb.LoadKey
{
    /// <summary>
    /// Loads the RSA key. Could be a private or public key.
    /// </summary>
    [ContractClass(typeof(KeyLoaderContracts<>))]
    public interface IKeyLoader<out TKey>
        where TKey : KeyBase
    {
        TKey Load(string ecKeyFilePath);
    }


    [ContractClassFor(typeof(IKeyLoader<>))]
    internal abstract class KeyLoaderContracts<TKey> : IKeyLoader<TKey> 
        where TKey : KeyBase
    {
        public TKey Load(string ecKeyFilePath)
        {
            Contract.Requires<ArgumentException>(typeof(TKey) == typeof(PlainTextKey) || 
                !string.IsNullOrWhiteSpace(ecKeyFilePath),
                "key file path not specified");
            Contract.Ensures(Contract.Result<TKey>() != null);
            Contract.Ensures(typeof(TKey) == typeof(PlainTextKey) || 
                !String.IsNullOrEmpty(Contract.Result<TKey>().Name));
            return default(TKey);
        }
    }
}
