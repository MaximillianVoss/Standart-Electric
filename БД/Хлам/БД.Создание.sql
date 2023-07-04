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
GO
--#endregion
--#endregion

--#endregion

--#region Создание таблиц

CREATE TABLE [SubGroups]
(
    [id]            INT IDENTITY (1, 1) ,
    -- 

    [idDescriptor]  INT NOT NULL ,
    -- id дескриптора-описания
    [idGroup]       INT ,
    -- id группы
    [idLoadDiagram] INT -- id схемы нагрузки, 
        PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Groups]
(
    [id]           INT IDENTITY (1, 1) ,
    -- 

    [idDescriptor] INT NOT NULL ,
    -- id дескриптора-описания
    [idClass]      INT  -- 
                  ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [BuisnessUnits]
(
    [id]               INT IDENTITY (1, 1) ,
    -- 

    [idDescriptor]     INT NOT NULL ,
    -- id дескриптора-описания
    [idBimLibraryFile] INT ,
    -- 

    [idDrawBlocksFile] INT ,
    -- 

    [idTypicalAlbum]   INT  -- 
                  ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Classes]
(
    [id]           INT IDENTITY (1, 1) ,
    -- 

    [idDescriptor] INT NOT NULL -- id дескриптора-описания, 
        PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Applications]
(
    [id]             INT IDENTITY (1, 1) ,
    -- 

    [idDescriptor]   INT NOT NULL ,
    -- id дескриптора-описания
    [idBuisnessUnit] INT -- id направления, 
        PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Perforations]
(
    [id]           INT IDENTITY (1, 1) ,
    -- 

    [idDescriptor] INT NOT NULL -- id дескриптора-описания, 
        PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Norms]
(
    [id]           INT IDENTITY (1, 1) ,
    -- 

    [idDescriptor] INT NOT NULL -- id дескриптора-описания, 
        PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Materials]
(
    [id]             INT           IDENTITY (1, 1) ,
    -- 

    [idDescriptor]   INT           NOT NULL ,
    [idMaterialNorm] INT ,
    -- Ссылка на нормативный документ
    [idPrepackNorm]  INT ,
    -- Ссылка на норматив 
    [type]           NVARCHAR(255) -- Обозначение материала или марка стали, 
        PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [LoadDiagrams]
(
    [id]           INT IDENTITY (1, 1) ,
    -- 

    [idDescriptor] INT -- id дескриптора-описания, 
        PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [GroupsApplications]
(
    [id]            INT IDENTITY (1, 1) ,
    -- 

    [idApplication] INT ,
    -- id применения
    [idSubGroup]    INT -- id группы, 
        PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Covers]
(
    [id]           INT IDENTITY (1, 1) ,
    -- 

    [idDescriptor] INT NOT NULL ,
    [idNorm]       INT NOT NULL ,
    -- id Стандарта
    [thickness]    INT -- Толщина покрытия, 
        PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Products]
(
    [id]            INT IDENTITY (1, 1) ,
    -- 

    [idDescriptor]  INT NOT NULL ,
    -- id дескриптора-описания
    [idNorm]        INT ,
    -- id Стандарта
    [idSubGroup]    INT ,
    -- Название норматива
    [idCover]       INT ,
    -- Покрытие
    [idMaterial]    INT ,
    -- Ссылка на нормативный документ
    [idPerforation] INT ,
    -- Номер типа перфорации
    [idPackage]     INT ,
    -- id упаковки
    [isInStock]     BIT NOT NULL -- Складская позиция 1 или заказная 0, 
        PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Units]
(
    [id]           INT IDENTITY (1, 1) ,
    -- 

    [idDescriptor] INT NOT NULL ,
    [OKEI]         INT -- Общероссийский классификатор единиц измерения, 
        PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [ProductsVendorCodes]
(
    [id]        INT IDENTITY (1, 1) ,
    -- 

    [idProduct] INT NOT NULL ,
    -- 

    [idCode]    INT NOT NULL  -- 
                  ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [VendorCodes]
(
    [id]             INT           IDENTITY (1, 1) ,
    -- 

    [idManufacturer] INT           NOT NULL ,
    [idDescriptor]   INT           NOT NULL ,
    [isActual]       BIT           NOT NULL ,
    -- устаревший или актуальный
    [isSale]         BIT           NOT NULL ,
    -- внешний лии внутренний
    [isPublic]       BIT ,
    -- Является ли артикул публичным (можно ли экспортировать)
    [codeAccountant] NVARCHAR(255) ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [ProductsAnalogs]
(
    [id]         INT IDENTITY (1, 1) ,
    -- 

    [idOriginal] INT ,
    -- id оригинала
    [idAnalog]   INT -- id аналога, 
        PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Descriptors]
(
    [id]           INT            IDENTITY (1, 1) ,
    [code]         NVARCHAR(255) ,
    [title]        NVARCHAR(255) ,
    [titleShort]   NVARCHAR(255) ,
    [titleDisplay] VARCHAR(255) ,
    [description]  NVARCHAR(2048) ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [UnitsProducts]
(
    [id]        INT   IDENTITY (1, 1) ,
    [idProduct] INT   NOT NULL ,
    [idUnit]    INT ,
    [idType]    INT ,
    [value]     FLOAT NOT NULL ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [UnitsTypes]
(
    [id]    INT           IDENTITY (1, 1) ,
    [title] NVARCHAR(255) NOT NULL ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Packages]
(
    [id]           INT IDENTITY (1, 1) ,
    [idDescriptor] INT NOT NULL -- id дескриптора-описания, 
        PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [UnitsPerforations]
(
    [id]            INT   NOT NULL IDENTITY (1, 1) ,
    [idPerforation] INT   NOT NULL ,
    [idUnit]        INT   NOT NULL ,
    [idType]        INT   NOT NULL ,
    [Value]         FLOAT NOT NULL ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [UnitsPakages]
(
    [id]        INT   NOT NULL IDENTITY (1, 1) ,
    [idPackage] INT   NOT NULL ,
    [idUnit]    INT ,
    [idType]    INT ,
    [Value]     FLOAT ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Manufacturers]
(
    [id]           INT         IDENTITY (1, 1) ,
    [idDescriptor] INT         NOT NULL ,
    [TIN]          VARCHAR(20) ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Tables]
(
    [id]           INT IDENTITY (1, 1) ,
    [idDescriptor] INT ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Fields]
(
    [id]           INT           IDENTITY (1, 1) ,
    [idDescriptor] INT ,
    [titleRus]     NVARCHAR(255) ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [DescriptorsResources]
(
    [id]             INT IDENTITY (1, 1) ,
    [idDescriptor]   INT NOT NULL ,
    [idResource]     INT NOT NULL ,
    [idResourceType] INT ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Resources]
(
    [id]  INT            IDENTITY (1, 1) ,
    [URL] NVARCHAR(2048) ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [ResourceTypes]
(
    [id]    INT           IDENTITY (1, 1) ,
    [title] NVARCHAR(255) NOT NULL ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [ViewsTables]
(
    [id]      INT IDENTITY (1, 1) ,
    [idTable] INT ,
    [idView]  INT ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [ViewTypes]
(
    [id]           INT NOT NULL IDENTITY (1, 1) ,
    [idDescriptor] INT NOT NULL ,
    PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Views]
(
    [id]           INT IDENTITY (1, 1) ,
    [idDescriptor] INT ,
    [idType]       INT ,
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

ALTER TABLE [ViewsTables] ADD FOREIGN KEY (idTable) REFERENCES [Tables] ([id]);

ALTER TABLE [ViewsTables] ADD FOREIGN KEY (idView) REFERENCES [Views] ([id]);

ALTER TABLE [ViewTypes] ADD FOREIGN KEY (idDescriptor) REFERENCES [Descriptors] ([id]);

ALTER TABLE [Views] ADD FOREIGN KEY (idDescriptor) REFERENCES [Descriptors] ([id]);

ALTER TABLE [Views] ADD FOREIGN KEY (idType) REFERENCES [ViewTypes] ([id]);

--#endregion

--#region Заполнение таблиц

-- Вставка данных в таблицу Descriptors

--#endregion

--#region Представления

--#region Для редактирования

--#region Представление AllProductsInfoView

--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[AllProductsInfoView]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[AllProductsInfoView]
AS
    (
    SELECT
        Products.id,
        Products.idDescriptor,
        Products.idNorm,
        Products.idSubGroup,
        Products.idCover,
        Products.idMaterial,
        Products.isInStock,
        Products.idPerforation,
        Products.idPackage,
        Descriptors.code,
        Descriptors.title,
        Descriptors.titleShort,
        Descriptors.description,
        Norms.idDescriptor AS idDescriptorNorm,
        SubGroups.idGroup,
        SubGroups.idDescriptor AS idDescriptorSubGroups,
        SubGroups.idLoadDiagram,
        Covers.thickness,
        Covers.idNorm AS idNormCovers,
        Materials.type AS typeMaterials,
        Materials.idMaterialNorm,
        Materials.idPrepackNorm,
        Perforations.idDescriptor AS idDescriptorPerforations,
        Packages.idDescriptor AS idDescriptorPackages,
        Materials.idDescriptor AS idDescriptorMaterials,
        Covers.idDescriptor AS idDescriptorCovers
    FROM
        Products INNER JOIN
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

--#region Представление AllProductUnitsInfoView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[AllProductUnitsInfoView]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[AllProductUnitsInfoView]
AS
    (
    SELECT
        UnitsProducts.id,
        UnitsProducts.idProduct,
        UnitsProducts.idUnit,
        UnitsProducts.idType,
        UnitsProducts.value,
        UnitsTypes.title AS titleUnitType,
        Units.OKEI,
        Descriptors.title AS titleUnit,
        Descriptors.description AS descriptionUnit,
        Descriptors.titleShort AS titleShortUnit
    FROM
        UnitsProducts INNER JOIN
        Units ON UnitsProducts.idUnit = Units.id INNER JOIN
        UnitsTypes ON UnitsProducts.idType = UnitsTypes.id INNER JOIN
        Descriptors ON Units.idDescriptor = Descriptors.id
);
--#endregion
--#endregion

--#region Представление ViewTypesView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.ViewTypesView; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.ViewTypesView
AS
    SELECT
        VT.id,
        D.title AS DescriptorTitle,
        D.titleShort AS DescriptorTitleShort,
        D.titleDisplay AS DescriptorTitleDisplay,
        D.description AS DescriptorDescription
    FROM
        ViewTypes VT
        INNER JOIN Descriptors D ON VT.idDescriptor = D.id;
--#endregion
--#endregion

--#region Представление TablesView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[TablesView]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[TablesView]
AS
    (
    SELECT
        T.id AS TableId,
        D.title AS TableTitle,
        D.titleShort AS TableTitleShort,
        D.titleDisplay AS TableTitleDisplay,
        D.description AS TableDescription
    FROM
        Tables T
        INNER JOIN Descriptors D ON T.idDescriptor = D.id
    );
--#endregion
--#endregion

--#region Представление ViewsView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[ViewsView]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[ViewsView]
AS
    (
    SELECT
        V.id AS ViewId,
        V.idType AS ViewIdType,
        VT.DescriptorTitle AS ViewTypeTitle,
        VT.DescriptorTitleShort AS ViewTypeTitleShort,
        D.title AS ViewTitle,
        D.titleShort AS ViewTitleShort,
        D.titleDisplay AS ViewTitleDisplay,
        D.description AS ViewDescription
    FROM
        dbo.[Views] V
        INNER JOIN dbo.[Descriptors] D ON V.idDescriptor = D.id
        INNER JOIN dbo.[ViewTypesView] VT ON V.idType = VT.id
    );
--#endregion
--#endregion

--#endregion

--#region Для отображения

--#region Представление DescriptorsViewDisplay
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[DescriptorsViewDisplay]; 
--#endregion
--#region Создание
GO
CREATE VIEW [DescriptorsViewDisplay]
AS
    SELECT
        ID AS 'ID дексриптора',
        Title AS 'Наименование',
        titleShort AS 'Сокращенное наименование',
        Code AS 'Код',
        Description AS 'Описание'
    FROM
        Descriptors;
--#endregion
--#endregion

--#region Представление NormsView 
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[NormsView ]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[NormsView]
AS
    (
    SELECT
        N.ID,
        D.[Код] AS [Номер документа],
        D.[Наименование] AS [Наименование документа]
    FROM
        Norms N
        JOIN [DescriptorsViewDisplay] D ON N.ID = D.[ID дексриптора]
    );
--#endregion
--#endregion

--#region Представление UnitsView 
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[UnitsView ]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[UnitsView ]
AS
    (
    SELECT
        U.ID,
        D.[Наименование] AS [Наименование],
        D.[Сокращенное наименование] AS [Сокращенное наименование],
        D.[Описание] AS [Описание],
        U.OKEI AS [Код ОКЕИ]
    FROM
        Units U
        JOIN [DescriptorsViewDisplay] D ON U.ID = D.[ID дексриптора]
    );
--#endregion
--#endregion

--#region Представление UnitsTypesView 
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[UnitsTypesView ]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[UnitsTypesView ]
AS
    (
    SELECT
        [ID] AS [ID],
        [title] AS [Название измерения]
    FROM
        dbo.[UnitsTypes]
);
--#endregion
--#endregion

--#region Представление MaterialsView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[MaterialsView]; 
--#endregion
--#region Создание
GO
CREATE VIEW [MaterialsView]
AS
    SELECT
        M.ID,
        D.[Наименование] AS [Полное наименование],
        D.[Сокращенное наименование] AS [Сокращенное наименование],
        D.[Код] AS [Обозначение материала],
        N.[Номер документа] AS [Стандарт материала],
        NR.[Номер документа] AS [Стандарт сырья]
    FROM
        Materials M
        JOIN [DescriptorsViewDisplay] D ON M.ID = D.[ID дексриптора]
        LEFT JOIN [NormsView] N ON M.idMaterialNorm = N.ID
        LEFT JOIN [NormsView] NR ON M.idPrepackNorm = NR.ID;
--#endregion
--#endregion

--#region Представление ManufacturersView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[ManufacturersView]; 
--#endregion
--#region Создание
GO
CREATE VIEW [ManufacturersView]
AS
    SELECT
        M.ID,
        D.[Наименование] AS [Полное наименование],
        D.[Сокращенное наименование] AS [Сокращенное наименование],
        M.TIN AS [ИНН]
    FROM
        Manufacturers M
        JOIN [DescriptorsViewDisplay] D ON M.ID = D.[ID дексриптора];
--#endregion
--#endregion

--#region Представление ResourceTypesView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[ResourceTypesView]; 
--#endregion
--#region Создание
GO
CREATE VIEW [ResourceTypesView]
AS
    SELECT
        [ID],
        [title] AS [Полное наименование]
    FROM
        [ResourceTypes];
--#endregion
--#endregion

--#region Представление CoversView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[CoversView ]; 
--#endregion
--#region Создание
GO
CREATE VIEW [CoversView]
AS
    SELECT
        C.ID,
        D.[Наименование] AS [Полное наименование],
        D.[Сокращенное наименование] AS [Сокращенное наименование],
        D.[Код] AS [Обозначение покрытия],
        C.[thickness] AS [Толщина покрытия, мкм],
        NV.[Наименование документа] AS [Наименование ГОСТ],
        NV.[Номер документа] AS [номер ГОСТ]
    FROM
        Covers C
        JOIN [DescriptorsViewDisplay] D ON C.ID = D.[ID дексриптора]
        JOIN [NormsView] NV ON C.idNorm = NV.ID;
--#endregion
--#endregion

--#region Представление PerforationsView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[PerforationsView]; 
--#endregion
--#region Создание
GO
CREATE VIEW [PerforationsView]
AS
    SELECT
        P.ID,
        D.[Наименование] AS [Полное обозначение],
        D.[Сокращенное наименование] AS [Сокращенное обозначение],
        D.[Описание] AS [Описание]
    FROM
        Perforations P
        JOIN [DescriptorsViewDisplay] D ON P.ID = D.[ID дексриптора];
--#endregion
--#endregion

--#region Представление PackagesView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[PackagesView]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[PackagesView]
AS
    SELECT
        P.ID,
        D.[Наименование] AS [Полное наименование],
        D.[Сокращенное наименование] AS [Краткое обозначение],
        D.[Описание] AS [Описание]
    FROM
        Packages P
        JOIN [DescriptorsViewDisplay] D ON P.ID = D.[ID дексриптора];
--#endregion
--#endregion

--#region Представление LoadDiagramsView 
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[LoadDiagramsView ]; 
--#endregion
--#region Создание
GO
CREATE VIEW [LoadDiagramsView]
AS
    SELECT
        LD.ID,
        D.[Наименование] AS [Полное наименование],
        D.[Сокращенное наименование] AS [Краткое обозначение],
        D.[Описание] AS [Описание]
    FROM
        LoadDiagrams LD
        JOIN [DescriptorsViewDisplay] D ON LD.ID = D.[ID дексриптора];
--#endregion
--#endregion

--#region Представление TablesViewsView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[TablesViewsView]; 
--#endregion
--#region Создание
GO
CREATE VIEW TablesViewsView
AS
    SELECT
        T.[id] AS [ID таблицы],
        T.[idDescriptor] AS [ID дескриптора таблицы],
        V.[id] AS [ID представления],
        V.[idDescriptor] AS [ID дескриптора представления],
        VT.[id] AS [ID типа представления],
        VT.[idDescriptor] AS [ID дескриптора типа представления]
    FROM
        Tables T
        JOIN ViewsTables VTbl ON T.[id] = VTbl.[idTable]
        JOIN Views V ON VTbl.[idView] = V.[id]
        JOIN ViewTypes VT ON V.[idType] = VT.[id];

--#endregion
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
    DECLARE @imagePath NVARCHAR(2048)
    DECLARE @vendorCode NVARCHAR(255)
    DECLARE @productId INT
    DECLARE @descriptorId INT
    DECLARE @resourceTypeId INT
    DECLARE @resourceId INT

    SELECT
        TOP 1
        @productId = id,
        @descriptorId = idDescriptor
    FROM
        INSERTED

    -- Get vendor code for the product
    SELECT
        TOP 1
        @vendorCode = dvc.title
    FROM
        VendorCodes vc
        INNER JOIN ProductsVendorCodes pvc ON pvc.idCode = vc.id
        INNER JOIN Descriptors AS dvc ON vc.idDescriptor = dvc.id
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
    SELECT
        TOP 1
        @resourceTypeId = id
    FROM
        ResourceTypes
    WHERE title = 'image'

    -- Get Resource id for the added image
    SELECT
        TOP 1
        @resourceId = id
    FROM
        Resources
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
    DECLARE @imagePath NVARCHAR(2048)
    DECLARE @vendorCode NVARCHAR(255)

    -- Create ResourceType 'image' if it does not exist
    IF NOT EXISTS (SELECT
        *
    FROM
        ResourceTypes
    WHERE title = 'image')
    BEGIN
        INSERT INTO ResourceTypes
            (title)
        VALUES
            ('image')
    END

    -- Loop through each product and add image path to Resources table
    DECLARE @productId INT
    DECLARE product_cursor CURSOR FOR 
        SELECT
        id
    FROM
        Products
    OPEN product_cursor
    FETCH NEXT FROM product_cursor INTO @productId
    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Get vendor code for the product
        SELECT
            TOP 1
            @vendorCode = dvc.title
        FROM
            VendorCodes vc
            INNER JOIN ProductsVendorCodes pvc ON pvc.idCode = vc.id
            INNER JOIN Descriptors AS dvc ON vc.idDescriptor = dvc.id
        WHERE pvc.idProduct = @productId

        -- Create image path using vendor code and add to Resources table
        SET @imagePath = 'Resources/images/' + @vendorCode + '.png'
        INSERT INTO Resources
            (URL)
        VALUES
            (@imagePath)

        -- Get ResourceTypes id for 'image'
        DECLARE @resourceTypeId INT
        SELECT
            TOP 1
            @resourceTypeId = id
        FROM
            ResourceTypes
        WHERE title = 'image'

        -- Get Resource id for the added image
        DECLARE @resourceId INT
        SELECT
            TOP 1
            @resourceId = id
        FROM
            Resources
        WHERE URL = @imagePath

        -- Add entry to DescriptorsResources table
        DECLARE @descriptorId INT
        SELECT
            TOP 1
            @descriptorId = idDescriptor
        FROM
            Products
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
    DECLARE @imagePath NVARCHAR(2048)
    DECLARE @vendorCode NVARCHAR(255)

    -- Create ResourceType 'image' if it does not exist
    IF NOT EXISTS (SELECT
        *
    FROM
        ResourceTypes
    WHERE title = 'image')
    BEGIN
        INSERT INTO ResourceTypes
            (title)
        VALUES
            ('image')
    END

    -- Loop through each sub group and add image path to Resources table
    DECLARE @subGroupId INT
    DECLARE sub_group_cursor CURSOR FOR 
        SELECT
        id
    FROM
        SubGroups
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
        SELECT
            TOP 1
            @vendorCode = sgd.code
        FROM
            SubGroups AS sg
            INNER JOIN Descriptors AS sgd ON sgd.id = sg.idDescriptor
        WHERE sg.id = @subGroupId

        -- Create image path using vendor code and add to Resources table
        SET @imagePath = 'Resources/images/' + @vendorCode + '.png'
        INSERT INTO Resources
            (URL)
        VALUES
            (@imagePath)

        -- Get ResourceTypes id for 'image'
        DECLARE @resourceTypeId INT
        SELECT
            TOP 1
            @resourceTypeId = id
        FROM
            ResourceTypes
        WHERE title = 'image'

        -- Get Resource id for the added image
        DECLARE @resourceId INT
        SELECT
            TOP 1
            @resourceId = id
        FROM
            Resources
        WHERE URL = @imagePath

        -- Add entry to DescriptorsResources table
        DECLARE @descriptorId INT
        SELECT
            TOP 1
            @descriptorId = idDescriptor
        FROM
            SubGroups
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
 