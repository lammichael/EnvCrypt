using System;
using System.Diagnostics.Contracts;

namespace EnvCrypt.Core.Key
{
    [ContractClass(typeof(KeyToExternalRepresentationMapperContract<,>))]
    interface IKeyToExternalRepresentationMapper<in TKey, in TExtRep>
        where TKey : KeyBase
        where TExtRep : class, IKeyExternalRepresentation<TKey>
    {
        void Map(TKey fromPoco, TExtRep toExternalRepresentationPoco);
    }


    [ContractClassFor(typeof(IKeyToExternalRepresentationMapper<,>))]
    internal abstract class KeyToExternalRepresentationMapperContract<TKey, TExtRep> : IKeyToExternalRepresentationMapper<TKey, TExtRep>
        where TKey : KeyBase
        where TExtRep : class, IKeyExternalRepresentation<TKey> 
    {
        public void Map(TKey fromPoco, TExtRep toExternalRepresentationPoco)
        {
            Contract.Requires<ArgumentNullException>(fromPoco != null, "fromPoco");
            Contract.Requires<ArgumentNullException>(toExternalRepresentationPoco != null, "toExternalRepresentationPoco");
        }
    }
}
