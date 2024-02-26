using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Diagnostics;

using BusinessObject;
using BusinessLogic;
using DataAccess;

namespace POSActiv8
{
    public partial class HomePage : System.Web.UI.Page
    {
        SystemLogsBL slBL = new SystemLogsBL();
        ErrorLogsBL elBL = new ErrorLogsBL();
        UserProfilesBL upBL = new UserProfilesBL();
        string strUserID;
        int intUserLevel;
        int intUserRole;
        string strSystemLogs;
        string strErrorLogs;
        
        //Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                strUserID = Request.QueryString["userid"];
                intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
                intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);
                //baseclass.UserInformation(strUserID, intUserLevel, intUserRole);

                //Error Message
                this.divErrorMessage.Visible = false;

                //Page Title
                this.divPageTitle.Visible = true;
                
                try
                {
                    //User Profiles
                    using (SqlDataReader drUSERPROFILES = upBL.UserProfiles_View(strUserID, string.Empty))
                    {
                        if (drUSERPROFILES.Read())
                        {
                            this.lblWelcomeMessage.Text = "Welcome, " + drUSERPROFILES["FullName"].ToString();
                            this.lblDateToday.Text = "Today is " + System.DateTime.Now.ToLongDateString();
                            this.lblLoginAttempts.Text = "You have " + drUSERPROFILES["LoginAttempts"].ToString() + " failed login attempt(s)";
                        }
                    }

                    //string message = @"File has been successfully uploaded.  \n \n Tank 387 - Innage 12345 cannot be found in the Low strap table \n \n Tank 387 - Innage 12345 cannot be found in the Low strap table.";
                    //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    //sb.Append("<script type = 'text/javascript'>");
                    //sb.Append("window.onload=function(){");
                    //sb.Append("alert('");
                    //sb.Append(message);
                    //sb.Append("')};");
                    //sb.Append("</script>");
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                }

                catch (Exception ex)
                {
                    //Error Message
                    this.divErrorMessage.Visible = true;
                    this.lblErrorMessage.Text = ex.Message;
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Home Page - Page Load", ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;
                }               
            }
        }

        //Error Message
        protected void btnReload_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            Response.Redirect("HomePage.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

    }
}
