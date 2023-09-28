--#region Создание БД 'DBSE'
--IF DB_ID('DBSE') IS NULL
--BEGIN
--    -- Recreate the database
--    CREATE DATABASE [DBSE]
--END
--#endregion
GO
USE [articles]
GO
--#region Очистка БД
:r "C:\Users\ivan.ivanov\source\repos\Standart-Electric\База артикулов\База артикулов\БД\БД.Очистка.sql"
--#endregion
GO
--#region Создание таблиц и связей
:r "C:\Users\ivan.ivanov\source\repos\Standart-Electric\База артикулов\База артикулов\БД\Таблицы.Создание.sql"
--#endregion
GO
--#region Создание хранимых процедур
:r "C:\Users\ivan.ivanov\source\repos\Standart-Electric\База артикулов\База артикулов\БД\Процедуры.Создание.sql"
--#endregion
GO
--#region Создание представлений
:r "C:\Users\ivan.ivanov\source\repos\Standart-Electric\База артикулов\База артикулов\БД\Представления.Создание.sql"
--#endregion
GO
--#region Создание триггеров
:r "C:\Users\ivan.ivanov\source\repos\Standart-Electric\База артикулов\База артикулов\БД\Триггеры.Создание.sql"
--#endregion
GO
--#region Заполнение таблиц
:r "C:\Users\ivan.ivanov\source\repos\Standart-Electric\База артикулов\База артикулов\БД\Таблицы.Заполнение.sql"
--#endregion
GO
--#region Заполнение БД
-- :r "C:\Users\ivan.ivanov\source\repos\Standart-Electric\База артикулов\База артикулов\БД\БД.Заполнение.sql"
--#endregion
