using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.BLL.Model
{
    public class PredicateWhere
    {
        [Key]
        public int PredicateWhereId { get; set; }
        public string Valor { get; set; }
        public string FiltroTipo { get; set; }
        public string Columna { get; set; }
        public string OperadorTipo { get; set; }
        public char[] CaraterSplit { get; set; }
    }
}
