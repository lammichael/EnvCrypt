using System;
using System.Diagnostics.Contracts;

namespace EnvCrypt.Core.Key.Mapper
{
    [ContractClass(typeof(ExternalRepresentationToKeyMapperContracts<,>))]
    interface IExternalRepresentationToKeyMapper<in TExtRep, out TKey>
        where TKey : KeyBase
        where TExtRep : class, IKeyExternalRepresentation<TKey>
    {
        TKey Map(TExtRep fromExternalRepresentationPoco);
    }


    [ContractClassFor(typeof(IExternalRepresentationToKeyMapper<,>))]
    internal abstract class ExternalRepresentationToKeyMapperContracts<TExtRep, TKey> : IExternalRepresentationToKeyMapper<TExtRep, TKey>
        where TExtRep : class, IKeyExternalRepresentation<TKey>
        where TKey : KeyBase
    {
        public TKey Map(TExtRep fromExternalRepresentationPoco)
        {
            Contract.Requires<ArgumentNullException>(fromExternalRepresentationPoco != null, "fromExternalRepresentationPoco");
            Contract.Ensures(Contract.Result<TKey>() != null);
            Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<TKey>().Name));

            return default(TKey);
        }
    }
}