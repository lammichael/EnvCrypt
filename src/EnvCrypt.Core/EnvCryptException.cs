using System;

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
    }
}
