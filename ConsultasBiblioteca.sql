-- #############################################################
-- CONSULTAS QUE PUEDES HACER EN LA BASE DE DATOS
-- ALUMNOS: GABRIELA PAOLA ORTIZ VELAZQUEZ, MARIA FERNANDA MOEDANO ALCANTARA, ALAN SOTO CADENA

-- #############################################################
-- 1. CONSULTAR TODOS LOS USUARIOS
SELECT * FROM USUARIO;
SELECT * FROM ITEM;

-- 2. CONSULTAR UN USUARIO POR SU ID
SELECT * FROM USUARIO WHERE USU_ID = 1;

-- 3. CONSULTAR UN USUARIO POR SU NOMBRE DE USUARIO (USERNAME)
SELECT * FROM USUARIO WHERE USU_USERNAME = 'portiz';
CALL spInsUsuario('Llith', 'Grill Hdez', 'lili@gmail.com', '21200875', 'lali88', '12345', 'imagenes/imagen2.jpg');

-- 4. CONSULTAR USUARIOS POR NOMBRE, APELLIDO O NOMBRE DE USUARIO (FILTRO)
SELECT 
    USU_ID, 
    USU_USERNAME, 
    RUTA_IMAGEN, 
    CONCAT(USU_NOMBRE, ' ', USU_APELLIDO) AS PROPIETARIO
FROM USUARIO
WHERE USU_USERNAME LIKE '%pepito%'
   OR USU_NOMBRE LIKE '%pepito%'
   OR USU_APELLIDO LIKE '%pepito%';

-- 5. CONSULTAR TODOS LOS ÍTEMS DISPONIBLES (USANDO LA VISTA)
SELECT * FROM vwItemsDisponibles;
CALL spDelItemDesdeVista(6);


-- 6. CONSULTAR LOS ÍTEMS DISPONIBLES FILTRADOS POR TIPO O UBICACIÓN
-- Filtrar por tipo de ítem (Ej. Libro)
SELECT * FROM vwItemsDisponibles WHERE TIPO_NOMBRE = 'Libro';

-- Filtrar por lugar (Ej. Biblioteca)
SELECT * FROM vwItemsDisponibles WHERE LUGAR = 'Biblioteca';

-- 7. CONSULTAR TODOS LOS TIPOS DE ÍTEMS
SELECT * FROM TIPO_ITEM;

-- 8. CONSULTAR TODOS LOS ESTADOS DE LOS ÍTEMS
SELECT * FROM ESTADO_ITEM;

-- 9. CONSULTAR TODAS LAS UBICACIONES DEL CAMPUS
SELECT * FROM UBICACION;

-- 10. CONSULTAR TODOS LOS ÍTEMS DE UN USUARIO (USU_ID = 1)
SELECT * FROM ITEM WHERE USU_ID = 1;

CALL spLogin('portiz', 'itic2025');


-- 11. CONSULTAR TODOS LOS ÍTEMS PRESTADOS (ESTADO = 'Prestado')
SELECT * FROM vwItemsDisponibles WHERE EST_DESCRIPCION = 'Prestado';

-- PROCEDIMIENTOS ALMACENADOS (LLAMADAS)

-- 12. LOGIN DE USUARIO (POR NOMBRE DE USUARIO Y CONTRASEÑA)
CALL spLogin('portiz', 'itic2025');

-- 13. INSERTAR UN NUEVO USUARIO
CALL spInsUsuario('Juan', 'Pérez', 'juan@itp.edu.mx', 'A21012350', 'juanp', '123456', 'imagenes/items/6.jpg');

-- 14. ELIMINAR UN USUARIO POR ID (USU_ID = 6)
CALL spDelUsuario(6);

-- 15. ACTUALIZAR UN USUARIO POR ID (USU_ID = 1)
CALL spUpdUsuario(1, 'Gabriela Paola', 'Ortiz Velázquez', 'paola@itp.edu.mx', 'A21012345', 'portiz', 'itic2025', 'imagenes/items/1.jpg');

-- FIN DEL SCRIPT
CALL spUpdItemUsuario(
  4,                           -- ITEM_ID
  'Monitor',  -- ITEM_NOMBRE
  'Pantalla curva de 27 pulgadas, resolución 4K', -- ITEM_DESCRIPCION
  2,                            -- TIPO_ID
  1,                            -- EST_ID
  'imagenes/items/pad.jpg',   -- RUTA_IMAGEN
  '14:00:00',                   -- HORA_ENTREGA
  'Jueves',                  -- DIA_ENTREGA
  2                          -- UBI_ID
);
