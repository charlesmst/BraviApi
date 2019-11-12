using System;
using BraviApi.Entity;

namespace BraviApi.Dto
{
    public class ContactDto
    {
        public Guid PersonId { get; set; }
        public ContactType Type { get; set; }
        public string Value { get; set; }

    }
}
