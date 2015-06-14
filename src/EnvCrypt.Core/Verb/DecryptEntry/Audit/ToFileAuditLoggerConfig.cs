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

        /// <summary>
        /// If the log file to write to already exists then a number will be added to
        /// the end of the file. This defines the maximum number of tries for this.
        /// </summary>
        public int MaxTriesToGetUniqueFileName = 50;

        public string LogDirectory;

        public int NumberOfDaysSinceCreationToKeep = 365;
    }
}
