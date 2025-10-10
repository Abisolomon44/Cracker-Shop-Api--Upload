-------------- Company Master
------------CREATE TABLE CompanyMaster (
------------    CompanyID BIGINT PRIMARY KEY IDENTITY(1,1),
------------    CompanyCode VARCHAR(50) UNIQUE NOT NULL,
------------    CompanyName VARCHAR(200) NOT NULL,
------------    Phone VARCHAR(20),
------------    AlternatePhone VARCHAR(20),
------------    Email VARCHAR(150),
------------    Website VARCHAR(150),

------------    AddressLine1 VARCHAR(250),
------------    AddressLine2 VARCHAR(250),
------------    AddressLine3 VARCHAR(250),
------------    AddressLine4 VARCHAR(250),
------------    City VARCHAR(100),
------------    State VARCHAR(100),
------------    Country VARCHAR(100),
------------    Pincode VARCHAR(20),

------------    GSTNumber VARCHAR(50),
------------    PANNumber VARCHAR(20),
------------    CINNumber VARCHAR(50),

------------    BankName VARCHAR(150),
------------    BankAccountNumber VARCHAR(50),
------------    IFSCCode VARCHAR(20),

------------    CompanyLogo VARBINARY(MAX),   -- Store logo binary data
------------    CompanyImage VARBINARY(MAX),  -- Store company photo/image

------------    IsActive BIT DEFAULT 1,

------------    CreatedByUserID BIGINT NULL,
------------    CreatedSystemName VARCHAR(100),
------------    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),

------------    UpdatedByUserID BIGINT NULL,
------------    UpdatedSystemName VARCHAR(100),
------------    UpdatedAt DATETIME2 DEFAULT SYSDATETIME()
------------);

-------------- Branch Master
------------CREATE TABLE BranchMaster (
------------    BranchID BIGINT PRIMARY KEY IDENTITY(1,1),
------------    CompanyID BIGINT NOT NULL,
------------    BranchCode VARCHAR(50) UNIQUE NOT NULL,
------------    BranchName VARCHAR(200) NOT NULL,
------------    Address VARCHAR(500),

------------    IsActive BIT DEFAULT 1,

------------    CreatedByUserID BIGINT NULL,
------------    CreatedSystemName VARCHAR(100),
------------    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),

------------    UpdatedByUserID BIGINT NULL,
------------    UpdatedSystemName VARCHAR(100),
------------    UpdatedAt DATETIME2 DEFAULT SYSDATETIME(),

------------    FOREIGN KEY (CompanyID) REFERENCES CompanyMaster(CompanyID)
------------);

-------------- Department Master
------------CREATE TABLE DepartmentMaster (
------------    DepartmentID BIGINT PRIMARY KEY IDENTITY(1,1),
------------    BranchID BIGINT NOT NULL,
------------    DepartmentCode VARCHAR(50) UNIQUE NOT NULL,
------------    DepartmentName VARCHAR(200) NOT NULL,

------------    IsActive BIT DEFAULT 1,

------------    CreatedByUserID BIGINT NULL,
------------    CreatedSystemName VARCHAR(100),
------------    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),

------------    UpdatedByUserID BIGINT NULL,
------------    UpdatedSystemName VARCHAR(100),
------------    UpdatedAt DATETIME2 DEFAULT SYSDATETIME(),

------------    FOREIGN KEY (BranchID) REFERENCES BranchMaster(BranchID)
------------);

-------------- Role Master
------------CREATE TABLE RoleMaster (
------------    RoleID BIGINT PRIMARY KEY IDENTITY(1,1),
------------    RoleCode VARCHAR(50) UNIQUE NOT NULL,
------------    RoleName VARCHAR(200) NOT NULL,

------------    IsActive BIT DEFAULT 1,

------------    CreatedByUserID BIGINT NULL,
------------    CreatedSystemName VARCHAR(100),
------------    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),

------------    UpdatedByUserID BIGINT NULL,
------------    UpdatedSystemName VARCHAR(100),
------------    UpdatedAt DATETIME2 DEFAULT SYSDATETIME()
------------);

-------------- User Master
------------CREATE TABLE UserMaster (
------------    UserID BIGINT PRIMARY KEY IDENTITY(1,1),
------------    CompanyID BIGINT NOT NULL,
------------    BranchID BIGINT NOT NULL,
------------    DepartmentID BIGINT NOT NULL,
------------    RoleID BIGINT NOT NULL,

------------    UserName VARCHAR(150) NOT NULL,
------------    Email VARCHAR(200) UNIQUE NOT NULL,
------------    PasswordHash VARCHAR(500) NOT NULL,

------------    IsActive BIT DEFAULT 1,

------------    CreatedByUserID BIGINT NULL,
------------    CreatedSystemName VARCHAR(100),
------------    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),

------------    UpdatedByUserID BIGINT NULL,
------------    UpdatedSystemName VARCHAR(100),
------------    UpdatedAt DATETIME2 DEFAULT SYSDATETIME(),

------------    FOREIGN KEY (CompanyID) REFERENCES CompanyMaster(CompanyID),
------------    FOREIGN KEY (BranchID) REFERENCES BranchMaster(BranchID),
------------    FOREIGN KEY (DepartmentID) REFERENCES DepartmentMaster(DepartmentID),
------------    FOREIGN KEY (RoleID) REFERENCES RoleMaster(RoleID)
------------);


----------CREATE TABLE CustomerMaster (
----------    CustomerID BIGINT PRIMARY KEY IDENTITY(1,1),
----------    CustomerCode VARCHAR(50) UNIQUE NOT NULL,
----------    CustomerName VARCHAR(200) NOT NULL,
----------    Phone VARCHAR(20),
----------    AlternatePhone VARCHAR(20),
----------    Email VARCHAR(150),
----------    AddressLine1 VARCHAR(250),
----------    AddressLine2 VARCHAR(250),
----------    City VARCHAR(100),
----------    State VARCHAR(100),
----------    Country VARCHAR(100),
----------    PostalCode VARCHAR(20),
----------    IsActive BIT DEFAULT 1,

----------    CreatedByUserID BIGINT NULL,
----------    CreatedSystemName VARCHAR(100),
----------    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),
----------    UpdatedByUserID BIGINT NULL,
----------    UpdatedSystemName VARCHAR(100),
----------    UpdatedAt DATETIME2 NULL
----------);

----------CREATE TABLE SupplierMaster (
----------    SupplierID BIGINT PRIMARY KEY IDENTITY(1,1),
----------    SupplierCode VARCHAR(50) UNIQUE NOT NULL,
----------    SupplierName VARCHAR(200) NOT NULL,
----------    Phone VARCHAR(20),
----------    AlternatePhone VARCHAR(20),
----------    Email VARCHAR(150),
----------    AddressLine1 VARCHAR(250),
----------    AddressLine2 VARCHAR(250),
----------    City VARCHAR(100),
----------    State VARCHAR(100),
----------    Country VARCHAR(100),
----------    PostalCode VARCHAR(20),
----------    GSTNumber VARCHAR(50),  -- if applicable
----------    IsActive BIT DEFAULT 1,

----------    CreatedByUserID BIGINT NULL,
----------    CreatedSystemName VARCHAR(100),
----------    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),
----------    UpdatedByUserID BIGINT NULL,
----------    UpdatedSystemName VARCHAR(100),
----------    UpdatedAt DATETIME2 NULL
----------);


----------CREATE TABLE LocationMaster (
----------    LocationID BIGINT PRIMARY KEY IDENTITY(1,1),
----------    LocationCode VARCHAR(50) UNIQUE NOT NULL,
----------    LocationName VARCHAR(200) NOT NULL,
----------    AddressLine1 VARCHAR(250),
----------    AddressLine2 VARCHAR(250),
----------    City VARCHAR(100),
----------    State VARCHAR(100),
----------    Country VARCHAR(100),
----------    PostalCode VARCHAR(20),
----------    Phone VARCHAR(20),
----------    IsActive BIT DEFAULT 1,

----------    CreatedByUserID BIGINT NULL,
----------    CreatedSystemName VARCHAR(100),
----------    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),
----------    UpdatedByUserID BIGINT NULL,
----------    UpdatedSystemName VARCHAR(100),
----------    UpdatedAt DATETIME2 NULL
----------);

---------- 1. CategoryMaster
--------CREATE TABLE CategoryMaster (
--------    CategoryID BIGINT IDENTITY(1,1) PRIMARY KEY,
--------    CategoryName NVARCHAR(255) NOT NULL,
--------    Description NVARCHAR(MAX) NULL,
--------    IsActive BIT DEFAULT 1,

--------    -- Audit Fields
--------    CreatedByUserID BIGINT NULL,
--------    CreatedSystemName NVARCHAR(100) NULL,
--------    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
--------    UpdatedByUserID BIGINT NULL,
--------    UpdatedSystemName NVARCHAR(100) NULL,
--------    UpdatedAt DATETIME2 NULL
--------);

---------- 2. SubCategoryMaster
--------CREATE TABLE SubCategoryMaster (
--------    SubCategoryID BIGINT IDENTITY(1,1) PRIMARY KEY,
--------    CategoryID BIGINT NOT NULL,
--------    SubCategoryName NVARCHAR(255) NOT NULL,
--------    Description NVARCHAR(MAX) NULL,
--------    IsActive BIT DEFAULT 1,

--------    -- Audit Fields
--------    CreatedByUserID BIGINT NULL,
--------    CreatedSystemName NVARCHAR(100) NULL,
--------    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
--------    UpdatedByUserID BIGINT NULL,
--------    UpdatedSystemName NVARCHAR(100) NULL,
--------    UpdatedAt DATETIME2 NULL,

--------    CONSTRAINT FK_SubCategory_Category FOREIGN KEY (CategoryID)
--------        REFERENCES CategoryMaster(CategoryID)
--------);

---------- 3. BrandMaster
--------CREATE TABLE BrandMaster (
--------    BrandID BIGINT IDENTITY(1,1) PRIMARY KEY,
--------    BrandName NVARCHAR(255) NOT NULL,
--------    Description NVARCHAR(MAX) NULL,
--------    IsActive BIT DEFAULT 1,

--------    -- Audit Fields
--------    CreatedByUserID BIGINT NULL,
--------    CreatedSystemName NVARCHAR(100) NULL,
--------    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
--------    UpdatedByUserID BIGINT NULL,
--------    UpdatedSystemName NVARCHAR(100) NULL,
--------    UpdatedAt DATETIME2 NULL
--------);

---------- 4. UnitMaster
--------CREATE TABLE UnitMaster (
--------    UnitID BIGINT IDENTITY(1,1) PRIMARY KEY,
--------    UnitName NVARCHAR(50) NOT NULL,
--------    UnitCode NVARCHAR(20) NOT NULL,
--------    IsActive BIT DEFAULT 1,

--------    -- Audit Fields
--------    CreatedByUserID BIGINT NULL,
--------    CreatedSystemName NVARCHAR(100) NULL,
--------    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
--------    UpdatedByUserID BIGINT NULL,
--------    UpdatedSystemName NVARCHAR(100) NULL,
--------    UpdatedAt DATETIME2 NULL
--------);

---------- 5. TaxMaster
--------CREATE TABLE TaxMaster (
--------    TaxID BIGINT IDENTITY(1,1) PRIMARY KEY,
--------    TaxName NVARCHAR(100) NOT NULL,
--------    TaxRate DECIMAL(7,4) NOT NULL,
--------    CessRate DECIMAL(7,4) DEFAULT 0,
--------    IsActive BIT DEFAULT 1,

--------    -- Audit Fields
--------    CreatedByUserID BIGINT NULL,
--------    CreatedSystemName NVARCHAR(100) NULL,
--------    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
--------    UpdatedByUserID BIGINT NULL,
--------    UpdatedSystemName NVARCHAR(100) NULL,
--------    UpdatedAt DATETIME2 NULL
--------);

---------- 6. HSNCodeMaster
--------CREATE TABLE HSNCodeMaster (
--------    HSNID BIGINT IDENTITY(1,1) PRIMARY KEY,
--------    HSNCode NVARCHAR(50) NOT NULL,
--------    Description NVARCHAR(MAX) NULL,
--------    TaxID BIGINT NOT NULL,
--------    IsActive BIT DEFAULT 1,

--------    -- Audit Fields
--------    CreatedByUserID BIGINT NULL,
--------    CreatedSystemName NVARCHAR(100) NULL,
--------    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
--------    UpdatedByUserID BIGINT NULL,
--------    UpdatedSystemName NVARCHAR(100) NULL,
--------    UpdatedAt DATETIME2 NULL,

--------    CONSTRAINT FK_HSN_Tax FOREIGN KEY (TaxID) REFERENCES TaxMaster(TaxID)
--------);

---------- 7. CessMaster
--------CREATE TABLE CessMaster (
--------    CessID BIGINT IDENTITY(1,1) PRIMARY KEY,
--------    CessName NVARCHAR(100) NOT NULL,
--------    CessRate DECIMAL(7,4) NOT NULL,
--------    Description NVARCHAR(MAX) NULL,
--------    IsActive BIT DEFAULT 1,

--------    -- Audit Fields
--------    CreatedByUserID BIGINT NULL,
--------    CreatedSystemName NVARCHAR(100) NULL,
--------    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
--------    UpdatedByUserID BIGINT NULL,
--------    UpdatedSystemName NVARCHAR(100) NULL,
--------    UpdatedAt DATETIME2 NULL
--------);

---------- 8. ProductMaster
--------CREATE TABLE ProductMaster (
--------    ProductID BIGINT IDENTITY(1,1) PRIMARY KEY,
--------    ProductName NVARCHAR(255) NOT NULL,
--------    ProductCode NVARCHAR(100) UNIQUE,
--------    CategoryID BIGINT NOT NULL,
--------    SubCategoryID BIGINT NULL,
--------    BrandID BIGINT NULL,
--------    UnitID BIGINT NOT NULL,
--------    HSNID BIGINT NULL,
--------    TaxID BIGINT NOT NULL,
--------    CessID BIGINT NULL,

--------    -- Pricing Fields
--------    PurchaseRate DECIMAL(18,4) NOT NULL,    -- Cost price
--------    RetailPrice DECIMAL(18,4) NOT NULL,     -- Retail selling price
--------    WholesalePrice DECIMAL(18,4) NULL,      -- Wholesale price
--------    SaleRate DECIMAL(18,4) NULL,            -- Optional actual sale rate
--------    MRP DECIMAL(18,4) NULL,                 -- Maximum Retail Price

--------    -- Product Image

--------    IsActive BIT DEFAULT 1,

--------    -- Audit Fields
--------    CreatedByUserID BIGINT NULL,
--------    CreatedSystemName NVARCHAR(100) NULL,
--------    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
--------    UpdatedByUserID BIGINT NULL,
--------    UpdatedSystemName NVARCHAR(100) NULL,
--------    UpdatedAt DATETIME2 NULL,

--------    -- Foreign Key Constraints
--------    CONSTRAINT FK_Product_Category FOREIGN KEY (CategoryID) REFERENCES CategoryMaster(CategoryID),
--------    CONSTRAINT FK_Product_SubCategory FOREIGN KEY (SubCategoryID) REFERENCES SubCategoryMaster(SubCategoryID),
--------    CONSTRAINT FK_Product_Brand FOREIGN KEY (BrandID) REFERENCES BrandMaster(BrandID),
--------    CONSTRAINT FK_Product_Unit FOREIGN KEY (UnitID) REFERENCES UnitMaster(UnitID),
--------    CONSTRAINT FK_Product_Tax FOREIGN KEY (TaxID) REFERENCES TaxMaster(TaxID),
--------    CONSTRAINT FK_Product_HSN FOREIGN KEY (HSNID) REFERENCES HSNCodeMaster(HSNID),
--------    CONSTRAINT FK_Product_Cess FOREIGN KEY (CessID) REFERENCES CessMaster(CessID)
--------);


---------- 9. ServiceMaster
--------CREATE TABLE ServiceMaster (
--------    ServiceID BIGINT IDENTITY(1,1) PRIMARY KEY,
--------    ServiceName NVARCHAR(255) NOT NULL,
--------    ServiceCode NVARCHAR(100) UNIQUE,
--------    CategoryID BIGINT NULL,
--------    HSNID BIGINT NULL,
--------    TaxID BIGINT NOT NULL,
--------    CessID BIGINT NULL,
--------    ServiceCharge DECIMAL(18,4) NOT NULL,
--------    IsActive BIT DEFAULT 1,

--------    -- Audit Fields
--------    CreatedByUserID BIGINT NULL,
--------    CreatedSystemName NVARCHAR(100) NULL,
--------    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
--------    UpdatedByUserID BIGINT NULL,
--------    UpdatedSystemName NVARCHAR(100) NULL,
--------    UpdatedAt DATETIME2 NULL,

--------    CONSTRAINT FK_Service_HSN FOREIGN KEY (HSNID) REFERENCES HSNCodeMaster(HSNID),
--------    CONSTRAINT FK_Service_Category FOREIGN KEY (CategoryID) REFERENCES CategoryMaster(CategoryID),
--------    CONSTRAINT FK_Service_Tax FOREIGN KEY (TaxID) REFERENCES TaxMaster(TaxID),
--------    CONSTRAINT FK_Service_Cess FOREIGN KEY (CessID) REFERENCES CessMaster(CessID)
--------);


------CREATE TABLE ProductImageMaster (
------    ProductImageID BIGINT IDENTITY(1,1) PRIMARY KEY,
------    ProductID BIGINT NOT NULL,                     -- FK to ProductMaster
------    ImageData VARBINARY(MAX) NOT NULL,             -- Actual image stored in DB
------    ImageName NVARCHAR(255) NULL,                  -- Original file name
------    ContentType NVARCHAR(100) NULL,                -- MIME type (image/png, image/jpeg)
------    IsPrimary BIT DEFAULT 0,                        -- Mark main image
------    DisplayOrder INT DEFAULT 0,                     -- Order for multiple images
------    IsActive BIT DEFAULT 1,

------    -- Audit Fields
------    CreatedByUserID BIGINT NULL,
------    CreatedSystemName NVARCHAR(100) NULL,
------    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
------    UpdatedByUserID BIGINT NULL,
------    UpdatedSystemName NVARCHAR(100) NULL,
------    UpdatedAt DATETIME2 NULL,

------    CONSTRAINT FK_ProductImage_Product FOREIGN KEY (ProductID) REFERENCES ProductMaster(ProductID)
------);


----CREATE TABLE PaymentModeMaster (
----    PaymentModeID BIGINT IDENTITY(1,1) PRIMARY KEY,
----    PaymentModeName NVARCHAR(100) NOT NULL,
----    PaymentType NVARCHAR(50) NOT NULL,       -- Cash, Card, Digital Wallet, Bank Transfer, Credit
----    Description NVARCHAR(500) NULL,
----    IsActive BIT DEFAULT 1,
    
----    -- Audit Fields
----    CreatedByUserID BIGINT NULL,
----    CreatedSystemName NVARCHAR(100) NULL,
----    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
----    UpdatedByUserID BIGINT NULL,
----    UpdatedSystemName NVARCHAR(100) NULL,
----    UpdatedAt DATETIME2 NULL
----);


----CREATE TABLE InvoicePaymentDetails (
----    PaymentDetailID BIGINT IDENTITY(1,1) PRIMARY KEY,  -- Unique record
----    InvoiceID BIGINT NOT NULL,                          -- Linked to InvoiceMaster
----    PaymentModeID BIGINT NOT NULL,                      -- FK to PaymentModeMaster
----    PaymentDate DATETIME2 DEFAULT SYSUTCDATETIME(),     -- Payment timestamp

----    -- Bank Transfer Details
----    BankName NVARCHAR(200) NULL,
----    BankAccountNumber NVARCHAR(50) NULL,
----    IFSCCode NVARCHAR(20) NULL,
----    BankTransactionRef NVARCHAR(200) NULL,

----    -- Card Payment Details
----    CardType NVARCHAR(50) NULL,                         -- Credit/Debit
----    CardNumberLast4 NVARCHAR(10) NULL,
----    CardHolderName NVARCHAR(100) NULL,
----    CardTransactionRef NVARCHAR(200) NULL,

----    -- UPI / Wallet Details
----    UPIAppName NVARCHAR(50) NULL,                       -- GPay, PhonePe, Paytm
----    UPITransactionRef NVARCHAR(200) NULL,
----    UPIUserID NVARCHAR(100) NULL,

----    -- Cash Payment Notes
----    CashCounter NVARCHAR(100) NULL,
----    CashReceiptNo NVARCHAR(100) NULL,

----    -- Audit Fields
----    CreatedByUserID BIGINT NULL,
----    CreatedSystemName NVARCHAR(100) NULL,
----    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
----    UpdatedByUserID BIGINT NULL,
----    UpdatedSystemName NVARCHAR(100) NULL,
----    UpdatedAt DATETIME2 NULL,

----    -- Foreign Keys
----    CONSTRAINT FK_PaymentMode FOREIGN KEY (PaymentModeID) REFERENCES PaymentModeMaster(PaymentModeID)
----    -- CONSTRAINT FK_Invoice FOREIGN KEY (InvoiceID) REFERENCES InvoiceMaster(InvoiceID)
----);


--ALTER TABLE ProductMaster
--ADD 
--    DiscountAmount DECIMAL(18,4) NULL,          -- Flat discount amount per product
--    DiscountPercentage DECIMAL(5,2) NULL,       -- Discount % per product
--    OpeningStock DECIMAL(18,4) DEFAULT 0,       -- Initial stock
--    ReorderLevel DECIMAL(18,4) DEFAULT 0,       -- Reorder alert
--    CurrentStock DECIMAL(18,4) DEFAULT 0,       -- Current stock level
--    Barcode NVARCHAR(100) NULL,                 -- Optional barcode
--    IsService BIT DEFAULT 0,                     -- 1 if this is a service, not a product
--    ProductDescription NVARCHAR(MAX) NULL,      -- Product description
--    ProductImage VARBINARY(MAX) NULL;           -- Optional product image stored as binary


1. CompanyMaster
2. BranchMaster
3. DepartmentMaster

---

### **User & Role Management Masters**

4. RoleMaster
5. UserMaster

Customer & Supplier Masters**

6. CustomerMaster
7. SupplierMaster

8. LocationMaster
**Product & Inventory Masters**

9. CategoryMaster
10. SubCategoryMaster
11. BrandMaster
12. UnitMaster
13. TaxMaster
14. HSNCodeMaster
15. CessMaster
16. ProductMaster
17. ServiceMaster
18. ProductImageMaster

---

### **Payment & Financial Masters**

19. PaymentModeMaster

