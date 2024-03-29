using System;
using BraviApi.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraviApi.Exceptions
{
    public class PersonNotFoundException : AppException
    {
        public PersonNotFoundException() : base("Person not found")
        {
            StatusCode = 404;
        }
    }
}
