using EnvCrypt.Core.EncrypedData;
using EnvCrypt.Core.EncrypedData.Mapper;

namespace EnvCrypt.Core.Verb.AddEntry
{
    class AddEntryWorkflow<TExtRep> where TExtRep : class, IDataExternalRepresentation
    {
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
