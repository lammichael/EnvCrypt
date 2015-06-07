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
        private readonly ToFileAuditLoggerConfig _options;
        private readonly IMyDirectory _myDirectory;
        private readonly IMyFile _myFile;
        private readonly IMyDateTime _myDateTime;
        private readonly IMyFileInfoFactory _myFileInfoFactory;

        public ToFileAuditLogger(ToFileAuditLoggerConfig options, IMyDirectory myDirectory, IMyFile myFile, IMyDateTime myDateTime, IMyFileInfoFactory myFileInfoFactory)
        {
            Contract.Requires<ArgumentNullException>(options != null, "options");
            Contract.Requires<ArgumentNullException>(myDirectory != null, "myDirectory");
            Contract.Requires<ArgumentNullException>(myFile != null, "myFile");
            Contract.Requires<ArgumentNullException>(myFileInfoFactory != null, "myFileInfoFactory");
            //
            _options = options;
            _myDirectory = myDirectory;
            _myFile = myFile;
            _myDateTime = myDateTime;
            _myFileInfoFactory = myFileInfoFactory;
        }


        public void LogDecryption(TWorkflowOptions withWorkflowOptions, EnvCryptDat ecDat, IList<TKey> usingLoadedKeys, IList<EntriesDecrypterResult<TKey>> results)
        {
            _myDirectory.CreateDirectory(_options.LogDirectory);
            
            var fileName = string.Format(_options.FileNameFormat,
                _myDateTime.UtcNow().ToString("O"), Process.GetCurrentProcess().MainModule.FileName) + _options.LogFileExtension;

            var content = string.Format("EC DDAT file: {1}{0}Entries decrypted: {2}",
                Environment.NewLine, withWorkflowOptions.DatFilePath,
                string.Join("  ",
                    results.Select(r => string.Join(":", r.CategoryEntryPair.Category, r.CategoryEntryPair.Entry))));

            _myFile.WriteAllText(Path.Combine(_options.LogDirectory, fileName), content);


            // Cleanup of old files
            string[] files;
            try
            {
                files = _myDirectory.GetFiles(_options.LogDirectory);
            }
            catch
            {
                return;
            }

            for (int fI = 0; fI < files.Length; fI++)
            {
                var fileInfo = _myFileInfoFactory.GetNewInstance(files[fI]);
                if (fileInfo.CreationTimeUtc < DateTime.Now.AddDays(-_options.NumberOfDaysSinceCreationToKeep))
                {
                    try
                    {
                        fileInfo.Delete();
                    }
                    catch
                    { }
                }
            }
        }
    }
}