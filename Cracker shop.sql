------ Company Master
----CREATE TABLE CompanyMaster (
----    CompanyID BIGINT PRIMARY KEY IDENTITY(1,1),
----    CompanyCode VARCHAR(50) UNIQUE NOT NULL,
----    CompanyName VARCHAR(200) NOT NULL,
----    Phone VARCHAR(20),
----    AlternatePhone VARCHAR(20),
----    Email VARCHAR(150),
----    Website VARCHAR(150),

----    AddressLine1 VARCHAR(250),
----    AddressLine2 VARCHAR(250),
----    AddressLine3 VARCHAR(250),
----    AddressLine4 VARCHAR(250),
----    City VARCHAR(100),
----    State VARCHAR(100),
----    Country VARCHAR(100),
----    Pincode VARCHAR(20),

----    GSTNumber VARCHAR(50),
----    PANNumber VARCHAR(20),
----    CINNumber VARCHAR(50),

----    BankName VARCHAR(150),
----    BankAccountNumber VARCHAR(50),
----    IFSCCode VARCHAR(20),

----    CompanyLogo VARBINARY(MAX),   -- Store logo binary data
----    CompanyImage VARBINARY(MAX),  -- Store company photo/image

----    IsActive BIT DEFAULT 1,

----    CreatedByUserID BIGINT NULL,
----    CreatedSystemName VARCHAR(100),
----    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),

----    UpdatedByUserID BIGINT NULL,
----    UpdatedSystemName VARCHAR(100),
----    UpdatedAt DATETIME2 DEFAULT SYSDATETIME()
----);

------ Branch Master
----CREATE TABLE BranchMaster (
----    BranchID BIGINT PRIMARY KEY IDENTITY(1,1),
----    CompanyID BIGINT NOT NULL,
----    BranchCode VARCHAR(50) UNIQUE NOT NULL,
----    BranchName VARCHAR(200) NOT NULL,
----    Address VARCHAR(500),

----    IsActive BIT DEFAULT 1,

----    CreatedByUserID BIGINT NULL,
----    CreatedSystemName VARCHAR(100),
----    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),

----    UpdatedByUserID BIGINT NULL,
----    UpdatedSystemName VARCHAR(100),
----    UpdatedAt DATETIME2 DEFAULT SYSDATETIME(),

----    FOREIGN KEY (CompanyID) REFERENCES CompanyMaster(CompanyID)
----);

------ Department Master
----CREATE TABLE DepartmentMaster (
----    DepartmentID BIGINT PRIMARY KEY IDENTITY(1,1),
----    BranchID BIGINT NOT NULL,
----    DepartmentCode VARCHAR(50) UNIQUE NOT NULL,
----    DepartmentName VARCHAR(200) NOT NULL,

----    IsActive BIT DEFAULT 1,

----    CreatedByUserID BIGINT NULL,
----    CreatedSystemName VARCHAR(100),
----    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),

----    UpdatedByUserID BIGINT NULL,
----    UpdatedSystemName VARCHAR(100),
----    UpdatedAt DATETIME2 DEFAULT SYSDATETIME(),

----    FOREIGN KEY (BranchID) REFERENCES BranchMaster(BranchID)
----);

------ Role Master
----CREATE TABLE RoleMaster (
----    RoleID BIGINT PRIMARY KEY IDENTITY(1,1),
----    RoleCode VARCHAR(50) UNIQUE NOT NULL,
----    RoleName VARCHAR(200) NOT NULL,

----    IsActive BIT DEFAULT 1,

----    CreatedByUserID BIGINT NULL,
----    CreatedSystemName VARCHAR(100),
----    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),

----    UpdatedByUserID BIGINT NULL,
----    UpdatedSystemName VARCHAR(100),
----    UpdatedAt DATETIME2 DEFAULT SYSDATETIME()
----);

------ User Master
----CREATE TABLE UserMaster (
----    UserID BIGINT PRIMARY KEY IDENTITY(1,1),
----    CompanyID BIGINT NOT NULL,
----    BranchID BIGINT NOT NULL,
----    DepartmentID BIGINT NOT NULL,
----    RoleID BIGINT NOT NULL,

----    UserName VARCHAR(150) NOT NULL,
----    Email VARCHAR(200) UNIQUE NOT NULL,
----    PasswordHash VARCHAR(500) NOT NULL,

----    IsActive BIT DEFAULT 1,

----    CreatedByUserID BIGINT NULL,
----    CreatedSystemName VARCHAR(100),
----    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),

----    UpdatedByUserID BIGINT NULL,
----    UpdatedSystemName VARCHAR(100),
----    UpdatedAt DATETIME2 DEFAULT SYSDATETIME(),

----    FOREIGN KEY (CompanyID) REFERENCES CompanyMaster(CompanyID),
----    FOREIGN KEY (BranchID) REFERENCES BranchMaster(BranchID),
----    FOREIGN KEY (DepartmentID) REFERENCES DepartmentMaster(DepartmentID),
----    FOREIGN KEY (RoleID) REFERENCES RoleMaster(RoleID)
----);


--CREATE TABLE CustomerMaster (
--    CustomerID BIGINT PRIMARY KEY IDENTITY(1,1),
--    CustomerCode VARCHAR(50) UNIQUE NOT NULL,
--    CustomerName VARCHAR(200) NOT NULL,
--    Phone VARCHAR(20),
--    AlternatePhone VARCHAR(20),
--    Email VARCHAR(150),
--    AddressLine1 VARCHAR(250),
--    AddressLine2 VARCHAR(250),
--    City VARCHAR(100),
--    State VARCHAR(100),
--    Country VARCHAR(100),
--    PostalCode VARCHAR(20),
--    IsActive BIT DEFAULT 1,

--    CreatedByUserID BIGINT NULL,
--    CreatedSystemName VARCHAR(100),
--    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),
--    UpdatedByUserID BIGINT NULL,
--    UpdatedSystemName VARCHAR(100),
--    UpdatedAt DATETIME2 NULL
--);

--CREATE TABLE SupplierMaster (
--    SupplierID BIGINT PRIMARY KEY IDENTITY(1,1),
--    SupplierCode VARCHAR(50) UNIQUE NOT NULL,
--    SupplierName VARCHAR(200) NOT NULL,
--    Phone VARCHAR(20),
--    AlternatePhone VARCHAR(20),
--    Email VARCHAR(150),
--    AddressLine1 VARCHAR(250),
--    AddressLine2 VARCHAR(250),
--    City VARCHAR(100),
--    State VARCHAR(100),
--    Country VARCHAR(100),
--    PostalCode VARCHAR(20),
--    GSTNumber VARCHAR(50),  -- if applicable
--    IsActive BIT DEFAULT 1,

--    CreatedByUserID BIGINT NULL,
--    CreatedSystemName VARCHAR(100),
--    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),
--    UpdatedByUserID BIGINT NULL,
--    UpdatedSystemName VARCHAR(100),
--    UpdatedAt DATETIME2 NULL
--);


--CREATE TABLE LocationMaster (
--    LocationID BIGINT PRIMARY KEY IDENTITY(1,1),
--    LocationCode VARCHAR(50) UNIQUE NOT NULL,
--    LocationName VARCHAR(200) NOT NULL,
--    AddressLine1 VARCHAR(250),
--    AddressLine2 VARCHAR(250),
--    City VARCHAR(100),
--    State VARCHAR(100),
--    Country VARCHAR(100),
--    PostalCode VARCHAR(20),
--    Phone VARCHAR(20),
--    IsActive BIT DEFAULT 1,

--    CreatedByUserID BIGINT NULL,
--    CreatedSystemName VARCHAR(100),
--    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),
--    UpdatedByUserID BIGINT NULL,
--    UpdatedSystemName VARCHAR(100),
--    UpdatedAt DATETIME2 NULL
--);
