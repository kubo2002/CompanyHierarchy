using CompanyManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyManagement.Domain.Entities
{
    /// <summary>
    /// Reprezentuje uzol v strukture firmy.
    /// </summary>
    public class Node
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Code { get; private set; }
        public NodeType Type { get; private set; }
        public Guid? ParentId { get; private set; }
        public Guid? LeaderEmployeeId { get; private set; }
        public ICollection<Employee> Employees { get; } = new List<Employee>();

        private Node() { }

        /// <summary>
        /// Vytvori novu instanciu uzla.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="type"></param>
        /// <param name="parentId"></param>
        public Node(Guid id, string name, string code, NodeType type, Guid? parentId)
        {
            Id = id;
            Name = name;
            Code = code;
            Type = type;
            ParentId = parentId;
        }

        /// <summary>
        /// Vykona aktualizaciu nazvu a kodu uzla.
        /// </summary>
        /// <param name="name">Meno firmy.</param>
        /// <param name="code">Kod firmy</param>
        public void Rename(string name, string code)
        {
            Name = name;
            Code = code;
        }

        /// <summary>
        /// Priradi uzlu veduceho.
        /// </summary>
        /// <param name="employeeId">Unikatne ID zamestnanca.</param>
        public void AssignLeader(Guid? employeeId)
        {
            LeaderEmployeeId = employeeId;
        }
    }
}
