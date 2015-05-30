using System.Collections.Generic;

namespace EnvCrypt.Core.EncrypedData.Poco
{
    /// <summary>
    /// Format agnostic POCO for the Envcrypt dat file containing the
    /// encrypted entries.
    /// </summary>
    public class EnvCryptDat
    {
        public IList<Category> Categories { get; set; }
    }
}
