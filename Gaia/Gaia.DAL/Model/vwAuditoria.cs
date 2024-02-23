using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gaia.DAL.Model
{
    public class vwAuditoria
    {
        [Key]
        public string AuditoriaId { get; set; }
        public string SessionID { get; set; }        
        public string UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public string RolId { get; set; }
        public string EntidadId { get; set; }
        public string DescripcionCorta { get; set; }
        public string Host { get; set; }
        public string PaginaAccedida { get; set; }
        public string NombreControlador { get; set; }
        public string NombreAccion { get; set; }
        public string FechaRegistro { get; set; }
        public string BUSQUEDA { get; set; }        
    }
}
