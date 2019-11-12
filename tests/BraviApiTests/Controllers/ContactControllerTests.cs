using System;
using Xunit;

using Microsoft.EntityFrameworkCore;
using BraviApi.Repository;
using BraviApi.Entity;
using System.Threading.Tasks;
using BraviApi.Exceptions;
using Moq;
using BraviApi.Service;
using System.Collections.Generic;
using BraviApi.Dto;
using BraviApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BraviApiTests.Controllers
{
    public class ContactControllerTests
    {
        public Guid PersonId { get; set; } = Guid.Parse("2fc4d2c4-3749-485f-8373-2c0d57bcb000");
        [Fact]
        public async Task GetShouldReturnAllPersonContactsTest()
        {
            var service = new Mock<IContactService>();
            var result = new List<ContactDto>(){
                new ContactDto()
            };
            service
                .Setup(x => x.FindAllByPerson(PersonId))
                .Returns(Task.FromResult(result));
            var controller = new ContactController(service.Object);
            var resultRequest = await controller.Get(PersonId);
            Assert.Same(result, ((OkObjectResult)resultRequest.Result).Value);
        }
        [Fact]
        public async Task GetShouldReturnOneTest()
        {
            var service = new Mock<IContactService>();
            var result = new ContactDto()
            {
                Id = Guid.NewGuid()
            };
            service
                .Setup(x => x.FindById(result.Id))
                .Returns(Task.FromResult(result));
            var controller = new ContactController(service.Object);
            var resultRequest = await controller.Get(PersonId, result.Id);
            Assert.Same(result, ((OkObjectResult)resultRequest.Result).Value);
        }

        [Fact]
        public async Task PostShouldSaveTest()
        {

            var service = new Mock<IContactService>();
            var result = new ContactDto()
            {
                Value = "john@abc.com",
                Type = ContactType.Email
            };
            var newId = Guid.NewGuid();
            service
                .Setup(x => x.Add(result))
                .Returns(Task.FromResult(new Contact()
                {
                    Id = newId
                }));
            var controller = new ContactController(service.Object);
            var resultRequest = await controller.Post(PersonId, result);
            var data = Assert.IsType<OkObjectResult>(resultRequest.Result);
            var crudOperation = Assert.IsType<CrudOperationDto>(data.Value);
            Assert.Equal(newId, crudOperation.Id);
            Assert.True(crudOperation.Success);

        }



        [Fact]
        public async Task PutShouldSaveTest()
        {

            var service = new Mock<IContactService>();
            var newId = Guid.NewGuid();
            var result = new ContactDto()
            {
                Id = newId,
                Value = "john@abc.com",
                Type = ContactType.Email
            };
            service
                .Setup(x => x.Update(result))
                .Returns(Task.FromResult(new Contact()
                {
                    Id = newId
                }));
            var controller = new ContactController(service.Object);
            var resultRequest = await controller.Put(PersonId, newId, result);
            var data = Assert.IsType<OkObjectResult>(resultRequest.Result);
            var crudOperation = Assert.IsType<CrudOperationDto>(data.Value);
            Assert.Equal(newId, crudOperation.Id);

            if (result.PersonId != PersonId)
            {
                Assert.True(false, "Put should set person Id based on route");
            }
            Assert.True(crudOperation.Success);

        }


        [Fact]
        public async Task DeleteShouldDeleteTest()
        {

            var service = new Mock<IContactService>();
            var newId = Guid.NewGuid();

            service
                .Setup(x => x.Delete(newId))
                .Returns(Task.CompletedTask);
            var controller = new ContactController(service.Object);
            var resultRequest = await controller.Delete(PersonId, newId);
            var data = Assert.IsType<OkObjectResult>(resultRequest.Result);
            var crudOperation = Assert.IsType<CrudOperationDto>(data.Value);
            Assert.Equal(newId, crudOperation.Id);
            Assert.True(crudOperation.Success);
            service.Verify(x => x.Delete(newId), Times.Once);

        }

    }
}
