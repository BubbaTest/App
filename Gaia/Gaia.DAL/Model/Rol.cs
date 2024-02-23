using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gaia.DAL.Model
{
    public class Rol
    {
        public Rol()
        {
            this.UsuarioRolEntidad = new HashSet<UsuarioRolEntidad>();
            this.RolOpciones = new HashSet<RolOpcion>();
        }
        [Key]
        public string RolId { get; set; }
        public string Descripcion { get; set; }
        public Nullable<DateTime> FechaBaja { get; set; }
        public Nullable<bool> Activo { get; set; }

        //public virtual ICollection<Usuario> Usuarios { get; set; }
        //public virtual ICollection<catOpcion> RolOpciones { get; set; }
        //public virtual ICollection<EntidadG> Entidades { get; set; }
        public virtual ICollection<RolOpcion> RolOpciones { get; set; }
        public virtual ICollection<UsuarioRolEntidad> UsuarioRolEntidad { get; set; }
    }
}
