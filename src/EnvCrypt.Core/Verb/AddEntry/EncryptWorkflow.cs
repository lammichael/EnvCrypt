using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncrypedData.UserStringConverter;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Verb.LoadKey;

namespace EnvCrypt.Core.Verb.AddEntry
{
    public class EncryptWorkflow<TKey>
        where TKey : KeyBase
    {
        private readonly IKeyLoader<TKey> _keyLoader;
        private readonly ICanEncryptUsingKeyChecker<TKey> _keyChecker;
        private readonly IUserStringConverter _userStringConverter;
        private readonly ISegmentEncryptionAlgo<TKey> _segmentEncrypter;

        public EncryptWorkflow(IKeyLoader<TKey> keyLoader, ICanEncryptUsingKeyChecker<TKey> keyChecker, IUserStringConverter userStringConverter, ISegmentEncryptionAlgo<TKey> segmentEncrypter)
        {
            Contract.Requires<ArgumentNullException>(keyLoader != null, "keyLoader");
            Contract.Requires<ArgumentNullException>(keyChecker != null, "keyChecker");
            Contract.Requires<ArgumentNullException>(userStringConverter != null, "userStringConverter");
            Contract.Requires<ArgumentNullException>(segmentEncrypter != null, "segmentEncrypter");
            //
            _keyLoader = keyLoader;
            _keyChecker = keyChecker;
            _userStringConverter = userStringConverter;
            _segmentEncrypter = segmentEncrypter;
        }


        public IList<byte[]> GetEncryptedSegments(string usingKeyFilePath, string toEncrypt, out TKey withKey)
        {
            Contract.Requires<ArgumentNullException>(usingKeyFilePath != null, "usingKeyFilePath");
            Contract.Requires<ArgumentNullException>(toEncrypt != null, "toEncrypt");
            Contract.Ensures(Contract.ValueAtReturn(out withKey) != null);
            //
            var key = _keyLoader.Load(usingKeyFilePath);
            
            if(!_keyChecker.IsEncryptingKey(key))
            {
                throw new EnvCryptException("impossible to encrypt using this {0} key. Name: {1}", key.Algorithm, key.Name);
            }

            var binaryToEncrypt = _userStringConverter.Encode(toEncrypt);

            withKey = key;
            return _segmentEncrypter.Encrypt(binaryToEncrypt, key);
        }
    }
}
