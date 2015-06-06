using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Key.Mapper;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO.StringWriter;

namespace EnvCrypt.Core.Verb.GenerateKey.Persister.Symmetric
{
    public class SymmetricKeyFilePersister<TKey, TKeyXmlPoco, TFileWriterOptions> : ISymmetricKeyFilePersister<TKey>
        where TKey : KeyBase, ISymmetricKeyMarker
        where TKeyXmlPoco : class, IKeyExternalRepresentation<TKey>, new()
        where TFileWriterOptions : StringToFileWriterOptions, new()
    {
        private readonly IKeyToExternalRepresentationMapper<TKey, TKeyXmlPoco> _pocoMapper;
        private readonly IXmlSerializationUtils<TKeyXmlPoco> _serializationUtils;
        private readonly IStringWriter<TFileWriterOptions> _writer;

        internal SymmetricKeyFilePersister(
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


        public void Persist(TKey thisKey, SymmetricKeyFilePersisterOptions withOptions)
        {
            var privateKeyXmlPoco = new TKeyXmlPoco();
            _pocoMapper.Map(thisKey, privateKeyXmlPoco);
            var toWrite = _serializationUtils.Serialize(privateKeyXmlPoco);

            var fileWriterOptions = new TFileWriterOptions()
            {
                Contents = toWrite,
                Encoding = _serializationUtils.GetUsedEncoding(),
                Path = withOptions.NewKeyFileFullPath,
                OverwriteIfFileExists = withOptions.OverwriteFileIfExists
            };
            _writer.Write(fileWriterOptions);
        }
    }
}
