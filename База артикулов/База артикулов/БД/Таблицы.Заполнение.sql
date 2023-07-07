--#region Заполнение ViewTypes плюс дескрипторы
-- Создание дескриптора для типа представления "Пользовательский"
INSERT INTO Descriptors
    (title, titleShort, titleDisplay, description)
VALUES
    ('Пользовательский', 'User', 'Пользовательский', 'Тип представления для пользовательского использования');

-- Создание дескриптора для типа представления "Технический"
INSERT INTO Descriptors
    (title, titleShort, titleDisplay, description)
VALUES
    ('Технический', 'Technical', 'Технический', 'Тип представления для технического использования');

-- Вставка значений в таблицу ViewTypes
INSERT INTO ViewTypes
    (idDescriptor)
VALUES
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'User')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'Technical'));

-- Обновление связанных дескрипторов в таблице ViewTypes
UPDATE ViewTypes
SET idDescriptor = (SELECT
    TOP 1
    id
FROM
    Descriptors
WHERE titleShort = 'User')
WHERE id = 1;

UPDATE ViewTypes
SET idDescriptor = (SELECT
    TOP 1
    id
FROM
    Descriptors
WHERE titleShort = 'Technical')
WHERE id = 2;

-- Просмотр данных
SELECT
    *
FROM
    ViewTypes;
SELECT
    *
FROM
    ViewTypesView;

--#endregion

--#region Заполнение Tables плюс дескрипторы
-- Создание таблицы Tables

-- Вставка данных в таблицу Descriptors для таблиц
INSERT INTO Descriptors
    (title, titleShort, titleDisplay, description)
VALUES
    ('Applications', 'Applications', 'Применения', 'Таблица применений для различных продуктов и услуг'),
    ('BuisnessUnits', 'BuisnessUnits', 'Направления', 'Таблица направлений бизнеса в различных областях'),
    ('Classes', 'Classes', 'Классы', 'Таблица классов товаров или услуг'),
    ('Covers', 'Covers', 'Покрытия', 'Таблица типов покрытий, используемых в продуктах или услугах'),
    ('Descriptors', 'Descriptors', 'Описания', 'Таблица описаний различных аспектов продуктов или услуг'),
    ('DescriptorsImages', 'DescriptorsImages', 'Изображения для описаний', 'Таблица изображений, связанных с описаниями продуктов или услуг'),
    ('Groups', 'Groups', 'Группы', 'Таблица групп товаров или услуг'),
    ('GroupsApplications', 'GroupsApplications', 'Применения групп', 'Таблица применений для различных групп товаров или услуг'),
    ('Images', 'Images', 'Изображения', 'Таблица изображений продуктов или услуг'),
    ('LoadDiagrams', 'LoadDiagrams', 'Типы нагрузок', 'Таблица различных типов нагрузок для продуктов или услуг'),
    ('Manufacturers', 'Manufacturers', 'Производители', 'Таблица производителей различных товаров или услуг'),
    ('Materials', 'Materials', 'Материалы', 'Таблица материалов, используемых в товарах или услугах'),
    ('Norms', 'Norms', 'Нормативные документы', 'Таблица нормативных документов, связанных с товарами или услугами'),
    ('Packages', 'Packages', 'Упаковки', 'Таблица типов упаковки для товаров или услуг'),
    ('Perforations', 'Perforations', 'Перфорации', 'Таблица типов перфорации для товаров или услуг'),
    ('Products', 'Products', 'Продукты', 'Таблица продуктов или услуг компании'),
    ('ProductsAnalogs', 'ProductsAnalogs', 'Продукты-Аналоги', 'Таблица продуктов-аналогов'),
    ('ProductsVendorCodes', 'ProductsVendorCodes', 'Коды продуктов поставщиков', 'Таблица кодов продуктов поставщиков'),
    ('SubGroups', 'SubGroups', 'Подгруппы', 'Таблица подгрупп товаров или услуг'),
    ('Units', 'Units', 'Единицы измерения', 'Таблица единиц измерения для различных параметров товаров или услуг'),
    ('UnitsPakages', 'UnitsPakages', 'Единицы измерения для упаковки', 'Таблица единиц измерения для параметров упаковки'),
    ('UnitsPerforations', 'UnitsPerforations', 'Единицы измерения для перфорации', 'Таблица единиц измерения для параметров перфорации'),
    ('UnitsProducts', 'UnitsProducts', 'Единицы измерения для продуктов', 'Таблица единиц измерения для параметров продуктов'),
    ('UnitsTypes', 'UnitsTypes', 'Типы единиц измерения', 'Таблица типов единиц измерения'),
    ('ViewTypes', 'ViewTypes', 'Типы представлений', 'Таблица типов представлений для таблиц'),
    ('ViewsTypes', 'ViewsTypes', 'Типы представлений для типов', 'Таблица типов представлений для различных типов в таблицах');


-- Вставка данных в таблицу Tables
INSERT INTO Tables
    (idDescriptor)
VALUES
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'Applications')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'BuisnessUnits')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'Classes')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'Covers')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'Descriptors')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'DescriptorsImages')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'Groups')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'GroupsApplications')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'Images')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'LoadDiagrams')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'Manufacturers')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'Materials')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'Norms')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'Packages')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'Perforations')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'Products')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'ProductsAnalogs')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'ProductsVendorCodes')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'SubGroups')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'Units')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'UnitsPakages')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'UnitsPerforations')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'UnitsProducts')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'UnitsTypes')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'ViewTypes')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'ViewsTypes'));

SELECT
    *
FROM
    TablesView;

--#endregion

--#region Заполнение Views плюс дескрипторы

-- Создание дескрипторов для представлений и привязка их к таблице Views

INSERT INTO Descriptors
    (title, titleShort, titleDisplay, description)
VALUES
    ('ProductsView', 'ProductsView', 'Продукты', 'Представление для таблицы Продукты'),
    ('ProductsAnalogsView', 'ProductsAnalogsView', 'Продукты-Аналоги', 'Представление для таблицы Продукты-Аналоги'),
    ('ProductsVendorCodesView', 'ProductsVendorCodesView', 'Коды продуктов поставщиков', 'Представление для таблицы Коды продуктов поставщиков'),
    ('ApplicationsView', 'ApplicationsView', 'Применения', 'Представление для таблицы Применения'),
    ('BuisnessUnitsView', 'BuisnessUnitsView', 'Направления', 'Представление для таблицы Направления'),
    ('ClassesView', 'ClassesView', 'Классы', 'Представление для таблицы Классы'),
    ('CoversView', 'CoversView', 'Покрытия', 'Представление для таблицы Покрытия'),
    ('DescriptorsView', 'DescriptorsView', 'Описания', 'Представление для таблицы Описания'),
    ('DescriptorsImagesView', 'DescriptorsImagesView', 'Изображения для описаний', 'Представление для таблицы Изображения для описаний'),
    ('GroupsView', 'GroupsView', 'Группы', 'Представление для таблицы Группы'),
    ('GroupsApplicationsView', 'GroupsApplicationsView', 'Применения групп', 'Представление для таблицы Применения групп'),
    ('ImagesView', 'ImagesView', 'Изображения', 'Представление для таблицы Изображения'),
    ('LoadDiagramsView', 'LoadDiagramsView', 'Типы нагрузок', 'Представление для таблицы Типы нагрузок'),
    ('ManufacturersView', 'ManufacturersView', 'Производители', 'Представление для таблицы Производители'),
    ('MaterialsView', 'MaterialsView', 'Материалы', 'Представление для таблицы Материалы'),
    ('NormsView', 'NormsView', 'Нормативные документы', 'Представление для таблицы Нормативные документы'),
    ('PackagesView', 'PackagesView', 'Упаковки', 'Представление для таблицы Упаковки'),
    ('PerforationsView', 'PerforationsView', 'Перфорации', 'Представление для таблицы Перфорации'),
    ('ProductsView', 'ProductsView', 'Продукты', 'Представление для таблицы Продукты'),
    ('ProductsAnalogsView', 'ProductsAnalogsView', 'Продукты-Аналоги', 'Представление для таблицы Продукты-Аналоги'),
    ('ProductsVendorCodesView', 'ProductsVendorCodesView', 'Коды продуктов поставщиков', 'Представление для таблицы Коды продуктов поставщиков'),
    ('SubGroupsView', 'SubGroupsView', 'Подгруппы', 'Представление для таблицы Подгруппы'),
    ('UnitsView', 'UnitsView', 'Единицы измерения', 'Представление для таблицы Единицы измерения'),
    ('UnitsPakagesView', 'UnitsPakagesView', 'измерения для упаковки', 'Представление для таблицы измерения для упаковки'),
    ('UnitsPerforationsView', 'UnitsPerforationsView', 'измерения для перфорации', 'Представление для таблицы измерения для перфорации'),
    ('UnitsProductsView', 'UnitsProductsView', 'измерения для продуктов', 'Представление для таблицы измерения для продуктов'),
    ('UnitsTypesView', 'UnitsTypesView', 'Типы единиц измерения', 'Представление для таблицы Типы единиц измерения');

-- Вставка данных в таблицу Views
INSERT INTO Views
    (idDescriptor)
VALUES
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'ProductsView')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'ProductsAnalogsView')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'ProductsVendorCodesView')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'ApplicationsView')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'BuisnessUnitsView')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'ClassesView')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'CoversView')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'DescriptorsView')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'DescriptorsImagesView')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'GroupsView')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'GroupsApplicationsView')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'ImagesView')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'LoadDiagramsView')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'ManufacturersView')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'MaterialsView')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'NormsView')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'PackagesView')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'PerforationsView')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'SubGroupsView')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'UnitsView')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'UnitsPakagesView')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'UnitsPerforationsView')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'UnitsProductsView')),
    ((SELECT
            TOP 1
            id
        FROM
            Descriptors
        WHERE titleShort = 'UnitsTypesView'));

-- Обновление типа представлений
-- Установка технических представлений
UPDATE Views
SET idType = (
    SELECT
    TOP 1
    id
FROM
    ViewTypesView
WHERE [Наименование] = 'Технический'
);

-- Установка пользовательских представлений
UPDATE Views
SET idType = ( SELECT
    TOP 1
    id
FROM
    ViewTypesView
WHERE [Наименование] = 'Пользовательский'
)
WHERE id IN
(
SELECT
    [ID]
FROM
    ViewsView
WHERE [Наименование представления] IN
(
    
'NormsView',
    'UnitsView',
    'UnitsTypesView',
    'MaterialsView',
    'ManufacturersView',
    'ResourceTypesView',
    'CoversView',
    'PerforationsView',
    'PackagesView',
    'LoadDiagramsView',
    'ProductsView'
)
);


SELECT
    *
FROM
    Views;

SELECT
    *
FROM
    dbo.[ViewsView];

--#endregion

--#region Заполнение ViewsTables 
SELECT
    *
FROM
    TablesView
INSERT INTO ViewsTables
    (idTable, idView)
VALUES
    ((SELECT
            ID
        FROM
            TablesView
        WHERE [Наименование] = 'Applications'), (SELECT
            ID
        FROM
            ViewsView
        WHERE [Наименование представления] = 'ApplicationsView')),
    ((SELECT
            ID
        FROM
            TablesView
        WHERE [Наименование] = 'BuisnessUnits'), (SELECT
            ID
        FROM
            ViewsView
        WHERE [Наименование представления] = 'BuisnessUnitsView')),
    ((SELECT
            ID
        FROM
            TablesView
        WHERE [Наименование] = 'Classes'), (SELECT
            ID
        FROM
            ViewsView
        WHERE [Наименование представления] = 'ClassesView')),
    ((SELECT
            ID
        FROM
            TablesView
        WHERE [Наименование] = 'Covers'), (SELECT
            ID
        FROM
            ViewsView
        WHERE [Наименование представления] = 'CoversView')),
    ((SELECT
            ID
        FROM
            TablesView
        WHERE [Наименование] = 'Descriptors'), (SELECT
            ID
        FROM
            ViewsView
        WHERE [Наименование представления] = 'DescriptorsView')),
    ((SELECT
            ID
        FROM
            TablesView
        WHERE [Наименование] = 'DescriptorsImages'), (SELECT
            ID
        FROM
            ViewsView
        WHERE [Наименование представления] = 'DescriptorsImagesView')),
    ((SELECT
            ID
        FROM
            TablesView
        WHERE [Наименование] = 'Groups'), (SELECT
            ID
        FROM
            ViewsView
        WHERE [Наименование представления] = 'GroupsView')),
    ((SELECT
            ID
        FROM
            TablesView
        WHERE [Наименование] = 'GroupsApplications'), (SELECT
            ID
        FROM
            ViewsView
        WHERE [Наименование представления] = 'GroupsApplicationsView')),
    ((SELECT
            ID
        FROM
            TablesView
        WHERE [Наименование] = 'Images'), (SELECT
            ID
        FROM
            ViewsView
        WHERE [Наименование представления] = 'ImagesView')),
    ((SELECT
            ID
        FROM
            TablesView
        WHERE [Наименование] = 'LoadDiagrams'), (SELECT
            ID
        FROM
            ViewsView
        WHERE [Наименование представления] = 'LoadDiagramsView')),
    ((SELECT
            ID
        FROM
            TablesView
        WHERE [Наименование] = 'Manufacturers'), (SELECT
            ID
        FROM
            ViewsView
        WHERE [Наименование представления] = 'ManufacturersView')),
    ((SELECT
            ID
        FROM
            TablesView
        WHERE [Наименование] = 'Materials'), (SELECT
            ID
        FROM
            ViewsView
        WHERE [Наименование представления] = 'MaterialsView')),
    ((SELECT
            ID
        FROM
            TablesView
        WHERE [Наименование] = 'Norms'), (SELECT
            ID
        FROM
            ViewsView
        WHERE [Наименование представления] = 'NormsView')),
    ((SELECT
            ID
        FROM
            TablesView
        WHERE [Наименование] = 'Packages'), (SELECT
            ID
        FROM
            ViewsView
        WHERE [Наименование представления] = 'PackagesView')),
    ((SELECT
            ID
        FROM
            TablesView
        WHERE [Наименование] = 'Perforations'), (SELECT
            ID
        FROM
            ViewsView
        WHERE [Наименование представления] = 'PerforationsView')),
    ((SELECT
            ID
        FROM
            TablesView
        WHERE [Наименование] = 'Products'), (SELECT
            ID
        FROM
            ViewsView
        WHERE [Наименование представления] = 'ProductsView')),
    ((SELECT
            ID
        FROM
            TablesView
        WHERE [Наименование] = 'ProductsAnalogs'), (SELECT
            ID
        FROM
            ViewsView
        WHERE [Наименование представления] = 'ProductsAnalogsView')),
    ((SELECT
            ID
        FROM
            TablesView
        WHERE [Наименование] = 'ProductsVendorCodes'), (SELECT
            ID
        FROM
            ViewsView
        WHERE [Наименование представления] = 'ProductsVendorCodesView')),
    ((SELECT
            ID
        FROM
            TablesView
        WHERE [Наименование] = 'SubGroups'), (SELECT
            ID
        FROM
            ViewsView
        WHERE [Наименование представления] = 'SubGroupsView')),
    ((SELECT
            ID
        FROM
            TablesView
        WHERE [Наименование] = 'Units'), (SELECT
            ID
        FROM
            ViewsView
        WHERE [Наименование представления] = 'UnitsView')),
    ((SELECT
            ID
        FROM
            TablesView
        WHERE [Наименование] = 'UnitsPakages'), (SELECT
            ID
        FROM
            ViewsView
        WHERE [Наименование представления] = 'UnitsPakagesView')),
    ((SELECT
            ID
        FROM
            TablesView
        WHERE [Наименование] = 'UnitsPerforations'), (SELECT
            ID
        FROM
            ViewsView
        WHERE [Наименование представления] = 'UnitsPerforationsView')),
    ((SELECT
            ID
        FROM
            TablesView
        WHERE [Наименование] = 'UnitsProducts'), (SELECT
            ID
        FROM
            ViewsView
        WHERE [Наименование представления] = 'UnitsProductsView')),
    ((

SELECT
            ID
        FROM
            TablesView
        WHERE [Наименование] = 'UnitsTypes'), (SELECT
            ID
        FROM
            ViewsView
        WHERE [Наименование представления] = 'UnitsTypesView'));


SELECT
    *
FROM
    dbo.[ViewsTables];
--#endregion