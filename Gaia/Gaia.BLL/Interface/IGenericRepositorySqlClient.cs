using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.BLL.Interface
{
    public interface IGenericRepositorySqlClient<TEntity>
    {
        IList<TEntity> SqlDataSet5(string cnnString, string Procedimiento, List<SqlParameter> Parametros);
        IList<TEntity> ToListReflection(DataTable table);
    }
}
