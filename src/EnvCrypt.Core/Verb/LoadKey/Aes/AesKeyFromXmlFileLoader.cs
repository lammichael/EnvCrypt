using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key.Aes;
using EnvCrypt.Core.Key.Mapper;
using EnvCrypt.Core.Key.XmlPoco;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO;

namespace EnvCrypt.Core.Verb.LoadKey.Aes
{
    public class AesKeyFromXmlFileLoader : KeyFromXmlFileLoader<AesKey, KeyFromFileDetails>
    {
        private readonly IExternalRepresentationToKeyMapper<EnvCryptKey, AesKey> _mapper;

        public AesKeyFromXmlFileLoader(IMyFile myFile, ITextReader xmlReader, IXmlSerializationUtils<EnvCryptKey> xmlSerializationUtils, IExternalRepresentationToKeyMapper<EnvCryptKey, AesKey> mapper)
            : base(myFile, xmlReader, xmlSerializationUtils)
        {
            Contract.Requires<ArgumentNullException>(myFile != null, "myFile");
            Contract.Requires<ArgumentNullException>(xmlReader != null, "xmlReader");
            Contract.Requires<ArgumentNullException>(xmlSerializationUtils != null, "xmlSerializationUtils");
            Contract.Requires<ArgumentNullException>(mapper != null, "mapper");
            //
            _mapper = mapper;
        }


        protected override AesKey MapToKeyPoco(EnvCryptKey fromXml)
        {
            return _mapper.Map(fromXml);
        }
    }
}