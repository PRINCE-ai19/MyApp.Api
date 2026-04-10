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

## 4. Update Category
This procedure updates an existing category.

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

    UPDATE [Categories]
    SET 
        [Name] = @Name,
        [Description] = @Description,
        [Code] = @Code,
        [RecordStatus] = @RecordStatus
    WHERE [Id] = @Id;

    -- Return the number of updated rows
    SELECT @@ROWCOUNT AS UpdatedCount;
END
GO
```
