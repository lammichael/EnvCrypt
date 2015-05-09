using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvCrypt.Core
{
    public static class StringUtils
    {
        public static byte[] ToUnicodeByteArray(this string str)
        {
            Contract.Requires<ArgumentNullException>(str != null, "str");
            Contract.Ensures(Contract.Result<byte[]>() != null);
            //
            var byteConverter = new UnicodeEncoding();
            return byteConverter.GetBytes(str);
        }
    }
}
