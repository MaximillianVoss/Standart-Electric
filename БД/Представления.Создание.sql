
--#region Представление DescriptorsView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[DescriptorsView]; 
--#endregion
--#region Создание
GO
CREATE VIEW [DescriptorsView]
AS
    (
    SELECT
        ID AS 'ID дескриптора',
        Title AS 'Наименование',
        titleShort AS 'Сокращенное Наименование',
        Code AS 'Код',
        Description AS 'Описание'
    FROM
        Descriptors
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
CREATE VIEW dbo.[LoadDiagramsView]
AS
    (
    SELECT
        LD.id AS 'ID',
        DV.*
    FROM
        LoadDiagrams LD
        JOIN DescriptorsView DV ON LD.[idDescriptor] = DV.[ID дескриптора]
);
--#endregion
--#endregion


--#region Представление SubGroupsView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[SubGroupsView]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[SubGroupsView]
AS
    (
    SELECT
        SG.id AS 'ID',
        DV.[Наименование] AS 'Дескриптор',
        SG.[idGroup] AS 'ID Группы',
        LD.[Наименование] AS 'Схема нагрузки'
    FROM
        SubGroups SG
        JOIN DescriptorsView DV ON SG.[idDescriptor] = DV.[ID дескриптора]
        LEFT JOIN LoadDiagramsView LD ON SG.[idLoadDiagram] = LD.[ID]
);
--#endregion
--#endregion

--#region Представление GroupsView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[GroupsView]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[GroupsView]
AS
    (
    SELECT
        G.id AS 'ID',
        DV.[Наименование] AS 'Дескриптор',
        G.[idClass] AS 'ID Класса'
    FROM
        Groups G
        JOIN DescriptorsView DV ON G.[idDescriptor] = DV.[ID дескриптора]
);
--#endregion
--#endregion

--#region Представление BuisnessUnitsView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[BuisnessUnitsView]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[BuisnessUnitsView]
AS
    (
    SELECT
        BU.id AS 'ID',
        DV.[Наименование] AS 'Дескриптор',
        BU.[idBimLibraryFile] AS 'ID Файла BIM-библиотеки',
        BU.[idDrawBlocksFile] AS 'ID Файла блоков чертежей',
        BU.[idTypicalAlbum] AS 'ID Типового альбома'
    FROM
        BuisnessUnits BU
        JOIN DescriptorsView DV ON BU.[idDescriptor] = DV.[ID дескриптора]
);
--#endregion
--#endregion

--#region Представление ClassesView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[ClassesView]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[ClassesView]
AS
    (
    SELECT
        C.id AS 'ID',
        DV.[Наименование] AS 'Дескриптор'
    FROM
        Classes C
        JOIN DescriptorsView DV ON C.[idDescriptor] = DV.[ID дескриптора]
);
--#endregion
--#endregion

--#region Представление ApplicationsView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[ApplicationsView]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[ApplicationsView]
AS
    (
    SELECT
        A.id AS 'ID',
        DV.[Наименование] AS 'Дескриптор',
        A.[idBuisnessUnit] AS 'ID Направления'
    FROM
        Applications A
        JOIN DescriptorsView DV ON A.[idDescriptor] = DV.[ID дескриптора]
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
CREATE VIEW dbo.[PerforationsView]
AS
    (
    SELECT
        PER.id AS 'ID',
        DV.[Наименование] AS 'Дескриптор'
    FROM
        Perforations PER
        JOIN DescriptorsView DV ON PER.[idDescriptor] = DV.[ID дескриптора]
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
        N.id AS 'ID',
        DV.[Наименование] AS 'Дескриптор'
    FROM
        Norms N
        JOIN DescriptorsView DV ON N.[idDescriptor] = DV.[ID дескриптора]
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
CREATE VIEW dbo.[MaterialsView]
AS
    (
    SELECT
        M.id AS 'ID',
        DV.[Наименование] AS 'Дескриптор',
        M.[idMaterialNorm] AS 'ID Нормативного документа',
        M.[idPrepackNorm] AS 'ID Норматива',
        M.[type] AS 'Обозначение материала или марка стали'
    FROM
        Materials M
        JOIN DescriptorsView DV ON M.[idDescriptor] = DV.[ID дескриптора]
);
--#endregion
--#endregion

--#region Представление GroupsApplicationsView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[GroupsApplicationsView]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[GroupsApplicationsView]
AS
    (
    SELECT
        GA.id AS 'ID',
        A.*,
        SG.*
    FROM
        GroupsApplications GA
        JOIN ApplicationsView A ON GA.[idApplication] = A.[ID]
        JOIN SubGroupsView SG ON GA.[idSubGroup] = SG.[ID]
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
CREATE VIEW dbo.[CoversView]
AS
    (
    SELECT
        CV.id AS 'ID',
        DV.[Наименование] AS 'Дескриптор',
        CV.[thickness] AS 'Толщина покрытия'
    FROM
        Covers CV
        JOIN DescriptorsView DV ON CV.[idDescriptor] = DV.[ID дескриптора]
);
--#endregion
--#endregion

--#region Представление ProductsView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[ProductsView]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[ProductsView]
AS
    (
    SELECT
        P.id AS 'ID',
        DV.[Наименование] AS 'Дескриптор',
        N.[Наименование] AS 'Стандарт',
        SG.[Наименование] AS 'Подгруппа',
        CV.[Наименование] AS 'Покрытие',
        M.[Наименование] AS 'Материал',
        PER.[Наименование] AS 'Перфорация',
        PK.[Наименование] AS 'Упаковка',
        P.[isInStock] AS 'Складская позиция'
    FROM
        Products P
        JOIN DescriptorsView DV ON P.[idDescriptor] = DV.[ID дескриптора]
        LEFT JOIN NormsView N ON P.[idNorm] = N.[ID]
        LEFT JOIN SubGroupsView SG ON P.[idSubGroup] = SG.[ID]
        LEFT JOIN CoversView CV ON P.[idCover] = CV.[ID]
        LEFT JOIN MaterialsView M ON P.[idMaterial] = M.[ID]
        LEFT JOIN PerforationsView PER ON P.[idPerforation] = PER.[ID]
        LEFT JOIN PackagesView PK ON P.[idPackage] = PK.[ID]
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
        U.id AS 'ID',
        DV.[Наименование] AS 'Дескриптор',
        U.[OKEI] AS 'Общероссийский классификатор единиц измерения'
    FROM
        Units U
        JOIN DescriptorsView DV ON U.[idDescriptor] = DV.[ID дескриптора]
);
--#endregion
--#endregion

--#region Представление ProductsVendorCodesView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[ProductsVendorCodesView]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[ProductsVendorCodesView]
AS
    (
    SELECT
        PVC.id AS 'ID',
        P.[Наименование] AS 'Продукт',
        VC.[Артикул'] AS 'Артикул'
    FROM
        ProductsVendorCodes PVC
        JOIN ProductsView P ON PVC.[idProduct] = P.[ID]
        JOIN VendorCodesView VC ON PVC.[idCode] = VC.[ID]
);
--#endregion
--#endregion

--#region Представление VendorCodesView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[VendorCodesView]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[VendorCodesView]
AS
    (
    SELECT
        VC.id AS 'ID',
        M.[Наименование] AS 'Производитель',
        DV.[Наименование] AS 'Дескриптор',
        VC.[isActual] AS 'Актуальность',
        VC.[isSale] AS 'Тип',
        VC.[isPublic] AS 'Публичность',
        VC.[codeAccountant] AS 'Артикул бухгалтерии'
    FROM
        VendorCodes VC
        JOIN ManufacturersView M ON VC.[idManufacturer] = M.[ID]
        JOIN DescriptorsView DV ON VC.[idDescriptor] = DV.[ID дескриптора]
);
--#endregion
--#endregion

--#region Представление ProductsAnalogsView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[ProductsAnalogsView]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[ProductsAnalogsView]
AS
    (
    SELECT
        PA.id AS 'ID',
        P1.[Наименование] AS 'Оригинал',
        P2.[Наименование] AS 'Аналог'
    FROM
        ProductsAnalogs PA
        JOIN ProductsView P1 ON PA.[idOriginal] = P1.[ID]
        JOIN ProductsView P2 ON PA.[idAnalog] = P2.[ID]
);
--#endregion
--#endregion

--#region Представление DescriptorsResourcesView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[DescriptorsResourcesView]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[DescriptorsResourcesView]
AS
    (
    SELECT
        DR.id AS 'ID',
        DV.[Наименование] AS 'Дескриптор',
        R.[URL] AS 'URL ресурса',
        RT.[Наименование] AS 'Тип ресурса'
    FROM
        DescriptorsResources DR
        JOIN DescriptorsView DV ON DR.[idDescriptor] = DV.[ID дескриптора]
        JOIN ResourcesView R ON DR.[idResource] = R.[ID]
        JOIN ResourceTypesView RT ON DR.[idResourceType] = RT.[ID]
);
--#endregion
--#endregion

--#region Представление ViewsTablesView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[ViewsTablesView]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[ViewsTablesView]
AS
    (
    SELECT
        VT.id AS 'ID',
        T.[Наименование] AS 'Таблица',
        V.[Наименование] AS 'Представление'
    FROM
        ViewsTables VT
        JOIN TablesView T ON VT.[idTable] = T.[ID]
        JOIN ViewsView V ON VT.[idView] = V.[ID]
);
--#endregion
--#endregion

--#region Представление ViewTypesView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[ViewTypesView]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[ViewTypesView]
AS
    (
    SELECT
        VT.id AS 'ID',
        DV.[Наименование] AS 'Дескриптор'
    FROM
        ViewTypes VT
        JOIN DescriptorsView DV ON VT.[idDescriptor] = DV.[ID дескриптора]
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
        V.id AS 'ID',
        DV.[Наименование] AS 'Дескриптор',
        VT.[Наименование] AS 'Тип'
    FROM
        Views V
        JOIN DescriptorsView DV ON V.[idDescriptor] = DV.[ID дескриптора]
        JOIN ViewTypesView VT ON V.[idType] = VT.[ID]
);
--#endregion
--#endregion
