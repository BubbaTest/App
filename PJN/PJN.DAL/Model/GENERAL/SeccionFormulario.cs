using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PJN.DAL.Model.GENERAL
{
    public class SeccionFormulario
    {
        
        public string SeccionId { get; set; }
        public string Descripcion { get; set; }
        public Nullable<bool> Activo { get; set; }
        public int CenEnc { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid relGUID { get; set; }

        public virtual ICollection<relSeccionVariable> relSeccionVariable { get; set; }
    }
}
