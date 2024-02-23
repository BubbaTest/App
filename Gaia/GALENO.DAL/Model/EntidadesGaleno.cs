using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace GALENO.DAL.Model
{
    public class EntidadesGaleno
    {
        [Key]
        public Int32 intCodEntidadPJ { get; set; }
        public string strSinonimo { get; set; }
    }
}
