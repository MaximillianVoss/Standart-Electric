
USE [DBSE];
GO

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
        -- TODO: добавить единицы измерения
        Cov.[Наименование покрытия],
        Cov.[Название стандарта],
        Cov.[Обозначение покрытия],
        Cov.[Толщина покрытия],
        --
        Mat.[Наименование материала],
        Mat.[Код стандарта материала],
        Mat.[Код стандарта сырья],
        PNorm.[Наименование документа],
        PNorm.[Номер документа],
        DV.Описание,
        P.isInStock AS 'В наличии/на заказ',
        PerfV.[Размер перфорации]




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



);
--#endregion
--#endregion

SELECT
    *
FROM
    ProductsView