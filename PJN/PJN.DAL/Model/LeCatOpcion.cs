using System;
using System.Collections.Generic;
using System.Data.Entity.Hierarchy;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PJN.DAL.Model
{
    public class LeCatOpcion
    {
        public LeCatOpcion()
        {
            OpcionesHijo = new HashSet<LeCatOpcion>();
            //this.RolOpciones = new HashSet<RolOpcion>();
        }

        [Key]
        public HierarchyId OpcionId { get; set; }
        public Int16 Nivel { get; set; }
        public HierarchyId Padre { get; set; }
        public string Opcion { get; set; }
        public string OpcionTipo { get; set; }
        public string Mapeo { get; set; }
        public string DescripcionGeneral { get; set; }
        public Nullable<bool> Activo { get; set; }

        [NotMapped]
        public virtual ICollection<LeCatOpcion> OpcionesHijo { get; set; }

        //public virtual ICollection<Rol> Roles { get; set; }
        public virtual ICollection<LeRolOpcion> RolOpciones { get; set; }
        //public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
