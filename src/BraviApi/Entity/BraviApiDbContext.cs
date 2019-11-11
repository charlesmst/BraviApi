using System;
using Microsoft.EntityFrameworkCore;

namespace BraviApi.Entity
{
    public class BraviApiDbContext : DbContext
    {

        public DbSet<Person> People { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public BraviApiDbContext()
        {

        }
        public BraviApiDbContext(DbContextOptions<BraviApiDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(500);
            });
            modelBuilder.Entity<Contact>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.PersonId).IsRequired();
                entity.HasOne(d => d.Person)
                  .WithMany(p => p.Contacts)
                  .HasForeignKey(d => d.PersonId);
            });

        }
    }
}
