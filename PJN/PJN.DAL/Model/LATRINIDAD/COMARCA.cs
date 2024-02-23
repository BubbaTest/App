using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PJN.DAL.Model.LATRINIDAD
{
    public class COMARCA
    {
        public COMARCA()
        {
            this.Comunidades = new HashSet<COMUNIDADES>();
            //    this.UsuarioSistema = new HashSet<UsuarioSistema>();
        }

        public string Nom_Comarc { get; set; }
        [Key]
        public string Id_Comarca { get; set; }
        public string Id_Municip { get; set; }

        public virtual ICollection<COMUNIDADES> Comunidades { get; set; }
    }
}
