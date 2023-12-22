using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace База_артикулов.Классы.Дополнения
{
    public class ResourcesViewProductsCustom
    {
        public int ID_ресурса
        {
            get; set;
        }
        public string URL_ресурса
        {
            get; set;
        }
        public string Наименование_ресурса
        {
            get; set;
        }
        public string Наименование_объекта
        {
            get; set;
        }
        public int ID_дескриптора_объекта
        {
            get; set;
        }
        public int ID_дескриптора_ресурса
        {
            get; set;
        }
        public int ID_типа_ресурса
        {
            get; set;
        }
        public string Наименование_типа_ресурса
        {
            get; set;
        }
        public string Расширение_ресурса
        {
            get; set;
        }
        public int ID_продукта
        {
            get; set;
        }
        public int ID_дескриптора_продукта
        {
            get; set;
        }
        public int ID_стандарта_продукта
        {
            get; set;
        }
        public int ID_подгруппы_продукта
        {
            get; set;
        }
        public int ID_покрытия_продукта
        {
            get; set;
        }
        public int ID_материала_продукта
        {
            get; set;
        }
        public int ID_перфорации_продукта
        {
            get; set;
        }
        public int ID_упаковки_продукта
        {
            get; set;
        }
        public bool На_складе_продукт
        {
            get; set;
        }
    }

}
