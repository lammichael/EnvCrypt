using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.Verb.GenerateKey.Persister
{
    abstract class AsymmetricKeyFilePersister<TKeyPoco, TKeyXmlPoco> : IAsymmetricKeyFilePersister<TKeyPoco, TKeyXmlPoco> 
        where TKeyPoco : KeyBase, IAsymmetricKeyMarker
        where TKeyXmlPoco : IKeyExternalRepresentation<TKeyPoco>
    {
        public abstract void WriteToFile(TKeyPoco thisKey, AsymmetricKeyFilePersisterOptions withOptions);

        [Pure]
        protected abstract TKeyPoco GetPublicKey(TKeyPoco fromPrivateKey);
    }
}
