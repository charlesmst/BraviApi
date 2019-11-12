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
    public class ContactServiceTests
    {

        [Fact]
        public async Task ShouldCallAddContactTest()
        {
            var repository = new Mock<IContactRepository>();
            var ContactDto = new ContactDto()
            {
                PersonId = Guid.NewGuid(),
                Type = ContactType.Email,
                Value = "charles.mst@gmail.com"
            };

            var service = new ContactService(repository.Object);
            await service.Add(ContactDto);
            repository
                .Verify(x => x.Add(It.Is<Contact>(y => y.PersonId == ContactDto.PersonId && y.Type == ContactDto.Type && y.Value == ContactDto.Value)), Times.Once());

        }

        // [Fact]
        // public async Task ShouldAddThrowErrorWhenContactExistsTest()
        // {
        //     var repository = new Mock<IContactRepository>();
        //     var ContactDto = new ContactDto()
        //     {
        //         Name = "Charles",
        //         BirthDate = Convert.ToDateTime("10/04/1995")
        //     };
        //     repository
        //         .Setup(x => x.FindByNameAndBirthDate(ContactDto.Name, ContactDto.BirthDate))
        //         .Returns(Task.FromResult(new Contact()
        //         {
        //             Id = Guid.NewGuid(),
        //             Name = ContactDto.Name,
        //             BirthDate = ContactDto.BirthDate
        //         }));
        //     var service = new ContactService(repository.Object);
        //     await Assert.ThrowsAsync<ContactAlreadyExistsException>(async () => await service.Add(ContactDto));

        // }


        [Fact]
        public async Task ShouldCallUpdateContactTest()
        {
            var repository = new Mock<IContactRepository>();
            var ContactDto = new ContactDto()
            {
                PersonId = Guid.NewGuid(),
                Type = ContactType.Email,
                Value = "charles.mst@gmail.com"
            };
            var returningContact = new Contact()
            {
                Id = Guid.NewGuid(),
                PersonId = ContactDto.PersonId,
                Type = ContactDto.Type,
                Value = ContactDto.Value
            };
            repository
                .Setup(x => x.FindById(ContactDto.Id))
                .Returns(Task.FromResult(returningContact));
            var service = new ContactService(repository.Object);
            ContactDto.Value = "charles.ms1t@gmail.com";
            await service.Update(ContactDto);
            repository
                .Verify(x => x.Update(It.Is<Contact>(y => y.Id == returningContact.Id)), Times.Once());

        }

        // [Fact]
        // public async Task ShouldUpdateThrowErrorWhenContactExistsTest()
        // {
        //     var repository = new Mock<IContactRepository>();
        //     var ContactDto = new ContactDto()
        //     {
        //         Id = Guid.NewGuid(),
        //         Name = "Charles",
        //         BirthDate = Convert.ToDateTime("10/04/1995")
        //     };
        //     repository
        //         .Setup(x => x.FindByNameAndBirthDate(ContactDto.Name, ContactDto.BirthDate))
        //         .Returns(Task.FromResult(new Contact()
        //         {
        //             Id = Guid.NewGuid(),
        //             Name = ContactDto.Name,
        //             BirthDate = ContactDto.BirthDate
        //         }));
        //     var service = new ContactService(repository.Object);
        //     await Assert.ThrowsAsync<ContactAlreadyExistsException>(async () => await service.Update(ContactDto));

        // }
        // [Fact]
        // public async Task ShouldUpdateNotThrowErrorWhenContactExistsWithSameIdTest()
        // {
        //     var repository = new Mock<IContactRepository>();
        //     var ContactDto = new ContactDto()
        //     {
        //         Id = Guid.NewGuid(),
        //         Name = "Charles",
        //         BirthDate = Convert.ToDateTime("10/04/1995")
        //     };
        //     var returningContact = new Contact()
        //     {
        //         Id = ContactDto.Id,//same id as updating
        //         Name = ContactDto.Name,
        //         BirthDate = ContactDto.BirthDate
        //     };
        //     repository
        //         .Setup(x => x.FindById(ContactDto.Id))
        //         .Returns(Task.FromResult(returningContact));


        //     repository
        //         .Setup(x => x.FindByNameAndBirthDate(ContactDto.Name, ContactDto.BirthDate))
        //         .Returns(Task.FromResult(returningContact));

        //     var service = new ContactService(repository.Object);
        //     await service.Update(ContactDto);
        //     repository
        //         .Verify(x => x.Update(It.Is<Contact>(y => y.Id == ContactDto.Id && y.Name == ContactDto.Name && y.BirthDate == ContactDto.BirthDate)), Times.Once());
        // }

        [Fact]
        public async Task ShouldCallDeleteContactTest()
        {
            var repository = new Mock<IContactRepository>();
            var ContactDto = new ContactDto()
            {
                Id = Guid.NewGuid(),

            };
            var returningContact = Task.FromResult(new Contact()
            {
                Id = ContactDto.Id,

            });
            repository
                .Setup(x => x.FindById(ContactDto.Id))
                .Returns(returningContact);
            var service = new ContactService(repository.Object);
            await service.Delete(ContactDto.Id);
            repository
                .Verify(x => x.Delete(It.Is<Contact>(y => y.Id == ContactDto.Id)), Times.Once());
        }
        [Fact]
        public async Task ShouldDeleteThrowNotFoundContactTest()
        {
            var repository = new Mock<IContactRepository>();
            var ContactDto = new ContactDto()
            {
                Id = Guid.NewGuid(),
            };
            repository
                .Setup(x => x.FindById(ContactDto.Id))
                .Returns(Task.FromResult<Contact>(null));
            var service = new ContactService(repository.Object);
            await Assert.ThrowsAsync<ContactNotFoundException>(async () => await service.Delete(ContactDto.Id));
        }

        [Fact]
        public async Task ShouldCallFindAllByPersonTest()
        {
            var repository = new Mock<IContactRepository>();
            var id = Guid.NewGuid();
            var contacts = new List<Contact>(){
                new Contact(){
                    Id = Guid.NewGuid()
                }
            };

            repository
                .Setup(x => x.FindAllByPerson(id))
                .Returns(Task.FromResult(contacts));

            var service = new ContactService(repository.Object);

            var results = await service.FindAllByPerson(id);
            repository
                .Verify(x => x.FindAllByPerson(id), Times.Once());
            Assert.Equal(contacts.Count, results.Count);
            Assert.Equal(contacts.First().Id, results.First().Id);
        }

        [Fact]
        public async Task ShouldCallFindByIdTest()
        {
            var expectedId = Guid.NewGuid();
            var repository = new Mock<IContactRepository>();
            repository
                .Setup(x => x.FindById(expectedId))
                .Returns(Task.FromResult<Contact>(new Contact()));
            var service = new ContactService(repository.Object);
            await service.FindById(expectedId);
            repository
                .Verify(x => x.FindById(expectedId), Times.Once());
        }
    }
}
