using System;
using System.Diagnostics.Contracts;

namespace EnvCrypt.Core.Utils.IO.StringWriter
{
    [ContractClass(typeof(StringWriterContracts<>))]
    public interface IStringWriter<in TOptions>
        where TOptions : StringWriterOptions
    {
        void Write(TOptions options); //, string path, bool overwriteExisting);
    }


    [ContractClassFor(typeof(IStringWriter<>))]
    internal abstract class StringWriterContracts<TOptions> : IStringWriter<TOptions>
        where TOptions : StringWriterOptions
    {
        public void Write(TOptions options)
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(options.Contents), "contents to write cannot be null or empty");
            Contract.Requires<ArgumentNullException>(options.Encoding != null, "encoding");
        }
    }
}
