using System;
using System.Collections.Generic;
using System.Data.Entity.Hierarchy;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gaia.DAL.Model
{
    public class Sistema
    {
        public Sistema()
        {
            UsuarioSistema = new HashSet<UsuarioSistema>();
        }

        [Key]
        public string SistemaId { get; set; }
        public HierarchyId OpcionId { get; set; }
        public string Descripcion { get; set; }
        public string LlaveCifrado { get; set; }
        public string AlgoritmoCifrado { get; set; }
        public Nullable<DateTime> FechaBaja { get; set; }
        public Nullable<bool> Activo { get; set; }

        public virtual ICollection<UsuarioSistema> UsuarioSistema { get; set; }
    }
}
