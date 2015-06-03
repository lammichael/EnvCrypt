using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.Verb.GenerateKey.Persister
{
    interface IAsymmetricKeyPersister<in TKeyPoco, TKeyXmlPoco> : IKeyPersister<TKeyPoco, TKeyXmlPoco, AsymmetricKeyToFilePersisterOptions> 
        where TKeyPoco : KeyBase, IAsymmetricKeyMarker
        where TKeyXmlPoco : IKeyExternalRepresentation<TKeyPoco>
    {}
}