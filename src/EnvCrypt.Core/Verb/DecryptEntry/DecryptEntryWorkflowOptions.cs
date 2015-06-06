using System.Collections.Generic;
using EnvCrypt.Core.Verb.DecryptEntry.PlainText;

namespace EnvCrypt.Core.Verb.DecryptEntry
{
    public class DecryptEntryWorkflowOptions : DecryptPlainTextEntryWorkflowOptions
    {
        public IList<string> KeyFilePaths { get; set; }
    }
}
