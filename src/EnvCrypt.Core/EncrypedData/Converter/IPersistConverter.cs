namespace EnvCrypt.Core.EncrypedData.Converter
{
    /// <summary>
    /// EnvCrypt persists data as String.
    /// </summary>
    interface IPersistConverter
    {
        string Encode(byte[] dataToPersist);
        byte[] Decode(string persistedStr);
    }
}
