using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvCrypt.Core.EncryptionAlgo.Poco;

namespace EnvCrypt.Core.Key
{
    /// <summary>
    /// Marker for a POCO used to deserialize a key.
    /// </summary>
    interface IKeyExternalRepresentation<T> where T : KeyBase
    {}
}
