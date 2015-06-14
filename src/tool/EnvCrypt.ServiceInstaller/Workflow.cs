using System;
using EnvCrypt.ServiceInstaller.CommandLine;

namespace EnvCrypt.ServiceInstaller
{
    class Workflow
    {
        public void Run(CommandLineOptions options)
        {
            ServiceInstaller.Uninstall(options.ServiceName);

            string fidPassword = null;
            if (string.IsNullOrWhiteSpace(options.FunctionalId))
            {
                options.FunctionalId = null;
            }
            else
            {
                
            }

            ServiceInstaller.Install(name: options.ServiceName, displayName: null,
                startType: GetBootFlag(options.GetServiceStartType().Value), 
                binaryPath: options.BinaryPath, runAsFid: options.FunctionalId,
                fidPassword: fidPassword);
        }


        private ServiceBootFlag GetBootFlag(ServiceStartTypeEnum startType)
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
