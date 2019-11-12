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
    public interface IContactService
    {
        Task<Contact> Add(ContactDto data);
        Task Update(ContactDto data);
        Task Delete(Guid id);
        Task<ContactDto> FindById(Guid id);
        Task<List<ContactDto>> FindAllByPerson(Guid personId);
    }
}
