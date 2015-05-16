﻿using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncryptionAlgo.Poco;

namespace EnvCrypt.Core.EncryptionAlgo
{
    /// <summary>
    /// To generate new keys
    /// </summary>
    /// <typeparam name="T">Key type</typeparam>
    /// <typeparam name="TO">Key generation options</typeparam>
    [ContractClass(typeof(KeyGeneratorContracts<,>))]
    public interface IKeyGenerator<out T, in TO>
        where T : KeyBase
        where TO : class, IKeyGenerationOptions<T>
    {
        T GetNewKey(TO options);
    }


    [ContractClassFor(typeof(IKeyGenerator<,>))]
    internal abstract class KeyGeneratorContracts<T, TO> : IKeyGenerator<T, TO>
        where T : KeyBase
        where TO : class, IKeyGenerationOptions<T>
    {
        public T GetNewKey(TO options)
        {
            Contract.Requires<ArgumentNullException>(options != null, "options");
            Contract.Ensures(Contract.Result<T>() != null,
                "generated key should not be null");

            return default(T);
        }
    }
}