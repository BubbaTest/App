using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Gaia.DAL.Model;

using Gaia.DAL.Model.notificacion;

//using Gaia.BLL.Model;

namespace Gaia.DAL
{
    //[DbConfigurationType(typeof(DBConfig))]
    public class GaiaDbContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        //System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<MvcDefensoria.Models.DefensoriaDbContext>());
        public GaiaDbContext()
            : base("name=cnnGaia")
        {
            //this.Configuration.LazyLoadingEnabled = false;
            //this.Configuration.ProxyCreationEnabled = false;
        }

        public GaiaDbContext(string cnn)
            :base(cnn)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Exception:
            //Model backing a DB Context has changed; Consider Code First Migrations
            //se agregaron las siguientes líneas, donde asociamos directamente la Entidad con la Tabla de la BD
            Database.SetInitializer<GaiaDbContext>(null);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //Si habilitamos esta línea de código, no se generarán clases dinámicas proxy, esto implica que 
            //se pierden las propiedades de navegabilidad entre clases asociadas
            //this.Configuration.ProxyCreationEnabled = false;
            //////Configuración mediante Fluent Api

            //Clase Auditoria (migajas)
            modelBuilder.Entity<Auditoria>().ToTable("Auditoria", "seguridad");

            //Clase catUsuario
            modelBuilder.Entity<Usuario>().ToTable("catUsuario");

            //Clase Rol
            modelBuilder.Entity<Rol>().ToTable("catRol");

            modelBuilder.Entity<catOpcion>().ToTable("catOpcion");

            modelBuilder.Entity<EntidadG>().ToTable("catEntidad");

            modelBuilder.Entity<catMateriaG>().ToTable("catMateria", "seguridad");

            modelBuilder.Entity<Sistema>().ToTable("catSistema", "seguridad");

            modelBuilder.Entity<UsuarioSistema>().ToTable("relUsuarioSistema", "seguridad");

            modelBuilder.Entity<Auditoria>().ToTable("Auditoria", "seguridad");

            modelBuilder.Entity<catSedeJudicial>().ToTable("catSedeJudicial", "seguridad");

            modelBuilder.Entity<catTipoObjeto>().ToTable("catTipoObjeto", "dbo");

            modelBuilder.Entity<CampoReporte>().ToTable("CampoReporte", "dbo");

            modelBuilder.Entity<relReporteCampo>().ToTable("relReporteCampo", "dbo");

            modelBuilder.Entity<relRolReporte>().ToTable("relRolReporte", "dbo"); 

            //Relación entre clases Usuario y catOpcion
            modelBuilder.Entity<Usuario>()
                .HasMany(o => o.UsuarioOpciones)
                .WithMany(r => r.Usuarios)
                .Map(ro =>
                {
                    ro.MapLeftKey("UsuarioId");
                    ro.MapRightKey("OpcionId");
                    ro.ToTable("relUsuarioOpcion");
                }
                );

            modelBuilder.Entity<BusquedaUsuarioGAIA>().ToTable("vwBusquedaUsuarioGAIA", "dbo");
            modelBuilder.Entity<Persona>().ToTable("vwPersona", "dbo");
            modelBuilder.Entity<IdentificadorPersonaTipo>().ToTable("catIdentificadorPersonaTipo");
            modelBuilder.Entity<UsuarioRolEntidad>().ToTable("relUsuarioRolEntidad");
            modelBuilder.Entity<vwAuditoria>().ToTable("vwAuditoria", "dbo");
            modelBuilder.Entity<vwRelUsuarioRolEntidadNicarao>().ToTable("vwRelUsuarioRolEntidadNicarao", "dbo");            

            modelBuilder.Entity<GaiaVariableControl>().ToTable("VariableControl", "dbo");            

            // Agregado por fernando...

            modelBuilder.Entity<catReporte>().ToTable("catReporte", "dbo");
            modelBuilder.Entity<catParametro>().ToTable("catParametro" , "dbo");
            //modelBuilder.Entity<catTipoObjeto>().ToTable("catTipoObjeto", "dbo");
            //modelBuilder.Entity<CampoReporte>().ToTable("CampoReporte", "dbo");
            //modelBuilder.Entity<relReporteCampo>().ToTable("relReporteCampo", "dbo");
            //modelBuilder.Entity<relRolReporte>().ToTable("relRolReporte", "dbo");


            // Fin

            //Notificación
            modelBuilder.Entity<SMS>().ToTable("SMS", "notificacion");
            modelBuilder.Entity<CORREO>().ToTable("CORREO", "notificacion");
            modelBuilder.Entity<SIGNALR>().ToTable("SIGNALR", "notificacion");
            modelBuilder.Entity<Configuracion>().ToTable("Configuracion", "notificacion");

            modelBuilder.HasDefaultSchema("seguridad");
            base.OnModelCreating(modelBuilder);            
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<catOpcion> Opciones { get; set; }
        public DbSet<EntidadG> EntidadesG { get; set; }
        public DbSet<BusquedaUsuarioGAIA> BusquedaUsuarioGAIA { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<UsuarioRolEntidad> UsuarioRolEntidad { get; set; }
        public DbSet<RolOpcion> RolOpcion { get; set; }
        public DbSet<Sistema> Sistema { get; set; }
        public DbSet<UsuarioSistema> UsuarioSistema { get; set; }
        public DbSet<Auditoria> Auditoria { get; set; }
        public DbSet<vwAuditoria> vwAuditoria { get; set; }
        public DbSet<catSedeJudicial> catSedeJudicial { get; set; }
        public DbSet<catMateriaG> catMateria { get; set; }        

        public DbSet<catTipoObjeto> catTipoObjeto { get; set; }
        public DbSet<CampoReporte> CampoReporte { get; set; }
        public DbSet<relReporteCampo> relReporteCampo { get; set; }
        public DbSet<relRolReporte> relRolReporte { get; set; }        

        // Agreado por fernando
        public DbSet<catReporte> catReporte { get; set; }
        public DbSet<catParametro> catParametro { get; set; }
        //public DbSet<catTipoObjeto> catTipoObjeto { get; set; }
        //public DbSet<CampoReporte> CampoReporte { get; set; }
        //public DbSet<relReporteCampo> relReporteCampo { get; set; }
        //public DbSet<relRolReporte> relRolReporte { get; set; }
        //fin

        //Notificación
        public DbSet<SMS> SMS { get; set; }
        public DbSet<CORREO> CORREO { get; set; }
        public DbSet<SIGNALR> SIGNALR { get; set; }
        public DbSet<Configuracion> Configuracion { get; set; }

        public DbSet<GaiaVariableControl> VariableControl { get; set; }
        public DbSet<vwRelUsuarioRolEntidadNicarao> vwRelUsuarioRolEntidadNicarao { get; set; }
    }
}
