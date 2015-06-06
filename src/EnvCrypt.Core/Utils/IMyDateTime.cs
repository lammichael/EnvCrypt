using System;

namespace EnvCrypt.Core.Utils
{
    public interface IMyDateTime
    {
        DateTime UtcNow();
        DateTime Now();
    }
}