using System.Collections.Generic;

namespace EnvCrypt.Core.EncrypedData.Poco
{
    public class Category
    {
        public string Name { get; set; }
        public IList<Entry> Entries { get; set; } 
    }
}
