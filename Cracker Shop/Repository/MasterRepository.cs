using Cracker_Shop.Code_Generator;
using Cracker_Shop.Models.MasterModels;
using Cracker_Shop.Repository.IRepository;
using Dapper;
using System.Data;
using System.Text.RegularExpressions;

namespace Cracker_Shop.Repository
{
    public class MasterRepository : IMasterRepository
    {
        private readonly IDbConnection _db;

        public MasterRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<long> AddUpdateDelete(BrandMaster brand)
        {
            if (string.IsNullOrWhiteSpace(brand.BrandName))
                throw new ArgumentException("BrandName is required.");

            brand.BrandName = System.Text.RegularExpressions.Regex.Replace(brand.BrandName.Trim(), @"\s+", " ");

            var duplicateCheckSql = @"
SELECT COUNT(1)
FROM BrandMaster
WHERE REPLACE(UPPER(LTRIM(RTRIM(BrandName))), ' ', '') = REPLACE(UPPER(LTRIM(RTRIM(@BrandName))), ' ', '')
  AND (@BrandID = 0 OR BrandID <> @BrandID)
  AND IsActive = 1";

            var count = await _db.ExecuteScalarAsync<int>(duplicateCheckSql, new { brand.BrandName, brand.BrandID });
            if (count > 0)
                throw new InvalidOperationException("A brand with the same name already exists.");

            if (brand.BrandID == 0) 
            {

                var sqlInsert = @"
            INSERT INTO BrandMaster
            ( BrandName, Description, IsActive, CreatedByUserID, CreatedSystemName, CreatedAt)
            VALUES
            (@BrandName, @Description, @IsActive, @CreatedByUserID, @CreatedSystemName, SYSDATETIME());
            SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

                return await _db.ExecuteScalarAsync<long>(sqlInsert, brand);
            }

            if (!brand.IsActive)
            {
                var sqlDelete = @"
            UPDATE BrandMaster
            SET IsActive = 0,
                UpdatedByUserID = @UpdatedByUserID,
                UpdatedSystemName = @UpdatedSystemName,
                UpdatedAt = SYSDATETIME()
            WHERE BrandID = @BrandID";

                await _db.ExecuteAsync(sqlDelete, brand);
                return brand.BrandID;
            }

            var sqlUpdate = @"
        UPDATE BrandMaster SET
            BrandName = @BrandName,
            Description = @Description,
            IsActive = @IsActive,
            UpdatedByUserID = @UpdatedByUserID,
            UpdatedSystemName = @UpdatedSystemName,
            UpdatedAt = SYSDATETIME()
        WHERE BrandID = @BrandID";

            await _db.ExecuteAsync(sqlUpdate, brand);
            return brand.BrandID;
        }




        public async Task<IEnumerable<BrandMaster>> GetActiveBrands()
        {
            var sql = @"
                SELECT *
                FROM BrandMaster
                WHERE IsActive = 1
                ORDER BY BrandName";

            return await _db.QueryAsync<BrandMaster>(sql);
        }



        // CategoryMaster Methods
        public async Task<long> SaveCategoryAsync(CategoryMaster category)
        {
            if (string.IsNullOrWhiteSpace(category.CategoryName))
                throw new ArgumentException("CategoryName is required.");

            category.CategoryName = Regex.Replace(category.CategoryName.Trim(), @"\s+", " ");

            var duplicateSql = @"
                SELECT COUNT(1)
                FROM CategoryMaster
                WHERE LTRIM(RTRIM(UPPER(CategoryName))) = UPPER(@CategoryName)
                  AND (@CategoryID = 0 OR CategoryID <> @CategoryID)
                  AND IsActive = 1";

            var count = await _db.ExecuteScalarAsync<int>(duplicateSql, new { category.CategoryName, category.CategoryID });
            if (count > 0)
                throw new InvalidOperationException("A category with the same name already exists.");

            if (category.CategoryID == 0)
            {
                var sqlInsert = @"
                    INSERT INTO CategoryMaster
                    (CategoryName, Description, IsActive, CreatedByUserID, CreatedSystemName, CreatedAt)
                    VALUES
                    (@CategoryName, @Description, @IsActive, @CreatedByUserID, @CreatedSystemName, SYSDATETIME());
                    SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

                return await _db.ExecuteScalarAsync<long>(sqlInsert, category);
            }

            if (!category.IsActive)
            {
                var sqlDelete = @"
                    UPDATE CategoryMaster
                    SET IsActive = 0,
                        UpdatedByUserID = @UpdatedByUserID,
                        UpdatedSystemName = @UpdatedSystemName,
                        UpdatedAt = SYSDATETIME()
                    WHERE CategoryID = @CategoryID";

                await _db.ExecuteAsync(sqlDelete, category);
                return category.CategoryID;
            }

            var sqlUpdate = @"
                UPDATE CategoryMaster SET
                    CategoryName = @CategoryName,
                    Description = @Description,
                    IsActive = @IsActive,
                    UpdatedByUserID = @UpdatedByUserID,
                    UpdatedSystemName = @UpdatedSystemName,
                    UpdatedAt = SYSDATETIME()
                WHERE CategoryID = @CategoryID";

            await _db.ExecuteAsync(sqlUpdate, category);
            return category.CategoryID;
        }

        public async Task<IEnumerable<CategoryMaster>> GetActiveCategoriesAsync()
        {
            var sql = @"
                SELECT *
                FROM CategoryMaster
                WHERE IsActive = 1
                ORDER BY CategoryName";

            return await _db.QueryAsync<CategoryMaster>(sql);
        }

        // CessMaster Methods
        public async Task<long> SaveCessAsync(CessMaster cess)
        {
            if (string.IsNullOrWhiteSpace(cess.CessName))
                throw new ArgumentException("CessName is required.");

            // Normalize CessName
            cess.CessName = System.Text.RegularExpressions.Regex.Replace(cess.CessName.Trim(), @"\s+", " ");

            // Duplicate check
            var duplicateSql = @"
        SELECT COUNT(1)
        FROM CessMaster
        WHERE LTRIM(RTRIM(UPPER(CessName))) = UPPER(@CessName)
          AND (@CessID = 0 OR CessID <> @CessID)
          AND IsActive = 1";

            var count = await _db.ExecuteScalarAsync<int>(duplicateSql, new { cess.CessName, cess.CessID });
            if (count > 0)
                throw new InvalidOperationException("A Cess with the same name already exists.");

            // Insert
            if (cess.CessID == 0)
            {
                var sqlInsert = @"
            INSERT INTO CessMaster
            (CessName, CessRate, Description, IsActive, CreatedByUserID, CreatedSystemName, CreatedAt)
            VALUES
            (@CessName, @CessRate, @Description, @IsActive, @CreatedByUserID, @CreatedSystemName, SYSDATETIME());
            SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

                return await _db.ExecuteScalarAsync<long>(sqlInsert, cess);
            }

            // Soft Delete
            if (!cess.IsActive)
            {
                var sqlDelete = @"
            UPDATE CessMaster
            SET IsActive = 0,
                UpdatedByUserID = @UpdatedByUserID,
                UpdatedSystemName = @UpdatedSystemName,
                UpdatedAt = SYSDATETIME()
            WHERE CessID = @CessID";

                await _db.ExecuteAsync(sqlDelete, cess);
                return cess.CessID;
            }

            // Update
            var sqlUpdate = @"
        UPDATE CessMaster SET
            CessName = @CessName,
            CessRate = @CessRate,
            Description = @Description,
            IsActive = @IsActive,
            UpdatedByUserID = @UpdatedByUserID,
            UpdatedSystemName = @UpdatedSystemName,
            UpdatedAt = SYSDATETIME()
        WHERE CessID = @CessID";

            await _db.ExecuteAsync(sqlUpdate, cess);
            return cess.CessID;
        }

        public async Task<IEnumerable<CessMaster>> GetActiveCessAsync()
        {
            var sql = @"
        SELECT *
        FROM CessMaster
        WHERE IsActive = 1
        ORDER BY CessName";

            return await _db.QueryAsync<CessMaster>(sql);
        }









        // CustomerMaster Methods
        public async Task<long> SaveCustomerAsync(CustomerMaster customer)
        {
            if (string.IsNullOrWhiteSpace(customer.CustomerName))
                throw new ArgumentException("CustomerName is required.");

            // Normalize CustomerName
            customer.CustomerName = System.Text.RegularExpressions.Regex.Replace(customer.CustomerName.Trim(), @"\s+", " ");

            // Duplicate check
            var duplicateSql = @"
        SELECT COUNT(1)
        FROM CustomerMaster
        WHERE LTRIM(RTRIM(UPPER(CustomerName))) = UPPER(@CustomerName)
          AND (@CustomerID = 0 OR CustomerID <> @CustomerID)
          AND IsActive = 1";

            var count = await _db.ExecuteScalarAsync<int>(duplicateSql, new { customer.CustomerName, customer.CustomerID });
            if (count > 0)
                throw new InvalidOperationException("A customer with the same name already exists.");

            if (customer.CustomerID == 0)
            {
                customer.CustomerCode = await CodeGenerator.GenerateNextCodeAsync(_db, "CustomerMaster", "CustomerCode", "CUS", 5);

                var sqlInsert = @"
            INSERT INTO CustomerMaster
            (CustomerCode, CustomerName, Phone, AlternatePhone, Email, AddressLine1, AddressLine2, City, State, Country, PostalCode, IsActive, CreatedByUserID, CreatedSystemName, CreatedAt)
            VALUES
            (@CustomerCode, @CustomerName, @Phone, @AlternatePhone, @Email, @AddressLine1, @AddressLine2, @City, @State, @Country, @PostalCode, @IsActive, @CreatedByUserID, @CreatedSystemName, SYSDATETIME());
            SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

                return await _db.ExecuteScalarAsync<long>(sqlInsert, customer);
            }

            // Soft Delete
            if (!customer.IsActive)
            {
                var sqlDelete = @"
            UPDATE CustomerMaster
            SET IsActive = 0,
                UpdatedByUserID = @UpdatedByUserID,
                UpdatedSystemName = @UpdatedSystemName,
                UpdatedAt = SYSDATETIME()
            WHERE CustomerID = @CustomerID";

                await _db.ExecuteAsync(sqlDelete, customer);
                return customer.CustomerID;
            }

            // Update
            var sqlUpdate = @"
        UPDATE CustomerMaster SET
            CustomerName = @CustomerName,
            Phone = @Phone,
            AlternatePhone = @AlternatePhone,
            Email = @Email,
            AddressLine1 = @AddressLine1,
            AddressLine2 = @AddressLine2,
            City = @City,
            State = @State,
            Country = @Country,
            PostalCode = @PostalCode,
            IsActive = @IsActive,
            UpdatedByUserID = @UpdatedByUserID,
            UpdatedSystemName = @UpdatedSystemName,
            UpdatedAt = SYSDATETIME()
        WHERE CustomerID = @CustomerID";

            await _db.ExecuteAsync(sqlUpdate, customer);
            return customer.CustomerID;
        }

        public async Task<IEnumerable<CustomerMaster>> GetActiveCustomersAsync()
        {
            var sql = @"
        SELECT *
        FROM CustomerMaster
        WHERE IsActive = 1
        ORDER BY CustomerName";

            return await _db.QueryAsync<CustomerMaster>(sql);
        }


        // HSNCodeMaster Methods
        public async Task<long> SaveHSNCodeAsync(HSNCodeMaster hsn)
        {
            if (string.IsNullOrWhiteSpace(hsn.HSNCode))
                throw new ArgumentException("HSNCode is required.");

            // Normalize HSNCode
            hsn.HSNCode = System.Text.RegularExpressions.Regex.Replace(hsn.HSNCode.Trim(), @"\s+", " ");

            // Duplicate check
            var duplicateSql = @"
        SELECT COUNT(1)
        FROM HSNCodeMaster
        WHERE LTRIM(RTRIM(UPPER(HSNCode))) = UPPER(@HSNCode)
          AND (@HSNID = 0 OR HSNID <> @HSNID)
          AND IsActive = 1";

            var count = await _db.ExecuteScalarAsync<int>(duplicateSql, new { hsn.HSNCode, hsn.HSNID });
            if (count > 0)
                throw new InvalidOperationException("An HSN code with the same value already exists.");

            // Insert
            if (hsn.HSNID == 0)
            {
                var sqlInsert = @"
            INSERT INTO HSNCodeMaster
            (HSNCode, Description, TaxID, IsActive, CreatedByUserID, CreatedSystemName, CreatedAt)
            VALUES
            (@HSNCode, @Description, @TaxID, @IsActive, @CreatedByUserID, @CreatedSystemName, SYSDATETIME());
            SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

                return await _db.ExecuteScalarAsync<long>(sqlInsert, hsn);
            }

            // Soft Delete
            if (!hsn.IsActive)
            {
                var sqlDelete = @"
            UPDATE HSNCodeMaster
            SET IsActive = 0,
                UpdatedByUserID = @UpdatedByUserID,
                UpdatedSystemName = @UpdatedSystemName,
                UpdatedAt = SYSDATETIME()
            WHERE HSNID = @HSNID";

                await _db.ExecuteAsync(sqlDelete, hsn);
                return hsn.HSNID;
            }

            // Update
            var sqlUpdate = @"
        UPDATE HSNCodeMaster SET
            HSNCode = @HSNCode,
            Description = @Description,
            TaxID = @TaxID,
            IsActive = @IsActive,
            UpdatedByUserID = @UpdatedByUserID,
            UpdatedSystemName = @UpdatedSystemName,
            UpdatedAt = SYSDATETIME()
        WHERE HSNID = @HSNID";

            await _db.ExecuteAsync(sqlUpdate, hsn);
            return hsn.HSNID;
        }

        public async Task<IEnumerable<HSNCodeMaster>> GetActiveHSNCodesAsync()
        {
            var sql = @"
        SELECT *
        FROM HSNCodeMaster
        WHERE IsActive = 1
        ORDER BY HSNCode";

            return await _db.QueryAsync<HSNCodeMaster>(sql);
        }

        // PaymentModeMaster Methods
        public async Task<long> SavePaymentModeAsync(PaymentModeMaster paymentMode)
        {
            if (string.IsNullOrWhiteSpace(paymentMode.PaymentModeName))
                throw new ArgumentException("PaymentModeName is required.");

            // Normalize PaymentModeName
            paymentMode.PaymentModeName = System.Text.RegularExpressions.Regex.Replace(paymentMode.PaymentModeName.Trim(), @"\s+", " ");

            // Duplicate check
            var duplicateSql = @"
        SELECT COUNT(1)
        FROM PaymentModeMaster
        WHERE LTRIM(RTRIM(UPPER(PaymentModeName))) = UPPER(@PaymentModeName)
          AND (@PaymentModeID = 0 OR PaymentModeID <> @PaymentModeID)
          AND IsActive = 1";

            var count = await _db.ExecuteScalarAsync<int>(duplicateSql, new { paymentMode.PaymentModeName, paymentMode.PaymentModeID });
            if (count > 0)
                throw new InvalidOperationException("A payment mode with the same name already exists.");

            // Insert
            if (paymentMode.PaymentModeID == 0)
            {
                var sqlInsert = @"
            INSERT INTO PaymentModeMaster
            (PaymentModeName, PaymentType, Description, IsActive, CreatedByUserID, CreatedSystemName, CreatedAt)
            VALUES
            (@PaymentModeName, @PaymentType, @Description, @IsActive, @CreatedByUserID, @CreatedSystemName, SYSDATETIME());
            SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

                return await _db.ExecuteScalarAsync<long>(sqlInsert, paymentMode);
            }

            // Soft Delete
            if (!paymentMode.IsActive)
            {
                var sqlDelete = @"
            UPDATE PaymentModeMaster
            SET IsActive = 0,
                UpdatedByUserID = @UpdatedByUserID,
                UpdatedSystemName = @UpdatedSystemName,
                UpdatedAt = SYSDATETIME()
            WHERE PaymentModeID = @PaymentModeID";

                await _db.ExecuteAsync(sqlDelete, paymentMode);
                return paymentMode.PaymentModeID;
            }

            // Update
            var sqlUpdate = @"
        UPDATE PaymentModeMaster SET
            PaymentModeName = @PaymentModeName,
            PaymentType = @PaymentType,
            Description = @Description,
            IsActive = @IsActive,
            UpdatedByUserID = @UpdatedByUserID,
            UpdatedSystemName = @UpdatedSystemName,
            UpdatedAt = SYSDATETIME()
        WHERE PaymentModeID = @PaymentModeID";

            await _db.ExecuteAsync(sqlUpdate, paymentMode);
            return paymentMode.PaymentModeID;
        }

        public async Task<IEnumerable<PaymentModeMaster>> GetActivePaymentModesAsync()
        {
            var sql = @"
        SELECT *
        FROM PaymentModeMaster
        WHERE IsActive = 1
        ORDER BY PaymentModeName";

            return await _db.QueryAsync<PaymentModeMaster>(sql);
        }



        // ProductImageMaster Methods
        public async Task<long> SaveProductImageAsync(ProductImageMaster image)
        {
            if (image.ProductID <= 0)
                throw new ArgumentException("ProductID is required.");

            if (image.ProductImageID == 0)
            {
                var sqlInsert = @"
            INSERT INTO ProductImageMaster
            (ProductID, ImageData, ImageName, ContentType, IsPrimary, DisplayOrder, IsActive, CreatedByUserID, CreatedSystemName, CreatedAt)
            VALUES
            (@ProductID, @ImageData, @ImageName, @ContentType, @IsPrimary, @DisplayOrder, @IsActive, @CreatedByUserID, @CreatedSystemName, SYSDATETIME());
            SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

                return await _db.ExecuteScalarAsync<long>(sqlInsert, image);
            }

            if (!image.IsActive)
            {
                var sqlDelete = @"
            UPDATE ProductImageMaster
            SET IsActive = 0, UpdatedByUserID=@UpdatedByUserID, UpdatedSystemName=@UpdatedSystemName, UpdatedAt=SYSDATETIME()
            WHERE ProductImageID=@ProductImageID";

                await _db.ExecuteAsync(sqlDelete, image);
            }
            else
            {
                var sqlUpdate = @"
            UPDATE ProductImageMaster
            SET ImageData=@ImageData, ImageName=@ImageName, ContentType=@ContentType,
                IsPrimary=@IsPrimary, DisplayOrder=@DisplayOrder, IsActive=@IsActive,
                UpdatedByUserID=@UpdatedByUserID, UpdatedSystemName=@UpdatedSystemName, UpdatedAt=SYSDATETIME()
            WHERE ProductImageID=@ProductImageID";

                await _db.ExecuteAsync(sqlUpdate, image);
            }

            return image.ProductImageID;
        }

        public async Task<IEnumerable<ProductImageMaster>> GetActiveProductImagesAsync(long productId)
        {
            var sql = @"
        SELECT *
        FROM ProductImageMaster
        WHERE ProductID=@ProductID AND IsActive=1
        ORDER BY DisplayOrder, CreatedAt";

            return await _db.QueryAsync<ProductImageMaster>(sql, new { ProductID = productId });
        }
        // ProductMaster Methods
        public async Task<long> SaveProductAsync(ProductMaster product)
        {
            if (string.IsNullOrWhiteSpace(product.ProductName))
                throw new ArgumentException("ProductName is required.");

            product.ProductName = System.Text.RegularExpressions.Regex.Replace(product.ProductName.Trim(), @"\s+", " ");

            // Duplicate check on ProductName for the same Company
            var duplicateSql = @"
        SELECT COUNT(1)
        FROM ProductMaster
        WHERE LTRIM(RTRIM(UPPER(ProductName))) = UPPER(@ProductName)
          AND (@ProductID = 0 OR ProductID <> @ProductID)
          AND CompanyID = @CompanyID
          AND IsActive = 1";

            var count = await _db.ExecuteScalarAsync<int>(duplicateSql, new { product.ProductName, product.ProductID, product.CompanyID });
            if (count > 0) throw new InvalidOperationException("A product with the same name already exists.");

            if (product.ProductID == 0)
            {
                // Generate ProductCode like PRD00001
                product.ProductCode = await CodeGenerator.GenerateNextCodeAsync(_db, "ProductMaster", "ProductCode", "PRD", 5);

                var sqlInsert = @"
            INSERT INTO ProductMaster
            (ProductCode, ProductName, CategoryID, SubCategoryID, BrandID, UnitID, HSNID, TaxID, CessID,
             PurchaseRate, RetailPrice, WholesalePrice, SaleRate, MRP, IsActive, CreatedByUserID, CreatedSystemName, CreatedAt,
             DiscountAmount, DiscountPercentage, OpeningStock, ReorderLevel, CurrentStock, Barcode, IsService, ProductDescription, ProductImage, CompanyID)
            VALUES
            (@ProductCode, @ProductName, @CategoryID, @SubCategoryID, @BrandID, @UnitID, @HSNID, @TaxID, @CessID,
             @PurchaseRate, @RetailPrice, @WholesalePrice, @SaleRate, @MRP, @IsActive, @CreatedByUserID, @CreatedSystemName, SYSDATETIME(),
             @DiscountAmount, @DiscountPercentage, @OpeningStock, @ReorderLevel, @CurrentStock, @Barcode, @IsService, @ProductDescription, @ProductImage, @CompanyID);
            SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

                return await _db.ExecuteScalarAsync<long>(sqlInsert, product);
            }

            if (!product.IsActive)
            {
                await _db.ExecuteAsync(@"
            UPDATE ProductMaster
            SET IsActive=0, UpdatedByUserID=@UpdatedByUserID, UpdatedSystemName=@UpdatedSystemName, UpdatedAt=SYSDATETIME()
            WHERE ProductID=@ProductID", product);
            }
            else
            {
                var sqlUpdate = @"
            UPDATE ProductMaster SET
                ProductName=@ProductName, CategoryID=@CategoryID, SubCategoryID=@SubCategoryID, BrandID=@BrandID, UnitID=@UnitID,
                HSNID=@HSNID, TaxID=@TaxID, CessID=@CessID,
                PurchaseRate=@PurchaseRate, RetailPrice=@RetailPrice, WholesalePrice=@WholesalePrice, SaleRate=@SaleRate, MRP=@MRP,
                DiscountAmount=@DiscountAmount, DiscountPercentage=@DiscountPercentage,
                OpeningStock=@OpeningStock, ReorderLevel=@ReorderLevel, CurrentStock=@CurrentStock,
                Barcode=@Barcode, IsService=@IsService, ProductDescription=@ProductDescription, ProductImage=@ProductImage,
                IsActive=@IsActive, UpdatedByUserID=@UpdatedByUserID, UpdatedSystemName=@UpdatedSystemName, UpdatedAt=SYSDATETIME()
            WHERE ProductID=@ProductID";

                await _db.ExecuteAsync(sqlUpdate, product);
            }

            return product.ProductID;
        }

        public async Task<IEnumerable<ProductMaster>> GetActiveProductsAsync(long companyId)
        {
            var sql = @"
        SELECT *
        FROM ProductMaster
        WHERE CompanyID=@CompanyID AND IsActive=1
        ORDER BY ProductName";

            return await _db.QueryAsync<ProductMaster>(sql, new { CompanyID = companyId });
        }

        public async Task<long> SaveServiceAsync(ServiceMaster service)
        {
            if (string.IsNullOrWhiteSpace(service.ServiceName))
                throw new ArgumentException("ServiceName is required.");

            service.ServiceName = System.Text.RegularExpressions.Regex.Replace(service.ServiceName.Trim(), @"\s+", " ");

            // Duplicate check
            var duplicate = await _db.ExecuteScalarAsync<int>(@"
        SELECT COUNT(1)
        FROM ServiceMaster
        WHERE LTRIM(RTRIM(UPPER(ServiceName))) = UPPER(@ServiceName)
          AND (@ServiceID = 0 OR ServiceID <> @ServiceID)
          AND IsActive = 1", new { service.ServiceName, service.ServiceID });

            if (duplicate > 0) throw new InvalidOperationException("A service with the same name already exists.");

            // Insert
            if (service.ServiceID == 0)
            {
                service.ServiceCode = await CodeGenerator.GenerateNextCodeAsync(_db, "ServiceMaster", "ServiceCode", "SRV", 5);

                var sqlInsert = @"
            INSERT INTO ServiceMaster
            (ServiceCode, ServiceName, CategoryID, HSNID, TaxID, CessID, ServiceCharge, IsActive, CreatedByUserID, CreatedSystemName, CreatedAt)
            VALUES
            (@ServiceCode, @ServiceName, @CategoryID, @HSNID, @TaxID, @CessID, @ServiceCharge, @IsActive, @CreatedByUserID, @CreatedSystemName, SYSDATETIME());
            SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

                return await _db.ExecuteScalarAsync<long>(sqlInsert, service);
            }

            // Soft Delete
            if (!service.IsActive)
            {
                await _db.ExecuteAsync(@"
            UPDATE ServiceMaster
            SET IsActive=0, UpdatedByUserID=@UpdatedByUserID, UpdatedSystemName=@UpdatedSystemName, UpdatedAt=SYSDATETIME()
            WHERE ServiceID=@ServiceID", service);
            }
            else
            {
                var sqlUpdate = @"
            UPDATE ServiceMaster SET
                ServiceName=@ServiceName, CategoryID=@CategoryID, HSNID=@HSNID, TaxID=@TaxID, CessID=@CessID,
                ServiceCharge=@ServiceCharge, IsActive=@IsActive,
                UpdatedByUserID=@UpdatedByUserID, UpdatedSystemName=@UpdatedSystemName, UpdatedAt=SYSDATETIME()
            WHERE ServiceID=@ServiceID";

                await _db.ExecuteAsync(sqlUpdate, service);
            }

            return service.ServiceID;
        }

        public async Task<IEnumerable<ServiceMaster>> GetActiveServicesAsync()
        {
            var sql = "SELECT * FROM ServiceMaster WHERE IsActive=1 ORDER BY ServiceName";
            return await _db.QueryAsync<ServiceMaster>(sql);
        }
        // SubCategoryMaster Methods
        public async Task<long> SaveSubCategoryAsync(SubCategoryMaster subCategory)
        {
            if (string.IsNullOrWhiteSpace(subCategory.SubCategoryName))
                throw new ArgumentException("SubCategoryName is required.");

            subCategory.SubCategoryName = System.Text.RegularExpressions.Regex.Replace(subCategory.SubCategoryName.Trim(), @"\s+", " ");

            // Duplicate check within the same Category
            var duplicate = await _db.ExecuteScalarAsync<int>(@"
        SELECT COUNT(1)
        FROM SubCategoryMaster
        WHERE LTRIM(RTRIM(UPPER(SubCategoryName))) = UPPER(@SubCategoryName)
          AND (@SubCategoryID = 0 OR SubCategoryID <> @SubCategoryID)
          AND (@CategoryID IS NULL OR CategoryID = @CategoryID)
          AND IsActive = 1",
                new { subCategory.SubCategoryName, subCategory.SubCategoryID, subCategory.CategoryID });

            if (duplicate > 0)
                throw new InvalidOperationException("A subcategory with the same name already exists in this category.");

            if (subCategory.SubCategoryID == 0)
            {
                var sqlInsert = @"
            INSERT INTO SubCategoryMaster
            (CategoryID, SubCategoryName, Description, IsActive, CreatedByUserID, CreatedSystemName, CreatedAt)
            VALUES
            (@CategoryID, @SubCategoryName, @Description, @IsActive, @CreatedByUserID, @CreatedSystemName, SYSDATETIME());
            SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

                return await _db.ExecuteScalarAsync<long>(sqlInsert, subCategory);
            }

            if (!subCategory.IsActive)
            {
                await _db.ExecuteAsync(@"
            UPDATE SubCategoryMaster
            SET IsActive=0, UpdatedByUserID=@UpdatedByUserID, UpdatedSystemName=@UpdatedSystemName, UpdatedAt=SYSDATETIME()
            WHERE SubCategoryID=@SubCategoryID", subCategory);
            }
            else
            {
                var sqlUpdate = @"
            UPDATE SubCategoryMaster SET
                CategoryID=@CategoryID, SubCategoryName=@SubCategoryName, Description=@Description,
                IsActive=@IsActive, UpdatedByUserID=@UpdatedByUserID, UpdatedSystemName=@UpdatedSystemName, UpdatedAt=SYSDATETIME()
            WHERE SubCategoryID=@SubCategoryID";

                await _db.ExecuteAsync(sqlUpdate, subCategory);
            }

            return subCategory.SubCategoryID;
        }

        public async Task<IEnumerable<SubCategoryMaster>> GetActiveSubCategoriesAsync(long? categoryId = null)
        {
            var sql = @"
        SELECT *
        FROM SubCategoryMaster
        WHERE IsActive=1"
                + (categoryId.HasValue ? " AND CategoryID=@CategoryID" : "") +
                " ORDER BY SubCategoryName";

            return await _db.QueryAsync<SubCategoryMaster>(sql, new { CategoryID = categoryId });
        }
        // SupplierMaster Methods
        public async Task<long> SaveSupplierAsync(SupplierMaster supplier)
        {
            if (string.IsNullOrWhiteSpace(supplier.SupplierName))
                throw new ArgumentException("SupplierName is required.");

            supplier.SupplierName = System.Text.RegularExpressions.Regex.Replace(supplier.SupplierName.Trim(), @"\s+", " ");

            // Duplicate check (by name)
            var duplicate = await _db.ExecuteScalarAsync<int>(@"
        SELECT COUNT(1)
        FROM SupplierMaster
        WHERE LTRIM(RTRIM(UPPER(SupplierName))) = UPPER(@SupplierName)
          AND (@SupplierID = 0 OR SupplierID <> @SupplierID)
          AND IsActive = 1",
                new { supplier.SupplierName, supplier.SupplierID });

            if (duplicate > 0) throw new InvalidOperationException("A supplier with the same name already exists.");

            // Insert
            if (supplier.SupplierID == 0)
            {
                supplier.SupplierCode = await CodeGenerator.GenerateNextCodeAsync(_db, "SupplierMaster", "SupplierCode", "SUP", 5);

                var sqlInsert = @"
            INSERT INTO SupplierMaster
            (SupplierCode, SupplierName, Phone, AlternatePhone, Email, AddressLine1, AddressLine2,
             City, State, Country, PostalCode, GSTNumber, IsActive, CreatedByUserID, CreatedSystemName, CreatedAt)
            VALUES
            (@SupplierCode, @SupplierName, @Phone, @AlternatePhone, @Email, @AddressLine1, @AddressLine2,
             @City, @State, @Country, @PostalCode, @GSTNumber, @IsActive, @CreatedByUserID, @CreatedSystemName, SYSDATETIME());
            SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

                return await _db.ExecuteScalarAsync<long>(sqlInsert, supplier);
            }

            // Soft Delete
            if (!supplier.IsActive)
            {
                await _db.ExecuteAsync(@"
            UPDATE SupplierMaster
            SET IsActive=0, UpdatedByUserID=@UpdatedByUserID, UpdatedSystemName=@UpdatedSystemName, UpdatedAt=SYSDATETIME()
            WHERE SupplierID=@SupplierID", supplier);
            }
            else
            {
                // Update
                var sqlUpdate = @"
            UPDATE SupplierMaster SET
                SupplierName=@SupplierName, Phone=@Phone, AlternatePhone=@AlternatePhone, Email=@Email,
                AddressLine1=@AddressLine1, AddressLine2=@AddressLine2, City=@City, State=@State,
                Country=@Country, PostalCode=@PostalCode, GSTNumber=@GSTNumber, IsActive=@IsActive,
                UpdatedByUserID=@UpdatedByUserID, UpdatedSystemName=@UpdatedSystemName, UpdatedAt=SYSDATETIME()
            WHERE SupplierID=@SupplierID";

                await _db.ExecuteAsync(sqlUpdate, supplier);
            }

            return supplier.SupplierID;
        }

        public async Task<IEnumerable<SupplierMaster>> GetActiveSuppliersAsync()
        {
            var sql = "SELECT * FROM SupplierMaster WHERE IsActive=1 ORDER BY SupplierName";
            return await _db.QueryAsync<SupplierMaster>(sql);
        }
        // TaxMaster Methods
        public async Task<long> SaveTaxAsync(TaxMaster tax)
        {
            if (string.IsNullOrWhiteSpace(tax.TaxName))
                throw new ArgumentException("TaxName is required.");

            tax.TaxName = System.Text.RegularExpressions.Regex.Replace(tax.TaxName.Trim(), @"\s+", " ");

            // Duplicate check
            var duplicate = await _db.ExecuteScalarAsync<int>(@"
        SELECT COUNT(1)
        FROM TaxMaster
        WHERE LTRIM(RTRIM(UPPER(TaxName))) = UPPER(@TaxName)
          AND (@TaxID = 0 OR TaxID <> @TaxID)
          AND IsActive = 1", new { tax.TaxName, tax.TaxID });

            if (duplicate > 0) throw new InvalidOperationException("A tax with the same name already exists.");

            // Insert
            if (tax.TaxID == 0)
            {
                var sqlInsert = @"
            INSERT INTO TaxMaster
            (TaxName, TaxRate, CessRate, IsActive, CreatedByUserID, CreatedSystemName, CreatedAt)
            VALUES
            (@TaxName, @TaxRate, @CessRate, @IsActive, @CreatedByUserID, @CreatedSystemName, SYSDATETIME());
            SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

                return await _db.ExecuteScalarAsync<long>(sqlInsert, tax);
            }

            // Soft Delete
            if (!tax.IsActive)
            {
                await _db.ExecuteAsync(@"
            UPDATE TaxMaster
            SET IsActive=0, UpdatedByUserID=@UpdatedByUserID, UpdatedSystemName=@UpdatedSystemName, UpdatedAt=SYSDATETIME()
            WHERE TaxID=@TaxID", tax);
            }
            else
            {
                // Update
                var sqlUpdate = @"
            UPDATE TaxMaster SET
                TaxName=@TaxName, TaxRate=@TaxRate, CessRate=@CessRate, IsActive=@IsActive,
                UpdatedByUserID=@UpdatedByUserID, UpdatedSystemName=@UpdatedSystemName, UpdatedAt=SYSDATETIME()
            WHERE TaxID=@TaxID";

                await _db.ExecuteAsync(sqlUpdate, tax);
            }

            return tax.TaxID;
        }

        public async Task<IEnumerable<TaxMaster>> GetActiveTaxesAsync()
        {
            var sql = "SELECT * FROM TaxMaster WHERE IsActive=1 ORDER BY TaxName";
            return await _db.QueryAsync<TaxMaster>(sql);
        }
        public async Task<long> SaveUnitAsync(UnitMaster unit)
        {
            if (string.IsNullOrWhiteSpace(unit.UnitName))
                throw new ArgumentException("UnitName is required.");

            unit.UnitName = System.Text.RegularExpressions.Regex.Replace(unit.UnitName.Trim(), @"\s+", " ");

            var duplicate = await _db.ExecuteScalarAsync<int>(@"
        SELECT COUNT(1)
        FROM UnitMaster
        WHERE LTRIM(RTRIM(UPPER(UnitName))) = UPPER(@UnitName)
          AND (@UnitID = 0 OR UnitID <> @UnitID)
          AND IsActive = 1", new { unit.UnitName, unit.UnitID });

            if (duplicate > 0) throw new InvalidOperationException("A unit with the same name already exists.");

            if (unit.UnitID == 0)
            {
                var sqlInsert = @"
            INSERT INTO UnitMaster
            (UnitName, UnitCode, IsActive, CreatedByUserID, CreatedSystemName, CreatedAt)
            VALUES
            (@UnitName, @UnitCode, @IsActive, @CreatedByUserID, @CreatedSystemName, SYSDATETIME());
            SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

                return await _db.ExecuteScalarAsync<long>(sqlInsert, unit);
            }

            if (!unit.IsActive)
            {
                await _db.ExecuteAsync(@"
            UPDATE UnitMaster
            SET IsActive=0, UpdatedByUserID=@UpdatedByUserID, UpdatedSystemName=@UpdatedSystemName, UpdatedAt=SYSDATETIME()
            WHERE UnitID=@UnitID", unit);
            }
            else
            {
                var sqlUpdate = @"
            UPDATE UnitMaster SET
                UnitName=@UnitName, UnitCode=@UnitCode, IsActive=@IsActive,
                UpdatedByUserID=@UpdatedByUserID, UpdatedSystemName=@UpdatedSystemName, UpdatedAt=SYSDATETIME()
            WHERE UnitID=@UnitID";

                await _db.ExecuteAsync(sqlUpdate, unit);
            }

            return unit.UnitID;
        }

        public async Task<IEnumerable<UnitMaster>> GetActiveUnitsAsync()
        {
            var sql = "SELECT * FROM UnitMaster WHERE IsActive=1 ORDER BY UnitName";
            return await _db.QueryAsync<UnitMaster>(sql);
        }


    }
}
