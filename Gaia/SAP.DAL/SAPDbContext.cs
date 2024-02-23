using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using SAP.DAL;
using SAP.DAL.Model;

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

            modelBuilder.Entity<catTiposIdentificaciones>().ToTable("catTiposIdentificaciones", "dbo");

            modelBuilder.Entity<tbl_usuario>().ToTable("tbl_usuario", "dbo");

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<catTiposIdentificaciones> catTiposIdentificaciones { get; set; }
        public DbSet<tbl_usuario> tbl_usuario { get; set; }
    }
}
