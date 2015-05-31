namespace EnvCrypt.Core.Verb.GetEntry
{
    public struct EntryDetails
    {
        public string Category { get; set; }
        public string Entry { get; set; }

        public EntryDetails(string category, string entry) : this()
        {
            Category = category;
            Entry = entry;
        }
    }
}
