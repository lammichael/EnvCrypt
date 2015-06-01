using System;

namespace EnvCrypt.Console
{
    public class EnvCryptConsoleException : Exception
    {
        public EnvCryptConsoleException()
        {}

        public EnvCryptConsoleException(string message)
            : base(message)
        {}

        public EnvCryptConsoleException(string message, Exception inner)
            : base(message, inner)
        {}

        public EnvCryptConsoleException(string format, params object[] args)
            : base(string.Format(format, args))
        {}
    }
}
