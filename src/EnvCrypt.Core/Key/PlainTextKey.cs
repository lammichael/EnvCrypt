using EnvCrypt.Core.EncryptionAlgo;

namespace EnvCrypt.Core.Key
{
    /// <summary>
    /// Key used when value is stored in plain text.
    /// </summary>
    class PlainTextKey : KeyBase
    {
        public override EnvCryptAlgoEnum Algorithm
        {
            get
            {
                return EnvCryptAlgoEnum.PlainText;
            }
        }
    }
}
