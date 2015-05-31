using System.Collections.Generic;

namespace EnvCrypt.Core.Utils
{
    static class ByteArrayHashUtils
    {
        /// <summary>
        /// Taken from http://stackoverflow.com/questions/7244699/gethashcode-on-byte-array
        /// </summary>
        public static int GetHashCode(byte[] array)
        {
            var equalityComparer = EqualityComparer<byte>.Default;
            unchecked
            {
                if (array == null)
                {
                    return 0;
                }
                int hash = 17;
                for (uint i = 0; i < array.Length; i++)
                {
                    hash = hash * 31 + equalityComparer.GetHashCode(array[i]);
                }
                return hash;
            }
        } 
    }
}
