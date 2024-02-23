using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace SAP.DAL.Model
{
    public class RolS
    {
        [Key]
        public int id_rol { get; set; }
        //public string Id_Usuario_Rol { get; set; }
        public string descripcion { get; set; }
    }
}