using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PJN.DAL.Model.GENERAL
{
    public class VARIABLECONTROL
    {
        [Key]
        public string VariableId { get; set; }
        public string Cadena { get; set; }
        public Nullable<bool> EsCatalogo { get; set; }
        public string Catalogo { get; set; }
        public int CenEnc { get; set; }
        public string Vista { get; set; }
    }
}
