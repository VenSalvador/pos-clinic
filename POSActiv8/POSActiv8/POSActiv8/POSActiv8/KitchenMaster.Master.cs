using System;
using System.Web;
using DataAccess;
using System.Web.UI;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

using BusinessObject;
using BusinessLogic;
using DataAccess;
using POSActiv8.Classes;
using System.IO;

namespace POSActiv8
{
    public partial class KitchenMaster : System.Web.UI.MasterPage
    {
        baseclass baseclass = new baseclass();
        SystemLogsBL slBL = new SystemLogsBL();
        ErrorLogsBL elBL = new ErrorLogsBL();
        SystemParametersBL sysparamBL = new SystemParametersBL();
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

                ////Check if user session is gone
                //if (Session["userid"] == null)
                //{
                //    Response.Redirect("sessiontimeout.aspx?userid=" + strUserID, false);
                //    Context.ApplicationInstance.CompleteRequest();
                //}

                //this.lblTransactionDate.InnerText = System.DateTime.Now.ToString("dddd, MMMM dd, yyyy");


                //POS Sessions
                System.Data.DataSet dsPOSSESSIONS = new System.Data.DataSet();
                dsPOSSESSIONS = DBHelper.GetData("SELECT TOP 1 SessionCode, SessionStatus, TerminalName, TransactionDate, OpeningAmount, ClosingAmount, PostedBy FROM POSSessions WHERE SessionStatus = '1'");

                if (dsPOSSESSIONS.Tables[0].Rows.Count == 0)
                {
                    this.lblTransactionDate.InnerText = String.Empty;
                }

                else
                {
                    
                    this.lblTransactionDate.InnerText = Convert.ToDateTime(dsPOSSESSIONS.Tables[0].Rows[0]["TransactionDate"]).ToString("dddd, MMMM dd, yyyy");
                }

                dsPOSSESSIONS.Clear();
            }
        }

        //Logout
        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];

            System.Data.DataSet dsUPDATE = new System.Data.DataSet();
            dsUPDATE = DBHelper.GetData("UPDATE UserProfiles SET LoginStatus = 0, DateTimeLogout = GETDATE() WHERE NetworkID= '" + strUserID + "'");
            dsUPDATE.Clear();
            //    //login_attempts = 0, //Removed on 09152020. Will check further if line is needed.

            //System Logs
            strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "User Logout", strUserID + " has successfully logout.", strUserID);

            Response.Redirect("Login.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

    }
}