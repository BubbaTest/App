using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Nicarao.DAL.Model.CEPOV
{
    public class Municipios
    {
        [Key]
        public string CodMunicipio { get; set; }
        public string NomMunicipio { get; set; }
        public string CodDepartamento { get; set; }
    }
}
