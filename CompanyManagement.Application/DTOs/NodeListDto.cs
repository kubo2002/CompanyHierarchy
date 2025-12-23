using CompanyManagement.Domain.Entities;
using CompanyManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyManagement.Application.DTOs
{
    public class NodeListDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Code { get; init; } = string.Empty;
        public NodeType Type { get; init; }
        public ParentNodeDto? Parent { get; init; }
    }
}
