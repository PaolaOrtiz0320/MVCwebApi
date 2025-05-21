// ==== UsuarioController.cs ====
// Proyecto: apiRESTCheckFinal (ITP Item Exchange Web API - estilo clásico)
// Alumna: Gabriela Paola Ortiz Velázquez
// Fecha: Mayo 2025

using System;
using System.Data;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using apiRESTCheckFinal.Models;
using apiCheckFinal.Models;

namespace apiRESTCheckFinal.Controllers
{
    [RoutePrefix("api/usuario")]
    public class UsuarioController : ApiController
    {

        [HttpPost]
        [Route("login")]
        public clsApiStatus Login([FromBody] clsUsuario modelo)
        {
            var respuesta = new clsApiStatus();
            var json = new JObject();
            try
            {
                var ds = modelo.spLogin();
                respuesta.statusExec = true;
                respuesta.ban = ds.Tables[0].Rows.Count;

                if (respuesta.ban > 0)
                {
                    var row = ds.Tables[0].Rows[0];
                    respuesta.msg = "Login exitoso";
                    json.Add("Id", row["USU_ID"].ToString());
                    json.Add("Nombre", row["USU_NOMBRE"].ToString());
                    json.Add("Apellido", row["USU_APELLIDO"].ToString());
                    json.Add("Email", row["USU_EMAIL"].ToString());
                    json.Add("Matricula", row["USU_MATRICULA"].ToString());
                    json.Add("Usuario", row["USU_USERNAME"].ToString());
                    json.Add("Imagen", row["RUTA_IMAGEN"].ToString());


                }
                else
                {
                    respuesta.msg = "Acceso denegado";
                }

                respuesta.datos = json;
            }
            catch (Exception ex)
            {
                respuesta.statusExec = false;
                respuesta.ban = -1;
                respuesta.msg = "Excepción en Login";
                json.Add("error", ex.Message);
                respuesta.datos = json;
            }
            return respuesta;
        }


        [HttpPost]
        [Route("insertar")]
        public clsApiStatus Insertar([FromBody] clsUsuario modelo)
        {
            var respuesta = new clsApiStatus();
            var json = new JObject();
            try
            {
                var ds = modelo.spInsUsuario();
                respuesta.statusExec = true;
                respuesta.ban = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                respuesta.msg = respuesta.ban == 0 ? "Usuario insertado correctamente" : "Error al insertar usuario";
                json.Add("resultado", respuesta.msg);
                respuesta.datos = json;
            }
            catch (Exception ex)
            {
                respuesta.statusExec = false;
                respuesta.ban = -1;
                respuesta.msg = "Excepción en Insertar";
                json.Add("error", ex.Message);
                respuesta.datos = json;
            }
            return respuesta;
        }


        [HttpGet]
        [Route("consultarAlumnoPorId")]
        public clsApiStatus ConsultarAlumnoPorId(int id)
        {
            var respuesta = new clsApiStatus();
            try
            {
                // Llamo al método que recibe un entero
                var ds = new clsUsuario().ConsultarAlumnoPorId(id);

                respuesta.statusExec = true;
                respuesta.ban = ds.Tables[0].Rows.Count;
                respuesta.msg = "Consulta de alumno exitosa";

                // Serializo la tabla completa a un array JSON
                string jsonArray = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                respuesta.datos = JObject.Parse($"{{\"alumno\":{jsonArray}}}");

                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.statusExec = false;
                respuesta.ban = -1;
                respuesta.msg = "Excepción en ConsultarAlumnoPorId";
                respuesta.datos = new JObject(new JProperty("error", ex.Message));
                return respuesta;
            }
        }

        [HttpPost]
        [Route("actualizar")]
        public clsApiStatus Actualizar([FromBody] clsUsuario modelo)
        {
            var respuesta = new clsApiStatus();
            var json = new JObject();
            try
            {
                var ds = modelo.spUpdUsuario();
                respuesta.statusExec = true;
                respuesta.ban = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                respuesta.msg = respuesta.ban == 1 ? "Usuario actualizado" : "Error al actualizar";
                respuesta.datos = new JObject(new JProperty("resultado", respuesta.msg));
            }
            catch (Exception ex)
            {
                respuesta.statusExec = false;
                respuesta.ban = -1;
                respuesta.msg = "Excepción en Actualizar";
                respuesta.datos = new JObject(new JProperty("error", ex.Message));
            }
            return respuesta;
        }

        [HttpPost]
        [Route("eliminar")]
        public clsApiStatus Eliminar([FromBody] clsUsuario modelo)
        {
            var respuesta = new clsApiStatus();
            var json = new JObject();
            try
            {
                var ds = modelo.spDelUsuario();
                respuesta.statusExec = true;
                respuesta.ban = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                respuesta.msg = respuesta.ban == 1 ? "Usuario eliminado" : "Error al eliminar";
                respuesta.datos = new JObject(new JProperty("resultado", respuesta.msg));
            }
            catch (Exception ex)
            {
                respuesta.statusExec = false;
                respuesta.ban = -1;
                respuesta.msg = "Excepción en Eliminar";
                respuesta.datos = new JObject(new JProperty("error", ex.Message));
            }
            return respuesta;
        }

       ///////////////////////// /////////////////////////////////////////////ITEMS

        [HttpGet]
        [Route("listarItemsDisponibles")]
        public clsApiStatus ListarItemsDisponibles()
        {
            var respuesta = new clsApiStatus();
            try
            {
                var ds = new clsUsuario().vwItemsDisponibles(); 
                respuesta.statusExec = true;
                respuesta.ban = ds.Tables[0].Rows.Count;
                respuesta.msg = "Consulta de items disponibles exitosa";
                string jsonString = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                respuesta.datos = JObject.Parse($"{{\"items\":{jsonString}}}");
            }
            catch (Exception ex)
            {
                respuesta.statusExec = false;
                respuesta.ban = -1;
                respuesta.msg = "Excepción en ListarItemsDisponibles";
                respuesta.datos = new JObject(new JProperty("error", ex.Message));
            }
            return respuesta;
        }

        [HttpGet]
        [Route("listarItems")]
        public clsApiStatus ListarItems([FromUri] string filtro = "")
        {
            var respuesta = new clsApiStatus();
            try
            {
                var ds = new clsUsuario().vwItemsDisponibles(filtro); // Con filtro por ITEM o usuario
                respuesta.statusExec = true;
                respuesta.ban = ds.Tables[0].Rows.Count;
                respuesta.msg = "Consulta de items exitosa";
                string jsonString = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                respuesta.datos = JObject.Parse($"{{\"items\":{jsonString}}}");
            }
            catch (Exception ex)
            {
                respuesta.statusExec = false;
                respuesta.ban = -1;
                respuesta.msg = "Excepción en ListarItems";
                respuesta.datos = new JObject(new JProperty("error", ex.Message));
            }
            return respuesta;
        }


        [HttpPost]
        [Route("insertarItem")]
        public clsApiStatus InsertarItem([FromBody] clsUsuario modelo)
        {
            var respuesta = new clsApiStatus();
            var json = new JObject();
            try
            {
                var ds = modelo.spInsertarItem();
                respuesta.statusExec = true;
                respuesta.ban = 0;
                respuesta.msg = "Item insertado correctamente";
                json.Add("resultado", respuesta.msg);
                respuesta.datos = json;
            }
            catch (Exception ex)
            {
                respuesta.statusExec = false;
                respuesta.ban = -1;
                respuesta.msg = "Excepción en InsertarItem";
                json.Add("error", ex.Message);
                respuesta.datos = json;
            }
            return respuesta;
        }



        [HttpPost]
        [Route("actualizarItem")]
        public clsApiStatus ActualizarItem([FromBody] clsUsuario modelo)
        {
            var respuesta = new clsApiStatus();
            var json = new JObject();
            try
            {
                var ds = modelo.spUpdItemUsuario();
                respuesta.statusExec = true;
                respuesta.ban = 0;
                respuesta.msg = "Item actualizado correctamente";
                json.Add("resultado", respuesta.msg);
                respuesta.datos = json;
            }
            catch (Exception ex)
            {
                respuesta.statusExec = false;
                respuesta.ban = -1;
                respuesta.msg = "Excepción en ActualizarItem";
                json.Add("error", ex.Message);
                respuesta.datos = json;
            }
            return respuesta;
        }


        [HttpPost]
        [Route("eliminarItem")]
        public clsApiStatus EliminarItem([FromBody] clsUsuario modelo)
        {
            var respuesta = new clsApiStatus();
            var json = new JObject();

            try
            {
                var ds = modelo.spDelItem();
                respuesta.statusExec = true;
                respuesta.ban = 0;
                respuesta.msg = "Item eliminado correctamente";
                json.Add("resultado", respuesta.msg);
                respuesta.datos = json;
            }
            catch (Exception ex)
            {
                respuesta.statusExec = false;
                respuesta.ban = -1;
                respuesta.msg = "Excepción en EliminarItem";
                json.Add("error", ex.Message);
                respuesta.datos = json;
            }

            return respuesta;
        }


        [HttpGet]
        [Route("listarTipos")]
        public clsApiStatus ListarTipos()
        {
            var respuesta = new clsApiStatus();
            try
            {
                var ds = new clsUsuario().obtenerTipos();
                respuesta.statusExec = true;
                respuesta.ban = ds.Tables[0].Rows.Count;
                respuesta.msg = "Consulta de tipos exitosa";
                string jsonString = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                respuesta.datos = JObject.Parse($"{{\"tipos\":{jsonString}}}");
            }
            catch (Exception ex)
            {
                respuesta.statusExec = false;
                respuesta.ban = -1;
                respuesta.msg = "Error en ListarTipos";
                respuesta.datos = new JObject(new JProperty("error", ex.Message));
            }
            return respuesta;
        }

        [HttpGet]
        [Route("listarEstados")]
        public clsApiStatus ListarEstados()
        {
            var respuesta = new clsApiStatus();
            try
            {
                var ds = new clsUsuario().obtenerEstados();
                respuesta.statusExec = true;
                respuesta.ban = ds.Tables[0].Rows.Count;
                respuesta.msg = "Consulta de estados exitosa";
                string jsonString = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                respuesta.datos = JObject.Parse($"{{\"estados\":{jsonString}}}");
            }
            catch (Exception ex)
            {
                respuesta.statusExec = false;
                respuesta.ban = -1;
                respuesta.msg = "Error en ListarEstados";
                respuesta.datos = new JObject(new JProperty("error", ex.Message));
            }
            return respuesta;
        }

        [HttpGet]
        [Route("listarUbicaciones")]
        public clsApiStatus ListarUbicaciones()
        {
            var respuesta = new clsApiStatus();
            try
            {
                var ds = new clsUsuario().obtenerUbicaciones();
                respuesta.statusExec = true;
                respuesta.ban = ds.Tables[0].Rows.Count;
                respuesta.msg = "Consulta de ubicaciones exitosa";
                string jsonString = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                respuesta.datos = JObject.Parse($"{{\"ubicaciones\":{jsonString}}}");
            }
            catch (Exception ex)
            {
                respuesta.statusExec = false;
                respuesta.ban = -1;
                respuesta.msg = "Error en ListarUbicaciones";
                respuesta.datos = new JObject(new JProperty("error", ex.Message));
            }
            return respuesta;
        }



    }
}
