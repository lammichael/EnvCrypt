using EnvCrypt.Core.EncrypedData.UserStringConverter;

namespace EnvCrypt.Core.Utils
{
    /// <summary>
    /// Created to keep the text in the DAT file as readable text.
    /// However another workflow for plaintext could be created to avoid
    /// unecessary convertion to and from Unicode (see <see cref="Utf16LittleEndianUserStringConverter"/>)
    /// </summary>
    class PlainTextPersistConverter : IStringPersistConverter
    {
        private readonly IUserStringConverter _userStringConverter;

        public PlainTextPersistConverter(IUserStringConverter userStringConverter)
        {
            _userStringConverter = userStringConverter;
        }


        public string Encode(byte[] dataToPersist)
        {
            return _userStringConverter.Decode(dataToPersist);
        }


        public byte[] Decode(string persistedStr)
        {
            return _userStringConverter.Encode(persistedStr);
        }
    }
}
