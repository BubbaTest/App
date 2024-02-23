using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace SAP.DAL.Model
{
    public class catTiposIdentificaciones
    {
        [Key]
        public int IdTipoIdentificacion { get; set; }
        public string NombreTipoIdentificacion { get; set; }
        public bool blnActivo { get; set; }
    }
}
