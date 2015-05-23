using System;
using System.Diagnostics.Contracts;
using System.Text;

namespace EnvCrypt.Core.Utils.IO
{
    [ContractClass(typeof(TextWriterContract))]
    public interface ITextWriter
    {
        void WriteAllText(String path, String contents, Encoding encoding);
    }


    [ContractClassFor(typeof(ITextWriter))]
    abstract class TextWriterContract : ITextWriter
    {
        public void WriteAllText(string path, string contents, Encoding encoding)
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(path), "path");
            Contract.Requires<ArgumentException>(contents != null, "contents");
            Contract.Requires<ArgumentException>(encoding != null, "encoding");
        }
    }

}