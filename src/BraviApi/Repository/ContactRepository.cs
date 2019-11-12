using System;
using BraviApi.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using BraviApi.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BraviApi.Repository
{
    public class ContactRepository : IContactRepository
    {
        public BraviApiDbContext DbContext { get; set; }

        public ContactRepository(BraviApiDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public async Task Add(Contact data)
        {
            data.Id = Guid.NewGuid();
            await DbContext.Contacts.AddAsync(data);
            await DbContext.SaveChangesAsync();
        }
        public async Task Update(Contact data)
        {
            DbContext.Entry(data).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
        }
        public async Task Delete(Contact Contact)
        {
            DbContext.Entry(Contact).State = EntityState.Deleted;
            await DbContext.SaveChangesAsync();
        }
        public async Task<Contact> FindById(Guid id)
        {
            return await DbContext.Contacts.FindAsync(id);
        }

        public async Task<List<Contact>> FindAllByPerson(Guid personId)
        {
            return await DbContext
                .Contacts
                .Where(x => x.PersonId == personId)
                .ToListAsync();
        }
    }
}
