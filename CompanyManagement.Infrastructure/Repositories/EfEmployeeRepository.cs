using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Domain.Entities;
using CompanyManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagement.Infrastructure.Repositories
{
    /// <summary>
    /// Entity Framework implementacia repozitara pre entitu Employee.
    /// Zodpoveda za ukladanie, nacitavanie a mazanie zamestnancov z databazy.
    /// </summary>
    public class EfEmployeeRepository : IEmployeeRepository
    {
        /// <summary>
        /// Databazovy kontext aplikacie.
        /// </summary>
        private readonly ManagementDbContext _dbContext;

        /// <summary>
        /// Inicializuje repozitar s databazovym kontextom.
        /// </summary>
        /// <param name="dbContext">Instancia databazoveho kontextu.</param>
        public EfEmployeeRepository(ManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Prida noveho zamestnanca do databazy.
        /// </summary>
        /// <param name="employee">Entita zamestnanca, ktora sa ma ulozit.</param>
        public async Task AddAsync(Employee employee)
        {
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Vrati zamestnanca na zaklade jeho identifikatora.
        /// </summary>
        /// <param name="id">Identifikator zamestnanca.</param>
        /// <returns>
        /// Entita zamestnanca alebo null, ak zamestnanec neexistuje.
        /// </returns>
        public async Task<Employee?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == id);
        }

        /// <summary>
        /// Overi, ci uz existuje zamestnanec s danym emailom.
        /// </summary>
        /// <param name="email">Emailova adresa zamestnanca.</param>
        /// <returns>
        /// True, ak zamestnanec s danym emailom existuje, inak false.
        /// </returns>
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _dbContext.Employees
                .AnyAsync(e => e.Email == email);
        }

        /// <summary>
        /// Odstrani zamestnanca z databazy.
        /// </summary>
        /// <param name="employee">Entita zamestnanca, ktora sa ma odstranit.</param>
        public async Task DeleteAsync(Employee employee)
        {
            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Aktualizuje parametre existujuceho zamestnanca v databaze.
        /// </summary>
        public async Task UpdateEmployeeParametersAsync(Employee employee)
        {
            _dbContext.Employees.Update(employee);
            await _dbContext.SaveChangesAsync();
        }
    }
}
