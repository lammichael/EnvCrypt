using System;
using System.Diagnostics.Contracts;

namespace EnvCrypt.Core.Key
{
    [ContractClass(typeof(KeyHasherContracts<>))]
    public interface IKeyHasher<in T> where T : KeyBase
    {
        byte[] GetHash(T forKey);
    }


    [ContractClassFor(typeof(IKeyHasher<>))]
    internal abstract class KeyHasherContracts<T> : IKeyHasher<T> where T : KeyBase
    {
        public byte[] GetHash(T forKey)
        {
            Contract.Requires<ArgumentNullException>(forKey != null, "forKey");
            Contract.Ensures(Contract.Result<byte[]>() != null);
            Contract.Ensures(Contract.Result<byte[]>().Length > 0);

            return default(byte[]);
        }
    }
}
