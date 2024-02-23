using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using SIRUFJ.DAL;

namespace SIRUFJ.DAL
{
    public class SIRUFJDbContext : DbContext
    {
        public SIRUFJDbContext()
            : base("name=cnnSIRUFJ")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<SIRUFJDbContext>(null);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder.Entity<Circunscripcion>()
            //    .ToTable("catCircunscripcion");

            //modelBuilder.Entity<Departamento>()
            //    .ToTable("catDepartamento")
            //    ;

            //modelBuilder.Entity<Municipio>()
            //    .ToTable("vwMunicipio")
            //    .HasRequired(d => d.Departamento)
            //    .WithMany(m => m.Municipios)
            //    .HasForeignKey(m => m.chrCodDepartamento);

            //modelBuilder.Entity<vwMunicipioDICEA>()
            //    .ToTable("vwMunicipioDICEA")
            //    .HasRequired(d => d.Departamento)
            //    .WithMany(m => m.vwMunicipioDICEA)
            //    .HasForeignKey(m => m.chrCodDepartamento);

            //modelBuilder.Entity<Localidad>()
            //    .ToTable("vwLocalidad")
            //    .HasRequired(m => m.Municipio)
            //    .WithMany(l => l.Localidades)
            //    .HasForeignKey(l => l.chrCodMunicipioDepartamento);

            //modelBuilder.Entity<Personal>()
            //    .ToTable("vwPersonal2");

            //modelBuilder.Entity<PersonalCATI>()
            //    .ToTable("vwPersonal_CATI");

            //modelBuilder.Entity<BusquedaPadron>().ToTable("vwBusquedaPadron");

            //modelBuilder.Entity<PadronElectoral>()
            //    .ToTable("PadronElectoral");

            //modelBuilder.Entity<Entidad>()
            //    .ToTable("vwEntidadPJ2")
            //    .HasKey(e => e.intCodEntidadPJ);

            //modelBuilder.Entity<RelMunicipioPjnPadronNicarao>()
            //    .ToTable("RelMunicipioPjnPadronNicarao");

            //modelBuilder.Entity<Cargo>().ToTable("catCargo");
            //modelBuilder.Entity<XSADominio>().ToTable("XSADominio");
            //modelBuilder.Entity<XSAValorDominio>().ToTable("XSAValorDominio");

            //modelBuilder.Entity<catMoneda>().ToTable("catMoneda");
            //modelBuilder.Entity<vwPersonalSol>().ToTable("vwPersonalSol", "dbo");
            //modelBuilder.Entity<catEdificio>().ToTable("catEdificio");

            base.OnModelCreating(modelBuilder);
        }
        //public DbSet<Municipio> Municipios { get; set; }
        //public DbSet<vwMunicipioDICEA> MunicipioDICEA { get; set; }
        //public DbSet<Departamento> Departamentos { get; set; }
        //public DbSet<Localidad> Localidades { get; set; }
        //public DbSet<PadronElectoral> PadronElectoral { get; set; }
        //public DbSet<Entidad> Entidades { get; set; }
        //public DbSet<BusquedaPadron> ResultadoBusquedaP { get; set; }
        //public DbSet<RelMunicipioPjnPadronNicarao> RelMunicipioPjnPadronNicarao { get; set; }
        //public DbSet<Cargo> Cargo { get; set; }
        //public DbSet<XSADominio> XSADominio { get; set; }
        //public DbSet<XSAValorDominio> XSAValorDominio { get; set; }
        //public DbSet<catMoneda> catMoneda { get; set; }
        //public DbSet<Personal> Personal { get; set; }
        //public DbSet<PersonalCATI> PersonalCATI { get; set; }
        //public DbSet<vwPersonalSol> PersonalSol { get; set; }
        //public DbSet<catEdificio> catEdificio { get; set; }
    }
}
