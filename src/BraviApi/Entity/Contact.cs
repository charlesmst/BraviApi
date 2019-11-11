using System;

namespace BraviApi.Entity
{
    public class Contact
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public Person Person { get; set; }
        public ICollection<ContactDto> Contacts { get; set; }


    }
}
