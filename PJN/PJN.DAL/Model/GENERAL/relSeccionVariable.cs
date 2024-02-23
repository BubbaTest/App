using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PJN.DAL.Model.GENERAL
{
    public class relSeccionVariable
    {
        [Column(Order = 0), Key]
        public Guid SeccionId { get; set; }
        [Column(Order = 1), Key]
        public string VariableId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid relGUID { get; set; }        
    }
}
