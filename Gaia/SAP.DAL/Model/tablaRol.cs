using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace SAP.DAL.Model
{
    public class tablaRol
    {
        [Key]
        public int id_rol { get; set; }        
        public string descripcion { get; set; }
    }
}