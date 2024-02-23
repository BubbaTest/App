using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Nicarao.DAL.Model.CEPOV
{
    public class Ocupacion
    {
        [Key]
        public string ID { get; set; }
        public string DESCRIPCION { get; set; }
    }
}
