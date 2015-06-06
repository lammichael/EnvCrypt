using System;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using EnvCrypt.Core.Key.Mapper;
using EnvCrypt.Core.Key.Rsa;
using EnvCrypt.Core.Key.XmlPoco;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO.StringWriter;
using EnvCrypt.Core.Verb.GenerateKey.Persister.Asymetric;

namespace EnvCrypt.Core.Verb.GenerateKey.Rsa
{
    class RsaKeyPersister : AsymmetricKeyFilePersister<RsaKey, EnvCryptKey, StringToFileWriterOptions>
    {
        public RsaKeyPersister(
            IKeyToExternalRepresentationMapper<RsaKey, EnvCryptKey> pocoMapper,
            IXmlSerializationUtils<EnvCryptKey> serializationUtils, 
            IStringWriter<StringToFileWriterOptions> writer)
            : base(pocoMapper, serializationUtils, writer)
        {
            Contract.Requires<ArgumentNullException>(pocoMapper != null, "pocoMapper");
            Contract.Requires<ArgumentNullException>(serializationUtils != null, "serializationUtils");
            Contract.Requires<ArgumentNullException>(writer != null, "writer");
        }

        protected override RsaKey GetPublicKey(RsaKey fromPrivateKey)
        {
            var publicKey = new RSAParameters()
            {
                Exponent = fromPrivateKey.Key.Exponent,
                Modulus = fromPrivateKey.Key.Modulus,
            };
            return new RsaKey(publicKey, fromPrivateKey.UseOaepPadding)
            {
                Name = fromPrivateKey.Name
            };
        }
    }
}