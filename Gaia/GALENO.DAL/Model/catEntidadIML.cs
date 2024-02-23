using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GALENO.DAL.Model
{
    public class catEntidadIML
    {
        [Key]
        public Int32 IdEntidadIML { get; set; }
        public string strNombreEntidadIML { get; set; }
    }
}
