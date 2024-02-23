using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using PJN.DAL.Model;
using PJN.DAL.Model.GENERAL;
using PJN.DAL.Model.LATRINIDAD;
namespace PJN.DAL
{
    public class PJNDbContext : DbContext
    {
        public PJNDbContext()
            : base("name=cnnPJN")
        { 
        
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<PJNDbContext>(null);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<SEGMENTOS>().ToTable("SEGMENTOS", "sde");
            modelBuilder.Entity<AsignacionSegmento>().ToTable("AsignacionSegmento", "sde");
            modelBuilder.Entity<Usuario>().ToTable("Usuario", "sde");
            modelBuilder.Entity<UsuarioRol>().ToTable("relUsuarioRol", "sde");
            modelBuilder.Entity<Rol>().ToTable("Rol", "sde");
            modelBuilder.Entity<SECTION7_CARAT_PERSONS>().ToTable("SECTION7_CARAT_PERSONS", "sde");
            modelBuilder.Entity<SeccionFormulario>().ToTable("SeccionFormulario", "sde");
            modelBuilder.Entity<VariableFormulario>().ToTable("VariableFormulario", "sde");
            modelBuilder.Entity<relSeccionVariable>().ToTable("relSeccionVariable", "sde");
            modelBuilder.Entity<CEPOV>().ToTable("CEPOV", "sde");
            modelBuilder.Entity<VARIABLECONTROL>().ToTable("VARIABLECONTROL", "sde");
           // modelBuilder.Entity<log_codificartrazas>().ToTable("lenincatalogo", "sde");
            modelBuilder.Entity<S04_LIST_EMIGRA>().ToTable("S04_LIST_EMIGRA", "sde");
            modelBuilder.Entity<CARACT_ESTAB_ECONOM>().ToTable("CARACT_ESTAB_ECONOM", "sde");
            modelBuilder.Entity<COMARCA>().ToTable("COMARCAs", "sde");
            modelBuilder.Entity<COMUNIDADES>().ToTable("COMUNIDADES", "sde");
            modelBuilder.Entity<relAsignacionPersonal>().ToTable("relAsignacionPersonal", "sde");
            modelBuilder.Entity<ArchivoBinario>().ToTable("ArchivoBinario", "sde");
            modelBuilder.Entity<catOpcion>().ToTable("catOpcion", "sde");
            modelBuilder.Entity<RolOpcion>().ToTable("relRolOpcion", "sde");
            modelBuilder.Entity<catCensoEncuesta>().ToTable("catCensoEncuesta", "sde");
            modelBuilder.Entity<DISTRITOS>().ToTable("DISTRITOS", "sde"); 

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<SEGMENTOS> SEGMENTOS { get; set; }
        public DbSet<AsignacionSegmento> AsignacionSegmento { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<UsuarioRol> UsuarioRol { get; set; }               
        public DbSet<SECTION7_CARAT_PERSONS> SECTION7_CARAT_PERSONS { get; set; }
        public DbSet<SeccionFormulario> SeccionFormulario { get; set; }
        public DbSet<VariableFormulario> VariableFormulario { get; set; }
        public DbSet<relSeccionVariable> relSeccionVariable { get; set; }
        public DbSet<CEPOV> CEPOV { get; set; }
        public DbSet<VARIABLECONTROL> VARIABLECONTROL { get; set; }
        public DbSet<log_codificartrazas> lenincatalogo { get; set; }
        public DbSet<S04_LIST_EMIGRA> S04_LIST_EMIGRA { get; set; }
        public DbSet<CARACT_ESTAB_ECONOM> CARACT_ESTAB_ECONOM { get; set; }
        public DbSet<COMARCA> COMARCA { get; set; }
        public DbSet<COMUNIDADES> COMUNIDADES { get; set; }
        public DbSet<relAsignacionPersonal> relAsignacionPersonal { get; set; }
        public DbSet<ArchivoBinario> ArchivoBinario { get; set; }
        public DbSet<catOpcion> catOpcion { get; set; }
        public DbSet<RolOpcion> RolOpcion { get; set; }
        public DbSet<catCensoEncuesta> CensoEncuesta { get; set; }
        public DbSet<DISTRITOS> DISTRITOS { get; set; }
    }
}
