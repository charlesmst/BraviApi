using System;
using BraviApi.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using BraviApi.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BraviApi.Repository
{    
    public interface IPersonRepository
    {
        BraviApiDbContext DbContext { get; set; }

        Task Add(Person data);
        Task Delete(Guid id);
        Task<List<Person>> FindAll();
        Task<Person> FindById(Guid id);
        Task Update(Person data);
    }
}
