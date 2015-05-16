using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.EncryptionAlgo.Poco;

namespace EnvCrypt.Core.Verb
{
    class GenerateKeyWorkflow<TKey, TKeyGenOptions> 
        where TKey : KeyBase
        where TKeyGenOptions : class, IKeyGenerationOptions<TKey>
    {
        private IKeyGenerator<TKey, TKeyGenOptions> _encryptionAlgo;
        private IKeyGenerationOptions<TKey> _generationOptions; 
        



        public void Run()
        {
            /*
             * Generate Key
             * Insert into Data structure
             * Map data structure to XML/JSON/?
             * Output publlic & private key to file
             */

        }
    }
}
