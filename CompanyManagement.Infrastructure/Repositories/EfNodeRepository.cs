using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Domain.Entities;
using CompanyManagement.Domain.Enums;
using CompanyManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagement.Infrastructure.Repositories
{
    /// <summary>
    /// Entity Framework implementacia repozitara pre entitu Node.
    /// Zodpoveda za perzistenciu, nacitavanie a spravu uzlov
    /// organizacnej hierarchie.
    /// </summary>
    public class EfNodeRepository : INodeRepository
    {
        /// <summary>
        /// Databazovy kontext aplikacie.
        /// </summary>
        private readonly ManagementDbContext _dbContext;

        /// <summary>
        /// Inicializuje repozitar s databazovym kontextom.
        /// </summary>
        /// <param name="dbContext">Instancia databazoveho kontextu.</param>
        public EfNodeRepository(ManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Prida novy uzol do databazy.
        /// </summary>
        /// <param name="node">Entita uzla, ktora sa ma ulozit.</param>
        public async Task AddAsync(Node node)
        {
            _dbContext.Nodes.Add(node);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Vrati uzol na zaklade jeho identifikatora.
        /// </summary>
        /// <param name="id">Identifikator uzla.</param>
        /// <returns>
        /// Entita uzla alebo null, ak uzol neexistuje.
        /// </returns>
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
        /// Vrati uzol, ktory je riadeny konkretnym zamestnancom.
        /// </summary>
        /// <param name="employeeId">Identifikator zamestnanca.</param>
        /// <returns>
        /// Uzol riadeny zamestnancom alebo null, ak taky neexistuje.
        /// </returns>
        public async Task<Node?> GetNodeManagedByEmployeeAsync(Guid employeeId)
        {
            return await _dbContext.Nodes.FirstOrDefaultAsync(n => n.LeaderEmployeeId == employeeId);
        }

        /// <summary>
        /// Overi, ci je zamestnanec priradeny ku konkretnemu uzlu.
        /// </summary>
        /// <param name="nodeId">Identifikator uzla.</param>
        /// <param name="employeeId">Identifikator zamestnanca.</param>
        /// <returns>
        /// True, ak je zamestnanec priradeny, inak false.
        /// </returns>
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
        /// Odstrani priradenie zamestnanca ku konkretnemu uzlu.
        /// Ak priradenie neexistuje, operacia sa ticho ukonci.
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
        /// Overi, ci je zamestnanec priradeny aspon k jednemu oddeleniu
        /// v ramci celej organizacnej hierarchie.
        /// </summary>
        /// <param name="employeeId">Identifikator zamestnanca.</param>
        /// <returns>
        /// True, ak je zamestnanec priradeny k aspon jednemu oddeleniu, inak false.
        /// </returns>
        public async Task<bool> IsEmployeeAssignedToAnyDepartmentAsync(Guid employeeId)
        {
            return await _dbContext.DepartmentEmployees
                .AnyAsync(de => de.EmployeeId == employeeId);
        }

        /// <summary>
        /// Vrati vsetky child uzly pre dany rodicovsky uzol.
        /// </summary>
        /// <param name="parentId">Id rodica (vrchol na ktorom prave stojim)</param>
        /// <returns></returns>
        public async Task<List<Node>> GetChildrenAsync(Guid parentId)
        {
            return await _dbContext.Nodes
                .Where(n => n.ParentId == parentId)
                .ToListAsync();
        }

        /// <summary>
        /// Vrati zoznam uzlov podla zadanych typov.
        /// </summary>
        /// <param name="types">Kolekcia typov uzlov.</param>
        public async Task<List<Node>> GetByTypesAsync(IEnumerable<NodeType> types)
        {
            return await _dbContext.Nodes
                .Where(n => types.Contains(n.Type))
                .ToListAsync();
        }

        /// <summary>
        /// Odstrani uzol z databazy.
        /// </summary>
        /// <param name="node">Uzol, ktory sa ma odstranit.</param>
        public async Task DeleteAsync(Node node)
        {
            _dbContext.Nodes.Remove(node);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Odstrani manazersku rolu zamestnanca z uzla,
        /// ktory momentalne riadi.
        /// </summary>
        /// <param name="employeeId">Identifikator zamestnanca.</param>
        public async Task UnassignManagerAsync(Guid employeeId)
        {
            var node = await _dbContext.Nodes.SingleOrDefaultAsync(n => n.LeaderEmployeeId == employeeId);


            if (node != null)
            {
                node.UnassignLeader();
            }
         
            await _dbContext.SaveChangesAsync();
        }
    }
}
