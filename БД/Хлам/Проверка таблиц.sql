Select * from dbo.Products
Select * FROM dbo.DescriptorsResources
Select * FROM dbo.Resources
Select * FROM Descriptors

select p.*, d.* FROM dbo.Products as p
INNER JOIN Descriptors as d ON
p.idDescriptor = d.id
where p.id=1

SELECT        Products.*, Descriptors.*, Resources.*, DescriptorsResources.*
FROM            Products INNER JOIN
                         Descriptors ON Products.idDescriptor = Descriptors.id INNER JOIN
                         DescriptorsResources ON Descriptors.id = DescriptorsResources.idDescriptor INNER JOIN
                         Resources ON DescriptorsResources.idResource = Resources.id


SELECT        SubGroups.*, DescriptorsResources.*, ResourceTypes.*, Resources.*, Descriptors.*
FROM            SubGroups INNER JOIN
                         Descriptors ON SubGroups.idDescriptor = Descriptors.id INNER JOIN
                         DescriptorsResources ON Descriptors.id = DescriptorsResources.idDescriptor INNER JOIN
                         Resources ON DescriptorsResources.idResource = Resources.id INNER JOIN
                         ResourceTypes ON DescriptorsResources.idResourceType = ResourceTypes.id




SELECT 
    T.id AS TableID, 
    T.titleDisplay AS TableTitleDisplay,
    VT.id AS ViewTypeID, 
    VT.title AS ViewTypeTitle,
    VTS.id AS ViewsTypesID,
    VTS.titleDisplay AS ViewsTypesTitleDisplay,
    D.title AS DescriptorTitle,
    D.titleShort AS DescriptorTitleShort,
    D.description AS DescriptorDescription
FROM Tables T
JOIN ViewsTypes VTS ON T.id = VTS.idTable
JOIN ViewTypes VT ON VTS.idType = VT.id
JOIN Descriptors D ON VTS.idDescriptor = D.id

