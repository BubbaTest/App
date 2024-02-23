using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;

namespace Gaia.BLL.Model
{
    public class DBConfig : DbConfiguration
    {
        public DBConfig()
        {
            SetTransactionHandler(SqlProviderServices.ProviderInvariantName, () => new CommitFailureHandler());
            SetExecutionStrategy(SqlProviderServices.ProviderInvariantName, () => new SqlAzureExecutionStrategy());
        }
    }
}