using CompanyManagement.Domain.Entities;

namespace CompanyManagement.Application.Abstractions.Repositories
{
    /// <summary>
    /// Rozhranie pre pristup k zamestnancom v perzistentnej vrstve aplikacie.
    /// Definuje zakladne operacie nad entitou Employee.
    /// </summary>
    public interface IEmployeeRepository
    {
        /// <summary>
        /// Prida noveho zamestnanca do uloziska.
        /// </summary>
        /// <param name="employee">Entita zamestnanca, ktora sa ma ulozit.</param>
        Task AddAsync(Employee employee);

        /// <summary>
        /// Vrati zamestnanca na zaklade jeho identifikatora.
        /// </summary>
        /// <param name="id">Identifikator zamestnanca.</param>
        /// <returns>
        /// Entita zamestnanca alebo null, ak zamestnanec neexistuje.
        /// </returns>
        Task<Employee?> GetByIdAsync(Guid id);

        /// <summary>
        /// Overi, ci uz existuje zamestnanec s danym emailom.
        /// </summary>
        /// <param name="email">Emailova adresa zamestnanca.</param>
        /// <returns>
        /// True, ak zamestnanec s danym emailom existuje, inak false.
        /// </returns>
        Task<bool> ExistsByEmailAsync(string email);

        /// <summary>
        /// Odstrani zamestnanca z uloziska.
        /// </summary>
        /// <param name="employee">Entita zamestnanca, ktora sa ma odstranit.</param>
        Task DeleteAsync(Employee employee);

        /// <summary>
        /// Aktualizuje informacie o zamestnancovi v databaze.
        /// </summary>
        Task UpdateEmployeeParametersAsync(Employee employee);
    }
}
