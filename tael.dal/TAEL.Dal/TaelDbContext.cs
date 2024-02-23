using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAEL.Dal.Model;

using TAEL.Dal.Model.CEPOVLATRINIDAD;

namespace TAEL.Dal
{
    public class TaelDBContext : DbContext
    {
        public TaelDBContext() : base("name=cnnTael") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<TaelDBContext>(null);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Cats1_q6>().ToTable("Cats1_q6", "dbo");

            

            base.OnModelCreating(modelBuilder);
        }


        public DbSet<Cats1_q6> Cats1_q6 { get; set; }

        
    }
}
