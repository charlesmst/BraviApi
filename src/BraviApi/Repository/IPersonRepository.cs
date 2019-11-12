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
        Task Add(Person data);
        Task<Person> FindByNameAndBirthDate(string name, DateTime birthDate);
        Task Delete(Person entity);
        Task<List<Person>> FindAll();
        Task<Person> FindById(Guid id);
        Task Update(Person data);
    }
}
