using CommandLine;

namespace EnvCrypt.Console.Options
{
    class VerbOptionsBase
    {
        [Option('v', "Verbose", HelpText = "Verbosity of logging output.", Required = false)]
        public bool Verbose { get; set; }
    }
}
