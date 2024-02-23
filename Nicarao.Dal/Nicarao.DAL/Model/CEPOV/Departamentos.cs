using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Nicarao.DAL.Model.CEPOV
{
    public class Departamentos
    {
        [Key]
        public string CodDepartamento { get; set; }
        public string NomDepartamento { get; set; }
        [JsonIgnore]
        public virtual ICollection<Municipios> Municipios { get; set; }
    }
}
