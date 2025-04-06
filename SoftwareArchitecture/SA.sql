-- Drop schema and tables if they exist
DROP TABLE IF EXISTS SoftwareArchitecture.EmployeeCredential;
DROP TABLE IF EXISTS SoftwareArchitecture.Employee;
DROP TABLE IF EXISTS SoftwareArchitecture.Person;
DROP TABLE IF EXISTS SoftwareArchitecture.Certification;
DROP TABLE IF EXISTS SoftwareArchitecture.License;
DROP TABLE IF EXISTS SoftwareArchitecture.CredentialBase;
DROP PROCEDURE IF EXISTS SoftwareArchitecture.AddPerson;
DROP PROCEDURE IF EXISTS SoftwareArchitecture.AddEmployee;
DROP SCHEMA IF EXISTS SoftwareArchitecture;
GO

-- Create schema
CREATE SCHEMA SoftwareArchitecture;
GO

-- Create CredentialBase table
CREATE TABLE SoftwareArchitecture.CredentialBase (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IssueDate DATE,
    ExpirationDate DATE,
    IssuingBody NVARCHAR(MAX)
);
GO

-- Create License table inheriting from CredentialBase
CREATE TABLE SoftwareArchitecture.License (
    LicenseIden NVARCHAR(MAX),
    LicenseName NVARCHAR(MAX),
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FOREIGN KEY (Id) REFERENCES SoftwareArchitecture.CredentialBase(Id)
);
GO

-- Create Certification table inheriting from CredentialBase
CREATE TABLE SoftwareArchitecture.Certification (
    CertificationIden NVARCHAR(MAX),
    CertificationName NVARCHAR(MAX),
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FOREIGN KEY (Id) REFERENCES SoftwareArchitecture.CredentialBase(Id)
);
GO

-- Create Person table
DROP TABLE IF EXISTS SoftwareArchitecture.Person;
CREATE TABLE SoftwareArchitecture.Person (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(MAX),
    Gender BIT,
    PhoneNumber NVARCHAR(MAX),
    Photo VARBINARY(MAX),
    Address NVARCHAR(MAX),
    DateOfBirth DATE,
    Email NVARCHAR(MAX)
);
GO

-- Create Employee table inheriting from Person
DROP TABLE IF EXISTS SoftwareArchitecture.Employee;
CREATE TABLE SoftwareArchitecture.Employee (
    Id INT PRIMARY KEY,
    Department NVARCHAR(MAX),
    Role NVARCHAR(MAX),
    FOREIGN KEY (Id) REFERENCES SoftwareArchitecture.Person(Id)
);
GO

-- Create EmployeeCredential junction table for many-to-many relationship
CREATE TABLE SoftwareArchitecture.EmployeeCredential (
    EmployeeId INT,
    CredentialId INT,
    PRIMARY KEY (EmployeeId, CredentialId),
    FOREIGN KEY (EmployeeId) REFERENCES SoftwareArchitecture.Employee(Id),
    FOREIGN KEY (CredentialId) REFERENCES SoftwareArchitecture.CredentialBase(Id)
);
GO

-- Create a function to add a person
CREATE PROCEDURE SoftwareArchitecture.AddPerson
    @Name NVARCHAR(MAX),
    @Gender BIT,
    @PhoneNumber NVARCHAR(MAX),
    @Photo VARBINARY(MAX),
    @Address NVARCHAR(MAX),
    @DateOfBirth DATE,
    @Email NVARCHAR(MAX),
    @NewPersonId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO SoftwareArchitecture.Person (Name, Gender, PhoneNumber, Photo, Address, DateOfBirth, Email)
    VALUES (@Name, @Gender, @PhoneNumber, @Photo, @Address, @DateOfBirth, @Email);

    SET @NewPersonId = SCOPE_IDENTITY();
END;
GO

-- Create a function to add an employee
CREATE PROCEDURE SoftwareArchitecture.AddEmployee
    @Name NVARCHAR(MAX),
    @Gender BIT,
    @PhoneNumber NVARCHAR(MAX),
    @Photo VARBINARY(MAX),
    @Address NVARCHAR(MAX),
    @DateOfBirth DATE,
    @Email NVARCHAR(MAX),
    @Department NVARCHAR(MAX),
    @Role NVARCHAR(MAX),
    @NewEmployeeId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NewPersonId INT;

    -- Add the person first
    EXEC SoftwareArchitecture.AddPerson
        @Name = @Name,
        @Gender = @Gender,
        @PhoneNumber = @PhoneNumber,
        @Photo = @Photo,
        @Address = @Address,
        @DateOfBirth = @DateOfBirth,
        @Email = @Email,
        @NewPersonId = @NewPersonId OUTPUT;

    -- Add the employee using the new person ID
    INSERT INTO SoftwareArchitecture.Employee (Id, Department, Role)
    VALUES (@NewPersonId, @Department, @Role);

    SET @NewEmployeeId = @NewPersonId;
END;
GO

-- Example usage of AddEmployee procedure
DECLARE @NewEmployeeId INT;

EXEC SoftwareArchitecture.AddEmployee
    @Name = 'John Doe',
    @Gender = 1,
    @PhoneNumber = '123-456-7890',
    @Photo = NULL,
    @Address = '123 Main St, Cityville',
    @DateOfBirth = '1990-01-01',
    @Email = 'johndoe@example.com',
    @Department = 'Engineering',
    @Role = 'Software Engineer',
    @NewEmployeeId = @NewEmployeeId OUTPUT;

SELECT @NewEmployeeId AS NewEmployeeId;