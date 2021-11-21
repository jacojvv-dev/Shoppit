using System;

namespace ApplicationCore.Exceptions
{
    public class ApplicationInvalidOperationException : Exception
    {
        public ApplicationInvalidOperationException(string message) : base(message)
        {

        }
    }
}
