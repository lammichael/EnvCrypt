using CommandLine;

namespace EnvCrypt.Console.AddEntry
{
    class AddEntryOptions
    {
        [Option('k', "Key", HelpText = "Full path to ECKey file to use for encryption. If excluded then key will be added in plaintext.", Required = true)]
        public string KeyFile { get; set; }

        [Option('c', "Category", HelpText = "Category to add entry under.", Required = true)]
        public string Category { get; set; }

        [Option('n', "Name", HelpText = "The new entry's name. Accepts comma separated names corresponding to entry values.", Required = true)]
        public string Name { get; set; }

        [Option('v', "Value", HelpText = "Value to encrypt and include in ECKey file. Accepts comma separated values corresponding to entry names.", Required = true)]
        public string ValueToEncrypt { get; set; }

        [Option('d', "Dat", HelpText = "Full path to ECDat file containing the encrypted entries use for encryption. If ECDat does not already exist then it will be created.", Required = true)]
        public string EcDatFile { get; set; }
    }
}
