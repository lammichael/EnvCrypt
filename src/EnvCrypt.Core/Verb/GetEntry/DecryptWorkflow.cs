using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncrypedData.UserStringConverter;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Verb.LoadKey;

namespace EnvCrypt.Core.Verb.GetEntry
{
    public class DecryptWorkflow<TKey>
        where TKey : KeyBase
    {
        private readonly IKeySuitabilityChecker<TKey> _keySuitabilityChecker;
        private readonly IUserStringConverter _userStringConverter;
        private readonly ISegmentEncryptionAlgo<TKey> _segmentEncrypter;

        public DecryptWorkflow(IKeySuitabilityChecker<TKey> keySuitabilityChecker, IUserStringConverter userStringConverter, ISegmentEncryptionAlgo<TKey> segmentEncrypter)
        {
            Contract.Requires<ArgumentNullException>(keySuitabilityChecker != null, "keyChecker");
            Contract.Requires<ArgumentNullException>(userStringConverter != null, "userStringConverter");
            Contract.Requires<ArgumentNullException>(segmentEncrypter != null, "segmentEncrypter");
            //
            _keySuitabilityChecker = keySuitabilityChecker;
            _userStringConverter = userStringConverter;
            _segmentEncrypter = segmentEncrypter;
        }


        public string GetDecryptedString(IList<byte[]> toDecrypt, TKey usingKey)
        {
            Contract.Requires<ArgumentNullException>(toDecrypt != null, "toDecrypt");
            Contract.Requires<ArgumentNullException>(usingKey != null, "usingKey");
            //
            if (!_keySuitabilityChecker.IsDecryptingKey(usingKey))
            {
                throw new EnvCryptException("impossible to decrypt using this {0} key. Name: {1}", usingKey.Algorithm, usingKey.Name);
            }

            var encodedDecryptedData = _segmentEncrypter.Decrypt(toDecrypt, usingKey);
            var decodedDecryptedData = _userStringConverter.Decode(encodedDecryptedData);
            return decodedDecryptedData;
        }
    }
}
