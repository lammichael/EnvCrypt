using System;
using System.Diagnostics.Contracts;

namespace EnvCrypt.Core.Utils.IO
{
    [ContractClass(typeof(TextReaderContracts))]
    public interface ITextReader
    {
        string ReadAllText(string path);
    }


    [ContractClassFor(typeof(ITextReader))]
    internal abstract class TextReaderContracts : ITextReader
    {
        public string ReadAllText(string path)
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(path), "path");
            Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()));

            return default(string);
        }
    }
}