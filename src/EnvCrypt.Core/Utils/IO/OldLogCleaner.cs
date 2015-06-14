using System;
using System.Diagnostics.Contracts;
using System.Linq;
using EnvCrypt.Core.Verb.DecryptEntry.Audit;

namespace EnvCrypt.Core.Utils.IO
{
    public class OldLogCleaner : IOldLogCleaner
    {
        private readonly ToFileAuditLoggerConfig _config;
        private readonly IMyDirectory _myDirectory;
        private readonly IMyFile _myFile;
        private readonly IMyFileInfoFactory _myFileInfoFactory;

        public OldLogCleaner(
            ToFileAuditLoggerConfig config,
            IMyDirectory myDirectory,
            IMyFile myFile,
            IMyFileInfoFactory myFileInfoFactory)
        {
            Contract.Requires<ArgumentNullException>(config != null, "options");
            Contract.Requires<ArgumentNullException>(myDirectory != null, "myDirectory");
            Contract.Requires<ArgumentNullException>(myFile != null, "myFile");
            Contract.Requires<ArgumentNullException>(myFileInfoFactory != null, "myFileInfoFactory");
            //
            _config = config;
            _myDirectory = myDirectory;
            _myFile = myFile;
            _myFileInfoFactory = myFileInfoFactory;
        }


        public void Run()
        {
            string[] files;
            try
            {
                files = _myDirectory.GetFiles(_config.LogDirectory);
            }
            catch
            {
                return;
            }

            files = files.Where(s => s.EndsWith(_config.LogFileExtension)).ToArray();

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
