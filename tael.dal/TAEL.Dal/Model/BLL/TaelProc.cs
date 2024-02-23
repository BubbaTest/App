using Gaia.BLL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace TAEL.Dal.Model.BLL
{
    public class TaelProc
    {
        private static int Error_Retorno = -99;
        private static string Error_Mensaje = "Ocurrio un error inesperado";
        TaelDBContext _dbTael = new TaelDBContext();

        public static bool InsertarEditarCatalogos(string Catalogo,string CampoId, string Id, string JsonParameter, string UsuarioId, string EntidadId, string CargoId, out int Retorno, out string Mensaje)
        {
            try
            {
                string strcomannd = "[dbo].[spInsertar] @Tabla,NULL,NULL,@Json,@UsuarioId,@EntidadId,@CargoId, @Retorno OUTPUT, @Mensaje OUTPUT";

                List<SqlParameter> Parametros = new List<SqlParameter>();

                Parametros.Add(new SqlParameter { ParameterName = "@Tabla", Value = Catalogo, SqlDbType = SqlDbType.NVarChar, Size = 50 });

                if (Id != "0")
                {
                    Parametros.Add(new SqlParameter { ParameterName = "@CampoId", Value = CampoId, SqlDbType = SqlDbType.NVarChar, Size = 50 });
                    Parametros.Add(new SqlParameter { ParameterName = "@Id", Value = Id, SqlDbType = SqlDbType.NVarChar, Size = 50 });
                    strcomannd = "[dbo].[spActualizar] @Tabla,@CampoId, @Id,@Json,@UsuarioId,@EntidadId,@CargoId,@Retorno OUTPUT, @Mensaje OUTPUT";
                } 
                
                Parametros.Add(new SqlParameter { ParameterName = "@Json", Value = JsonParameter, SqlDbType = SqlDbType.NVarChar, Size = -1 });
                Parametros.Add(new SqlParameter { ParameterName = "@UsuarioId", Value = UsuarioId, SqlDbType = SqlDbType.NVarChar, Size = 30 });
                Parametros.Add(new SqlParameter { ParameterName = "@EntidadId", Value = EntidadId, SqlDbType = SqlDbType.NVarChar, Size = 9 });
                Parametros.Add(new SqlParameter { ParameterName = "@CargoId", Value = CargoId, SqlDbType = SqlDbType.NVarChar, Size = 9 });

                SqlParameter pRetorno = new SqlParameter { ParameterName = "@Retorno", SqlDbType = SqlDbType.Int };
                SqlParameter pMensaje = new SqlParameter { ParameterName = "@Mensaje", SqlDbType = SqlDbType.NVarChar, Size = 1024 };
                pRetorno.Direction = ParameterDirection.Output;
                pMensaje.Direction = ParameterDirection.Output;
                Parametros.Add(pRetorno);
                Parametros.Add(pMensaje);

                SqlParameter[] ParametroArray = Parametros.ToArray();
                var gr = new GenericRepository<dynamic>(typeof(TaelDBContext));
                gr.ExecuteNonQuery(strcomannd, ParametroArray);

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                return true;
            }
            catch (Exception ex)
            {
                Retorno = Error_Retorno;
                Mensaje = Error_Mensaje;

                return false;
            }
        }

        public static string ObtenerConsulta(string JsonWhere, string OrdenPor, string OrdenTipo, int NPagina, int NRegPag,
            string UsuarioId,string RolId, string EntidadId)
        {

            try
            {
                GenericRepository<string> cnn = new GenericRepository<string>(typeof(TaelDBContext));

                List<SqlParameter> Parametros = new List<SqlParameter>();
                Parametros.Add(new SqlParameter { ParameterName = "@JsonAcotaciones", Value = JsonWhere, SqlDbType = SqlDbType.NVarChar, Size = -1 });
                Parametros.Add(new SqlParameter { ParameterName = "@OrdenPor", Value = OrdenPor, SqlDbType = SqlDbType.NVarChar, Size = 100 });
                Parametros.Add(new SqlParameter { ParameterName = "@OrdenTipo", Value = OrdenTipo, SqlDbType = SqlDbType.NVarChar, Size = 10 });
                Parametros.Add(new SqlParameter { ParameterName = "@NRegIni", Value = NPagina, SqlDbType = SqlDbType.Int });
                Parametros.Add(new SqlParameter { ParameterName = "@NRegPag", Value = NRegPag, SqlDbType = SqlDbType.Int });
                Parametros.Add(new SqlParameter { ParameterName = "@UsuarioId", Value = UsuarioId, SqlDbType = SqlDbType.NVarChar, Size = 30 });
                Parametros.Add(new SqlParameter { ParameterName = "@RolId", Value = RolId, SqlDbType = SqlDbType.NVarChar, Size = 30 });
                Parametros.Add(new SqlParameter { ParameterName = "@EntidadId", Value = EntidadId, SqlDbType = SqlDbType.NVarChar, Size = 30 });
                
                SqlParameter[] oParametros = Parametros.ToArray();
                string result = cnn.ExecuteStoreProcedure("dbo.spAsuntoConActuacionObtener @JsonAcotaciones, @OrdenPor, @OrdenTipo, @NRegIni, @NRegPag, @UsuarioId, @RolId,@EntidadId", oParametros).FirstOrDefault();
                
                return result;
            }
            catch (Exception ex)
            {
                return "{\"Retorno\": " + Error_Retorno + ", \"Mensaje\": " + Error_Mensaje + ", \"JsonDatosAsunto\":[]}";
            }

        }

        public static string spActuacionConDocumentoObtener(string JsonAcotaciones, string OrdenPor, string OrdenTipo, int NRegIni, int NRegPag, string UsuarioId, string EntidadId, string RolId)//, out int NRegistro, out int NTotalRegistro, out int Retorno, out string Mensaje)
        {
            int NRegistro_Aout = 0;
            int TotalRegistro_Aout = 0;

            try
            {
                GenericRepository<string> cnn = new GenericRepository<string>(typeof(TaelDBContext));

                string sqlCommand = "[dbo].[spActuacionConDocumentoObtener] @JsonAcotaciones,@OrdenPor,@OrdenTipo,@NRegIni,@NRegPag,@UsuarioId,@EntidadId,@RolId";//,@NRegistro OUTPUT,@TotalRegistro OUTPUT,@Retorno OUTPUT,@Mensaje OUTPUT";

                List<SqlParameter> Parametros = new List<SqlParameter>();

                Parametros.Add(new SqlParameter { ParameterName = "@JsonAcotaciones", Value = JsonAcotaciones, SqlDbType = SqlDbType.NVarChar, Size = -1 });
                Parametros.Add(new SqlParameter { ParameterName = "@OrdenPor", Value = OrdenPor, SqlDbType = SqlDbType.NVarChar, Size = 100 });
                Parametros.Add(new SqlParameter { ParameterName = "@OrdenTipo", Value = OrdenTipo, SqlDbType = SqlDbType.NVarChar, Size = 10 });
                Parametros.Add(new SqlParameter { ParameterName = "@NRegIni", Value = NRegIni, SqlDbType = SqlDbType.Int, Size = -1 });
                Parametros.Add(new SqlParameter { ParameterName = "@NRegPag", Value = NRegPag, SqlDbType = SqlDbType.Int, Size = -1 });

                Parametros.Add(new SqlParameter { ParameterName = "@UsuarioId", Value = UsuarioId, SqlDbType = SqlDbType.NVarChar, Size = 9 });
                Parametros.Add(new SqlParameter { ParameterName = "@RolId", Value = RolId, SqlDbType = SqlDbType.NVarChar, Size = 30 });
                Parametros.Add(new SqlParameter { ParameterName = "@EntidadId", Value = EntidadId, SqlDbType = SqlDbType.NVarChar, Size = 9 });
                
                SqlParameter[] ParametrosArray = Parametros.ToArray();
                string Result2 = cnn.ExecuteStoreProcedure(sqlCommand, ParametrosArray).FirstOrDefault();
                
                return Result2;
            }
            catch (Exception e)
            {
                return "";
            }
        }
        
        public static string spDocumentoBBDDObtenerDatos(string DocumentosActuacionId, string ActuacionId, string UsuarioId, string EntidadId, string CargoId)//, out int NRegistro, out int NTotalRegistro, out int Retorno, out string Mensaje)
        {


            try
            {
                GenericRepository<string> cnn = new GenericRepository<string>(typeof(TaelDBContext));

                string sqlCommand = "[dbo].[spDocumentoBBDDObtenerDatos] @DocumentosActuacionId,@ActuacionId,@UsuarioId,@EntidadId,@CargoId";//,@NRegistro OUTPUT,@TotalRegistro OUTPUT,@Retorno OUTPUT,@Mensaje OUTPUT";

                List<SqlParameter> Parametros = new List<SqlParameter>();

                Parametros.Add(new SqlParameter { ParameterName = "@DocumentosActuacionId", Value = DocumentosActuacionId, SqlDbType = SqlDbType.Int, Size = -1 });
                Parametros.Add(new SqlParameter { ParameterName = "@ActuacionId", Value = ActuacionId, SqlDbType = SqlDbType.Int, Size = -1 });


                Parametros.Add(new SqlParameter { ParameterName = "@UsuarioId", Value = UsuarioId, SqlDbType = SqlDbType.NVarChar, Size = 30});
                Parametros.Add(new SqlParameter { ParameterName = "@EntidadId", Value = EntidadId, SqlDbType = SqlDbType.NVarChar, Size = 30 });
                Parametros.Add(new SqlParameter { ParameterName = "@CargoId", Value = CargoId, SqlDbType = SqlDbType.NVarChar, Size = 30 });
                
                SqlParameter[] ParametrosArray = Parametros.ToArray();
                string Result2 = cnn.ExecuteStoreProcedure(sqlCommand, ParametrosArray).FirstOrDefault();
                
                return Result2;
            }
            catch (Exception e)
            {
                return "";
            }
        }


    }
}
