using System;
using BraviApi.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraviApi.Exceptions
{
    public class ContactNotFoundException : AppException
    {
        public ContactNotFoundException() : base("Contact not found")
        {
            StatusCode = 404;
        }

    }
}
