using System;
using System.Collections.Generic;
using System.Data.Entity.Hierarchy;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gaia.DAL.Model
{
    public class catMateriaG
    {
        public catMateriaG()
        {
            this.relEntidadMateria = new HashSet<relEntidadMateria>();
        }
        [Key]
        public string MateriaId { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<relEntidadMateria> relEntidadMateria { get; set; }
    }
}
