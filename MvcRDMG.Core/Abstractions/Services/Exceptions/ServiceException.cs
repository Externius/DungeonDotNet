using System;

namespace MvcRDMG.Core.Abstractions.Services.Exceptions
{
    public class ServiceException : Exception
    {
        public ServiceException(string message) : base(message)
        {
        }
    }
}
