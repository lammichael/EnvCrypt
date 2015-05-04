using System;

namespace EnvCrypt.Core.EncryptionAlgo
{
    public class EnvCryptAlgoException : Exception
    {
        public EnvCryptAlgoException()
        {}

        public EnvCryptAlgoException(string message)
            : base(message)
        {}

        public EnvCryptAlgoException(string message, Exception inner)
            : base(message, inner)
        {}
    }
}
