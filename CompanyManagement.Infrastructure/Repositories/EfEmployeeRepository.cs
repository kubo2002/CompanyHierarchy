using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Domain.Entities;
using CompanyManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagement.Infrastructure.Repositories
{
    public class EfEmployeeRepository : IEmployeeRepository
    {
        private readonly ManagementDbContext _dbContext;

        /// <summary>
        /// Implementacia repozitara pre entitu Employee pomocou Entity Framework Core.
        /// Zodpoveda za perzistenciu a nacitavanie zamestnancov z databazy.
        /// </summary>
        public EfEmployeeRepository(ManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Prida noveho zamestnanca do databazy.
        /// </summary>
        /// <param name="employee">Zamestnanec, ktory sa ma ulozit.</param>
        public async Task AddAsync(Employee employee)
        {
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Vrati zamestnanca podla jeho identifikatora.
        /// </summary>
        /// <param name="id">Identifikator zamestnanca.</param>
        /// <returns>Zamestnanec alebo null, ak neexistuje.</returns>
        public async Task<Employee?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
