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
    public class PersonRepositoryTests
    {
        private List<Person> SeededPeople = new List<Person>(){
            new Person()
                {
                    Id = Guid.Parse("20a2b035-d2d2-4a04-ab62-9e62538fab19"),
                    Name = "Charles Stein",
                    BirthDate = Convert.ToDateTime("10/04/1995")
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
                await context.SaveChangesAsync();
            }
            return options;
        }

        [Fact]
        public async Task ShouldAddPersonTest()
        {
            var options = await SeededDb("ShouldAddPersonTest");


            var person = new Person()
            {
                Name = "John Johnson",
                BirthDate = Convert.ToDateTime("10/04/1995")
            };
            Guid id;
            using (var context = new BraviApiDbContext(options))
            {
                var repository = new PersonRepository(context);
                await repository.Add(person);
                id = person.Id;
            }

            using (var context = new BraviApiDbContext(options))
            {
                Assert.Equal(SeededPeople.Count + 1, await context.People.CountAsync());
                var dbPerson = await context.People.FindAsync(id);
                Assert.Equal(person.Name, dbPerson.Name);
                Assert.Equal(person.BirthDate, dbPerson.BirthDate);
            }
        }

        [Fact]
        public async Task ShouldUpdatePersonTest()
        {
            var options = await SeededDb("ShouldUpdatePersonTest");

            using (var context = new BraviApiDbContext(options))
            {
                var repository = new PersonRepository(context);
                await repository.Update(new Person()
                {
                    Id = Guid.Parse("20a2b035-d2d2-4a04-ab62-9e62538fab19"),
                    Name = "John Updated",
                    BirthDate = Convert.ToDateTime("10/04/1995")
                });
            }
            using (var context = new BraviApiDbContext(options))
            {
                Assert.Equal(SeededPeople.Count, await context.People.CountAsync());
                var dbPerson = await context.People.FirstAsync();
                Assert.Equal("John Updated", dbPerson.Name);
                Assert.Equal(Convert.ToDateTime("10/04/1995"), dbPerson.BirthDate);
            }
        }

        [Fact]
        public async Task ShouldDeletePersonTest()
        {
            var options = await SeededDb("ShouldDeletePersonTest");

            using (var context = new BraviApiDbContext(options))
            {
                var repository = new PersonRepository(context);
                await repository.Delete(SeededPeople.First());
            }
            using (var context = new BraviApiDbContext(options))
            {
                Assert.Equal(SeededPeople.Count - 1, await context.People.CountAsync());
            }
        }


        [Fact]
        public async Task ShouldFindByIdPersonTest()
        {
            var options = await SeededDb("ShouldFindByIdPersonTest");

            using (var context = new BraviApiDbContext(options))
            {
                var repository = new PersonRepository(context);
                var foundPerson = await repository.FindById(SeededPeople.First().Id);
                Assert.NotNull(foundPerson);
                Assert.Equal(SeededPeople.First().Name, foundPerson.Name);
            }
        }

        [Fact]
        public async Task ShouldFindAllPeopleTest()
        {
            var options = await SeededDb("ShouldFindAllPeopleTest");

            using (var context = new BraviApiDbContext(options))
            {
                var repository = new PersonRepository(context);
                var foundPeople = await repository.FindAll();
                Assert.NotNull(foundPeople);
                Assert.Equal(SeededPeople.Count, foundPeople.Count);
            }
        }


        [Fact]
        public async Task ShouldFindByNameAndBirthdatePeopleTest()
        {
            var options = await SeededDb("ShouldFindByNameAndBirthdatePeopleTest");

            using (var context = new BraviApiDbContext(options))
            {
                var repository = new PersonRepository(context);
                var firstPerson = SeededPeople.First();
                var foundPerson = await repository.FindByNameAndBirthDate(firstPerson.Name, firstPerson.BirthDate);
                Assert.NotNull(foundPerson);
                Assert.Equal(firstPerson.Name, foundPerson.Name);
                Assert.Equal(firstPerson.BirthDate, foundPerson.BirthDate);
            }
        }
    }
}
