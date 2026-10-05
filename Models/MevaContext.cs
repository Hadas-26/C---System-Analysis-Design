using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MedicaidEmploymentVerificationApplication.Models
{
    public class MevaContext : IdentityDbContext<User>
    {
        public MevaContext(DbContextOptions<MevaContext> options) : base(options) { }

        //Add all needed databases here
        public DbSet<Applicant> Applicants { get; set; } = null!;
        public DbSet<Application> Applications { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<ApplicationDocument> Documents { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Apply configurations here
            modelBuilder.Entity<Applicant>().HasIndex(a => a.Id).IsUnique();
            //modelBuilder.Entity<Employee>().HasIndex(e => e.EmployeeId).IsUnique();

            //modelBuilder.Entity<Employee>().HasData(
            //    new Employee { }
            //    )

            //Add any default data here
        }
    }
}
