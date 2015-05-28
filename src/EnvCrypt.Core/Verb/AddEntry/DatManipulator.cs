using System.Collections.Generic;
using EnvCrypt.Core.EncrypedData.Poco;
using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.Verb.AddEntry
{
    class DatManipulator
    {
        public void AddEntry(EnvCryptDat toDatPoco, string categoryName, string entryName, KeyBase key,
            IList<byte[]> segments, bool overwriteIfEntryExists = false)
        {
            var isNewCategory = true;
            var categoryToAddTo = new Category()
            {
                Name = categoryName,
                Entries = new List<Entry>()
            };
            var isNewEntry = true;
            var entryToAdd = new Entry();

            // Search for entries with the same name in the desired category
            for (uint catI = 0; catI < toDatPoco.Categories.Count; catI++)
            {
                var currentCategory = toDatPoco.Categories[(int) catI];
                if (currentCategory.Name == categoryName)
                {
                    isNewCategory = false;
                    categoryToAddTo = currentCategory;
                    break;
                }
            }

            for (uint entryI = 0; entryI < categoryToAddTo.Entries.Count; entryI++)
            {
                var currentEntry = categoryToAddTo.Entries[(int) entryI];
                if (currentEntry.Name == entryName)
                {
                    if (overwriteIfEntryExists)
                    {
                        isNewEntry = false;
                        entryToAdd = currentEntry;
                        break;
                    }
                    else
                    {
                        throw new EnvCryptException(
                            "the entry '{0}' already exists in the category '{1}' and the option to overwrite was not chosen",
                            entryName, categoryName);
                    }
                }
            }

            if (isNewCategory)
            {
                toDatPoco.Categories.Add(categoryToAddTo);
            }
            if (isNewEntry)
            {
                categoryToAddTo.Entries.Add(entryToAdd);
            }

            entryToAdd.Name = entryName;
            entryToAdd.KeyName = key.Name;
            entryToAdd.KeyHash = key.GetHashCode();
            entryToAdd.EncryptionAlgorithm = key.Algorithm;
            entryToAdd.EncryptedValue = segments;
        }
    }
}
