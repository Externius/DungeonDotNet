namespace RDMG.Core.Abstractions.Services.Exceptions;

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
}