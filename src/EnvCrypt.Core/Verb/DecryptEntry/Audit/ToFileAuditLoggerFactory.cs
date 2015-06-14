using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO;
using EnvCrypt.Core.Verb.DecryptEntry.PlainText;

namespace EnvCrypt.Core.Verb.DecryptEntry.Audit
{
    public static class ToFileAuditLoggerFactory
    {
        public static IAuditLogger<TKey, TWorkflowOptions> GetToFileAuditLogger<TKey, TWorkflowOptions>(ToFileAuditLoggerConfig config)
            where TKey : KeyBase
            where TWorkflowOptions : DecryptPlainTextEntryWorkflowOptions
        {
            Contract.Requires<ArgumentNullException>(config != null, "config");
            //
            var myDir = new MyDirectory();
            var myFile = new MyFile();

            return new ToFileAuditLogger<TKey, TWorkflowOptions>(config, myDir, myFile, new MyDateTime(),
                new OldLogCleaner(new ToFileAuditLoggerConfig(), myDir, myFile, new MyFileInfoFactory()));
        }
    }
}
