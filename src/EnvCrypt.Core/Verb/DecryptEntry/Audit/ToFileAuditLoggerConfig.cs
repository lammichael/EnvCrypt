namespace EnvCrypt.Core.Verb.DecryptEntry.Audit
{
    public class ToFileAuditLoggerConfig
    {
        /// <summary>
        /// {0} = Date & time stamp
        /// {1} = EXE name.
        /// </summary>
        public string FileNameFormat = @"{0}.{1}.log";
        public string LogDirectory { get; set; }
    }
}
