using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CustomControlsWPF
{
    /// <summary>
    /// Хранит данные из таблиц типа DbSet
    /// </summary>
    public class TableData
    {
        #region Поля

        #endregion

        #region Свойства
        public String title { set; get; }
        public String titleDisplay { set; get; }

        /// <summary>
        /// Тип элементов, которые хранятся в коллекции
        /// </summary>
        public Type ItemsType { set; get; }

        public List<string> ColumnNames { get; set; }

        // Коллекция всех элементов
        public ObservableCollection<object> ItemsAll { get; set; } = new ObservableCollection<object>();
        #endregion

        #region Методы
        // Получение элементов указанной страницы из коллекции ItemsAll.
        // Нумерация страниц начинается с 1.
        public IEnumerable GetPage(int pageNumber, int itemsPerPage)
        {
            return this.ItemsAll
                .Skip((pageNumber - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToList();
        }

        public int GetRowsCount()
        {
            return this.ItemsAll.Count;
        }

        // Проверяет, существует ли указанная страница
        public bool HasPage(int pageNumber, int itemsPerPage)
        {
            return this.ItemsAll.Skip((pageNumber - 1) * itemsPerPage).Any();
        }

        // Метод фильтрации элементов по определенному условию. Вы можете его изменить под ваши нужды.
        public void ApplyFilter(Predicate<object> filterCriteria)
        {
            var filtered = new ObservableCollection<object>();

            foreach (var item in this.ItemsAll)
            {
                if (filterCriteria(item))
                {
                    filtered.Add(item);
                }
            }

            this.ItemsAll = filtered;
        }

        // Если вы хотите вернуть все элементы, например, при сбросе фильтра
        public void ResetFilter()
        {
            // В данной реализации фильтрация напрямую меняет ItemsAll.
            // Если вам нужно восстановить первоначальное состояние, вам придется хранить копию исходной коллекции.
        }

        public void AddRange(IEnumerable items)
        {
            if (this.ItemsAll == null)
                this.ItemsAll = new ObservableCollection<object>();
            foreach (var item in items)
            {
                this.ItemsAll.Add(item);
            }
            // Если нужно сразу сбросить фильтр после добавления элементов:
            this.ResetFilter();
        }

        /// <summary>
        /// Заменяет элемент в коллекции <see cref="ItemsAll"/> на основе идентификатора id.
        /// </summary>
        /// <param name="newItem">Новый элемент, который следует вставить вместо существующего.</param>
        /// <exception cref="ArgumentException">Вызывается, если <paramref name="newItem"/> не содержит свойства id или если элемент с таким id не найден в коллекции.</exception>
        public void ReplaceItemById(object newItem)
        {
            // Проверяем, имеет ли объект свойство id
            var idProperty = newItem.GetType().GetProperty("id") ?? throw new ArgumentException("Объект не содержит свойства id.");

            // Получаем значение id для нового элемента
            var newId = idProperty.GetValue(newItem) ?? throw new ArgumentException("Значение id не может быть null.");

            var itemToReplace = this.ItemsAll.FirstOrDefault(item =>
                item.GetType().GetProperty("id")?.GetValue(item) == newId) ?? throw new ArgumentException($"Элемент с id = {newId} не найден.");

            int index = this.ItemsAll.IndexOf(itemToReplace);
            this.ItemsAll[index] = newItem;
        }
        /// <summary>
        /// Очищает коллекцию
        /// </summary>
        public void Clear()
        {
            this.ItemsAll.Clear();
        }
        #endregion

        #region Конструкторы/Деструкторы
        // Конструктор
        public TableData(String title, String titleDisplay, Type itemsType, List<string> columnNames, ObservableCollection<object> items)
        {
            this.title = title;
            this.titleDisplay = titleDisplay;
            this.ItemsType = itemsType;
            this.ColumnNames = columnNames;
            this.ItemsAll = items;
        }

        public TableData(String title, String titleDisplay, Type itemsType, List<string> columnNames) :
            this(title, titleDisplay, itemsType, columnNames, null)
        {

        }

        public TableData(TableData other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            this.title = other.title;
            this.titleDisplay = other.titleDisplay;
            this.ItemsType = other.ItemsType;

            // Копирование списка имен колонок
            this.ColumnNames = new List<string>(other.ColumnNames);

            // Копирование элементов. Здесь мы предполагаем, что items - это простой объект и нам не нужна глубокая копия.
            // Если объекты в списке items сложные, то возможно вам потребуется выполнить глубокое копирование.
            if (other.ItemsAll != null)
            {
                this.ItemsAll = new ObservableCollection<object>(other.ItemsAll);
            }
            else
            {
                this.ItemsAll = null;
            }
        }

        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий

        #endregion

    }
}
