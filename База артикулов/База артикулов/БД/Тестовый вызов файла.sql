USE [DBSE]

--#region ��������� Sample_Procedure 
--#region ��������
GO
DROP PROCEDURE IF EXISTS dbo.ExecuteScriptFromFile;
--#endregion
--#region ��������
GO
CREATE PROCEDURE dbo.ExecuteScriptFromFile
    @ScriptPath NVARCHAR(500)
AS
BEGIN
    DECLARE @Cmd NVARCHAR(1000)

    -- ���������� ������� ��� ���������� �������
    SET @Cmd = 'sqlcmd -S [LAPTOP-BBFM8MMD\SQLEXPRESS] -d [DBSE] -E -i "' + @ScriptPath + '"'

    -- ��������� ������� � ������� xp_cmdshell
    EXEC xp_cmdshell @Cmd
END
--#endregion
--#endregion



EXEC sp_configure 'show advanced options', 1
RECONFIGURE
EXEC sp_configure 'xp_cmdshell', 1
RECONFIGURE
EXEC dbo.ExecuteScriptFromFile 'C:\Users\FossW\OneDrive\�������������\����������������\������\���� ������\�������� ��������\���� ������\��.�������.sql'
EXEC dbo.ExecuteScriptFromFile 'C:\Users\FossW\OneDrive\�������������\����������������\������\���� ������\�������� ��������\���� ������\�������.����������.sql'