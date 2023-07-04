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
    (
    SELECT
        ID AS 'ID дексриптора',
        Title AS 'Наименование',
        titleShort AS 'Сокращенное наименование',
        Code AS 'Код',
        Description AS 'Описание'
    FROM
        Descriptors
);
--#endregion
--#endregion

--#region Представление NormsView 
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[NormsView]; 
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
DROP VIEW IF EXISTS dbo.[UnitsView]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[UnitsView]
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
DROP VIEW IF EXISTS dbo.[UnitsTypesView]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[UnitsTypesView]
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
    (
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
        LEFT JOIN [NormsView] NR ON M.idPrepackNorm = NR.ID
);
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
    (
    SELECT
        M.ID,
        D.[Наименование] AS [Полное наименование],
        D.[Сокращенное наименование] AS [Сокращенное наименование],
        M.TIN AS [ИНН]
    FROM
        Manufacturers M
        JOIN [DescriptorsViewDisplay] D ON M.ID = D.[ID дексриптора]
);
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
    (
    SELECT
        [ID],
        [title] AS [Полное наименование]
    FROM
        [ResourceTypes]
);
--#endregion
--#endregion

--#region Представление CoversView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[CoversView]; 
--#endregion
--#region Создание
GO
CREATE VIEW [CoversView]
AS
    (
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
        JOIN [NormsView] NV ON C.idNorm = NV.ID
);
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
    (
    SELECT
        P.ID,
        D.[Наименование] AS [Полное обозначение],
        D.[Сокращенное наименование] AS [Сокращенное обозначение],
        D.[Описание] AS [Описание]
    FROM
        Perforations P
        JOIN [DescriptorsViewDisplay] D ON P.ID = D.[ID дексриптора]
);
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
    (
    SELECT
        P.ID,
        D.[Наименование] AS [Полное наименование],
        D.[Сокращенное наименование] AS [Краткое обозначение],
        D.[Описание] AS [Описание]
    FROM
        Packages P
        JOIN [DescriptorsViewDisplay] D ON P.ID = D.[ID дексриптора]
);
--#endregion
--#endregion

--#region Представление LoadDiagramsView 
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[LoadDiagramsView]; 
--#endregion
--#region Создание
GO
CREATE VIEW [LoadDiagramsView]
AS
    (
    SELECT
        LD.ID,
        D.[Наименование] AS [Полное наименование],
        D.[Сокращенное наименование] AS [Краткое обозначение],
        D.[Описание] AS [Описание]
    FROM
        LoadDiagrams LD
        JOIN [DescriptorsViewDisplay] D ON LD.ID = D.[ID дексриптора]
);
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
    (
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
        JOIN ViewTypes VT ON V.[idType] = VT.[id]
);
--#endregion
--#endregion

--#region Представление ClassesView 
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[ClassesView ]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[ClassesView ]
AS
    (
    SELECT
        C.ID,
        D.[Наименование] AS [Наименование],
        D.[Сокращенное наименование] AS [Сокращенное наименование],
        D.[Описание] AS [Описание]
    FROM
        Classes C
        JOIN [DescriptorsViewDisplay] D ON C.ID = D.[ID дексриптора]
);
--#endregion
--#endregion

--#region Представление GroupsView 
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[GroupsView ]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[GroupsView ]
AS
    (
    SELECT
        G.ID,
        D.[Наименование] AS [Наименование],
        D.[Сокращенное наименование] AS [Сокращенное наименование],
        D.[Описание] AS [Описание],
        C.[ID] AS [ID класса],
        C.[Наименование] AS [Наименование класса]
    FROM
        Groups G
        JOIN [DescriptorsViewDisplay] D ON G.ID = D.[ID дексриптора]
        LEFT JOIN ClassesView C ON G.idClass = C.ID
);
--#endregion
--#endregion

--#region Представление SubGroupsView 
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[SubGroupsView ]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[SubGroupsView ]
AS
    (
    SELECT
        SG.ID,
        D.[Наименование] AS [Наименование],
        D.[Сокращенное наименование] AS [Сокращенное наименование],
        D.[Описание] AS [Описание],
        G.[ID] AS [ID группы],
        G.[Наименование] AS [Наименование группы]
    FROM
        SubGroups SG
        JOIN [DescriptorsViewDisplay] D ON SG.ID = D.[ID дексриптора]
        LEFT JOIN GroupsView G ON SG.idGroup = G.ID
);
--#endregion
--#endregion

--#endregion
