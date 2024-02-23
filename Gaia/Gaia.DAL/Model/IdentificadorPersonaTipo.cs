using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gaia.DAL.Model
{
    public class IdentificadorPersonaTipo
    {
        public IdentificadorPersonaTipo()
        {
            this.Usuarios = new HashSet<Usuario>(); 
        }
        [Key]
        public string TipoIdentificadorId { get; set; }
        public string Descripcion { get; set; }
        public string Conexion { get; set; }
        public bool EsEliminado { get; set; }

        public virtual ICollection<Usuario> Usuarios  { get; set; }
    }
}
