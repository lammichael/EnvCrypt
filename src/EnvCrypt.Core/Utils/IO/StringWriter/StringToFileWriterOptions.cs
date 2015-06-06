namespace EnvCrypt.Core.Utils.IO.StringWriter
{
    public class StringToFileWriterOptions : StringWriterOptions
    {
        public string Path { get; set; }
        public bool OverwriteIfFileExists { get; set; }


        protected bool Equals(StringToFileWriterOptions other)
        {
            return base.Equals(other) && string.Equals(Path, other.Path) && OverwriteIfFileExists.Equals(other.OverwriteIfFileExists);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((StringToFileWriterOptions) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ (Path != null ? Path.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ OverwriteIfFileExists.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(StringToFileWriterOptions left, StringToFileWriterOptions right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(StringToFileWriterOptions left, StringToFileWriterOptions right)
        {
            return !Equals(left, right);
        }
    }
}