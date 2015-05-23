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
        private readonly IKeyGenerator<TKey, TKeyGenOptions> _encryptionAlgo;
        private readonly TKeyGenOptions _generationOptions;
        private readonly IKeyFilePersister<TKey, TKeyExtRep, TPersisterOptions> _filePersister;
        private readonly TPersisterOptions _filePersisterOptions;

        public GenerateKeyWorkflow(IKeyGenerator<TKey, TKeyGenOptions> encryptionAlgo, TKeyGenOptions generationOptions, IKeyFilePersister<TKey, TKeyExtRep, TPersisterOptions> filePersister, TPersisterOptions filePersisterOptions)
        {
            _encryptionAlgo = encryptionAlgo;
            _generationOptions = generationOptions;
            _filePersister = filePersister;
            _filePersisterOptions = filePersisterOptions;
        }


        /// <summary>
        /// <list type="number">
        ///     <item>Generates new Key (symmetric or asymmetric)</item>
        ///     <item>Inserts into the POCO</item>
        ///     <item>Maps data structure to XML/JSON/...</item>
        ///     <item>Writes the public & private key to its respective file</item>
        /// </list>
        /// </summary>
        public void Run()
        {
            var newKey = _encryptionAlgo.GetNewKey(_generationOptions);
            _filePersister.WriteToFile(newKey, _filePersisterOptions);
        }
    }
}
