
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
        titleShort AS 'Сокращенное наименование',
        titleDisplay AS 'Отображаемое название',
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
        DV.[Наименование] AS 'Наименование',
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
        DV.[Наименование] AS 'Наименование',
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
        DV.[Наименование] AS 'Наименование',
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
        DV.[Наименование] AS 'Наименование'
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
        DV.[Наименование] AS 'Наименование',
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
        DV.[Наименование] AS 'Наименование'
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
        DV.[Код] AS 'Номер документа',
        DV.[Наименование] AS 'Наименование документа'
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
        -- M.[idMaterialNorm] AS 'ID Нормативного документа материалов',
        -- M.[idPrepackNorm] AS 'ID Норматива документа сырья',
        DV.[Наименование] AS 'Наименование',
        DV.[Сокращенное наименование] AS 'Сокращенное наименование',
        DV.[Код] AS 'Обозначение материала',
        M.[type] AS 'Обозначение материала/марка стали',
        NVM.[Наименование документа] AS 'Стандарт материала',
        NVM.[Номер документа] AS 'Код стандарта материала',
        NVP.[Наименование документа] AS 'Стандарт сырья',
        NVP.[Номер документа] AS 'Код стандарта сырья'
    FROM
        Materials M
        JOIN DescriptorsView DV ON M.[idDescriptor] = DV.[ID дескриптора]
        JOIN NormsView NVM ON M.idMaterialNorm = NVM.ID
        JOIN NormsView NVP ON M.idPrepackNorm = NVP.ID
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
        DV.[Наименование] AS 'Наименование',
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
        DV.[Наименование] AS 'Наименование',
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
        DV.[Наименование] AS 'Наименование',
        DV.[Сокращенное наименование] AS 'Сокращенное наименование',
        DV.[Описание] AS 'Описание',
        U.[OKEI] AS 'Общероссийский классификатор единиц измерения'
    FROM
        Units U
        JOIN DescriptorsView DV ON U.[idDescriptor] = DV.[ID дескриптора]
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
        UT.id AS 'ID',
        UT.[title] AS 'Наименование измерения'
    FROM
        UnitsTypes UT
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
        DV.[Наименование] AS 'Наименование',
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

--#region Представление ResourcesView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[ResourcesView]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[ResourcesView]
AS
    (
    SELECT
        R.id AS 'ID',
        R.[URL] AS 'URL ресурса'
    FROM
        Resources R
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
        DV.[Наименование] AS 'Наименование',
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
        [id] AS 'ID',
        DV.*
    FROM
        [Tables] AS T
        JOIN DescriptorsView DV
        ON T.[idDescriptor] = DV.[ID дескриптора]
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
        DV.[Наименование]
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
        DV.[Наименование] AS 'Наименование представления',
        DV.[Сокращенное наименование] AS 'Сокращенное наименование представления',
        DV.[Отображаемое название] AS 'Отображаемое название представления',
        DV.[ID дескриптора] AS 'ID дескриптора представления',
        VT.[ID] AS 'ID Типа',
        VT.[Наименование] AS 'Тип'
    FROM
        Views V
        JOIN DescriptorsView DV ON V.[idDescriptor] = DV.[ID дескриптора]
        JOIN ViewTypesView VT ON V.[idType] = VT.[ID]
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
        V.id AS 'ID',
        DV.[ID дескриптора] AS 'ID дескриптора представления',
        DV.[Наименование] AS 'Наименование представления',
        DV.[Сокращенное наименование] AS 'Сокращенное наименование представления',
        DV.[Отображаемое название] AS 'Отображаемое название представления',
        VT.[Наименование] AS 'Тип',
        TV.[ID дескриптора] AS 'ID дескриптора таблицы',
        TV.[Наименование] AS 'Наименование таблицы',
        TV.[Сокращенное наименование] AS 'Сокращенное наименование таблицы',
        TV.[Отображаемое название] AS 'Отображаемое название таблицы'
    FROM
        Views V
        JOIN DescriptorsView DV ON V.[idDescriptor] = DV.[ID дескриптора]
        JOIN ViewTypesView VT ON V.[idType] = VT.[ID]
        JOIN ViewsTables ON V.id = ViewsTables.idView
        JOIN TablesView TV ON TV.[id] = ViewsTables.idTable  
);
--#endregion
--#endregion
