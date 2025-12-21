using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyManagement.Domain.Entities
{
    /// <summary>
    /// Reprezentuje zamestnanca vo firme.
    /// Trieda obsahuje vsetky potrebne 
    /// </summary>
    public class Employee
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public ICollection<Node> MemberOfNodes { get; } = new List<Node>();
        private Employee() { }

        /// <summary>
        /// Vytvori novu instanciu zamestnanca.
        /// </summary>
        /// <param name="id">Unikatne ID zamestnanca</param>
        /// <param name="firstName">Krstne meno zamestnanca.</param>
        /// <param name="lastName">Priezvisko zamestnanca.</param>
        /// <param name="email">Emial zamestnanca.</param>
        /// <param name="phone">Telefon zamestnanca.</param>
        public Employee(Guid id, string firstName, string lastName, string email, string phone)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
        }
    }
}
