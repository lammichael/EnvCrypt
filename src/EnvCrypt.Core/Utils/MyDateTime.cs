using System;

namespace EnvCrypt.Core.Utils
{
    public class MyDateTime : IMyDateTime
    {
        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }

        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}
