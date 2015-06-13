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

        [Option('d', "Dat", HelpText = "Full path to ECDat file containing the encrypted entries use for encryption. If ECDat does not already exist then it will be created.", Required = true)]
        public string DatFile { get; set; }

        [Option('k', "Key", HelpText = "Full path to ECKey file to use for encryption.", Required = false)]
        public string KeyFile { get; set; }

        [Option('c', "Categories", HelpText = "Category to add entry under. Accepts pipe (|) separated names corresponding to the specified entries.", Required = true)]
        public string Categories { get; set; }

        [Option('e', "Entries", HelpText = "The new entry's name. Accepts pipe (|) separated names corresponding to the strings for each entry.", Required = true)]
        public string Entries { get; set; }


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
