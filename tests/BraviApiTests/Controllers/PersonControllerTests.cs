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
    public class PersonControllerTests
    {
        [Fact]
        public async Task GetShouldReturnAllTest()
        {
            var service = new Mock<IPersonService>();
            var result = new List<PersonDto>(){
                new PersonDto()
            };
            service
                .Setup(x => x.FindAll())
                .Returns(Task.FromResult(result));
            var controller = new PersonController(service.Object);
            var resultRequest = await controller.Get();
            Assert.Same(result, ((OkObjectResult)resultRequest.Result).Value);
        }
        [Fact]
        public async Task GetShouldReturnOneTest()
        {
            var service = new Mock<IPersonService>();
            var result = new PersonDto()
            {
                Id = Guid.NewGuid()
            };
            service
                .Setup(x => x.FindById(result.Id))
                .Returns(Task.FromResult(result));
            var controller = new PersonController(service.Object);
            var resultRequest = await controller.Get(result.Id);
            Assert.Same(result, ((OkObjectResult)resultRequest.Result).Value);
        }

        [Fact]
        public async Task PostShouldSaveTest()
        {

            var service = new Mock<IPersonService>();
            var result = new PersonDto()
            {
                Name = "John"
            };
            var newId = Guid.NewGuid();
            service
                .Setup(x => x.Add(result))
                .Returns(Task.FromResult(new Person()
                {
                    Id = newId
                }));
            var controller = new PersonController(service.Object);
            var resultRequest = await controller.Post(result);
            var data = Assert.IsType<OkObjectResult>(resultRequest.Result);
            var crudOperation = Assert.IsType<CrudOperationDto>(data.Value);
            Assert.Equal(newId, crudOperation.Id);
            Assert.True(crudOperation.Success);

        }



        [Fact]
        public async Task PutShouldSaveTest()
        {

            var service = new Mock<IPersonService>();
            var newId = Guid.NewGuid();
            var result = new PersonDto()
            {
                Id = newId,
                Name = "John"
            };
            service
                .Setup(x => x.Update(result))
                .Returns(Task.FromResult(new Person()
                {
                    Id = newId
                }));
            var controller = new PersonController(service.Object);
            var resultRequest = await controller.Put(newId, result);
            var data = Assert.IsType<OkObjectResult>(resultRequest.Result);
            var crudOperation = Assert.IsType<CrudOperationDto>(data.Value);
            Assert.Equal(newId, crudOperation.Id);
            Assert.True(crudOperation.Success);

        }


        [Fact]
        public async Task DeleteShouldDeleteTest()
        {

            var service = new Mock<IPersonService>();
            var newId = Guid.NewGuid();

            service
                .Setup(x => x.Delete(newId))
                .Returns(Task.CompletedTask);
            var controller = new PersonController(service.Object);
            var resultRequest = await controller.Delete(newId);
            var data = Assert.IsType<OkObjectResult>(resultRequest.Result);
            var crudOperation = Assert.IsType<CrudOperationDto>(data.Value);
            Assert.Equal(newId, crudOperation.Id);
            Assert.True(crudOperation.Success);
            service.Verify(x => x.Delete(newId), Times.Once);

        }

    }
}
