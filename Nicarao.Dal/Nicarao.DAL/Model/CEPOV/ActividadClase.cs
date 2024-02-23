using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Nicarao.DAL.Model.CEPOV
{
    public class ActividadClase
    {        
        public string SECCION { get; set; }
        [Key]
        public string CLASE { get; set; }
        public string DESCRIPCIÓN { get; set; }
    }
}
