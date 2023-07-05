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
        DV.*,
        A.[idBuisnessUnit] AS 'ID Направления'
    FROM
        Applications A
        JOIN DescriptorsView DV ON A.[idDescriptor] = DV.[ID дескриптора]
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
        RT.id AS 'ID',
        RT.title AS 'Наименование'
    FROM
        ResourceTypes RT
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

SELECT
    *
FROM
    ApplicationsView;
SELECT
    *
FROM
    ResourceTypesView;

SELECT
    *
FROM
    DescriptorsResourcesView;