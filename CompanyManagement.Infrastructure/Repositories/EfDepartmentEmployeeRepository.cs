using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Domain.Entities;
using CompanyManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagement.Infrastructure.Repositories
{
    /// <summary>
    /// Entity Framework implementacia repozitara pre vazbu
    /// medzi oddeleniami a zamestnancami.
    /// </summary>
    public class EfDepartmentEmployeeRepository : IDepartmentEmployeeRepository
    {
        /// <summary>
        /// Databazovy kontext aplikacie.
        /// </summary>
        private readonly ManagementDbContext _dbContext;

        /// <summary>
        /// Inicializuje repozitar s databazovym kontextom.
        /// </summary>
        /// <param name="dbContext">Instancia databazoveho kontextu.</param>
        public EfDepartmentEmployeeRepository(ManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Zisti, ci je zamestnanec priradeny k aspon jednemu oddeleniu.
        /// </summary>
        /// <param name="employeeId">Identifikator zamestnanca.</param>
        /// <returns>
        /// True, ak existuje aspon jedno priradenie, inak false.
        /// </returns>
        public async Task<bool> IsEmployeeAssignedAsync(Guid employeeId)
        {
            return await _dbContext.DepartmentEmployees.AnyAsync(de => de.EmployeeId == employeeId);
        }

        /// <summary>
        /// Vytvori nove priradenie zamestnanca k oddeleniu.
        /// </summary>
        /// <param name="departmentId">Identifikator oddelenia.</param>
        /// <param name="employeeId">Identifikator zamestnanca.</param>
        public async Task AddAsync(Guid departmentId, Guid employeeId)
        {
            _dbContext.DepartmentEmployees.Add(
                new DepartmentEmployee
                {
                    NodeId = departmentId,
                    EmployeeId = employeeId
                });

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Odstrani vsetky priradenia zamestnanca k oddeleniam
        /// na zaklade jeho identifikatora.
        /// </summary>
        /// <param name="employeeId">Identifikator zamestnanca.</param>
        public async Task RemoveByEmployeeIdAsync(Guid employeeId)
        {
            var links = await _dbContext.DepartmentEmployees
                .Where(de => de.EmployeeId == employeeId)
                .ToListAsync();

            if (links.Count == 0)
            {
                return;
            }

            _dbContext.DepartmentEmployees.RemoveRange(links);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Vrati zoznam vsetkych zamestnancov priradených k danému oddeleniu.
        /// </summary>
        public async Task<List<Employee>> GetEmployeesByDepartmentIdAsync(Guid departmentId)
        {
            return await (
                from de in _dbContext.DepartmentEmployees
                join e in _dbContext.Employees
                    on de.EmployeeId equals e.Id
                where de.NodeId == departmentId
                select e
            ).ToListAsync();
        }
    }
}
