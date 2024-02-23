using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using Nicarao.DAL.Model;
using Nicarao.DAL.Model.CEPOV;

namespace Nicarao.DAL
{
    public class NicaraoDbContext : System.Data.Entity.DbContext
    {
      
        public NicaraoDbContext()
            : base("name=SedeJudicial_14") { }

        public NicaraoDbContext(string stringConnection)
            : base(stringConnection) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //this.Configuration.ProxyCreationEnabled = false;

            Database.SetInitializer<NicaraoDbContext>(null);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Departamentos>().ToTable("Departamentos", "dbo");
            modelBuilder.Entity<Municipios>().ToTable("Municipios", "dbo");
            modelBuilder.Entity<LoginUsuarios>().ToTable("LoginUsuarios", "dbo");
            modelBuilder.Entity<ActividadClase>().ToTable("ActividadClase", "dbo");
            modelBuilder.Entity<Ocupacion>().ToTable("Ocupacion", "dbo");
            modelBuilder.Entity<CatalogoPais>().ToTable("CatalogoPais", "dbo");
            modelBuilder.Entity<CUTP>().ToTable("CUTP", "dbo");
            
        }

        public DbSet<Departamentos> Departamentos { get; set; }
        public DbSet<Municipios> Municipios { get; set; }
        public DbSet<LoginUsuarios> LoginUsuarios { get; set; }
        public DbSet<ActividadClase> ActividadClase { get; set; }
        public DbSet<Ocupacion> Ocupacion { get; set; }
        public DbSet<CatalogoPais> CatalogoPais { get; set; }


    }
}
