using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Gaia.DAL.Model
{
    public class catTipoObjeto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal TipoObjetoId { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
    }
}
