using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace База_артикулов.Классы.Дополнения
{
    public class ProductsViewLiteWrappedCustom
    {
        public int ProductId { get; set; } // Соответствует [ID продукта]
        public string Article { get; set; } // Соответствует [Артикул]
        public string ProductName { get; set; } // Соответствует [Наименование продукта]
        public int SubGroupId { get; set; } // Соответствует [ID подгруппы]
        public string SubGroupName { get; set; } // Соответствует [Наименование подгруппы]
        public int GroupId { get; set; } // Соответствует [ID группы]
        public string GroupName { get; set; } // Соответствует [Наименование группы]
        public int ClassId { get; set; } // Соответствует [ID класса]
        public string ClassName { get; set; } // Соответствует [Наименование класса]
        public double Weight { get; set; } // Соответствует [Вес (кг)]
        public string PackagingName { get; set; } // Соответствует [Наименование упаковки]
        public double PackagingWeight { get; set; } // Соответствует [Вес упаковки с товаром (кг)]
    }
}
