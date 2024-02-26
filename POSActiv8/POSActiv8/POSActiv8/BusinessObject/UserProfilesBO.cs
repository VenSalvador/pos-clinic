using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject
{
    public class UserProfilesBO
    {
        public int recordid { get; set; }
        public string networkid { get; set; }
        public string fullname { get; set; }
        public string emailaddress { get; set; }
        public string department { get; set; }
        public DateTime expirationdate { get; set; }
        public int userlevel { get; set; }
        public int userrole { get; set; }
        public int userstatus { get; set; }
        public int loginstatus { get; set; }
        public string ipaddress { get; set; }
    }
}
