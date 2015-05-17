using EnvCrypt.Core.EncrypedData.Poco;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.EncrypedData
{
    class EntryMaker<T> where T : KeyBase
    {
        private IEncryptionAlgo<T> _algo;


        /*public Entry CreateEntry(string containingStr, T usingKey)
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(containingStr));
            //
            var binaryToEncrypt = containingStr.ToUnicodeByteArray();
            return CreateEntry(binaryToEncrypt, usingKey);
        }*/


        public Entry CreateEntry(byte[] containingBinary, T usingKey)
        {

            var encryptedData = _algo.Encrypt(containingBinary, usingKey);
            var ret = new Entry()
            {

            };

            return ret;
        }
    }
}
