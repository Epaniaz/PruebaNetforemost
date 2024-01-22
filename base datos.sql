CREATE DATABASE PRUEBA;
GO

USE PRUEBA;
GO

CREATE TABLE ExpedienteUsuario
(
	Id UNIQUEIDENTIFIER DEFAULT NEWSEQUENTIALID(),
	Identificativo VARCHAR(30),
	Nombres NVARCHAR(255),
	Apellidos NVARCHAR(255),
	Correo VARCHAR(100),
	Telefono VARCHAR(100),
	Direccion NVARCHAR(500),
	Activo BIT DEFAULT 1,
	PRIMARY KEY(Id)
)
GO

CREATE TABLE Usuario
(
	Id UNIQUEIDENTIFIER DEFAULT NEWSEQUENTIALID(),
	ExpedienteId UNIQUEIDENTIFIER NOT NULL,
	[Login] NVARCHAR(30) NOT NULL,
	Contrasena NVARCHAR(200) NOT NULL,
	Avatar NVARCHAR(100),
	FechaBaja DATETIME NULL,
	FechaRegistro DATETIME DEFAULT GETDATE(),
	Activo BIT DEFAULT 1,
	PRIMARY KEY(Id),
	
	CONSTRAINT expediente_usuario_fkey FOREIGN KEY("ExpedienteId") 
	REFERENCES ExpedienteUsuario("Id")
)
GO

CREATE TABLE UsuarioLogin
(
	Id UNIQUEIDENTIFIER DEFAULT NEWSEQUENTIALID(),
	UsuarioId UNIQUEIDENTIFIER NOT NULL,
	Token NVARCHAR(MAX) NOT NULL,
	FechaExpiracion DATETIME,
	FechaRegistro DATETIME DEFAULT GETDATE(),
	PRIMARY KEY(Id),
	
	CONSTRAINT usuario_usuariologin_fkey FOREIGN KEY("UsuarioId") 
	REFERENCES UsuarioLogin("Id")
)
GO

INSERT INTO ExpedienteUsuario(Identificativo, Nombres, Apellidos, Correo, Telefono, Direccion)
VALUES('987654321', 'Pedro Jose', 'Flores', 'pedroflores@mail.com', '87796633', 'Managua, Nicaragua')

INSERT INTO Usuario(ExpedienteId, [Login], Contrasena, Avatar)
SELECT TOP(1) eu.Id, 'pflores', 'D23E57E0921626A6C6EB36B46FF33A16A566EB80DE72CD9E25C792F2A4F6D475:5F9DFCFB921B0D823EE663116CACCCBA:50000:SHA256', 'https://api.api-ninjas.com/v1/randomimage?category=nature' FROM ExpedienteUsuario AS eu

select * from Usuario u
inner join ExpedienteUsuario ue on ue.Id = u.expedienteId
where login='pflores'