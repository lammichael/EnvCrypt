using System.Collections.Generic;

namespace EnvCrypt.Core.Verb.GetEntry
{
    public class DecryptEntryWorkflowOptions
    {
        public string DatFilePath { get; set; }
        public IList<string> KeyFilePaths { get; set; }
        public IList<EntryDetails> CategoryEntryDetails { get; set; }
    }
}
