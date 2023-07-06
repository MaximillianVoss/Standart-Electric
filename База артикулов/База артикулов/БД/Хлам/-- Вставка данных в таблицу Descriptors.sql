-- Вставка данных в таблицу Descriptors
INSERT INTO Descriptors
    (code, title, titleShort, titleDisplay, description)
VALUES
    ('Applications', 'Применения', 'Applications', 'Применения', 'Таблица применений для различных продуктов и услуг'),
    ('BuisnessUnits', 'Направления', 'BuisnessUnits', 'Направления', 'Таблица направлений бизнеса в различных областях'),
    ('Classes', 'Классы', 'Classes', 'Классы', 'Таблица классов товаров или услуг'),
    ('Covers', 'Покрытия', 'Covers', 'Покрытия', 'Таблица типов покрытий, используемых в продуктах или услугах'),
    ('Descriptors', 'Описания', 'Descriptors', 'Описания', 'Таблица описаний различных аспектов продуктов или услуг'),
    ('DescriptorsImages', 'Изображения для описаний', 'DescriptorsImages', 'Изображения для описаний', 'Таблица изображений, связанных с описаниями продуктов или услуг'),
    ('Groups', 'Группы', 'Groups', 'Группы', 'Таблица групп товаров или услуг'),
    ('GroupsApplications', 'Применения групп', 'GroupsApplications', 'Применения групп', 'Таблица применений для различных групп товаров или услуг'),
    ('Images', 'Изображения', 'Images', 'Изображения', 'Таблица изображений продуктов или услуг'),
    ('LoadDiagrams', 'Типы нагрузок', 'LoadDiagrams', 'Типы нагрузок', 'Таблица различных типов нагрузок для продуктов или услуг'),
    ('Manufacturers', 'Производители', 'Manufacturers', 'Производители', 'Таблица производителей различных товаров или услуг'),
    ('Materials', 'Материалы', 'Materials', 'Материалы', 'Таблица материалов, используемых в товарах или услугах'),
    ('Norms', 'Нормативные документы', 'Norms', 'Нормативные документы', 'Таблица нормативных документов, связанных с товарами или услугами'),
    ('Packages', 'Упаковки', 'Packages', 'Упаковки', 'Таблица типов упаковки для товаров или услуг'),
    ('Perforations', 'Перфорации', 'Perforations', 'Перфорации', 'Таблица типов перфорации для товаров или услуг'),
    ('Products', 'Продукты', 'Products', 'Продукты', 'Таблица продуктов или услуг компании'),
    ('ProductsAnalogs', 'Продукты-Аналоги', 'ProductsAnalogs', 'Продукты-Аналоги', 'Таблица продуктов-аналогов'),
    ('ProductsVendorCodes', 'Коды продуктов поставщиков', 'ProductsVendorCodes', 'Коды продуктов поставщиков', 'Таблица кодов продуктов поставщиков'),
    ('SubGroups', 'Подгруппы', 'SubGroups', 'Подгруппы', 'Таблица подгрупп товаров или услуг'),
    ('Units', 'Единицы измерения', 'Units', 'Единицы измерения', 'Таблица единиц измерения для различных параметров товаров или услуг'),
    ('UnitsPakages', 'измерения для упаковки', 'UnitsPakages', 'измерения для упаковки', 'Таблица единиц измерения для параметров упаковки'),
    ('UnitsPerforations', 'измерения для перфорации', 'UnitsPerforations', 'измерения для перфорации', 'Таблица единиц измерения для параметров перфорации'),
    ('UnitsProducts', 'измерения для продуктов', 'UnitsProducts', 'измерения для продуктов', 'Таблица единиц измерения для параметров продуктов'),
    ('UnitsTypes', 'Типы единиц измерения', 'UnitsTypes', 'Типы единиц измерения', 'Таблица типов единиц измерения'),
    ('ViewTypes', 'Типы представлений', 'ViewTypes', 'Типы представлений', 'Таблица типов представлений для таблиц'),
    ('ViewsTypes', 'Типы представлений для типов', 'ViewsTypes', 'Типы представлений для типов', 'Таблица типов представлений для различных типов в таблицах');

-- Вставка данных в таблицу ViewTypes
INSERT INTO ViewTypes
    (idDescriptor)
SELECT
    id
FROM
    Descriptors
WHERE code IN ('ViewTypes', 'ViewsTypes');

-- Вставка данных в таблицу Tables
INSERT INTO Tables
    (idDescriptor)
SELECT
    id
FROM
    Descriptors
WHERE code IN ('Applications', 'BuisnessUnits', 'Classes', 'Covers', 'Descriptors', 'DescriptorsImages', 'Groups', 'GroupsApplications', 'Images', 'LoadDiagrams', 'Manufacturers', 'Materials', 'Norms', 'Packages', 'Perforations', 'Products', 'ProductsAnalogs', 'ProductsVendorCodes', 'SubGroups', 'Units', 'UnitsPakages', 'UnitsPerforations', 'UnitsProducts', 'UnitsTypes');

-- Вставка данных в таблицу Views
INSERT INTO Views
    (idDescriptor, idType)
SELECT
    id,
    (SELECT
        id
    FROM
        ViewTypes
    WHERE code = 'User')
FROM
    Descriptors
WHERE code = 'ProductsView';

-- Вставка данных в таблицу ViewsTables
INSERT INTO ViewsTables
    (idTable, idView)
SELECT
    t.id,
    v.id
FROM
    Tables t
    JOIN Views v ON t.idDescriptor = v.idDescriptor
WHERE t.idDescriptor IN (SELECT
    id
FROM
    Descriptors
WHERE code IN ('Applications', 'BuisnessUnits', 'Classes', 'Covers', 'Descriptors', 'DescriptorsImages', 'Groups', 'GroupsApplications', 'Images', 'LoadDiagrams', 'Manufacturers', 'Materials', 'Norms', 'Packages', 'Perforations', 'Products', 'ProductsAnalogs', 'ProductsVendorCodes', 'SubGroups', 'Units', 'UnitsPakages', 'UnitsPerforations', 'UnitsProducts', 'UnitsTypes'));

-- Обновление типа представлений на пользовательский
UPDATE Views
SET idType = (SELECT
    id
FROM
    ViewTypes
WHERE idDescriptor = 
(SELECT
    id
FROM
    Descriptors
WHERE code = 'User'))
WHERE id IN (SELECT
    idView
FROM
    ViewsTables);


SELECT
    *
FROM
    Tables;
SELECT
    *
FROM
    Views;
SELECT
    *
FROM
    ViewTypes;
SELECT
    *
FROM
    ViewsTables;


INSERT INTO Descriptors
    (code, title, titleShort, titleDisplay, description)
VALUES
    ('User', 'Пользовательский', 'User', 'Пользовательский', 'Тип представления для пользовательского использования');

INSERT INTO Descriptors
    (code, title, titleShort, titleDisplay, description)
VALUES
    ('Technical', 'Технический', 'Technical', 'Технический', 'Тип представления для технического использования');

UPDATE ViewTypes
SET idDescriptor = (SELECT
    id
FROM
    Descriptors
WHERE code = 'User')
WHERE id = 1;
-- Идентификатор типа представления "Пользовательский"

UPDATE ViewTypes
SET idDescriptor = (SELECT
    id
FROM
    Descriptors
WHERE code = 'Technical')
WHERE id = 2;
-- Идентификатор типа представления "Технический"


SELECT
    *
FROM
    ViewTypes;

SELECT
    *
FROM
    TablesViewsView;
