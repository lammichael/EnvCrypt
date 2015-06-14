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
    /// <summary>
    /// Writes a log file containing information about what process requested
    /// for decryption. Not immune to race conditions causing overwriting
    /// of existing log files, but it makes an attempt to create a unique file name to write to each time.
    /// </summary>
    public class ToFileAuditLogger<TKey, TWorkflowOptions> : IAuditLogger<TKey, TWorkflowOptions> 
        where TKey : KeyBase
        where TWorkflowOptions : DecryptPlainTextEntryWorkflowOptions
    {
        public const string DateTimeFormatInFileName = @"yyyy-MM-dd.HH.mm.ss";

        private readonly ToFileAuditLoggerConfig _config;
        private readonly IMyDirectory _myDirectory;
        private readonly IMyFile _myFile;
        private readonly IMyDateTime _myDateTime;
        private readonly IOldLogCleaner _oldLogCleaner;

        public ToFileAuditLogger(ToFileAuditLoggerConfig config, IMyDirectory myDirectory, IMyFile myFile, IMyDateTime myDateTime, IOldLogCleaner oldLogCleaner)
        {
            Contract.Requires<ArgumentNullException>(config != null, "options");
            Contract.Requires<ArgumentNullException>(myDirectory != null, "myDirectory");
            Contract.Requires<ArgumentNullException>(myFile != null, "myFile");
            Contract.Requires<ArgumentNullException>(myDateTime != null, "myDateTime");
            Contract.Requires<ArgumentNullException>(oldLogCleaner != null, "oldLogCleaner");
            Contract.Requires<EnvCryptException>(config.NumberOfDaysSinceCreationToKeep >= 1, "number of days to keep audit log files must be >= 1");
            Contract.Requires<EnvCryptException>(!string.IsNullOrWhiteSpace(config.FileNameFormat), "filename format cannot be empty");
            Contract.Requires<EnvCryptException>(!string.IsNullOrWhiteSpace(config.LogDirectory), "log directory cannot be empty");
            //
            _config = config;
            _myDirectory = myDirectory;
            _myFile = myFile;
            _myDateTime = myDateTime;
            _oldLogCleaner = oldLogCleaner;
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
                _myDateTime.UtcNow().ToString(DateTimeFormatInFileName),
                Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName));

            var logFilePathWithoutUidOrExt = Path.Combine(_config.LogDirectory, fileName);
            var logFilePathWithoutUid = logFilePathWithoutUidOrExt + _config.LogFileExtension;
            string finalLogPath = null;

            /*
             * If original file exists then try to create a unique one.
             */
            if (_myFile.Exists(logFilePathWithoutUid))
            {
                if (_config.MaxTriesToGetUniqueFileName <= 0)
                {
                    return;
                }

                var foundUniqueFileName = false;
                for (int uid = 0; uid < _config.MaxTriesToGetUniqueFileName; uid++)
                {
                    finalLogPath = string.Format("{0}-{1}{2}", logFilePathWithoutUidOrExt, uid, _config.LogFileExtension);

                    if (!_myFile.Exists(finalLogPath))
                    {
                        foundUniqueFileName = true;
                        break;
                    }
                }
                if (!foundUniqueFileName)
                {
                    return;
                }
            }
            else
            {
                finalLogPath = logFilePathWithoutUid;
            }

            Contract.Assert(finalLogPath != null, "a potentially unique final log path must be found at this point");
            var content = GetLogContent(withWorkflowOptions, results);
            try
            {
                _myFile.WriteAllText(finalLogPath, content);
            }
            catch
            {
                return;
            }

            
            _oldLogCleaner.Run();
        }


        private static string GetLogContent(TWorkflowOptions withWorkflowOptions, IList<EntriesDecrypterResult<TKey>> results)
        {
            var content = string.Format("EC DDAT file:{0}{1}{0}Category\tEntry\tKey Name & Type{0}{2}",
                Environment.NewLine, withWorkflowOptions.DatFilePath,
                string.Join(Environment.NewLine,
                    results.Select(
                        r =>
                            string.Join("\t", r.CategoryEntryPair.Category, r.CategoryEntryPair.Entry, r.DecryptedUsingKey.Name,
                                r.DecryptedUsingKey.Algorithm))));
            return content;
        }
    }
}