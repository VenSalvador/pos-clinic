using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;

using BusinessLogic;
using DataAccess;

namespace POSActiv8
{
    public partial class SessionTimeout : System.Web.UI.Page
    {
        SystemLogsBL slBL = new SystemLogsBL();
        string strSystemLogs;

        //Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session.Abandon();

            string strUserID = Request.QueryString["userid"];
            System.Data.DataSet dsUPDATE = new System.Data.DataSet();
            dsUPDATE = DBHelper.GetData("UPDATE UserProfiles SET LoginStatus = 0, DateTimeLogout = GETDATE() WHERE NetworkID = '" + strUserID + "'");
            dsUPDATE.Clear();

            //System Logs
            strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Session Timeout", strUserID + " " + "session has expired.", strUserID);
        }

        //Button Control
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

    }
}