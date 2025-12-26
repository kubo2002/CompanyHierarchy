using CompanyManagement.Application.Abstractions.Repositories;

namespace CompanyManagement.Application.UseCases
{
    /// <summary>
    /// Use case zodpovedny za odobratie zamestnanca z oddelenia.
    ///
    /// Aktualna implementacia odstrani vsetky priradenia
    /// zamestnanca k oddeleniam na zaklade jeho identifikatora.
    /// </summary>
    public class UnassignEmployeeFromDepartment
    {
        /// <summary>
        /// Repozitar pre spravu vazby medzi oddeleniami a zamestnancami.
        /// </summary>
        private readonly IDepartmentEmployeeRepository _departmentEmployeeRepository;

        /// <summary>
        /// Inicializuje use case s potrebnym repozitarom.
        /// </summary>
        /// <param name="departmentEmployeeRepository">
        /// Repozitar pre priradenia zamestnancov k oddeleniam.
        /// </param>
        public UnassignEmployeeFromDepartment(IDepartmentEmployeeRepository departmentEmployeeRepository)
        {
            _departmentEmployeeRepository = departmentEmployeeRepository;
        }

        /// <summary>
        /// Odstrani zamestnanca z oddelenia.
        ///
        /// Poznamka:
        /// Metoda aktualne odstrani vsetky priradenia zamestnanca
        /// k oddeleniam, bez ohladu na zadany departmentId.
        /// </summary>
        /// <param name="departmentId">Identifikator oddelenia.</param>
        /// <param name="employeeId">Identifikator zamestnanca.</param>
        public async Task ExecuteAsync(Guid employeeId)
        {
            await _departmentEmployeeRepository.RemoveByEmployeeIdAsync(employeeId);
        }
    }
}
