using System.Collections.Generic;

namespace EnvCrypt.Core.EncrypedData.Poco
{
    /// <summary>
    /// A category is used to group together related entries.
    /// E.G., Production, UAT, and Development database connections can each have its own category.
    /// </summary>
    public class Category
    {
        public string Name { get; set; }
        public IList<Entry> Entries { get; set; } 
    }
}
