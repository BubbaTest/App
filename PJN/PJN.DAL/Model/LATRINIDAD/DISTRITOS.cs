using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PJN.DAL.Model.LATRINIDAD
{
    public  class DISTRITOS
    {
        public string Nom_Distri { get; set; }
        public string Id_Municip { get; set; }
        [Key]
        public string Cod_Distri { get; set; }
    }
}
