using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.Verb.GenerateKey.Persister.Asymetric
{
    interface IAsymmetricKeyFilePersister<in TKey, TKeyXmlPoco> : IKeyPersister<TKey, AsymmetricKeyFilePersisterOptions> 
        where TKey : KeyBase, IAsymmetricKeyMarker
        where TKeyXmlPoco : IKeyExternalRepresentation<TKey>, new()
    {}
}