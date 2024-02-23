using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Gaia.DAL.Model
{
    public class Persona
    {
        public Persona()
        {
            this.Usuarios = new HashSet<Usuario>();
        }
        [Key]
        public string Identificador { get; set; }
        public string strNombreCompleto { get; set; }
        public string intCodEntidadPJ { get; set; }
        public string strEntidad { get; set; }
        public string intCodCargo { get; set; }
        public string strCargo { get; set; }
        public string Busqueda { get; set; }
        public string TipoIdentificadorId { get; set; }
        public string UsuarioId { get; set; }
        public string Celular { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
