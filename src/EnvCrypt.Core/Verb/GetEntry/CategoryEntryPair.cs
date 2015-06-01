namespace EnvCrypt.Core.Verb.GetEntry
{
    public struct CategoryEntryPair
    {
        public string Category { get; set; }
        public string Entry { get; set; }

        public CategoryEntryPair(string category, string entry) : this()
        {
            Category = category;
            Entry = entry;
        }
    }
}
