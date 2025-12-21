using CompanyManagement.Domain.Entities;
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
    }
}
