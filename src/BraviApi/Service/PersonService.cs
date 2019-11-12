using System;
using BraviApi.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using BraviApi.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using BraviApi.Repository;
using BraviApi.Dto;

namespace BraviApi.Service
{
    public class PersonService : IPersonService
    {
        public IPersonRepository PersonRepository { get; }

        public PersonService(IPersonRepository personRepository)
        {
            this.PersonRepository = personRepository;
        }

        public async Task<Person> Add(PersonDto data)
        {

            if (await PersonRepository.FindByNameAndBirthDate(data.Name, data.BirthDate) != null)
            {
                throw new PersonAlreadyExistsException();
            }
            var newPerson = new Person()
            {
                Name = data.Name,
                BirthDate = data.BirthDate,
            };
            await PersonRepository.Add(newPerson);
            return newPerson;
        }
        public async Task Update(PersonDto data)
        {
            var existingPerson = await PersonRepository.FindByNameAndBirthDate(data.Name, data.BirthDate);
            if (existingPerson != null && existingPerson.Id != data.Id)
            {
                throw new PersonAlreadyExistsException();
            }
            var existing = await PersonRepository.FindById(data.Id);
            existing.Name = data.Name;
            existing.BirthDate = data.BirthDate;
            await PersonRepository.Update(existing);
        }
        public async Task Delete(Guid id)
        {
            var originalPerson = await PersonRepository.FindById(id);
            if (originalPerson == null)
            {
                throw new PersonNotFoundException();
            }
            await PersonRepository.Delete(originalPerson);
        }
        public async Task<List<PersonDto>> FindAll()
        {
            return (await PersonRepository.FindAll())
                .Select(x => PersonDto.FromPerson(x))
                .ToList();
        }
        public async Task<PersonDto> FindById(Guid id)
        {
            var entity = await PersonRepository.FindById(id);

            return PersonDto.FromPerson(entity);
        }
    }
}
