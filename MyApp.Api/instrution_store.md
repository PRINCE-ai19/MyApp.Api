# Kế hoạch cải tiến Dapper Helper để gọi Stored Procedure nhanh chóng

Chào bạn, đây là kế hoạch để tạo ra một Store Helper tiện dụng như bạn mong muốn. Với Helper này, code trong Repository sẽ cực kỳ ngắn gọn và dễ bảo trì.

## 1. Mục tiêu
Tạo ra một service trung gian quản lý việc gọi Stored Procedure. Thay vì phải copy-paste logic mở kết nối, map tham số và thực thi Dapper ở mọi nơi, bạn chỉ cần gọi 1 dòng lệnh duy nhất.

## 2. Các thành phần cần thực hiện

### Bước 1: Tạo Interface `IStoreHelper`
Tạo interface này để khai báo các phương thức dùng chung.
- Đường dẫn dự kiến: `MyApp.Domain/Interfaces_store/IStoreHelper.cs`.
- Các phương thức: `QueryAsync<T>`, `QueryFirstOrDefaultAsync<T>`.

### Bước 2: Tạo lớp triển khai `StoreHelper`
- Sử dụng `DapperHelper.MapParametersAsync` để ánh xạ tham số.
- Đường dẫn: `MyApp.Infrastructure/Helpers/StoreHelper.cs`.

### Bước 3: Đăng ký DI trong `Program.cs`
- `builder.Services.AddScoped<IStoreHelper, StoreHelper>();`

### Bước 4: Refactor Repository mẫu
- Cập nhật Repository để sử dụng `IStoreHelper`.

## 3. Lợi ích
- Code sạch hơn, dễ bảo trì, tránh lỗi quên đóng kết nối.

---

## 4. Stored Procedure: Lấy sản phẩm theo danh mục
Dưới đây là code SQL để lấy danh sách sản phẩm theo `CategoryId`, bao gồm việc kiểm tra trạng thái `RecordStatus` để tránh lấy các bản ghi đã xóa.

```sql
-- Procedure: Lấy sản phẩm theo CategoryId (Có validate RAISERROR)
CREATE PROCEDURE sp_GetProductsByCategoryId
    @CategoryId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- 1. Validate tham số đầu vào
    IF @CategoryId IS NULL OR @CategoryId <= 0
    BEGIN
        DECLARE @ErrMsg1 NVARCHAR(255) = 'InvalidCategoryId';
        RAISERROR(@ErrMsg1, 16, 1);
        RETURN;
    END

    -- 2. Kiểm tra danh mục có tồn tại và đang hoạt động không (RecordStatus khác '0')
    IF NOT EXISTS (SELECT 1 FROM Categories WHERE Id = @CategoryId AND (RecordStatus IS NULL OR RecordStatus <> '0'))
    BEGIN
        DECLARE @ErrMsg2 NVARCHAR(255) = 'CategoryNotFound';
        RAISERROR(@ErrMsg2, 16, 1);
        RETURN;
    END

    -- 3. Thực hiện lấy dữ liệu sản phẩm
    SELECT 
        p.Id, 
        p.Name, 
        p.Price, 
        p.Description, 
        p.StockQuantity, 
        p.CategoryId, 
        p.CreatedAt, 
        p.UpdatedAt, 
        p.Img, 
        p.code, 
        p.RecordStatus,
        c.Name AS CategoryName
    FROM Products p
    INNER JOIN Categories c ON p.CategoryId = c.Id
    WHERE p.CategoryId = @CategoryId 
      AND (p.RecordStatus IS NULL OR p.RecordStatus <> '0');
END
GO
```

### Cách gọi từ Repository (Sử dụng StoreHelper):
```csharp
public async Task<IEnumerable<ProductDto>> GetByCategoryIdAsync(int categoryId)
{
    // Mapping tham số tự động qua StoreHelper
    var parameters = new { CategoryId = categoryId };
    return await _storeHelper.QueryAsync<ProductDto>("sp_GetProductsByCategoryId", parameters);
}
```

---

## 5. Stored Procedure: Thêm sản phẩm mới (Insert Product)
Dưới đây là mẫu Stored Procedure cho việc thêm sản phẩm, tuân thủ đúng pattern **Validation** và **Transaction** (giống như `sp_InsertCategory`).

```sql
-- Procedure: Thêm sản phẩm mới
CREATE PROCEDURE sp_InsertProduct
    @Name NVARCHAR(100),
    @Price DECIMAL(18, 2),
    @Description NVARCHAR(255) = NULL,
    @StockQuantity INT = 0,
    @CategoryId INT,
    @Img NVARCHAR(MAX) = NULL,
    @Code NVARCHAR(100),
    @RecordStatus NVARCHAR(1) = '1'
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Success BIT = 0;
    DECLARE @Message NVARCHAR(255) = '';
    DECLARE @Now DATETIME = GETDATE();

    -- 1. Validate dữ liệu đầu vào
    IF ISNULL(TRIM(@Name), '') = ''
    BEGIN
        SET @Message = 'ProductNameRequired';
    END
    ELSE IF @Price <= 0
    BEGIN
        SET @Message = 'InvalidPrice';
    END
    ELSE IF ISNULL(TRIM(@Code), '') = ''
    BEGIN
        SET @Message = 'ProductCodeRequired';
    END
    -- Kiểm tra CategoryId có tồn tại và đang hoạt động không (khác '0')
    ELSE IF NOT EXISTS (SELECT 1 FROM Categories WHERE Id = @CategoryId AND (RecordStatus IS NULL OR RecordStatus <> '0'))
    BEGIN
        SET @Message = 'CategoryNotFoundOrDeleted';
    END
    -- Kiểm tra trùng mã code sản phẩm (khác '0')
    ELSE IF EXISTS (SELECT 1 FROM Products WHERE Code = TRIM(@Code) AND (RecordStatus IS NULL OR RecordStatus <> '0'))
    BEGIN
        SET @Message = 'DuplicateProductCode';
    END
    ELSE
    BEGIN
        -- 2. Thực hiện Insert trong Transaction
        BEGIN TRY
            BEGIN TRANSACTION;

            INSERT INTO Products (Name, Price, Description, StockQuantity, CategoryId, Img, Code, RecordStatus, CreatedAt)
            VALUES (TRIM(@Name), @Price, TRIM(@Description), @StockQuantity, @CategoryId, @Img, UPPER(TRIM(@Code)), @RecordStatus, @Now);

            COMMIT TRANSACTION;
            SET @Success = 1;
            SET @Message = 'InsertProductSuccess';
        END TRY
        BEGIN CATCH
            IF @@TRANCOUNT > 0
                ROLLBACK TRANSACTION;

            SET @Success = 0;
            SET @Message = ERROR_MESSAGE();
        END CATCH
    END

    -- 3. Trả về kết quả status cho StoreHelper
    SELECT @Success AS Success, @Message AS Message;
END
GO
```

### Cách gọi từ Repository:
```csharp
public async Task<SpResponse> AddAsync(Product product)
{
    // StoreHelper sẽ tự động map các property của product vào tham số SP
    return await _storeHelper.QueryFirstOrDefaultAsync<SpResponse>("sp_InsertProduct", product);
}
```

---

## 6. Stored Procedure: Cập nhật sản phẩm (Update Product)
Mẫu Stored Procedure để cập nhật thông tin sản phẩm, bao gồm kiểm tra sự tồn tại và tính duy nhất của mã Code.

```sql
-- Procedure: Cập nhật sản phẩm
CREATE PROCEDURE sp_UpdateProduct
    @Id INT,
    @Name NVARCHAR(100),
    @Price DECIMAL(18, 2),
    @Description NVARCHAR(255) = NULL,
    @StockQuantity INT = 0,
    @CategoryId INT,
    @Img NVARCHAR(MAX) = NULL,
    @Code NVARCHAR(100),
    @RecordStatus NVARCHAR(1) = '1'
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Success BIT = 0;
    DECLARE @Message NVARCHAR(255) = '';
    DECLARE @Now DATETIME = GETDATE();

    -- 1. Validate dữ liệu đầu vào
    IF NOT EXISTS (SELECT 1 FROM Products WHERE Id = @Id)
    BEGIN
        SET @Message = 'ProductNotFound';
    END
    ELSE IF ISNULL(TRIM(@Name), '') = ''
    BEGIN
        SET @Message = 'ProductNameRequired';
    END
    ELSE IF @Price <= 0
    BEGIN
        SET @Message = 'InvalidPrice';
    END
    -- Kiểm tra CategoryId có tồn tại và đang hoạt động không (khác '0')
    ELSE IF NOT EXISTS (SELECT 1 FROM Categories WHERE Id = @CategoryId AND (RecordStatus IS NULL OR RecordStatus <> '0'))
    BEGIN
        SET @Message = 'CategoryNotFoundOrDeleted';
    END
    -- Kiểm tra trùng mã code với sản phẩm khác (khác '0')
    ELSE IF EXISTS (SELECT 1 FROM Products WHERE Code = TRIM(@Code) AND Id <> @Id AND (RecordStatus IS NULL OR RecordStatus <> '0'))
    BEGIN
        SET @Message = 'DuplicateProductCode';
    END
    ELSE
    BEGIN
        -- 2. Thực hiện Update trong Transaction
        BEGIN TRY
            BEGIN TRANSACTION;

            UPDATE Products
            SET 
                Name = TRIM(@Name),
                Price = @Price,
                Description = TRIM(@Description),
                StockQuantity = @StockQuantity,
                CategoryId = @CategoryId,
                Img = @Img,
                Code = UPPER(TRIM(@Code)),
                RecordStatus = @RecordStatus,
                UpdatedAt = @Now
            WHERE Id = @Id;

            COMMIT TRANSACTION;
            SET @Success = 1;
            SET @Message = 'UpdateProductSuccess';
        END TRY
        BEGIN CATCH
            IF @@TRANCOUNT > 0
                ROLLBACK TRANSACTION;

            SET @Success = 0;
            SET @Message = ERROR_MESSAGE();
        END CATCH
    END

    -- 3. Trả về kết quả
    SELECT @Success AS Success, @Message AS Message;
END
GO
```

### Cách gọi từ Repository:
```csharp
public async Task<SpResponse> UpdateAsync(Product product)
{
    return await _storeHelper.QueryFirstOrDefaultAsync<SpResponse>("sp_UpdateProduct", product);
}
```

---

## 7. Stored Procedure: Xóa sản phẩm (Delete Product)
Thủ tục này thực hiện **Soft Delete** bằng cách cập nhật `RecordStatus` thành 'Deleted'.

```sql
-- Procedure: Xóa sản phẩm (Soft Delete)
CREATE PROCEDURE sp_DeleteProduct
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Success BIT = 0;
    DECLARE @Message NVARCHAR(255) = '';

    -- 1. Kiểm tra tồn tại
    IF NOT EXISTS (SELECT 1 FROM Products WHERE Id = @Id)
    BEGIN
        SET @Message = 'ProductNotFound';
    END
    ELSE
    BEGIN
        BEGIN TRY
            BEGIN TRANSACTION;

            UPDATE Products
            SET RecordStatus = 'Deleted',
                UpdatedAt = GETDATE()
            WHERE Id = @Id;

            COMMIT TRANSACTION;
            SET @Success = 1;
            SET @Message = 'DeleteProductSuccess';
        END TRY
        BEGIN CATCH
            IF @@TRANCOUNT > 0
                ROLLBACK TRANSACTION;

            SET @Success = 0;
            SET @Message = ERROR_MESSAGE();
        END CATCH
    END

    SELECT @Success AS Success, @Message AS Message;
END
GO
```

### Cách gọi từ Repository:
```csharp
public async Task<SpResponse> DeleteAsync(int id)
{
    // Truyền tham số Id vào SP để thực hiện xóa mềm
    return await _storeHelper.QueryFirstOrDefaultAsync<SpResponse>("sp_DeleteProduct", new { Id = id });
}
```

