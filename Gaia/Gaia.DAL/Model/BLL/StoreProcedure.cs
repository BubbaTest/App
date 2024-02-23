using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gaia.BLL.Repository;
using System.Data.SqlClient;
using System.Data;

namespace Gaia.DAL.Model.BLL
{
    public static class StoreProcedure
    {
        private const int LOCAL_ERROR_NUMBER = -99;
        private const string LOCAL_ERROR_MESSAGE = "Ocurrio inesperado durante la ejecucion de la rutina";
        private static GaiaDbContext _dbGaia = new GaiaDbContext();

        public static string spcatUsuarioInsertar(string TipoIdentificadorId, string Identificador, out int Retorno, out string Mensaje)
        {
            try
            {
                GenericRepository<string> cnn = new GenericRepository<string>(new GaiaDbContext());

                List<SqlParameter> Parametros = new List<SqlParameter>();
                Parametros.Add(new SqlParameter { ParameterName = "@TipoIdentificadorId", Value = TipoIdentificadorId, SqlDbType = SqlDbType.NVarChar, Size = 9 });
                Parametros.Add(new SqlParameter { ParameterName = "@Identificador", Value = Identificador, SqlDbType = SqlDbType.NVarChar, Size = 8000 });

                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                Parametros.Add(pRetorno);

                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                Parametros.Add(pMensaje);

                SqlParameter[] oParametros = Parametros.ToArray();
                //var result = cnn.ExecuteScalar("SELECT '<html><body>Hello World!!! <br>'+ Identificador +'</body></html>' AS html", oParametros);
                //var result = cnn.ExecuteFunction<string>("SELECT '<html><body>Hello World!!! <br>"+ Identificador +"</body></html>' AS html").ToString();
                var result = cnn.ExecStoreProcedure("seguridad.spcatUsuarioInsertar @TipoIdentificadorId, @Identificador, @Retorno OUTPUT, @Mensaje OUTPUT", oParametros).FirstOrDefault();
                //var result = "SELECT '<html><body>Hello World!!! <br>'+ Identificador +'</body></html>' AS html";
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                return result;

            }
            catch (Exception ex)
            {
                Retorno = LOCAL_ERROR_NUMBER;
                Mensaje = LOCAL_ERROR_MESSAGE;

                return null;
            }
        }

        public static string RecuperarPasswordSistema(string UsuarioId, string SistemaId, out int Retorno, out string Mensaje)
        {
            try
            {
                GenericRepository<string> cnn = new GenericRepository<string>(new GaiaDbContext());

                List<SqlParameter> Parametros = new List<SqlParameter>();
                Parametros.Add(new SqlParameter { ParameterName = "@UsuarioId", Value = UsuarioId, SqlDbType = SqlDbType.VarChar, Size = 30 });
                Parametros.Add(new SqlParameter { ParameterName = "@SistemaId", Value = SistemaId, SqlDbType = SqlDbType.VarChar, Size = 30 });
                Parametros.Add(new SqlParameter { ParameterName = "@pwsAlterna", Value = DBNull.Value, SqlDbType = SqlDbType.VarChar, Size = 100 });

                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                Parametros.Add(pRetorno);

                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 4000);
                pMensaje.Direction = ParameterDirection.Output;
                Parametros.Add(pMensaje);

                SqlParameter[] oParametros = Parametros.ToArray();
                
                var result = _dbGaia.Database.ExecuteSqlCommand("EXECUTE seguridad.spRecuperarPasswordSistema @UsuarioId, @SistemaId, @Retorno OUTPUT, @Mensaje OUTPUT, @pwsAlterna", oParametros);
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                return result.ToString();

            }
            catch (Exception ex)
            {
                Retorno = LOCAL_ERROR_NUMBER;
                Mensaje = LOCAL_ERROR_MESSAGE;

                return null;
            }
        }

        public static bool relActualizaInserta(string Origen, string StringTabla, string StringCampo, string StringId, string reljson, string StringUsuarioId, string StringEntidadId, out int Retorno, out string Mensaje)
        {
            try
            {
                GaiaDbContext _db = new GaiaDbContext();
                List<SqlParameter> parametros = new List<SqlParameter>();
                string sqlCommand = "EXECUTE [dbo].[spActualizar] @Tabla, @CampoId, @Id, @Json, @Retorno OUTPUT, @Mensaje OUTPUT";

                if (Origen == "Inserta")
                { sqlCommand = "EXECUTE [dbo].[spInsertar] @Tabla, @CampoId, @Id, @Json, @UsuarioId, @EntidadUsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT"; }

                parametros.Add(new SqlParameter { ParameterName = "@Tabla", Value = StringTabla, SqlDbType = SqlDbType.VarChar, Size = 100 });
                parametros.Add(new SqlParameter { ParameterName = "@CampoId", Value = StringCampo, SqlDbType = SqlDbType.VarChar, Size = 100 });
                parametros.Add(new SqlParameter { ParameterName = "@Id", Value = StringId, SqlDbType = SqlDbType.VarChar, Size = 20 });
                parametros.Add(new SqlParameter { ParameterName = "@Json", Value = reljson, SqlDbType = SqlDbType.VarChar, Size = -1 });
                
                if (Origen == "Inserta")
                {
                    parametros.Add(new SqlParameter { ParameterName = "@UsuarioId", Value = StringUsuarioId, SqlDbType = SqlDbType.VarChar, Size = 30 });
                    parametros.Add(new SqlParameter { ParameterName = "@EntidadUsuarioId", Value = StringEntidadId, SqlDbType = SqlDbType.VarChar, Size = 9 });
                }

                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);

                SqlParameter[] oParametros = parametros.ToArray();
                _db.Database.ExecuteSqlCommand(sqlCommand, oParametros);

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Retorno = LOCAL_ERROR_NUMBER;
                Mensaje = LOCAL_ERROR_MESSAGE;

                return false;
            }
        }

        public static bool relEliminar(string StringTabla, string StringId, string StringId2, string StringId3, out int Retorno, out string Mensaje)
        {
            try
            {
                GaiaDbContext _db = new GaiaDbContext();
                List<SqlParameter> parametros = new List<SqlParameter>();

                parametros.Add(new SqlParameter { ParameterName = "@Tabla", Value = StringTabla, SqlDbType = SqlDbType.VarChar, Size = 100 });
                parametros.Add(new SqlParameter { ParameterName = "@Id", Value = StringId, SqlDbType = SqlDbType.VarChar, Size = 100 });
                parametros.Add(new SqlParameter { ParameterName = "@Id2", Value = StringId2, SqlDbType = SqlDbType.VarChar, Size = 100 });
                parametros.Add(new SqlParameter { ParameterName = "@Id3", Value = StringId3, SqlDbType = SqlDbType.VarChar, Size = 100 });
               
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);

                SqlParameter[] oParametros = parametros.ToArray();
                _db.Database.ExecuteSqlCommand("EXECUTE [dbo].[spEliminarRelacion] @Tabla, @Id, @Id2, @Id3, @Retorno OUTPUT, @Mensaje OUTPUT", oParametros);

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Retorno = LOCAL_ERROR_NUMBER;
                Mensaje = LOCAL_ERROR_MESSAGE;

                return false;
            }
        }
               

        //public static string spGuardaPJN(string pjnjson)
        //{
        //    try
        //    {
        //        GenericRepository<string> cnn = new GenericRepository<string>(new GaiaDbContext());

        //        List<SqlParameter> Parametros = new List<SqlParameter>();
        //        Parametros.Add(new SqlParameter { ParameterName = "@JonPersonalPJN", Value = pjnjson, SqlDbType = SqlDbType.NVarChar, Size = -1 });
        //        SqlParameter[] oParametros = Parametros.ToArray();
        //        var result = cnn.ExecStoreProcedure("dbo.spInsertarActualizarPersonalPJN @JonPersonalPJN", oParametros).FirstOrDefault();

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        return "{\"Retorno\":99,\"Mensaje\": \"No se puede ejecutar la acción solicitada.\"}";
        //    }
        //}

        public static string CambiarPasswordSistemaConVerificacion(string JsonDatos, string UsuarioId, string RolId, string EntidadId, out string JsonRetorno)
        {
            try
            {
                GenericRepository<string> cnn = new GenericRepository<string>(new GaiaDbContext());

                List<SqlParameter> Parametros = new List<SqlParameter>();
                Parametros.Add(new SqlParameter { ParameterName = "@JsonDatos", Value = JsonDatos, SqlDbType = SqlDbType.NVarChar, Size = -1 });
                Parametros.Add(new SqlParameter { ParameterName = "@UsuarioId", Value = UsuarioId, SqlDbType = SqlDbType.VarChar, Size = 30});
                Parametros.Add(new SqlParameter { ParameterName = "@RolId", Value = RolId, SqlDbType = SqlDbType.VarChar, Size = 30 });
                Parametros.Add(new SqlParameter { ParameterName = "@EntidadId", Value = EntidadId, SqlDbType = SqlDbType.VarChar, Size = 9 });

                ///////////////////////////////////////////////////////
                SqlParameter[] oParametros = Parametros.ToArray();
                //var result = cnn.ExecuteScalar("SELECT '<html><body>Hello World!!! <br>'+ Identificador +'</body></html>' AS html", oParametros);
                //var result = cnn.ExecuteFunction<string>("SELECT '<html><body>Hello World!!! <br>"+ Identificador +"</body></html>' AS html").ToString();

                JsonRetorno = cnn.ExecStoreProcedure("seguridad.spCambiarPasswordSistemaConVerificacion @JsonDatos, @UsuarioId, @RolId, @EntidadId", oParametros).FirstOrDefault();
                
                return JsonRetorno;
            }
            catch (Exception ex)
            {
                JsonRetorno = null;
                return JsonRetorno;
            }
        }

        public static Persona PersonaObtenerPerfil(string UsuarioId, string Identificador, string Conexion, out int Retorno, out string Mensaje)
        {
            Retorno = -1;
            Mensaje = "Ocurrio un error no controlado";

            try
            {                

                GenericRepository<Persona> PerfilUsuario = new GenericRepository<Persona>(new GaiaDbContext());

                List<SqlParameter> Parametros = new List<SqlParameter>();
                Parametros.Add(new SqlParameter { ParameterName = "@UsuarioId", Value = UsuarioId, SqlDbType = SqlDbType.VarChar, Size = 30 });
                Parametros.Add(new SqlParameter { ParameterName = "@Identificador", Value = Identificador, SqlDbType = SqlDbType.VarChar, Size = 9 });
                Parametros.Add(new SqlParameter { ParameterName = "@Conexion", Value = Conexion, SqlDbType = SqlDbType.VarChar, Size = 100 });

                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                Parametros.Add(pRetorno);

                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                Parametros.Add(pMensaje);
                SqlParameter[] oParametros = Parametros.ToArray();

                Persona resultado = PerfilUsuario.ExecStoreProcedure("dbo.spPersonaObtenerPerfil @UsuarioId, @Identificador, @Conexion, @Retorno OUTPUT, @Mensaje OUTPUT;", oParametros).FirstOrDefault();

                return resultado;
            }
            catch (Exception ex)
            {
                return new Persona();
            }
        }
        //@UsuarioId VARCHAR(30),
        //        @Password VARCHAR(MAX),
        //@SistemaId VARCHAR(30),
        //@RolId VARCHAR(30),
        //@EntidadId VARCHAR(9)
        public static string UsuarioSistemaValidar(string UsuarioId, string Password, string SistemaId, string RolId, string EntidadId, out string JsonRetorno)
        {
            try
            {
                GenericRepository<string> cnn = new GenericRepository<string>(new GaiaDbContext());

                List<SqlParameter> Parametros = new List<SqlParameter>();
                
                Parametros.Add(new SqlParameter { ParameterName = "@UsuarioId", Value = UsuarioId, SqlDbType = SqlDbType.VarChar, Size = 30 });
                Parametros.Add(new SqlParameter { ParameterName = "@Password", Value = Password, SqlDbType = SqlDbType.VarChar, Size = -1 });
                Parametros.Add(new SqlParameter { ParameterName = "@SistemaId", Value = SistemaId, SqlDbType = SqlDbType.VarChar, Size = -1 });

                if (RolId == null)
                {
                    Parametros.Add(new SqlParameter { ParameterName = "@RolId", Value = DBNull.Value, SqlDbType = SqlDbType.VarChar, Size = 30 });
                }
                else
                {
                    Parametros.Add(new SqlParameter { ParameterName = "@RolId", Value = RolId, SqlDbType = SqlDbType.VarChar, Size = 30 });
                }

                if (EntidadId == null)
                {
                    Parametros.Add(new SqlParameter { ParameterName = "@EntidadId", Value = DBNull.Value, SqlDbType = SqlDbType.VarChar, Size = 9 });
                }
                else
                {
                    Parametros.Add(new SqlParameter { ParameterName = "@EntidadId", Value = EntidadId, SqlDbType = SqlDbType.VarChar, Size = 9 });
                }

                SqlParameter[] oParametros = Parametros.ToArray();                

                JsonRetorno = cnn.ExecStoreProcedure("seguridad.spUsuarioSistemaValidar @UsuarioId, @Password, @SistemaId, @RolId, @EntidadId", oParametros).FirstOrDefault();
                //JsonRetorno = "{\"UsuarioId\":\"VFLORES\",\"EmpleadoId\":2828,\"Password\":null,\"Correo\":\"lbarrera @poderjudicial.gob.ni\",\"Avatar\":null,\"CodMunicipio\":\"2555\",\"EstadoId\":null,\"TipoIdentificadorId\":\"PJN\",\"Identificador\":\"2828\",\"ResetPassword\":null,\"UsuarioOpciones\":[],\"UsuarioRolEntidad\":[{\"UsuarioId\":\"VFLORES\",\"RolId\":\"RolGaiaAdmin\",\"EntidadId\":\"0400\",\"Usuario\":null,\"Rol\":{\"RolId\":\"RolGaiaAdmin\",\"Descripcion\":\"Rol Administrador de GAIA (Seguridad)\",\"FechaBaja\":null,\"Activo\":null,\"RolOpciones\":[{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":1,\"PadreId\":{},\"Opcion\":\"Gaia\",\"OpcionTipo\":\"Gaia\",\"Mapeo\":null,\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":2,\"PadreId\":{},\"Opcion\":\"Seguridad\",\"OpcionTipo\":\"Menu, Controlador\",\"Mapeo\":null,\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":3,\"PadreId\":{},\"Opcion\":\"CATI\",\"OpcionTipo\":\"Menu, Accion\",\"Mapeo\":\"Mantenimiento / _mttoCatGaia ? ProyectoId = Cati\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":3,\"PadreId\":{},\"Opcion\":\"Alexa\",\"OpcionTipo\":\"Menu, Accion\",\"Mapeo\":\"Mantenimiento / _mttoCatGaia ? ProyectoId = Alexa\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":3,\"PadreId\":{},\"Opcion\":\"Urano\",\"OpcionTipo\":\"Menu, Accion\",\"Mapeo\":\"Mantenimiento / _mttoCatGaia ? ProyectoId = Urano\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":3,\"PadreId\":{},\"Opcion\":\"NICARAO\",\"OpcionTipo\":\"Menu, Accion\",\"Mapeo\":\"Mantenimiento / _mttoCatGaia ? ProyectoId = NICARAO\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":3,\"PadreId\":{},\"Opcion\":\"ASISTA\",\"OpcionTipo\":\"Menu, Accion\",\"Mapeo\":\"Mantenimiento / _mttoCatGaia ? ProyectoId = ASISTA\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":3,\"PadreId\":{},\"Opcion\":\"Credencial\",\"OpcionTipo\":\"Menu, Accion\",\"Mapeo\":\"Mantenimiento / _mttoCatGaia ? ProyectoId = Credencial\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":3,\"PadreId\":{},\"Opcion\":\"Dicea\",\"OpcionTipo\":\"Menu, Accion\",\"Mapeo\":\"Mantenimiento / _mttoCatGaia ? ProyectoId = Dicea\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":3,\"PadreId\":{},\"Opcion\":\"Gaia\",\"OpcionTipo\":\"Menu, Accion\",\"Mapeo\":\"Mantenimiento / _mttoCatGaia ? ProyectoId = Gaia\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":3,\"PadreId\":{},\"Opcion\":\"Eon\",\"OpcionTipo\":\"Menu, Accion\",\"Mapeo\":\"Mantenimiento / _mttoCatGaia ? ProyectoId = Eon\",\"NombreIcono\":\"\",\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":3,\"PadreId\":{},\"Opcion\":\"Sape\",\"OpcionTipo\":\"Menu, Accion\",\"Mapeo\":\"Mantenimiento / _mttoCatGaia ? ProyectoId = Sape\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":3,\"PadreId\":{},\"Opcion\":\"CasacionSCA\",\"OpcionTipo\":\"Menu, Accion\",\"Mapeo\":\"Mantenimiento / _mttoCatGaia ? ProyectoId = CASACIONSCA\",\"NombreIcono\":\"\",\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":3,\"PadreId\":{},\"Opcion\":\"Infodirac\",\"OpcionTipo\":\"Menu, Accion\",\"Mapeo\":\"Mantenimiento / _mttoCatGaia ? ProyectoId = Infodirac\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":3,\"PadreId\":{},\"Opcion\":\"MONICA\",\"OpcionTipo\":\"Menu, Accion\",\"Mapeo\":\"Mantenimiento / _mttoCatGaia ? ProyectoId = MONICA\",\"NombreIcono\":\"\",\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":3,\"PadreId\":{},\"Opcion\":\"CasacionPEN\",\"OpcionTipo\":\"Menu, Accion\",\"Mapeo\":\"Mantenimiento / _mttoCatGaia ? ProyectoId = CasacionPEN\",\"NombreIcono\":\"\",\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":3,\"PadreId\":{},\"Opcion\":\"IRIS\",\"OpcionTipo\":\"Menu, Accion\",\"Mapeo\":\"Mantenimiento / _mttoCatGaia ? ProyectoId = IRIS\",\"NombreIcono\":\"\",\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":3,\"PadreId\":{},\"Opcion\":\"Sistema Prueba\",\"OpcionTipo\":\"Menu, Accion\",\"Mapeo\":\"Mantenimiento / _mttoCatGaia ? ProyectoId = SistemaPrueba\",\"NombreIcono\":\"\",\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":2,\"PadreId\":{},\"Opcion\":\"Elimina Usuario\",\"OpcionTipo\":\"Accion\",\"Mapeo\":\"Mantenimiento / eliminaUsuario\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":2,\"PadreId\":{},\"Opcion\":\"Actualizar Usuario\",\"OpcionTipo\":\"Accion\",\"Mapeo\":\"Mantenimiento / ActualizarUsuario\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":2,\"PadreId\":{},\"Opcion\":\"Agregar Usuario\",\"OpcionTipo\":\"Accion\",\"Mapeo\":\"Mantenimiento / AgregarUsuario\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":2,\"PadreId\":{},\"Opcion\":\"Guardar RelUsuarioRol\",\"OpcionTipo\":\"Accion\",\"Mapeo\":\"Mantenimiento / GuardarRelUsuarioRolEntidad\",\"NombreIcono\":\"\",\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":2,\"PadreId\":{},\"Opcion\":\"Elimina RelUsuario\",\"OpcionTipo\":\"Accion\",\"Mapeo\":\"Mantenimiento / eliminarelUsuario\",\"NombreIcono\":\"\",\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":2,\"PadreId\":{},\"Opcion\":\"Elimina Rol\",\"OpcionTipo\":\"Accion\",\"Mapeo\":\"Mantenimiento / eliminaRol\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":2,\"PadreId\":{},\"Opcion\":\"Actualizar Rol\",\"OpcionTipo\":\"Accion\",\"Mapeo\":\"Mantenimiento / ActualizarRol\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":2,\"PadreId\":{},\"Opcion\":\"Agregar Rol\",\"OpcionTipo\":\"Accion\",\"Mapeo\":\"Mantenimiento / AgregarRol\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":2,\"PadreId\":{},\"Opcion\":\"Guardar RelUsuarioRol\",\"OpcionTipo\":\"Accion\",\"Mapeo\":\"Mantenimiento / GuardarRelUsuarioRol\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":2,\"PadreId\":{},\"Opcion\":\"Guardar RelRolOpcion\",\"OpcionTipo\":\"Accion\",\"Mapeo\":\"Mantenimiento / GuardarRelRolOpcion\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":2,\"PadreId\":{},\"Opcion\":\"Elimina RelOpcAso\",\"OpcionTipo\":\"Accion\",\"Mapeo\":\"Mantenimiento / eliminarelOpcAso\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":2,\"PadreId\":{},\"Opcion\":\"Guardar RelRolEntidad\",\"OpcionTipo\":\"Accion\",\"Mapeo\":\"Mantenimiento / GuardarRelRolEntidad\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":2,\"PadreId\":{},\"Opcion\":\"Elimina RelEntidad\",\"OpcionTipo\":\"Accion\",\"Mapeo\":\"Mantenimiento / eliminarelEntidad\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":2,\"PadreId\":{},\"Opcion\":\"Elimina OpcAso\",\"OpcionTipo\":\"Accion\",\"Mapeo\":\"Mantenimiento / eliminaOpcAso\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":2,\"PadreId\":{},\"Opcion\":\"Actualizar OpcAso\",\"OpcionTipo\":\"Accion\",\"Mapeo\":\"Mantenimiento / ActualizarOpcAso\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":2,\"PadreId\":{},\"Opcion\":\"Agregar OpcAso\",\"OpcionTipo\":\"Accion\",\"Mapeo\":\"Mantenimiento / AgregarOpcAso\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":2,\"PadreId\":{},\"Opcion\":\"Elimina Entidad\",\"OpcionTipo\":\"Accion\",\"Mapeo\":\"Mantenimiento / eliminaEntidad\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":2,\"PadreId\":{},\"Opcion\":\"Actualizar Entidad\",\"OpcionTipo\":\"Accion\",\"Mapeo\":\"Mantenimiento / ActualizarEntidad\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":2,\"PadreId\":{},\"Opcion\":\"Agregar Entidad\",\"OpcionTipo\":\"Accion\",\"Mapeo\":\"Mantenimiento / AgregarEntidad\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":2,\"PadreId\":{},\"Opcion\":\"Usuario\",\"OpcionTipo\":\"Menu, Accion\",\"Mapeo\":\"Mantenimiento / _mttoUsuario\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":2,\"PadreId\":{},\"Opcion\":\"Guardar Usuario Opcion\",\"OpcionTipo\":\"Accion\",\"Mapeo\":\"Mantenimiento / GuardarRelUsuarioOpcAso\",\"NombreIcono\":\"\",\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":2,\"PadreId\":{},\"Opcion\":\"Sistemas\",\"OpcionTipo\":\"Menu, Accion\",\"Mapeo\":\"Mantenimiento / _mttoSistema\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":2,\"PadreId\":{},\"Opcion\":\"Entidad\",\"OpcionTipo\":\"Menu, Accion\",\"Mapeo\":\"Mantenimiento / _mttoEntidad\",\"NombreIcono\":\"\",\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":2,\"PadreId\":{},\"Opcion\":\"Auditoria\",\"OpcionTipo\":\"Menu, Accion\",\"Mapeo\":\"Auditoria / ListaAuditoria\",\"NombreIcono\":\"\",\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":2,\"PadreId\":{},\"Opcion\":\"Configurar Reporte\",\"OpcionTipo\":\"Menu, Accion\",\"Mapeo\":\"Mantenimiento / _mttoConfRep\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":2,\"PadreId\":{},\"Opcion\":\"Variable de Control\",\"OpcionTipo\":\"Menu, Accion\",\"Mapeo\":\"Mantenimiento / _mttoCatVariableControl\",\"NombreIcono\":null,\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}},{\"RolId\":\"RolGaiaAdmin\",\"OpcionId\":{},\"Default\":false,\"Rol\":null,\"Opcion\":{\"OpcionId\":{},\"Nivel\":2,\"PadreId\":{},\"Opcion\":\"Lista los datos de auditoria\",\"OpcionTipo\":\"Accion\",\"Mapeo\":\"Auditoria / FiltrarAuditoria\",\"NombreIcono\":\"\",\"DescripcionGeneral\":null,\"FechaBaja\":null,\"Activo\":true,\"OpcionesHijo\":[],\"RolOpciones\":[],\"Usuarios\":null}}],\"UsuarioRolEntidad\":[]},\"EntidadG\":{\"EntidadId\":\"0400\",\"Descripcion\":\"División de Sistemas Jurisdiccionales y Administrativos\",\"CodDepartamento\":\"55\",\"CodMunicipio\":\"2555\",\"Domicilio\":\"NIVEL CENTRAL\",\"Telefonos\":\"NULL\",\"Estado\":\"NULL\",\"Circuito\":\"NULL\",\"DescripcionCorta\":\"NULL\",\"SedeJudicial\":\"NULL\",\"intCodEntidadPJ\":400,\"chrCodEdificio\":null,\"FechaBaja\":null,\"Activo\":null,\"UsuarioRolEntidad\":[]}}],\"UsuarioSistema\":[{\"Id\":18935.0,\"UsuarioId\":\"VFLORES\",\"SistemaId\":\"Gaia\",\"Password\":null,\"Token\":\"$2a$10$lykUVv07HGecqVYBVhk7xuQEAQY / XpY3i9H3ZcpNyrGxZJitjO0Zm\",\"ResetPassword\":null,\"Sistema\":null,\"Usuario\":null}],\"IdentificadorPersonaTipo\":null,\"Persona\":{\"Identificador\":\"2828\",\"strNombreCompleto\":\"VICTOR MANUEL FLORES BALTODANO\",\"intCodEntidadPJ\":\"1317\",\"strEntidad\":null,\"intCodCargo\":\"471\",\"strCargo\":\"ING.EN ANALISIS, DISEÑO Y PROGRAMACION DE SISTEMAS\",\"Busqueda\":null,\"TipoIdentificadorId\":null,\"UsuarioId\":null,\"Usuarios\":[]}}";
                return JsonRetorno;
            }
            catch (Exception ex)
            {
                JsonRetorno = null;
                return JsonRetorno;
            }
        }        

        public static string catalogoObtener(string JsonDatos, string JsonAcotacion, string UsuarioId, string RolId, string EntidadId, out int Retorno, out string Mensaje)
        {
            Retorno = -1;
            Mensaje = "Ocurrio un error no controlado";

            try
            {

                GenericRepository<string> tabla = new GenericRepository<string>(new GaiaDbContext());

                List<SqlParameter> Parametros = new List<SqlParameter>();
                Parametros.Add(new SqlParameter { ParameterName = "@JsonDatos", Value = JsonDatos, SqlDbType = SqlDbType.NVarChar, Size = -1 });
                Parametros.Add(new SqlParameter { ParameterName = "@JsonAcotacion", Value = JsonAcotacion, SqlDbType = SqlDbType.NVarChar, Size = -1 });
                Parametros.Add(new SqlParameter { ParameterName = "@UsuarioId", Value = UsuarioId, SqlDbType = SqlDbType.VarChar, Size = 30 });
                Parametros.Add(new SqlParameter { ParameterName = "@RolId", Value = RolId, SqlDbType = SqlDbType.VarChar, Size = 30 });
                Parametros.Add(new SqlParameter { ParameterName = "@EntidadId", Value = EntidadId, SqlDbType = SqlDbType.VarChar, Size = 9 });

                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                Parametros.Add(pRetorno);

                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                Parametros.Add(pMensaje);
                SqlParameter[] oParametros = Parametros.ToArray();

                string resultado = tabla.ExecStoreProcedure("seguridad.spObtenerRegistrosTabla @JsonDatos, @JsonAcotacion, @UsuarioId, @RolId, @EntidadId, @Retorno OUTPUT, @Mensaje OUTPUT;", oParametros).FirstOrDefault();

                return resultado;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region

        /// <summary>
        /// Procedimiento que genera el código de PIN para confirmación de la acción a realizar
        /// </summary>
        /// <param name="Identificativo"></param>
        /// <param name="AppIdInserta"></param>
        /// <param name="IdentificadorTipoId"></param>
        /// <param name="LoginInserta"></param>
        /// <param name="RolIdInserta"></param>
        /// <param name="DireccionIpInserta"></param>
        /// <param name="HostNameInserta"></param>
        /// <param name="AppNameInserta"></param>
        /// <param name="Retorno"></param>
        /// <param name="Mensaje"></param>
        /// <returns></returns>
        public static string credencialesSmsEnviar(string Identificativo, string AppIdInserta, string IdentificadorTipoId, string LoginInserta, string RolIdInserta, string DireccionIpInserta, string HostNameInserta, string AppNameInserta, string MedioLocalizacion, out int Retorno, out string Mensaje)
        {
            string Result = "[]";
            try
            {
                string sqlCommand = "[credenciales].[spGenerarPINSeguridad] @Identificativo,@AppIdInserta,@IdentificadorTipoId,@LoginInserta,@RolIdInserta,@DireccionIpInserta,@HostNameInserta,@AppNameInserta,@MedioLocalizacion,@Retorno OUTPUT,@Mensaje OUTPUT";
                List<SqlParameter> Parametros = new List<SqlParameter>();

                Parametros.Add(new SqlParameter { ParameterName = "@Identificativo", Value = Identificativo, SqlDbType = SqlDbType.NVarChar, Size = 100 });
                Parametros.Add(new SqlParameter { ParameterName = "@AppIdInserta", Value = AppIdInserta, SqlDbType = SqlDbType.VarChar, Size = 9 });
                Parametros.Add(new SqlParameter { ParameterName = "@IdentificadorTipoId", Value = IdentificadorTipoId, SqlDbType = SqlDbType.VarChar, Size = 9 });               

                Parametros.Add(new SqlParameter { ParameterName = "@LoginInserta", Value = LoginInserta, SqlDbType = SqlDbType.VarChar, Size = 30 });
                Parametros.Add(new SqlParameter { ParameterName = "@RolIdInserta", Value = RolIdInserta, SqlDbType = SqlDbType.VarChar, Size = 30 });

                Parametros.Add(new SqlParameter { ParameterName = "@DireccionIpInserta", Value = DireccionIpInserta, SqlDbType = SqlDbType.VarChar, Size = 30 });
                Parametros.Add(new SqlParameter { ParameterName = "@HostNameInserta", Value = HostNameInserta, SqlDbType = SqlDbType.VarChar, Size = 50 });
                Parametros.Add(new SqlParameter { ParameterName = "@AppNameInserta", Value = AppNameInserta, SqlDbType = SqlDbType.VarChar, Size = 50 });

                Parametros.Add(new SqlParameter { ParameterName = "@MedioLocalizacion", Value = MedioLocalizacion, SqlDbType = SqlDbType.VarChar, Size = 100 });
                               

                SqlParameter pRetorno = new SqlParameter { ParameterName = "@Retorno", SqlDbType = SqlDbType.Int };
                SqlParameter pMensaje = new SqlParameter { ParameterName = "@Mensaje", SqlDbType = SqlDbType.NVarChar, Size = 1024 };
                pRetorno.Direction = ParameterDirection.Output;
                pMensaje.Direction = ParameterDirection.Output;

                Parametros.Add(pRetorno);
                Parametros.Add(pMensaje);

                SqlParameter[] ParametrosArray = Parametros.ToArray();
                GenericRepository<string> cnn = new GenericRepository<string>(typeof(GaiaDbContext));

                Result = cnn.ExecuteStoreProcedure(sqlCommand, ParametrosArray).FirstOrDefault();

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0)
                {
                    return Result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return Result;
        }

        public static string credencialesSmsConfirmar(string Identificativo, string IdentificadorTipoId, string AppIdInserta, string LoginInserta, string RolIdInserta, string CodigoSeguridad, out int Retorno, out string Mensaje)
        {
            string Result = "[]";
            try
            {
                string sqlCommand = "[credenciales].[spPINSeguridadConfirmar] @Identificativo,@IdentificadorTipoId,@AppIdInserta,@LoginInserta,@RolIdInserta,@CodigoSeguridad, @Retorno OUTPUT,@Mensaje OUTPUT";
                List<SqlParameter> Parametros = new List<SqlParameter>();

                Parametros.Add(new SqlParameter { ParameterName = "@Identificativo", Value = Identificativo, SqlDbType = SqlDbType.NVarChar, Size = 100 });
                Parametros.Add(new SqlParameter { ParameterName = "@AppIdInserta", Value = AppIdInserta, SqlDbType = SqlDbType.VarChar, Size = 9 });
                Parametros.Add(new SqlParameter { ParameterName = "@IdentificadorTipoId", Value = IdentificadorTipoId, SqlDbType = SqlDbType.VarChar, Size = 9 });

                Parametros.Add(new SqlParameter { ParameterName = "@LoginInserta", Value = LoginInserta, SqlDbType = SqlDbType.VarChar, Size = 30 });
                Parametros.Add(new SqlParameter { ParameterName = "@RolIdInserta", Value = RolIdInserta, SqlDbType = SqlDbType.VarChar, Size = 30 });

                Parametros.Add(new SqlParameter { ParameterName = "@CodigoSeguridad", Value = CodigoSeguridad, SqlDbType = SqlDbType.VarChar, Size = 4 });

                SqlParameter pRetorno = new SqlParameter { ParameterName = "@Retorno", SqlDbType = SqlDbType.Int };
                SqlParameter pMensaje = new SqlParameter { ParameterName = "@Mensaje", SqlDbType = SqlDbType.NVarChar, Size = 1024 };
                pRetorno.Direction = ParameterDirection.Output;
                pMensaje.Direction = ParameterDirection.Output;

                Parametros.Add(pRetorno);
                Parametros.Add(pMensaje);

                SqlParameter[] ParametrosArray = Parametros.ToArray();
                GenericRepository<string> cnn = new GenericRepository<string>(typeof(GaiaDbContext));

                Result = cnn.ExecuteStoreProcedure(sqlCommand, ParametrosArray).FirstOrDefault();

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0)
                {
                    return Result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return Result;
        }

        #endregion sms
        

        public static string UsuarioNicarao(string UsuarioId, string SistemaId, string EntidadId, string RolId, out int Retorno, out string Mensaje)
        {
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>();
                GenericRepository<string> cnn = new GenericRepository<string>(typeof(GaiaDbContext));

                parametros.Add(new SqlParameter { ParameterName = "@UsuarioId", Value = UsuarioId, SqlDbType = SqlDbType.NVarChar, Size = 20 });
                parametros.Add(new SqlParameter { ParameterName = "@SistemaId", Value = SistemaId, SqlDbType = SqlDbType.NVarChar, Size = 20 });
                parametros.Add(new SqlParameter { ParameterName = "@EntidadId", Value = EntidadId, SqlDbType = SqlDbType.NVarChar, Size = 20 });
                parametros.Add(new SqlParameter { ParameterName = "@RolId", Value = RolId, SqlDbType = SqlDbType.NVarChar, Size = 70 });

                SqlParameter pRetorno = new SqlParameter { ParameterName = "@Retorno", SqlDbType = SqlDbType.Int };
                SqlParameter pMensaje = new SqlParameter { ParameterName = "@Mensaje", SqlDbType = SqlDbType.NVarChar, Size = 1024 };
                pRetorno.Direction = ParameterDirection.Output;
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                parametros.Add(pMensaje);

                SqlParameter[] oParametros = parametros.ToArray();
                var result = cnn.ExecuteStoreProcedure("seguridad.relUsuarioRolUsuarioNicObtener @UsuarioId,@SistemaId,@EntidadId,@RolId,@Retorno OUTPUT,@Mensaje OUTPUT", oParametros).FirstOrDefault();

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                return result;

            }
            catch (Exception ex)
            {
                Retorno = 99;
                Mensaje = "Ocurrio un error inesperado";

                return "";

            }
        }

        /// <summary>
        /// Verificación de Autorización Gaia
        /// </summary>
        /// <param name="UsuarioAutoriza">Usuario que autoriza la acción</param>
        /// <param name="SistemaId">Sistema en el que se encuentra el usuario logueado</param>
        /// <param name="Password">Contraseña del usuario que autoriza</param>
        /// <param name="Opcion">Opción que deberá validarse contra los permisos del usuario asociados</param>
        /// <param name="UsuarioSession">Usuario perteneciente a la sesión</param>
        /// <param name="EntidadSession">Entidad perteneciente a la sesión</param>
        /// <param name="RolSession">Rol perteneciente a la sesión</param>
        /// <param name="JsonRetorno">Mensaje de respuesta</param>
        /// <returns>Cadena en formato JSON</returns>
        public static string UsuarioSistemaValidarAutorizacion(string UsuarioAutoriza, string SistemaId, string Password, string Opcion, string UsuarioSession, string EntidadSession, string RolSession, out string JsonRetorno)
        {
            try
            {
                GenericRepository<string> cnn = new GenericRepository<string>(new GaiaDbContext());

                List<SqlParameter> Parametros = new List<SqlParameter>();

                Parametros.Add(new SqlParameter { ParameterName = "@UsuarioAutoriza", Value = UsuarioAutoriza, SqlDbType = SqlDbType.VarChar, Size = 30 });                
                Parametros.Add(new SqlParameter { ParameterName = "@PasswordUsuAut", Value = Password, SqlDbType = SqlDbType.VarChar, Size = -1 });
                Parametros.Add(new SqlParameter { ParameterName = "@SistemaId", Value = SistemaId, SqlDbType = SqlDbType.VarChar, Size = -1 });
                Parametros.Add(new SqlParameter { ParameterName = "@Opcion", Value = Opcion, SqlDbType = SqlDbType.VarChar, Size = -1 });                

                if (UsuarioSession == null)
                {
                    Parametros.Add(new SqlParameter { ParameterName = "@Usuario", Value = DBNull.Value, SqlDbType = SqlDbType.VarChar, Size = 30 });
                }
                else
                {
                    Parametros.Add(new SqlParameter { ParameterName = "@Usuario", Value = UsuarioSession, SqlDbType = SqlDbType.VarChar, Size = 30 });
                }

                if (RolSession == null)
                {
                    Parametros.Add(new SqlParameter { ParameterName = "@RolId", Value = DBNull.Value, SqlDbType = SqlDbType.VarChar, Size = 30 });
                }
                else
                {
                    Parametros.Add(new SqlParameter { ParameterName = "@RolId", Value = RolSession, SqlDbType = SqlDbType.VarChar, Size = 30 });
                }

                if (EntidadSession == null)
                {
                    Parametros.Add(new SqlParameter { ParameterName = "@EntidadId", Value = DBNull.Value, SqlDbType = SqlDbType.VarChar, Size = 9 });
                }
                else
                {
                    Parametros.Add(new SqlParameter { ParameterName = "@EntidadId", Value = EntidadSession, SqlDbType = SqlDbType.VarChar, Size = 9 });
                }

                SqlParameter[] oParametros = Parametros.ToArray();

                JsonRetorno = cnn.ExecStoreProcedure("seguridad.spVerificarAutorizacion @UsuarioAutoriza, @PasswordUsuAut, @Opcion, @Usuario, @SistemaId, @EntidadId, @RolId", oParametros).FirstOrDefault();
                
                return JsonRetorno;
            }
            catch (Exception ex)
            {
                JsonRetorno = "{\"Retorno\":\"-9999\",\"Mensaje\": \"Ha ocurrido un error durante la ejecución.\"}";
                return JsonRetorno;
            }
        }

    }
}
