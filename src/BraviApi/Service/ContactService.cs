using System;
using BraviApi.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using BraviApi.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using BraviApi.Repository;
using BraviApi.Dto;
using System.ComponentModel.DataAnnotations;

namespace BraviApi.Service
{
    public class ContactService : IContactService
    {
        public IContactRepository ContactRepository { get; }

        public ContactService(IContactRepository ContactRepository)
        {
            this.ContactRepository = ContactRepository;
        }

        public virtual void ValidateContact(ContactDto contact)
        {
            switch (contact.Type)
            {
                case ContactType.Email:
                    if (!new EmailAddressAttribute().IsValid(contact.Value))
                    {
                        throw new ContactInvalidValueException();
                    }
                    break;
                case ContactType.Phone:
                case ContactType.Whatsapp:
                    if (!new PhoneAttribute().IsValid(contact.Value))
                    {
                        throw new ContactInvalidValueException();
                    }
                    break;
            }

        }
        public async Task<Contact> Add(ContactDto data)
        {
            ValidateContact(data);
            var newContact = new Contact()
            {
                PersonId = data.PersonId,
                Value = data.Value,
                Type = data.Type,
            };
            await ContactRepository.Add(newContact);
            return newContact;
        }
        public async Task Update(ContactDto data)
        {
            ValidateContact(data);
            var existing = await ContactRepository.FindById(data.Id);
            existing.Type = data.Type;
            existing.Value = data.Value;
            await ContactRepository.Update(existing);
        }
        public async Task Delete(Guid id)
        {
            var originalContact = await ContactRepository.FindById(id);
            if (originalContact == null)
            {
                throw new ContactNotFoundException();
            }
            await ContactRepository.Delete(originalContact);
        }
        public async Task<ContactDto> FindById(Guid id)
        {
            var entity = await ContactRepository.FindById(id);
            return ContactDto.FromContact(entity);
        }


        public async Task<List<ContactDto>> FindAllByPerson(Guid personId)
        {
            return (await ContactRepository.FindAllByPerson(personId))
             .Select(x => ContactDto.FromContact(x))
             .ToList();
        }


    }
}
