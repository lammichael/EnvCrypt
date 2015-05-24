using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using EnvCrypt.Core.EncrypedData.Poco;
using EnvCrypt.Core.EncrypedData.XmlPoco;
using EnvCrypt.Core.Utils;

namespace EnvCrypt.Core.EncrypedData.Mapper.Xml.ToXmlPoco
{
    class DatToXmlMapper : IDatToExternalRepresentationMapper<EnvCryptEncryptedData>
    {
        private readonly IStringPersistConverter _strConverter;

        public DatToXmlMapper(IStringPersistConverter strConverter)
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
                    var currentEntry = currentCategory.Entries[(int)entryI];

                    //TODO


                    xmlCategoryEntries.Add(xmlEntryToAdd);
                }
                xmlCategoryToAdd.Entry = xmlCategoryEntries.ToArray();

                xmlCategories.Add(xmlCategoryToAdd);
            }


            return new EnvCryptEncryptedData()
            {
                Items = xmlCategories.ToArray()
            };
        }
    }
}
