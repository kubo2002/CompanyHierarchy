using CompanyManagement.Domain.Entities;
using CompanyManagement.Domain.Enums;

namespace CompanyManagement.Application.Abstractions.Repositories
{
    /// <summary>
    /// Rozhranie pre pracu s uzlami organizacnej hierarchie.
    /// Umoznuje spravu uzlov, ich vztahov a priradenie zamestnancov.
    /// </summary>
    public interface INodeRepository
    {
        /// <summary>
        /// Prida novy uzol do uloziska.
        /// </summary>
        /// <param name="node">Entita uzla, ktora sa ma ulozit.</param>
        Task AddAsync(Node node);

        /// <summary>
        /// Vrati uzol na zaklade jeho identifikatora.
        /// </summary>
        /// <param name="id">Identifikator uzla.</param>
        /// <returns>
        /// Entita uzla alebo null, ak uzol neexistuje.
        /// </returns>
        Task<Node?> GetByIdAsync(Guid id);

        /// <summary>
        /// Aktualizuje existujuci uzol v ulozisku.
        /// </summary>
        /// <param name="node">Entita uzla s aktualizovanymi udajmi.</param>
        Task UpdateAsync(Node node);

        /// <summary>
        /// Odstrani uzol z uloziska.
        /// </summary>
        /// <param name="node">Entita uzla, ktora sa ma odstranit.</param>
        Task DeleteAsync(Node node);

        /// <summary>
        /// Vrati uzol, ktory je riadeny konkretnym zamestnancom.
        /// </summary>
        /// <param name="employeeId">Identifikator zamestnanca.</param>
        /// <returns>
        /// Uzol riadeny zamestnancom alebo null, ak taky neexistuje.
        /// </returns>
        Task<Node?> GetNodeManagedByEmployeeAsync(Guid employeeId);

        /// <summary>
        /// Overi, ci je zamestnanec priradeny ku konkretnemu uzlu.
        /// </summary>
        /// <param name="nodeId">Identifikator uzla.</param>
        /// <param name="employeeId">Identifikator zamestnanca.</param>
        /// <returns>
        /// True, ak je zamestnanec priradeny k uzlu, inak false.
        /// </returns>
        Task<bool> IsEmployeeAssignedToNodeAsync(Guid nodeId, Guid employeeId);

        /// <summary>
        /// Priradi zamestnanca ku konkretnemu uzlu.
        /// </summary>
        /// <param name="nodeId">Identifikator uzla.</param>
        /// <param name="employeeId">Identifikator zamestnanca.</param>
        Task AssignEmployeeToNodeAsync(Guid nodeId, Guid employeeId);

        /// <summary>
        /// Odstrani priradenie zamestnanca ku konkretnemu uzlu.
        /// </summary>
        /// <param name="nodeId">Identifikator uzla.</param>
        /// <param name="employeeId">Identifikator zamestnanca.</param>
        Task RemoveEmployeeFromNodeAsync(Guid nodeId, Guid employeeId);

        /// <summary>
        /// Odstrani zamestnanca z role veduceho uzla.
        /// </summary>
        /// <param name="employeeId">Identifikator zamestnanca.</param>
        Task UnassignManagerAsync(Guid employeeId);

        /// <summary>
        /// Overi, ci je zamestnanec priradeny aspon k jednemu oddeleniu
        /// v ramci celej organizacnej hierarchie.
        /// </summary>
        /// <param name="employeeId">Identifikator zamestnanca.</param>
        /// <returns>
        /// True, ak je zamestnanec priradeny k aspon jednemu oddeleniu, inak false.
        /// </returns>
        Task<bool> IsEmployeeAssignedToAnyDepartmentAsync(Guid employeeId);

        /// <summary>
        /// Vrati zoznam potomkov daneho uzla.
        /// Pouziva sa pri zostaveni stromovej struktury.
        /// </summary>
        /// <param name="parentId">Identifikator rodicovskeho uzla.</param>
        Task<List<Node>> GetChildrenAsync(Guid parentId);

        /// <summary>
        /// Vrati zoznam uzlov podla zadanych typov.
        /// </summary>
        /// <param name="types">Kolekcia typov uzlov.</param>
        Task<List<Node>> GetByTypesAsync(IEnumerable<NodeType> types);
    }
}
