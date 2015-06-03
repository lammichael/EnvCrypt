using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.Verb.GenerateKey.Persister
{
    [ContractClass(typeof(AsymmetricKeyPersisterContracts<,>))]
    public abstract class AsymmetricKeyPersister<TKeyPoco, TKeyXmlPoco> : IAsymmetricKeyPersister<TKeyPoco, TKeyXmlPoco> 
        where TKeyPoco : KeyBase, IAsymmetricKeyMarker
        where TKeyXmlPoco : IKeyExternalRepresentation<TKeyPoco>
    {
        public abstract void Persist(TKeyPoco thisKey, AsymmetricKeyToFilePersisterOptions withOptions);

        [Pure]
        protected abstract TKeyPoco GetPublicKey(TKeyPoco fromPrivateKey);
    }


    [ContractClassFor(typeof(AsymmetricKeyPersister<,>))]
    internal abstract class AsymmetricKeyPersisterContracts<TKeyPoco, TKeyXmlPoco> : AsymmetricKeyPersister<TKeyPoco, TKeyXmlPoco> where TKeyPoco : KeyBase, IAsymmetricKeyMarker where TKeyXmlPoco : IKeyExternalRepresentation<TKeyPoco>
    {
        protected override TKeyPoco GetPublicKey(TKeyPoco fromPrivateKey)
        {
            Contract.Ensures(Contract.Result<TKeyPoco>() != null);
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<TKeyPoco>().Name));

            return default(TKeyPoco);
        }
    }
}
