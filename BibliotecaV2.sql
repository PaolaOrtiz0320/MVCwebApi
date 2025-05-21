#############################################################
-- PRÁCTICA FINAL INTEGRADORA - BASE DE DATOS ITEMS
-- ALUMNOS: GABRIELA PAOLA ORTIZ VELAZQUEZ, MARIA FERNANDA MOEDANO ALCANTARA, ALAN SOTO CADENA
-- FECHA: 21 MAYO 2025
##########################################################
-- drop schema ITP_ITEM_EXCHANGE;
-- 1. CREACIÓN DE LA BASE DE DATOS
CREATE DATABASE IF NOT EXISTS ITP_ITEM_EXCHANGE;
USE ITP_ITEM_EXCHANGE;

-- 2. TABLA PRINCIPAL UNIFICADA (USUARIO)
-- Contiene datos personales y credenciales del usuario (alumno).
CREATE TABLE IF NOT EXISTS USUARIO (
USU_ID INT PRIMARY KEY AUTO_INCREMENT,                -- ID del usuario
USU_NOMBRE VARCHAR(50) NOT NULL,                      -- Nombre del usuario
USU_APELLIDO VARCHAR(50) NOT NULL,                   -- Apellido del usuario
USU_EMAIL VARCHAR(100) NOT NULL,                     -- Correo electrónico institucional
USU_MATRICULA VARCHAR(20) NOT NULL UNIQUE,           -- Matrícula institucional única
USU_USERNAME VARCHAR(20) NOT NULL UNIQUE,            -- Nombre de usuario para login
USU_PASSWORD VARCHAR(64) NOT NULL,                   -- Contraseña del usuario (hash SHA2)
RUTA_IMAGEN VARCHAR(200)                             -- Ruta de la imagen de perfil
);

-- 3. TABLAS DE APOYO
-- Catálogo de tipos de ítems
CREATE TABLE IF NOT EXISTS TIPO_ITEM (
TIPO_ID INT PRIMARY KEY AUTO_INCREMENT,
TIPO_NOMBRE VARCHAR(50) NOT NULL
);

-- Catálogo de estados de ítems (ej. disponible, prestado)
CREATE TABLE IF NOT EXISTS ESTADO_ITEM (
EST_ID INT PRIMARY KEY AUTO_INCREMENT,
EST_DESCRIPCION VARCHAR(20) NOT NULL
);

-- Catálogo de ubicaciones dentro del campus
CREATE TABLE IF NOT EXISTS UBICACION (
UBI_ID INT PRIMARY KEY AUTO_INCREMENT,
LUGAR VARCHAR(100) NOT NULL
);

-- Disponibilidad del ítem para entrega (hora, día, lugar)
CREATE TABLE IF NOT EXISTS DISPONIBILIDAD (
DISP_ID INT PRIMARY KEY AUTO_INCREMENT,
HORA_ENTREGA TIME NOT NULL,
DIA_ENTREGA VARCHAR(20) NOT NULL,
UBI_ID INT NOT NULL,
FOREIGN KEY (UBI_ID) REFERENCES UBICACION(UBI_ID)
);

-- Tabla de ítems que los usuarios publican
CREATE TABLE IF NOT EXISTS ITEM (
ITEM_ID INT PRIMARY KEY AUTO_INCREMENT,
ITEM_NOMBRE VARCHAR(100) NOT NULL,
ITEM_DESCRIPCION TEXT,
TIPO_ID INT NOT NULL,
EST_ID INT NOT NULL,
USU_ID INT NOT NULL,
RUTA_IMAGEN VARCHAR(200),
DISP_ID INT,
FOREIGN KEY (TIPO_ID) REFERENCES TIPO_ITEM(TIPO_ID),
FOREIGN KEY (EST_ID) REFERENCES ESTADO_ITEM(EST_ID),
FOREIGN KEY (USU_ID) REFERENCES USUARIO(USU_ID),
FOREIGN KEY (DISP_ID) REFERENCES DISPONIBILIDAD(DISP_ID)
);

-- 4. REGISTROS DE PRUEBA
-- Tipos de ítems
INSERT INTO TIPO_ITEM VALUES
(NULL, 'Libro'),
(NULL, 'Software'),
(NULL, 'Accesorio Electrónico'),
(NULL, 'Asesoría');

-- Estados
INSERT INTO ESTADO_ITEM VALUES
(NULL, 'Disponible'),
(NULL, 'Prestado');

-- Ubicaciones
INSERT INTO UBICACION VALUES
(NULL, 'Biblioteca'),
(NULL, 'Edificio A - Piso 1'),
(NULL, 'Laboratorio de Redes');

-- Disponibilidad
INSERT INTO DISPONIBILIDAD VALUES
(NULL, '10:00:00', 'Lunes', 1),
(NULL, '12:30:00', 'Miércoles', 2),
(NULL, '14:00:00', 'Viernes', 3);

-- Usuarios de prueba con contraseñas en SHA2
INSERT INTO USUARIO VALUES
(NULL, 'Gabriela Paola', 'Ortiz Velázquez', 'paola@itp.edu.mx', 'A21012345', 'portiz', 'itic2025', 'imagenes/fotos/1.jpg'),
(NULL, 'Maria Fernanda', 'Moedano Cruz', 'mafer@itp.edu.mx', 'A21012346', 'mafer', 'itic2025', 'imagenes/fotos/2.jpg'),
(NULL, 'David Fidel', 'Guzmán Soto', 'fidel@itp.edu.mx', 'A21012347', 'fidelcastro', 'itic2025', 'imagenes/fotos/3.jpg'),
(NULL, 'Alan', 'Soto Cadena', 'alan@itp.edu.mx', 'A21012348', 'alansoto', 'itic2025', 'imagenes/fotos/4.jpg'),
(NULL, 'Edgar', 'Hernández Mendoza', 'edgar@itp.edu.mx', 'A21012349', 'edgarmendz', 'itic2025', 'imagenes/fotos/5.jpg');


-- Ítems de prueba
INSERT INTO ITEM VALUES
(NULL, 'Libro de Base de Datos', 'Libro usado pero completo', 1, 1, 1, 'imagenes/items/libro.jpg', 1),
(NULL, 'USB 32GB', 'Funciona perfectamente', 3, 1, 2, 'imagenes/items/usb.jpg', 2),
(NULL, 'Instalador de Visual Studio', 'Listo en DVD', 2, 2, 3, 'imagenes/items/visual.jpg', 3),
(NULL, 'Curso de Python', 'Asesoría personalizada', 4, 1, 4, 'imagenes/items/curso.jpg', 1),
(NULL, 'Libro de Cálculo', 'Edición 5, Stewart', 1, 2, 5, 'imagenes/items/libro2.jpg', 2);

-- 5. VISTA PARA CONSULTA DE ÍTEMS DISPONIBLES
-- Une ítems disponibles con información de usuario y lugar
CREATE OR REPLACE VIEW vwItemsDisponibles AS
SELECT
    i.ITEM_ID,
    i.ITEM_NOMBRE,
    i.ITEM_DESCRIPCION,
    i.RUTA_IMAGEN,
    t.TIPO_NOMBRE,
    e.EST_DESCRIPCION,
    CONCAT(u.USU_NOMBRE, ' ', u.USU_APELLIDO) AS PROPIETARIO,
    d.HORA_ENTREGA,
    d.DIA_ENTREGA,
    ub.LUGAR
FROM ITEM i
JOIN TIPO_ITEM t ON i.TIPO_ID = t.TIPO_ID
JOIN ESTADO_ITEM e ON i.EST_ID = e.EST_ID
JOIN USUARIO u ON i.USU_ID = u.USU_ID
JOIN DISPONIBILIDAD d ON i.DISP_ID = d.DISP_ID
JOIN UBICACION ub ON d.UBI_ID = ub.UBI_ID;


-- 6. PROCEDIMIENTOS ALMACENADOS
DELIMITER $$

-- Login de usuario 
CREATE PROCEDURE spLogin (
IN p_username VARCHAR(20),
IN p_password VARCHAR(20)
)
BEGIN
IF EXISTS(
SELECT USU_USERNAME FROM USUARIO
WHERE USU_USERNAME = p_username
AND USU_PASSWORD = p_password
) THEN
-- Devuelve todos los datos del usuario
SELECT
USU_ID, USU_NOMBRE, USU_APELLIDO, USU_EMAIL,
USU_MATRICULA, USU_USERNAME, RUTA_IMAGEN
FROM USUARIO
WHERE USU_USERNAME = p_username AND USU_PASSWORD = p_password;
ELSE
-- Si no existe el usuario, se devuelve 'acceso denegado'
SELECT '0' AS acceso_denegado;
END IF;
END$$

-- Insertar nuevo usuario con contraseña hasheada
CREATE PROCEDURE spInsUsuario (
IN p_nombre       VARCHAR(50),
IN p_apellido     VARCHAR(50),
IN p_email        VARCHAR(100),
IN p_matricula    VARCHAR(20),
IN p_username     VARCHAR(20),
IN p_password     VARCHAR(20),
IN p_ruta_imagen  VARCHAR(200)
)
BEGIN
DECLARE EXIT HANDLER FOR SQLEXCEPTION
BEGIN
SELECT -1 AS resultado;
END;

INSERT INTO USUARIO (
USU_NOMBRE, USU_APELLIDO, USU_EMAIL, USU_MATRICULA,
USU_USERNAME, USU_PASSWORD, RUTA_IMAGEN
)
VALUES (
p_nombre, p_apellido, p_email, p_matricula,
p_username, p_password, p_ruta_imagen
);

SELECT 0 AS resultado;
END$$

-- Eliminar usuario por ID
CREATE PROCEDURE spDelUsuario (
IN p_usu_id INT
)
BEGIN
DELETE FROM USUARIO WHERE USU_ID = p_usu_id;
IF ROW_COUNT() > 0 THEN
SELECT 1 AS resultado;
ELSE
SELECT 0 AS resultado;
END IF;
END$$

-- Actualizar usuario con contraseña hasheada
CREATE PROCEDURE spUpdUsuario (
IN p_usu_id       INT,
IN p_nombre       VARCHAR(50),
IN p_apellido     VARCHAR(50),
IN p_email        VARCHAR(100),
IN p_matricula    VARCHAR(20),
IN p_username     VARCHAR(20),
IN p_password     VARCHAR(20),
IN p_ruta_imagen  VARCHAR(200)
)
BEGIN
UPDATE USUARIO
SET USU_NOMBRE = p_nombre,
USU_APELLIDO = p_apellido,
USU_EMAIL = p_email,
USU_MATRICULA = p_matricula,
USU_USERNAME = p_username,
USU_PASSWORD = p_password,
RUTA_IMAGEN = p_ruta_imagen
WHERE USU_ID = p_usu_id;

SELECT ROW_COUNT() AS filas_actualizadas;
END$$

DELIMITER ;


DELIMITER $$
CREATE PROCEDURE spInsItemUsuario (
  IN p_item_nombre VARCHAR(100),
  IN p_item_descripcion TEXT,
  IN p_tipo_id INT,
  IN p_est_id INT,
  IN p_usu_id INT,
  IN p_ruta_imagen VARCHAR(200),
  IN p_hora_entrega TIME,
  IN p_dia_entrega VARCHAR(20),
  IN p_ubi_id INT
)
BEGIN
  DECLARE v_disp_id INT;

  -- Insertar disponibilidad
  INSERT INTO DISPONIBILIDAD (HORA_ENTREGA, DIA_ENTREGA, UBI_ID)
  VALUES (p_hora_entrega, p_dia_entrega, p_ubi_id);

  -- Obtener el DISP_ID generado
  SET v_disp_id = LAST_INSERT_ID();

  -- Insertar el ítem con DISP_ID
  INSERT INTO ITEM (
    ITEM_NOMBRE,
    ITEM_DESCRIPCION,
    TIPO_ID,
    EST_ID,
    USU_ID,
    RUTA_IMAGEN,
    DISP_ID
  )
  VALUES (
    p_item_nombre,
    p_item_descripcion,
    p_tipo_id,
    p_est_id,
    p_usu_id,
    p_ruta_imagen,
    v_disp_id
  );
END$$

DELIMITER ;

CALL spInsItemUsuario(
  'Tablet Samsung Galaxy',
  'Pantalla 10", incluye cargador',
  3,               -- TIPO_ID (Accesorio Electrónico)
  1,               -- EST_ID (Disponible)
  2,               -- USU_ID (Maria Fernanda)
  'imagenes/items/6.jpg',
  '16:00:00',      -- HORA_ENTREGA
  'Martes',        -- DIA_ENTREGA
  2                -- UBI_ID (Edificio A - Piso 1)
);



-- Actualizar Item
DELIMITER $$

CREATE PROCEDURE spUpdItemUsuario (
  IN p_item_id INT,
  IN p_item_nombre VARCHAR(100),
  IN p_item_descripcion TEXT,
  IN p_tipo_id INT,
  IN p_est_id INT,
  IN p_ruta_imagen VARCHAR(200),
  IN p_hora_entrega TIME,
  IN p_dia_entrega VARCHAR(20),
  IN p_ubi_id INT
)
BEGIN
  DECLARE v_disp_id INT;

  -- Obtener el DISP_ID actual del item
  SELECT DISP_ID INTO v_disp_id
  FROM ITEM
  WHERE ITEM_ID = p_item_id;

  -- Actualizar la tabla DISPONIBILIDAD
  UPDATE DISPONIBILIDAD
  SET 
    HORA_ENTREGA = p_hora_entrega,
    DIA_ENTREGA = p_dia_entrega,
    UBI_ID = p_ubi_id
  WHERE DISP_ID = v_disp_id;

DELIMITER $$

CREATE PROCEDURE spUpdItemUsuario (
  IN p_item_id INT,
  IN p_item_nombre VARCHAR(100),
  IN p_item_descripcion TEXT,
  IN p_tipo_id INT,
  IN p_est_id INT,
  IN p_ruta_imagen VARCHAR(255),
  IN p_hora_entrega TIME,
  IN p_dia_entrega VARCHAR(20),
  IN p_ubi_id INT
)
BEGIN
  DECLARE v_disp_id INT;

  -- Obtener el DISP_ID actual del item
  SELECT DISP_ID INTO v_disp_id
  FROM ITEM
  WHERE ITEM_ID = p_item_id;

  -- Actualizar DISPONIBILIDAD
  UPDATE DISPONIBILIDAD
  SET 
    HORA_ENTREGA = p_hora_entrega,
    DIA_ENTREGA = p_dia_entrega,
    UBI_ID = p_ubi_id
  WHERE DISP_ID = v_disp_id;

  -- Actualizar ITEM con nuevo propietario y datos
  UPDATE ITEM
  SET
    ITEM_NOMBRE = p_item_nombre,
    ITEM_DESCRIPCION = p_item_descripcion,
    TIPO_ID = p_tipo_id,
    EST_ID = p_est_id,
    RUTA_IMAGEN = p_ruta_imagen
  WHERE ITEM_ID = p_item_id;

END$$

DELIMITER ;


CALL spUpdItemUsuario(
  5,                            -- ITEM_ID
  'Tablet Lenovo Actualizada',  -- ITEM_NOMBRE
  'Con teclado externo incluido', -- ITEM_DESCRIPCION
  3,                            -- TIPO_ID
  2,                            -- EST_ID
  'imagenes/items/6_new.jpg',   -- RUTA_IMAGEN
  '14:30:00',                   -- HORA_ENTREGA
  'Miércoles',                  -- DIA_ENTREGA
  3                              -- UBI_ID
);


DELIMITER $$

CREATE PROCEDURE spDelItemDesdeVista (
  IN p_item_id INT
)
BEGIN
  DECLARE v_disp_id INT;

  -- Verificar que el item exista y obtener el DISP_ID asociado
  SELECT DISP_ID INTO v_disp_id
  FROM ITEM
  WHERE ITEM_ID = p_item_id;

  -- Eliminar el ítem
  DELETE FROM ITEM
  WHERE ITEM_ID = p_item_id;

  -- Eliminar disponibilidad si ya no está en uso
  IF NOT EXISTS (
    SELECT 1 FROM ITEM WHERE DISP_ID = v_disp_id
  ) THEN
    DELETE FROM DISPONIBILIDAD
    WHERE DISP_ID = v_disp_id;
  END IF;
END$$

DELIMITER ;


