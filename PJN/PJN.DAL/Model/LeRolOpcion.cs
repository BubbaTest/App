using System;
using System.Collections.Generic;
using System.Data.Entity.Hierarchy;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PJN.DAL.Model
{
    public class LeRolOpcion
    {
        [Column(Order = 0), Key]
        public string RolId { get; set; }
        [Column(Order = 1), Key]
        public HierarchyId OpcionId { get; set; }
        public bool Default { get; set; }

        [ForeignKey("RolId")]
        public virtual LeRole Rol { get; set; }
        [ForeignKey("OpcionId")]
        public virtual LeCatOpcion Opcion { get; set; }
    }
}
