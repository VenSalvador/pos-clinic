using System;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Management;
using System.Net;
using System.IO;

using BusinessObject;
using BusinessLogic;
using DataAccess;
using POSActiv8.Classes;

namespace POSActiv8
{
    public partial class POSMaster : System.Web.UI.MasterPage
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
              
                //Get IP Address of the machine
                string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                this.hfIPAddress.Value = ipAddress;

                if (string.IsNullOrEmpty(ipAddress))
                {
                    ipAddress = Request.ServerVariables["REMOTE_ADDR"];
                }

                this.hfIPAddress.Value = ipAddress;

                //POS Sessions
                System.Data.DataSet dsPOSSESSIONS = new System.Data.DataSet();
                dsPOSSESSIONS = DBHelper.GetData("SELECT TOP 1 SessionCode, SessionStatus, TerminalName, FORMAT(TransactionDate, 'MMMM dd, yyyy') AS TransactionDate, OpeningAmount, ClosingAmount FROM POSSessions WHERE SessionStatus = '1' AND PostedBy = '" + strUserID + "'");

                if (dsPOSSESSIONS.Tables[0].Rows.Count == 0)
                {
                    this.lblTransactionDate.InnerText = String.Empty;
                    this.btnPOSSession.Text = "Start Session";
                }

                else
                {
                    this.lblTransactionDate.InnerText = dsPOSSESSIONS.Tables[0].Rows[0]["TransactionDate"].ToString();
                    this.btnPOSSession.Text = "End Session";
                }

                dsPOSSESSIONS.Clear();

            }
        }

        //POS Session
        protected void btnPOSSession_Click(object sender, EventArgs e)
        {
            this.lblPOSSessionTitle.Text = System.Environment.MachineName;

            //Button Controls
            System.Data.DataSet dsPOSSESSIONS = new System.Data.DataSet();
            dsPOSSESSIONS = DBHelper.GetData("SELECT TOP 1 SessionCode, SessionStatus, TerminalName, TransactionDate, OpeningAmount, ClosingAmount FROM POSSessions WHERE SessionStatus = '1'");

            if (dsPOSSESSIONS.Tables[0].Rows.Count == 0)
            {
                this.lblPOSSessionTitle.Text = "Start Session";

                this.txtOpeningAmount.Enabled = true;
                this.txtOpeningAmount.Text = "0.00";
                this.txtOpeningAmount.Focus();
                this.divClosing.Visible = false;
                this.txtRemarks.InnerText = String.Empty;

                //Button Controls
                this.btnCancel.Visible = true;
                this.btnPost.Visible = true;
                this.btnPost.Text = "Start Session";
                this.lblValidationMessage.InnerText = string.Empty;

                //Show Modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showPOSSession();", true);

            }

            else if (dsPOSSESSIONS.Tables[0].Rows[0]["SessionStatus"].ToString() == "1" && dsPOSSESSIONS.Tables[0].Rows[0]["ClosingAmount"].ToString() == "0.00")
            {
                //Check if there are unposted transactions before closing register
                System.Data.DataSet dsPOINTOFSALE = new System.Data.DataSet();
                dsPOINTOFSALE = DBHelper.GetData("SELECT ControlNumber, TransactionDate, OrderStatus FROM OrderHeader WHERE TransactionDate = '" + Convert.ToDateTime(dsPOSSESSIONS.Tables[0].Rows[0]["TransactionDate"]) + "' AND OrderStatus = '1'");

                if (dsPOINTOFSALE.Tables[0].Rows.Count > 0)
                {
                    //Alert
                    ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('End Session Not Allowed', 'End session for ' + '" + Convert.ToDateTime(dsPOSSESSIONS.Tables[0].Rows[0]["TransactionDate"]).ToString("MMMM dd, yyyy") + "' + ' cannot be posted since there are still unposted items detected in your POS.', 'warning'); ", true);
                }

                else
                {
                    this.lblPOSSessionTitle.Text = "End Session";

                    this.hfSessionCode.Value = dsPOSSESSIONS.Tables[0].Rows[0]["SessionCode"].ToString();

                    this.txtTransactionDate.Value = Convert.ToDateTime(dsPOSSESSIONS.Tables[0].Rows[0]["TransactionDate"]).ToString("yyyy-MM-dd");
                    this.txtOpeningAmount.Enabled = false;
                    this.txtOpeningAmount.Text = Convert.ToDecimal(dsPOSSESSIONS.Tables[0].Rows[0]["OpeningAmount"]).ToString("N2");
                    this.divClosing.Visible = true;
                    this.txtClosingAmount.Text = "0.00";
                    this.txtClosingAmount.Focus();
                    this.txtRemarks.InnerText = String.Empty;

                    //Button Controls
                    this.btnCancel.Visible = true;
                    this.btnPost.Visible = true;
                    this.btnPost.Text = "End Session";

                    this.lblValidationMessage.InnerText = string.Empty;

                    //Show Modal
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showPOSSession();", true);
                }

                dsPOINTOFSALE.Clear();
            }

            else
            {
                //Alert
                ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Session Closed', 'Session for ' + '" + this.txtTransactionDate.Value + "' + ' is already closed.', 'info'); ", true);

                //this.lblRegisterShiftTitle.Text = "Closed Register";

                //this.txtOpeningAmount.Disabled = true;
                //this.txtOpeningAmount.Value = dsREGISTERSHIFT.Tables[0].Rows[0]["OpeningAmount"].ToString();
                //this.divClosing.Visible = true;
                //this.txtClosingAmount.Value = dsREGISTERSHIFT.Tables[0].Rows[0]["ClosingAmount"].ToString();
                //this.txtClosingAmount.Disabled = true;

                ////Button Controls
                //this.btnCancel.Visible = true;
                //this.btnPost.Visible = false;
                //this.btnPost.Text = "Closed Register";
            }

            dsPOSSESSIONS.Clear();


            //Show Modal
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            if (this.btnPost.Text == "Start Session" && this.txtOpeningAmount.Text == String.Empty)
            {
                this.lblValidationMessage.InnerText = "Opening amount must not be empty.";
                this.lblValidationMessage.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showPOSSession();", true);
            }

            else if (this.btnPost.Text == "End Session" && this.txtClosingAmount.Text == string.Empty)
            {
                this.lblValidationMessage.InnerText = "Closing amount must not be empty.";
                this.lblValidationMessage.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showPOSSession();", true);
            }

            else if (this.txtRemarks.InnerText == String.Empty)
            {
                this.lblValidationMessage.InnerText = "Remarks must not be empty.";
                this.lblValidationMessage.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showPOSSession();", true);
            }

            else if (this.txtRemarks.InnerText.Length > 200)
            {
                this.lblValidationMessage.InnerText = "Remarks must not be greater than 200 characters.";
                this.lblValidationMessage.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showPOSSession();", true);
            }

            else
            {
                try
                {
                    var strTerminalName = this.hfIPAddress.Value; //Get IP Address

                    if (this.btnPost.Text == "Start Session") //Start Session
                    {
                        possessBO.sessioncode = seqBL.ControlNumber_Generate(8);
                        possessBO.sessionstatus = 1;
                        possessBO.transactiondate = Convert.ToDateTime(this.txtTransactionDate.Value);
                        possessBO.terminalname = strTerminalName;
                        possessBO.openingamount = Convert.ToDecimal(this.txtOpeningAmount.Text);
                        possessBO.closingamount = 0;
                    }

                    else //End Session
                    {
                        possessBO.sessioncode = this.hfSessionCode.Value;
                        possessBO.sessionstatus = 2;
                        possessBO.transactiondate = Convert.ToDateTime(this.txtTransactionDate.Value);
                        possessBO.terminalname = strTerminalName;
                        possessBO.openingamount = Convert.ToDecimal(this.txtOpeningAmount.Text);
                        possessBO.closingamount = Convert.ToDecimal(this.txtClosingAmount.Text);
                    }

                    possessBO.remarks = this.txtRemarks.Value.Trim();

                    //Post
                    string strPost = possessBL.POS_Sessions_Post(possessBO, strUserID);

                    if (this.btnPost.Text == "Start Session") //Start Session
                    {
                        //System Logs
                        strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "POS Sessions - Start", "Session started at " + strTerminalName + " on " + Convert.ToDateTime(this.txtTransactionDate.Value).ToString("MMMM dd, yyyy"), strUserID);

                        //Redirect
                        ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Session Started', 'Session started at ' + '" + strTerminalName + "' + ' on '+ '" + Convert.ToDateTime(this.txtTransactionDate.Value).ToString("MMMM dd, yyyy") + "', 'success') .then((value) => { window.location.href = 'POS.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole + "'; }); ", true);
                    }

                    else
                    {
                        //System Logs
                        strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "POS Sessions - End", "Session ended at " + strTerminalName + " on " + Convert.ToDateTime(this.txtTransactionDate.Value).ToString("MMMM dd, yyyy"), strUserID);

                        //Logout
                        System.Data.DataSet dsLOGOUT = new System.Data.DataSet();
                        dsLOGOUT = DBHelper.GetData("UPDATE UserProfiles SET LoginStatus = 0, DateTimeLogout = GETDATE() WHERE NetworkID= '" + strUserID + "'");
                        dsLOGOUT.Clear();

                        //Redirect
                        ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Session Ended', 'Session ended at ' + '" + strTerminalName + "' + ' on '+ '" + Convert.ToDateTime(this.txtTransactionDate.Value).ToString("MMMM dd, yyyy") + "', 'success') .then((value) => { window.location.href = 'Login.aspx'; }); ", true);
                    }
                }

                catch (Exception ex)
                {
                    baseclass.Alert(ex.Message);

                    ////Error Message
                    //this.divErrorMessage.Visible = true;
                    //this.lblErrorMessage.InnerText = ex.Message;
                    //strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Register Shift - " + this.btnPost.Text, ex.Message, strUserID);

                    ////Page Title
                    //this.divPageTitle.Visible = false;

                    ////Button Controls
                    //this.divButtonControls.Visible = false;

                    ////Register Shift
                    //this.divRegisterShift.Visible = false;
                }
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