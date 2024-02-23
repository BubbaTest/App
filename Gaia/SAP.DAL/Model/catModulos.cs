using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace SAP.DAL.Model
{
    public class catModulos
    {
        [Key]
        public int IdModulo { get; set; }
        public string DescpModulo { get; set; }
    }
}
