using System;
using Xunit;

using Microsoft.EntityFrameworkCore;
using BraviApi.Repository;
using BraviApi.Entity;
using System.Threading.Tasks;
using BraviApi.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace BraviApiTests.Repository
{
    public class ContactRepositoryTests
    {
        private List<Person> SeededPeople = new List<Person>(){
            new Person()
                {

                    Id = Guid.Parse("770ba6b5-bbb3-43f3-9da6-eb834c702273"),
                    Name="Charles Stein",
                },
                   new Person()
                {

                    Id = Guid.Parse("2fc4d2c4-3749-485f-8373-2c0d57bcb000"),
                    Name="person2",
                },
        };
        private List<Contact> SeededContacts = new List<Contact>(){
            new Contact()
                {
                    Id = Guid.Parse("20a2b035-d2d2-4a04-ab62-9e62538fab19"),
                    PersonId = Guid.Parse("770ba6b5-bbb3-43f3-9da6-eb834c702273"),
                    Type = ContactType.Email,
                    Value="charles.mst@gmail.com"
                },
                new Contact()
                {
                    Id = Guid.Parse("5ceaf54e-1097-45b9-87aa-2451442b0793"),
                    PersonId = Guid.Parse("770ba6b5-bbb3-43f3-9da6-eb834c702273"),
                    Type = ContactType.Whatsapp,
                    Value="55999999999"
                },
                 new Contact()
                {
                    Id = Guid.Parse("02552cb8-8b35-4c73-84f0-f16727d0b351"),
                    PersonId = Guid.Parse("2fc4d2c4-3749-485f-8373-2c0d57bcb000"),
                    Type = ContactType.Email,
                    Value="person2@gmail.com"
                },
                new Contact()
                {
                    Id = Guid.Parse("e08d5e99-b508-4c42-8986-441b77928b1e"),
                    PersonId = Guid.Parse("2fc4d2c4-3749-485f-8373-2c0d57bcb000"),
                    Type = ContactType.Phone,
                    Value="55999999991"
                }
        };
        private async Task<DbContextOptions<BraviApiDbContext>> SeededDb(string dbName)
        {
            var options = new DbContextOptionsBuilder<BraviApiDbContext>()
                            .UseInMemoryDatabase(databaseName: dbName)
                            .Options;

            using (var context = new BraviApiDbContext(options))
            {
                await context.People.AddRangeAsync(SeededPeople);
                await context.Contacts.AddRangeAsync(SeededContacts);
                await context.SaveChangesAsync();
            }
            return options;
        }

        [Fact]
        public async Task ShouldAddContactTest()
        {
            var options = await SeededDb("ShouldAddContactTest");


            var contact = new Contact()
            {
                PersonId = Guid.Parse("2fc4d2c4-3749-485f-8373-2c0d57bcb000"),
                Type = ContactType.Whatsapp,
                Value = "55999999991"
            };
            Guid id;
            using (var context = new BraviApiDbContext(options))
            {
                var repository = new ContactRepository(context);
                await repository.Add(contact);
                id = contact.Id;
            }

            using (var context = new BraviApiDbContext(options))
            {
                Assert.Equal(SeededContacts.Count + 1, await context.Contacts.CountAsync());
                var dbContact = await context.Contacts.FindAsync(id);
                Assert.Equal(contact.PersonId, dbContact.PersonId);
                Assert.Equal(contact.Type, dbContact.Type);
                Assert.Equal(contact.Value, dbContact.Value);
            }
        }

        [Fact]
        public async Task ShouldUpdateContactTest()
        {
            var options = await SeededDb("ShouldUpdateContactTest");
            var testId = Guid.Parse("5ceaf54e-1097-45b9-87aa-2451442b0793");

            using (var context = new BraviApiDbContext(options))
            {
                var contact = await context.Contacts.FindAsync(testId);
                contact.Value = "5555555555";
                var repository = new ContactRepository(context);
                await repository.Update(contact);
            }
            using (var context = new BraviApiDbContext(options))
            {
                Assert.Equal(SeededContacts.Count, await context.Contacts.CountAsync());
                var contact = await context.Contacts.FindAsync(testId);
                Assert.Equal("5555555555", contact.Value);
            }
        }

        [Fact]
        public async Task ShouldDeleteContactTest()
        {
            var options = await SeededDb("ShouldDeleteContactTest");
            var testId = Guid.Parse("5ceaf54e-1097-45b9-87aa-2451442b0793");
            using (var context = new BraviApiDbContext(options))
            {
                var entity = await context.Contacts.FindAsync(testId);

                var repository = new ContactRepository(context);
                await repository.Delete(entity);
            }
            using (var context = new BraviApiDbContext(options))
            {
                Assert.Equal(SeededContacts.Count - 1, await context.Contacts.CountAsync());
                Assert.Null(await context.Contacts.FindAsync(testId));

            }
        }


        [Fact]
        public async Task ShouldFindByIdContactTest()
        {
            var options = await SeededDb("ShouldFindByIdContactTest");
            var testId = Guid.Parse("5ceaf54e-1097-45b9-87aa-2451442b0793");

            using (var context = new BraviApiDbContext(options))
            {
                var repository = new ContactRepository(context);
                var foundContact = await repository.FindById(testId);
                Assert.NotNull(foundContact);
                Assert.Equal(testId, foundContact.Id);
            }
        }

        [Fact]
        public async Task ShouldFindAllPersonContactsTest()
        {
            var options = await SeededDb("ShouldFindAllPersonContactsTest");

            using (var context = new BraviApiDbContext(options))
            {
                var repository = new ContactRepository(context);
                var foundContact = await repository.FindAllByPerson(Guid.Parse("770ba6b5-bbb3-43f3-9da6-eb834c702273"));
                Assert.NotNull(foundContact);
                Assert.Equal(2, foundContact.Count());
            }
        }

    }
}
