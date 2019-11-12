using System;
using BraviApi.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using BraviApi.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using BraviApi.Dto;

namespace BraviApi.Service
{
    public interface IPersonService
    {

        Task<Person> Add(PersonDto data);
        Task Delete(Guid id);
        Task<List<PersonDto>> FindAll();
        Task<PersonDto> FindById(Guid id);
        Task Update(PersonDto data);
    }
}
