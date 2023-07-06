--#region Представление Sample_View
--#region Удаление
GO
DROP VIEW IF EXISTS dbo.[Sample_View]; 
--#endregion
--#region Создание
GO
CREATE VIEW dbo.[Sample_View]
AS
    (
    SELECT
        *
    FROM
        dbo.[Sample_Table]
);
--#endregion
--#endregion