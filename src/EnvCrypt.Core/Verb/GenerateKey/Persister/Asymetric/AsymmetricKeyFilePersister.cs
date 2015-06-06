using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Key.Mapper;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO.StringWriter;

namespace EnvCrypt.Core.Verb.GenerateKey.Persister.Asymetric
{
    [ContractClass(typeof(AsymmetricKeyFilePersisterContracts<,,>))]
    public abstract class AsymmetricKeyFilePersister<TKey, TKeyXmlPoco, TFileWriterOptions> : IAsymmetricKeyFilePersister<TKey, TKeyXmlPoco> 
        where TKey : KeyBase, IAsymmetricKeyMarker
        where TKeyXmlPoco : class, IKeyExternalRepresentation<TKey>, new()
        where TFileWriterOptions : StringToFileWriterOptions, new()
    {
        private readonly IKeyToExternalRepresentationMapper<TKey, TKeyXmlPoco> _pocoMapper;
        private readonly IXmlSerializationUtils<TKeyXmlPoco> _serializationUtils;
        private readonly IStringWriter<TFileWriterOptions> _writer;

        internal AsymmetricKeyFilePersister(
            IKeyToExternalRepresentationMapper<TKey, TKeyXmlPoco> pocoMapper,
            IXmlSerializationUtils<TKeyXmlPoco> serializationUtils,
            IStringWriter<TFileWriterOptions> writer)
        {
            Contract.Requires<ArgumentNullException>(pocoMapper != null, "pocoMapper");
            Contract.Requires<ArgumentNullException>(serializationUtils != null, "serializationUtils");
            Contract.Requires<ArgumentNullException>(writer != null, "writer");
            //
            _pocoMapper = pocoMapper;
            _serializationUtils = serializationUtils;
            _writer = writer;
        }


        public void Persist(TKey thisKey, AsymmetricKeyFilePersisterOptions withOptions)
        {
            {
                // Write private key
                var privateKeyXmlPoco = new TKeyXmlPoco();
                _pocoMapper.Map(thisKey, privateKeyXmlPoco);
                var toWrite = _serializationUtils.Serialize(privateKeyXmlPoco);

                var fileWriterOptions = new TFileWriterOptions()
                {
                    Contents = toWrite,
                    Encoding = _serializationUtils.GetUsedEncoding(),
                    Path = withOptions.NewPrivateKeyFullFilePath,
                    OverwriteIfFileExists = withOptions.OverwriteFileIfExists
                };
                _writer.Write(fileWriterOptions);
            }

            {
                // Write public key
                var publicKeyXmlPoco = new TKeyXmlPoco();
                _pocoMapper.Map(GetPublicKey(thisKey), publicKeyXmlPoco);
                var toWrite = _serializationUtils.Serialize(publicKeyXmlPoco);

                var fileWriterOptions = new TFileWriterOptions()
                {
                    Contents = toWrite,
                    Encoding = _serializationUtils.GetUsedEncoding(),
                    Path = withOptions.NewPublicKeyFullFilePath,
                    OverwriteIfFileExists = withOptions.OverwriteFileIfExists
                };
                _writer.Write(fileWriterOptions);
            }
        }

        [Pure]
        protected abstract TKey GetPublicKey(TKey fromPrivateKey);
    }



    [ContractClassFor(typeof(AsymmetricKeyFilePersister<,,>))]
    internal abstract class AsymmetricKeyFilePersisterContracts<TKey, TKeyXmlPoco, TFileWriterOptions> : AsymmetricKeyFilePersister<TKey, TKeyXmlPoco, TFileWriterOptions>
        where TKey : KeyBase, IAsymmetricKeyMarker
        where TKeyXmlPoco : class, IKeyExternalRepresentation<TKey>, new()
        where TFileWriterOptions : StringToFileWriterOptions, new()
    {
        protected AsymmetricKeyFilePersisterContracts(IKeyToExternalRepresentationMapper<TKey, TKeyXmlPoco> pocoMapper, IXmlSerializationUtils<TKeyXmlPoco> serializationUtils, IStringWriter<TFileWriterOptions> writer)
            : base(pocoMapper, serializationUtils, writer)
        {}

        protected override TKey GetPublicKey(TKey fromPrivateKey)
        {
            Contract.Ensures(Contract.Result<TKey>() != null);
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<TKey>().Name));

            return default(TKey);
        }
    }
}
