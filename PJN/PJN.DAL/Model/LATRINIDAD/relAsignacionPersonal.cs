using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PJN.DAL.Model.LATRINIDAD
{
    public class relAsignacionPersonal
    {
        [Column(Order = 0), Key]
        public string JUsuarioId { get; set; }
        [Column(Order = 1), Key]
        public string SUsuarioId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid relGUID { get; set; }
        public string RolId { get; set; }
    }
}
