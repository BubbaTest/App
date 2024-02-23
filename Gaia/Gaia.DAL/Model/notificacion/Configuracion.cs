using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.DAL.Model.notificacion
{
    public class Configuracion
    {
        public Configuracion()
        {
            SMS = new HashSet<SMS>();
            CORREO = new HashSet<CORREO>();
            SIGNALR = new HashSet<SIGNALR>();
        }

        public decimal ConfiguracionId { get; set; }
        [Key]
        public Guid ConfiguracionGuid { get; set; }
        public bool RequiereCorreo { get; set; }
        public bool RequiereSms { get; set; }
        public bool RequiereApp { get; set; }
        public string UsuarioId { get; set; }
        public string RolId { get; set; }
        public string Html { get; set; }
        public bool Desactivado { get; set; }
        public Nullable<DateTime> FechaDesactivado { get; set; }
        public DateTime FechaRegistra { get; set; }
        public string UsuarioRegistraId { get; set; }
        public string EntidadRegistraId { get; set; }
        public bool Activo { get; set; }

        [JsonIgnore]
        public virtual ICollection<SMS> SMS { get; set; }
        [JsonIgnore]
        public virtual ICollection<CORREO> CORREO { get; set; }
        [JsonIgnore]
        public virtual ICollection<SIGNALR> SIGNALR { get; set; }
    }
}
