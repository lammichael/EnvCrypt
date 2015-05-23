using System;
using CommandLine;
using EnvCrypt.Core.EncryptionAlgo;

namespace EnvCrypt.Console.GenerateKey
{
    [Verb("generatekey", HelpText = "Generates a new key for encryption & decryption.")]
    class GenerateKeyVerbOptions //: VerbOptionsBase
    {
        /// <summary>
        /// String is used here instead of EnvCryptAlgorithmEnum enum because 
        /// CommandLineParser throws an ArgumentException if the user text 
        /// does not exactly match any of the enum's values.
        /// </summary>
        [Option('a', "Algorithm", HelpText = "Algorithm to use - RSA or AES.", Required = true)]
        public string AlgorithmToUse { get; set; }

        [Option('n', "Name", HelpText = "The new key's name.", Required = true)]
        public string KeyName { get; set; }

        [Option('d', "Directory", HelpText = "Directory to output the key file(s).", Required = true)]
        public string OutputDirectory { get; set; }

        [Option('c', "WriteToConsole", HelpText = "Write resulting key XML to console.", Required = false, DefaultValue = false)]
        public bool OutputKeyToConsole { get; set; }

        [Option('v', "Verbose", HelpText = "Verbosity of logging output.", Required = false, DefaultValue = false)]
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
