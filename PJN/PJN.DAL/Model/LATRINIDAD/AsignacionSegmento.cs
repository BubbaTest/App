using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;


namespace PJN.DAL.Model.LATRINIDAD
{
    public class AsignacionSegmento
    {
        [Key]
        public string IdSegmento { get; set; }
        public string IDMUNIC { get; set; }
        public string UsuarioId { get; set; }
        public bool Activo { get; set; }
        public string AsignoId { get; set; }
        public int CenEnc { get; set; }
    }    
}
