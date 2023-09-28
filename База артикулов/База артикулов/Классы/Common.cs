namespace База_артикулов.Классы
{
    public static class Common
    {
        public static class Strings
        {
            public static class RegularExpressions
            {
                public static string regInt = @"[0-9]{1,}$";
                public static string regFloat = @"[+-]?([0-9]*[,])?[0-9]+$";
            }
            public static class Errors
            {
                public static string fieldIsNotFoundInObject = "Поле не найдено в объекте";
                public static string incorrectPath = "Указан некорректный путь!";
                public static string incorrectList = "Передан некорректный список";
                public static string incorrectIntStr = "Ожидается целое число";
                public static string incorrectFloatStr = "Ожидается вещественное число";
                public static string incorrectDescriptorsCount = "Количество дескрипторов не соответствует числу записей!";
                public static string emptyObject = "Передана пустая ссылка на объект!";
                public static string notFolder = "Элемент не является папкой!";
            }
            public static class Warnings
            {

            }
            public static class Messages
            {
                public static string importNotStarted = "Импорт файлов еще не запущен!";
                public static string importStarted = "Импорт файлов запущен!";
                public static string functionalityDisabled = "Эта функциональность отключена";
            }
            public static class Controls
            {
                public static string btnEdit = "Изменить";
                public static string btnDelete = "Удалить";
                public static string btnAdd = "Добавить";
                public static string isContainsHeaders = "Таблица содержит заголовки";
                public static string isNotContainsHeaders = "Таблица не содержит заголовки";
                public static string isContainsDescription = "Таблица содержит описание для заголовков (2 строка)";
                public static string isNotContainsDescription = "Таблица не содержит описание для заголовков (2 строка)";
            }
            public static class Extensions
            {
                public static string xlsx = ".xlsx";
                public static string csv = ".csv";
            }
            public static class Columns
            {
                public static string id = "id";
                public static string idDescriptor = "idDescriptor";
            }
            public static class Path
            {
                public static class Cloud
                {
                    public static string Resources = "Resources";
                    public static string bim = "Resources/bim";
                    public static string dwg = "Resources/dwg";
                    public static string images = "Resources/images";
                    public static string pdf = "Resources/pdf";
                }
                public static class Local
                {
                    public static string imagesFolderName = "Изображения";
                    public static string imagesCachedFolderName = "Кешированные";
                    public static string cache = "Изображения/Кешированные/";
                }
            }
            public static class Titles
            {
                public static class Windows
                {
                    public static string noAction = "Действие для окна не указано";
                    public static string add = "Добавление элемента";
                    public static string edit = "Редактирование элемента";
                    public static string delete = "Удаление элемента";
                }
                public static class Controls
                {
                    public static class Buttons
                    {
                        public static string createItem = "Создать";
                        public static string addItem = "Добавить";
                        public static string saveChanges = "Сохранить изменения";
                        public static string cancel = "Отменить";
                    }
                }

            }
        }
    }
}
