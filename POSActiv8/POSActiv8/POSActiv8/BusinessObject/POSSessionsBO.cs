using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject
{
    public class POSSessionsBO
    {
        public int recordid { get; set; }
        public string sessioncode { get; set; }
        public int sessionstatus { get; set; }
        public string terminalname { get; set; }
        public DateTime transactiondate { get; set; }
        public string userid { get; set; }
        public decimal openingamount { get; set; }
        public decimal closingamount { get; set; }
        public string remarks { get; set; }        

    }
}
