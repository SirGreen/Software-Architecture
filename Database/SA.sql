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
    Name NVARCHAR(MAX),
    IssueDate DATE,
    ExpirationDate DATE,
    IssuingBody NVARCHAR(MAX)
);
GO

-- Create License table inheriting from CredentialBase
CREATE TABLE SoftwareArchitecture.License (
    Number NVARCHAR(MAX),
    Restriction NVARCHAR(MAX),
    Id INT PRIMARY KEY,
    FOREIGN KEY (Id) REFERENCES SoftwareArchitecture.CredentialBase(Id)
);
GO

-- Create Certification table inheriting from CredentialBase
CREATE TABLE SoftwareArchitecture.Certification (
    Level NVARCHAR(MAX),
    Version NVARCHAR(MAX),
    Id INT PRIMARY KEY,
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
    @Address NVARCHAR(MAX),
    @DateOfBirth DATE,
    @Email NVARCHAR(MAX),
    @NewPersonId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO SoftwareArchitecture.Person (Name, Gender, PhoneNumber , Address, DateOfBirth, Email)
    VALUES (@Name, @Gender, @PhoneNumber, @Address, @DateOfBirth, @Email);

    SET @NewPersonId = SCOPE_IDENTITY();
END;
GO

-- Create a function to add an employee
CREATE PROCEDURE SoftwareArchitecture.AddEmployee
    @Name NVARCHAR(MAX),
    @Gender BIT,
    @PhoneNumber NVARCHAR(MAX),
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

-- Create a procedure to edit an existing employee based on Id
CREATE PROCEDURE SoftwareArchitecture.EditEmployee
    @Id INT,
    @Name NVARCHAR(MAX),
    @Gender BIT,
    @PhoneNumber NVARCHAR(MAX),
    @Address NVARCHAR(MAX),
    @DateOfBirth DATE,
    @Email NVARCHAR(MAX),
    @Department NVARCHAR(MAX),
    @Role NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    -- Update the Person table
    UPDATE SoftwareArchitecture.Person
    SET Name = @Name,
        Gender = @Gender,
        PhoneNumber = @PhoneNumber,
        Address = @Address,
        DateOfBirth = @DateOfBirth,
        Email = @Email
    WHERE Id = @Id;

    -- Update the Employee table
    UPDATE SoftwareArchitecture.Employee
    SET Department = @Department,
        Role = @Role
    WHERE Id = @Id;
END;
GO

-- Create a procedure to find an employee by Id
DROP PROCEDURE IF EXISTS SoftwareArchitecture.FindById;
GO
CREATE PROCEDURE SoftwareArchitecture.FindById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.Id,
        p.Name,
        p.Gender,
        p.PhoneNumber,
        p.Address,
        p.DateOfBirth,
        p.Email,
        e.Department,
        e.Role
    FROM SoftwareArchitecture.Person p
    INNER JOIN SoftwareArchitecture.Employee e ON p.Id = e.Id
    WHERE p.Id = @Id;
END;
GO

-- Create a procedure to get all credentials of an employee
CREATE PROCEDURE SoftwareArchitecture.GetEmployeeCredentials
    @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        cb.Id AS CredentialId,
        cb.Name AS CredentialName,
        cb.IssueDate,
        cb.ExpirationDate,
        cb.IssuingBody,
        CASE 
            WHEN l.Id IS NOT NULL THEN 'License'
            WHEN c.Id IS NOT NULL THEN 'Certification'
            ELSE 'Unknown'
        END AS CredentialType,
        l.Number AS LicenseNumber,
        l.Restriction AS LicenseRestriction,
        c.Level AS CertificationLevel,
        c.Version AS CertificationVersion
    FROM SoftwareArchitecture.EmployeeCredential ec
    INNER JOIN SoftwareArchitecture.CredentialBase cb ON ec.CredentialId = cb.Id
    LEFT JOIN SoftwareArchitecture.License l ON cb.Id = l.Id
    LEFT JOIN SoftwareArchitecture.Certification c ON cb.Id = c.Id
    WHERE ec.EmployeeId = @EmployeeId;
END;
GO

EXEC SoftwareArchitecture.GetEmployeeCredentials
    @EmployeeId = 1;

-- Create a procedure to get all employees
CREATE PROCEDURE SoftwareArchitecture.GetAllEmployees
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.Id,
        p.Name,
        p.Gender,
        p.PhoneNumber,
        p.Address,
        p.DateOfBirth,
        p.Email,
        e.Department,
        e.Role
    FROM SoftwareArchitecture.Person p
    INNER JOIN SoftwareArchitecture.Employee e ON p.Id = e.Id;
END;
GO

EXEC SoftwareArchitecture.GetAllEmployees;

-- Example usage of GetEmployeeCredentials procedure
EXEC SoftwareArchitecture.GetEmployeeCredentials
    @EmployeeId = 1;

-- Create a procedure to delete an employee by Id
CREATE PROCEDURE SoftwareArchitecture.DeleteEmployee
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Delete from EmployeeCredential table first to maintain referential integrity
    DELETE FROM SoftwareArchitecture.EmployeeCredential
    WHERE EmployeeId = @Id;

    -- Delete from Employee table
    DELETE FROM SoftwareArchitecture.Employee
    WHERE Id = @Id;

    -- Delete from Person table
    DELETE FROM SoftwareArchitecture.Person
    WHERE Id = @Id;
END;
GO

-- Create a procedure to add a CredentialBase
DROP PROCEDURE IF EXISTS SoftwareArchitecture.AddCredentialBase;
GO
CREATE PROCEDURE SoftwareArchitecture.AddCredentialBase
    @Name NVARCHAR(MAX),
    @IssueDate DATE,
    @ExpirationDate DATE,
    @IssuingBody NVARCHAR(MAX),
    @NewCredentialBaseId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO SoftwareArchitecture.CredentialBase (Name, IssueDate, ExpirationDate, IssuingBody)
    VALUES (@Name, @IssueDate, @ExpirationDate, @IssuingBody);

    SET @NewCredentialBaseId = SCOPE_IDENTITY();
END;
GO

-- Create a procedure to add a License
DROP PROCEDURE IF EXISTS SoftwareArchitecture.AddLicense;
GO
CREATE PROCEDURE SoftwareArchitecture.AddLicense
    @Name NVARCHAR(MAX),
    @IssueDate DATE,
    @ExpirationDate DATE,
    @IssuingBody NVARCHAR(MAX),
    @Number NVARCHAR(MAX),
    @Restriction NVARCHAR(MAX),
    @NewLicenseId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NewCredentialBaseId INT;

    -- Add the CredentialBase first
    EXEC SoftwareArchitecture.AddCredentialBase
        @Name = @Name,
        @IssueDate = @IssueDate,
        @ExpirationDate = @ExpirationDate,
        @IssuingBody = @IssuingBody,
        @NewCredentialBaseId = @NewCredentialBaseId OUTPUT;

    -- Add the License using the new CredentialBase ID
    INSERT INTO SoftwareArchitecture.License (Id, Number, Restriction)
    VALUES (@NewCredentialBaseId, @Number, @Restriction);

    SET @NewLicenseId = @NewCredentialBaseId;
END;
GO

-- Create a procedure to add a Certification
DROP PROCEDURE IF EXISTS SoftwareArchitecture.AddCertification;
GO
CREATE PROCEDURE SoftwareArchitecture.AddCertification
    @Name NVARCHAR(MAX),
    @IssueDate DATE,
    @ExpirationDate DATE,
    @IssuingBody NVARCHAR(MAX),
    @Level NVARCHAR(MAX),
    @Version NVARCHAR(MAX),
    @NewCertificationId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NewCredentialBaseId INT;

    -- Add the CredentialBase first
    EXEC SoftwareArchitecture.AddCredentialBase
        @Name = @Name,
        @IssueDate = @IssueDate,
        @ExpirationDate = @ExpirationDate,
        @IssuingBody = @IssuingBody,
        @NewCredentialBaseId = @NewCredentialBaseId OUTPUT;

    -- Add the Certification using the new CredentialBase ID
    INSERT INTO SoftwareArchitecture.Certification (Id, Level, Version)
    VALUES (@NewCredentialBaseId, @Level, @Version);

    SET @NewCertificationId = @NewCredentialBaseId;
END;
GO

-- Create a procedure to add a Credential (License or Certification) based on CredentialType
CREATE PROCEDURE SoftwareArchitecture.AddCredential
    @CredentialType NVARCHAR(MAX), -- 'License' or 'Certificate'
    @Name NVARCHAR(MAX),
    @IssueDate DATE,
    @ExpirationDate DATE,
    @IssuingBody NVARCHAR(MAX),
    @Number NVARCHAR(MAX) = NULL, -- For License
    @Restriction NVARCHAR(MAX) = NULL, -- For License
    @Level NVARCHAR(MAX) = NULL, -- For Certification
    @Version NVARCHAR(MAX) = NULL, -- For Certification
    @NewCredentialId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @CredentialType = 'License'
    BEGIN
        -- Call AddLicense procedure
        EXEC SoftwareArchitecture.AddLicense
            @Name = @Name,
            @IssueDate = @IssueDate,
            @ExpirationDate = @ExpirationDate,
            @IssuingBody = @IssuingBody,
            @Number = @Number,
            @Restriction = @Restriction,
            @NewLicenseId = @NewCredentialId OUTPUT;
    END
    ELSE IF @CredentialType = 'Certificate'
    BEGIN
        -- Call AddCertification procedure
        EXEC SoftwareArchitecture.AddCertification
            @Name = @Name,
            @IssueDate = @IssueDate,
            @ExpirationDate = @ExpirationDate,
            @IssuingBody = @IssuingBody,
            @Level = @Level,
            @Version = @Version,
            @NewCertificationId = @NewCredentialId OUTPUT;
    END
    ELSE
    BEGIN
        PRINT 'Invalid CredentialType. Please specify either "License" or "Certificate".';
        SET @NewCredentialId = NULL;
    END
END;
GO

-- Create a procedure to get information from License by Id
DROP PROCEDURE IF EXISTS SoftwareArchitecture.GetLicenseById;
GO
CREATE PROCEDURE SoftwareArchitecture.GetLicenseById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        cb.Id,
        cb.Name,
        cb.IssueDate,
        cb.ExpirationDate,
        cb.IssuingBody,
        l.Number,
        l.Restriction
    FROM SoftwareArchitecture.CredentialBase cb
    INNER JOIN SoftwareArchitecture.License l ON cb.Id = l.Id
    WHERE cb.Id = @Id;
END;
GO

-- Create a procedure to get information from Certification by Id
CREATE PROCEDURE SoftwareArchitecture.GetCertificationById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        cb.Id,
        cb.Name,
        cb.IssueDate,
        cb.ExpirationDate,
        cb.IssuingBody,
        c.Level,
        c.Version
    FROM SoftwareArchitecture.CredentialBase cb
    INNER JOIN SoftwareArchitecture.Certification c ON cb.Id = c.Id
    WHERE cb.Id = @Id;
END;
GO

-- Create a procedure to get credential details by Id
CREATE PROCEDURE SoftwareArchitecture.GetCredentialById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if the Id exists in the License table
    IF EXISTS (SELECT 1 FROM SoftwareArchitecture.License WHERE Id = @Id)
    BEGIN
        -- Call GetLicenseById procedure
        EXEC SoftwareArchitecture.GetLicenseById @Id = @Id;
    END
    ELSE IF EXISTS (SELECT 1 FROM SoftwareArchitecture.Certification WHERE Id = @Id)
    BEGIN
        -- Call GetCertificationById procedure
        EXEC SoftwareArchitecture.GetCertificationById @Id = @Id;
    END
    ELSE
    BEGIN
        -- If the Id does not exist in either table, return a message
        PRINT 'No credential found with the given Id.';
    END
END;
GO

EXEC SoftwareArchitecture.GetCredentialById @Id = 1;

-- Create a procedure to assign an employee to a CredentialBase
CREATE PROCEDURE SoftwareArchitecture.AssignEmployeeToCredential
    @EmployeeId INT,
    @CredentialId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Insert into EmployeeCredential table
    INSERT INTO SoftwareArchitecture.EmployeeCredential (EmployeeId, CredentialId)
    VALUES (@EmployeeId, @CredentialId);
END;
GO

EXEC SoftwareArchitecture.AssignEmployeeToCredential
    @EmployeeId = 1, @CredentialId = 1;
GO
EXEC SoftwareArchitecture.GetEmployeeCredentials
    @EmployeeId = 1;

-- Create a procedure to update a Credential (License or Certification)
CREATE PROCEDURE SoftwareArchitecture.UpdateCredential
    @Id INT,
    @Name NVARCHAR(MAX),
    @IssueDate DATE,
    @ExpirationDate DATE,
    @IssuingBody NVARCHAR(MAX),
    @CredentialType NVARCHAR(MAX), -- 'License' or 'Certificate'
    @Number NVARCHAR(MAX) = NULL, -- For License
    @Restriction NVARCHAR(MAX) = NULL, -- For License
    @Level NVARCHAR(MAX) = NULL, -- For Certification
    @Version NVARCHAR(MAX) = NULL -- For Certification
AS
BEGIN
    SET NOCOUNT ON;

    -- Update the CredentialBase table
    UPDATE SoftwareArchitecture.CredentialBase
    SET Name = @Name,
        IssueDate = @IssueDate,
        ExpirationDate = @ExpirationDate,
        IssuingBody = @IssuingBody
    WHERE Id = @Id;

    -- Update the specific type of credential
    IF @CredentialType = 'License'
    BEGIN
        UPDATE SoftwareArchitecture.License
        SET Number = @Number,
            Restriction = @Restriction
        WHERE Id = @Id;
    END
    ELSE IF @CredentialType = 'Certificate'
    BEGIN
        UPDATE SoftwareArchitecture.Certification
        SET Level = @Level,
            Version = @Version
        WHERE Id = @Id;
    END
    ELSE
    BEGIN
        PRINT 'Invalid CredentialType. Please specify either "License" or "Certificate".';
    END
END;
GO

-- Create a procedure to delete a Credential (License or Certification)
DROP PROCEDURE IF EXISTS SoftwareArchitecture.DeleteCredential;
GO
CREATE PROCEDURE SoftwareArchitecture.DeleteCredential
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Delete associations from EmployeeCredential table
    DELETE FROM SoftwareArchitecture.EmployeeCredential
    WHERE CredentialId = @Id;

    -- Check if the Id exists in the License table
    IF EXISTS (SELECT 1 FROM SoftwareArchitecture.License WHERE Id = @Id)
    BEGIN
        -- Delete from License table
        DELETE FROM SoftwareArchitecture.License
        WHERE Id = @Id;
    END
    ELSE IF EXISTS (SELECT 1 FROM SoftwareArchitecture.Certification WHERE Id = @Id)
    BEGIN
        -- Delete from Certification table
        DELETE FROM SoftwareArchitecture.Certification
        WHERE Id = @Id;
    END

    -- Delete from CredentialBase table
    DELETE FROM SoftwareArchitecture.CredentialBase
    WHERE Id = @Id;
END;
GO

EXEC SoftwareArchitecture.DeleteCredential @Id = 2;

EXEC SoftwareArchitecture.AssignEmployeeToCredential
    @EmployeeId = 1, @CredentialId = 2;

EXEC SoftwareArchitecture.GetLicenseById @Id = 1;
EXEC SoftwareArchitecture.GetCertificationById @Id = 1;

-- Example usage of AddCertification procedure
DECLARE @NewCertificationId INT;

EXEC SoftwareArchitecture.AddCertification
    @Name = 'Certified Software Architect',
    @IssueDate = '2023-01-01',
    @ExpirationDate = '2025-01-01',
    @IssuingBody = 'Certification Authority',
    @Level = 'Expert',
    @Version = '1.0',
    @NewCertificationId = @NewCertificationId OUTPUT;

SELECT @NewCertificationId AS NewCertificationId;

-- Example usage of AddLicense procedure
DECLARE @NewLicenseId INT;

EXEC SoftwareArchitecture.AddLicense
    @Name = 'Professional Software License',
    @IssueDate = '2023-02-01',
    @ExpirationDate = '2026-02-01',
    @IssuingBody = 'Licensing Authority',
    @Number = 'LIC-67890',
    @Restriction = 'None',
    @NewLicenseId = @NewLicenseId OUTPUT;

SELECT @NewLicenseId AS NewLicenseId;

EXEC SoftwareArchitecture.DeleteEmployee @Id = 4;
EXEC SoftwareArchitecture.FindById @Id = 3;

-- Example usage of EditEmployee procedure
EXEC SoftwareArchitecture.EditEmployee
    @Id = 2,
    @Name = 'Jane Smith',
    @Gender = 0,
    @PhoneNumber = '987-654-3210',
    @Address = '456 Elm St, Townsville',
    @DateOfBirth = '1985-05-15',
    @Email = 'janesmith@example.com',
    @Department = 'Human Resources',
    @Role = 'HR Manager';

-- Example usage of AddEmployee procedure
DECLARE @NewEmployeeId INT;

EXEC SoftwareArchitecture.AddEmployee
    @Name = 'John Doe',
    @Gender = 1,
    @PhoneNumber = '123-456-7890',
    @Address = '123 Main St, Cityville',
    @DateOfBirth = '1990-01-01',
    @Email = 'johndoe@example.com',
    @Department = 'Engineering',
    @Role = 'Software Engineer',
    @NewEmployeeId = @NewEmployeeId OUTPUT;

SELECT @NewEmployeeId AS NewEmployeeId;