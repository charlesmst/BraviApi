using System;
using Xunit;

using Microsoft.EntityFrameworkCore;
using BraviApi.Repository;
using BraviApi.Entity;
using System.Threading.Tasks;
using BraviApi.Exceptions;
using System.Collections.Generic;
using System.Linq;
using Moq;
using BraviApi.Dto;
using BraviApi.Service;

namespace BraviApiTests.Service
{
    public class PersonServiceTests
    {

        [Fact]
        public async Task ShouldCallAddPersonTest()
        {
            var repository = new Mock<IPersonRepository>();
            var personDto = new PersonDto()
            {
                Name = "Charles",
            };

            var service = new PersonService(repository.Object);
            await service.Add(personDto);
            repository
                .Verify(x => x.Add(It.Is<Person>(y => y.Name == personDto.Name )), Times.Once());

        }

        [Fact]
        public async Task ShouldAddThrowErrorWhenPersonExistsTest()
        {
            var repository = new Mock<IPersonRepository>();
            var personDto = new PersonDto()
            {
                Name = "Charles"
            };
            repository
                .Setup(x => x.FindByName(personDto.Name))
                .Returns(Task.FromResult(new Person()
                {
                    Id = Guid.NewGuid(),
                    Name = personDto.Name,
                }));
            var service = new PersonService(repository.Object);
            await Assert.ThrowsAsync<PersonAlreadyExistsException>(async () => await service.Add(personDto));

        }


        [Fact]
        public async Task ShouldCallUpdatePersonTest()
        {
            var repository = new Mock<IPersonRepository>();
            var personDto = new PersonDto()
            {
                Id = Guid.NewGuid(),
                Name = "Charles",
            };
            var returningPerson = Task.FromResult(new Person()
            {
                Id = personDto.Id,
                Name = personDto.Name,
            });
            repository
                .Setup(x => x.FindById(personDto.Id))
                .Returns(returningPerson);
            var service = new PersonService(repository.Object);
            await service.Update(personDto);
            repository
                .Verify(x => x.Update(It.Is<Person>(y => y.Id == personDto.Id && y.Name == personDto.Name)), Times.Once());

        }

        [Fact]
        public async Task ShouldUpdateThrowErrorWhenPersonExistsTest()
        {
            var repository = new Mock<IPersonRepository>();
            var personDto = new PersonDto()
            {
                Id = Guid.NewGuid(),
                Name = "Charles",
            };
            repository
                .Setup(x => x.FindByName(personDto.Name))
                .Returns(Task.FromResult(new Person()
                {
                    Id = Guid.NewGuid(),
                    Name = personDto.Name,
                }));
            var service = new PersonService(repository.Object);
            await Assert.ThrowsAsync<PersonAlreadyExistsException>(async () => await service.Update(personDto));

        }
        [Fact]
        public async Task ShouldUpdateNotThrowErrorWhenPersonExistsWithSameIdTest()
        {
            var repository = new Mock<IPersonRepository>();
            var personDto = new PersonDto()
            {
                Id = Guid.NewGuid(),
                Name = "Charles",
            };
            var returningPerson = new Person()
            {
                Id = personDto.Id,//same id as updating
                Name = personDto.Name,
            };
            repository
                .Setup(x => x.FindById(personDto.Id))
                .Returns(Task.FromResult(returningPerson));


            repository
                .Setup(x => x.FindByName(personDto.Name))
                .Returns(Task.FromResult(returningPerson));

            var service = new PersonService(repository.Object);
            await service.Update(personDto);
            repository
                .Verify(x => x.Update(It.Is<Person>(y => y.Id == personDto.Id && y.Name == personDto.Name)), Times.Once());
        }

        [Fact]
        public async Task ShouldCallDeletePersonTest()
        {
            var repository = new Mock<IPersonRepository>();
            var personDto = new PersonDto()
            {
                Id = Guid.NewGuid(),
                Name = "Charles",
            };
            var returningPerson = Task.FromResult(new Person()
            {
                Id = personDto.Id,
                Name = personDto.Name,
            });
            repository
                .Setup(x => x.FindById(personDto.Id))
                .Returns(returningPerson);
            var service = new PersonService(repository.Object);
            await service.Delete(personDto.Id);
            repository
                .Verify(x => x.Delete(It.Is<Person>(y => y.Id == personDto.Id)), Times.Once());
        }
        [Fact]
        public async Task ShouldDeleteThrowNotFoundPersonTest()
        {
            var repository = new Mock<IPersonRepository>();
            var personDto = new PersonDto()
            {
                Id = Guid.NewGuid(),
                Name = "Charles",
            };
            repository
                .Setup(x => x.FindById(personDto.Id))
                .Returns(Task.FromResult<Person>(null));
            var service = new PersonService(repository.Object);
            await Assert.ThrowsAsync<PersonNotFoundException>(async () => await service.Delete(personDto.Id));
        }

        [Fact]
        public async Task ShouldCallFindAllTest()
        {
            var repository = new Mock<IPersonRepository>();
            repository
                .Setup(x => x.FindAll())
                .Returns(Task.FromResult(new List<Person>()));
            var service = new PersonService(repository.Object);
            await service.FindAll();
            repository
                .Verify(x => x.FindAll(), Times.Once());
        }

        [Fact]
        public async Task ShouldCallFindByIdTest()
        {
            var expectedId = Guid.NewGuid();
            var repository = new Mock<IPersonRepository>();
            repository
                .Setup(x => x.FindById(expectedId))
                .Returns(Task.FromResult<Person>(new Person()));
            var service = new PersonService(repository.Object);
            await service.FindById(expectedId);
            repository
                .Verify(x => x.FindById(expectedId), Times.Once());
        }
    }
}
