using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PJN.DAL.Model.LATRINIDAD
{
    public class CARACT_ESTAB_ECONOM
    {
        [Key]
        public int GDB_ARCHIVE_OID { get; set; }
        public int es02e00 { get; set; }
        public string es03p01 { get; set; }
        public string es03p02_2 { get; set; }
        public string es03p04_2 { get; set; }
        public Nullable<DateTime> last_edited_date { get; set; }
        public int OBJECTID { get; set; }
    }
}
