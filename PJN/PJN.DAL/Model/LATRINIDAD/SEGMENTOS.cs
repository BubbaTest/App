using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PJN.DAL.Model.LATRINIDAD
{
    public class SEGMENTOS
    {
        [Key]
        public int OBJECTID { get; set; }
        public string Id_Segment { get; set; }
        public string Cod_Segmen { get; set; }
        public string Id_Municip { get; set; }
    }
}
