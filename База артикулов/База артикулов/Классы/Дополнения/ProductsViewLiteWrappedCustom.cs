using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace База_артикулов.Классы.Дополнения
{
    public class ProductsViewLiteWrappedCustom
    {
        public int ID_продукта { get; set; } // Соответствует [ID продукта]
        public string Артикул { get; set; } // Соответствует [Артикул]
        public string Наименование_продукта { get; set; } // Соответствует [Наименование продукта]
        public int ID_подгруппы { get; set; } // Соответствует [ID подгруппы]
        public string Наименование_подгруппы { get; set; } // Соответствует [Наименование подгруппы]
        public int ID_группы { get; set; } // Соответствует [ID группы]
        public string Наименование_группы { get; set; } // Соответствует [Наименование группы]
        public int ID_класса { get; set; } // Соответствует [ID класса]
        public string Наименование_класса { get; set; } // Соответствует [Наименование класса]
        public double Вес { get; set; } // Соответствует [Вес (кг)]
        public string Наименование_упаковки { get; set; } // Соответствует [Наименование упаковки]
        public double Вес_упаковки_с_товаром { get; set; } // Соответствует [Вес упаковки с товаром (кг)]
    }

}
