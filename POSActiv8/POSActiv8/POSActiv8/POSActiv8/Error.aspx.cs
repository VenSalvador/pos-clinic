using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Data;
using System.Xml;

namespace POSActiv8
{
    public partial class Error : System.Web.UI.Page
    {
        string strUserID;
        int intUserLevel;
        int intUserRole;
        int intCMD;
        string strPageName;

        //Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intCMD = Convert.ToInt32(Request.QueryString["cmd"]);
            strPageName = Request.QueryString["pagename"];

            if (intCMD == 1) //No userid found
            {
                this.lblErrorMessage.InnerText = strUserID.ToUpper().Trim() + " is either missing or not found.";
            }

            else if (intCMD == 2) //User not authorized
            {
                this.lblErrorMessage.InnerText = strUserID.ToUpper().Trim() + " is not authorized to access Fleet";
            }

            else if (intCMD == 3) //User not authorized to access a page
            {
                this.lblErrorMessage.InnerText = strUserID.ToUpper().Trim() + " is not authorized to access the " + strPageName + " page.";
            }

            else
            {
                this.lblErrorMessage.InnerText = strUserID.ToUpper().Trim() + " has encountered a problem.";
            }
        }

        //Button Control
        protected void btnHome_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            Response.Redirect("HomePage.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

    }
}

