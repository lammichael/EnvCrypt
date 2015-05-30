using EnvCrypt.Core.EncryptionAlgo.Aes.Key;
using EnvCrypt.Core.Key.Mapper;
using EnvCrypt.Core.Key.XmlPoco;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO;

namespace EnvCrypt.Core.Verb.LoadKey
{
    class AesKeyLoader : KeyLoaderFromXmlFile<AesKey>
    {
        private readonly IExternalRepresentationToKeyMapper<EnvCryptKey, AesKey> _mapper;

        public AesKeyLoader(IMyFile myFile, ITextReader xmlReader, IXmlSerializationUtils<EnvCryptKey> xmlSerializationUtils, IExternalRepresentationToKeyMapper<EnvCryptKey, AesKey> mapper)
            : base(myFile, xmlReader, xmlSerializationUtils)
        {
            _mapper = mapper;
        }


        protected override AesKey MapToKeyPoco(EnvCryptKey fromXml)
        {
            return _mapper.Map(fromXml);
        }
    }
}