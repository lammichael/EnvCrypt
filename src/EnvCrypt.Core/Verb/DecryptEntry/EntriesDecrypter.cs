using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using EnvCrypt.Core.EncrypedData;
using EnvCrypt.Core.EncrypedData.Poco;
using EnvCrypt.Core.EncrypedData.UserStringConverter;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.Verb.DecryptEntry
{
    public class EntriesDecrypter<TKey>
        where TKey : KeyBase
    {
        private readonly IKeySuitabilityChecker<TKey> _keySuitabilityChecker;
        private readonly IUserStringConverter _userStringConverter;
        private readonly ISegmentEncryptionAlgo<TKey> _segmentEncrypter;

        public EntriesDecrypter(IKeySuitabilityChecker<TKey> keySuitabilityChecker, IUserStringConverter userStringConverter, ISegmentEncryptionAlgo<TKey> segmentEncrypter)
        {
            Contract.Requires<ArgumentNullException>(keySuitabilityChecker != null, "keyChecker");
            Contract.Requires<ArgumentNullException>(userStringConverter != null, "userStringConverter");
            Contract.Requires<ArgumentNullException>(segmentEncrypter != null, "segmentEncrypter");
            //
            _keySuitabilityChecker = keySuitabilityChecker;
            _userStringConverter = userStringConverter;
            _segmentEncrypter = segmentEncrypter;
        }


        public IList<EntriesDecrypterResult> Decrypt(IList<TKey> usingKeys, EnvCryptDat inDat, IList<CategoryEntryPair> categoryEntryPairs, bool throwExceptionIfEntryNotFound = true, bool throwIfDecryptingKeyNotFound = true, bool throwIfKeyCannotDecrypt = true)
        {
            Contract.Requires<ArgumentNullException>(usingKeys != null, "usingKeys");
            Contract.Requires<ArgumentException>(usingKeys.Any(), "usingKeys");
            Contract.Requires<EnvCryptException>(Contract.ForAll(usingKeys, k => k != null), "all keys in the list can be null");
            Contract.Requires<ArgumentNullException>(inDat != null, "inDat");
            Contract.Requires<ArgumentNullException>(categoryEntryPairs != null, "entryDetails");

            //
            var ret = new List<EntriesDecrypterResult>();

            var keysToUse = new List<TKey>(usingKeys.Count);
            for (uint kI = 0; kI < usingKeys.Count; kI++)
            {
                var currentKey = usingKeys[(int) kI];
                if (!_keySuitabilityChecker.IsDecryptingKey(currentKey))
                {
                    if (throwIfKeyCannotDecrypt)
                    {
                        throw new EnvCryptException("impossible to decrypt using this {0} key. Name: {1}",
                            currentKey.Algorithm, currentKey.Name);
                    }
                }
                else
                {
                    keysToUse.Add(currentKey);
                }
            }

            if (!keysToUse.Any())
            {
                return ret;
            }


            for (uint tI = 0; tI < categoryEntryPairs.Count; tI++)
            {
                var currentRequest = categoryEntryPairs[(int)tI];
                var catName = currentRequest.Category;
                var entryName = currentRequest.Entry;

                Entry foundEntry;
                if (inDat.SearchForEntry(catName, entryName, out foundEntry))
                {
                    EntriesDecrypterResult toAdd = null;
                    for (uint kI = 0; kI < keysToUse.Count; kI++)
                    {
                        
                        var currentKey = keysToUse[(int)kI];
                        if (currentKey.Name == foundEntry.KeyName &&
                            currentKey.GetHashCode() == foundEntry.KeyHash)
                        {
                            var encodedDecryptedData = _segmentEncrypter.Decrypt(foundEntry.EncryptedValue, currentKey);

                            toAdd = new EntriesDecrypterResult
                            {
                                CategoryEntryPair = currentRequest,
                                DecryptedValue = _userStringConverter.Decode(encodedDecryptedData)
                            };

                            ret.Add(toAdd);
                            break;
                        }
                    }
                    if (toAdd == null)
                    {
                        // Haven't found the key to decrypt this entry
                        if (throwIfDecryptingKeyNotFound)
                        {
                            throw new EnvCryptException("cannot find suitable key to decrypt. Entry name: {0}  Category: {1}  Required Key Name: {2}  Required Key Hash: {3}", entryName, catName, foundEntry.Name, foundEntry.KeyHash);
                        }
                    }
                }
                else
                {
                    if (throwExceptionIfEntryNotFound)
                    {
                        throw new EnvCryptException("entry not found.  Entry name: {0}  Category: {1}", entryName, catName);
                    }
                }
            }

            return ret;
        }


        public IList<EntriesDecrypterResult> Decrypt(
            IList<TKey> usingKeys,
            EnvCryptDat inDat,
            CategoryEntryPair categoryEntryPair,
            bool throwExceptionIfEntryNotFound = true,
            bool throwIfDecryptingKeyNotFound = true,
            bool throwIfKeyCannotDecrypt = true)
        {
            Contract.Requires<ArgumentNullException>(usingKeys != null, "usingKeys");
            Contract.Requires<ArgumentException>(usingKeys.Any(), "usingKeys");
            Contract.Requires<EnvCryptException>(Contract.ForAll(usingKeys, k => k != null), "all keys in the list can be null");
            Contract.Requires<ArgumentNullException>(inDat != null, "inDat");
            //
            return this.Decrypt(usingKeys, inDat, new[] {categoryEntryPair}, throwExceptionIfEntryNotFound,
                throwIfDecryptingKeyNotFound, throwIfKeyCannotDecrypt);
        }


        public IList<EntriesDecrypterResult> Decrypt(
            TKey usingKey,
            EnvCryptDat inDat,
            CategoryEntryPair categoryEntryPair,
            bool throwExceptionIfEntryNotFound = true,
            bool throwIfDecryptingKeyNotFound = true,
            bool throwIfKeyCannotDecrypt = true)
        {
            Contract.Requires<ArgumentNullException>(usingKey != null, "usingKey");
            Contract.Requires<ArgumentNullException>(inDat != null, "inDat");
            //
            return this.Decrypt(new[] {usingKey}, inDat, new[] {categoryEntryPair}, throwExceptionIfEntryNotFound,
                throwIfDecryptingKeyNotFound, throwIfKeyCannotDecrypt);
        }
    }
}
