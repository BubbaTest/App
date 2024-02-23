using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAP.DAL.Model
{
    public class catAccion
    {
        [Key]
        public int IdAccion { get; set; }
        public string DescpAccion { get; set; }
    }
}
