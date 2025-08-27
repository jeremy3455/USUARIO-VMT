------CRUD ROLES-------
-- Insertar un nuevo rol
CREATE PROCEDURE sp_InsertRol
    @RolName VARCHAR(50)
AS
BEGIN
    INSERT INTO Roles (RolName)
    VALUES (@RolName);
END;
GO

-- Actualizar un rol
CREATE PROCEDURE sp_UpdateRol
    @RolId INT,
    @RolName VARCHAR(50)
AS
BEGIN
    UPDATE Roles
    SET RolName = @RolName
    WHERE RolId = @RolId;
END;
GO

-- Eliminar un rol
CREATE PROCEDURE sp_DeleteRol
    @RolId INT
AS
BEGIN
    DELETE FROM Roles
    WHERE RolId = @RolId;
END;
GO

-- Listar roles
CREATE PROCEDURE sp_GetRoles
AS
BEGIN
    SELECT * FROM Roles;
END;
GO

-----------------------------------------------------------------
--------------------CRUD PERSONAS--------------------------------
-- Insertar persona
CREATE PROCEDURE sp_InsertPersona
    @Nombres VARCHAR(100),
    @Apellidos VARCHAR(100),
    @Identificacion VARCHAR(10),
    @FechaNacimiento DATE
AS
BEGIN
    INSERT INTO Personas (Nombres, Apellidos, Identificacion, FechaNacimiento)
    VALUES (@Nombres, @Apellidos, @Identificacion, @FechaNacimiento);
END;
GO

-- Actualizar persona
CREATE PROCEDURE sp_UpdatePersona
    @PersonaId INT,
    @Nombres VARCHAR(100),
    @Apellidos VARCHAR(100),
    @Identificacion VARCHAR(10),
    @FechaNacimiento DATE
AS
BEGIN
    UPDATE Personas
    SET Nombres = @Nombres,
        Apellidos = @Apellidos,
        Identificacion = @Identificacion,
        FechaNacimiento = @FechaNacimiento
    WHERE PersonaId = @PersonaId;
END;
GO

-- Eliminar persona
CREATE PROCEDURE sp_DeletePersona
    @PersonaId INT
AS
BEGIN
    DELETE FROM Personas
    WHERE PersonaId = @PersonaId;
END;
GO

-- Listar personas
CREATE PROCEDURE sp_GetPersonas
AS
BEGIN
    SELECT * FROM Personas;
END;
GO

----------------------------------------------------------------
---------------------REGISTRO SESSION -------------------------
-- Registrar inicio de sesión
CREATE PROCEDURE sp_LoginUsuario
    @UserNmame VARCHAR(20),
    @Password VARCHAR(200)
AS
BEGIN
    DECLARE @UsuarioId INT;
    DECLARE @Status VARCHAR(20);

    SELECT @UsuarioId = UsuarioId, @Status = Status
    FROM Usuarios
    WHERE UserNmame = @UserNmame AND Password = @Password;

    IF @UsuarioId IS NOT NULL AND @Status = 'ACTIVO'
    BEGIN
        -- Insertar sesión
        INSERT INTO Sesiones (UsuarioId)
        VALUES (@UsuarioId);

        -- Resetear intentos fallidos
        UPDATE Usuarios
        SET IntentosFallidos = 0
        WHERE UsuarioId = @UsuarioId;

        SELECT 'LOGIN EXITOSO' AS Mensaje, @UsuarioId AS UsuarioId;
    END
    ELSE
    BEGIN
        -- Incrementar intentos fallidos
        UPDATE Usuarios
        SET IntentosFallidos = IntentosFallidos + 1
        WHERE UserNmame = @UserNmame;

        SELECT 'LOGIN FALLIDO' AS Mensaje;
    END
END;
GO


--------------------------------------------------------
--------------ROL USUARIO-------------------------------
-- Asignar rol a usuario
CREATE PROCEDURE sp_AssignRolUsuario
    @RolId INT,
    @UsuarioId INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Rol_Usuarios WHERE RolId = @RolId AND UsuarioId = @UsuarioId)
    BEGIN
        INSERT INTO Rol_Usuarios (RolId, UsuarioId)
        VALUES (@RolId, @UsuarioId);
        SELECT 'ROL ASIGNADO' AS Mensaje;
    END
    ELSE
    BEGIN
        SELECT 'ROL YA ASIGNADO' AS Mensaje;
    END
END;
GO

------------------------------------------------------
--------------CRUD USUARIOS--------------------------
-- Insertar usuario
CREATE PROCEDURE sp_InsertUsuario
    @PersonaId INT,
    @UserNmame VARCHAR(20),
    @Mail VARCHAR(100),
    @Password VARCHAR(200)
AS
BEGIN
    INSERT INTO Usuarios (PersonaId, UserNmame, Mail, Password, Status, IntentosFallidos)
    VALUES (@PersonaId, @UserNmame, @Mail, @Password, 'ACTIVO', 0);
END;
GO

-- Actualizar usuario
CREATE PROCEDURE sp_UpdateUsuario
    @UsuarioId INT,
    @PersonaId INT,
    @UserNmame VARCHAR(20),
    @Mail VARCHAR(100),
    @Password VARCHAR(200),
    @Status VARCHAR(20)
AS
BEGIN
    UPDATE Usuarios
    SET PersonaId = @PersonaId,
        UserNmame = @UserNmame,
        Mail = @Mail,
        Password = @Password,
        Status = @Status
    WHERE UsuarioId = @UsuarioId;
END;
GO

-- Eliminar usuario
CREATE PROCEDURE sp_DeleteUsuario
    @UsuarioId INT
AS
BEGIN
    DELETE FROM Usuarios
    WHERE UsuarioId = @UsuarioId;
END;
GO

-- Listar usuarios con datos de persona
CREATE PROCEDURE sp_GetUsuarios
AS
BEGIN
    SELECT u.UsuarioId, u.UserNmame, u.Mail, u.Status, u.IntentosFallidos,
           p.Nombres, p.Apellidos, p.Identificacion, p.FechaNacimiento
    FROM Usuarios u
    INNER JOIN Personas p ON u.PersonaId = p.PersonaId;
END;
GO

------------------------------------------------------------------
---------------------CRUD ROLOPCIONES-----------------------------
-- Insertar opción de rol
CREATE PROCEDURE sp_InsertRolOpcion
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(200)
AS
BEGIN
    INSERT INTO RolOpciones (Nombre, Descripcion)
    VALUES (@Nombre, @Descripcion);
END;
GO

-- Actualizar opción de rol
CREATE PROCEDURE sp_UpdateRolOpcion
    @RolOpcionId INT,
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(200)
AS
BEGIN
    UPDATE RolOpciones
    SET Nombre = @Nombre,
        Descripcion = @Descripcion
    WHERE RolOpcionId = @RolOpcionId;
END;
GO

-- Eliminar opción de rol
CREATE PROCEDURE sp_DeleteRolOpcion
    @RolOpcionId INT
AS
BEGIN
    DELETE FROM RolOpciones
    WHERE RolOpcionId = @RolOpcionId;
END;
GO

-- Listar opciones de rol
CREATE PROCEDURE sp_GetRolOpciones
AS
BEGIN
    SELECT * FROM RolOpciones;
END;
GO

---------------------------------------------
-- Asignar opción a un rol
CREATE PROCEDURE sp_AssignRolOpcion
    @RolId INT,
    @RolOpcionId INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Rol_RolOpciones WHERE RolId = @RolId AND RolOpcionId = @RolOpcionId)
    BEGIN
        INSERT INTO Rol_RolOpciones (RolId, RolOpcionId)
        VALUES (@RolId, @RolOpcionId);
        SELECT 'OPCIÓN ASIGNADA' AS Mensaje;
    END
    ELSE
    BEGIN
        SELECT 'OPCIÓN YA ASIGNADA' AS Mensaje;
    END
END;
GO

-- Quitar opción de un rol
CREATE PROCEDURE sp_RemoveRolOpcion
    @RolId INT,
    @RolOpcionId INT
AS
BEGIN
    DELETE FROM Rol_RolOpciones
    WHERE RolId = @RolId AND RolOpcionId = @RolOpcionId;
END;
GO

-- Listar opciones asignadas a un rol
CREATE PROCEDURE sp_GetRolOpcionesByRol
    @RolId INT
AS
BEGIN
    SELECT ro.RolOpcionId, ro.Nombre, ro.Descripcion
    FROM Rol_RolOpciones rro
    INNER JOIN RolOpciones ro ON rro.RolOpcionId = ro.RolOpcionId
    WHERE rro.RolId = @RolId;
END;
GO

