using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Verb.GenerateKey.Persister;

namespace EnvCrypt.Core.Verb.GenerateKey
{
    class GenerateKeyWorkflow<TKey, TKeyGenOptions, TKeyExtRep, TPersisterOptions> 
        where TKey : KeyBase
        where TKeyGenOptions : KeyGenerationOptions<TKey>
        where TKeyExtRep : IKeyExternalRepresentation<TKey>
        where TPersisterOptions : KeyFilePersisterOptions
    {
        private IKeyGenerator<TKey, TKeyGenOptions> _encryptionAlgo;
        private TKeyGenOptions _generationOptions;
        private IKeyFilePersister<TKey, TKeyExtRep, TPersisterOptions> _filePersister;
        private TPersisterOptions _filePersisterOptions;

        
        public void Run()
        {
            /*
             * Generate new Key (symmsetric or asymmetric)
             * Insert into Data structure
             * Map data structure to XML/JSON/?
             * Output public & private key to file
             */
            var newKey = _encryptionAlgo.GetNewKey(_generationOptions);
            _filePersister.WriteToFile(newKey, _filePersisterOptions);
        }
    }
}
