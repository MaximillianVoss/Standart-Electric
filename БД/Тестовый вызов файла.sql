USE [DBSE]

--#region Процедура Sample_Procedure 
--#region Удаление
GO
DROP PROCEDURE IF EXISTS dbo.ExecuteScriptFromFile;
--#endregion
--#region Создание
GO
CREATE PROCEDURE dbo.ExecuteScriptFromFile
    @ScriptPath NVARCHAR(500)
AS
BEGIN
    DECLARE @Cmd NVARCHAR(1000)

    -- Составляем команду для выполнения скрипта
    SET @Cmd = 'sqlcmd -S [LAPTOP-BBFM8MMD\SQLEXPRESS] -d [DBSE] -E -i "' + @ScriptPath + '"'

    -- Выполняем команду с помощью xp_cmdshell
    EXEC xp_cmdshell @Cmd
END
--#endregion
--#endregion



EXEC sp_configure 'show advanced options', 1
RECONFIGURE
EXEC sp_configure 'xp_cmdshell', 1
RECONFIGURE
EXEC dbo.ExecuteScriptFromFile 'C:\Users\FossW\OneDrive\Репетиторство\Программирование\Заказы\Иван Иванов\Стандарт электрик\База данных\БД.Очистка.sql'
EXEC dbo.ExecuteScriptFromFile 'C:\Users\FossW\OneDrive\Репетиторство\Программирование\Заказы\Иван Иванов\Стандарт электрик\База данных\Таблицы.Заполнение.sql'