﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace База_артикулов.Модели
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class DBSEEntities : DbContext
    {
        public DBSEEntities()
            : base("name=DBSEEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Applications> Applications { get; set; }
        public virtual DbSet<BuisnessUnits> BuisnessUnits { get; set; }
        public virtual DbSet<Classes> Classes { get; set; }
        public virtual DbSet<Covers> Covers { get; set; }
        public virtual DbSet<Descriptors> Descriptors { get; set; }
        public virtual DbSet<DescriptorsResources> DescriptorsResources { get; set; }
        public virtual DbSet<Fields> Fields { get; set; }
        public virtual DbSet<Groups> Groups { get; set; }
        public virtual DbSet<GroupsApplications> GroupsApplications { get; set; }
        public virtual DbSet<LoadDiagrams> LoadDiagrams { get; set; }
        public virtual DbSet<Manufacturers> Manufacturers { get; set; }
        public virtual DbSet<Materials> Materials { get; set; }
        public virtual DbSet<Norms> Norms { get; set; }
        public virtual DbSet<Packages> Packages { get; set; }
        public virtual DbSet<Perforations> Perforations { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<ProductsAnalogs> ProductsAnalogs { get; set; }
        public virtual DbSet<ProductsVendorCodes> ProductsVendorCodes { get; set; }
        public virtual DbSet<Resources> Resources { get; set; }
        public virtual DbSet<ResourceTypes> ResourceTypes { get; set; }
        public virtual DbSet<SubGroups> SubGroups { get; set; }
        public virtual DbSet<Tables> Tables { get; set; }
        public virtual DbSet<Units> Units { get; set; }
        public virtual DbSet<UnitsPackages> UnitsPackages { get; set; }
        public virtual DbSet<UnitsPerforations> UnitsPerforations { get; set; }
        public virtual DbSet<UnitsProducts> UnitsProducts { get; set; }
        public virtual DbSet<UnitsTypes> UnitsTypes { get; set; }
        public virtual DbSet<VendorCodes> VendorCodes { get; set; }
        public virtual DbSet<Views> Views { get; set; }
        public virtual DbSet<ViewsTables> ViewsTables { get; set; }
        public virtual DbSet<ViewTypes> ViewTypes { get; set; }
        public virtual DbSet<ApplicationsView> ApplicationsView { get; set; }
        public virtual DbSet<BuisnessUnitsView> BuisnessUnitsView { get; set; }
        public virtual DbSet<ClassesView> ClassesView { get; set; }
        public virtual DbSet<CoversView> CoversView { get; set; }
        public virtual DbSet<DescriptorsResourcesView> DescriptorsResourcesView { get; set; }
        public virtual DbSet<DescriptorsView> DescriptorsView { get; set; }
        public virtual DbSet<GroupsApplicationsView> GroupsApplicationsView { get; set; }
        public virtual DbSet<GroupsView> GroupsView { get; set; }
        public virtual DbSet<LoadDiagramsView> LoadDiagramsView { get; set; }
        public virtual DbSet<ManufacturersView> ManufacturersView { get; set; }
        public virtual DbSet<MaterialsView> MaterialsView { get; set; }
        public virtual DbSet<NormsView> NormsView { get; set; }
        public virtual DbSet<PackagesView> PackagesView { get; set; }
        public virtual DbSet<PerforationsView> PerforationsView { get; set; }
        public virtual DbSet<ProductsAnalogsView> ProductsAnalogsView { get; set; }
        public virtual DbSet<ProductsView> ProductsView { get; set; }
        public virtual DbSet<ProductsViewLite> ProductsViewLite { get; set; }
        public virtual DbSet<ProductsViewLiteWrapped> ProductsViewLiteWrapped { get; set; }
        public virtual DbSet<ProductUnitsView> ProductUnitsView { get; set; }
        public virtual DbSet<ResourcesView> ResourcesView { get; set; }
        public virtual DbSet<ResourcesViewProducts> ResourcesViewProducts { get; set; }
        public virtual DbSet<ResourceTypesView> ResourceTypesView { get; set; }
        public virtual DbSet<SubGroupsView> SubGroupsView { get; set; }
        public virtual DbSet<TablesView> TablesView { get; set; }
        public virtual DbSet<UnitsTypesView> UnitsTypesView { get; set; }
        public virtual DbSet<UnitsView> UnitsView { get; set; }
        public virtual DbSet<VendorCodesView> VendorCodesView { get; set; }
        public virtual DbSet<ViewsTablesView> ViewsTablesView { get; set; }
        public virtual DbSet<ViewsView> ViewsView { get; set; }
        public virtual DbSet<ViewTypesView> ViewTypesView { get; set; }
    
        public virtual int InsertProductImages()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertProductImages");
        }
    
        public virtual int AddProductImagePaths()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AddProductImagePaths");
        }
    
        public virtual int AddSubGroupImagePaths()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AddSubGroupImagePaths");
        }
    
        public virtual ObjectResult<GetFilteredProducts_Result> GetFilteredProducts(string group, string @class, string subGroup)
        {
            var groupParameter = group != null ?
                new ObjectParameter("Group", group) :
                new ObjectParameter("Group", typeof(string));
    
            var classParameter = @class != null ?
                new ObjectParameter("Class", @class) :
                new ObjectParameter("Class", typeof(string));
    
            var subGroupParameter = subGroup != null ?
                new ObjectParameter("SubGroup", subGroup) :
                new ObjectParameter("SubGroup", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetFilteredProducts_Result>("GetFilteredProducts", groupParameter, classParameter, subGroupParameter);
        }
    
        public virtual ObjectResult<GetFilteredProductsLite_Result> GetFilteredProductsLite(string group, string @class, string subGroup)
        {
            var groupParameter = group != null ?
                new ObjectParameter("Group", group) :
                new ObjectParameter("Group", typeof(string));
    
            var classParameter = @class != null ?
                new ObjectParameter("Class", @class) :
                new ObjectParameter("Class", typeof(string));
    
            var subGroupParameter = subGroup != null ?
                new ObjectParameter("SubGroup", subGroup) :
                new ObjectParameter("SubGroup", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetFilteredProductsLite_Result>("GetFilteredProductsLite", groupParameter, classParameter, subGroupParameter);
        }
    
        public virtual ObjectResult<GetFilteredProducts1_Result> GetFilteredProducts1(string group, string @class, string subGroup)
        {
            var groupParameter = group != null ?
                new ObjectParameter("Group", group) :
                new ObjectParameter("Group", typeof(string));
    
            var classParameter = @class != null ?
                new ObjectParameter("Class", @class) :
                new ObjectParameter("Class", typeof(string));
    
            var subGroupParameter = subGroup != null ?
                new ObjectParameter("SubGroup", subGroup) :
                new ObjectParameter("SubGroup", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetFilteredProducts1_Result>("GetFilteredProducts1", groupParameter, classParameter, subGroupParameter);
        }
    
        public virtual ObjectResult<GetFilteredProductsLite1_Result> GetFilteredProductsLite1(string group, string @class, string subGroup)
        {
            var groupParameter = group != null ?
                new ObjectParameter("Group", group) :
                new ObjectParameter("Group", typeof(string));
    
            var classParameter = @class != null ?
                new ObjectParameter("Class", @class) :
                new ObjectParameter("Class", typeof(string));
    
            var subGroupParameter = subGroup != null ?
                new ObjectParameter("SubGroup", subGroup) :
                new ObjectParameter("SubGroup", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetFilteredProductsLite1_Result>("GetFilteredProductsLite1", groupParameter, classParameter, subGroupParameter);
        }
    }
}
