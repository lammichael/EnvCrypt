namespace EnvCrypt.Core.Verb.DecryptEntry.Audit
{
    public class ToFileAuditLoggerConfig
    {
        public string LogFileExtension = ".log";
        /// <summary>
        /// {0} = Date & time stamp
        /// {1} = EXE name.
        /// </summary>
        public string FileNameFormat = @"{0}.{1}";

        public string LogDirectory;

        public int NumberOfDaysSinceCreationToKeep = 365;
    }
}
