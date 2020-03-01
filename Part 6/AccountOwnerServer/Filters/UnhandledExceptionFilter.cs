using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountOwnerServer.Filters
{

    //https://stackoverflow.com/questions/48624123/asp-net-core-2-error-handling-how-to-return-formatted-exception-details-in-http

    public class UnhandledExceptionFilter : ActionFilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            //Write to application log here

            context.Result = new BadRequestObjectResult(context.Exception.Message);
        }
    }
}
