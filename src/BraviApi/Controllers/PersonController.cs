using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BraviApi.Dto;
using BraviApi.Entity;
using BraviApi.Exceptions;
using BraviApi.Repository;
using Microsoft.AspNetCore.Mvc;
using BraviApi.Filters;
namespace BraviApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PersonController : ControllerBase
    {
        public IPersonRepository Repository { get; }

        public PersonController(IPersonRepository repository)
        {
            this.Repository = repository;
        }
        [HttpGet]

        public async Task<ActionResult<IEnumerable<PersonDto>>> Get()
        {
            var data = await Repository.FindAll();
            return Ok(
                data.Select(x => new PersonDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    BirthDate = x.BirthDate
                }).ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonDto>> Get(Guid id)
        {
            var entity = await Repository.FindById(id);
            return Ok(new PersonDto()
            {
                Id = entity.Id,
                Name = entity.Name,
                BirthDate = entity.BirthDate
            });
        }

        [HttpPost]
        public async Task<ActionResult<CrudOperationDto>> Post([FromBody] PersonDto value)
        {
            var entity = new Person()
            {
                Name = value.Name,
                BirthDate = value.BirthDate
            };


            await Repository.Add(entity);
            return Ok(new CrudOperationDto()
            {
                Success = true,
                Id = entity.Id
            });



        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CrudOperationDto>> Put(Guid id, [FromBody] PersonDto value)
        {
            var entity = await Repository.FindById(id);
            entity.Name = value.Name;
            entity.BirthDate = value.BirthDate;
            await Repository.Update(entity);
            return Ok(new CrudOperationDto()
            {
                Success = true,
                Id = entity.Id
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CrudOperationDto>>  Delete(Guid id)
        {
            await Repository.Delete(id);
            return Ok(new CrudOperationDto()
            {
                Success = true,
                Id = id
            });
        }
    }
}
