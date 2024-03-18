using System;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Management;
using System.Net;
using System.IO;
using MySql.Data.MySqlClient;

using BusinessObject;
using BusinessLogic;
using DataAccess;
using POSActiv8.Classes;

namespace POSActiv8
{
    public partial class GameXRegistrationMaster : System.Web.UI.MasterPage
    {
        baseclass baseclass = new baseclass();
        SystemLogsBL slBL = new SystemLogsBL();
        ErrorLogsBL elBL = new ErrorLogsBL();
        SequenceBL seqBL = new SequenceBL();
        SystemParametersBL sysparamBL = new SystemParametersBL();
        POSSessionsBO possessBO = new POSSessionsBO();
        POSSessionsBL possessBL = new POSSessionsBL();
        int intEntity;
        string strUserID;
        int intUserLevel;
        int intUserRole;
        string strSystemLogs = string.Empty;
        string strErrorLogs = string.Empty;

        public void Alert(String strMess)
        {
            String strScript = "alert('" + strMess + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "MyScript", strScript, true);
        }

        //Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            if (!IsPostBack)
            {
                if (Session["Mess"] == null)
                { }

                else
                {
                    if (Session["Mess"].ToString().Trim().Length > 0)
                    {
                        Alert(Session["Mess"].ToString());
                        //baseclass.Alert(Session["Mess"].ToString());
                        Session["Mess"] = null;
                    }
                }
              
               

            }
        }

        
        


    }
}