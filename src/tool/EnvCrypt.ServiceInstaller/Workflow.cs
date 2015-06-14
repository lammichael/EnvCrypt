using System;
using EnvCrypt.Core.Verb.DecryptEntry;
using EnvCrypt.Core.Verb.DecryptEntry.Aes;
using EnvCrypt.Core.Verb.DecryptEntry.Generic;
using EnvCrypt.Core.Verb.DecryptEntry.PlainText;
using EnvCrypt.Core.Verb.DecryptEntry.Rsa;
using EnvCrypt.ServiceInstaller.CommandLine;

namespace EnvCrypt.ServiceInstaller
{
    class Workflow
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public void Run(CommandLineOptions options)
        {
            Logger.Info("Uninstalling service: {0}", options.ServiceName);
            ServiceInstaller.Uninstall(options.ServiceName, false);

            string fidPassword = null;
            if (string.IsNullOrWhiteSpace(options.FunctionalId))
            {
                options.FunctionalId = null;
            }
            else
            {
                Logger.Info("Decrypting password. Category: {0}  Entry: {1}",
                    options.Category, options.Entry);

                var builder = new DecryptGenericWorkflowBuilder(
                new DecryptPlainTextEntryWorkflowBuilder(), 
                new DecryptRsaEntryWorkflowBuilder(), 
                new DecryptAesEntryWorkflowBuilder());
                var result = builder.Build().Run(new DecryptGenericWorkflowOptions()
                {
                    CategoryEntryPair = new []
                    {
                        new CategoryEntryPair(options.Category, options.Entry), 
                    },
                    DatFilePath = options.DatFile,
                    KeyFilePath = options.KeyFile,
                    ThrowExceptionIfEntryNotFound = true,
                });

                if (result.Count == 0)
                {
                    throw new Exception("could not find Category: " + options.Category + " Entry:" + options.Entry);
                }
                fidPassword = result[0].DecryptedValue;
            }

            Logger.Info("Installing service: {0}", options.ServiceName);
            ServiceInstaller.Install(
                name: options.ServiceName,
                displayName: options.DisplayName,
                startType: GetBootFlag(options.GetServiceStartType()),
                binaryPath: "\"" + options.BinaryPath + "\"",
                runAsFid: options.FunctionalId,
                fidPassword: fidPassword);
        }


        private ServiceBootFlag GetBootFlag(ServiceStartTypeEnum? startType)
        {
            switch (startType)
            {
                case ServiceStartTypeEnum.Disabled:
                    return ServiceBootFlag.Disabled;
                    break;
                case ServiceStartTypeEnum.AutoStart:
                    return ServiceBootFlag.AutoStart;
                    break;
                case ServiceStartTypeEnum.DemandStart:
                    return ServiceBootFlag.DemandStart;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("startType");
            }
        }
    }
}
