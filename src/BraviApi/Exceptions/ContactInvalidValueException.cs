using System;
using BraviApi.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraviApi.Exceptions
{
    public class ContactInvalidValueException : AppException
    {
        public ContactInvalidValueException() : base("Invalid value for contact")
        {

        }

    }
}
