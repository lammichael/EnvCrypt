using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.EncryptionAlgo
{
    /// <summary>
    /// To generate new keys
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TO"></typeparam>
    [ContractClass(typeof(KeyGeneratorContracts<,>))]
    public interface IKeyGenerator<out T, in TO>
        where T : KeyBase
        where TO : class, IKeyGenerationOptions
    {
        T GetNewKey(TO options);
    }


    [ContractClassFor(typeof(IKeyGenerator<,>))]
    internal abstract class KeyGeneratorContracts<T, TO> : IKeyGenerator<T, TO> where T : KeyBase
        where TO : class, IKeyGenerationOptions
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