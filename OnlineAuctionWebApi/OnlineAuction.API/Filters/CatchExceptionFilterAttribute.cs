using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using OnlineAuction.BLL.Exceptions;

namespace OnlineAuction.API.Filters
{
    /// <summary>
    /// Filter that catches unhandled exceptions.
    /// </summary>
    public class CatchExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// Returns HTTP response code on exceptions.
        /// For NotFoundException - 404, for ArgumentException - 400, for ValidationException - 400, other - 500.
        /// </summary>
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