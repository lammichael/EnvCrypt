using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Verb.GenerateKey.Persister;

namespace EnvCrypt.Core.Verb.GenerateKey
{
    class GenerateKeyWorkflow<TKey, TKeyGenOptions, TKeyExtRep, TPersisterOptions> 
        where TKey : KeyBase
        where TKeyGenOptions : KeyGenerationOptions<TKey>
        where TKeyExtRep : IKeyExternalRepresentation<TKey>
        where TPersisterOptions : KeyPersisterOptions
    {
        private readonly IKeyGenerator<TKey, TKeyGenOptions> _encryptionAlgo;
        private readonly IKeyPersister<TKey, TPersisterOptions> _persister;

        public GenerateKeyWorkflow(IKeyGenerator<TKey, TKeyGenOptions> encryptionAlgo, IKeyPersister<TKey, TPersisterOptions> persister)
        {
            Contract.Requires<ArgumentNullException>(encryptionAlgo != null, "encryptionAlgo");
            Contract.Requires<ArgumentNullException>(persister != null, "persister");
            //
            _encryptionAlgo = encryptionAlgo;
            _persister = persister;
        }


        /// <summary>
        /// <list type="number">
        ///     <item>Generates new Key (symmetric or asymmetric)</item>
        ///     <item>Inserts into the POCO</item>
        ///     <item>Maps data structure to XML/JSON/...</item>
        ///     <item>Writes the public & private key to its respective file</item>
        /// </list>
        /// </summary>
        public void Run(TKeyGenOptions generationOptions, TPersisterOptions filePersisterOptions)
        {
            var newKey = _encryptionAlgo.GetNewKey(generationOptions);
            _persister.Persist(newKey, filePersisterOptions);
        }
    }
}
