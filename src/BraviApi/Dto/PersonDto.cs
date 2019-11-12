using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BraviApi.Dto
{
    public class PersonDto
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(500)]
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
