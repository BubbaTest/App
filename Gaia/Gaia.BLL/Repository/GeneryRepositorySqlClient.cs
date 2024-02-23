using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;

using Gaia.BLL.Interface;
using System.Reflection;

namespace Gaia.BLL.Repository
{
    public class GeneryRepositorySqlClien<TEntity> : IGenericRepositorySqlClient<TEntity> where TEntity : class//, new()
    {
        //public static IList<T> SqlDataSet<T>(string ConnectionString, string SQLQueryString, string SQLTableString) where T : new()
        //{
        //    string connStr;
        //    connStr = ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString;

        //    SqlConnection dbConnection = new SqlConnection(connStr);

        //    // Se utiliza un objeto SqlCommand para ejecutar los comandos SQL.
        //    SqlCommand cmd = new SqlCommand(SQLQueryString, dbConnection);

        //    // SqlDataAdapter es responsable de utilizar un objeto SqlCommand para
        //    // rellenar un conjunto de datos.
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    DataSet ds = new DataSet();

        //    da.Fill(ds, SQLTableString);

        //    return ds.Tables[0].ToListReflection<T>();
        //}

        //public static IList<T> SqlDataSet2<T>(string cnnString, string SQLQueryString) where T : new()
        //{
        //    string connStr;
        //    connStr = ConfigurationManager.ConnectionStrings[cnnString].ConnectionString;

        //    SqlConnection dbConnection = new SqlConnection(connStr);

        //    // Se utiliza un objeto SqlCommand para ejecutar los comandos SQL.
        //    SqlCommand cmd = new SqlCommand(SQLQueryString, dbConnection);

        //    // SqlDataAdapter es responsable de utilizar un objeto SqlCommand para
        //    // rellenar un conjunto de datos.
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    DataSet ds = new DataSet();

        //    da.Fill(ds);
            
        //    return ds.Tables[0].ToListReflection<T>();
        //}

        //public static IList<T> SqlDataSet3<T>(string cnnString, string Procedimiento, List<SqlParameter> Parametros) where T : new()
        //{
        //    string connStr;
        //    connStr = ConfigurationManager.ConnectionStrings[cnnString].ConnectionString;
        //    SqlConnection conn = new SqlConnection(connStr);
        //    SqlCommand cmd = new SqlCommand();
        //    SqlDataAdapter da = new SqlDataAdapter();
        //    DataSet dt = new DataSet();

        //    cmd.CommandType = CommandType.StoredProcedure;
        //    conn.Open();
        //    cmd.Connection = conn;
        //    cmd.CommandText = Procedimiento;

        //    da.SelectCommand = cmd;
        //    //SqlCommandBuilder.DeriveParameters(cmd);

        //    try
        //    {
        //        if (Parametros != null)
        //        {
        //            cmd.Parameters.Clear();
        //            foreach (var param in Parametros)
        //                cmd.Parameters.Add(param);
        //            //for (var i = 1; i <= Parametros.Count; i++)
        //            //    cmd.Parameters[i] = Parametros[i - 1];
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    da.Fill(dt);
        //    conn.Close();

        //    return dt.Tables[0].ToListReflection<T>();
        //}

        //public static IList<T> SqlDataSet4<T>(string cnnString, string Procedimiento, params object[] Parametros) where T : new()
        //{
        //    string connStr;
        //    connStr = ConfigurationManager.ConnectionStrings[cnnString].ConnectionString;
        //    SqlConnection conn = new SqlConnection(connStr);
        //    SqlCommand cmd = new SqlCommand();
        //    SqlDataAdapter da = new SqlDataAdapter();
        //    DataSet dt = new DataSet();

        //    cmd.CommandType = CommandType.StoredProcedure;
        //    conn.Open();
        //    cmd.Connection = conn;
        //    cmd.CommandText = Procedimiento;

        //    da.SelectCommand = cmd;
        //    SqlCommandBuilder.DeriveParameters(cmd);

        //    try
        //    {
        //        if (Parametros != null)
        //        {
        //            for (var i = 1; i <= Parametros.Length; i++)
        //                cmd.Parameters[i].Value = ((SqlParameter)Parametros[i - 1]).Value;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    da.Fill(dt);
        //    conn.Close();

        //    return dt.Tables[0].ToListReflection<T>();
        //}

        public IList<TEntity> SqlDataSet5(string cnnString, string Procedimiento, List<SqlParameter> Parametros) //where T : class, new()
        {
            string connStr;
            connStr = ConfigurationManager.ConnectionStrings[cnnString].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet dt = new DataSet();

            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = Procedimiento;

            da.SelectCommand = cmd;
            //SqlCommandBuilder.DeriveParameters(cmd);

            try
            {
                if (Parametros != null)
                {
                    cmd.Parameters.Clear();
                    foreach (var param in Parametros)
                        cmd.Parameters.Add(param);
                    //for (var i = 1; i <= Parametros.Count; i++)
                    //    cmd.Parameters[i] = Parametros[i - 1];
                }
            }
            catch (Exception ex)
            {
            }

            da.Fill(dt);
            conn.Close();

            return ToListReflection(dt.Tables[0]);
        }

        public IList<TEntity> ToListReflection(DataTable table)// where T : new()
        {
            List<TEntity> result = new List<TEntity>();
            IList<PropertyInfo> properties = typeof(TEntity).GetProperties().ToList();

            foreach (var row in table.Rows)
            {
                result.Add(CreateItemFromRow((DataRow)row, properties));
            }

            return result;
        }

        private TEntity CreateItemFromRow(DataRow row, IList<PropertyInfo> properties)// where T : new()
        {
            TEntity item = null;

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(DayOfWeek))
                {
                    DayOfWeek day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), row[property.Name].ToString());
                    property.SetValue(item, day, null);
                }
                else
                {
                    if (row.Table.Columns[property.Name] != null)
                    {
                        if (row[property.Name] == DBNull.Value)
                            property.SetValue(item, null, null);
                        else
                            property.SetValue(item, row[property.Name], null);
                    }
                }
            }

            return item;
        }
    }
}
