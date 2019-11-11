using System;
using Xunit;

using Microsoft.EntityFrameworkCore;
using BraviApi.Repository;
using BraviApi.Entity;
using System.Threading.Tasks;
using BraviApi.Exceptions;

namespace BraviApiTests.Repository
{
    public class PersonRepositoryTests
    {
        [Fact]
        public async Task ShouldAddNormallyTest()
        {
            var options = new DbContextOptionsBuilder<BraviApiDbContext>()
                .UseInMemoryDatabase(databaseName: "ShouldAddNormallyTest")
                .Options;

            var person = new Person()
            {
                Name = "Charles Stein",
                BirthDate = Convert.ToDateTime("10/04/1995")
            };

            using (var context = new BraviApiDbContext(options))
            {
                var repository = new PersonRepository(context);
                await repository.Add(person);
            }

            using (var context = new BraviApiDbContext(options))
            {
                Assert.Equal(1, await context.People.CountAsync());
                var dbPerson = await context.People.FirstAsync();
                Assert.Equal(person.Name, dbPerson.Name);
                Assert.Equal(person.BirthDate, dbPerson.BirthDate);
            }
        }

        [Fact]
        public async Task ShouldFailWithAddExistingPersonTests()
        {
            var options = new DbContextOptionsBuilder<BraviApiDbContext>()
                .UseInMemoryDatabase(databaseName: "ShouldFailWithAddExistingPersonTests")
                .Options;

            var person = new Person()
            {
                Name = "Charles Stein",
                BirthDate = Convert.ToDateTime("10/04/1995")
            };

            using (var context = new BraviApiDbContext(options))
            {
                await context.People.AddAsync(person);
                await context.SaveChangesAsync();
            }

            using (var context = new BraviApiDbContext(options))
            {
                var repository = new PersonRepository(context);
                await Assert.ThrowsAsync<PersonAlreadyExistsException>(async () =>
                {
                    await repository.Add(new Person()
                    {
                        Name = "Charles Stein",
                        BirthDate = Convert.ToDateTime("10/04/1995")
                    });
                });
            }

        }

        [Fact]
        public async Task ShouldUpdateSuccessfullyTests()
        {
            var options = new DbContextOptionsBuilder<BraviApiDbContext>()
                .UseInMemoryDatabase(databaseName: "ShouldUpdateSuccessfullyTests")
                .Options;

            var person = new Person()
            {
                Id = Guid.Parse("20a2b035-d2d2-4a04-ab62-9e62538fab19"),
                Name = "Charles Stein",
                BirthDate = Convert.ToDateTime("10/04/1995")
            };
            using (var context = new BraviApiDbContext(options))
            {
                await context.People.AddAsync(person);
                await context.SaveChangesAsync();
            }

            //Updates person normally
            using (var context = new BraviApiDbContext(options))
            {
                var repository = new PersonRepository(context);
                await repository.Update(new Person()
                {
                    Id = Guid.Parse("20a2b035-d2d2-4a04-ab62-9e62538fab19"),
                    Name = "Charles Stein Updated",
                    BirthDate = Convert.ToDateTime("10/04/1995")
                });
            }
            using (var context = new BraviApiDbContext(options))
            {
                Assert.Equal(1, await context.People.CountAsync());
                var dbPerson = await context.People.FirstAsync();
                Assert.Equal("Charles Stein Updated", dbPerson.Name);
            }
        }


        [Fact]
        public async Task ShouldUpdateFailWhenNotFoundTests()
        {
            var options = new DbContextOptionsBuilder<BraviApiDbContext>()
                .UseInMemoryDatabase(databaseName: "ShouldUpdateFailWhenNotFoundTests")
                .Options;

            var person = new Person()
            {
                Id = Guid.Parse("20a2b035-d2d2-4a04-ab62-9e62538fab19"),
                Name = "Charles Stein",
                BirthDate = Convert.ToDateTime("10/04/1995")
            };
            using (var context = new BraviApiDbContext(options))
            {
                await context.People.AddAsync(person);
                await context.SaveChangesAsync();
            }

            using (var context = new BraviApiDbContext(options))
            {
                var repository = new PersonRepository(context);
                await Assert.ThrowsAsync<PersonNotFoundException>(async () => await repository.Update(
                    new Person()
                    {
                        Id = Guid.Parse("1c26b3b4-d134-4948-98d3-8be687b0681b"),//Not Found Id
                        Name = "Charles Stein Updated",
                        BirthDate = Convert.ToDateTime("10/04/1995")
                    }));
            }
        }
        [Fact]
        public async Task ShouldDeleteSuccessfullyTests()
        {
            var options = new DbContextOptionsBuilder<BraviApiDbContext>()
                .UseInMemoryDatabase(databaseName: "ShouldDeleteSuccessfullyTests")
                .Options;

            var person = new Person()
            {
                Id = Guid.Parse("20a2b035-d2d2-4a04-ab62-9e62538fab19"),
                Name = "Charles Stein",
                BirthDate = Convert.ToDateTime("10/04/1995")
            };
            using (var context = new BraviApiDbContext(options))
            {
                await context.People.AddAsync(person);
                await context.SaveChangesAsync();
            }

            using (var context = new BraviApiDbContext(options))
            {
                var repository = new PersonRepository(context);
                await repository.Delete(Guid.Parse("20a2b035-d2d2-4a04-ab62-9e62538fab19"));
            }
            using (var context = new BraviApiDbContext(options))
            {
                Assert.Equal(0, await context.People.CountAsync());
            }
        }


        [Fact]
        public async Task ShouldFailDeleteNotFoundTests()
        {
            var options = new DbContextOptionsBuilder<BraviApiDbContext>()
                .UseInMemoryDatabase(databaseName: "ShouldFailDeleteNotFoundTests")
                .Options;

            var person = new Person()
            {
                Id = Guid.Parse("20a2b035-d2d2-4a04-ab62-9e62538fab19"),
                Name = "Charles Stein",
                BirthDate = Convert.ToDateTime("10/04/1995")
            };
            using (var context = new BraviApiDbContext(options))
            {
                await context.People.AddAsync(person);
                await context.SaveChangesAsync();
            }
            using (var context = new BraviApiDbContext(options))
            {
                var repository = new PersonRepository(context);
                await Assert.ThrowsAsync<PersonNotFoundException>(async () => await repository.Delete(Guid.Parse("1c26b3b4-d134-4948-98d3-8be687b0681b")));
            }
        }
    }
}
