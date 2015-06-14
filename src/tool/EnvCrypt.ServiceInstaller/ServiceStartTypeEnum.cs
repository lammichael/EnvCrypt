namespace EnvCrypt.ServiceInstaller
{
    public enum ServiceStartTypeEnum
    {
        /// <summary>
        /// A service that cannot be started.
        /// </summary>
        Disabled,
        /// <summary>
        /// A service started automatically by the service control manager during system startup.
        /// </summary>
        AutoStart,
        /// <summary>
        /// A service started by the service control manager when a process calls the StartService function.
        /// </summary>
        DemandStart,
    }
}