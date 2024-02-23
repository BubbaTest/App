using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PJN.DAL.Model.LATRINIDAD
{
    public class NoRelRolOpcion
    {
        [Key]
        public string OpcionId { get; set; }
        public string Opcion { get; set; }
    }
}
