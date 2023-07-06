--#region Представление ProductsViewDisplay
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[ProductsViewDisplay]; 
--#endregion
--#region Создание
GO
CREATE VIEW ProductsViewDisplay
AS
    SELECT
        P.id,
        D1.'Артикул' AS 'Артикул',
        D2.'Наименование' AS 'Полное наименование',
        D2.'Сокращенное наименование' AS 'Краткое обозначение товарной подгруппы',
        D2.'Наименование' AS 'Краткое обозначение продукта',
        D2.'Наименование' AS 'Наименование товарной подгруппы',
        D2.'Код' AS 'Код товарной подгруппы',
        R1.URL AS 'URL изображения товарной подгруппы',
        D3.'Наименование' AS 'Наименование товарной группы',
        D3.'Код' AS 'Код товарной группы',
        R2.URL AS 'URL изображения товарной группы',
        D4.'Наименование' AS 'Наименование товарного класса',
        D4.'Код' AS 'Код товарного класса',
        R3.URL AS 'URL изображения товарного класса',
        D5.'Наименование' AS 'Единица измерения',
        UP.weight AS 'Вес, кг',
        UP.weightPerMeter AS 'Вес, кг/м',
        UP.height AS 'Высота, мм',
        UP.width AS 'Ширина, мм',
        UP.length AS 'Длина, мм',
        UP.thickness AS 'Толщина, мм',
        UP.volume AS 'Объем, мл',
        D6.'Наименование' AS 'Стандарт покрытия',
        D7.'Наименование' AS 'Наименование покрытия',
        D7.'Код' AS 'Обозначение покрытия',
        UP.coatingThickness AS 'Толщина покрытия, мкм',
        D8.'Наименование' AS 'Материал',
        D8.'Описание' AS 'ГОСТ материала',
        D9.'Описание' AS 'ГОСТ сырья',
        D10.'Наименование' AS 'ТУ номер',
        D10.'Сокращенное наименование' AS 'ТУ титул',
        D11.'Описание' AS 'Описание краткое',
        D11.'Описание' AS 'Описание полное',
        P.isInStock AS 'Наличие/заказ',
        D12.'Наименование' AS 'Размер перфорации',
        UPF.spacing AS 'Шаг перфорации, мм',
        UPK.quantity AS 'Количество в упаковке, шт',
        UP.minimumOrderQuantity AS 'Минимальный заказ, шт',
        UP.orderQuantityMultiple AS 'Кратность заказа',
        (UP.weight * UPK.quantity) AS 'Вес упаковки, кг',
        UPK.width AS 'Ширина упаковки, мм',
        UPK.length AS 'Длина упаковки, мм',
        UPK.height AS 'Высота упаковки, мм',
        UP.concentratedLoadCapacity AS 'Несущая способность сосредоточенная (БРН), kN',
        UP.distributedLoadCapacity AS 'Несущая способность распределенная (БРН), kNm',
        D13.'Наименование' AS 'Схема нагрузки, Наименование',
        R4.URL AS 'Схема нагрузки, URL',
        D14.'Наименование' AS 'Альбом типовых узлов, Наименование',
        R5.URL AS 'Наименование, URL',
        D15.'Наименование' AS 'Динамические блоки Autocad, Наименование',
        R6.URL AS 'Динамические блоки Autocad, URL',
        D16.'Наименование' AS 'Библиотека BIM для Revit, Наименование',
        R7.URL AS 'Библиотека BIM для Revit, URL',
        VC.isActual AS 'Артикул действующий или устаревший',
        VC.isSale AS 'Артикул в продаже или снят с продаж',
        VC.isPublic AS 'Артикул публичный или внутренний',
        VC.codeAccountant AS 'Бухгалтерский код',
        D17.'Наименование' AS 'Производитель'
    FROM
        Products AS P
        JOIN DescriptorsViewDisplay D1 ON P.idDescriptor = D1.'ID дексриптора'
        JOIN DescriptorsViewDisplay D2 ON D1.'ID дексриптора' = D2.'ID дексриптора'
        JOIN DescriptorsViewDisplay D3 ON D1.'ID дексриптора' = D3.'ID дексриптора'
        JOIN DescriptorsViewDisplay D4 ON D1.'ID дексриптора' = D4.'ID дексриптора'
        JOIN DescriptorsViewDisplay D5 ON P.idUnit = D5.'ID дексриптора'
        JOIN UnitsProducts UP ON P.id = UP.idProduct
        JOIN DescriptorsViewDisplay D6 ON UP.idCover = D6.'ID дексриптора'
        JOIN DescriptorsViewDisplay D7 ON D6.'ID дексриптора' = D7.'ID дексриптора'
        JOIN DescriptorsViewDisplay D8 ON P.idMaterial = D8.'ID дексриптора'
        JOIN DescriptorsViewDisplay D9 ON D8.'ID дексриптора' = D9.'ID дексриптора'
        JOIN DescriptorsViewDisplay D10 ON D8.'ID дексриптора' = D10.'ID дексриптора'
        JOIN DescriptorsViewDisplay D11 ON D1.'ID дексриптора' = D11.'ID дексриптора'
        JOIN DescriptorsViewDisplay D12 ON P.idPerforation = D12.'ID дексриптора'
        JOIN UnitsPerforations UPF ON D12.'ID дексриптора' = UPF.idPerforation AND UP.idUnit = UPF.idUnit
        JOIN UnitsPackages UPK ON P.idPackage = UPK.idPackage AND UP.idUnit = UPK.idUnit
        JOIN VendorCodes VC ON P.id = VC.idProduct
        JOIN DescriptorsViewDisplay D13 ON P.idLoadDiagram = D13.'ID дексриптора'
        JOIN Resources R4 ON D13.'ID дексриптора' = R4.id
        JOIN DescriptorsViewDisplay D14 ON D1.'ID дексриптора' = D14.'ID дексриптора'
        JOIN Resources R5 ON D14.'ID дексриптора' = R5.id
        JOIN DescriptorsViewDisplay D15 ON D1.'ID дексриптора' = D15.'ID дексриптора'
        JOIN Resources R6 ON D15.'ID дексриптора' = R6.id
        JOIN DescriptorsViewDisplay D16 ON D1.'ID дексриптора' = D16.'ID дексриптора'
        JOIN Resources R7 ON D16.'ID дексриптора' = R7.id
        JOIN DescriptorsViewDisplay D17 ON VC.idManufacturer = D17.'ID дексриптора';
--#endregion
--#endregion
