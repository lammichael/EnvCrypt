using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncrypedData.Poco;
using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.EncrypedData
{
    static class DatExtMethods
    {
        public static bool SearchForCategory(this EnvCryptDat inDatPoco,
            string withCategoryName, out Category foundCategory)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(withCategoryName), "withCategoryName");
            //
            for (uint catI = 0; catI < inDatPoco.Categories.Count; catI++)
            {
                var currentCategory = inDatPoco.Categories[(int)catI];
                if (currentCategory.Name == withCategoryName)
                {
                    foundCategory = currentCategory;
                    return true;
                }
            }

            foundCategory = default(Category);
            return false;
        }


        public static bool SearchForEntry(this Category inCategory,
            string withEntryName, out Entry foundEntry)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(withEntryName), "withEntryName");
            //
            for (uint entryI = 0; entryI < inCategory.Entries.Count; entryI++)
            {
                var currentEntry = inCategory.Entries[(int)entryI];
                if (currentEntry.Name == withEntryName)
                {
                    foundEntry = currentEntry;
                    return true;
                }
            }

            foundEntry = default(Entry);
            return false;
        }


        public static void AddEntry(this EnvCryptDat toDatPoco,
            string categoryName, string entryName,
            KeyBase key, IList<byte[]> segments, bool overwriteIfEntryExists = false)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(categoryName), "categoryName");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(entryName), "entryName");
            //
            var isNewEntry = true;
            var entryToAdd = new Entry();

            var isNewCategory = true;
            var categoryToAddTo = new Category()
            {
                Name = categoryName,
                Entries = new List<Entry>()
            };
            

            // Search for entries with the same name in the desired category
            {
                Category foundCategory;
                if (toDatPoco.SearchForCategory(categoryName, out foundCategory))
                {
                    isNewCategory = false;
                    categoryToAddTo = foundCategory;
                }
            }

            if (!isNewCategory)
            {
                Entry foundEntry;
                if (categoryToAddTo.SearchForEntry(entryName, out foundEntry))
                {
                    if (overwriteIfEntryExists)
                    {
                        isNewEntry = false;
                        entryToAdd = foundEntry;
                        toDatPoco.RemoveEntry(categoryName, entryName);
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
                Contract.Assert(isNewEntry);
            }
            
            entryToAdd.Name = entryName;
            entryToAdd.KeyName = key.Name;
            entryToAdd.KeyHash = key.GetHashCode();
            entryToAdd.EncryptionAlgorithm = key.Algorithm;
            entryToAdd.EncryptedValue = segments;

            categoryToAddTo.Entries.Add(entryToAdd);
        }


        public static bool RemoveEntry(this EnvCryptDat fromDatPoco,
            string inCategory, string withEntryName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(inCategory), "inCategory");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(withEntryName), "withEntryName");
            Contract.Requires<EnvCryptException>(fromDatPoco != null, "fromDatPoco");
            Contract.Requires<EnvCryptException>(fromDatPoco.Categories != null, "category list in DAT POCO cannnot be null");
            Contract.Ensures(fromDatPoco.Categories.Count == Contract.OldValue(fromDatPoco.Categories.Count), "Category count changed");
            //
            Category foundCategory;
            if (fromDatPoco.SearchForCategory(inCategory, out foundCategory))
            {
                Entry foundEntry;
                if (foundCategory.SearchForEntry(withEntryName, out foundEntry))
                {
                    return foundCategory.Entries.Remove(foundEntry);
                }
            }
            return false;
        }
    }
}
