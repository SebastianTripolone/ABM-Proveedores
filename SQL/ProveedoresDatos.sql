Create Database ProveedorDatos

use ProveedorDatos

CREATE TABLE Proveedor (
    Id INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(MAX) NOT NULL,
    Email NVARCHAR(MAX) NOT NULL,
	Direccion NVARCHAR(MAX) NOT NULL,
	Telefono NVARCHAR(20),
    Imagen VARBINARY(MAX)
);