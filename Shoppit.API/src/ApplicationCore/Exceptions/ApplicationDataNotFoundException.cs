using System;

namespace ApplicationCore.Exceptions
{
    public class ApplicationDataNotFoundException : Exception
    {
        public ApplicationDataNotFoundException(string message) : base(message)
        {

        }
    }
}
