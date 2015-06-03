using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Key.XmlPoco;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO;

namespace EnvCrypt.Core.Verb.LoadKey
{
    [ContractClass(typeof(KeyFromXmlFileLoaderContracts<,>))]
    abstract class KeyFromXmlFileLoader<TKey, TLoadDetails> : IKeyLoader<TKey, TLoadDetails>
        where TKey : KeyBase
        where TLoadDetails : KeyFromFileDetails
    {
        private readonly IMyFile _myFile;
        private readonly ITextReader _xmlReader;
        private readonly IXmlSerializationUtils<EnvCryptKey> _xmlSerializationUtils;

        protected KeyFromXmlFileLoader(IMyFile myFile, ITextReader xmlReader, IXmlSerializationUtils<EnvCryptKey> xmlSerializationUtils)
        {
            Contract.Requires<ArgumentNullException>(myFile != null, "myFile");
            Contract.Requires<ArgumentNullException>(xmlReader != null, "xmlReader");
            Contract.Requires<ArgumentNullException>(xmlSerializationUtils != null, "xmlSerializationUtils");
            //
            _myFile = myFile;
            _xmlReader = xmlReader;
            _xmlSerializationUtils = xmlSerializationUtils;
        }


        public TKey Load(TLoadDetails keyFileDetails)
        {
            Contract.Requires<ArgumentNullException>(keyFileDetails != null, "keyFileDetails");
            Contract.Requires<ArgumentException>(!String.IsNullOrWhiteSpace(keyFileDetails.FilePath), "key file path cannot be empty");
            //
            if (!_myFile.Exists(keyFileDetails.FilePath))
            {
                throw new EnvCryptException("key file does not exist: {0}", keyFileDetails);
            }

            var xmlFromFile = _xmlReader.ReadAllText(keyFileDetails.FilePath);
            if (string.IsNullOrWhiteSpace(xmlFromFile))
            {
                throw new EnvCryptException("key file is empty: {0}", keyFileDetails);
            }

            var xmlPoco = _xmlSerializationUtils.Deserialize(xmlFromFile);

            return MapToKeyPoco(xmlPoco);
        }


        protected abstract TKey MapToKeyPoco(EnvCryptKey fromXml);
    }


    [ContractClassFor(typeof(KeyFromXmlFileLoader<,>))]
    internal abstract class KeyFromXmlFileLoaderContracts<TKey, TLoadDetails> : KeyFromXmlFileLoader<TKey, TLoadDetails>
        where TKey : KeyBase
        where TLoadDetails : KeyFromFileDetails
    {
        protected KeyFromXmlFileLoaderContracts(IMyFile myFile, ITextReader xmlReader, IXmlSerializationUtils<EnvCryptKey> xmlSerializationUtils) : base(myFile, xmlReader, xmlSerializationUtils)
        {}

        protected override TKey MapToKeyPoco(EnvCryptKey fromXml)
        {
            Contract.Requires<ArgumentNullException>(fromXml != null, "fromXml");
            Contract.Ensures(Contract.Result<TKey>() != null);

            return default(TKey);
        }
    }
}