using System;
using System.Diagnostics.Contracts;
using CommandLine;

namespace EnvCrypt.Console
{
    [ContractClass(typeof(VerbCommandLineProcessorContracts<>))]
    internal interface IVerbCommandLineProcessor<TOptions>
        where TOptions : class
    {
        bool Run(ParserResult<object> parserResult);
    }


    [ContractClassFor(typeof(IVerbCommandLineProcessor<>))]
    internal abstract class VerbCommandLineProcessorContracts<TOptions> : IVerbCommandLineProcessor<TOptions>
        where TOptions : class
    {
        public bool Run(ParserResult<object> parserResult)
        {
            Contract.Requires<ArgumentNullException>(parserResult != null, "parserResult");

            return default(bool);
        }
    }
}