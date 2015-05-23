using System;
using System.Diagnostics.Contracts;
using System.Text;

namespace EnvCrypt.Core.Utils.IO
{
    [ContractClass(typeof(StringToFileWriterContracts))]
    interface IStringToFileWriter
    {
        void Write(string path, string contents, bool overwriteExisting,Encoding encoding);
    }


    [ContractClassFor(typeof(IStringToFileWriter))]
    internal abstract class StringToFileWriterContracts : IStringToFileWriter
    {
        public void Write(string path, string contents, bool overwriteExisting, Encoding encoding)
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(path), "path");
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(contents), "contents");
            Contract.Requires<ArgumentNullException>(encoding != null, "encoding");
        }
    }
}
