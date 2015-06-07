using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using CommandLine;
using EnvCrypt.Core.EncryptionAlgo;

namespace EnvCrypt.Console.DecryptEntry
{
    [Verb("DecryptEntry", HelpText = "Decrypts entries using the keys specified. The appropriate key (if specified) will be found to decrypt each entry.")]
    class DecryptEntryVerbOptions //: VerbOptionsBase
    {
        public const char Delimiter = '|';

        [Option('a', "Algorithm", HelpText = "Algorithm to use - RSA or AES.", Required = true)]
        public string AlgorithmToUse { get; set; }

        [Option('d', "Dat", HelpText = "Full path to ECDat file containing the encrypted entries use for encryption. If ECDat does not already exist then it will be created.", Required = true)]
        public string DatFile { get; set; }

        [Option('k', "Keys", HelpText = "Full path to ECKey files to use for encryption. If excluded then key will be added in plaintext. Accepts multiple key files pipe (|) separated.", Required = false)]
        public string KeyFiles { get; set; }

        [Option('c', "Categories", HelpText = "Category to add entry under. Accepts pipe (|) separated names corresponding to the specified entries.", Required = true)]
        public string Categories { get; set; }

        [Option('e', "Entries", HelpText = "The new entry's name. Accepts pipe (|) separated names corresponding to the strings for each entry.", Required = true)]
        public string Entries { get; set; }


        public EnvCryptAlgoEnum? GetAlgorithm()
        {
            EnvCryptAlgoEnum ret;
            if (Enum.TryParse(AlgorithmToUse, true, out ret))
            {
                return ret;
            }
            return null;
        }


        [Pure]
        public IList<string> GetKeyFiles()
        {
            if (KeyFiles == null)
            {
                return new List<string>();
            }
            return KeyFiles.Split(Delimiter);
        }


        [Pure]
        public IList<string> GetCategories()
        {
            Contract.Requires<NullReferenceException>(Categories != null, "Categories");
            //
            return Categories.Split(Delimiter);
        }


        [Pure]
        public IList<string> GetEntries()
        {
            Contract.Requires<NullReferenceException>(Entries != null, "Entries");
            //
            return Entries.Split(Delimiter);
        }
    }
}
