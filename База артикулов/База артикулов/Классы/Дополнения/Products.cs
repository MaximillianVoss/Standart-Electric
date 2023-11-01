using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;


namespace База_артикулов.Модели
{
    public partial class Products
    {
        public Products(int idDescriptor, int idNorm, int idSubGroup, int idCover, int idMaterial, int idPerforation, int idPackage, bool isInStock)
        {
            this.idDescriptor = idDescriptor;
            this.idNorm = idNorm;
            this.idSubGroup = idSubGroup;
            this.idCover = idCover;
            this.idMaterial = idMaterial;
            this.idPerforation = idPerforation;
            this.idPackage = idPackage;
            this.isInStock = isInStock;
        }
    }
}
