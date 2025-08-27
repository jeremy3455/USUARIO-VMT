CREATE DATABASE PruebaNetAngular;
GO
USE PruebaNetAngular;
GO

-- =============================
-- TABLA PERSONAS
-- =============================
CREATE TABLE Personas (
    PersonaId INT IDENTITY(1,1) PRIMARY KEY,
    Nombres VARCHAR(100) NOT NULL,
    Apellidos VARCHAR(100) NOT NULL,
    Identificacion VARCHAR(10) UNIQUE NOT NULL,
    FechaNacimiento DATE NULL
);

-- =============================
-- TABLA USUARIOS
-- =============================
CREATE TABLE Usuarios (
    UsuarioId INT IDENTITY(1,1) PRIMARY KEY,
    PersonaId INT NOT NULL,
    UserNmame VARCHAR(20) UNIQUE NOT NULL,
    Mail VARCHAR(100) UNIQUE NOT NULL,
    Password VARCHAR(200) NOT NULL,
	SessionActive CHAR,
    Status VARCHAR(20) NOT NULL DEFAULT('ACTIVO'), -- ACTIVO / INACTIVO / BLOQUEADO
    IntentosFallidos INT DEFAULT 0,
    FOREIGN KEY (PersonaId) REFERENCES Personas(PersonaId)
);

-- =============================
-- TABLA ROLES
-- =============================
CREATE TABLE Roles (
    RolId INT IDENTITY(1,1) PRIMARY KEY,
   RolName VARCHAR(50) NOT NULL
);

-- =============================
-- TABLA RELACION ROL-USUARIOS
-- =============================
CREATE TABLE Rol_Usuarios (
    RolUsuarioId INT IDENTITY(1,1) PRIMARY KEY,
    RolId INT NOT NULL,
    UsuarioId INT NOT NULL,
    FOREIGN KEY (RolId) REFERENCES Roles(RolId),
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(UsuarioId)
);

-- =============================
-- TABLA OPCIONES DE ROL
-- =============================
CREATE TABLE RolOpciones (
    RolOpcionId INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(200) NULL
);

-- =============================
-- TABLA RELACION ROL-ROLOPCIONES
-- =============================
CREATE TABLE Rol_RolOpciones (
    RolRolOpcionId INT IDENTITY(1,1) PRIMARY KEY,
    RolId INT NOT NULL,
    RolOpcionId INT NOT NULL,
    FOREIGN KEY (RolId) REFERENCES Roles(RolId),
    FOREIGN KEY (RolOpcionId) REFERENCES RolOpciones(RolOpcionId)
);

-- =============================
-- TABLA SESIONES
-- =============================
CREATE TABLE Sesiones (
    SesionId INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioId INT NOT NULL,
    FechaInicio DATETIME NOT NULL DEFAULT(GETDATE()),
    FechaFin DATETIME NULL,
    Estado VARCHAR(20) NOT NULL DEFAULT('ACTIVA'), -- ACTIVA / INACTIVA
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(UsuarioId)
);


