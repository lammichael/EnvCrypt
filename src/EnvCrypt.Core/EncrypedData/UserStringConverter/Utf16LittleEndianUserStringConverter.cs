using System.Text;

namespace EnvCrypt.Core.EncrypedData.UserStringConverter
{
    /// <summary>
    /// Converts user inputted string to a binary representation and back using
    /// UTF-16 and little endian.
    /// </summary>
    class Utf16LittleEndianUserStringConverter : IUserStringConverter
    {
        public byte[] Encode(string userStr)
        {
            return Encoding.Unicode.GetBytes(userStr);
        }

        public string Decode(byte[] decrypedData)
        {
            return Encoding.Unicode.GetString(decrypedData);
        }
    }
}