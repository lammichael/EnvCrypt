namespace EnvCrypt.Core.Verb.AddEntry.PlainText
{
    public class AddPlainTextEntryWorkflowOptions
    {
        public string DatFilePath { get; set; }
        public string CategoryName { get; set; }
        public string EntryName { get; set; }
        public string StringToEncrypt { get; set; }
    }
}
