/* =========================================================
   Base de datos para la API de Productos (SQL Server)
   Ejecutar completo en SQL Server Management Studio o Azure Data Studio.
   ========================================================= */

IF DB_ID('ProductosDB') IS NULL
    CREATE DATABASE ProductosDB;
GO

USE ProductosDB;
GO

IF OBJECT_ID('dbo.Productos', 'U') IS NOT NULL
    DROP TABLE dbo.Productos;
GO

CREATE TABLE dbo.Productos
(
    Id             INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Productos PRIMARY KEY,
    Nombre         NVARCHAR(100)     NOT NULL,
    Descripcion    NVARCHAR(250)     NULL,
    Precio         DECIMAL(10,2)     NOT NULL CONSTRAINT CK_Productos_Precio CHECK (Precio >= 0),
    Stock          INT               NOT NULL CONSTRAINT DF_Productos_Stock DEFAULT (0)
                                              CONSTRAINT CK_Productos_Stock CHECK (Stock >= 0),
    Categoria      NVARCHAR(50)      NULL,
    FechaRegistro  DATETIME2         NOT NULL CONSTRAINT DF_Productos_Fecha DEFAULT (SYSDATETIME())
);
GO

-- Datos de prueba
INSERT INTO dbo.Productos (Nombre, Descripcion, Precio, Stock, Categoria) VALUES
(N'Mouse inalámbrico',   N'Mouse óptico 2.4 GHz con receptor USB', 249.90, 35, N'Electrónica'),
(N'Teclado mecánico',    N'Teclado switches rojos, retroiluminado', 899.00, 12, N'Electrónica'),
(N'Cuaderno profesional',N'Cuaderno cuadro chico 100 hojas',         65.50, 80, N'Papelería'),
(N'Memoria USB 64 GB',   N'USB 3.2 Gen 1',                          189.00, 40, N'Electrónica'),
(N'Plumas gel (paq. 5)', N'Tinta negra, punto fino 0.7 mm',          79.00, 60, N'Papelería');
GO

SELECT * FROM dbo.Productos;
GO
