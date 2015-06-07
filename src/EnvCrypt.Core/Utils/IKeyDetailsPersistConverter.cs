namespace EnvCrypt.Core.Utils
{
    /// <summary>
    /// Used to map binary to text and back for the key.
    /// Currently all binaries for all keys are converted using Base64.
    /// </summary>
    interface IKeyDetailsPersistConverter
    {
        string Encode(byte[] dataToPersist);
        byte[] Decode(string persistedStr);
    }
}