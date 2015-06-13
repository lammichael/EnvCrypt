using EnvCrypt.Core.Verb.DecryptEntry.PlainText;

namespace EnvCrypt.Core.Verb.DecryptEntry.Generic
{
    public class DecryptGenericWorkflowOptions : DecryptPlainTextEntryWorkflowOptions
    {
        public string KeyFilePath { get; set; }
        public bool ThrowExceptionIfEntryNotFound { get; set; }
    }
}
