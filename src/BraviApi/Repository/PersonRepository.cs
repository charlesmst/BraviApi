using System;
using BraviApi.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using BraviApi.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BraviApi.Repository
{
    public class PersonRepository : IPersonRepository
    {
        public BraviApiDbContext DbContext { get; set; }

        public PersonRepository(BraviApiDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public async Task Add(Person data)
        {
            data.Id = Guid.NewGuid();

            if (await DbContext.People.Where(x => x.Name == data.Name && x.BirthDate == data.BirthDate).AnyAsync())
            {
                throw new PersonAlreadyExistsException();
            }
            await DbContext.People.AddAsync(data);
            await DbContext.SaveChangesAsync();
        }
        public async Task Update(Person data)
        {
            var originalPerson = await DbContext.People.FindAsync(data.Id);
            if (originalPerson == null)
            {
                throw new PersonNotFoundException();
            }
            originalPerson.Name = data.Name;
            originalPerson.BirthDate = data.BirthDate;

            await DbContext.SaveChangesAsync();
        }
        public async Task Delete(Guid id)
        {
            var originalPerson = await DbContext.People.FindAsync(id);
            if (originalPerson == null)
            {
                throw new PersonNotFoundException();
            }
            DbContext.Entry(originalPerson).State = EntityState.Deleted;
            await DbContext.SaveChangesAsync();
        }
        public async Task<List<Person>> FindAll()
        {
            return await DbContext.People.ToListAsync();
        }
        public async Task<Person> FindById(Guid id)
        {
            return await DbContext.People.FindAsync(id);
        }
    }
}
