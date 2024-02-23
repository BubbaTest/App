using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PJN.DAL.Model.LATRINIDAD
{
    public class listaCatalogos
    {
        [Key]
        public string ID { get; set; }
        public string DESCRIPCION { get; set; }
    }
}
