--#region Хранимые процедуры

--#region Процедура AddProductImagePaths 
--#region Создание
GO
CREATE PROCEDURE [dbo].[AddProductImagePaths]
AS
BEGIN
    PRINT 'trg_AddProductImagePath start'
    DECLARE @imagePath NVARCHAR(2048)
    DECLARE @vendorCode NVARCHAR(255)

    -- Create ResourceType 'image' if it does not exist
    IF NOT EXISTS (SELECT
        *
    FROM
        ResourceTypes
    WHERE title = 'image')
    BEGIN
        INSERT INTO ResourceTypes
            (title)
        VALUES
            ('image')
    END

    -- Loop through each product and add image path to Resources table
    DECLARE @productId INT
    DECLARE product_cursor CURSOR FOR 
        SELECT
        id
    FROM
        Products
    OPEN product_cursor
    FETCH NEXT FROM product_cursor INTO @productId
    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Get vendor code for the product
        SELECT
            TOP 1
            @vendorCode = dvc.title
        FROM
            VendorCodes vc
            INNER JOIN ProductsVendorCodes pvc ON pvc.idCode = vc.id
            INNER JOIN Descriptors AS dvc ON vc.idDescriptor = dvc.id
        WHERE pvc.idProduct = @productId

        -- Create image path using vendor code and add to Resources table
        SET @imagePath = 'Resources/images/' + @vendorCode + '.png'
        INSERT INTO Resources
            (URL)
        VALUES
            (@imagePath)

        -- Get ResourceTypes id for 'image'
        DECLARE @resourceTypeId INT
        SELECT
            TOP 1
            @resourceTypeId = id
        FROM
            ResourceTypes
        WHERE title = 'image'

        -- Get Resource id for the added image
        DECLARE @resourceId INT
        SELECT
            TOP 1
            @resourceId = id
        FROM
            Resources
        WHERE URL = @imagePath

        -- Add entry to DescriptorsResources table
        DECLARE @descriptorId INT
        SELECT
            TOP 1
            @descriptorId = idDescriptor
        FROM
            Products
        WHERE id = @productId
        INSERT INTO DescriptorsResources
            (idDescriptor, idResource, idResourceType)
        VALUES
            (@descriptorId, @resourceId, @resourceTypeId)

        FETCH NEXT FROM product_cursor INTO @productId
    END
    CLOSE product_cursor
    DEALLOCATE product_cursor
END
--#endregion
--#endregion

--#region Процедура AddSubGroupImagePaths 
--#region Создание
GO
CREATE PROCEDURE [dbo].[AddSubGroupImagePaths]
AS
BEGIN
    DECLARE @imagePath NVARCHAR(2048)
    DECLARE @vendorCode NVARCHAR(255)

    -- Create ResourceType 'image' if it does not exist
    IF NOT EXISTS (SELECT
        *
    FROM
        ResourceTypes
    WHERE title = 'image')
    BEGIN
        INSERT INTO ResourceTypes
            (title)
        VALUES
            ('image')
    END

    -- Loop through each sub group and add image path to Resources table
    DECLARE @subGroupId INT
    DECLARE sub_group_cursor CURSOR FOR 
        SELECT
        id
    FROM
        SubGroups
    OPEN sub_group_cursor
    FETCH NEXT FROM sub_group_cursor INTO @subGroupId
    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Get vendor code for the sub group
        -- SELECT TOP 1
        --     @vendorCode = vc.code
        -- FROM VendorCodes vc
        --     INNER JOIN Classes c ON vc.idDescriptor = c.idDescriptor
        --     INNER JOIN Groups g ON g.idClass = c.id
        --     INNER JOIN SubGroups sg ON sg.idGroup = g.id
        -- WHERE sg.id = @subGroupId
        SELECT
            TOP 1
            @vendorCode = sgd.code
        FROM
            SubGroups AS sg
            INNER JOIN Descriptors AS sgd ON sgd.id = sg.idDescriptor
        WHERE sg.id = @subGroupId

        -- Create image path using vendor code and add to Resources table
        SET @imagePath = 'Resources/images/' + @vendorCode + '.png'
        INSERT INTO Resources
            (URL)
        VALUES
            (@imagePath)

        -- Get ResourceTypes id for 'image'
        DECLARE @resourceTypeId INT
        SELECT
            TOP 1
            @resourceTypeId = id
        FROM
            ResourceTypes
        WHERE title = 'image'

        -- Get Resource id for the added image
        DECLARE @resourceId INT
        SELECT
            TOP 1
            @resourceId = id
        FROM
            Resources
        WHERE URL = @imagePath

        -- Add entry to DescriptorsResources table
        DECLARE @descriptorId INT
        SELECT
            TOP 1
            @descriptorId = idDescriptor
        FROM
            SubGroups
        WHERE id = @subGroupId
        INSERT INTO DescriptorsResources
            (idDescriptor, idResource, idResourceType)
        VALUES
            (@descriptorId, @resourceId, @resourceTypeId)

        FETCH NEXT FROM sub_group_cursor INTO @subGroupId
    END
    CLOSE sub_group_cursor
    DEALLOCATE sub_group_cursor
END

--#endregion
--#endregion

--#endregion