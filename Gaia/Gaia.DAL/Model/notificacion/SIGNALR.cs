using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.DAL.Model.notificacion
{
    public class SIGNALR
    {
        [Key]
        public decimal Signalr { get; set; }
        public Guid ConfiguracionGuid { get; set; }
        public string UsuarioId { get; set; }
        public string RolId { get; set; }
        public string Html { get; set; }
        public DateTime FechaNotificar { get; set; }
        public TimeSpan HoraNotificar { get; set; }
        public bool Notificado { get; set; }
        public Nullable<DateTime> FechaNotificado { get; set; }
        public DateTime FechaRegistra { get; set; }
        public string UsuarioRegistraId { get; set; }
        public string EntidadRegistraId { get; set; }
        public bool Activo { get; set; }

        public virtual Configuracion Configuracion { get; set; }
    }
}
