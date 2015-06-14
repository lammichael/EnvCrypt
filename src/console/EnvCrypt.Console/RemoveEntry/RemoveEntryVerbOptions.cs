using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using CommandLine;

namespace EnvCrypt.Console.RemoveEntry
{
    [Verb("RemoveEntry", HelpText = "Removes one or more entries from a DAT file.")]
    class RemoveEntryVerbOptions //: VerbOptionsBase
    {
        public const char Delimiter = '|';

        [Option('d', "Dat", HelpText = "Full path to ECDat file containing the encrypted entries use for encryption. If ECDat does not already exist then it will be created.", Required = true)]
        public string DatFile { get; set; }
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
