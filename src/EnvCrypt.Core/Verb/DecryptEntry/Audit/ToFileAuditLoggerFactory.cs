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
            return new ToFileAuditLogger<TKey, TWorkflowOptions>(config, new MyDirectory(), new MyFile(), new MyDateTime(), new MyFileInfoFactory());
        }
    }
}
