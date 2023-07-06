--#region Пример выборки для представления XXX
SELECT
    *
FROM
    XXX;
--#endregion


SELECT
    P.id AS 'ID продукта',
    DV.[Наименование] AS 'Наименование продукта',
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

SELECT * FROM UnitsView

