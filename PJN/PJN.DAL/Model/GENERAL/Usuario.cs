using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PJN.DAL.Model.GENERAL
{
    public class Usuario
    {
        //public Usuario()
        //{
        //    this.UsuarioRol = new HashSet<UsuarioRol>();
        ////    this.UsuarioSistema = new HashSet<UsuarioSistema>();
        //}
        [Key]
        [Required]
        [RegularExpression("^[a-zA-Z0-9.]*$", ErrorMessage = "Por favor. Ingrese un Nombre de Usuario Válido")]
        public string UsuarioId { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Password { get; set; }
        public string Correo { get; set; }        
        public Nullable<bool> Activo { get; set; }
        

        //public virtual ICollection<Rol> Roles { get; set; }
        //public virtual ICollection<catOpcion> UsuarioOpciones { get; set; }
        //public virtual ICollection<UsuarioRol> UsuarioRol { get; set; }
        //public virtual ICollection<UsuarioSistema> UsuarioSistema { get; set; }

        //[ForeignKey("TipoIdentificadorId")]
        //public virtual IdentificadorPersonaTipo IdentificadorPersonaTipo { get; set; }
        //public virtual Persona Persona { get; set; }
    }
}
