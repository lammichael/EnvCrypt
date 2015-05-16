namespace EnvCrypt.Core
{
    /// <summary>
    /// Defines how EnvCrypt persists binary to a String for storage to file.
    /// </summary>
    interface IPersistConverter
    {
        string Encode(byte[] dataToPersist);
        byte[] Decode(string persistedStr);
    }
}
