using System;
using CommandLine;
using EnvCrypt.Core.EncryptionAlgo;

namespace EnvCrypt.Console.DecryptEntry
{
    [Verb("AddEntry", HelpText = "Generates a new key for encryption & decryption.")]
    class DecryptEntryVerbOptions //: VerbOptionsBase
    {
        [Option('k', "Key", HelpText = "Full path to ECKey file to use for encryption. If excluded then key will be added in plaintext.", Required = false)]
        public string KeyFile { get; set; }

        [Option('a', "Algorithm", HelpText = "Algorithm to use - RSA or AES.", Required = true)]
        public string AlgorithmToUse { get; set; }

        [Option('c', "Category", HelpText = "Category to add entry under.", Required = true)]
        public string Category { get; set; }

        [Option('n', "Name", HelpText = "The new entry's name. Accepts comma separated names corresponding to the strings for each entry.", Required = true)]
        public string EntryName { get; set; }

        [Option('d', "Dat", HelpText = "Full path to ECDat file containing the encrypted entries use for encryption. If ECDat does not already exist then it will be created.", Required = true)]
        public string DatFile { get; set; }


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
