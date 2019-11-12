using System;
using BraviApi.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc.Filters;
using BraviApi.Exceptions;
using Microsoft.AspNetCore.Mvc;
using BraviApi.Dto;
using Microsoft.AspNetCore.Hosting;

namespace BraviApi.Filters
{

    public class HttpResponseExceptionFilter : IActionFilter
    {
        public IHostingEnvironment Environment { get; }

        public HttpResponseExceptionFilter(IHostingEnvironment env)
        {
            Environment = env;
        }

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                if (context.Exception is AppException exception)
                {
                    context.Result = new ObjectResult(new CrudOperationDto()
                    {
                        Success = false,
                        Error = exception.Message
                    })
                    {
                        StatusCode = exception.StatusCode
                    };
                    context.ExceptionHandled = true;
                }
                else
                {
                    context.Result = new ObjectResult(new CrudOperationDto()
                    {
                        Success = false,
                        Error = Environment.IsDevelopment() ? context.Exception.Message : "An error has occured"
                    })
                    {
                        StatusCode = 500
                    };

                    System.Diagnostics.Trace.TraceError("Filtered error: " + context.Exception.Message);
                    context.ExceptionHandled = true;
                }
            }
        }
    }
}
