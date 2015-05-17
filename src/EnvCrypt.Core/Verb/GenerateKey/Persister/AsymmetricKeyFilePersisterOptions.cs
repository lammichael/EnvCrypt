namespace EnvCrypt.Core.Verb.GenerateKey.Persister
{
    class AsymmetricKeyFilePersisterOptions : KeyFilePersisterOptions
    {
        public string NewPrivateKeyFileFullPath { get; set; }
        public string NewPublicKeyFileFullPath { get; set; }
    }
}
