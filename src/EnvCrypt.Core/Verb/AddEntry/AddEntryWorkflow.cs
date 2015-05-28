using EnvCrypt.Core.EncrypedData;
using EnvCrypt.Core.EncrypedData.Mapper;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Verb.LoadKey;

namespace EnvCrypt.Core.Verb.AddEntry
{
    class AddEntryWorkflow<TExtRep,TKey>
        where TExtRep : class, IDataExternalRepresentation
        where TKey : KeyBase
    {
        private IKeyLoader<TKey> _keyLoader;
        private IExternalRepresentationToDatMapper<TExtRep> _xmlToPoco;

        private IDatToExternalRepresentationMapper<TExtRep> _pocoToXml;


        public void Run()
        {
            /*
             * Get Entries from file or create a new file if not already there
             * Get key from file
             * Map keys to entries
             * Encrypt value using key (or leave in plain text)
             * Add encrypted value to POCO
             * Write POCO to file
             */
        }
    }
}
