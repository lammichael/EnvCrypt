using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvCrypt.Console.GenerateKey
{
    class GenerateKeyOptionsValidator
    {
        /// <summary>
        /// Writes the validation errors of the command line options to standard error,
        /// and returns if there were errors found or not.
        /// </summary>
        /// <param name="options">to validate</param>
        /// <returns>true if errors were found</returns>
        public bool HasErrorsAndReport(GenerateKeyVerbOptions options)
        {
            Contract.Requires<ArgumentNullException>(options != null, "options");
            //
            var hasErrors = false;
            if (options.GetAlgorithm() == null)
            {
                System.Console.Error.WriteLine("Unrecognised algorithm: {0}.", options.AlgorithmToUse);
                hasErrors = true;
            }
            if (string.IsNullOrWhiteSpace(options.KeyName))
            {
                System.Console.Error.WriteLine("Key name not defined.");
                hasErrors = true;
            }
            if (string.IsNullOrWhiteSpace(options.OutputDirectory))
            {
                System.Console.Error.WriteLine("Output directory not defined.");
                hasErrors = true;
            }

            return hasErrors;
        }
    }
}
