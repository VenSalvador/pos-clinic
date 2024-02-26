using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject
{
    public class PointofSaleBO
    {
        public int recordid { get; set; }
        public string transactioncode { get; set; }
        public string controlnumber { get; set; }
        public string tablecode { get; set; }
        public string categorycode { get; set; }
        public string itemcode { get; set; }
        public decimal itemprice { get; set; }
        public int itemquantity { get; set; }
        public int discounttype { get; set; }
        public decimal discountamount { get; set; }
        public string discountremarks { get; set; }
        public decimal servicecharge { get; set; }
        public decimal totalamount { get; set; }
        public int paymenttype { get; set; }
        public decimal paymentamount { get; set; }

        //Cash
        public decimal changedue { get; set; }

        //Credit Card
        public string cardtype { get; set; }
        public string cardnumber { get; set; }
        public string accountname { get; set; }
        public DateTime expirydate { get; set; }

        //GCash
        public string gcashreferencenumber { get; set; }

        //Void Order
        public string voidauthorizedby { get; set; }

        //Cancel Order
        public int canceltype { get; set; }
        public string cancelauthorizedby { get; set; }
        public string cancelremarks { get; set; }

        public int orderstatus { get; set; }

    }
}
