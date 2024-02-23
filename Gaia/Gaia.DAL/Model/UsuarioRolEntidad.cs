using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gaia.DAL.Model
{
    public class UsuarioRolEntidad
    {
        [Column(Order =0), Key]
        public string UsuarioId { get; set; }
        [Column(Order = 1), Key]
        public string RolId { get; set; }
        [Column(Order = 2), Key]
        public string EntidadId { get; set; }
        public Guid relGUID { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual Rol Rol { get; set; }
        public virtual EntidadG EntidadG { get; set; }
    }
}
