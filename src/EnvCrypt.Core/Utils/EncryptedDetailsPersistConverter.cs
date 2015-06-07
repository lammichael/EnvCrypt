using System;
using EnvCrypt.Core.EncrypedData.UserStringConverter;
using EnvCrypt.Core.EncryptionAlgo;

namespace EnvCrypt.Core.Utils
{
    class EncryptedDetailsPersistConverter : IEncryptedDetailsPersistConverter
    {
        private readonly IUserStringConverter _userStringConverter;

        public EncryptedDetailsPersistConverter(IUserStringConverter userStringConverter)
        {
            _userStringConverter = userStringConverter;
        }


        public string Encode(byte[] dataToPersist, EnvCryptAlgoEnum algorithm)
        {
            if (algorithm == EnvCryptAlgoEnum.PlainText)
            {
                return _userStringConverter.Decode(dataToPersist);
            }

            return Convert.ToBase64String(dataToPersist);
        }


        public byte[] Decode(string persistedStr, EnvCryptAlgoEnum algorithm)
        {
            if (algorithm == EnvCryptAlgoEnum.PlainText)
            {
                return _userStringConverter.Encode(persistedStr);
            }

            return Convert.FromBase64String(persistedStr);
        }
    }
}