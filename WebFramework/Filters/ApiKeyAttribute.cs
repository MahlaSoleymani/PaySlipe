using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Service.UserServices;

namespace WebFramework.Filters
{
    //[AttributeUsage(validOn:AttributeTargets.Class)]
    public class ApiKeyAttribute : Attribute , IAsyncActionFilter
    {
        private const string APIKEYNAME = "ApiKey";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Api Key was not provided."
                };
                return;
            }

            var userService = context.HttpContext.RequestServices.GetService<IUserService>();

            // var result = userService!.CheckApiKey(extractedApiKey.ToString());
            //
            // if (result is false)
            // {
            //     context.Result = new ContentResult()
            //     {
            //         StatusCode = 401,
            //         Content = "Api Key is not valid."
            //     };
            //     return;
            // }

            await next();
        }
    }
}
