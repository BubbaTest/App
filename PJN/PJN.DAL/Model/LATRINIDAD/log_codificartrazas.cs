using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PJN.DAL.Model.LATRINIDAD
{
    public class log_codificartrazas
    {        
        public string SUsuarioId { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid relGUID { get; set; }
        public DateTime FechaModifica { get; set; }
    }
}
