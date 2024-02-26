using System;
using System.Web;
using DataAccess;
using System.Data;
using System.Web.UI;
using System.Configuration;
using System.Data.SqlClient;

using BusinessLogic;
using BusinessObject;
using POSActiv8.Classes;
using System.IO;

namespace POSActiv8
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        baseclass baseclass = new baseclass();
        SystemLogsBL slBL = new SystemLogsBL();
        ErrorLogsBL elBL = new ErrorLogsBL();
        UserProfilesBL userprofBL = new UserProfilesBL();
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
                strUserID = Request.QueryString["userid"];

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
                //    Response.Redirect("SessionTimeout.aspx?userid=" + strUserID, false);
                //    Context.ApplicationInstance.CompleteRequest();
                //}
            }
        }

        //Dashboard
        protected void lnkDashboard_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            //Redirect
            Response.Redirect("Dashboard.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        //Reports
        //protected void lnkRegisterShift_Click(object sender, EventArgs e)
        //{
        //    strUserID = Request.QueryString["userid"];
        //    intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
        //    intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

        //    //Redirect
        //    Response.Redirect("RegisterShift.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
        //    Context.ApplicationInstance.CompleteRequest();
        //}

        protected void lnkXReport_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            //Redirect
            Response.Redirect("XZReport.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole + "&transactiontype=" + 1, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void lnkZReport_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            //Redirect
            Response.Redirect("XZReport.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole + "&transactiontype=" + 2, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void lbtnSalesInvoice_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            //Redirect
            Response.Redirect("SalesInvoice.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void lbtnVoidItems_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            //Redirect
            Response.Redirect("VoidItems.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void lbtnCancelledOrders_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            //Redirect
            Response.Redirect("CancelledOrders.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        //Configurations
        protected void lnkFloorLocation_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            //Redirect
            Response.Redirect("FloorLocation.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void lnkTableNames_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            //Redirect
            Response.Redirect("TableNames.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void lnkItemCategory_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            //Redirect
            Response.Redirect("ItemCategory.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void lnkItemSubCategory_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            //Redirect
            Response.Redirect("ItemSubCategory.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void lnkItemMaster_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            //Redirect
            Response.Redirect("ItemMaster.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void lnkDiscounts_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            //Redirect
            Response.Redirect("Discounts.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void lnkTax_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            //Redirect
            Response.Redirect("Tax.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        //Settings
        protected void lnkUserProfiles_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            //Redirect
            Response.Redirect("UserProfiles.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void lnkSystemLogs_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            //Redirect
            Response.Redirect("SystemLogs.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void lnkErrorLogs_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            //Redirect
            Response.Redirect("ErrorLogs.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
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