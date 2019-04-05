using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using OnlineAuction.BLL.Exceptions;

namespace OnlineAuction.API.Filters
{
    public class CatchExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            HttpStatusCode code = HttpStatusCode.InternalServerError;
            if (context.Exception is NotFoundException)
            {
                code = HttpStatusCode.NotFound;
            }
            if (context.Exception is ArgumentException || context.Exception is ValidationException)
            {
                code = HttpStatusCode.BadRequest;
            }
            context.Response = context.Request.CreateErrorResponse(code, context.Exception.Message);
        }
    }
}