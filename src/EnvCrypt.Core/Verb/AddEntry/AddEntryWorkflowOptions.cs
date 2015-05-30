namespace EnvCrypt.Core.Verb.AddEntry
{
    public class AddEntryWorkflowOptions
    {
        public string KeyFilePath { get; set; }
        public string DatFilePath { get; set; }
        public string CategoryName { get; set; }
        public string EntryName { get; set; }
        public string StringToEncrypt { get; set; }
    }
}
