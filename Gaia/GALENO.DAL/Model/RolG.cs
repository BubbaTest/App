using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace GALENO.DAL.Model
{
    public class RolG
    {
        [Key]        
        public string Id { get; set; }
        public string Descripcion { get; set; }
    }
}
