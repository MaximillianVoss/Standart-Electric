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
