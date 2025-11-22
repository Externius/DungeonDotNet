using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using RDMG.Core.Abstractions.Services.Exceptions;

namespace RDMG.Web.Extensions;

public static class ControllerExtensions
{
    public static void HandleException(this Controller controller, Exception ex, ILogger logger,
        string? defaultError = null)
    {
        switch (ex)
        {
            case ServiceAggregateException ae:
            {
                foreach (var exception in ae.GetInnerExceptions())
                {
                    controller.ModelState.AddModelError(string.Empty, exception.Message);
                }

                break;
            }
            case ServiceException e:
                controller.ModelState.AddModelError(string.Empty, e.Message);
                break;
            default:
                logger.LogError(ex, "{GetDisplayUrl}: {DefaultError}", controller.Request.GetDisplayUrl(),
                    defaultError ?? ServiceException.GeneralError);
                controller.ModelState.AddModelError(string.Empty, defaultError ?? ServiceException.GeneralError);
                break;
        }
    }
}