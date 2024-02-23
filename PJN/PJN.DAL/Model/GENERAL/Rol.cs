using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PJN.DAL.Model.GENERAL
{
    public class Rol
    {
        public Rol()
        {
            this.UsuarioRol = new HashSet<UsuarioRol>();
        }
        [Key]
        public string RolId { get; set; }
        public string Descripcion { get; set; }
        public Nullable<bool> Activo { get; set; }

        public virtual ICollection<RolOpcion> RolOpciones { get; set; }
        public virtual ICollection<UsuarioRol> UsuarioRol { get; set; }
    }
}
