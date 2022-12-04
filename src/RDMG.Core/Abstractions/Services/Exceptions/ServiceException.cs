using System;

namespace RDMG.Core.Abstractions.Services.Exceptions;

public class ServiceException : Exception
{
    public ServiceException(string message) : base(message)
    {
    }
}