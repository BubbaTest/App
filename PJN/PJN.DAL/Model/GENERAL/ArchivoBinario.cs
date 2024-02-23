using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PJN.DAL.Model.GENERAL
{
    public class ArchivoBinario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ArchivoBinarioId { get; set; }
        //public byte[] Archivo { get; set; }
        public string Nombre { get; set; }
        public string Ext { get; set; }
        public string Archivo64 { get; set; }
        //public string Referencias { get; set; }
        //public string Fecha { get; set; }
        //public DateTime FechaRegistro { get; set; }
        //public bool Activo { get; set; }

    }
}
