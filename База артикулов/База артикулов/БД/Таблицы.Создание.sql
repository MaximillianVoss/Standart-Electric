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

CREATE TABLE [UnitsPackages]
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
    [id]         INT           IDENTITY (1, 1) ,
    [title]      NVARCHAR(255) NOT NULL ,
    [extension ] VARCHAR(255) ,
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

ALTER TABLE [UnitsPackages] ADD FOREIGN KEY (idPackage) REFERENCES [Packages] ([id]);

ALTER TABLE [UnitsPackages] ADD FOREIGN KEY (idUnit) REFERENCES [Units] ([id]);

ALTER TABLE [UnitsPackages] ADD FOREIGN KEY (idType) REFERENCES [UnitsTypes] ([id]);

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