using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Gaia.DAL.Model
{
    public class catParametro
    {
        public catParametro () {
            //this.Reportes = new HashSet<catReporte>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ParametroId { get; set; }
        public string Descripcion { get; set; }                        
        public bool Activo { get; set; }
        public string Valor { get; set; }


        //[JsonIgnore]
        //public virtual ICollection<catReporte> Reportes { get; set; }
        
        //[NotMapped]
        //public string NombreObjeto { get {
        //        if ( this.Tipo == "select" )
        //        {
        //            return "ddl" + this.CampoId;
        //        }
        //        else {
        //            return "txt" + this.CampoId;
        //        }
        //} }

        //[NotMapped]
        //public string OrderObjeto { get {
        //        int inicio = this.CampoId.Length - 4;
        //        return this.CampoId.Substring(inicio , 2);
        //} }

        //[NotMapped]
        //public string Dependencia { get {
        //        int inicio = this.CampoId.Length -2;
        //        return this.CampoId.Substring(inicio , 2);
        //} }
    }
}
