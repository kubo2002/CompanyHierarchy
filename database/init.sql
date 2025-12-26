IF DB_ID('CompanyManagementDb') IS NULL
BEGIN
    CREATE DATABASE CompanyManagementDb;
END;

USE CompanyManagementDb;

CREATE TABLE [dbo].[Employees] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [FirstName] NVARCHAR(100) NOT NULL,
    [LastName] NVARCHAR(100) NOT NULL,
    [Email] NVARCHAR(255) NOT NULL,
    [AcademicTitle] NVARCHAR(50) NULL,
    [Phone] NVARCHAR(255) NULL
    CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY];


CREATE TABLE dbo.Nodes (
    Id UNIQUEIDENTIFIER NOT NULL,
    Name NVARCHAR(200) NOT NULL,
    Code NVARCHAR(50) NOT NULL,
    Type INT NOT NULL,
    ParentId UNIQUEIDENTIFIER NULL,
    LeaderEmployeeId UNIQUEIDENTIFIER NULL,

    CONSTRAINT PK_Nodes PRIMARY KEY (Id),

    CONSTRAINT FK_Nodes_Parent
        FOREIGN KEY (ParentId)
        REFERENCES dbo.Nodes (Id),

    CONSTRAINT FK_Nodes_LeaderEmployee
        FOREIGN KEY (LeaderEmployeeId)
        REFERENCES dbo.Employees (Id)
        ON DELETE SET NULL
);



CREATE TABLE [dbo].[DepartmentEmployees] (
    [NodeId] UNIQUEIDENTIFIER NOT NULL,
    [EmployeeId] UNIQUEIDENTIFIER NOT NULL,

    CONSTRAINT [PK_NodeEmployee]
        PRIMARY KEY CLUSTERED ([NodeId], [EmployeeId]),

    CONSTRAINT [FK_NodeEmployee_Node]
        FOREIGN KEY ([NodeId])
        REFERENCES [dbo].[Nodes] ([Id])
        ON DELETE CASCADE,

    CONSTRAINT [FK_NodeEmployee_Employee]
        FOREIGN KEY ([EmployeeId])
        REFERENCES [dbo].[Employees] ([Id])
        ON DELETE CASCADE
);
