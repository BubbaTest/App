using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PJN.DAL.Model.LATRINIDAD
{
    public class CEPOV
    {
        //public CEPOV()
        //{
        //    this.SECTION7_CARAT_PERSONS = new HashSet<SECTION7_CARAT_PERSONS>();
        //}
        [Key]
        public int GDB_ARCHIVE_OID { get; set; }
        public string s1_q2 { get; set; }
        public string s1_q3 { get; set; }
        public string s1_q5 { get; set; }
        public string s1_q6 { get; set; }
        public string s1_q13 { get; set; }
        public Nullable<DateTime> last_edited_date { get; set; }        
        public int OBJECTID { get; set; }

        //public virtual ICollection<SECTION7_CARAT_PERSONS> SECTION7_CARAT_PERSONS { get; set; }
    }
}
