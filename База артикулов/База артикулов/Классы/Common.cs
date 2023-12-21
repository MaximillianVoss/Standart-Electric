using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Security.AccessControl;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using База_артикулов.Модели;
using База_артикулов.Формы;

namespace База_артикулов.Классы
{
    public static class Common
    {
        public static class Strings
        {
            public static class RegularExpressions
            {
                public static readonly string regInt = @"[0-9]{1,}$";
                public static readonly string regFloat = @"[+-]?([0-9]*[,])?[0-9]+$";
            }
            public static class WindowsTitles
            {
                public static readonly string create = "Создание";
                public static readonly string update = "Редактирование";
                public static readonly string delete = "Удаление";
            }
            public static class Errors
            {
                public static readonly string fieldIsNotFoundInObject = "Поле не найдено в объекте";
                public static readonly string incorrectPath = "Указан некорректный путь!";
                public static readonly string incorrectList = "Передан некорректный список";
                public static readonly string incorrectIntStr = "Ожидается целое число";
                public static readonly string incorrectFloatStr = "Ожидается вещественное число";
                public static readonly string incorrectDescriptorsCount = "Количество дескрипторов не соответствует числу записей!";
                public static readonly string emptyObject = "Передана пустая ссылка на объект!";
                public static readonly string notFolder = "Элемент не является папкой!";
                public static readonly string failedToGetParam = "Не получилось извлечь параметр!";
                public static readonly string incorrectCreateElement = "Не выбран элемент для создания!";
                public static readonly string incorrectUpdateElement = "Не выбран элемент для редактирования!";
                public static readonly string incorrectDeleteElement = "Не выбран элемент для удаления!";
            }
            public static class Warnings
            {
                // Определения предупреждений
            }
            public static class Messages
            {
                public static readonly string importNotStarted = "Импорт файлов еще не запущен!";
                public static readonly string importStarted = "Импорт файлов запущен!";
                public static readonly string functionalityDisabled = "Эта функциональность отключена";
            }
            public static class Controls
            {
                public static readonly string btnEdit = "Изменить";
                public static readonly string btnDelete = "Удалить";
                public static readonly string btnAdd = "Добавить";
                public static readonly string isContainsHeaders = "Таблица содержит заголовки";
                public static readonly string isNotContainsHeaders = "Таблица не содержит заголовки";
                public static readonly string isContainsDescription = "Таблица содержит описание для заголовков (2 строка)";
                public static readonly string isNotContainsDescription = "Таблица не содержит описание для заголовков (2 строка)";
                //public static readonly string isAvaliable = "";
                //public static readonly string isNotAvaliable = "";
            }
            public static class Extensions
            {
                public static readonly string xlsx = ".xlsx";
                public static readonly string csv = ".csv";
                public static readonly string txt = ".txt";
                public static readonly string pdf = ".pdf";
                public static readonly string docx = ".docx";
                public static readonly string jpg = ".jpg";
                public static readonly string png = ".png";
                public static readonly string html = ".html";
                public static readonly string xml = ".xml";
                public static readonly string json = ".json";
                // Дополнительные расширения
            }

            public static class Columns
            {
                public static readonly string id = "id";
                public static readonly string idDescriptor = "idDescriptor";
            }
            public static class Path
            {
                public static class Cloud
                {
                    public static readonly string Resources = "Resources";
                    public static readonly string bim = "Resources/bim";
                    public static readonly string dwg = "Resources/dwg";
                    public static readonly string images = "Resources/images";
                    public static readonly string pdf = "Resources/pdf";
                }
                public static class Local
                {
                    public static readonly string imagesFolderName = "Изображения";
                    public static readonly string imagesCachedFolderName = "Кешированные";
                    public static readonly string cache = "Изображения/Кешированные/";
                }
            }
            public static class Titles
            {
                public static class Windows
                {
                    public static readonly string noAction = "Действие для окна не указано";
                    public static readonly string add = "Добавление элемента";
                    public static readonly string edit = "Редактирование элемента";
                    public static readonly string delete = "Удаление элемента";
                }
                public static class Controls
                {
                    public static class Buttons
                    {
                        public static readonly string createItem = "Создать";
                        public static readonly string addItem = "Добавить";
                        public static readonly string saveChanges = "Сохранить изменения";
                        public static readonly string cancel = "Отменить";
                    }
                }
            }
        }

        public static class WindowSizes
        {
            public static class SmallH240W300
            {
                public const int Width = 300;
                public const int Height = 240;
            }

            public static class SmallH320W400
            {
                public const int Width = 400;
                public const int Height = 320;
            }

            public static class MediumH600W800
            {
                public const int Width = 800;
                public const int Height = 600;
            }

            public static class LargeH900W1200
            {
                public const int Width = 1200;
                public const int Height = 900;
            }

            // Используйте этот подход для определения дополнительных размеров окон
            // ...

            // Пример класса для окна полноэкранного режима на обычном HD дисплее
            public static class FullscreenHD
            {
                public const int Width = 1920;
                public const int Height = 1080;
            }
        }

        public static class EntityRussianNames
        {
            /// <summary>
            /// Наименования сущностей именительном падеже
            /// </summary>
            public static readonly Dictionary<Type, string> NamesNominative = new Dictionary<Type, string>
            {
               { typeof(Applications), "Приложения" },
                { typeof(BuisnessUnits), "Бизнес-юниты" },
                { typeof(Classes), "Классы" },
                { typeof(Covers), "Крышки" },
                { typeof(Descriptors), "Дескрипторы" },
                { typeof(DescriptorsResources), "Ресурсы дескрипторов" },
                { typeof(Fields), "Поля" },
                { typeof(Groups), "Группы" },
                { typeof(GroupsApplications), "Приложения групп" },
                { typeof(LoadDiagrams), "Схемы загрузки" },
                { typeof(Manufacturers), "Производители" },
                { typeof(Materials), "Материалы" },
                { typeof(Norms), "Нормы" },
                { typeof(Packages), "Упаковки" },
                { typeof(Perforations), "Перфорации" },
                { typeof(Products), "Продукты" },
                { typeof(ProductsAnalogs), "Аналоги продуктов" },
                { typeof(ProductsVendorCodes), "Коды продавцов продуктов" },
                { typeof(Resources), "Ресурсы" },
                { typeof(ResourceTypes), "Типы ресурсов" },
                { typeof(SubGroups), "Подгруппы" },
                { typeof(Tables), "Таблицы" },
                { typeof(Units), "Единицы измерения" },
                { typeof(UnitsPackages), "Упаковки единиц" },
                { typeof(UnitsPerforations), "Перфорации единиц" },
                { typeof(UnitsProducts), "Продукты единиц" },
                { typeof(UnitsTypes), "Типы единиц" },
                { typeof(VendorCodes), "Коды продавцов" },
                { typeof(Views), "Виды" },
                // ... и так далее для всех DbSet объявленных в контексте.
            };
            /// <summary>
            /// Наименования сущностей родительском падеже
            /// </summary>
            public static readonly Dictionary<Type, string> NamesGenitive = new Dictionary<Type, string>
            {
                { typeof(Applications), "Приложений" },
                { typeof(BuisnessUnits), "Бизнес-юнитов" },
                { typeof(Classes), "Классов" },
                { typeof(Covers), "Крышек" },
                { typeof(Descriptors), "Дескрипторов" },
                { typeof(DescriptorsResources), "Ресурсов дескрипторов" },
                { typeof(Fields), "Полей" },
                { typeof(Groups), "Групп" },
                { typeof(GroupsApplications), "Приложений групп" },
                { typeof(LoadDiagrams), "Схем загрузки" },
                { typeof(Manufacturers), "Производителей" },
                { typeof(Materials), "Материалов" },
                { typeof(Norms), "Норм" },
                { typeof(Packages), "Упаковок" },
                { typeof(Perforations), "Перфораций" },
                { typeof(Products), "Продуктов" },
                { typeof(ProductsAnalogs), "Аналогов продуктов" },
                { typeof(ProductsVendorCodes), "Кодов продавцов продуктов" },
                { typeof(Resources), "Ресурсов" },
                { typeof(ResourceTypes), "Типов ресурсов" },
                { typeof(SubGroups), "Подгрупп" },
                { typeof(Tables), "Таблиц" },
                { typeof(Units), "Единиц измерения" },
                { typeof(UnitsPackages), "Упаковок единиц" },
                { typeof(UnitsPerforations), "Перфораций единиц" },
                { typeof(UnitsProducts), "Продуктов единиц" },
                { typeof(UnitsTypes), "Типов единиц" },
                { typeof(VendorCodes), "Кодов продавцов" },
                { typeof(Views), "Видов" },
                // ... и так далее для всех DbSet объявленных в контексте.
            };
            /// <summary>
            /// Наименования сущностей в именительном падеже единственного числа
            /// </summary>
            public static readonly Dictionary<Type, string> NameNominativeSingular = new Dictionary<Type, string>
            {
                { typeof(Application), "Приложение" },
                { typeof(BuisnessUnits), "Бизнес-юнит" },
                { typeof(Classes), "Класс" },
                { typeof(Covers), "Крышка" },
                { typeof(Descriptors), "Дескриптор" },
                { typeof(DescriptorsResources), "Ресурс дескриптора" },
                { typeof(Fields), "Поле" },
                { typeof(Groups), "Группа" },
                { typeof(GroupsApplications), "Приложение группы" },
                { typeof(LoadDiagrams), "Схема загрузки" },
                { typeof(Manufacturers), "Производитель" },
                { typeof(Material), "Материал" },
                { typeof(Norms), "Норма" },
                { typeof(Package), "Упаковка" },
                { typeof(Perforations), "Перфорация" },
                { typeof(Products), "Продукт" },
                { typeof(ProductsAnalogs), "Аналог продукта" },
                { typeof(ProductsVendorCodes), "Код продавца продукта" },
                { typeof(Resources), "Ресурс" },
                { typeof(ResourceTypes), "Тип ресурса" },
                { typeof(SubGroups), "Подгруппа" },
                { typeof(Table), "Таблица" },
                { typeof(Units), "Единица измерения" },
                { typeof(UnitsPackages), "Упаковка единицы" },
                { typeof(UnitsPerforations), "Перфорация единицы" },
                { typeof(UnitsProducts), "Продукт единицы" },
                { typeof(UnitsTypes), "Тип единицы" },
                { typeof(VendorCodes), "Код продавца" },
                { typeof(Views), "Вид" },
                // ... и так далее для всех DbSet объявленных в контексте.
            };
            /// <summary>
            /// Наименования сущностей в родительном падеже единственного числа
            /// </summary>
            public static readonly Dictionary<Type, string> NameGenitiveSingular = new Dictionary<Type, string>
            {
                { typeof(Application), "Приложения" },
                { typeof(BuisnessUnits), "Бизнес-юнита" },
                { typeof(Classes), "Класса" },
                { typeof(Covers), "Крышки" },
                { typeof(Descriptors), "Дескриптора" },
                { typeof(DescriptorsResources), "Ресурса дескриптора" },
                { typeof(Fields), "Поля" },
                { typeof(Groups), "Группы" },
                { typeof(GroupsApplications), "Приложения группы" },
                { typeof(LoadDiagrams), "Схемы загрузки" },
                { typeof(Manufacturers), "Производителя" },
                { typeof(Material), "Материала" },
                { typeof(Norms), "Нормы" },
                { typeof(Package), "Упаковки" },
                { typeof(Perforations), "Перфорации" },
                { typeof(Products), "Продукта" },
                { typeof(ProductsAnalogs), "Аналога продукта" },
                { typeof(ProductsVendorCodes), "Кода продавца продукта" },
                { typeof(Resources), "Ресурса" },
                { typeof(ResourceTypes), "Типа ресурса" },
                { typeof(SubGroups), "Подгруппы" },
                { typeof(Table), "Таблицы" },
                { typeof(Units), "Единицы измерения" },
                { typeof(UnitsPackages), "Упаковки единицы" },
                { typeof(UnitsPerforations), "Перфорации единицы" },
                { typeof(UnitsProducts), "Продукта единицы" },
                { typeof(UnitsTypes), "Типа единицы" },
                { typeof(VendorCodes), "Кода продавца" },
                { typeof(Views), "Вида" },
                // ... и так далее для всех DbSet объявленных в контексте.
            };
        }

        public static class EditModesDescriptions
        {
            public static readonly Dictionary<EditModes, string> Descriptions = new Dictionary<EditModes, string>
            {
                { EditModes.Create, "Создание" },
                { EditModes.Update, "Обновление" },
                { EditModes.Delete, "Удаление" },
                { EditModes.None, "Нет действия" }
            };

            public static readonly Dictionary<EditModes, string> ActionDescriptions = new Dictionary<EditModes, string>
            {
                { EditModes.Create, "Создать" },
                { EditModes.Update, "Обновить" },
                { EditModes.Delete, "Удалить" },
                { EditModes.None, "Нет действия" }
            };

        }
    }
}
