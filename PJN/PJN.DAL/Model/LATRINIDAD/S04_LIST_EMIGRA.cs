using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PJN.DAL.Model.LATRINIDAD
{
    public class S04_LIST_EMIGRA
    {
        [Key]
        public int GDB_ARCHIVE_OID { get; set; }
        public Nullable<int> s04e00 { get; set; }
        public string s04nombre { get; set; }
        public string s04p04c { get; set; }
        public string s04p06 { get; set; }
        public string s04p06c { get; set; }
        public Nullable<DateTime> last_edited_date { get; set; }
        public int OBJECTID { get; set; }
    }
}
