using System;
using System.Collections.Generic;
using System.Data.Entity.Hierarchy;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Gaia.DAL.Model
{
    public class UsuarioSistema
    {
        //public UsuarioSistema()
        //{
        //    Sistema = new HashSet<Sistema>();            
        //}
                
        public decimal Id { get; set; }
        [Column(Order = 0), Key]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Por favor, ingrese un nombre de usuario válido")]
        public string UsuarioId { get; set; }
        [Column(Order = 1), Key]
        public string SistemaId { get; set; }
        //[RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Por favor, ingrese una contraseña válida")]
        public string Password { get; set; }
        public string Token { get; set; }
        public Nullable<bool> ResetPassword { get; set; }

        public virtual Sistema Sistema { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
