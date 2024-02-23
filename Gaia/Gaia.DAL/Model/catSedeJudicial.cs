using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gaia.DAL.Model
{
     public class catSedeJudicial
    {
        [Key]
        public string SedeId { get; set; }
        public string Descripcion { get; set; }
        public string DBServerName { get; set; }        
    }
}
