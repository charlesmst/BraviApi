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
using BraviApi.Service;

namespace BraviApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PersonController : ControllerBase
    {
        public IPersonService Service { get; }

        public PersonController(IPersonService service)
        {
            this.Service = service;
        }
        [HttpGet]

        public async Task<ActionResult<IEnumerable<PersonDto>>> Get()
        {
            return Ok(await Service.FindAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonDto>> Get(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await Service.FindById(id));
        }

        [HttpPost]
        public async Task<ActionResult<CrudOperationDto>> Post([FromBody] PersonDto value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await Service.Add(value);

            return Ok(new CrudOperationDto()
            {
                Success = true,
                Id = entity.Id
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CrudOperationDto>> Put(Guid id, [FromBody] PersonDto value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            value.Id = id;

            await Service.Update(value);
            return Ok(new CrudOperationDto()
            {
                Success = true,
                Id = value.Id
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CrudOperationDto>> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await Service.Delete(id);
            return Ok(new CrudOperationDto()
            {
                Success = true,
                Id = id
            });
        }
    }
}
