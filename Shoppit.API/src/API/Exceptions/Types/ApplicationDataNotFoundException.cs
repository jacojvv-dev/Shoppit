using System;

namespace API.Exceptions.Types
{
    public class ApplicationDataNotFoundException : Exception
    {
        public ApplicationDataNotFoundException(string message) : base(message)
        {

        }
    }
}
