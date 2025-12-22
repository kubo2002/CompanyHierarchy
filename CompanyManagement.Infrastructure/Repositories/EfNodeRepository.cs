using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Domain.Entities;
using CompanyManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagement.Infrastructure.Repositories
{
    public class EfNodeRepository : INodeRepository
    {
        private readonly ManagementDbContext _dbContext;

        /// <summary>
        /// Implementacia repozitara pre entitu Node pomocou Entity Framework Core.
        /// Zodpoveda za perzistenciu a nacitavanie uzlov organizacnej struktury.
        /// </summary>
        public EfNodeRepository(ManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Prida novy uzol do databazy.
        /// </summary>
        /// <param name="node">Uzol, ktory sa ma ulozit.</param>
        public async Task AddAsync(Node node)
        {
            _dbContext.Nodes.Add(node);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Vrati uzol podla jeho identifikatora.
        /// </summary>
        /// <param name="id">Identifikator uzla.</param>
        /// <returns>Uzol alebo null, ak neexistuje.</returns>
        public async Task<Node?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Nodes.FirstOrDefaultAsync(n => n.Id == id);
        }

        /// <summary>
        /// Aktualizuje existujuci uzol v databaze.
        /// </summary>
        /// <param name="node">Uzol s aktualizovanymi hodnotami.</param>
        public async Task UpdateAsync(Node node)
        {
            _dbContext.Nodes.Update(node);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Vrati uzol, ktory je riadeny (managed) konkretnym zamestnancom.
        /// </summary>
        /// <param name="employeeId">Identifikator zamestnanca.</param>
        /// <returns>Uzol alebo null, ak zamestnanec nie je leader ziadneho uzla.</returns>
        public async Task<Node?> GetNodeManagedByEmployeeAsync(Guid employeeId)
        {
            return await _dbContext.Nodes.FirstOrDefaultAsync(n => n.LeaderEmployeeId == employeeId);
        }

        /// <summary>
        /// Zisti, ci je zamestnanec priradeny ku konkretnemu uzlu.
        /// </summary>
        /// <param name="nodeId">Identifikator uzla.</param>
        /// <param name="employeeId">Identifikator zamestnanca.</param>
        /// <returns>True, ak je zamestnanec priradeny, inak false.</returns>
        public async Task<bool> IsEmployeeAssignedToNodeAsync(Guid nodeId, Guid employeeId)
        {
            return await _dbContext.DepartmentEmployees
                .AnyAsync(de =>
                    de.NodeId == nodeId &&
                    de.EmployeeId == employeeId);
        }

        /// <summary>
        /// Priradi zamestnanca ku konkretnemu uzlu.
        /// </summary>
        /// <param name="nodeId">Identifikator uzla.</param>
        /// <param name="employeeId">Identifikator zamestnanca.</param>
        /// <exception cref="InvalidOperationException">
        /// Vyhodi sa, ak uzol alebo zamestnanec neexistuje.
        /// </exception>
        public async Task AssignEmployeeToNodeAsync(Guid nodeId, Guid employeeId)
        {
            _dbContext.DepartmentEmployees.Add(new DepartmentEmployee
            {
                NodeId = nodeId,
                EmployeeId = employeeId
            });

            await _dbContext.SaveChangesAsync();
          
        }

        /// <summary>
        /// Odstrani zamestnanca z konkretneho uzla.
        /// Ak uzol alebo zamestnanec neexistuje, operacia sa ticho ukonci.
        /// </summary>
        /// <param name="nodeId">Identifikator uzla.</param>
        /// <param name="employeeId">Identifikator zamestnanca.</param>
        public async Task RemoveEmployeeFromNodeAsync(Guid nodeId, Guid employeeId)
        {
            var relation = await _dbContext.DepartmentEmployees.FirstOrDefaultAsync(de =>
                    de.NodeId == nodeId &&
                    de.EmployeeId == employeeId);

            if (relation == null)
            {
                return; 
            }

            _dbContext.DepartmentEmployees.Remove(relation);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Kontroleuje, ci je zamestnanec priradeny k aspon jednému oddeleniu v organizacnej strukture.
        /// </summary>
        /// <param name="employeeId">ID zamestnanca</param>
        /// <returns></returns>
        public async Task<bool> IsEmployeeAssignedToAnyDepartmentAsync(Guid employeeId)
        {
            return await _dbContext.DepartmentEmployees
                .AnyAsync(de => de.EmployeeId == employeeId);
        }
    }
}
