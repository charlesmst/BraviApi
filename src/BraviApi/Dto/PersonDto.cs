using System;
using System.Collections.Generic;

namespace BraviApi.Dto
{
    public class PersonDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
        public List<ContactDto> Contacts { get; set; }

    }
}
