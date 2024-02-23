using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Nicarao.DAL.Model.CEPOV
{
    public class CatalogoPais
    {
        [Key]
        public string CodPais { get; set; }
        public string NombrePais { get; set; }
    }
}
