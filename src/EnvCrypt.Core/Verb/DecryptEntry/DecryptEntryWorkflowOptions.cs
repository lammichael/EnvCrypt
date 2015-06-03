using System.Collections.Generic;

namespace EnvCrypt.Core.Verb.DecryptEntry
{
    public class DecryptEntryWorkflowOptions
    {
        public string DatFilePath { get; set; }
        public IList<string> KeyFilePaths { get; set; }
        public IList<CategoryEntryPair> CategoryEntryPair { get; set; }
    }
}
