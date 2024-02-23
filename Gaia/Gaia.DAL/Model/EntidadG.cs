using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gaia.DAL.Model
{
    public class EntidadG
    {
        public EntidadG()
        {
            this.UsuarioRolEntidad = new HashSet<UsuarioRolEntidad>();
            this.relEntidadMateria = new HashSet<relEntidadMateria>();
        }
        [Key]
        public string EntidadId { get; set; }
        public string Descripcion { get; set; }
        public string CodDepartamento { get; set; }
        public string CodMunicipio { get; set; }
        public string Domicilio { get; set; }
        public string Telefonos { get; set; }
        public string Estado { get; set; }
        public string Circuito { get; set; }
        public string DescripcionCorta { get; set; }
        public string SedeJudicial { get; set; }
        public Nullable<int> intCodEntidadPJ { get; set; }
        public string chrCodEdificio { get; set; }
        public Nullable<DateTime> FechaBaja { get; set; }
        public Nullable<bool> Activo { get; set; }

        //public virtual ICollection<Rol> Roles { get; set; }
        public virtual ICollection<UsuarioRolEntidad> UsuarioRolEntidad { get; set; }
        public virtual ICollection<relEntidadMateria> relEntidadMateria { get; set; }
    }
}
