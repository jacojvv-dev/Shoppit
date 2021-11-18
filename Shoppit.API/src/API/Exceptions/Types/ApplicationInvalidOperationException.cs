using System;

namespace API.Exceptions.Types
{
    public class ApplicationInvalidOperationException : Exception
    {
        public ApplicationInvalidOperationException(string message) : base(message)
        {

        }
    }
}
