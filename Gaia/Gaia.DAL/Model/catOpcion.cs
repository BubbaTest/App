using System;
using System.Collections.Generic;
using System.Data.Entity.Hierarchy;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gaia.DAL.Model
{
    public class catOpcion
    {
        //private HierarchyId _hierarchyId;

        public catOpcion()
        {
            OpcionesHijo = new HashSet<catOpcion>();
            this.RolOpciones = new HashSet<RolOpcion>();
        }

        //[NotMapped]
        //private string OpcionIdString { get; set; }

        [Key]
        public HierarchyId OpcionId { get; set; }
        //public HierarchyId OpcionId
        //{
        //    get
        //    {
        //        //if (this.OpcionIdString != null)
        //        //{
        //        //    return HierarchyId.Parse(this.OpcionIdString);
        //        //}
        //        //else
        //        //{
        //        //    return _hierarchyId;
        //        //}
        //        return _hierarchyId;
        //    }

        //    set
        //    {
        //        if (value != null)
        //        {
        //            //Type t = value.GetType();
        //            //if (t.Equals(typeof(string)))
        //            //{
        //            //    _hierarchyId = HierarchyId.Parse(value.ToString());
        //            //}
        //            _hierarchyId = HierarchyId.Parse(value.ToString());
        //            //_hierarchyId = HierarchyId.Parse(this.OpcionIdString);
        //        }
        //    }
        //}
        public Int16 Nivel { get; set; }
        public HierarchyId PadreId { get; set; }
        public string Opcion { get; set; }
        public string OpcionTipo { get; set; }
        public string Mapeo { get; set; }
        public string NombreIcono { get; set; }
        public string DescripcionGeneral { get; set; }
        public Nullable<DateTime> FechaBaja { get; set; }
        public Nullable<bool> Activo { get; set; }

        [NotMapped]
        public virtual ICollection<catOpcion> OpcionesHijo { get; set; }


        //public virtual ICollection<Rol> Roles { get; set; }
        public virtual ICollection<RolOpcion> RolOpciones { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
    //public class PersonConverter : CustomCreationConverter<Person>
    //{
    //    public override Person Create(Type objectType)
    //    {
    //        return new Employee();
    //    }
    //}
}
