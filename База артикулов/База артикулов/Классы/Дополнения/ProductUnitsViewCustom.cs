using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace База_артикулов.Классы.Дополнения
{
    public class ProductUnitsViewCustom
    {
        public int ID_связи_продуктИзмерение
        {
            get; set;
        } // ID связи продукт-измерение
        public int ID_продукта
        {
            get; set;
        } // ID продукта
        public int ID_дескриптора_товара
        {
            get; set;
        } // ID дескриптора товара
        public int ID_единицы_измерения
        {
            get; set;
        } // ID единицы измерения
        public string Наименование_единицы_измерения
        {
            get; set;
        } // Наименование единицы измерения
        public string Сокращенное_наименование_единицы_измерения
        {
            get; set;
        } // Сокращенное наименование единицы измерения
        public int ID_типа_измерения
        {
            get; set;
        } // ID типа измерения
        public string Наименование_типа_единицы_измерения
        {
            get; set;
        } // Наименование типа единицы измерения
        public decimal Значение
        {
            get; set;
        } // Значение
    }
}
