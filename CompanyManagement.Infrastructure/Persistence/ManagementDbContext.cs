using CompanyManagement.Domain.Entities;
using CompanyManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagement.Infrastructure.Persistence
{
    /// <summary>
    /// Databazovy kontext aplikacie.
    /// Zabezpecuje pristup k databaze a mapovanie domenovych entit
    /// na databazove tabulky pomocou Entity Framework Core.
    /// </summary>
    public class ManagementDbContext : DbContext
    {
        /// <summary>
        /// Kolekcia uzlov organizacnej struktury (oddelenia, timy, firmy).
        /// Reprezentuje tabulku Nodes v databaze.
        /// </summary>
        public DbSet<Node> Nodes => Set<Node>();

        /// <summary>
        /// Kolekcia zamestnancov.
        /// Reprezentuje tabulku Employees v databaze.
        /// </summary>
        public DbSet<Employee> Employees => Set<Employee>();

        public DbSet<DepartmentEmployee> DepartmentEmployees => Set<DepartmentEmployee>();
        /// <summary>
        /// Inicializuje novu instanciu databazoveho kontextu
        /// s pouzitim zadanych konfiguracnych moznosti.
        /// </summary>
        /// <param name="options">
        /// Konfiguracne moznosti databazoveho kontextu,
        /// napriklad typ databazy a connection string.
        /// </param>
        public ManagementDbContext(DbContextOptions<ManagementDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ManagementDbContext).Assembly);

            modelBuilder.Entity<DepartmentEmployee>(builder =>
            {
                builder.ToTable("DepartmentEmployees");

                builder.HasKey(de => new { de.NodeId, de.EmployeeId });

                builder.Property(de => de.NodeId).IsRequired();
                builder.Property(de => de.EmployeeId).IsRequired();
            });
        }
    }
}
