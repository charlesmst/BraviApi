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

namespace BraviApiTests.Repository
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
                BirthDate = Convert.ToDateTime("10/04/1995")
            };

            var service = new PersonService(repository.Object);
            await service.Add(personDto);
            repository
                .Verify(x => x.Add(It.Is<Person>(y => y.Name == personDto.Name && y.BirthDate == personDto.BirthDate)), Times.Once());

        }

        [Fact]
        public async Task ShouldAddThrowErrorWhenPersonExistsTest()
        {
            var repository = new Mock<IPersonRepository>();
            var personDto = new PersonDto()
            {
                Name = "Charles",
                BirthDate = Convert.ToDateTime("10/04/1995")
            };
            repository
                .Setup(x => x.FindByNameAndBirthDate(personDto.Name, personDto.BirthDate))
                .Returns(Task.FromResult(new Person()
                {
                    Id = Guid.NewGuid(),
                    Name = personDto.Name,
                    BirthDate = personDto.BirthDate
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
                BirthDate = Convert.ToDateTime("10/04/1995")
            };
            var returningPerson = Task.FromResult(new Person()
            {
                Id = personDto.Id,
                Name = personDto.Name,
                BirthDate = personDto.BirthDate
            });
            repository
                .Setup(x => x.FindById(personDto.Id))
                .Returns(returningPerson);
            var service = new PersonService(repository.Object);
            await service.Update(personDto);
            repository
                .Verify(x => x.Update(It.Is<Person>(y => y.Id == personDto.Id && y.Name == personDto.Name && y.BirthDate == personDto.BirthDate)), Times.Once());

        }

        [Fact]
        public async Task ShouldUpdateThrowErrorWhenPersonExistsTest()
        {
            var repository = new Mock<IPersonRepository>();
            var personDto = new PersonDto()
            {
                Id = Guid.NewGuid(),
                Name = "Charles",
                BirthDate = Convert.ToDateTime("10/04/1995")
            };
            repository
                .Setup(x => x.FindByNameAndBirthDate(personDto.Name, personDto.BirthDate))
                .Returns(Task.FromResult(new Person()
                {
                    Id = Guid.NewGuid(),
                    Name = personDto.Name,
                    BirthDate = personDto.BirthDate
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
                BirthDate = Convert.ToDateTime("10/04/1995")
            };
            var returningPerson = new Person()
            {
                Id = personDto.Id,//same id as updating
                Name = personDto.Name,
                BirthDate = personDto.BirthDate
            };
            repository
                .Setup(x => x.FindById(personDto.Id))
                .Returns(Task.FromResult(returningPerson));


            repository
                .Setup(x => x.FindByNameAndBirthDate(personDto.Name, personDto.BirthDate))
                .Returns(Task.FromResult(returningPerson));

            var service = new PersonService(repository.Object);
            await service.Update(personDto);
            repository
                .Verify(x => x.Update(It.Is<Person>(y => y.Id == personDto.Id && y.Name == personDto.Name && y.BirthDate == personDto.BirthDate)), Times.Once());
        }

        [Fact]
        public async Task ShouldCallDeletePersonTest()
        {
            var repository = new Mock<IPersonRepository>();
            var personDto = new PersonDto()
            {
                Id = Guid.NewGuid(),
                Name = "Charles",
                BirthDate = Convert.ToDateTime("10/04/1995")
            };
            var returningPerson = Task.FromResult(new Person()
            {
                Id = personDto.Id,
                Name = personDto.Name,
                BirthDate = personDto.BirthDate
            });
            repository
                           .Setup(x => x.FindById(personDto.Id))
                           .Returns(returningPerson);
            var service = new PersonService(repository.Object);
            await service.Delete(personDto.Id);
            repository
                .Verify(x => x.Delete(personDto.Id), Times.Once());

        }
        [Fact]
        public async Task ShouldDeleteThrowNotFoundPersonTest()
        {
            var repository = new Mock<IPersonRepository>();
            var personDto = new PersonDto()
            {
                Id = Guid.NewGuid(),
                Name = "Charles",
                BirthDate = Convert.ToDateTime("10/04/1995")
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

            var service = new PersonService(repository.Object);
            await service.FindAll();
            repository
                .Verify(x => x.FindAll(), Times.Once());

        }

        [Fact]
        public async Task ShouldCallFindByIdTest()
        {
            var repository = new Mock<IPersonRepository>();

            var expectedId = Guid.NewGuid();
            var service = new PersonService(repository.Object);
            await service.FindById(expectedId);
            repository
                .Verify(x => x.FindById(expectedId), Times.Once());

        }
    }
}
