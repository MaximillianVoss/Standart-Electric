--#region Создание БД 'DBSE'
IF DB_ID('DBSE') IS NULL
BEGIN
    -- Recreate the database
    CREATE DATABASE [DBSE]
END
--#endregion
GO
USE [DBSE]
GO
--#region Очистка БД
:r "C:\Users\FossW\OneDrive\Репетиторство\Программирование\Заказы\Иван Иванов\Стандарт электрик\БД\БД.Очистка.sql"
--#endregion
GO
--#region Создание таблиц и связей
:r "C:\Users\FossW\OneDrive\Репетиторство\Программирование\Заказы\Иван Иванов\Стандарт электрик\БД\Таблицы.Создание.sql"
--#endregion
GO
--#region Создание представлений
:r "C:\Users\FossW\OneDrive\Репетиторство\Программирование\Заказы\Иван Иванов\Стандарт электрик\БД\Представления.Создание.sql"
--#endregion
GO
--#region Создание триггеров
:r "C:\Users\FossW\OneDrive\Репетиторство\Программирование\Заказы\Иван Иванов\Стандарт электрик\БД\Триггеры.Создание.sql"
--#endregion
GO
--#region Создание хранимых процедур
:r "C:\Users\FossW\OneDrive\Репетиторство\Программирование\Заказы\Иван Иванов\Стандарт электрик\БД\Процедуры.Создание.sql"
--#endregion
GO
--#region Заполнение таблиц
:r "C:\Users\FossW\OneDrive\Репетиторство\Программирование\Заказы\Иван Иванов\Стандарт электрик\БД\Таблицы.Заполнение.sql"
--#endregion
GO
--#region Заполнение БД
:r "C:\Users\FossW\OneDrive\Репетиторство\Программирование\Заказы\Иван Иванов\Стандарт электрик\БД\БД.Заполнение.sql"
--#endregion
