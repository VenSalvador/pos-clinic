using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject
{
    public class TaxBO
    {
        public int recordid { get; set; }
        public string taxname { get; set; }
        public decimal taxamount { get; set; }
        public int recordstatus { get; set; }

    }
}
