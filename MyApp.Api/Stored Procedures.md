# Stored Procedures for Category

## 1. Get All Categories
This procedure retrieves all categories that are active (RecordStatus = '1' or active logic).

```sql
CREATE OR ALTER PROCEDURE sp_GetAllCategories
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        [Id],
        [Name],
        [Description],
        [Code],
        [RecordStatus]
    FROM [Categories]
    WHERE [RecordStatus] = '1' -- Hoặc bỏ qua WHERE nếu muốn lấy tất cả
    ORDER BY [Name] ASC;
END
GO
```

## 2. Get Category By ID
This procedure retrieves a single category by its unique identifier.

```sql
CREATE OR ALTER PROCEDURE sp_GetCategoryById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- 1. Validate: Kiểm tra tồn tại
    IF NOT EXISTS (SELECT 1 FROM [Categories] WHERE [Id] = @Id)
    BEGIN
        -- Quăng lỗi chủ động từ SQL Server
        DECLARE @ErrMsg NVARCHAR(255) = N'Lỗi hệ thống: Không tìm thấy danh mục có ID = ' + CAST(@Id AS NVARCHAR(10));
        RAISERROR(@ErrMsg, 16, 1);
        RETURN;
    END

    -- 2. Trả về dữ liệu nếu hợp lệ
    SELECT 
        [Id],
        [Name],
        [Description],
        [Code],
        [RecordStatus]
    FROM [Categories]
    WHERE [Id] = @Id;
END
GO
```

## 3. Check Category Code Exists
Useful for validation during Create/Update.

```sql
CREATE OR ALTER PROCEDURE sp_CheckCategoryCodeExisted
    @Code NVARCHAR(100),
    @CurrentId INT = 0
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1 
        FROM [Categories] 
        WHERE [Code] = @Code AND [Id] != @CurrentId
    )
    BEGIN
        SELECT CAST(1 AS BIT) AS Exists;
    END
    ELSE
    BEGIN
        SELECT CAST(0 AS BIT) AS Exists;
    END
END
GO
```

## 4. Update Category (With Validation)
This procedure updates an existing category with built-in data validation.

```sql
CREATE OR ALTER PROCEDURE sp_UpdateCategory
    @Id INT,
    @Name NVARCHAR(100),
    @Description NVARCHAR(255),
    @Code NVARCHAR(100),
    @RecordStatus NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Success BIT = 0;
    DECLARE @Message NVARCHAR(255) = '';

    -- 1. Validate: Kiểm tra tồn tại
    IF NOT EXISTS (SELECT 1 FROM [Categories] WHERE [Id] = @Id)
    BEGIN
        SET @Message = N'Lỗi: Không tìm thấy danh mục có ID = ' + CAST(@Id AS NVARCHAR(10));
    END
    -- 2. Validate: Tên không được để trống
    ELSE IF ISNULL(TRIM(@Name), '') = ''
    BEGIN
        SET @Message = N'Lỗi: Tên danh mục không được để trống.';
    END
    -- 3. Validate: Mã không được để trống
    ELSE IF ISNULL(TRIM(@Code), '') = ''
    BEGIN
        SET @Message = N'Lỗi: Mã danh mục không được để trống.';
    END
    -- 4. Validate: Kiểm tra trùng mã với danh mục khác
    ELSE IF EXISTS (SELECT 1 FROM [Categories] WHERE [Code] = TRIM(@Code) AND [Id] != @Id)
    BEGIN
        SET @Message = N'Lỗi: Mã danh mục [' + TRIM(@Code) + N'] đã tồn tại hệ thống.';
    END
    ELSE
    BEGIN
        -- 5. Thực hiện cập nhật
        UPDATE [Categories]
        SET 
            [Name] = TRIM(@Name),
            [Description] = TRIM(@Description),
            [Code] = UPPER(TRIM(@Code)),
            [RecordStatus] = @RecordStatus
        WHERE [Id] = @Id;

        SET @Success = 1;
        SET @Message = N'Cập nhật danh mục thành công!';
    END

    -- 6. Trả về kết quả dạng bảng phục vụ C# xử lý
    SELECT @Success AS Success, @Message AS Message;
END
GO
```

## 5. Insert Category (With Validation)
This procedure inserts a new category with built-in validation logic and default RecordStatus.

```sql
CREATE OR ALTER PROCEDURE sp_InsertCategory
    @Name NVARCHAR(100),
    @Description NVARCHAR(255),
    @Code NVARCHAR(100),
    @RecordStatus NVARCHAR(50) = '1' -- Default to '1' if not provided
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Success BIT = 0;
    DECLARE @Message NVARCHAR(255) = '';
    DECLARE @NewId INT = 0;

    -- 1. Validate: Name is required
    IF ISNULL(TRIM(@Name), '') = ''
    BEGIN
        SET @Message = N'Lỗi: Tên danh mục không được để trống.';
    END
    -- 2. Validate: Code is required
    ELSE IF ISNULL(TRIM(@Code), '') = ''
    BEGIN
        SET @Message = N'Lỗi: Mã danh mục không được để trống.';
    END
    -- 3. Validate: Check if Code already exists
    ELSE IF EXISTS (SELECT 1 FROM [Categories] WHERE [Code] = TRIM(@Code))
    BEGIN
        SET @Message = N'Lỗi: Mã danh mục [' + TRIM(@Code) + N'] đã tồn tại.';
    END
    ELSE
    BEGIN
        -- 4. Insert new record
        INSERT INTO [Categories] ([Name], [Description], [Code], [RecordStatus])
        VALUES (TRIM(@Name), TRIM(@Description), UPPER(TRIM(@Code)), @RecordStatus);

        SET @NewId = SCOPE_IDENTITY();
        SET @Success = 1;
        SET @Message = N'Thêm danh mục mới thành công!';
    END

    -- 5. Return result
    SELECT @Success AS Success, @Message AS Message, @NewId AS NewId;
END
GO

## 6. Delete Category (Soft Delete)
This procedure performs a soft delete by setting RecordStatus to '0'.

```sql
CREATE OR ALTER PROCEDURE sp_DeleteCategory
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Success BIT = 0;
    DECLARE @Message NVARCHAR(255) = '';

    -- 1. Validate: Kiểm tra tồn tại
    IF NOT EXISTS (SELECT 1 FROM [Categories] WHERE [Id] = @Id)
    BEGIN
        SET @Message = N'Lỗi: Không tìm thấy danh mục có ID = ' + CAST(@Id AS NVARCHAR(10));
    END
    -- 2. Kiểm tra xem đã bị xóa trước đó chưa (tùy chọn)
    ELSE IF EXISTS (SELECT 1 FROM [Categories] WHERE [Id] = @Id AND [RecordStatus] = '0')
    BEGIN
        SET @Message = N'Thông báo: Danh mục này đã được xóa từ trước.';
        SET @Success = 1; -- Coi như thành công nếu đã xóa rồi
    END
    ELSE
    BEGIN
        -- 3. Thực hiện Soft Delete
        UPDATE [Categories]
        SET [RecordStatus] = '0'
        WHERE [Id] = @Id;

        SET @Success = 1;
        SET @Message = N'Xóa danh mục thành công!';
    END

    -- 4. Trả về kết quả
    SELECT @Success AS Success, @Message AS Message;
END
GO
```
```
