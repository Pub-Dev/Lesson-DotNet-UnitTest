using Microsoft.AspNetCore.Mvc;

namespace PubDev.UnitTests.API.Interfaces.Presenters;

public interface IPresenter
{
    IActionResult GetResult<TResult, TResponse>(TResult result, Func<TResult, TResponse> responseFunc)
        where TResult : class
        where TResponse : class;

    IActionResult AcceptedResult<TResult, TResponse>(
        TResult result,
        Func<TResult, TResponse> responseFunc,
        Func<TResult, (string actionName, string controller, object routeValues)> locationFunc)
        where TResult : class
        where TResponse : class;

    public IActionResult CreateResult<TResult, TResponse>(
         TResult result,
         Func<TResult, TResponse> responseFunc,
         Func<TResult, (string actionName, string controller, object routeValues)> locationFunc)
         where TResult : class
         where TResponse : class;
}
