using Microsoft.AspNetCore.Mvc;
using PubDev.UnitTests.API.Enums;
using PubDev.UnitTests.API.Interfaces.Presenters;
using PubDev.UnitTests.API.Responses;
using System.Diagnostics;

namespace PubDev.UnitTests.API.Presenters;

public class Presenter : IPresenter
{
    private readonly NotificationContext _notificationContext;

    public Presenter(NotificationContext notificationContext)
    {
        _notificationContext = notificationContext;
    }

    public IActionResult GetResult<TResult, TResponse>(TResult result, Func<TResult, TResponse> responseFunc)
        where TResult : class
        where TResponse : class
    {
        if (_notificationContext.IsValid)
        {
            return new OkObjectResult(responseFunc(result));
        }

        return GetInvalidObjectResult();
    }

    public IActionResult AcceptedResult<TResult, TResponse>(
        TResult result,
        Func<TResult, TResponse> responseFunc,
        Func<TResult, (string actionName, string controller, object routeValues)> locationFunc)
        where TResult : class
        where TResponse : class
    {
        if (_notificationContext.IsValid)
        {
            var (actionName, controller, routeValues) = locationFunc(result);

            return new AcceptedAtActionResult(actionName, controller, routeValues, responseFunc(result));
        }

        return GetInvalidObjectResult();
    }

    public IActionResult CreateResult<TResult, TResponse>(
        TResult result,
        Func<TResult, TResponse> responseFunc,
        Func<TResult, (string actionName, string controller, object routeValues)> locationFunc)
        where TResult : class
        where TResponse : class
    {
        if (_notificationContext.IsValid)
        {
            var (actionName, controller, routeValues) = locationFunc(result);

            return new CreatedAtActionResult(actionName, controller, routeValues, responseFunc(result));
        }

        return GetInvalidObjectResult();
    }

    private IActionResult GetInvalidObjectResult()
    {
        var rootId = Guid.Parse(Activity.Current.RootId);

        var errorResponse = new ErrorResponse(rootId, _notificationContext.ErrorMessages.Select(message => (ErrorMessageResponse)message));

        if (_notificationContext.ErrorMessages.Any(x => x.ErrorType == ErrorType.NotFound))
        {
            return new NotFoundObjectResult(errorResponse);
        }
        else
        {
            return new BadRequestObjectResult(errorResponse);
        }
    }
}