using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PJN.DAL.Model.LATRINIDAD
{
    public class SECTION7_CARAT_PERSONS
    {
        [Key]
        public int GDB_ARCHIVE_OID { get; set; }
        public string s07_nombre { get; set; }
        public string s07p23 { get; set; }
        public Nullable<int> s07p23cod { get; set; }
        public Nullable<DateTime> last_edited_date { get; set; }       
        public int OBJECTID { get; set; }
    }
}
