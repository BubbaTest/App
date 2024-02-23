using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PJN.DAL.Model.LATRINIDAD
{
    public class COMUNIDADES
    {
        public string Nom_Comuni { get; set; }
        public string Id_Comarca { get; set; }
        [Key]
        public string Id_Comunid { get; set; }
    }
}
