using System;

namespace EnvCrypt.ServiceInstaller.CommandLine
{
    static class Validator
    {
        public static void ThrowIfInvalid(CommandLineOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.ServiceName))
            {
                throw new Exception("Service Name not defined.");
            }
            if (string.IsNullOrWhiteSpace(options.DisplayName))
            {
                throw new Exception("Display Name not defined.");
            }
            if (options.GetServiceStartType() == null)
            {
                throw new Exception("Service Start Type not recognised - " + options.ServiceStartType);
            }
            if (string.IsNullOrWhiteSpace(options.BinaryPath))
            {
                throw new Exception("Binary Path not defined.");
            }

            if (!string.IsNullOrWhiteSpace(options.FunctionalId))
            {
                if (string.IsNullOrWhiteSpace(options.DatFile))
                {
                    throw new Exception("DAT file path not defined.");
                }
                if (string.IsNullOrWhiteSpace(options.Category))
                {
                    throw new Exception("Category not defined.");
                }
                if (string.IsNullOrWhiteSpace(options.Entry))
                {
                    throw new Exception("Entry name not defined.");
                }
            }
        }
    }
}
