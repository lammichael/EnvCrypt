using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using EnvCrypt.Core.EncrypedData.Poco;
using EnvCrypt.Core.EncrypedData.XmlPoco;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Utils;

namespace EnvCrypt.Core.EncrypedData.Mapper.Xml.ToXmlPoco
{
    class DatToXmlMapper : IDatToExternalRepresentationMapper<EnvCryptEncryptedData>
    {
        private readonly IEncryptedDetailsPersistConverter _strConverter;

        public DatToXmlMapper(IEncryptedDetailsPersistConverter strConverter)
        {
            Contract.Requires<ArgumentNullException>(strConverter != null, "strConverter");
            _strConverter = strConverter;
        }


        public EnvCryptEncryptedData Map(EnvCryptDat fromDatPoco)
        {
            var xmlCategories = new List<EnvCryptEncryptedDataCategory>(fromDatPoco.Categories.Count);

            for (uint catI = 0; catI < fromDatPoco.Categories.Count; catI++)
            {
                // For each caegory
                var xmlCategoryToAdd = new EnvCryptEncryptedDataCategory();
                var currentCategory = fromDatPoco.Categories[(int)catI];
                if (currentCategory == null || !currentCategory.Entries.Any())
                {
                    // Don't add it to the XML if there is nothing in the category
                    continue;
                }
                xmlCategoryToAdd.Name = currentCategory.Name;

                var xmlCategoryEntries = new List<EnvCryptEncryptedDataCategoryEntry>(currentCategory.Entries.Count);
                // For each entry in the category
                for (uint entryI = 0; entryI < currentCategory.Entries.Count; entryI++)
                {
                    var xmlEntryToAdd = new EnvCryptEncryptedDataCategoryEntry();
                    xmlCategoryEntries.Add(xmlEntryToAdd);
                    var currentEntry = currentCategory.Entries[(int) entryI];

                    /*
                     * If there are no encrypted values or all segments are empty,
                     * then don't add the entry at all.
                     */
                    if (currentEntry.EncryptedValue == null ||
                        !currentEntry.EncryptedValue.Any() ||
                        currentEntry.EncryptedValue.Count(b => b.Any()) == 0)
                    {
                        continue;
                    }

                    xmlEntryToAdd.Name = currentEntry.Name;

                    // Add encrypted values
                    {
                        var xmlEncryptedValues = new List<EnvCryptEncryptedDataCategoryEntryEncryptedValue>();
                        for (uint valueI = 0; valueI < currentEntry.EncryptedValue.Count; valueI++)
                        {
                            var currentValueAsByteArr = currentEntry.EncryptedValue[(int) valueI];
                            if (currentValueAsByteArr == null || !currentValueAsByteArr.Any())
                            {
                                continue;
                            }

                            /*
                             * Note: There is no check here for 'empty' array items 
                             * because the default byte value of 0 could be valid.
                             */

                            xmlEncryptedValues.Add(new EnvCryptEncryptedDataCategoryEntryEncryptedValue()
                            {
                                Value = _strConverter.Encode(currentValueAsByteArr, currentEntry.EncryptionAlgorithm)
                            });
                        }
                        xmlEntryToAdd.EncryptedValue = xmlEncryptedValues.ToArray();
                    }

                    // Don't add Decryption element at all if plaintext, but still add values (above)
                    if (currentEntry.EncryptionAlgorithm == EnvCryptAlgoEnum.PlainText)
                    {
                        xmlEntryToAdd.Decryption = null;
                        continue;
                    }

                    // Only add entry to XML if one exists
                    if (currentEntry.KeyName != null)
                    {
                        xmlEntryToAdd.Decryption = new EnvCryptEncryptedDataCategoryEntryDecryption()
                        {
                            KeyName = currentEntry.KeyName,
                            Algo = currentEntry.EncryptionAlgorithm.ToString(),
                            Hash = currentEntry.KeyHash,
                        };
                    }
                }

                // Add our prepared Entries
                xmlCategoryToAdd.Entry = xmlCategoryEntries.ToArray();

                xmlCategories.Add(xmlCategoryToAdd);
            }

            // Add out prepared Categories
            return new EnvCryptEncryptedData()
            {
                Items = xmlCategories.ToArray()
            };
        }
    }
}
