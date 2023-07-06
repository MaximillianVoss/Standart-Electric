DECLARE @OutputFolder nvarchar(255) = 'C:\Users\ivan.ivanov\Desktop\SQL data'
DECLARE @Cmd nvarchar(max)

-- ������� ������ ������� � ��������� ��������� ����
DECLARE @TableName nvarchar(255)
DECLARE table_cursor CURSOR FOR 
SELECT name FROM sys.tables
OPEN table_cursor
FETCH NEXT FROM table_cursor INTO @TableName
WHILE @@FETCH_STATUS = 0
BEGIN
    SET @Cmd = 'bcp ' + DB_NAME() + '.dbo.' + @TableName + ' out ' + @OutputFolder + @TableName + '.txt -T -c'
    EXEC master.dbo.xp_cmdshell @Cmd
    FETCH NEXT FROM table_cursor INTO @TableName
END
CLOSE table_cursor
DEALLOCATE table_cursor

-- ��������� �� ��������� ��������
PRINT 'All tables exported successfully.'
