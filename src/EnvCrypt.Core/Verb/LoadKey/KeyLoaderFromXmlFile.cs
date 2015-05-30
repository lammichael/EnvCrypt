using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Key.XmlPoco;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO;

namespace EnvCrypt.Core.Verb.LoadKey
{
    [ContractClass(typeof(KeyLoaderFromXmlFileContracts<>))]
    abstract class KeyLoaderFromXmlFile<TKey> : IKeyLoader<TKey>
        where TKey : KeyBase
    {
        private readonly IMyFile _myFile;
        private readonly ITextReader _xmlReader;
        private readonly IXmlSerializationUtils<EnvCryptKey> _xmlSerializationUtils;

        protected KeyLoaderFromXmlFile(IMyFile myFile, ITextReader xmlReader, IXmlSerializationUtils<EnvCryptKey> xmlSerializationUtils)
        {
            Contract.Requires<ArgumentNullException>(myFile != null, "myFile");
            Contract.Requires<ArgumentNullException>(xmlReader != null, "xmlReader");
            Contract.Requires<ArgumentNullException>(xmlSerializationUtils != null, "xmlSerializationUtils");
            //
            _myFile = myFile;
            _xmlReader = xmlReader;
            _xmlSerializationUtils = xmlSerializationUtils;
        }


        public TKey Load(string ecKeyFilePath)
        {
            if (!_myFile.Exists(ecKeyFilePath))
            {
                throw new EnvCryptException("key file does not exist: {0}", ecKeyFilePath);
            }

            var xmlFromFile = _xmlReader.ReadAllText(ecKeyFilePath);
            if (string.IsNullOrWhiteSpace(xmlFromFile))
            {
                throw new EnvCryptException("key file is empty: {0}", ecKeyFilePath);
            }

            var xmlPoco = _xmlSerializationUtils.Deserialize(xmlFromFile);

            return MapToKeyPoco(xmlPoco);
        }


        protected abstract TKey MapToKeyPoco(EnvCryptKey fromXml);
    }


    [ContractClassFor(typeof(KeyLoaderFromXmlFile<>))]
    internal abstract class KeyLoaderFromXmlFileContracts<TKey> : KeyLoaderFromXmlFile<TKey>
        where TKey : KeyBase
    {
        protected KeyLoaderFromXmlFileContracts(IMyFile myFile, ITextReader xmlReader, IXmlSerializationUtils<EnvCryptKey> xmlSerializationUtils) : base(myFile, xmlReader, xmlSerializationUtils)
        {}


        protected override TKey MapToKeyPoco(EnvCryptKey fromXml)
        {
            Contract.Requires<ArgumentNullException>(fromXml != null, "fromXml");
            Contract.Ensures(Contract.Result<TKey>() != null);

            return default(TKey);
        }
    }
}