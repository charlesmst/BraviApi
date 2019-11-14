using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BraviApi.Entity;

namespace BraviApi.Dto
{
    public class PersonDto
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(500)]
        public string Name { get; set; }
        public IList<ContactDto> Contacts { get; set; }
        public static PersonDto FromPerson(Person person)
        {
            if (person == null)
            {
                return null;
            }
            return new PersonDto()
            {
                Id = person.Id,
                Name = person.Name,
                Contacts = person.Contacts?.Select(x => ContactDto.FromContact(x)).ToList()
            };
        }
    }
}
