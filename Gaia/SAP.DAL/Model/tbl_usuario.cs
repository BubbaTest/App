using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SAP.DAL.Model
{
    public class tbl_usuario
    {        
        [Key]
        [Required]        
        public int id_usuario { get; set; }
        public Nullable<int> IdPadre { get; set; }
        public string nombre_usuario { get; set; }
        public string contrasenna { get; set; }
        public DateTime dtFechaInsercion { get; set; }
        public Nullable<bool> blnActivo { get; set; }
        public Nullable<bool> DeBaja { get; set; }
        public int intCodEntidadPJ { get; set; }
        public byte[] Avatar { get; set; }
    }
}
