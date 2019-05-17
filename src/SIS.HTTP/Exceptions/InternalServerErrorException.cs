using System;

namespace SIS.HTTP.Exceptions
{
    public class InternalServerErrorException : Exception
    {
        private const string InternalserverErrorMessage = "The Server has encountered an error.";

        public InternalServerErrorException() : this(InternalserverErrorMessage)
        {

        }

        public InternalServerErrorException(string name) : base(name)
        {

        }
    }

}
