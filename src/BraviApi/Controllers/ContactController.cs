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

    public class ContactController : ControllerBase
    {
        public IContactService Service { get; }

        public ContactController(IContactService service)
        {
            this.Service = service;
        }
        [HttpGet("{personId}")]

        public async Task<ActionResult<IEnumerable<ContactDto>>> Get(Guid personId)
        {
            return Ok(await Service.FindAllByPerson(personId));
        }

        [HttpGet("{personId}/{id}")]
        public async Task<ActionResult<ContactDto>> Get(Guid personId, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await Service.FindById(id));
        }

        [HttpPost("{personId}")]
        public async Task<ActionResult<CrudOperationDto>> Post(Guid personId, [FromBody] ContactDto value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            value.PersonId = personId;

            var entity = await Service.Add(value);

            return Ok(new CrudOperationDto()
            {
                Success = true,
                Id = entity.Id
            });
        }

        [HttpPut("{personId}/{id}")]
        public async Task<ActionResult<CrudOperationDto>> Put(Guid personId, Guid id, [FromBody] ContactDto value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            value.PersonId = personId;
            value.Id = id;
            await Service.Update(value);
            return Ok(new CrudOperationDto()
            {
                Success = true,
                Id = value.Id
            });
        }

        [HttpDelete("{personId}/{id}")]
        public async Task<ActionResult<CrudOperationDto>> Delete(Guid personId, Guid id)
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
