using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using SAP.DAL;
using SAP.Model;

namespace SAP.DAL
{
    public class SAPDbContext : DbContext
    {
        public SAPDbContext()
            : base("name=cnnSap")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<SAPDbContext>(null);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();            

            base.OnModelCreating(modelBuilder);
        }
        
    }
}
