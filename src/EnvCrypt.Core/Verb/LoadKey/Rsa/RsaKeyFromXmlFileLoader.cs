using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key.Mapper;
using EnvCrypt.Core.Key.Rsa;
using EnvCrypt.Core.Key.XmlPoco;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO;

namespace EnvCrypt.Core.Verb.LoadKey.Rsa
{
    public class RsaKeyFromXmlFileLoader : KeyFromXmlFileLoader<RsaKey, KeyFromFileDetails>
    {
        private readonly IExternalRepresentationToKeyMapper<EnvCryptKey, RsaKey> _mapper;

        public RsaKeyFromXmlFileLoader(IMyFile myFile, ITextReader xmlReader, IXmlSerializationUtils<EnvCryptKey> xmlSerializationUtils, IExternalRepresentationToKeyMapper<EnvCryptKey, RsaKey> mapper) : base(myFile, xmlReader, xmlSerializationUtils)
        {
            Contract.Requires<ArgumentNullException>(myFile != null, "myFile");
            Contract.Requires<ArgumentNullException>(xmlReader != null, "xmlReader");
            Contract.Requires<ArgumentNullException>(xmlSerializationUtils != null, "xmlSerializationUtils");
            Contract.Requires<ArgumentNullException>(mapper != null, "mapper");
            //
            _mapper = mapper;
        }


        protected override RsaKey MapToKeyPoco(EnvCryptKey fromXml)
        {
            return _mapper.Map(fromXml);
        }
    }
}