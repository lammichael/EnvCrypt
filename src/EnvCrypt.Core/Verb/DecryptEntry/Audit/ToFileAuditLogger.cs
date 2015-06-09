using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using EnvCrypt.Core.EncrypedData.Poco;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO;
using EnvCrypt.Core.Verb.DecryptEntry.PlainText;

namespace EnvCrypt.Core.Verb.DecryptEntry.Audit
{
    public class ToFileAuditLogger<TKey, TWorkflowOptions> : IAuditLogger<TKey, TWorkflowOptions> 
        where TKey : KeyBase
        where TWorkflowOptions : DecryptPlainTextEntryWorkflowOptions
    {
        private readonly ToFileAuditLoggerConfig _config;
        private readonly IMyDirectory _myDirectory;
        private readonly IMyFile _myFile;
        private readonly IMyDateTime _myDateTime;
        private readonly IMyFileInfoFactory _myFileInfoFactory;

        public ToFileAuditLogger(ToFileAuditLoggerConfig config, IMyDirectory myDirectory, IMyFile myFile, IMyDateTime myDateTime, IMyFileInfoFactory myFileInfoFactory)
        {
            Contract.Requires<ArgumentNullException>(config != null, "options");
            Contract.Requires<ArgumentNullException>(myDirectory != null, "myDirectory");
            Contract.Requires<ArgumentNullException>(myFile != null, "myFile");
            Contract.Requires<ArgumentNullException>(myDateTime != null, "myDateTime");
            Contract.Requires<ArgumentNullException>(myFileInfoFactory != null, "myFileInfoFactory");
            Contract.Requires<EnvCryptException>(config.NumberOfDaysSinceCreationToKeep >= 1, "number of days to keep audit log files must be >= 1");
            Contract.Requires<EnvCryptException>(!string.IsNullOrWhiteSpace(config.FileNameFormat), "filename format cannot be empty");
            Contract.Requires<EnvCryptException>(!string.IsNullOrWhiteSpace(config.LogDirectory), "log directory cannot be empty");
            //
            _config = config;
            _myDirectory = myDirectory;
            _myFile = myFile;
            _myDateTime = myDateTime;
            _myFileInfoFactory = myFileInfoFactory;
        }


        public void LogDecryption(TWorkflowOptions withWorkflowOptions, EnvCryptDat ecDat, IList<TKey> usingLoadedKeys,
            IList<EntriesDecrypterResult<TKey>> results)
        {
            try
            {
                _myDirectory.CreateDirectory(_config.LogDirectory);
            }
            catch
            {
                return;
            }

            var fileName = string.Format(_config.FileNameFormat,
                _myDateTime.UtcNow().ToString("O"),
                Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName)) +
                           _config.LogFileExtension;

            var content = string.Format("EC DDAT file: {1}{0}Entries decrypted: {2}",
                Environment.NewLine, withWorkflowOptions.DatFilePath,
                string.Join("  ",
                    results.Select(r => string.Join(":", r.CategoryEntryPair.Category, r.CategoryEntryPair.Entry))));

            try
            {
                var finalLogFilePath = Path.Combine(_config.LogDirectory, fileName);
                _myFile.WriteAllText(finalLogFilePath, content);
            }
            catch
            {
                return;
            }

            // Cleanup of old files
            string[] files;
            try
            {
                files = _myDirectory.GetFiles(_config.LogDirectory);
            }
            catch
            {
                return;
            }

            for (int fI = 0; fI < files.Length; fI++)
            {
                var fileInfo = _myFileInfoFactory.GetNewInstance(files[fI]);
                if (fileInfo.CreationTimeUtc < DateTime.Now.AddDays(-_config.NumberOfDaysSinceCreationToKeep))
                {
                    try
                    {
                        _myFile.Delete(files[fI]);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}