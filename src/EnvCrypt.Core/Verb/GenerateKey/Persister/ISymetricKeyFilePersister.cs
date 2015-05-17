using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.Verb.GenerateKey.Persister
{
    interface ISymetricKeyFilePersister<in TKeyPoco, TKeyXmlPoco> : IKeyFilePersister<TKeyPoco, TKeyXmlPoco, SymmetricKeyFilePersisterOptions>
        where TKeyPoco : KeyBase, ISymmetricKeyMarker
        where TKeyXmlPoco : IKeyExternalRepresentation<TKeyPoco>
    {}
}