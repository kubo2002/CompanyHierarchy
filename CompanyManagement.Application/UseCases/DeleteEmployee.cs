using CompanyManagement.Application.Abstractions.Repositories;

namespace CompanyManagement.Application.UseCases
{
    /// <summary>
    /// Use case zodpovedny za odstranenie zamestnanca zo systemu.
    ///
    /// Pred vymazanim zamestnanca zabezpeci:
    /// - odstranenie jeho priradeni k oddeleniam
    /// - odobratie manazerskej role zo vsetkych uzlov
    /// </summary>
    public class DeleteEmployee
    {
        /// <summary>
        /// Repozitar pre pracu so zamestnancami.
        /// </summary>
        private readonly IEmployeeRepository _employeeRepository;

        /// <summary>
        /// Repozitar pre pracu s uzlami organizacnej hierarchie.
        /// </summary>
        private readonly INodeRepository _nodeRepository;

        /// <summary>
        /// Repozitar pre vazbu medzi oddeleniami a zamestnancami.
        /// </summary>
        private readonly IDepartmentEmployeeRepository _departmentRepository;

        /// <summary>
        /// Inicializuje use case s potrebnymi repozitarmi.
        /// </summary>
        /// <param name="employeeRepository">Repozitar zamestnancov.</param>
        /// <param name="nodeRepository">Repozitar uzlov.</param>
        /// <param name="departmentRepository">
        /// Repozitar pre spravu priradeni zamestnancov k oddeleniam.
        /// </param>
        public DeleteEmployee(IEmployeeRepository employeeRepository,INodeRepository nodeRepository, IDepartmentEmployeeRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _nodeRepository = nodeRepository;
            _departmentRepository = departmentRepository;
        }

        /// <summary>
        /// Vykona odstranenie zamestnanca zo systemu.
        ///
        /// Najskor odstrani vsetky jeho vazby na oddelenia a uzly,
        /// nasledne vymaze samotnu entitu zamestnanca.
        /// </summary>
        /// <param name="employeeId">Identifikator zamestnanca.</param>
        public async Task ExecuteAsync(Guid employeeId)
        {
            // Odstranenie vsetkych priradeni zamestnanca k oddeleniam
            await _departmentRepository.RemoveByEmployeeIdAsync(employeeId);

            // Odobratie manazerskej role zamestnanca zo vsetkych uzlov
            await _nodeRepository.UnassignManagerAsync(employeeId);

            // Nacitanie zamestnanca z databazy
            var employee = await _employeeRepository.GetByIdAsync(employeeId);

            // Ak zamestnanec neexistuje, operacia sa ukonci
            if (employee == null)
            {
                return;
            }

            // Odstranenie zamestnanca z databazy
            await _employeeRepository.DeleteAsync(employee);

        }
    }
}
