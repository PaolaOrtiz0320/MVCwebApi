// Proyecto: apiRESTCheckFinal (ITP Item Exchange Web API)
// Alumna: Gabriela Paola Ortiz Velázquez, Alan Soto Cadena, Maria Fernanda Moedano Alcantara
// Fecha: Mayo 2025

// ==== clsUsuario.cs ==== //

using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace apiRESTCheckFinal.Models
{
    public class clsUsuario //Clase para las variables a usar
    {//Variables Usuario
        public int UsuId { get; set; }
        public string UsuUsername { get; set; }
        public string UsuPassword { get; set; }
        public string UsuNombre { get; set; }
        public string UsuApellido { get; set; }
        public string UsuEmail { get; set; }
        public string UsuMatricula { get; set; }
        public string Imagen { get; set; }

        //Variables Items
        public int ItemId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int TipoId { get; set; }
        public int EstId { get; set; }
        public string Rutaimagen { get; set; }
        public string HoraEntrega { get; set; } // Formato: "HH:mm:ss"
        public string DiaEntrega { get; set; }  // Ej: "Lunes"
        public int UbiId { get; set; }



        private string cadConn = ConfigurationManager.ConnectionStrings["ITP_ITEM_EXCHANGE"].ConnectionString;
        public clsUsuario()
        {

        }

        /// <summary>
        ///CONSTRUCTORES PARA ALUMNO, ITEMS, Y ACCESO
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public clsUsuario(string username, string password)
        {
            UsuUsername = username;
            UsuPassword = password;
        }

        public clsUsuario(int ItemId, string Descripcion, int TipoId, int EstId, string Rutaimagen, string HoraEntrega, string DiaEntrega, int UbiId)
        {
            ItemId = ItemId;
            Descripcion = Descripcion;
            TipoId = TipoId;
            EstId = EstId;
            Rutaimagen = Rutaimagen;
            HoraEntrega = HoraEntrega;
            DiaEntrega = DiaEntrega;
            UbiId = UbiId;
        }


        public clsUsuario(int id, string username, string password, string nombre, string apellido, string email, string matricula, string imagen)
        {
            UsuId = id;
            UsuUsername = username;
            UsuPassword = password;
            UsuNombre = nombre;
            UsuApellido = apellido;
            UsuEmail = email;
            UsuMatricula = matricula;
            Imagen = imagen;
        }
        //Acceso
        public DataSet spLogin()
        {
            string sql = $"CALL spLogin('{this.UsuUsername}', '{this.UsuPassword}');";
            using (var cnn = new MySqlConnection(cadConn))
            {
                using (var da = new MySqlDataAdapter(sql, cnn))
                {
                    var ds = new DataSet();
                    da.Fill(ds, "spLogin");
                    return ds;
                }
            }
        }


        //iNSERTAR ALUMNO
        public DataSet spInsUsuario()
        {
            string sql = $"CALL spInsUsuario('{this.UsuNombre}', '{this.UsuApellido}', '{this.UsuEmail}', '{this.UsuMatricula}', '{this.UsuUsername}', '{this.UsuPassword}', '{this.Imagen}');";

            using (var cnn = new MySqlConnection(cadConn))
            {
                using (var da = new MySqlDataAdapter(sql, cnn))
                {
                    var ds = new DataSet();
                    da.Fill(ds, "spInsUsuario");
                    return ds;
                }
            }
        }


        // Consultar alumno por id 
        // Firma que recibe un int
        public DataSet ConsultarAlumnoPorId(int id)
        {
            string sql = @"
        SELECT USU_ID, USU_NOMBRE, USU_APELLIDO, USU_EMAIL,
               USU_MATRICULA, USU_USERNAME, USU_PASSWORD, RUTA_IMAGEN
        FROM USUARIO
        WHERE USU_ID = @id;
    ";

            using (var cnn = new MySqlConnection(cadConn))
            using (var da = new MySqlDataAdapter(sql, cnn))
            {
                da.SelectCommand.Parameters.AddWithValue("@id", id);
                var ds = new DataSet();
                da.Fill(ds, "USUARIO");
                return ds;
            }
        }

        //actualizar alumno
        public DataSet spUpdUsuario()
        {
            string sql = $"CALL spUpdUsuario({this.UsuId}, '{this.UsuNombre}', '{this.UsuApellido}', '{this.UsuEmail}', '{this.UsuMatricula}', '{this.UsuUsername}', '{this.UsuPassword}', '{this.Imagen}');";
            using (var cnn = new MySqlConnection(cadConn))
            {
                using (var da = new MySqlDataAdapter(sql, cnn))
                {
                    var ds = new DataSet();
                    da.Fill(ds, "spUpdUsuario");
                    return ds;
                }
            }
        }


        //Eliminar alumno
        public DataSet spDelUsuario()
        {
            string sql = $"CALL spDelUsuario({this.UsuId});";
            using (var cnn = new MySqlConnection(cadConn))
            {
                using (var da = new MySqlDataAdapter(sql, cnn))
                {
                    var ds = new DataSet();
                    da.Fill(ds, "spDelUsuario");
                    return ds;
                }
            }
        }



        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// //ITEMS
    public DataSet vwItemsDisponibles()//VER TODOS LOS ITEMS DISPONIBLES
    {
        const string sql = @"
            SELECT 
                ITEM_ID AS ID,
                ITEM_NOMBRE AS Nombre,
                ITEM_DESCRIPCION AS Descripción,
                RUTA_IMAGEN AS Imagen,
                TIPO_NOMBRE AS Tipo,
                EST_DESCRIPCION AS Estado,
                PROPIETARIO AS Alumno,
                HORA_ENTREGA AS HoraEntrega,
                DIA_ENTREGA AS DíaEntrega,
                LUGAR AS Lugar
            FROM vwItemsDisponibles;
        ";

        using (var cnn = new MySqlConnection(cadConn))
        using (var da = new MySqlDataAdapter(sql, cnn))
        {
            var ds = new DataSet();
            da.Fill(ds, "vwItemsDisponibles");
            return ds;
        }
    }


        public DataSet vwItemsDisponibles(string filtro)//VER ITEMS DISPONIBLES A TRAVES DE FILTRO YA SEA NOMBRE, ID, Y PROPIETARIO DEL ITEM
        {
            string sql = @"
        SELECT 
            ITEM_ID,
            ITEM_NOMBRE,
            ITEM_DESCRIPCION,
            RUTA_IMAGEN,
            TIPO_NOMBRE,
            EST_DESCRIPCION,
            PROPIETARIO,
            HORA_ENTREGA,
            DIA_ENTREGA,
            LUGAR 
        FROM vwItemsDisponibles
        WHERE 
            ITEM_NOMBRE LIKE @filtro 
            OR PROPIETARIO LIKE @filtro 
            OR ITEM_ID = @filtroId;
    ";

            using (var cnn = new MySqlConnection(cadConn))
            using (var da = new MySqlDataAdapter(sql, cnn))
            {
                da.SelectCommand.Parameters.AddWithValue("@filtro", $"%{filtro}%");

                if (int.TryParse(filtro, out int id))
                    da.SelectCommand.Parameters.AddWithValue("@filtroId", id);
                else
                    da.SelectCommand.Parameters.AddWithValue("@filtroId", -1);

                var ds = new DataSet();
                da.Fill(ds, "vwItemsDisponibles");
                return ds;
            }
        }


        //INSERTAR ITEM
        public DataSet spInsertarItem()
        {

            string sql = $"CALL spInsItemUsuario('{this.Nombre}', '{this.Descripcion}', {this.TipoId}, {this.EstId}, {this.UsuId}, '{this.Rutaimagen}', '{this.HoraEntrega}', '{this.DiaEntrega}', {this.UbiId});";

            using (var cnn = new MySqlConnection(cadConn))
            using (var da = new MySqlDataAdapter(sql, cnn))
            {
                var ds = new DataSet();
                da.Fill(ds, "spInsItemUsuario");
                return ds;
            }
        }

        //ACTUALIZAR ITEMS
        public DataSet spUpdItemUsuario()
        {
            string sql = $"CALL spUpdItemUsuario({this.ItemId}, '{this.Nombre}', '{this.Descripcion}', {this.TipoId}, {this.EstId}, '{this.Rutaimagen}', '{this.HoraEntrega}', '{this.DiaEntrega}', {this.UbiId});";

            using (var cnn = new MySqlConnection(cadConn))
            using (var da = new MySqlDataAdapter(sql, cnn))
            {
                var ds = new DataSet();
                da.Fill(ds, "spUpdItemUsuario");
                return ds;
            }
        }

        // ELIMINAR ITEMS
        public DataSet spDelItem()
        {
            string sql = $"CALL spDelItemDesdeVista({this.ItemId});";

            using (var cnn = new MySqlConnection(cadConn))
            using (var da = new MySqlDataAdapter(sql, cnn))
            {
                var ds = new DataSet();
                da.Fill(ds, "spDelItemDesdeVista");
                return ds;
            }
        }


//CONSULTAS PARA OBTENER LOS DATOS DE LOS DROWNLIST COMO ESTADO, TIPOS, UBICACION 

        public DataSet obtenerTipos()
        {
            const string sql = @"SELECT * FROM TIPO_ITEM;";
            using (var cnn = new MySqlConnection(cadConn))
            using (var da = new MySqlDataAdapter(sql, cnn))
            {
                var ds = new DataSet();
                da.Fill(ds, "TIPO_ITEM");
                return ds;
            }
        }

        public DataSet obtenerEstados()
        {
            const string sql = @"SELECT * FROM ESTADO_ITEM;";
            using (var cnn = new MySqlConnection(cadConn))
            using (var da = new MySqlDataAdapter(sql, cnn))
            {
                var ds = new DataSet();
                da.Fill(ds, "ESTADO_ITEM");
                return ds;
            }
        }

        public DataSet obtenerUbicaciones()
        {
            const string sql = @"SELECT * FROM UBICACION;";
            using (var cnn = new MySqlConnection(cadConn))
            using (var da = new MySqlDataAdapter(sql, cnn))
            {
                var ds = new DataSet();
                da.Fill(ds, "UBICACION");
                return ds;
            }
        }



    }
}
