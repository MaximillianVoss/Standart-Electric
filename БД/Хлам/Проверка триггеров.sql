use DBSE

INSERT INTO dbo.Products (idDescriptor, idNorm, idSubGroup, idCover, idMaterial, idPerforation, idPackage, isInStock)
VALUES (35341, 2, 3, 4, 5, 6, 4, 1)



select p.*, d.* FROM dbo.Products as p
INNER JOIN Descriptors as d ON
p.idDescriptor = d.id
where p.idDescriptor=35341
