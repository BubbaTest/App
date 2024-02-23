using System;
using System.Collections.Generic;
using System.Data.Entity.Hierarchy;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PJN.DAL.Model.GENERAL
{
    public class catCensoEncuesta
    {
        [Key]
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public Nullable<bool> Activo { get; set; }
    }
}
