using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAP.DAL.Model
{
    public class catTipoAccion
    {
        [Key]        
        public int IdTipoAccion { get; set; }
        public string DescTipoAccion { get; set; }
    }
}
