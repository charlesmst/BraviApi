using System;
using BraviApi.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace BraviApi.Exceptions
{
    public class AppException : Exception
    {
        public virtual int StatusCode { get; set; } = 500;
        public AppException()
        {
        }

        public AppException(string message) : base(message)
        {
        }

        public AppException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AppException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
