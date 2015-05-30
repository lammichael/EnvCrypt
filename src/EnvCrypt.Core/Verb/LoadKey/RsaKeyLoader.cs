using EnvCrypt.Core.EncryptionAlgo.Rsa.Key;
using EnvCrypt.Core.Key.Mapper;
using EnvCrypt.Core.Key.XmlPoco;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO;

namespace EnvCrypt.Core.Verb.LoadKey
{
    class RsaKeyLoader : KeyLoaderFromXmlFile<RsaKey>
    {
        private readonly IExternalRepresentationToKeyMapper<EnvCryptKey, RsaKey> _mapper;

        public RsaKeyLoader(IMyFile myFile, ITextReader xmlReader, IXmlSerializationUtils<EnvCryptKey> xmlSerializationUtils, IExternalRepresentationToKeyMapper<EnvCryptKey, RsaKey> mapper) : base(myFile, xmlReader, xmlSerializationUtils)
        {
            _mapper = mapper;
        }


        protected override RsaKey MapToKeyPoco(EnvCryptKey fromXml)
        {
            return _mapper.Map(fromXml);
        }
    }
}