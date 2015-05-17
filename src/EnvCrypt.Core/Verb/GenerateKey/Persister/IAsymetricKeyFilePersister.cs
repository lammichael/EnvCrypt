﻿using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.Verb.GenerateKey.Persister
{
    interface IAsymetricKeyFilePersister<in TKeyPoco, TKeyXmlPoco> : IKeyFilePersister<TKeyPoco, TKeyXmlPoco, AsymmetricKeyFilePersisterOptions> 
        where TKeyPoco : KeyBase, IAsymmetricKeyMarker
        where TKeyXmlPoco : IKeyExternalRepresentation<TKeyPoco>
    {}
}