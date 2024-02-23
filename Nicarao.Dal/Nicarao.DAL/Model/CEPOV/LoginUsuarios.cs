using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;


namespace Nicarao.DAL.Model.CEPOV
{
    public class LoginUsuarios
    {
        [Key]
        public int IdUsuario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public bool Activo { get; set; }
    }
}
