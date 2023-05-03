using System;

namespace X_Guide
{
    internal class CriticalErrorException : Exception
    {
        public CriticalErrorException(string message) : base(message)
        {
        }
    }
}