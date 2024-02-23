using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAP.DAL.Model
{
    public class catMenu
    {
        [Key]
        public int id_menu { get; set; }
        public string codigo { get; set; }
    }
}
