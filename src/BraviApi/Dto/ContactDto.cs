using System;
using BraviApi.Entity;

namespace BraviApi.Dto
{
    public class ContactDto
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public ContactType Type { get; set; }
        public string Value { get; set; }

        internal static ContactDto FromContact(Contact entity)
        {
            if (entity == null)
            {
                return null;
            }
            return new ContactDto()
            {
                Id = entity.Id,
                PersonId = entity.PersonId,
                Type = entity.Type,
                Value = entity.Value
            };
        }
    }
}
