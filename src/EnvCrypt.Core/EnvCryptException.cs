using System;
using System.Diagnostics.Contracts;

namespace EnvCrypt.Core
{
    public class EnvCryptException : Exception
    {
        public EnvCryptException()
        {}

        public EnvCryptException(string message)
            : base(message)
        {}

        public EnvCryptException(string message, Exception inner)
            : base(message, inner)
        {}

        public EnvCryptException(string format, params object[] args)
            : base(string.Format(format, args))
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(format));            
        }
    }
}
