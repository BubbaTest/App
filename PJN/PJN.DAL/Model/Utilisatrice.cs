using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PJN.DAL.Model.GENERAL;

namespace PJN.DAL.Model
{
    public class Utilisatrice
    {
        [Key]
        [Required]
        [RegularExpression("^[a-zA-Z0-9.]*$", ErrorMessage = "Por favor. Ingrese un Nombre de Usuario Válido")]
        public string UsuarioId { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Password { get; set; }
        public string Correo { get; set; }
        public Nullable<bool> Activo { get; set; }
        public string Rol { get; set; }
        public string RolDescripcion { get; set; }
        public string VIN { get; set; }

        public virtual ICollection<LeUsuarioRol> UsuarioRol { get; set; }
    }
}
