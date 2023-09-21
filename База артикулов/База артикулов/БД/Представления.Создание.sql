
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

--#region Представления и их типы

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

--#endregion

--#region Представление DescriptorsResourcesView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[DescriptorsResourcesView]; 
--#endregion
--#region Создание
GO
CREATE VIEW [DescriptorsResourcesView]
AS
    (
    SELECT
        D.ID AS 'ID дескриптора ресурса',
        D.Title AS 'Наименование объекта',
        DR.title AS 'Наименование ресурса',
        titleShort AS 'Сокращенное наименование ресурса',
        titleDisplay AS 'Отображаемое название ресурса',
        Code AS 'Код ресурса',
        Description AS 'Описание ресурса',
        R.URL AS 'URL ресурса',
        RT.title AS 'Тип ресурса'
    FROM
        Descriptors D
        LEFT JOIN DescriptorsResources DR ON DR.idDescriptor = D.id
        LEFT JOIN Resources R ON DR.idResource = R.id
        LEFT JOIN ResourceTypes RT ON DR.idResourceType = RT.id
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
        LD.id AS 'ID схемы нагрузок',
        DV.[ID дескриптора] AS 'ID дескриптора схемы нагрузок',
        DV.[Наименование] AS 'Наименование схемы нагрузок',
        DV.[Сокращенное наименование] AS 'Сокращенное наименование схемы нагрузок',
        DV.[Отображаемое название] AS 'Отображаемое название схемы нагрузок',
        DV.[Код] AS 'Код схемы нагрузок',
        DV.[Описание] AS 'Описание схемы нагрузок'
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
        SG.id AS 'ID подгруппы',
        DV.[Наименование объекта] AS 'Наименование подгруппы',
        DV.[Сокращенное наименование ресурса] AS 'Сокращенное наименование подгруппы',
        DV.[Описание ресурса] AS 'Описание подгруппы',
        DV.[Код ресурса] AS 'Код подгруппы',
        DV.[URL ресурса] AS 'URL изображения подгруппы',
        SG.[idGroup] AS 'ID группы',
        LD.*
    FROM
        SubGroups SG
        LEFT JOIN DescriptorsResourcesView DV ON SG.[idDescriptor] = DV.[ID дескриптора ресурса]
        LEFT JOIN LoadDiagramsView LD ON SG.[idLoadDiagram] = LD.[ID схемы нагрузок]
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
        G.id AS 'ID группы',
        DV.[Наименование объекта] AS 'Наименование группы',
        DV.[Сокращенное наименование ресурса] AS 'Сокращенное наименование группы',
        DV.[Описание ресурса] AS 'Описание группы',
        DV.[Код ресурса] AS 'Код группы',
        DV.[URL ресурса] AS 'URL изображения группы',
        G.[idClass] AS 'ID класса'
    FROM
        Groups G
        LEFT JOIN DescriptorsResourcesView DV ON G.[idDescriptor] = DV.[ID дескриптора ресурса]
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
        C.id AS 'ID класса',
        DV.[Наименование объекта] AS 'Наименование класса',
        DV.[Сокращенное наименование ресурса] AS 'Сокращенное наименование класса',
        DV.[Описание ресурса] AS 'Описание класса',
        DV.[Код ресурса] AS 'Код класса',
        DV.[URL ресурса] AS 'URL изображения класса'
    FROM
        Classes C
        JOIN DescriptorsResourcesView DV ON C.[idDescriptor] = DV.[ID дескриптора ресурса]
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
        BU.id AS 'ID направления',
        DV.[Наименование] AS 'Наименование направления',
        BU.idDescriptor AS 'ID схемы нагрузки',
        DRV1.[Наименование объекта] AS 'Наименование схема нагрузки',
        DRV1.[URL ресурса] AS 'URL схемы нагрузки',
        BU.[idBimLibraryFile] AS 'ID Файла BIM-библиотеки',
        DRV2.[Наименование объекта]AS 'Наименование BIM-библиотеки',
        DRV2.[URL ресурса]AS 'URL BIM-библиотеки',
        BU.[idDrawBlocksFile] AS 'ID Файла блоков чертежей',
        DRV3.[Наименование объекта]AS 'Наименование файла блоков чертежей',
        DRV3.[URL ресурса]AS 'URL блоков чертежей',
        BU.[idTypicalAlbum] AS 'ID Альбома типовых узлов',
        DRV4.[Наименование объекта]AS 'Наименование альбома типовых узлов',
        DRV4.[URL ресурса]AS 'URL альбома типовых узлов'
    FROM
        BuisnessUnits BU
        JOIN DescriptorsView DV ON BU.[idDescriptor] = DV.[ID дескриптора]
        LEFT JOIN DescriptorsResourcesView DRV1 ON BU.[idDescriptor] = DRV1.[ID дескриптора ресурса]
        LEFT JOIN DescriptorsResourcesView DRV2 ON BU.[idBimLibraryFile] = DRV2.[ID дескриптора ресурса]
        LEFT JOIN DescriptorsResourcesView DRV3 ON BU.[idDrawBlocksFile] = DRV3.[ID дескриптора ресурса]
        LEFT JOIN DescriptorsResourcesView DRV4 ON BU.[idTypicalAlbum] = DRV4.[ID дескриптора ресурса]

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
        A.id AS 'ID применения',
        DV.[Наименование] AS 'Наименование применения',
        BUV.*
    FROM
        Applications A
        JOIN DescriptorsView DV ON A.[idDescriptor] = DV.[ID дескриптора]
        LEFT JOIN BuisnessUnitsView BUV ON A.idBuisnessUnit = BUV.[ID направления]
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
        U.id AS 'ID единицы измерения',
        DV.[Наименование] AS 'Наименование единицы измерения',
        DV.[Сокращенное наименование] AS 'Сокращенное наименование единицы измерения',
        DV.[Описание] AS 'Описание единицы измерения',
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
        UT.id AS 'ID типа измерения',
        UT.[title] AS 'Наименование типа единицы измерения'
    FROM
        UnitsTypes UT
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
        *,
        PivotTable.[Шаг] AS 'Шаг перфорации, мм',
        PivotTable.[Ширина] AS 'Ширина перфорации, мм',
        PivotTable.[Длина] AS 'Длина перфорации, мм'
    FROM
        (
        SELECT
            PER.id AS 'ID перфорации',
            DV.[Наименование] AS 'Наименование перфорации',
            DV.[Сокращенное наименование] AS 'Сокращенное наименование перфорации',
            DV.[Описание] AS 'Описание перфорации',
            UP.[Value] AS 'Размер перфорации',
            UTV.[Наименование типа единицы измерения] AS 'Единица измерения'
        FROM
            Perforations PER
            JOIN DescriptorsView DV ON PER.[idDescriptor] = DV.[ID дескриптора]
            JOIN UnitsPerforations UP ON UP.idPerforation = PER.id
            JOIN UnitsView UV ON UV.[ID единицы измерения] = UP.idUnit
            JOIN UnitsTypesView UTV ON UTV.[ID типа измерения] = UP.idType
        ) AS SourceTable
    PIVOT
        (
        MAX([Размер перфорации])
        FOR [Единица измерения] IN ([Шаг], [Ширина], [Длина])
        ) AS PivotTable
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
    SELECT
        PK.id AS 'ID упаковки',
        DV.[Наименование] AS 'Наименование упаковки',
        DV.[Сокращенное наименование] AS 'Сокращенное наименование упаковки',
        DV.[Описание] AS 'Описание упаковки',
        PivotTable.[Вес] AS 'Вес упаковки, кг',
        PivotTable.[Ширина] AS 'Ширина упаковки, мм',
        PivotTable.[Длина] AS 'Длина упаковки, мм',
        PivotTable.[Высота] AS 'Высота упаковки, мм'
    FROM
        (
    SELECT
            PK.id,
            UTV.[Наименование типа единицы измерения],
            UP.[Value]
        FROM
            Packages PK
            JOIN DescriptorsView DV ON PK.[idDescriptor] = DV.[ID дескриптора]
            LEFT JOIN UnitsPackages UP ON UP.idPackage = PK.id
            LEFT JOIN UnitsView UV ON UV.[ID единицы измерения] = UP.idUnit
            LEFT JOIN UnitsTypesView UTV ON UTV.[ID типа измерения] = UP.idType
    ) AS SourceTable
PIVOT (
    MAX([Value])
    FOR [Наименование типа единицы измерения] IN 
    (
        [Вес], 
        [Ширина], 
        [Длина], 
        [Высота]
    )
) AS PivotTable
        JOIN Packages PK ON PivotTable.id = PK.id
        JOIN DescriptorsView DV ON PK.[idDescriptor] = DV.[ID дескриптора];

--#endregion
--#endregion

--#region Представление ProductUnitsView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[ProductUnitsView]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[ProductUnitsView]
AS
    (
    SELECT
        UP.id AS 'ID связи продукт-измерение',
        P.id AS 'ID товара',
        P.idDescriptor AS 'ID дескриптора товара',
        U.[ID единицы измерения],
        U.[Наименование единицы измерения],
        U.[Сокращенное наименование единицы измерения],
        UT.[ID типа измерения],
        UT.[Наименование типа единицы измерения],
        UP.[value] AS 'Значение'
    FROM
        Products P
        LEFT JOIN UnitsProducts UP ON UP.idProduct = P.id
        LEFT JOIN UnitsView U ON U.[ID единицы измерения] = UP.idUnit
        LEFT JOIN UnitsTypesView UT ON UT.[ID типа измерения] = UP.idType
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
        N.id AS 'ID документа',
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
        M.id AS 'ID материала',
        -- M.[idMaterialNorm] AS 'ID Нормативного документа материалов',
        -- M.[idPrepackNorm] AS 'ID Норматива документа сырья',
        DV.[Наименование] AS 'Наименование материала',
        DV.[Сокращенное наименование] AS 'Сокращенное наименование материала',
        DV.[Код] AS 'Обозначение материала',
        M.[type] AS 'Обозначение материала/марка стали',
        NVM.[Наименование документа] AS 'Стандарт материала',
        NVM.[Номер документа] AS 'Код стандарта материала',
        NVP.[Наименование документа] AS 'Стандарт сырья',
        NVP.[Номер документа] AS 'Код стандарта сырья'
    FROM
        Materials M
        LEFT JOIN DescriptorsView DV ON M.[idDescriptor] = DV.[ID дескриптора]
        LEFT JOIN NormsView NVM ON M.idMaterialNorm = NVM.[ID документа]
        LEFT JOIN NormsView NVP ON M.idPrepackNorm = NVP.[ID документа]
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
        GA.id AS 'ID применения подгруппы',
        A.*,
        SG.*
    FROM
        GroupsApplications GA
        LEFT JOIN ApplicationsView A ON GA.[idApplication] = A.[ID направления]
        LEFT JOIN SubGroupsView SG ON GA.[idSubGroup] = SG.[ID подгруппы]
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
        CV.id AS 'ID покрытия',
        DV.[Наименование] AS 'Наименование покрытия',
        DV.[Сокращенное наименование] AS 'Сокращенное наименование покрытия',
        DV.[Код] AS 'Обозначение покрытия',
        CV.[thickness] AS 'Толщина покрытия',
        NV.[Наименование документа] AS 'Название стандарта',
        NV.[Номер документа] AS 'Номер стандарта'
    FROM
        Covers CV
        JOIN DescriptorsView DV ON CV.[idDescriptor] = DV.[ID дескриптора]
        LEFT JOIN NormsView NV ON NV.[ID документа] = CV.idNorm
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
CREATE VIEW dbo.[ManufacturersView]
AS
    (
    SELECT
        M.id AS 'ID производителя',
        DV.[Наименование] AS 'Наименование производителя',
        DV.[Сокращенное наименование] AS 'Сокращенное наименование производителя',
        M.TIN AS 'ИНН'
    FROM
        Manufacturers M
        JOIN DescriptorsView DV ON M.[idDescriptor] = DV.[ID дескриптора]
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
        VC.id AS 'ID артикула',
        P.id AS 'ID продукта',
        -- Как это ни странно само значение артикула лежит в именовании, 
        -- поскольку код - целое число и там не могут быть артикулы типа A-1234-E
        DV.[Наименование] AS 'Артикул',
        -- DV.[Код] AS 'Артикул',
        VC.[isActual] AS 'Актуальность',
        VC.[isSale] AS 'Тип',
        VC.[isPublic] AS 'Публичность',
        VC.[codeAccountant] AS 'Бухгалтерский код',
        M.[Наименование производителя] AS 'Производитель'
    FROM
        VendorCodes VC
        JOIN DescriptorsView DV ON VC.[idDescriptor] = DV.[ID дескриптора]
        LEFT JOIN ManufacturersView M ON VC.[idManufacturer] = M.[ID производителя]
        LEFT JOIN ProductsVendorCodes PVC ON PVC.idCode  = VC.id
        LEFT JOIN Products P ON P.id = PVC.idProduct
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
CREATE VIEW dbo.[ResourceTypesView]
AS
    (
    SELECT
        RT.id AS 'ID типа ресурса',
        RT.title AS 'Наименование типа ресурса',
        RT.[extension ] AS 'Расширение ресурса'
    FROM
        ResourceTypes RT
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
        R.id AS 'ID ресурса',
        R.[URL] AS 'URL ресурса',
        DR.title AS 'Наименование ресурса',
        DV.Наименование AS 'Наименование объекта',
        DR.idDescriptor AS 'ID дескриптора объекта',
        DV.[ID дескриптора] AS 'ID дескриптора ресурса',
        RTV.[ID типа ресурса],
        RTV.[Наименование типа ресурса],
        RTV.[Расширение ресурса]
    FROM
        Resources R
        LEFT JOIN DescriptorsResources DR ON DR.idResource = R.id
        LEFT JOIN DescriptorsView DV ON DV.[ID дескриптора] = DR.idDescriptor
        LEFT JOIN ResourceTypesView RTV ON RTV.[ID типа ресурса] = DR.idResourceType

);
--#endregion
--#endregion

-- Проверить файлы
--#region Представление ResourcesViewProducts
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[ResourcesViewProducts]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[ResourcesViewProducts]
AS
    (
    -- SELECT
    --     [ID ресурса],
    --     [URL ресурса],
    --     [Наименование объекта],
    --     [Динамические блоки Autocad, Наименование],
    --     [Библиотека BIM для Revit, Наименование],
    --     [Альбом типовых узлов, Наименование]
    -- FROM
    --     (
    --     SELECT
    --         *
    --     FROM
    --         ResourcesView
    --     ) AS SourceTable
    -- PIVOT
    --     (
    --         MAX([Расширение ресурса])
    --         FOR [Наименование типа ресурса] IN 
    --             (
    --                 [Динамические блоки Autocad, Наименование], 
    --                 [Библиотека BIM для Revit, Наименование],
    --                 [Альбом типовых узлов, Наименование]
    --             )
    --     ) AS PivotTable
    SELECT
        RV."ID ресурса" AS "ID ресурса",
        RV."URL ресурса" AS "URL ресурса",
        RV."Наименование ресурса" AS "Наименование ресурса",
        RV."Наименование объекта" AS "Наименование объекта",
        RV."ID дескриптора объекта" AS "ID дескриптора объекта",
        RV."ID дескриптора ресурса" AS "ID дескриптора ресурса",
        RV."ID типа ресурса" AS "ID типа ресурса",
        RV."Наименование типа ресурса" AS "Наименование типа ресурса",
        RV."Расширение ресурса" AS "Расширение ресурса",

        P.id AS "ID продукта",
        P.idDescriptor AS "ID дескриптора продукта",
        P.idNorm AS "ID стандарта продукта",
        P.idSubGroup AS "ID подгруппы продукта",
        P.idCover AS "ID покрытия продукта",
        P.idMaterial AS "ID материала продукта",
        P.idPerforation AS "ID перфорации продукта",
        P.idPackage AS "ID упаковки продукта",
        P.isInStock AS "На складе продукт"

    FROM
        ResourcesView AS RV
        INNER JOIN
        Products AS P
        ON
    P.idDescriptor = RV."ID дескриптора объекта"

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
    SELECT
        *
    FROM
        (
    SELECT
            P.id AS 'ID продукта',
            VC.Артикул,
            DV.[Наименование] AS 'Наименование продукта',
            DV.[Сокращенное наименование] AS 'Сокращенное наименование продукта',
            DV.Описание AS 'Описание продукта',
            SG.[Наименование подгруппы],
            SG.[Сокращенное наименование подгруппы],
            SG.[Код подгруппы],
            SG.[URL изображения подгруппы],
            G.[Наименование группы],
            G.[Сокращенное наименование группы],
            G.[Код группы],
            G.[URL изображения группы],
            C.[Наименование класса],
            C.[Сокращенное наименование класса],
            C.[Код класса],
            C.[URL изображения класса],
            Cov.[Наименование покрытия],
            Cov.[Название стандарта],
            Cov.[Обозначение покрытия],
            Cov.[Толщина покрытия],
            Mat.[Наименование материала],
            Mat.[Код стандарта материала],
            Mat.[Код стандарта сырья],
            PNorm.[Наименование документа],
            PNorm.[Номер документа],
            P.isInStock AS 'В наличии/на заказ',
            PerfV.[Наименование перфорации],
            PerfV.[Шаг перфорации, мм],
            UT.[Наименование типа единицы измерения],
            UP.[value] AS 'Значение',
            PacV.[Наименование упаковки],
            PacV.[Сокращенное наименование упаковки],
            PacV.[Вес упаковки, кг],
            PacV.[Длина упаковки, мм],
            PacV.[Ширина упаковки, мм],
            PacV.[Высота упаковки, мм],
            VCV.Актуальность AS 'Актуальность артикула',
            VCV.Публичность AS 'Публичность артикула',
            VCV.Тип AS 'В продаже',
            VCV.[Бухгалтерский код],
            VCV.Производитель,
            BUV.[Наименование схема нагрузки],
            BUV.[URL схемы нагрузки],
            BUV.[Наименование BIM-библиотеки],
            BUV.[URL BIM-библиотеки],
            BUV.[Наименование альбома типовых узлов],
            BUV.[URL альбома типовых узлов],
            BUV.[Наименование файла блоков чертежей],
            BUV.[URL блоков чертежей]
        FROM
            Products P
            JOIN DescriptorsView DV ON P.[idDescriptor] = DV.[ID дескриптора]
            LEFT JOIN VendorCodesView VC ON P.id = VC.[ID продукта]
            LEFT JOIN SubGroupsView SG ON SG.[ID подгруппы] = P.idSubGroup
            LEFT JOIN GroupsView G ON G.[ID группы] = SG.[ID группы]
            LEFT JOIN ClassesView C ON c.[ID класса] = G.[ID класса]
            LEFT JOIN CoversView Cov ON cov.[ID покрытия] = p.idCover
            LEFT JOIN MaterialsView Mat ON Mat.[ID материала] = p.idMaterial
            LEFT JOIN NormsView PNorm ON PNorm.[ID документа] = p.idNorm
            LEFT JOIN PerforationsView PerfV ON PerfV.[ID перфорации] = p.idPerforation
            LEFT JOIN UnitsProducts UP ON UP.idProduct = P.id
            LEFT JOIN UnitsTypesView UT ON UT.[ID типа измерения] = UP.idType
            LEFT JOIN PackagesView PacV ON PacV.[ID упаковки] = P.idPackage
            LEFT JOIN VendorCodesView VCV ON VCV.[ID продукта] = P.id
            LEFT JOIN GroupsApplicationsView GAV ON GAV.[ID подгруппы] = P.idSubGroup
            LEFT JOIN ApplicationsView AV ON AV.[ID направления] = GAV.[ID направления]
            LEFT JOIN BuisnessUnitsView BUV ON AV.[ID направления] = BUV.[ID направления]
    ) AS SourceTable
PIVOT (
    MAX([Значение])
    FOR [Наименование типа единицы измерения] IN 
    (
        [Вес], 
        [Длина], 
        [Ширина], 
        [Высота], 
        [Объем], 
        [Количество], 
        [Сосредоточенная нагрузка], 
        [Распределенная нагрузка], 
        [Толщина], 
        [Количество в упаковке], 
        [Минимальный заказ], 
        [Кратность заказа]
    )
) AS PivotTable;
--#endregion
--#endregion

-- Представления ниже нуждаются в проверке!

--Пока что отключено 
--#region Представление ProductsVendorCodesView
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[ProductsVendorCodesView]; 
--#endregion
--#region Создание
GO
-- CREATE VIEW dbo.[ProductsVendorCodesView]
-- AS
--     (
--     SELECT
--         PVC.id AS 'ID',
--         P.[Наименование продукта] AS 'Продукт',
--         VC.[Артикул] AS 'Артикул'
--     FROM
--         ProductsVendorCodes PVC
--         JOIN ProductsView P ON PVC.[idProduct] = P.[ID продукта]
--         JOIN VendorCodesView VC ON PVC.[idCode] = VC.[ID артикула]
-- );
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
        P1.[ID продукта] AS 'ID Оригинала',
        P1.[Наименование продукта] AS 'Наименование оригинала',
        P2.[ID продукта] AS 'ID Аналога',
        P2.[Наименование продукта] AS 'Наименование аналога'
    FROM
        ProductsAnalogs PA
        JOIN ProductsView P1 ON PA.[idOriginal] = P1.[ID продукта]
        JOIN ProductsView P2 ON PA.[idAnalog] = P2.[ID продукта]
);
--#endregion
--#endregion



