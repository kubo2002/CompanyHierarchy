using CompanyManagement.Domain.Entities;

namespace CompanyManagement.Application.Abstractions.Repositories
{
    /// <summary>
    /// Rozhranie pre pracu s priradenim zamestnancov k oddeleniam.
    /// Zodpoveda za kontrolu, pridanie a odstranenie vazby zamestnanec–oddelenie.
    /// </summary>
    public interface IDepartmentEmployeeRepository
    {
        /// <summary>
        /// Zisti, ci je zamestnanec priradeny k nejakemu oddeleniu.
        /// </summary>
        /// <param name="employeeId">Identifikator zamestnanca.</param>
        /// <returns>
        /// True, ak je zamestnanec priradeny k oddeleniu, inak false.
        /// </returns>
        Task<bool> IsEmployeeAssignedAsync(Guid employeeId);

        /// <summary>
        /// Priradi zamestnanca ku konkretnemu oddeleniu.
        /// </summary>
        /// <param name="departmentId">Identifikator oddelenia.</param>
        /// <param name="employeeId">Identifikator zamestnanca.</param>
        Task AddAsync(Guid departmentId, Guid employeeId);

        /// <summary>
        /// Odstrani priradenie zamestnanca k oddeleniu na zaklade jeho ID.
        /// </summary>
        /// <param name="employeeId">Identifikator zamestnanca.</param>
        Task RemoveByEmployeeIdAsync(Guid employeeId);

        /// <summary>
        /// Vrati zoznam zamestnancov priradenych k konkretnemu oddeleniu.
        /// </summary>
        Task<List<Employee>> GetEmployeesByDepartmentIdAsync(Guid departmentId);
    }
}
