using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvCrypt.Core.Verb.AddEntry
{
    class AddEntryWorkflow
    {

        public void Run()
        {
            /*
             * Get Entries from file or create a new file if not already there
             * Get key from file
             * Map keys to entries
             * Encrypt value using key (or leave in plain text)
             * Add encrypted value to POCO
             * Write POCO to file
             */
        }
    }
}
