using System;
using BraviApi.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using BraviApi.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BraviApi.Repository
{
    public interface IContactRepository
    {
        Task Add(Contact data);
        Task Update(Contact data);
        Task Delete(Contact entity);
        Task<Contact> FindById(Guid id);
        Task<List<Contact>> FindAllByPerson(Guid personId);
    }
}
