using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PJN.DAL.Model.GENERAL
{
    public class UsuarioRol
    {
        [Column(Order = 0), Key]
        public string UsuarioId { get; set; }
        [Column(Order = 1), Key]
        public string RolId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid relGUID { get; set; }

        public virtual Rol Rol { get; set; }
        //public virtual ICollection<RolOpcion> RolOpciones { get; set; }

        //public virtual Rol Rol { get; set; }
        //public virtual Usuario Usuario { get; set; }
       
    }
}
