using System.Text;

namespace EnvCrypt.Core.Utils.IO.StringWriter
{
    public class StringWriterOptions
    {
        public string Contents { get; set; }
        public Encoding Encoding { get; set; }


        protected bool Equals(StringWriterOptions other)
        {
            return string.Equals(Contents, other.Contents) && Equals(Encoding, other.Encoding);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((StringWriterOptions) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Contents != null ? Contents.GetHashCode() : 0)*397) ^ (Encoding != null ? Encoding.GetHashCode() : 0);
            }
        }

        public static bool operator ==(StringWriterOptions left, StringWriterOptions right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(StringWriterOptions left, StringWriterOptions right)
        {
            return !Equals(left, right);
        }
    }
}
