using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gaia.DAL.Model
{
    
    public class Usuario
    {
        public Usuario()
        {
            this.UsuarioRolEntidad = new HashSet<UsuarioRolEntidad>();
            this.UsuarioSistema = new HashSet<UsuarioSistema>();            
        }
        [Key]
        [Required]
        [RegularExpression("^[a-zA-Z0-9.]*$", ErrorMessage ="Por favor. Ingrese un Nombre de Usuario Válido")]
        public string UsuarioId { get; set; }
        public Nullable<Int64> EmpleadoId { get; set; }
        public string Password { get; set; }
        public string Correo { get; set; }
        public byte[] Avatar { get; set; }
        public string CodMunicipio { get; set; }
        public string EstadoId { get; set; }
        public string TipoIdentificadorId { get; set; }
        public string Identificador { get; set; }
        public Nullable<bool> ResetPassword { get; set; }

        //public virtual ICollection<Rol> Roles { get; set; }
        public virtual ICollection<catOpcion> UsuarioOpciones { get; set; }
        public virtual ICollection<UsuarioRolEntidad> UsuarioRolEntidad { get; set; }
        public virtual ICollection<UsuarioSistema> UsuarioSistema { get; set; }
        
        [ForeignKey("TipoIdentificadorId")]
        public virtual IdentificadorPersonaTipo IdentificadorPersonaTipo { get; set; }
        public virtual Persona Persona { get; set; }
    }
}
