using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvCrypt.Core.EncryptionAlgo.Aes.Key;

namespace EnvCrypt.Core.Key.Xml
{
    class AesKeyToXmlMapper : IKeyToExternalRepresentationMapper<AesKey, Key.Xml.EnvCryptKey>
    {
        public void Map(AesKey fromPoco, Key.Xml.EnvCryptKey toExternalRepresentationPoco)
        {
            
        }
    }
}
