using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.Verb.GenerateKey.Persister
{
    interface ISymmetricKeyPersister<in TKeyPoco, TKeyXmlPoco> : IKeyPersister<TKeyPoco, TKeyXmlPoco, SymmetricKeyPersisterOptions>
        where TKeyPoco : KeyBase, ISymmetricKeyMarker
        where TKeyXmlPoco : IKeyExternalRepresentation<TKeyPoco>
    {}
}