using System;

namespace EnvCrypt.Core.UnitTest
{
    internal static class RandomByteArrayUtils
    {
        public static byte[] CreateRandomByteArray(int ofSize)
        {
            var ret = new byte[ofSize];
            var ran = new Random();
            ran.NextBytes(ret);
            return ret;
        }
    }
}
