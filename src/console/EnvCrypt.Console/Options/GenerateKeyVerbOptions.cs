using System;
using CommandLine;
using EnvCrypt.Core;
using EnvCrypt.Core.EncryptionAlgo;

namespace EnvCrypt.Console.Options
{
    [Verb("generatekey", HelpText = "Generates a new key for encryption & decryption.")]
    class GenerateKeyVerbOptions //: VerbOptionsBase
    {
        /// <summary>
        /// String is used here instead of EnvCryptAlgorithmEnum enum because 
        /// CommandLineParser throws an ArgumentException if the user text 
        /// does not exactly match any of the enum's values.
        /// </summary>
        [Option('a', "Algorithm", HelpText = "Algorithm to use - Rsa or Aes.", Required = true)]
        public string AlgorithmToUse { get; set; }

        [Option('n', "Name", HelpText = "The new key's name.", Required = true)]
        public string KeyName { get; set; }

        [Option('d', "OutDirectory", HelpText = "Directory to output the key file(s).", Required = true)]
        public string OutputDirectory { get; set; }

        [Option('v', "Verbose", HelpText = "Verbosity of logging output.", Required = false)]
        public bool Verbose { get; set; }


        public EnvCryptAlgoEnum? GetAlgorithm()
        {
            EnvCryptAlgoEnum ret;
            if (Enum.TryParse(AlgorithmToUse, true, out ret))
            {
                return ret;
            }
            return null;
        }
    }
}
