using Microsoft.EntityFrameworkCore;

namespace EmpolyeeManagement.Model
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Department> Departments { get; set; }  

        public DbSet<Designation> designations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Department -> Designation (1 to many)
            modelBuilder.Entity<Designation>()
                .HasOne<Department>()
                .WithMany()
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Designation -> Employee (1 to many)
            modelBuilder.Entity<Employee>()
                .HasOne<Designation>()
                .WithMany()
                .HasForeignKey(e => e.DesignationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

