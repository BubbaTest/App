using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gaia.DAL.Model
{
    public class Auditoria
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public decimal AuditoriaId { get; set; }
        public string SessionID { get; set; }
        public string UsuarioId { get; set; }
        public string RolId { get; set; }
        public string EntidadId { get; set; }
        public string Host { get; set; }
        public string PaginaAccedida { get; set; }
        public Nullable<DateTime> FechaRegistro { get; set; }
        public string NombreControlador { get; set; }
        public string NombreAccion { get; set; }
        public string Parametro { get; set; }
    }
}
