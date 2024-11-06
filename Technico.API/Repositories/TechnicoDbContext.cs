using Microsoft.EntityFrameworkCore;
using Technico.API.Domain;
using Technico.API.Domain.Enums;

namespace Technico.API.Repositories;

public class TechnicoDbContext : DbContext
{
    public DbSet<Owner> Owner { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<Repair> Repairs { get; set; }

    public TechnicoDbContext(DbContextOptions<TechnicoDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Unique constraints on Email and Vat in User
        modelBuilder.Entity<Owner>()
            .HasIndex(p => p.Email)
            .IsUnique();

        modelBuilder.Entity<Owner>()
            .HasIndex(p => p.Vat)
            .IsUnique();

        modelBuilder.Entity<Owner>().HasData(
            new Owner()
            {
                Firstname = "John",
                Lastname = "Doe",
                Vat = "123098765",
                Phone = "6945542230",
                Email = "john.doe@example.com",
                Password = "Password123@",
                Role = TypeOfUser.Admin,
                Address = "Dramas 1, 64100, Eleftheroupoli, Greece",
                //Properties = new List<Property>()

            },

            new Owner()
            {
                Firstname = "Mary",
                Lastname = "Smith",
                Vat = "0987654321",
                Phone = "6945672530",
                Email = "mary.smith@example.com",
                Password = "Password123@",
                Role = TypeOfUser.User,
                Address = "Kazatzaki 8, Aleksandroupoli, 64100, Greece"
                //Properties = new List<Property>()
            }

        );
        base.OnModelCreating(modelBuilder);
    }
}
