using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.Verb.GenerateKey.Persister
{
    [ContractClass(typeof(AsymmetricKeyFilePersisterContracts<,>))]
    abstract class AsymmetricKeyFilePersister<TKeyPoco, TKeyXmlPoco> : IAsymmetricKeyFilePersister<TKeyPoco, TKeyXmlPoco> 
        where TKeyPoco : KeyBase, IAsymmetricKeyMarker
        where TKeyXmlPoco : IKeyExternalRepresentation<TKeyPoco>
    {
        public abstract void WriteToFile(TKeyPoco thisKey, AsymmetricKeyFilePersisterOptions withOptions);

        [Pure]
        protected abstract TKeyPoco GetPublicKey(TKeyPoco fromPrivateKey);
    }


    [ContractClassFor(typeof(AsymmetricKeyFilePersister<,>))]
    internal abstract class AsymmetricKeyFilePersisterContracts<TKeyPoco, TKeyXmlPoco> : AsymmetricKeyFilePersister<TKeyPoco, TKeyXmlPoco> where TKeyPoco : KeyBase, IAsymmetricKeyMarker where TKeyXmlPoco : IKeyExternalRepresentation<TKeyPoco>
    {
        protected override TKeyPoco GetPublicKey(TKeyPoco fromPrivateKey)
        {
            Contract.Ensures(Contract.Result<TKeyPoco>() != null);
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<TKeyPoco>().Name));

            return default(TKeyPoco);
        }
    }
}
