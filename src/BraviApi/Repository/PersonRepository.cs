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
            await DbContext.People.AddAsync(data);
            await DbContext.SaveChangesAsync();
        }
        public async Task Update(Person data)
        {
            DbContext.Entry(data).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
        }
        public async Task Delete(Person person)
        {
            DbContext.Entry(person).State = EntityState.Deleted;
            await DbContext.SaveChangesAsync();
        }
        public async Task<List<Person>> FindAll()
        {
            return await DbContext.People
                .Include(x => x.Contacts)
                .ToListAsync();
        }
        public async Task<Person> FindById(Guid id)
        {
            return await DbContext.People.FindAsync(id);
        }

        public async Task<Person> FindByName(string name)
        {
            return await DbContext.People.Where(x => x.Name == name).FirstOrDefaultAsync();
        }
    }
}
