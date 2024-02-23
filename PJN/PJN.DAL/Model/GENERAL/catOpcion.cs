using System;
using System.Collections.Generic;
using System.Data.Entity.Hierarchy;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PJN.DAL.Model.GENERAL
{
    public  class catOpcion
    {
        public catOpcion()
        {
            OpcionesHijo = new HashSet<catOpcion>();
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
        //public Nullable<DateTime> FechaBaja { get; set; }
        public Nullable<bool> Activo { get; set; }

        [NotMapped]
        public virtual ICollection<catOpcion> OpcionesHijo { get; set; }

        //public virtual ICollection<Rol> Roles { get; set; }
        //public virtual ICollection<RolOpcion> RolOpciones { get; set; }
        //public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
