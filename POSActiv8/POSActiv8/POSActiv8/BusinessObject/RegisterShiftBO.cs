using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject
{
    public class RegisterShiftBO
    {
        public int recordid { get; set; }
        public string controlnumber { get; set; }
        public DateTime transactiondate { get; set; }
        public int transactiontype { get; set; }
        public decimal openingamount { get; set; }
        public decimal closingamount { get; set; }
        public string remarks { get; set; }
        public int transactionstatus { get; set; }

    }
}
