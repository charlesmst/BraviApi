using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BraviApi.Entity;

namespace BraviApi.Dto
{
    public class PersonDto
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(500)]
        public string Name { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        public static PersonDto FromPerson(Person person)
        {
            return new PersonDto()
            {
                Id = person.Id,
                Name = person.Name,
                BirthDate = person.BirthDate
            };
        }
    }
}
