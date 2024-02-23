using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Nicarao.DAL.Model.CEPOV
{
    public class CUTP
    {
        [Key]
        public double CodTecnico { get; set; }
        public string NombreTecnico { get; set; }
    }
}
