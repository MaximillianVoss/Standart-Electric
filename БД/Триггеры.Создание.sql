--#region Триггер trg_Descriptors_Insert
--#region Создание
-- GO
-- CREATE TRIGGER [dbo].[trg_Descriptors_Insert]
-- ON [dbo].[Descriptors]
-- AFTER INSERT
-- AS
-- BEGIN
--     DECLARE @idResource TABLE (id INT)
--     -- Insert the new row into the Resources table
--     INSERT INTO Resources
--         (URL)
--     OUTPUT inserted.id INTO @idResource
--     SELECT 'Resources/images/' + code + '.png'
--     FROM inserted

--     -- Insert the new row into the DescriptorsResources table
--     INSERT INTO DescriptorsResources
--         (idDescriptor, idResource, idResourceType)
--     SELECT id, (SELECT TOP 1
--             id
--         FROM @idResource), (SELECT id
--         FROM ResourceTypes
--         WHERE title = 'Image')
--     FROM inserted
-- END

--#endregion
--#endregion

--#region Триггер НАЗВАНИЕ

--#region Создание
GO
CREATE TRIGGER trg_AddProductImagePath
ON dbo.Products
AFTER INSERT
AS
BEGIN
    PRINT 'trg_AddProductImagePath start'
    DECLARE @imagePath NVARCHAR(2048)
    DECLARE @vendorCode NVARCHAR(255)
    DECLARE @productId INT
    DECLARE @descriptorId INT
    DECLARE @resourceTypeId INT
    DECLARE @resourceId INT

    SELECT
        TOP 1
        @productId = id,
        @descriptorId = idDescriptor
    FROM
        INSERTED

    -- Get vendor code for the product
    SELECT
        TOP 1
        @vendorCode = dvc.title
    FROM
        VendorCodes vc
        INNER JOIN ProductsVendorCodes pvc ON pvc.idCode = vc.id
        INNER JOIN Descriptors AS dvc ON vc.idDescriptor = dvc.id
    WHERE pvc.idProduct = @productId

    IF @vendorCode IS NULL
    BEGIN
        PRINT 'Vendor code is NULL. Cannot create image path.'
        RETURN
    END

    -- Create image path using vendor code and add to Resources table
    SET @imagePath = 'Resources/images/' + @vendorCode + '.png'
    INSERT INTO Resources
        (URL)
    VALUES
        (@imagePath)

    -- Get ResourceTypes id for 'image'
    SELECT
        TOP 1
        @resourceTypeId = id
    FROM
        ResourceTypes
    WHERE title = 'image'

    -- Get Resource id for the added image
    SELECT
        TOP 1
        @resourceId = id
    FROM
        Resources
    WHERE URL = @imagePath

    -- Check if resourceId is not NULL before inserting into DescriptorsResources
    IF @resourceId IS NOT NULL
    BEGIN
        INSERT INTO DescriptorsResources
            (idDescriptor, idResource, idResourceType)
        VALUES
            (@descriptorId, @resourceId, @resourceTypeId)
    END
    ELSE
    BEGIN
        PRINT 'Resource ID is NULL. Cannot insert into DescriptorsResources.'
    END
END

--#endregion
--#endregion
