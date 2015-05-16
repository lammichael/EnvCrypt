using EnvCrypt.Core.EncryptionAlgo.Poco;

namespace EnvCrypt.Core.Key
{
    interface IKeyToExternalRepresentationMapper<in TKey, in TExtRep>
        where TKey : KeyBase
        where TExtRep : IKeyExternalRepresentation<TKey>
    {
        void Map(TKey fromPoco, TExtRep toExternalRepresentationPoco);
    }
}
