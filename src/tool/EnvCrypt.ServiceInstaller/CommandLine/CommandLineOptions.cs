using System;
using CommandLine;

namespace EnvCrypt.ServiceInstaller.CommandLine
{
    class CommandLineOptions
    {
        [Option('s', "ServiceName", HelpText = "Name of service to be created.", Required = true)]
        public string ServiceName { get; set; }

        [Option('n', "DisplayName", HelpText = "Display name of service.", Required = true)]
        public string DisplayName { get; set; }

        [Option('t', "StartType", HelpText = "Start type of service - Disabled, AutoStart, DemandStart", Required = true)]
        public string ServiceStartType { get; set; }

        [Option('b', "BinaryPath", HelpText = "Path to binary (no need to include escaped quotes to cater for spaces).", Required = true)]
        public string BinaryPath { get; set; }

        [Option('f', "FID", HelpText = "Functional ID the service should run as.  If left blank, the service will be created without this and the EnvCrypt decryption will not be done.", Required = true)]
        public string FunctionalId { get; set; }

        [Option('d', "Dat", HelpText = "Full path to ECDat file containing the encrypted entries use for encryption. If ECDat does not already exist then it will be created.", Required = false)]
        public string DatFile { get; set; }

        [Option('k', "Key", HelpText = "Full path to ECKey file to use for encryption.", Required = false)]
        public string KeyFile { get; set; }

        [Option('c', "Category", HelpText = "Category of Functional ID password.", Required = false)]
        public string Category { get; set; }

        [Option('e', "Entry", HelpText = "Entry of Functional ID password.", Required = false)]
        public string Entry { get; set; }


        public ServiceStartTypeEnum? GetServiceStartType()
        {
            ServiceStartTypeEnum ret;
            if (Enum.TryParse(ServiceStartType, true, out ret))
            {
                return ret;
            }
            return null;
        }
    }
}
