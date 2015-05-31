using EnvCrypt.Core.EncryptionAlgo;

namespace EnvCrypt.Core.Key.PlainText
{
    /// <summary>
    /// Key used when value is stored in plain text.
    /// All PlainText keys are the same and have the same hash code.
    /// </summary>
    class PlainTextKey : KeyBase
    {
        public override EnvCryptAlgoEnum Algorithm
        {
            get
            {
                return EnvCryptAlgoEnum.PlainText;
            }
        }


        protected bool Equals(PlainTextKey other)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PlainTextKey) obj);
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
