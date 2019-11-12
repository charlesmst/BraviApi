using System;
using BraviApi.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraviApi.Exceptions
{
    public class PersonAlreadyExistsException : AppException
    {
        public PersonAlreadyExistsException() : base("Person already exist")
        {

        }
    }
}
