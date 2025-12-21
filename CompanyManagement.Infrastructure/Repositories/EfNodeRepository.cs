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
            var node = await _dbContext.Nodes.Include(n => n.Employees).FirstOrDefaultAsync(n => n.Id == nodeId);

            if (node == null)
            {
                return false;
            }
          
            return node.Employees.Any(e => e.Id == employeeId);
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
            var node = await _dbContext.Nodes.Include(n => n.Employees).FirstOrDefaultAsync(n => n.Id == nodeId)
                ?? throw new InvalidOperationException("Node not found");

            var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == employeeId)
                ?? throw new InvalidOperationException("Employee not found");

            node.Employees.Add(employee);
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
            var node = await _dbContext.Nodes.Include(n => n.Employees).FirstOrDefaultAsync(n => n.Id == nodeId);

            if (node == null)
            {
                return; 
            }

            var employee = node.Employees.FirstOrDefault(e => e.Id == employeeId);

            if (employee == null) 
            {
                return;
            }
        

            node.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();
        }
    }
}
