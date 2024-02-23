using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gaia.DAL.Model
{
    public class BusquedaUsuarioGAIA
    {
        [Key]        
        public string USUARIOID { get; set; }
        public string CORREO { get; set; }
        public Nullable<Int64> EMPLEADOID { get; set; }
        public string NOMBRE { get; set; }
        public string CARGO { get; set; }
        public string ENTIDAD { get; set; }
        public string DEPARTAMENTO { get; set; }
        public string MUNICIPIO { get; set; }
        public Nullable<Int32> EntidadId { get; set; }
        public string BUSQUEDA { get; set; }        
    }
}
