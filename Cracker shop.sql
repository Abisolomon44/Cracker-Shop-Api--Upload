------------------ Company Master
----------------CREATE TABLE CompanyMaster (
----------------    CompanyID BIGINT PRIMARY KEY IDENTITY(1,1),
----------------    CompanyCode VARCHAR(50) UNIQUE NOT NULL,
----------------    CompanyName VARCHAR(200) NOT NULL,
----------------    Phone VARCHAR(20),
----------------    AlternatePhone VARCHAR(20),
----------------    Email VARCHAR(150),
----------------    Website VARCHAR(150),

----------------    AddressLine1 VARCHAR(250),
----------------    AddressLine2 VARCHAR(250),
----------------    AddressLine3 VARCHAR(250),
----------------    AddressLine4 VARCHAR(250),
----------------    City VARCHAR(100),
----------------    State VARCHAR(100),
----------------    Country VARCHAR(100),
----------------    Pincode VARCHAR(20),

----------------    GSTNumber VARCHAR(50),
----------------    PANNumber VARCHAR(20),
----------------    CINNumber VARCHAR(50),

----------------    BankName VARCHAR(150),
----------------    BankAccountNumber VARCHAR(50),
----------------    IFSCCode VARCHAR(20),

----------------    CompanyLogo VARBINARY(MAX),   -- Store logo binary data
----------------    CompanyImage VARBINARY(MAX),  -- Store company photo/image

----------------    IsActive BIT DEFAULT 1,

----------------    CreatedByUserID BIGINT NULL,
----------------    CreatedSystemName VARCHAR(100),
----------------    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),

----------------    UpdatedByUserID BIGINT NULL,
----------------    UpdatedSystemName VARCHAR(100),
----------------    UpdatedAt DATETIME2 DEFAULT SYSDATETIME()
----------------);

------------------ Branch Master
----------------CREATE TABLE BranchMaster (
----------------    BranchID BIGINT PRIMARY KEY IDENTITY(1,1),
----------------    CompanyID BIGINT NOT NULL,
----------------    BranchCode VARCHAR(50) UNIQUE NOT NULL,
----------------    BranchName VARCHAR(200) NOT NULL,
----------------    Address VARCHAR(500),

----------------    IsActive BIT DEFAULT 1,

----------------    CreatedByUserID BIGINT NULL,
----------------    CreatedSystemName VARCHAR(100),
----------------    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),

----------------    UpdatedByUserID BIGINT NULL,
----------------    UpdatedSystemName VARCHAR(100),
----------------    UpdatedAt DATETIME2 DEFAULT SYSDATETIME(),

----------------    FOREIGN KEY (CompanyID) REFERENCES CompanyMaster(CompanyID)
----------------);

------------------ Department Master
----------------CREATE TABLE DepartmentMaster (
----------------    DepartmentID BIGINT PRIMARY KEY IDENTITY(1,1),
----------------    BranchID BIGINT NOT NULL,
----------------    DepartmentCode VARCHAR(50) UNIQUE NOT NULL,
----------------    DepartmentName VARCHAR(200) NOT NULL,

----------------    IsActive BIT DEFAULT 1,

----------------    CreatedByUserID BIGINT NULL,
----------------    CreatedSystemName VARCHAR(100),
----------------    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),

----------------    UpdatedByUserID BIGINT NULL,
----------------    UpdatedSystemName VARCHAR(100),
----------------    UpdatedAt DATETIME2 DEFAULT SYSDATETIME(),

----------------    FOREIGN KEY (BranchID) REFERENCES BranchMaster(BranchID)
----------------);

------------------ Role Master
----------------CREATE TABLE RoleMaster (
----------------    RoleID BIGINT PRIMARY KEY IDENTITY(1,1),
----------------    RoleCode VARCHAR(50) UNIQUE NOT NULL,
----------------    RoleName VARCHAR(200) NOT NULL,

----------------    IsActive BIT DEFAULT 1,

----------------    CreatedByUserID BIGINT NULL,
----------------    CreatedSystemName VARCHAR(100),
----------------    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),

----------------    UpdatedByUserID BIGINT NULL,
----------------    UpdatedSystemName VARCHAR(100),
----------------    UpdatedAt DATETIME2 DEFAULT SYSDATETIME()
----------------);

------------------ User Master
----------------CREATE TABLE UserMaster (
----------------    UserID BIGINT PRIMARY KEY IDENTITY(1,1),
----------------    CompanyID BIGINT NOT NULL,
----------------    BranchID BIGINT NOT NULL,
----------------    DepartmentID BIGINT NOT NULL,
----------------    RoleID BIGINT NOT NULL,

----------------    UserName VARCHAR(150) NOT NULL,
----------------    Email VARCHAR(200) UNIQUE NOT NULL,
----------------    PasswordHash VARCHAR(500) NOT NULL,

----------------    IsActive BIT DEFAULT 1,

----------------    CreatedByUserID BIGINT NULL,
----------------    CreatedSystemName VARCHAR(100),
----------------    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),

----------------    UpdatedByUserID BIGINT NULL,
----------------    UpdatedSystemName VARCHAR(100),
----------------    UpdatedAt DATETIME2 DEFAULT SYSDATETIME(),

----------------    FOREIGN KEY (CompanyID) REFERENCES CompanyMaster(CompanyID),
----------------    FOREIGN KEY (BranchID) REFERENCES BranchMaster(BranchID),
----------------    FOREIGN KEY (DepartmentID) REFERENCES DepartmentMaster(DepartmentID),
----------------    FOREIGN KEY (RoleID) REFERENCES RoleMaster(RoleID)
----------------);


--------------CREATE TABLE CustomerMaster (
--------------    CustomerID BIGINT PRIMARY KEY IDENTITY(1,1),
--------------    CustomerCode VARCHAR(50) UNIQUE NOT NULL,
--------------    CustomerName VARCHAR(200) NOT NULL,
--------------    Phone VARCHAR(20),
--------------    AlternatePhone VARCHAR(20),
--------------    Email VARCHAR(150),
--------------    AddressLine1 VARCHAR(250),
--------------    AddressLine2 VARCHAR(250),
--------------    City VARCHAR(100),
--------------    State VARCHAR(100),
--------------    Country VARCHAR(100),
--------------    PostalCode VARCHAR(20),
--------------    IsActive BIT DEFAULT 1,

--------------    CreatedByUserID BIGINT NULL,
--------------    CreatedSystemName VARCHAR(100),
--------------    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),
--------------    UpdatedByUserID BIGINT NULL,
--------------    UpdatedSystemName VARCHAR(100),
--------------    UpdatedAt DATETIME2 NULL
--------------);

--------------CREATE TABLE SupplierMaster (
--------------    SupplierID BIGINT PRIMARY KEY IDENTITY(1,1),
--------------    SupplierCode VARCHAR(50) UNIQUE NOT NULL,
--------------    SupplierName VARCHAR(200) NOT NULL,
--------------    Phone VARCHAR(20),
--------------    AlternatePhone VARCHAR(20),
--------------    Email VARCHAR(150),
--------------    AddressLine1 VARCHAR(250),
--------------    AddressLine2 VARCHAR(250),
--------------    City VARCHAR(100),
--------------    State VARCHAR(100),
--------------    Country VARCHAR(100),
--------------    PostalCode VARCHAR(20),
--------------    GSTNumber VARCHAR(50),  -- if applicable
--------------    IsActive BIT DEFAULT 1,

--------------    CreatedByUserID BIGINT NULL,
--------------    CreatedSystemName VARCHAR(100),
--------------    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),
--------------    UpdatedByUserID BIGINT NULL,
--------------    UpdatedSystemName VARCHAR(100),
--------------    UpdatedAt DATETIME2 NULL
--------------);


--------------CREATE TABLE LocationMaster (
--------------    LocationID BIGINT PRIMARY KEY IDENTITY(1,1),
--------------    LocationCode VARCHAR(50) UNIQUE NOT NULL,
--------------    LocationName VARCHAR(200) NOT NULL,
--------------    AddressLine1 VARCHAR(250),
--------------    AddressLine2 VARCHAR(250),
--------------    City VARCHAR(100),
--------------    State VARCHAR(100),
--------------    Country VARCHAR(100),
--------------    PostalCode VARCHAR(20),
--------------    Phone VARCHAR(20),
--------------    IsActive BIT DEFAULT 1,

--------------    CreatedByUserID BIGINT NULL,
--------------    CreatedSystemName VARCHAR(100),
--------------    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),
--------------    UpdatedByUserID BIGINT NULL,
--------------    UpdatedSystemName VARCHAR(100),
--------------    UpdatedAt DATETIME2 NULL
--------------);

-------------- 1. CategoryMaster
------------CREATE TABLE CategoryMaster (
------------    CategoryID BIGINT IDENTITY(1,1) PRIMARY KEY,
------------    CategoryName NVARCHAR(255) NOT NULL,
------------    Description NVARCHAR(MAX) NULL,
------------    IsActive BIT DEFAULT 1,

------------    -- Audit Fields
------------    CreatedByUserID BIGINT NULL,
------------    CreatedSystemName NVARCHAR(100) NULL,
------------    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
------------    UpdatedByUserID BIGINT NULL,
------------    UpdatedSystemName NVARCHAR(100) NULL,
------------    UpdatedAt DATETIME2 NULL
------------);

-------------- 2. SubCategoryMaster
------------CREATE TABLE SubCategoryMaster (
------------    SubCategoryID BIGINT IDENTITY(1,1) PRIMARY KEY,
------------    CategoryID BIGINT NOT NULL,
------------    SubCategoryName NVARCHAR(255) NOT NULL,
------------    Description NVARCHAR(MAX) NULL,
------------    IsActive BIT DEFAULT 1,

------------    -- Audit Fields
------------    CreatedByUserID BIGINT NULL,
------------    CreatedSystemName NVARCHAR(100) NULL,
------------    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
------------    UpdatedByUserID BIGINT NULL,
------------    UpdatedSystemName NVARCHAR(100) NULL,
------------    UpdatedAt DATETIME2 NULL,

------------    CONSTRAINT FK_SubCategory_Category FOREIGN KEY (CategoryID)
------------        REFERENCES CategoryMaster(CategoryID)
------------);

-------------- 3. BrandMaster
------------CREATE TABLE BrandMaster (
------------    BrandID BIGINT IDENTITY(1,1) PRIMARY KEY,
------------    BrandName NVARCHAR(255) NOT NULL,
------------    Description NVARCHAR(MAX) NULL,
------------    IsActive BIT DEFAULT 1,

------------    -- Audit Fields
------------    CreatedByUserID BIGINT NULL,
------------    CreatedSystemName NVARCHAR(100) NULL,
------------    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
------------    UpdatedByUserID BIGINT NULL,
------------    UpdatedSystemName NVARCHAR(100) NULL,
------------    UpdatedAt DATETIME2 NULL
------------);

-------------- 4. UnitMaster
------------CREATE TABLE UnitMaster (
------------    UnitID BIGINT IDENTITY(1,1) PRIMARY KEY,
------------    UnitName NVARCHAR(50) NOT NULL,
------------    UnitCode NVARCHAR(20) NOT NULL,
------------    IsActive BIT DEFAULT 1,

------------    -- Audit Fields
------------    CreatedByUserID BIGINT NULL,
------------    CreatedSystemName NVARCHAR(100) NULL,
------------    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
------------    UpdatedByUserID BIGINT NULL,
------------    UpdatedSystemName NVARCHAR(100) NULL,
------------    UpdatedAt DATETIME2 NULL
------------);

-------------- 5. TaxMaster
------------CREATE TABLE TaxMaster (
------------    TaxID BIGINT IDENTITY(1,1) PRIMARY KEY,
------------    TaxName NVARCHAR(100) NOT NULL,
------------    TaxRate DECIMAL(7,4) NOT NULL,
------------    CessRate DECIMAL(7,4) DEFAULT 0,
------------    IsActive BIT DEFAULT 1,

------------    -- Audit Fields
------------    CreatedByUserID BIGINT NULL,
------------    CreatedSystemName NVARCHAR(100) NULL,
------------    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
------------    UpdatedByUserID BIGINT NULL,
------------    UpdatedSystemName NVARCHAR(100) NULL,
------------    UpdatedAt DATETIME2 NULL
------------);

-------------- 6. HSNCodeMaster
------------CREATE TABLE HSNCodeMaster (
------------    HSNID BIGINT IDENTITY(1,1) PRIMARY KEY,
------------    HSNCode NVARCHAR(50) NOT NULL,
------------    Description NVARCHAR(MAX) NULL,
------------    TaxID BIGINT NOT NULL,
------------    IsActive BIT DEFAULT 1,

------------    -- Audit Fields
------------    CreatedByUserID BIGINT NULL,
------------    CreatedSystemName NVARCHAR(100) NULL,
------------    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
------------    UpdatedByUserID BIGINT NULL,
------------    UpdatedSystemName NVARCHAR(100) NULL,
------------    UpdatedAt DATETIME2 NULL,

------------    CONSTRAINT FK_HSN_Tax FOREIGN KEY (TaxID) REFERENCES TaxMaster(TaxID)
------------);

-------------- 7. CessMaster
------------CREATE TABLE CessMaster (
------------    CessID BIGINT IDENTITY(1,1) PRIMARY KEY,
------------    CessName NVARCHAR(100) NOT NULL,
------------    CessRate DECIMAL(7,4) NOT NULL,
------------    Description NVARCHAR(MAX) NULL,
------------    IsActive BIT DEFAULT 1,

------------    -- Audit Fields
------------    CreatedByUserID BIGINT NULL,
------------    CreatedSystemName NVARCHAR(100) NULL,
------------    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
------------    UpdatedByUserID BIGINT NULL,
------------    UpdatedSystemName NVARCHAR(100) NULL,
------------    UpdatedAt DATETIME2 NULL
------------);

-------------- 8. ProductMaster
------------CREATE TABLE ProductMaster (
------------    ProductID BIGINT IDENTITY(1,1) PRIMARY KEY,
------------    ProductName NVARCHAR(255) NOT NULL,
------------    ProductCode NVARCHAR(100) UNIQUE,
------------    CategoryID BIGINT NOT NULL,
------------    SubCategoryID BIGINT NULL,
------------    BrandID BIGINT NULL,
------------    UnitID BIGINT NOT NULL,
------------    HSNID BIGINT NULL,
------------    TaxID BIGINT NOT NULL,
------------    CessID BIGINT NULL,

------------    -- Pricing Fields
------------    PurchaseRate DECIMAL(18,4) NOT NULL,    -- Cost price
------------    RetailPrice DECIMAL(18,4) NOT NULL,     -- Retail selling price
------------    WholesalePrice DECIMAL(18,4) NULL,      -- Wholesale price
------------    SaleRate DECIMAL(18,4) NULL,            -- Optional actual sale rate
------------    MRP DECIMAL(18,4) NULL,                 -- Maximum Retail Price

------------    -- Product Image

------------    IsActive BIT DEFAULT 1,

------------    -- Audit Fields
------------    CreatedByUserID BIGINT NULL,
------------    CreatedSystemName NVARCHAR(100) NULL,
------------    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
------------    UpdatedByUserID BIGINT NULL,
------------    UpdatedSystemName NVARCHAR(100) NULL,
------------    UpdatedAt DATETIME2 NULL,

------------    -- Foreign Key Constraints
------------    CONSTRAINT FK_Product_Category FOREIGN KEY (CategoryID) REFERENCES CategoryMaster(CategoryID),
------------    CONSTRAINT FK_Product_SubCategory FOREIGN KEY (SubCategoryID) REFERENCES SubCategoryMaster(SubCategoryID),
------------    CONSTRAINT FK_Product_Brand FOREIGN KEY (BrandID) REFERENCES BrandMaster(BrandID),
------------    CONSTRAINT FK_Product_Unit FOREIGN KEY (UnitID) REFERENCES UnitMaster(UnitID),
------------    CONSTRAINT FK_Product_Tax FOREIGN KEY (TaxID) REFERENCES TaxMaster(TaxID),
------------    CONSTRAINT FK_Product_HSN FOREIGN KEY (HSNID) REFERENCES HSNCodeMaster(HSNID),
------------    CONSTRAINT FK_Product_Cess FOREIGN KEY (CessID) REFERENCES CessMaster(CessID)
------------);


-------------- 9. ServiceMaster
------------CREATE TABLE ServiceMaster (
------------    ServiceID BIGINT IDENTITY(1,1) PRIMARY KEY,
------------    ServiceName NVARCHAR(255) NOT NULL,
------------    ServiceCode NVARCHAR(100) UNIQUE,
------------    CategoryID BIGINT NULL,
------------    HSNID BIGINT NULL,
------------    TaxID BIGINT NOT NULL,
------------    CessID BIGINT NULL,
------------    ServiceCharge DECIMAL(18,4) NOT NULL,
------------    IsActive BIT DEFAULT 1,

------------    -- Audit Fields
------------    CreatedByUserID BIGINT NULL,
------------    CreatedSystemName NVARCHAR(100) NULL,
------------    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
------------    UpdatedByUserID BIGINT NULL,
------------    UpdatedSystemName NVARCHAR(100) NULL,
------------    UpdatedAt DATETIME2 NULL,

------------    CONSTRAINT FK_Service_HSN FOREIGN KEY (HSNID) REFERENCES HSNCodeMaster(HSNID),
------------    CONSTRAINT FK_Service_Category FOREIGN KEY (CategoryID) REFERENCES CategoryMaster(CategoryID),
------------    CONSTRAINT FK_Service_Tax FOREIGN KEY (TaxID) REFERENCES TaxMaster(TaxID),
------------    CONSTRAINT FK_Service_Cess FOREIGN KEY (CessID) REFERENCES CessMaster(CessID)
------------);


----------CREATE TABLE ProductImageMaster (
----------    ProductImageID BIGINT IDENTITY(1,1) PRIMARY KEY,
----------    ProductID BIGINT NOT NULL,                     -- FK to ProductMaster
----------    ImageData VARBINARY(MAX) NOT NULL,             -- Actual image stored in DB
----------    ImageName NVARCHAR(255) NULL,                  -- Original file name
----------    ContentType NVARCHAR(100) NULL,                -- MIME type (image/png, image/jpeg)
----------    IsPrimary BIT DEFAULT 0,                        -- Mark main image
----------    DisplayOrder INT DEFAULT 0,                     -- Order for multiple images
----------    IsActive BIT DEFAULT 1,

----------    -- Audit Fields
----------    CreatedByUserID BIGINT NULL,
----------    CreatedSystemName NVARCHAR(100) NULL,
----------    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
----------    UpdatedByUserID BIGINT NULL,
----------    UpdatedSystemName NVARCHAR(100) NULL,
----------    UpdatedAt DATETIME2 NULL,

----------    CONSTRAINT FK_ProductImage_Product FOREIGN KEY (ProductID) REFERENCES ProductMaster(ProductID)
----------);


--------CREATE TABLE PaymentModeMaster (
--------    PaymentModeID BIGINT IDENTITY(1,1) PRIMARY KEY,
--------    PaymentModeName NVARCHAR(100) NOT NULL,
--------    PaymentType NVARCHAR(50) NOT NULL,       -- Cash, Card, Digital Wallet, Bank Transfer, Credit
--------    Description NVARCHAR(500) NULL,
--------    IsActive BIT DEFAULT 1,
    
--------    -- Audit Fields
--------    CreatedByUserID BIGINT NULL,
--------    CreatedSystemName NVARCHAR(100) NULL,
--------    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
--------    UpdatedByUserID BIGINT NULL,
--------    UpdatedSystemName NVARCHAR(100) NULL,
--------    UpdatedAt DATETIME2 NULL
--------);


--------CREATE TABLE InvoicePaymentDetails (
--------    PaymentDetailID BIGINT IDENTITY(1,1) PRIMARY KEY,  -- Unique record
--------    InvoiceID BIGINT NOT NULL,                          -- Linked to InvoiceMaster
--------    PaymentModeID BIGINT NOT NULL,                      -- FK to PaymentModeMaster
--------    PaymentDate DATETIME2 DEFAULT SYSUTCDATETIME(),     -- Payment timestamp

--------    -- Bank Transfer Details
--------    BankName NVARCHAR(200) NULL,
--------    BankAccountNumber NVARCHAR(50) NULL,
--------    IFSCCode NVARCHAR(20) NULL,
--------    BankTransactionRef NVARCHAR(200) NULL,

--------    -- Card Payment Details
--------    CardType NVARCHAR(50) NULL,                         -- Credit/Debit
--------    CardNumberLast4 NVARCHAR(10) NULL,
--------    CardHolderName NVARCHAR(100) NULL,
--------    CardTransactionRef NVARCHAR(200) NULL,

--------    -- UPI / Wallet Details
--------    UPIAppName NVARCHAR(50) NULL,                       -- GPay, PhonePe, Paytm
--------    UPITransactionRef NVARCHAR(200) NULL,
--------    UPIUserID NVARCHAR(100) NULL,

--------    -- Cash Payment Notes
--------    CashCounter NVARCHAR(100) NULL,
--------    CashReceiptNo NVARCHAR(100) NULL,

--------    -- Audit Fields
--------    CreatedByUserID BIGINT NULL,
--------    CreatedSystemName NVARCHAR(100) NULL,
--------    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
--------    UpdatedByUserID BIGINT NULL,
--------    UpdatedSystemName NVARCHAR(100) NULL,
--------    UpdatedAt DATETIME2 NULL,

--------    -- Foreign Keys
--------    CONSTRAINT FK_PaymentMode FOREIGN KEY (PaymentModeID) REFERENCES PaymentModeMaster(PaymentModeID)
--------    -- CONSTRAINT FK_Invoice FOREIGN KEY (InvoiceID) REFERENCES InvoiceMaster(InvoiceID)
--------);


------ALTER TABLE ProductMaster
------ADD 
------    DiscountAmount DECIMAL(18,4) NULL,          -- Flat discount amount per product
------    DiscountPercentage DECIMAL(5,2) NULL,       -- Discount % per product
------    OpeningStock DECIMAL(18,4) DEFAULT 0,       -- Initial stock
------    ReorderLevel DECIMAL(18,4) DEFAULT 0,       -- Reorder alert
------    CurrentStock DECIMAL(18,4) DEFAULT 0,       -- Current stock level
------    Barcode NVARCHAR(100) NULL,                 -- Optional barcode
------    IsService BIT DEFAULT 0,                     -- 1 if this is a service, not a product
------    ProductDescription NVARCHAR(MAX) NULL,      -- Product description
------    ProductImage VARBINARY(MAX) NULL;           -- Optional product image stored as binary





----CREATE TABLE [CrackerShop].[dbo].[SalesOrder] (
----    SalesID BIGINT IDENTITY(1,1) PRIMARY KEY,
----    SONumber NVARCHAR(50) NOT NULL,
----    SalesDate DATETIME DEFAULT GETDATE(),

----    CompanyID BIGINT NOT NULL,
----    CompanyName NVARCHAR(200) NULL,
----    BranchID BIGINT NOT NULL,
----    BranchName NVARCHAR(200) NULL,

----    CustomerID BIGINT NOT NULL,
----    CustomerName NVARCHAR(200) NULL,
----    CustomerContact NVARCHAR(50) NULL,
----    CustomerAddress NVARCHAR(MAX) NULL,

----    StatusID INT DEFAULT 0,
----    StatusName NVARCHAR(50) NULL,

----    InvoiceNumber NVARCHAR(50) NULL,
----    InvoiceDate DATETIME NULL,

----    BrandID BIGINT NULL,
----    UnitID BIGINT NULL,
----    HSNID BIGINT NULL,
----    CategoryID BIGINT NULL,
----    SubCategoryID BIGINT NULL,

----    Barcode NVARCHAR(100) NULL,
----    ProductCode NVARCHAR(100) NULL,
----    ProductName NVARCHAR(200) NULL,

----    ProductRate DECIMAL(18,2) DEFAULT 0,
----    Quantity DECIMAL(18,3) DEFAULT 0,
----    SaleRate DECIMAL(18,2) DEFAULT 0,
----    RetailPrice DECIMAL(18,2) DEFAULT 0,
----    WholesalePrice DECIMAL(18,2) DEFAULT 0,
----    MRP DECIMAL(18,2) DEFAULT 0,
----    DiscountAmount DECIMAL(18,2) DEFAULT 0,
----    DiscountPercentage DECIMAL(5,2) DEFAULT 0,

----    InclusiveAmount DECIMAL(18,2) DEFAULT 0,
----    ExclusiveAmount DECIMAL(18,2) DEFAULT 0,
----    GstPercentage DECIMAL(5,2) DEFAULT 0,
----    GstAmount DECIMAL(18,2) DEFAULT 0,
----    CGSTRate DECIMAL(5,2) DEFAULT 0,
----    CGSTAmount DECIMAL(18,2) DEFAULT 0,
----    SGSTRate DECIMAL(5,2) DEFAULT 0,
----    SGSTAmount DECIMAL(18,2) DEFAULT 0,
----    IGSTRate DECIMAL(5,2) DEFAULT 0,
----    IGSTAmount DECIMAL(18,2) DEFAULT 0,
----    CESSRate DECIMAL(5,2) DEFAULT 0,
----    CESSAmount DECIMAL(18,2) DEFAULT 0,
----    TaxableValue DECIMAL(18,2) DEFAULT 0,
----    IsGSTInclusive BIT DEFAULT 0,

----    OrderedQuantity DECIMAL(18,3) DEFAULT 0,
----    DeliveredQuantity DECIMAL(18,3) DEFAULT 0,
----    ReturnedQuantity DECIMAL(18,3) DEFAULT 0,
----    RemainingQuantity DECIMAL(18,3) DEFAULT 0,
----    OpeningStock DECIMAL(18,3) DEFAULT 0,
----    ReorderLevel DECIMAL(18,3) DEFAULT 0,
----    CurrentStock DECIMAL(18,3) DEFAULT 0,

----    Color NVARCHAR(50) NULL,
----    Size NVARCHAR(50) NULL,
----    Weight DECIMAL(18,3) NULL,
----    Volume DECIMAL(18,3) NULL,
----    Material NVARCHAR(50) NULL,
----    FinishType NVARCHAR(50) NULL,
----    ShadeCode NVARCHAR(50) NULL,
----    Capacity NVARCHAR(50) NULL,
----    ModelNumber NVARCHAR(50) NULL,
----    ExpiryDate DATETIME NULL,
----    IsService BIT DEFAULT 0,

----    TotalAmount DECIMAL(18,2) DEFAULT 0,
----    TaxAmount DECIMAL(18,2) DEFAULT 0,
----    GrandTotal DECIMAL(18,2) DEFAULT 0,

----    Remarks NVARCHAR(MAX) NULL,
----    IsActive BIT DEFAULT 1,

----    CreatedByUserID BIGINT NULL,
----    CreatedSystemName NVARCHAR(100) NULL,
----    CreatedAt DATETIME DEFAULT GETDATE(),
----    UpdatedByUserID BIGINT NULL,
----    UpdatedSystemName NVARCHAR(100) NULL,
----    UpdatedAt DATETIME NULL,

----    DeliveryNumber NVARCHAR(50) NULL,
----    DeliveryDate DATETIME NULL,
----    PaymentMode NVARCHAR(50) NULL,
----    PaidDays INT NULL,
----    TaxType NVARCHAR(50) NULL,
----    SecondaryUnitID BIGINT NULL
----);










--CREATE TABLE SalesEntryMaster (
--    InvoiceID BIGINT IDENTITY(1,1) PRIMARY KEY,
--    InvoiceNumber NVARCHAR(50) NOT NULL,
--    InvoiceDate DATETIME DEFAULT GETDATE(),
--    CompanyID BIGINT NOT NULL,
--    CompanyName NVARCHAR(200) NULL,
--    BranchID BIGINT NOT NULL,
--    BranchName NVARCHAR(200) NULL,
--    CustomerID BIGINT NOT NULL,
--    CustomerName NVARCHAR(200) NULL,
--    CustomerContact NVARCHAR(50) NULL,
--    CustomerGSTIN NVARCHAR(20) NULL,
--    CustomerState NVARCHAR(100) NULL,      
--    CompanyState NVARCHAR(100) NULL,        
--    AccountingYear NVARCHAR(100) NULL, 
--    BillingType NVARCHAR(100) NULL, 
--	IsGSTApplicable BIT DEFAULT 1, 
--	GSTType NVARCHAR(20) NULL,
--    ProductID BIGINT NOT NULL,
--    Barcode NVARCHAR(100) NULL,
--    ProductCode NVARCHAR(100) NULL,
--    ProductName NVARCHAR(200) NULL,
--    BrandID BIGINT NULL,
--    CategoryID BIGINT NULL,
--    SubCategoryID BIGINT NULL,
--    HSNID BIGINT NULL,
--    UnitID BIGINT NULL,
--    SecondaryUnitID BIGINT NULL,
--    Color NVARCHAR(50) NULL,
--    Size NVARCHAR(50) NULL,
--    Weight DECIMAL(18,3) NULL,
--    Volume DECIMAL(18,3) NULL,
--    Material NVARCHAR(50) NULL,
--    FinishType NVARCHAR(50) NULL,
--    ShadeCode NVARCHAR(50) NULL,
--    Capacity NVARCHAR(50) NULL,
--    ModelNumber NVARCHAR(50) NULL,
--    ExpiryDate DATETIME NULL,
--	ManufacturingDate DATETIME NULL,
--    Quantity DECIMAL(18,3) NOT NULL,
--    ProductRate DECIMAL(18,2) DEFAULT 0,
--    SaleRate DECIMAL(18,2) DEFAULT 0,
--    RetailPrice DECIMAL(18,2) DEFAULT 0,
--    WholesalePrice DECIMAL(18,2) DEFAULT 0,
--    MRP DECIMAL(18,2) DEFAULT 0,
--    DiscountPercentage DECIMAL(5,2) DEFAULT 0,
--    DiscountAmount DECIMAL(18,2) DEFAULT 0,
--    InclusiveAmount DECIMAL(18,2) DEFAULT 0,
--    ExclusiveAmount DECIMAL(18,2) DEFAULT 0,
--    GstPercentage DECIMAL(5,2) DEFAULT 0,
--    GstAmount DECIMAL(18,2) DEFAULT 0,
--    CGSTRate DECIMAL(5,2) DEFAULT 0,
--    CGSTAmount DECIMAL(18,2) DEFAULT 0,
--    SGSTRate DECIMAL(5,2) DEFAULT 0,
--    SGSTAmount DECIMAL(18,2) DEFAULT 0,
--    IGSTRate DECIMAL(5,2) DEFAULT 0,
--    IGSTAmount DECIMAL(18,2) DEFAULT 0,
--    CESSRate DECIMAL(5,2) DEFAULT 0,
--    CESSAmount DECIMAL(18,2) DEFAULT 0,

--    GrossAmount DECIMAL(18,2) DEFAULT 0,    
--    CustomerDiscount DECIMAL(18,2) DEFAULT 0, 
--    NetAmount DECIMAL(18,2) DEFAULT 0,     
--    TaxableAmount DECIMAL(18,2) DEFAULT 0,
--    GrandTotal DECIMAL(18,2) DEFAULT 0, 


--    BillingMode NVARCHAR(50),
--    CashAmount DECIMAL(18,2) DEFAULT 0,     
--    CardAmount DECIMAL(18,2) DEFAULT 0,   
--    UPIAmount DECIMAL(18,2) DEFAULT 0,     
--    AdvanceAmount DECIMAL(18,2) DEFAULT 0, 
--    PaidAmount DECIMAL(18,2) DEFAULT 0,  
--    BalanceAmount DECIMAL(18,2) DEFAULT 0, 
--    Status NVARCHAR(50),
--    IsActive BIT DEFAULT 0,
--    IsService BIT DEFAULT 0,

--    Remarks NVARCHAR(MAX) NULL,
--    CreatedBy NVARCHAR(100) NULL,
--    CreatedDate DATETIME DEFAULT GETDATE(),
--    UpdatedBy NVARCHAR(100) NULL,
--    UpdatedDate DATETIME NULL
--);









CREATE TABLE EstimationInvoiceTotal (
    InvoiceID BIGINT IDENTITY(1,1) PRIMARY KEY,
    InvoiceNumber NVARCHAR(50) NOT NULL,
    InvoiceDate DATETIME DEFAULT GETDATE(),
    CompanyID BIGINT NOT NULL,
    CompanyName NVARCHAR(200) NULL,
    BranchID BIGINT NOT NULL,
    BranchName NVARCHAR(200) NULL,
    CustomerID BIGINT NOT NULL,
    CustomerName NVARCHAR(200) NULL,
    CustomerContact NVARCHAR(50) NULL,
    CustomerGSTIN NVARCHAR(20) NULL,
    CustomerState NVARCHAR(100) NULL,      
    CompanyState NVARCHAR(100) NULL,        
    AccountingYear NVARCHAR(100) NULL, 
    BillingType NVARCHAR(100) NULL, 
    IsGSTApplicable BIT DEFAULT 1, 
    GSTType NVARCHAR(20) NULL,

    -- Totals
    TotalQuantity DECIMAL(18,3) DEFAULT 0,
    TotalSaleRate DECIMAL(18,2) DEFAULT 0,
    TotalDiscountAmount DECIMAL(18,2) DEFAULT 0,
    TotalCGSTAmount DECIMAL(18,2) DEFAULT 0,
    TotalSGSTAmount DECIMAL(18,2) DEFAULT 0,
    TotalIGSTAmount DECIMAL(18,2) DEFAULT 0,
    TotalCESSAmount DECIMAL(18,2) DEFAULT 0,
    TotalGrossAmount DECIMAL(18,2) DEFAULT 0,
    TotalTaxableAmount DECIMAL(18,2) DEFAULT 0,
    GrandTotal DECIMAL(18,2) DEFAULT 0,

    -- Payment
    BillingMode NVARCHAR(50),
    CashAmount DECIMAL(18,2) DEFAULT 0,     
    CardAmount DECIMAL(18,2) DEFAULT 0,   
    UPIAmount DECIMAL(18,2) DEFAULT 0,     
    AdvanceAmount DECIMAL(18,2) DEFAULT 0, 
    PaidAmount DECIMAL(18,2) DEFAULT 0,  
    BalanceAmount DECIMAL(18,2) DEFAULT 0, 

    Status NVARCHAR(50),
    IsActive BIT DEFAULT 0,
    IsService BIT DEFAULT 0,

    Remarks NVARCHAR(MAX) NULL,
    CreatedBy NVARCHAR(100) NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedBy NVARCHAR(100) NULL,
    UpdatedDate DATETIME NULL
);

















----1. CompanyMaster
----2. BranchMaster
----3. DepartmentMaster

-------

----### **User & Role Management Masters**

----4. RoleMaster
----5. UserMaster

----Customer & Supplier Masters**

----6. CustomerMaster
----7. SupplierMaster

----8. LocationMaster
----**Product & Inventory Masters**

----9. CategoryMaster
----10. SubCategoryMaster
----11. BrandMaster
----12. UnitMaster
----13. TaxMaster
----14. HSNCodeMaster
----15. CessMaster
----16. ProductMaster
----17. ServiceMaster
----18. ProductImageMaster

-------

----### **Payment & Financial Masters**

----19. PaymentModeMaster

