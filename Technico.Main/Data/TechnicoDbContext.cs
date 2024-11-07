using Microsoft.EntityFrameworkCore;
using Technico.Main.Models.Enums;
using Technico.Main.Models;

namespace Technico.Main.Data;

public class TechnicoDbContext: DbContext
{
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<Repair> Repairs { get; set; }


    public TechnicoDbContext(DbContextOptions<TechnicoDbContext> options) : base(options) { }



    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    string connectionString = "Data Source=(local);Initial Catalog=TechnicoWeb; Integrated Security = True;TrustServerCertificate=True;";
    //    optionsBuilder.UseSqlServer(connectionString);
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Unique constraints on Email and Vat in User
        modelBuilder.Entity<Owner>()
            .HasIndex(p => p.Email)
            .IsUnique();

        modelBuilder.Entity<Owner>()
            .HasIndex(p => p.Vat)
            .IsUnique();

        modelBuilder.Entity<Property>()
            .HasIndex(p => p.E9)
            .IsUnique();

        /* modelBuilder.Entity<Owner>().HasData(
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
                 Properties = new List<Property>()

             },
           */

        //base.OnModelCreating(modelBuilder);
    }
}
