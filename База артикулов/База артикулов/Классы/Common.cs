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

    }
}
