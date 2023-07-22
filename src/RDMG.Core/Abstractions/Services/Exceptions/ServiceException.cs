using System;
using System.Runtime.Serialization;

namespace RDMG.Core.Abstractions.Services.Exceptions;

[Serializable]
public class ServiceException : Exception
{
    public const string GeneralError = "GeneralError";
    public const string GeneralAggregateError = "GeneralAggregateError";
    
    public ServiceException() : base(GeneralError)
    {
    }

    public ServiceException(string message)
        : base(message)
    {
    }

    protected ServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}