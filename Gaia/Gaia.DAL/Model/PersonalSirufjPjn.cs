using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.DAL.Model
{
    public class PersonalSirufjPjn
    {
        [Key]
        public Int64 ExpEmpleado_ID { get; set; }        
        public string CID { get; set; }
        public string NombreCompleto { get; set; }
        public string SituacionEstatus { get; set; }
        public string CodEmpleado { get; set; }
    }
}
