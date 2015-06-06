using System.Collections.Generic;

namespace EnvCrypt.Core.Verb.DecryptEntry.PlainText
{
    public class DecryptPlainTextEntryWorkflowOptions
    {
        public string DatFilePath { get; set; }
        public IList<CategoryEntryPair> CategoryEntryPair { get; set; }
    }
}
