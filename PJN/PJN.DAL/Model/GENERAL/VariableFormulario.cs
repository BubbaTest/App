using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace PJN.DAL.Model.GENERAL
{
    public class VariableFormulario
    {
        [Key]
        public string VariableId { get; set; }
        public string Descripcion { get; set; }
        public Nullable<bool> Activo { get; set; }
        public string Clasificacion { get; set; }
        public int CenEnc { get; set; }

        public virtual ICollection<relSeccionVariable> relSeccionVariable { get; set; }
    }
}
