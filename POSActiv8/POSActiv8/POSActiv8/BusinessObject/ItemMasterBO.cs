using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject
{
    public class ItemMasterBO
    {
        public int recordid { get; set; }
        public string itemcode { get; set; }
        public string itemname { get; set; }
        public string itemdescription { get; set; }
        public string itemcategory { get; set; }
        public string itemsubcategory { get; set; }
        public decimal itemprice { get; set; }
        public int itemstatus { get; set; }

    }
}
