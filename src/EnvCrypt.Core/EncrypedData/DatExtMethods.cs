using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using EnvCrypt.Core.EncrypedData.Poco;
using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.EncrypedData
{
    static class DatExtMethods
    {
        #region private methods
        [Pure]
        private static bool SearchForCategory(this EnvCryptDat inDatPoco,
            string withCategoryName, out Category foundCategory)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(withCategoryName), "withCategoryName");
            Contract.Ensures(Contract.Result<bool>() ?
                Contract.ValueAtReturn(out foundCategory) != null :
                Contract.ValueAtReturn(out foundCategory) == default(Category));
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


        [Pure]
        private static bool SearchForEntry(this Category inCategory,
            string withEntryName, out Entry foundEntry)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(withEntryName), "withEntryName");
            Contract.Ensures(Contract.Result<bool>() ? 
                Contract.ValueAtReturn(out foundEntry) != null : 
                Contract.ValueAtReturn(out foundEntry) == default(Entry));
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
        #endregion


        [Pure]
        public static bool SearchForEntry(this EnvCryptDat inDatPoco,
            string withCategoryName, string withEntryName, out Entry foundEntry)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(withCategoryName), "withCategoryName");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(withEntryName), "withEntryName");
            Contract.Ensures(Contract.Result<bool>() ?
                Contract.ValueAtReturn(out foundEntry) != null :
                Contract.ValueAtReturn(out foundEntry) == default(Entry));
            //
            Category foundCategory;
            if (inDatPoco.SearchForCategory(withCategoryName, out foundCategory))
            {
                Entry entry;
                if (foundCategory.SearchForEntry(withEntryName, out entry))
                {
                    foundEntry = entry;
                    return true;
                }
            }
            foundEntry = null;
            return false;
        }


        public static void AddEntry(this EnvCryptDat toDatPoco,
            string categoryName, string entryName,
            KeyBase key, IList<byte[]> segments, bool overwriteIfEntryExists = false)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(categoryName), "categoryName");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(entryName), "entryName");
            Contract.Requires<ArgumentNullException>(key != null, "key");
            Contract.Requires<ArgumentNullException>(segments != null, "segments");
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
            
            entryToAdd.Name = entryName;
            entryToAdd.KeyName = key.Name;
            entryToAdd.KeyHash = key.GetHashCode();
            entryToAdd.EncryptionAlgorithm = key.Algorithm;
            entryToAdd.EncryptedValue = segments;

            categoryToAddTo.Entries.Add(entryToAdd);

            if (isNewCategory || !isNewEntry)
            {
                toDatPoco.Categories.Add(categoryToAddTo);
            }
        }


        public static bool RemoveEntry(this EnvCryptDat fromDatPoco,
            string inCategory, string withEntryName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(inCategory), "inCategory");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(withEntryName), "withEntryName");
            Contract.Requires<EnvCryptException>(fromDatPoco != null, "fromDatPoco");
            Contract.Requires<EnvCryptException>(fromDatPoco.Categories != null, "category list in DAT POCO cannnot be null");
            Contract.Ensures(fromDatPoco.Categories.Count == Contract.OldValue(fromDatPoco.Categories.Count) || 
                fromDatPoco.Categories.Count == Contract.OldValue(fromDatPoco.Categories.Count) - 1);
            //
            Category foundCategory;
            if (fromDatPoco.SearchForCategory(inCategory, out foundCategory))
            {
                Entry foundEntry;
                if (foundCategory.SearchForEntry(withEntryName, out foundEntry))
                {
                    foundCategory.Entries.Remove(foundEntry);

                    if (!foundCategory.Entries.Any())
                    {
                        fromDatPoco.Categories.Remove(foundCategory);
                    }

                    return true;
                }
            }
            return false;
        }
    }
}
