using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gaia.DAL.Model
{
    public class relEntidadMateria
    {
        [Column(Order = 0), Key]
        public string EntidadId { get; set; }
        [Column(Order = 1), Key]
        public string MateriaId { get; set; }        

        public virtual EntidadG EntidadG { get; set; }
        public virtual catMateriaG catMateria { get; set; }        
    }
}
