using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.Verb.GenerateKey.Persister.Symmetric
{
    interface ISymmetricKeyFilePersister<in TKeyPoco> : IKeyPersister<TKeyPoco, SymmetricKeyFilePersisterOptions>
        where TKeyPoco : KeyBase, ISymmetricKeyMarker
    {}
}