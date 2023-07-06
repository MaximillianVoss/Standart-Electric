IF DB_ID('DBSE') IS NULL
BEGIN
    -- Recreate the database
    CREATE DATABASE [DBSE]
END
GO
USE [DBSE]
GO

--#region Очистка БД

--#region DROP ALL non-system stored procs
DECLARE @name VARCHAR(128)
DECLARE @SQL VARCHAR(254)

SELECT
    @name = (SELECT
        TOP 1
        [name]
    FROM
        sysobjects
    WHERE [type] = 'P' AND category = 0
    ORDER BY [name])

WHILE @name IS NOT NULL
BEGIN
    SELECT
        @SQL = 'DROP PROCEDURE [dbo].[' + RTRIM(@name) +']'
    EXEC (@SQL)
    PRINT 'Dropped Procedure: ' + @name
    SELECT
        @name = (SELECT
            TOP 1
            [name]
        FROM
            sysobjects
        WHERE [type] = 'P' AND category = 0 AND [name] > @name
        ORDER BY [name])
END
GO
--#endregion

--#region DROP ALL views
DECLARE @name VARCHAR(128)
DECLARE @SQL VARCHAR(254)

SELECT
    @name = (SELECT
        TOP 1
        [name]
    FROM
        sysobjects
    WHERE [type] = 'V' AND category = 0
    ORDER BY [name])

WHILE @name IS NOT NULL
BEGIN
    SELECT
        @SQL = 'DROP VIEW [dbo].[' + RTRIM(@name) +']'
    EXEC (@SQL)
    PRINT 'Dropped View: ' + @name
    SELECT
        @name = (SELECT
            TOP 1
            [name]
        FROM
            sysobjects
        WHERE [type] = 'V' AND category = 0 AND [name] > @name
        ORDER BY [name])
END
GO
--#endregion

--#region DROP ALL functions
DECLARE @name VARCHAR(128)
DECLARE @SQL VARCHAR(254)

SELECT
    @name = (SELECT
        TOP 1
        [name]
    FROM
        sysobjects
    WHERE [type] IN (N'FN', N'IF', N'TF', N'FS', N'FT') AND category = 0
    ORDER BY [name])

WHILE @name IS NOT NULL
BEGIN
    SELECT
        @SQL = 'DROP FUNCTION [dbo].[' + RTRIM(@name) +']'
    EXEC (@SQL)
    PRINT 'Dropped Function: ' + @name
    SELECT
        @name = (SELECT
            TOP 1
            [name]
        FROM
            sysobjects
        WHERE [type] IN (N'FN', N'IF', N'TF', N'FS', N'FT') AND category = 0 AND [name] > @name
        ORDER BY [name])
END
GO
--#endregion

--#region DROP ALL FOREIGN KEY constraints
DECLARE @name VARCHAR(128)
DECLARE @constraint VARCHAR(254)
DECLARE @SQL VARCHAR(254)

SELECT
    @name = (SELECT
        TOP 1
        TABLE_NAME
    FROM
        INFORMATION_SCHEMA.TABLE_CONSTRAINTS
    WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'FOREIGN KEY'
    ORDER BY TABLE_NAME)

WHILE @name IS NOT NULL
BEGIN
    SELECT
        @constraint = (SELECT
            TOP 1
            CONSTRAINT_NAME
        FROM
            INFORMATION_SCHEMA.TABLE_CONSTRAINTS
        WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'FOREIGN KEY' AND TABLE_NAME = @name
        ORDER BY CONSTRAINT_NAME)
    WHILE @constraint IS NOT NULL
    BEGIN
        SELECT
            @SQL = 'ALTER TABLE [dbo].[' + RTRIM(@name) +'] DROP CONSTRAINT [' + RTRIM(@constraint) +']'
        EXEC (@SQL)
        PRINT 'Dropped FK Constraint: ' + @constraint + ' on ' + @name
        SELECT
            @constraint = (SELECT
                TOP 1
                CONSTRAINT_NAME
            FROM
                INFORMATION_SCHEMA.TABLE_CONSTRAINTS
            WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'FOREIGN KEY' AND CONSTRAINT_NAME <> @constraint AND TABLE_NAME = @name
            ORDER BY CONSTRAINT_NAME)
    END
    SELECT
        @name = (SELECT
            TOP 1
            TABLE_NAME
        FROM
            INFORMATION_SCHEMA.TABLE_CONSTRAINTS
        WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'FOREIGN KEY'
        ORDER BY TABLE_NAME)
END
GO
--#endregion

--#region DROP ALL PRIMARY KEY constraints
DECLARE @name VARCHAR(128)
DECLARE @constraint VARCHAR(254)
DECLARE @SQL VARCHAR(254)

SELECT
    @name = (SELECT
        TOP 1
        TABLE_NAME
    FROM
        INFORMATION_SCHEMA.TABLE_CONSTRAINTS
    WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'PRIMARY KEY'
    ORDER BY TABLE_NAME)

WHILE @name IS NOT NULL
BEGIN
    SELECT
        @constraint = (SELECT
            TOP 1
            CONSTRAINT_NAME
        FROM
            INFORMATION_SCHEMA.TABLE_CONSTRAINTS
        WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = @name
        ORDER BY CONSTRAINT_NAME)
    WHILE @constraint IS NOT NULL
    BEGIN
        SELECT
            @SQL = 'ALTER TABLE [dbo].[' + RTRIM(@name) +'] DROP CONSTRAINT [' + RTRIM(@constraint)+']'
        EXEC (@SQL)
        PRINT 'Dropped PK Constraint: ' + @constraint + ' on ' + @name
        SELECT
            @constraint = (SELECT
                TOP 1
                CONSTRAINT_NAME
            FROM
                INFORMATION_SCHEMA.TABLE_CONSTRAINTS
            WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'PRIMARY KEY' AND CONSTRAINT_NAME <> @constraint AND TABLE_NAME = @name
            ORDER BY CONSTRAINT_NAME)
    END
    SELECT
        @name = (SELECT
            TOP 1
            TABLE_NAME
        FROM
            INFORMATION_SCHEMA.TABLE_CONSTRAINTS
        WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'PRIMARY KEY'
        ORDER BY TABLE_NAME)
END
GO
--#endregion

--#region DROP ALL TABLES
DECLARE @name VARCHAR(128)
DECLARE @SQL VARCHAR(254)

SELECT
    @name = (SELECT
        TOP 1
        [name]
    FROM
        sysobjects
    WHERE [type] = 'U' AND category = 0
    ORDER BY [name])

WHILE @name IS NOT NULL
BEGIN
    SELECT
        @SQL = 'DROP TABLE [dbo].[' + RTRIM(@name) +']'
    EXEC (@SQL)
    PRINT 'Dropped Table: ' + @name
    SELECT
        @name = (SELECT
            TOP 1
            [name]
        FROM
            sysobjects
        WHERE [type] = 'U' AND category = 0 AND [name] > @name
        ORDER BY [name])
END
GO
--#endregion

--#endregion

--#region Генератор случайных чисел

--#region Представление для работы генератора случайных чисел
--#region Удаление представления
GO
DROP VIEW IF EXISTS dbo.vRand; 
--#endregion
--#region Создание представления
GO
CREATE VIEW dbo.[vRand]
(
    V
)
AS
    SELECT
        RAND()
--#endregion
--#endregion

--#region Процедура для генерации случайного числа в указанном промежутке
--#region Удаление
GO
IF OBJECT_ID (N'dbo.GetRandInt', N'FN') IS NOT NULL  
    DROP FUNCTION GetRandInt; 
--#endregion
--#region Создание
GO
CREATE FUNCTION dbo.GetRandInt(@min INT,@max INT)  
RETURNS INT   
AS   
BEGIN
    RETURN (SELECT
        FLOOR(@max*V + @min)
    FROM
        dbo.vRand)
END;
--#endregion
--#endregion

--#endregion

--#region Создание таблиц
GO
CREATE TABLE [SubGroups]
(
    [id] int IDENTITY (1, 1) ,
    -- 

    [idDescriptor] int NOT NULL ,
    -- id дескриптора-описания
    [idGroup] int ,
    -- id группы
    [idLoadDiagram] int -- id схемы нагрузки, 
        PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Groups]
(
    [id] int IDENTITY (1, 1) ,
    -- 

    [idDescriptor] int NOT NULL ,
    -- id дескриптора-описания
    [idClass] int  -- 
                  ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [BuisnessUnits]
(
    [id] int IDENTITY (1, 1) ,
    -- 

    [idDescriptor] int NOT NULL ,
    -- id дескриптора-описания
    [idBimLibraryFile] int ,
    -- 

    [idDrawBlocksFile] int ,
    -- 

    [idTypicalAlbum] int  -- 
                  ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Classes]
(
    [id] int IDENTITY (1, 1) ,
    -- 

    [idDescriptor] int NOT NULL -- id дескриптора-описания, 
        PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Applications]
(
    [id] int IDENTITY (1, 1) ,
    -- 

    [idDescriptor] int NOT NULL ,
    -- id дескриптора-описания
    [idBuisnessUnit] int -- id направления, 
        PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Perforations]
(
    [id] int IDENTITY (1, 1) ,
    -- 

    [idDescriptor] int NOT NULL -- id дескриптора-описания, 
        PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Norms]
(
    [id] int IDENTITY (1, 1) ,
    -- 

    [idDescriptor] int NOT NULL -- id дескриптора-описания, 
        PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Materials]
(
    [id] int IDENTITY (1, 1) ,
    -- 

    [idDescriptor] int NOT NULL ,
    [idMaterialNorm] int ,
    -- Ссылка на нормативный документ
    [idPrepackNorm] int ,
    -- Ссылка на норматив 
    [type] nVarchar(255) -- Обозначение материала или марка стали, 
        PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [LoadDiagrams]
(
    [id] int IDENTITY (1, 1) ,
    -- 

    [idDescriptor] int -- id дескриптора-описания, 
        PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [GroupsApplications]
(
    [id] int IDENTITY (1, 1) ,
    -- 

    [idApplication] int ,
    -- id применения
    [idSubGroup] int -- id группы, 
        PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Covers]
(
    [id] int IDENTITY (1, 1) ,
    -- 

    [idDescriptor] int NOT NULL ,
    [idNorm] int NOT NULL ,
    -- id Стандарта
    [thickness] int -- Толщина покрытия, 
        PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Products]
(
    [id] int IDENTITY (1, 1) ,
    -- 

    [idDescriptor] int NOT NULL ,
    -- id дескриптора-описания
    [idNorm] int ,
    -- id Стандарта
    [idSubGroup] int ,
    -- Название норматива
    [idCover] int ,
    -- Покрытие
    [idMaterial] int ,
    -- Ссылка на нормативный документ
    [idPerforation] int ,
    -- Номер типа перфорации
    [idPackage] int ,
    -- id упаковки
    [isInStock] bit NOT NULL -- Складская позиция 1 или заказная 0, 
        PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Units]
(
    [id] int IDENTITY (1, 1) ,
    -- 

    [idDescriptor] int NOT NULL ,
    [OKEI] int -- Общероссийский классификатор единиц измерения, 
        PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [ProductsVendorCodes]
(
    [id] int IDENTITY (1, 1) ,
    -- 

    [idProduct] int NOT NULL ,
    -- 

    [idCode] int NOT NULL  -- 
                  ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [VendorCodes]
(
    [id] int IDENTITY (1, 1) ,
    -- 

    [idManufacturer] int NOT NULL ,
    [idDescriptor] int NOT NULL ,
    [isActual] bit NOT NULL ,
    -- устаревший или актуальный
    [isSale] bit NOT NULL ,
    -- внешний лии внутренний
    [isPublic] bit ,
    -- Является ли артикул публичным (можно ли экспортировать)
    [codeAccountant] nvarchar(255) ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [ProductsAnalogs]
(
    [id] int IDENTITY (1, 1) ,
    -- 

    [idOriginal] int ,
    -- id оригинала
    [idAnalog] int -- id аналога, 
        PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Descriptors]
(
    [id] int IDENTITY (1, 1) ,
    [code] nvarchar(255) ,
    [title] nvarchar(255) ,
    [titleShort] nvarchar(255) ,
    [description] nvarchar(2048) ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [UnitsProducts]
(
    [id] int IDENTITY (1, 1) ,
    [idProduct] int NOT NULL ,
    [idUnit] int ,
    [idType] int ,
    [value] float NOT NULL ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [UnitsTypes]
(
    [id] int IDENTITY (1, 1) ,
    [title] nvarchar(255) NOT NULL ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Packages]
(
    [id] int IDENTITY (1, 1) ,
    [idDescriptor] int NOT NULL -- id дескриптора-описания, 
        PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [UnitsPerforations]
(
    [id] int NOT NULL IDENTITY (1, 1) ,
    [idPerforation] int NOT NULL ,
    [idUnit] int NOT NULL ,
    [idType] int NOT NULL ,
    [Value] float NOT NULL ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [UnitsPakages]
(
    [id] int NOT NULL IDENTITY (1, 1) ,
    [idPackage] int NOT NULL ,
    [idUnit] int ,
    [idType] int ,
    [Value] float ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Manufacturers]
(
    [id] int IDENTITY (1, 1) ,
    [idDescriptor] int NOT NULL ,
    [TIN] varchar(20) ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Tables]
(
    [id] int IDENTITY (1, 1) ,
    [idDescriptor] int ,
    [titleRus] nvarchar(255) ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Fields]
(
    [id] int IDENTITY (1, 1) ,
    [idDescriptor] int ,
    [titleRus] nvarchar(255) ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [DescriptorsResources]
(
    [id] int IDENTITY (1, 1) ,
    [idDescriptor] int NOT NULL ,
    [idResource] int NOT NULL ,
    [idResourceType] int ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Resources]
(
    [id] int IDENTITY (1, 1) ,
    [URL] nvarchar(2048) ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [ResourceTypes]
(
    [id] int IDENTITY (1, 1) ,
    [title] nvarchar(255) NOT NULL ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Views]
(
    [id] int IDENTITY (1, 1) ,
    [idDescriptor] int NOT NULL ,
    [idTable] int ,
    [titleRus] nVarchar(255) ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

ALTER TABLE [SubGroups] ADD FOREIGN KEY (idDescriptor) REFERENCES [Descriptors] ([id]);

ALTER TABLE [SubGroups] ADD FOREIGN KEY (idGroup) REFERENCES [Groups] ([id]);

ALTER TABLE [SubGroups] ADD FOREIGN KEY (idLoadDiagram) REFERENCES [LoadDiagrams] ([id]);

ALTER TABLE [Groups] ADD FOREIGN KEY (idDescriptor) REFERENCES [Descriptors] ([id]);

ALTER TABLE [Groups] ADD FOREIGN KEY (idClass) REFERENCES [Classes] ([id]);

ALTER TABLE [BuisnessUnits] ADD FOREIGN KEY (idDescriptor) REFERENCES [Descriptors] ([id]);

ALTER TABLE [BuisnessUnits] ADD FOREIGN KEY (idBimLibraryFile) REFERENCES [Descriptors] ([id]);

ALTER TABLE [BuisnessUnits] ADD FOREIGN KEY (idDrawBlocksFile) REFERENCES [Descriptors] ([id]);

ALTER TABLE [BuisnessUnits] ADD FOREIGN KEY (idTypicalAlbum) REFERENCES [Descriptors] ([id]);

ALTER TABLE [Classes] ADD FOREIGN KEY (idDescriptor) REFERENCES [Descriptors] ([id]);

ALTER TABLE [Applications] ADD FOREIGN KEY (idDescriptor) REFERENCES [Descriptors] ([id]);

ALTER TABLE [Applications] ADD FOREIGN KEY (idBuisnessUnit) REFERENCES [BuisnessUnits] ([id]);

ALTER TABLE [Perforations] ADD FOREIGN KEY (idDescriptor) REFERENCES [Descriptors] ([id]);

ALTER TABLE [Norms] ADD FOREIGN KEY (idDescriptor) REFERENCES [Descriptors] ([id]);

ALTER TABLE [Materials] ADD FOREIGN KEY (idDescriptor) REFERENCES [Descriptors] ([id]);

ALTER TABLE [Materials] ADD FOREIGN KEY (idMaterialNorm) REFERENCES [Norms] ([id]);

ALTER TABLE [Materials] ADD FOREIGN KEY (idPrepackNorm) REFERENCES [Norms] ([id]);

ALTER TABLE [LoadDiagrams] ADD FOREIGN KEY (idDescriptor) REFERENCES [Descriptors] ([id]);

ALTER TABLE [GroupsApplications] ADD FOREIGN KEY (idApplication) REFERENCES [Applications] ([id]);

ALTER TABLE [GroupsApplications] ADD FOREIGN KEY (idSubGroup) REFERENCES [SubGroups] ([id]);

ALTER TABLE [Covers] ADD FOREIGN KEY (idDescriptor) REFERENCES [Descriptors] ([id]);

ALTER TABLE [Covers] ADD FOREIGN KEY (idNorm) REFERENCES [Norms] ([id]);

ALTER TABLE [Products] ADD FOREIGN KEY (idDescriptor) REFERENCES [Descriptors] ([id]);

ALTER TABLE [Products] ADD FOREIGN KEY (idNorm) REFERENCES [Norms] ([id]);

ALTER TABLE [Products] ADD FOREIGN KEY (idSubGroup) REFERENCES [SubGroups] ([id]);

ALTER TABLE [Products] ADD FOREIGN KEY (idCover) REFERENCES [Covers] ([id]);

ALTER TABLE [Products] ADD FOREIGN KEY (idMaterial) REFERENCES [Materials] ([id]);

ALTER TABLE [Products] ADD FOREIGN KEY (idPerforation) REFERENCES [Perforations] ([id]);

ALTER TABLE [Products] ADD FOREIGN KEY (idPackage) REFERENCES [Packages] ([id]);

ALTER TABLE [Units] ADD FOREIGN KEY (idDescriptor) REFERENCES [Descriptors] ([id]);

ALTER TABLE [ProductsVendorCodes] ADD FOREIGN KEY (idProduct) REFERENCES [Products] ([id]);

ALTER TABLE [ProductsVendorCodes] ADD FOREIGN KEY (idCode) REFERENCES [VendorCodes] ([id]);

ALTER TABLE [VendorCodes] ADD FOREIGN KEY (idManufacturer) REFERENCES [Manufacturers] ([id]);

ALTER TABLE [VendorCodes] ADD FOREIGN KEY (idDescriptor) REFERENCES [Descriptors] ([id]);

ALTER TABLE [ProductsAnalogs] ADD FOREIGN KEY (idOriginal) REFERENCES [Products] ([id]);

ALTER TABLE [ProductsAnalogs] ADD FOREIGN KEY (idAnalog) REFERENCES [Products] ([id]);

ALTER TABLE [UnitsProducts] ADD FOREIGN KEY (idProduct) REFERENCES [Products] ([id]);

ALTER TABLE [UnitsProducts] ADD FOREIGN KEY (idUnit) REFERENCES [Units] ([id]);

ALTER TABLE [UnitsProducts] ADD FOREIGN KEY (idType) REFERENCES [UnitsTypes] ([id]);

ALTER TABLE [Packages] ADD FOREIGN KEY (idDescriptor) REFERENCES [Descriptors] ([id]);

ALTER TABLE [UnitsPerforations] ADD FOREIGN KEY (idPerforation) REFERENCES [Perforations] ([id]);

ALTER TABLE [UnitsPerforations] ADD FOREIGN KEY (idUnit) REFERENCES [Units] ([id]);

ALTER TABLE [UnitsPerforations] ADD FOREIGN KEY (idType) REFERENCES [UnitsTypes] ([id]);

ALTER TABLE [UnitsPakages] ADD FOREIGN KEY (idPackage) REFERENCES [Packages] ([id]);

ALTER TABLE [UnitsPakages] ADD FOREIGN KEY (idUnit) REFERENCES [Units] ([id]);

ALTER TABLE [UnitsPakages] ADD FOREIGN KEY (idType) REFERENCES [UnitsTypes] ([id]);

ALTER TABLE [Manufacturers] ADD FOREIGN KEY (idDescriptor) REFERENCES [Descriptors] ([id]);

ALTER TABLE [Tables] ADD FOREIGN KEY (idDescriptor) REFERENCES [Descriptors] ([id]);

ALTER TABLE [Fields] ADD FOREIGN KEY (idDescriptor) REFERENCES [Descriptors] ([id]);

ALTER TABLE [DescriptorsResources] ADD FOREIGN KEY (idDescriptor) REFERENCES [Descriptors] ([id]);

ALTER TABLE [DescriptorsResources] ADD FOREIGN KEY (idResource) REFERENCES [Resources] ([id]);

ALTER TABLE [DescriptorsResources] ADD FOREIGN KEY (idResourceType) REFERENCES [ResourceTypes] ([id]);

ALTER TABLE [Views] ADD FOREIGN KEY (idDescriptor) REFERENCES [Descriptors] ([id]);

ALTER TABLE [Views] ADD FOREIGN KEY (idTable) REFERENCES [Tables] ([id]);
--#endregion

--#region Заполнение таблиц

--#endregion

--#region Представления

--#region Представление vAllProductsInfo

--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[vAllProductsInfo]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[vAllProductsInfo]
AS
    (
    SELECT Products.id, Products.idDescriptor, Products.idNorm, Products.idSubGroup, Products.idCover, Products.idMaterial, Products.isInStock, Products.idPerforation, Products.idPackage,
        Descriptors.code, Descriptors.title, Descriptors.titleShort, Descriptors.description,
        Norms.idDescriptor AS idDescriptorNorm, SubGroups.idGroup, SubGroups.idDescriptor AS idDescriptorSubGroups, SubGroups.idLoadDiagram, Covers.thickness,
        Covers.idNorm AS idNormCovers, Materials.type AS typeMaterials, Materials.idMaterialNorm, Materials.idPrepackNorm, Perforations.idDescriptor AS idDescriptorPerforations,
        Packages.idDescriptor AS idDescriptorPackages, Materials.idDescriptor AS idDescriptorMaterials, Covers.idDescriptor AS idDescriptorCovers
    FROM Products INNER JOIN
        Descriptors ON Products.idDescriptor = Descriptors.id INNER JOIN
        Norms ON Products.idNorm = Norms.id AND Descriptors.id = Norms.idDescriptor INNER JOIN
        SubGroups ON Products.idSubGroup = SubGroups.id AND Descriptors.id = SubGroups.idDescriptor INNER JOIN
        Covers ON Products.idCover = Covers.id AND Norms.id = Covers.idNorm INNER JOIN
        Materials ON Products.idMaterial = Materials.id AND Norms.id = Materials.idMaterialNorm AND Norms.id = Materials.idPrepackNorm INNER JOIN
        Perforations ON Products.idPerforation = Perforations.id AND Descriptors.id = Perforations.idDescriptor INNER JOIN
        Packages ON Products.idPackage = Packages.id AND Descriptors.id = Packages.idDescriptor
);
--#endregion
--#endregion

--#region Представление vAllProductUnitsInfo
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[vAllProductUnitsInfo]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[vAllProductUnitsInfo]
AS
    (
    SELECT UnitsProducts.id, UnitsProducts.idProduct, UnitsProducts.idUnit, UnitsProducts.idType, UnitsProducts.value, UnitsTypes.title AS titleUnitType, Units.OKEI, Descriptors.title AS titleUnit, Descriptors.description AS descriptionUnit,
        Descriptors.titleShort AS titleShortUnit
    FROM UnitsProducts INNER JOIN
        Units ON UnitsProducts.idUnit = Units.id INNER JOIN
        UnitsTypes ON UnitsProducts.idType = UnitsTypes.id INNER JOIN
        Descriptors ON Units.idDescriptor = Descriptors.id
);
--#endregion
--#endregion



--#endregion

--#region Триггеры

--#region Триггер trg_Descriptors_Insert
--#region Создание
-- GO
-- CREATE TRIGGER [dbo].[trg_Descriptors_Insert]
-- ON [dbo].[Descriptors]
-- AFTER INSERT
-- AS
-- BEGIN
--     DECLARE @idResource TABLE (id INT)
--     -- Insert the new row into the Resources table
--     INSERT INTO Resources
--         (URL)
--     OUTPUT inserted.id INTO @idResource
--     SELECT 'Resources/images/' + code + '.png'
--     FROM inserted

--     -- Insert the new row into the DescriptorsResources table
--     INSERT INTO DescriptorsResources
--         (idDescriptor, idResource, idResourceType)
--     SELECT id, (SELECT TOP 1
--             id
--         FROM @idResource), (SELECT id
--         FROM ResourceTypes
--         WHERE title = 'Image')
--     FROM inserted
-- END

--#endregion
--#endregion



--#region Триггер НАЗВАНИЕ

--#region Создание
GO
CREATE TRIGGER trg_AddProductImagePath
ON dbo.Products
AFTER INSERT
AS
BEGIN
    PRINT 'trg_AddProductImagePath start'
    DECLARE @imagePath nvarchar(2048)
    DECLARE @vendorCode nvarchar(255)
    DECLARE @productId int
    DECLARE @descriptorId int
    DECLARE @resourceTypeId int
    DECLARE @resourceId int

    SELECT TOP 1
        @productId = id,
        @descriptorId = idDescriptor
    FROM INSERTED

    -- Get vendor code for the product
    SELECT TOP 1
        @vendorCode = dvc.title
    FROM VendorCodes vc
        INNER JOIN ProductsVendorCodes pvc ON pvc.idCode = vc.id
        INNER JOIN Descriptors as dvc ON vc.idDescriptor = dvc.id
    WHERE pvc.idProduct = @productId

    IF @vendorCode IS NULL
    BEGIN
        PRINT 'Vendor code is NULL. Cannot create image path.'
        RETURN
    END

    -- Create image path using vendor code and add to Resources table
    SET @imagePath = 'Resources/images/' + @vendorCode + '.png'
    INSERT INTO Resources
        (URL)
    VALUES
        (@imagePath)

    -- Get ResourceTypes id for 'image'
    SELECT TOP 1
        @resourceTypeId = id
    FROM ResourceTypes
    WHERE title = 'image'

    -- Get Resource id for the added image
    SELECT TOP 1
        @resourceId = id
    FROM Resources
    WHERE URL = @imagePath

    -- Check if resourceId is not NULL before inserting into DescriptorsResources
    IF @resourceId IS NOT NULL
    BEGIN
        INSERT INTO DescriptorsResources
            (idDescriptor, idResource, idResourceType)
        VALUES
            (@descriptorId, @resourceId, @resourceTypeId)
    END
    ELSE
    BEGIN
        PRINT 'Resource ID is NULL. Cannot insert into DescriptorsResources.'
    END
END



--#endregion
--#endregion







--#endregion

--#region Хранимые процедуры

--#region Процедура AddProductImagePaths 
--#region Создание
GO
CREATE PROCEDURE [dbo].[AddProductImagePaths]
AS
BEGIN
    PRINT 'trg_AddProductImagePath start'
    DECLARE @imagePath nvarchar(2048)
    DECLARE @vendorCode nvarchar(255)

    -- Create ResourceType 'image' if it does not exist
    IF NOT EXISTS (SELECT *
    FROM ResourceTypes
    WHERE title = 'image')
    BEGIN
        INSERT INTO ResourceTypes
            (title)
        VALUES
            ('image')
    END

    -- Loop through each product and add image path to Resources table
    DECLARE @productId int
    DECLARE product_cursor CURSOR FOR 
        SELECT id
    FROM Products
    OPEN product_cursor
    FETCH NEXT FROM product_cursor INTO @productId
    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Get vendor code for the product
        SELECT TOP 1
            @vendorCode = dvc.title
        FROM VendorCodes vc
            INNER JOIN ProductsVendorCodes pvc ON pvc.idCode = vc.id
            INNER JOIN Descriptors as dvc ON vc.idDescriptor = dvc.id
        WHERE pvc.idProduct = @productId

        -- Create image path using vendor code and add to Resources table
        SET @imagePath = 'Resources/images/' + @vendorCode + '.png'
        INSERT INTO Resources
            (URL)
        VALUES
            (@imagePath)

        -- Get ResourceTypes id for 'image'
        DECLARE @resourceTypeId int
        SELECT TOP 1
            @resourceTypeId = id
        FROM ResourceTypes
        WHERE title = 'image'

        -- Get Resource id for the added image
        DECLARE @resourceId int
        SELECT TOP 1
            @resourceId = id
        FROM Resources
        WHERE URL = @imagePath

        -- Add entry to DescriptorsResources table
        DECLARE @descriptorId int
        SELECT TOP 1
            @descriptorId = idDescriptor
        FROM Products
        WHERE id = @productId
        INSERT INTO DescriptorsResources
            (idDescriptor, idResource, idResourceType)
        VALUES
            (@descriptorId, @resourceId, @resourceTypeId)

        FETCH NEXT FROM product_cursor INTO @productId
    END
    CLOSE product_cursor
    DEALLOCATE product_cursor
END
--#endregion
--#endregion

--#region Процедура AddSubGroupImagePaths 
--#region Создание
GO
CREATE PROCEDURE [dbo].[AddSubGroupImagePaths]
AS
BEGIN
    DECLARE @imagePath nvarchar(2048)
    DECLARE @vendorCode nvarchar(255)

    -- Create ResourceType 'image' if it does not exist
    IF NOT EXISTS (SELECT *
    FROM ResourceTypes
    WHERE title = 'image')
    BEGIN
        INSERT INTO ResourceTypes
            (title)
        VALUES
            ('image')
    END

    -- Loop through each sub group and add image path to Resources table
    DECLARE @subGroupId int
    DECLARE sub_group_cursor CURSOR FOR 
        SELECT id
    FROM SubGroups
    OPEN sub_group_cursor
    FETCH NEXT FROM sub_group_cursor INTO @subGroupId
    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Get vendor code for the sub group
        -- SELECT TOP 1
        --     @vendorCode = vc.code
        -- FROM VendorCodes vc
        --     INNER JOIN Classes c ON vc.idDescriptor = c.idDescriptor
        --     INNER JOIN Groups g ON g.idClass = c.id
        --     INNER JOIN SubGroups sg ON sg.idGroup = g.id
        -- WHERE sg.id = @subGroupId
        SELECT TOP 1
            @vendorCode = sgd.code
        FROM SubGroups as sg
            INNER JOIN Descriptors as sgd ON sgd.id = sg.idDescriptor
        WHERE sg.id = @subGroupId

        -- Create image path using vendor code and add to Resources table
        SET @imagePath = 'Resources/images/' + @vendorCode + '.png'
        INSERT INTO Resources
            (URL)
        VALUES
            (@imagePath)

        -- Get ResourceTypes id for 'image'
        DECLARE @resourceTypeId int
        SELECT TOP 1
            @resourceTypeId = id
        FROM ResourceTypes
        WHERE title = 'image'

        -- Get Resource id for the added image
        DECLARE @resourceId int
        SELECT TOP 1
            @resourceId = id
        FROM Resources
        WHERE URL = @imagePath

        -- Add entry to DescriptorsResources table
        DECLARE @descriptorId int
        SELECT TOP 1
            @descriptorId = idDescriptor
        FROM SubGroups
        WHERE id = @subGroupId
        INSERT INTO DescriptorsResources
            (idDescriptor, idResource, idResourceType)
        VALUES
            (@descriptorId, @resourceId, @resourceTypeId)

        FETCH NEXT FROM sub_group_cursor INTO @subGroupId
    END
    CLOSE sub_group_cursor
    DEALLOCATE sub_group_cursor
END

--#endregion
--#endregion

--#endregion
 

--#region Представление vAllProductsInfo
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[vAllProductsInfo]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[vAllProductsInfo]
AS
(
SELECT        Products.id, Products.idDescriptor, Products.idNorm, Products.idSubGroup, Products.idCover, Products.idMaterial, Products.isInStock, Products.idPerforation, Products.idPackage, 
                         Descriptors.code, Descriptors.title, Descriptors.titleShort, Descriptors.description, 
                         Norms.idDescriptor AS idDescriptorNorm, SubGroups.idGroup, SubGroups.idDescriptor AS idDescriptorSubGroups, SubGroups.idLoadDiagram,  Covers.thickness, 
                         Covers.idNorm AS idNormCovers, Materials.type AS typeMaterials, Materials.idMaterialNorm, Materials.idPrepackNorm, Perforations.idDescriptor AS idDescriptorPerforations, 
                         Packages.idDescriptor AS idDescriptorPackages, Materials.idDescriptor AS idDescriptorMaterials, Covers.idDescriptor AS idDescriptorCovers
FROM            Products INNER JOIN
                         Descriptors ON Products.idDescriptor = Descriptors.id INNER JOIN
                         Norms ON Products.idNorm = Norms.id AND Descriptors.id = Norms.idDescriptor INNER JOIN
                         SubGroups ON Products.idSubGroup = SubGroups.id AND Descriptors.id = SubGroups.idDescriptor INNER JOIN
                         Covers ON Products.idCover = Covers.id AND Norms.id = Covers.idNorm INNER JOIN
                         Materials ON Products.idMaterial = Materials.id AND Norms.id = Materials.idMaterialNorm AND Norms.id = Materials.idPrepackNorm INNER JOIN
                         Perforations ON Products.idPerforation = Perforations.id AND Descriptors.id = Perforations.idDescriptor INNER JOIN
                         Packages ON Products.idPackage = Packages.id AND Descriptors.id = Packages.idDescriptor
);
--#endregion
--#endregion



--#region Представление vAllProductUnitsInfo
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[vAllProductUnitsInfo]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[vAllProductUnitsInfo]
AS
(
	SELECT        UnitsProducts.id, UnitsProducts.idProduct, UnitsProducts.idUnit, UnitsProducts.idType, UnitsProducts.value, UnitsTypes.title AS titleUnitType, Units.OKEI, Descriptors.title AS titleUnit, Descriptors.description AS descriptionUnit, 
                         Descriptors.titleShort AS titleShortUnit
FROM            UnitsProducts INNER JOIN
                         Units ON UnitsProducts.idUnit = Units.id INNER JOIN
                         UnitsTypes ON UnitsProducts.idType = UnitsTypes.id INNER JOIN
                         Descriptors ON Units.idDescriptor = Descriptors.id
);
--#endregion
--#endregion

