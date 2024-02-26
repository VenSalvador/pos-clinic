 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;
using System.IO;
//using OfficeOpenXml;
using System.Web.Configuration;
using System.Web.Security;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Data.SqlClient; 

using BusinessObject;
using BusinessLogic;
using DataAccess;

using POSActiv8.Classes;

namespace POSActiv8
{
    public partial class GameXRegLogin : System.Web.UI.Page
    {
        baseclass baseclass = new baseclass();
        SystemLogsBL slBL = new SystemLogsBL();
        ErrorLogsBL elBL = new ErrorLogsBL();
        UserProfilesBO upBO = new UserProfilesBO();
        UserProfilesBL upBL = new UserProfilesBL();
        //systemparametersBL sysparamBL = new systemparametersBL();
        string strSystemLogs;
        string strErrorLogs = string.Empty;

        public string ComputeHash(string input, HashAlgorithm algorithm)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);
            return BitConverter.ToString(hashedBytes);
        }

        //Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            //Error Message
            this.divErrorMessage.Visible = false;

            //Login
            this.divLogin.Visible = true;
            this.txtUserName.Focus();
            this.lblFooter.Text = "Copyright " + System.DateTime.Now.ToString("yyyy") + " NSJ Technologies";

            //Get IP Address of the machine
            string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            this.lblHostName.Text = ipAddress;

            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = Request.ServerVariables["REMOTE_ADDR"];
            }

            this.lblIPAddress.Text = ipAddress;
            this.lblIPAddress.Visible = false;

            //var tanrov = "VP0000000015";
            //tanrov = HttpUtility.UrlEncode(baseclass.Encrypt(tanrov.ToString()));


            ////Files Uploaded //1 localhost or 2 UAT server folder
            //var strFilesUploadedPath = @"C:\Users\ractan\Desktop\";

            ////Save the file uploaded to the FilesUploaded folder
            ////this.fuFileUpload.SaveAs(Path.Combine(strFilesUploadedPath, strFileName));

            ////New File
            //FileInfo strNewFile = new FileInfo(Path.Combine(strFilesUploadedPath, "matdocs_dec_2021.xlsx"));

            ////Create excel package
            //using (var packageSchedule1 = new ExcelPackage(strNewFile))
            //{
            //    //Worksheet
            //    ExcelWorksheet worksheetDTI = packageSchedule1.Workbook.Worksheets[0];

            //    int intRowCount = worksheetDTI.Dimension.Rows;

            //    for (int i = 2; i <= intRowCount; i++) //Start at row 2
            //    {
            //        //Inputs
            //        var budat = Convert.ToDateTime(worksheetDTI.Cells[i, 1].Value);
            //        string mjahr = worksheetDTI.Cells[i, 2].Value.ToString().Trim();
            //        string bwart = worksheetDTI.Cells[i, 3].Value.ToString().Trim();
            //        string matnr = worksheetDTI.Cells[i, 4].Value.ToString().Trim();
            //        string werks = worksheetDTI.Cells[i, 5].Value.ToString().Trim();
            //        string lgort = worksheetDTI.Cells[i, 6].Value.ToString().Trim();
            //        string shkzg = worksheetDTI.Cells[i, 7].Value.ToString().Trim();
            //        string shnumber = string.Empty;

            //        if (worksheetDTI.Cells[i, 8].Value == null)
            //        {
            //            shnumber = string.Empty;
            //        }

            //        else
            //        {
            //            shnumber = worksheetDTI.Cells[i, 8].Value.ToString().Trim();
            //        }

            //        string umwrk = string.Empty;

            //        if (worksheetDTI.Cells[i, 9].Value == null)
            //        {
            //            umwrk = string.Empty;
            //        }

            //        else
            //        {
            //            umwrk = worksheetDTI.Cells[i, 9].Value.ToString().Trim();
            //        }

            //        string umlgo = string.Empty;

            //        if (worksheetDTI.Cells[i, 10].Value == null)
            //        {
            //            umlgo = string.Empty;
            //        }

            //        else
            //        {
            //            umlgo = worksheetDTI.Cells[i, 10].Value.ToString().Trim();
            //        }

            //        string klair = worksheetDTI.Cells[i, 11].Value.ToString().Trim();
            //        string kl15 = worksheetDTI.Cells[i, 12].Value.ToString().Trim();
            //        string bbl = worksheetDTI.Cells[i, 13].Value.ToString().Trim();
            //        string bbl60 = worksheetDTI.Cells[i, 14].Value.ToString().Trim();
            //        string kg = worksheetDTI.Cells[i, 15].Value.ToString().Trim();
            //        string mto = worksheetDTI.Cells[i, 16].Value.ToString().Trim();

            //        System.Data.DataSet ds = new System.Data.DataSet();

            //        ds = DBHelper.GetData("INSERT INTO MATDOCS " +
            //                              "(budat, " +
            //                              "mjahr, " +
            //                              "bwart, " +
            //                              "matnr, " +
            //                              "werks, " +
            //                              "lgort, " +
            //                              "shkzg, " +
            //                              "shnumber, " +
            //                              "umwrk, " +
            //                              "umlgo, " +
            //                              "kl, " +
            //                              "kl1, " +
            //                              "bbl, " +
            //                              "bb6, " +
            //                              "kg, " +
            //                              "mto) " +
            //                              "VALUES " +
            //                              "('" + budat + "', " +
            //                              "'" + mjahr + "', " +
            //                              "'" + bwart + "', " +
            //                              "'" + matnr + "', " +
            //                              "'" + werks + "', " +
            //                              "'" + lgort + "', " +
            //                              "'" + shkzg + "', " +
            //                              "'" + shnumber + "', " +
            //                              "'" + umwrk + "', " +
            //                              "'" + umlgo + "', " +
            //                              "'" + klair + "', " +
            //                              "'" + kl15 + "', " +
            //                              "'" + bbl + "', " +
            //                              "'" + bbl60 + "', " +
            //                              "'" + kg + "', " +
            //                              "'" + mto + "')");
            //    }

            //}

            //baseclass.Alert("WTF");

            //using (DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://pdc.nsjbi.com"))
            //{
            //    using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "NSJBI.COM"))
            //    {
            //        var user = UserPrincipal.Current.SamAccountName;

            //    }
            //}



            //var test = RFCMethods.RFC_GetInventory("1000", "1101");

            //var test = RFCMethods.RFC_GetMBEWH(03, 2022, "ractan");

        }

        //Button Control
        protected void btnLogin_Click(object sender, System.EventArgs e)
        {
            //Production : use LDAP as login
            string strUserID = this.txtUserName.Text.Trim();
            string strPassword = this.txtPassword.Text.Trim();
            string strUserPassword = ComputeHash(this.txtPassword.Text, new MD5CryptoServiceProvider());

            if (this.txtUserName.Text == string.Empty && this.txtPassword.Text == string.Empty)
            {
                //Alert
                ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Required Field', 'User ID and password must not be empty.', 'warning'); ", true);
            }

            else if (this.txtUserName.Text == string.Empty)
            {
                //Alert
                ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Required Field', 'User ID must not be empty.', 'warning'); ", true);
            }

            else if (this.txtPassword.Text == string.Empty)
            {
                //Alert
                ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Required Field', 'Password must not be empty.', 'warning'); ", true);
            }

            else
            {
                //UserProfiles
                System.Data.DataSet dsUSERPROFILES = new System.Data.DataSet();
                dsUSERPROFILES = DBHelper.GetData("SELECT NetworkID, FullName, UserLevel, UserRole FROM UserProfiles WHERE NetworkID = '" + strUserID + "'");

                if (dsUSERPROFILES.Tables[0].Rows.Count == 0)
                {
                    //Alert
                    ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Unknown User', 'Your user id is not recognized as a valid user to access POS', 'warning'); ", true);
                }

                else
                {
                    //UserProfiles
                    System.Data.DataSet dsUSERPROFILE = new System.Data.DataSet();
                    dsUSERPROFILE = DBHelper.GetData("SELECT NetworkID, FullName, UserLevel, UserRole FROM UserProfiles WHERE NetworkID = '" + strUserID + "' AND UserPassword = '" + strUserPassword + "'");

                    if (dsUSERPROFILE.Tables[0].Rows.Count == 0)
                    {
                        //Alert
                        ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Invalid Credentials', 'The user id or password is incorrect. Please try again.', 'warning'); ", true);
                        return;
                    }

                    else
                    {
                        //User Profiles
                        using (SqlDataReader drUSERPROFILES = upBL.UserProfiles_View(strUserID, string.Empty))
                        {
                            if (drUSERPROFILES.Read())
                            {
                                if (Convert.ToInt32(drUSERPROFILES["UserStatus"]) == 0) //User Inactive
                                {
                                    //Alert
                                    ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('User Inactive', 'Your user id is currently inactive.', 'warning'); ", true);
                                }

                                //else if (Convert.ToInt32(drUSERPROFILES["LoginStatus"]) == 2) //User Locked
                                //{
                                //    //Alert
                                //    ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('User Locked', 'Your user id is locked.', 'warning'); ", true);
                                //}

                                //else if (Convert.ToInt32(drUSERPROFILES["LoginStatus"]) == 1) //User Already Login
                                //{
                                //    //Alert
                                //    ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('User Already Login', 'Your user id is currently logged in.', 'warning'); ", true);
                                //}

                                else
                                {
                                    //Check if userid exist
                                    System.Data.DataSet dsUF = new System.Data.DataSet();
                                    dsUF = DBHelper.GetData("SELECT NetworkID, FullName, UserLevel, UserRole FROM UserProfiles WHERE NetworkID = '" + strUserID + "'");

                                    //Update user login details
                                    System.Data.DataSet dsUPDATE = new System.Data.DataSet();
                                    dsUPDATE = DBHelper.GetData("UPDATE UserProfiles SET DateTimeLogin = GETDATE(), LoginStatus = 1, LoginAttempts = '" + Convert.ToInt32(this.lblLoginAttempts.InnerText) + "', IPAddress = '" + this.lblIPAddress.Text.Trim() + "' WHERE NetworkID = '" + strUserID + "'");
                                    dsUPDATE.Clear();

                                    //System Logs
                                    strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "User Login", strUserID + " " + "has successfully login with " + this.lblLoginAttempts.InnerText + " invalid login attempts using " + this.lblIPAddress.Text.Trim() + ".", strUserID);

                                    if (Convert.ToInt32(dsUF.Tables[0].Rows[0]["UserRole"]) == 1 || Convert.ToInt32(dsUF.Tables[0].Rows[0]["UserRole"]) == 2 || Convert.ToInt32(dsUF.Tables[0].Rows[0]["UserRole"]) == 3) //SysAd //Manager //Supervisor
                                    {
                                        Session["userid"] = strUserID;
                                        Response.Redirect("Dashboard.aspx?userid=" + strUserID + "&userlevel=" + Convert.ToInt32(dsUF.Tables[0].Rows[0]["UserLevel"]) + "&userrole=" + Convert.ToInt32(dsUF.Tables[0].Rows[0]["UserRole"]), false);
                                        Context.ApplicationInstance.CompleteRequest();
                                    }

                                    else //Waiter //Cashier
                                    {
                                        Session["userid"] = strUserID;
                                        Response.Redirect("GameXRegistration.aspx?userid=" + strUserID, false);
                                        Context.ApplicationInstance.CompleteRequest();
                                    }


                                }
                            }
                        }

                        dsUSERPROFILE.Clear();
                    }

                    dsUSERPROFILES.Clear();
                }
            }

                


                

            //    //LDAP
            //    using (DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://pdc.nsjbi.com"))
            //    {
            //        bool ValidUser = false;

            //        using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "NSJBI.COM"))
            //        {
            //            //Validate the credentials
            //            ValidUser = pc.ValidateCredentials(strUserID, strPassword);

            //            if (ValidUser == true)
            //            {
            //                //User Profiles
            //                using (SqlDataReader drUSERPROFILES = upBL.UserProfiles_View(strUserID, string.Empty))
            //                {
            //                    if (drUSERPROFILES.HasRows)
            //                    {
            //                        if (drUSERPROFILES.Read())
            //                        {
            //                            if (Convert.ToInt32(drUSERPROFILES["UserStatus"]) == 0) //User Inactive
            //                            {
            //                                ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('User Inactive', 'Your user id is currently inactive.', 'warning'); ", true);
            //                            }

            //                            else if (Convert.ToInt32(drUSERPROFILES["LoginStatus"]) == 2) //User Locked
            //                            {
            //                                ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('User Locked', 'Your user id is currently locked.', 'warning'); ", true);
            //                            }

            //                            else if (Convert.ToInt32(drUSERPROFILES["LoginStatus"]) == 1) //User Already Login
            //                            {
            //                                ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('User Already Login', 'Your user id is currently logged in.', 'warning'); ", true);
            //                            }

            //                            else
            //                            {
            //                                //Check if userid exist
            //                                System.Data.DataSet dsUF = new System.Data.DataSet();
            //                                dsUF = DBHelper.GetData("SELECT NetworkID, FullName, UserLevel, UserRole FROM UserProfiles WHERE NetworkID = '" + strUserID + "'");

            //                                //Update user login details
            //                                System.Data.DataSet dsUPDATE = new System.Data.DataSet();
            //                                dsUPDATE = DBHelper.GetData("UPDATE UserProfiles SET DateTimeLogin = GETDATE(), LoginStatus = 1, LoginAttempts = '" + Convert.ToInt32(this.lblLoginAttempts.InnerText) + "', IPAddress = '" + this.lblIPAddress.Text.Trim() + "' WHERE NetworkID = '" + strUserID + "'");
            //                                dsUPDATE.Clear();

            //                                //System Logs
            //                                strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "User Login", strUserID + " " + "has successfully login with " + this.lblLoginAttempts.InnerText + " invalid login attempts using " + this.lblIPAddress.Text.Trim() + ".", strUserID);

            //                                Session["userid"] = strUserID;
            //                                Response.Redirect("POS.aspx?userid=" + strUserID + "&userlevel=" + Convert.ToInt32(dsUF.Tables[0].Rows[0]["UserLevel"]) + "&userrole=" + Convert.ToInt32(dsUF.Tables[0].Rows[0]["UserRole"]), false);
            //                                Context.ApplicationInstance.CompleteRequest();



            //                                //Show Modal
            //                                //ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalUserDisclaimer').modal('show');</script>", false);


            //                                // this.btnAcceptDisclaimer.Focus();
            //                                // this.popupdisclaimer.Show();

            //                                ////User Disclaimer
            //                                //using (SqlDataReader drUSERDISCLAIMER = sysparamBL.SystemParameters_View(20))
            //                                //{
            //                                //    if (drUSERDISCLAIMER.Read())
            //                                //    {
            //                                //        this.lblUserDisclaimer.Text = drUSERDISCLAIMER["parameter_description"].ToString();
            //                                //    }
            //                                //}
            //                            }
            //                        }
            //                    }

            //                    else
            //                    {
            //                        ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Unknown User', 'Your user id is not recognized as a valid user to access Bank Financing', '', 'warning'); ", true);
            //                    }

            //                }
            //            }

            //            else
            //            {
            //                int intLoginAttempts = Convert.ToInt32(this.lblLoginAttempts.InnerText);

            //                //Count the invalid login attempts
            //                intLoginAttempts = intLoginAttempts + 1;
            //                this.lblLoginAttempts.InnerText = intLoginAttempts.ToString();

            //                if (intLoginAttempts == 3) //If invalid login attempts reached 3 then lock user id
            //                {
            //                    System.Data.DataSet dsUSERPROFILES = new System.Data.DataSet();
            //                    dsUSERPROFILES = DBHelper.GetData("UPDATE UserProfiles SET LoginStatus = 2, LoginAttempts = '" + Convert.ToInt32(this.lblLoginAttempts.InnerText) + "' WHERE NetworkID = '" + strUserID + "'");
            //                    dsUSERPROFILES.Clear();

            //                    //System Logs
            //                    //strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "User Locked", strUserID + " " + "has been locked with " + this.lblLoginAttempts.InnerText + " invalid login attempts using " + this.lblIPAddress.Text.Trim() + ".", strUserID);

            //                    //Alert
            //                    //this.lblAlertTitle.InnerText = "User Locked";
            //                    //this.lblAlertContent.InnerText = "Your user id will be lock because you have encoded your password incorrectly for more than 3 times. Please contact your system administrator.";
            //                    //this.popupalert.Show();
            //                    //this.lblLoginAttempts.InnerText = "0";

            //                    ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('User Locked', '', 'warning'); ", true);
            //                }

            //                else
            //                {
            //                    ////Alert
            //                    //this.lblAlertTitle.InnerText = "Invalid Credentials";
            //                    //this.lblAlertContent.InnerText = "The user id or password is incorrect. Please try again.";
            //                    //this.popupalert.Show();

            //                    ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Invalid Credentials', 'The user id or password is incorrect. Please try again.', 'warning'); ", true);
            //                    return;
            //                }
            //            }
            //        }
            //    }
            //}



            //UAT
            //Use of SQL Login

            ////Username and password
            //string strUserName = this.txtUserName.Text.ToUpper().Trim();
            //string strUserPassword = ComputeHash(this.txtPassword.Text.ToUpper().Trim(), new MD5CryptoServiceProvider());
            //string strUserIP = Request.UserHostAddress.ToUpper().Trim();

            //if (this.txtUserName.Text == string.Empty)
            //{
            //    this.lblAlertTitle.InnerText = "Invalid Credentials";
            //    this.lblAlertContent.InnerText = "User ID must not be empty";
            //    this.popupalert.Show();
            //}

            //else if (this.txtPassword.Text == string.Empty)
            //{
            //    this.lblAlertTitle.InnerText = "Invalid Credentials";
            //    this.lblAlertContent.InnerText = "Password must not be empty";
            //    this.popupalert.Show();
            //}

            //else
            //{
            //    //Userfile
            //    System.Data.DataSet dsUF, dsUPDATE = new System.Data.DataSet();
            //    dsUF = DBHelper.GetData("SELECT network_id, user_password, full_name, sol_id, user_level, user_role, user_access, user_status, first_log, password_change FROM userfile WHERE network_id = '" + strUserName + "' ");

            //    if (dsUF.Tables[0].Rows.Count == 0)
            //    {
            //        this.lblAlertTitle.InnerText = "Invalid Account";
            //        this.lblAlertContent.InnerText = strUserName + " " + "is not authorized to access CorpBonds";
            //        this.popupalert.Show();
            //    }

            //    else
            //    {
            //        string strUserID = dsUF.Tables[0].Rows[0]["network_id"].ToString();
            //        string strUserPass = dsUF.Tables[0].Rows[0]["user_password"].ToString();
            //        int intLoginAttempt = Convert.ToInt32(this.lblLoginAttempt.InnerText);

            //        if (string.Equals(strUserPassword, strUserPass) == false)
            //        {
            //            //Count the login attempt made by the user
            //            intLoginAttempt = intLoginAttempt + 1;
            //            this.lblLoginAttempt.InnerText = intLoginAttempt.ToString();

            //            if (intLoginAttempt == 4)
            //            {
            //                //Alert
            //                this.lblAlertTitle.InnerText = "Invalid Credentials";
            //                this.lblAlertContent.InnerText = "This user has attempted to login incorrectly 3 times. This userid will be blocked.";
            //                this.popupalert.Show();
            //                this.lblLoginAttempt.InnerText = "0";

            //                //Block user access
            //                dsUPDATE = DBHelper.GetData("UPDATE userfile SET user_access = 1 WHERE network_id = '" + strUserName + "'");
            //                dsUPDATE.Clear();
            //            }

            //            else
            //            {
            //                //Alert
            //                this.lblAlertTitle.InnerText = "Invalid Credentials";
            //                this.lblAlertContent.InnerText = "The password is incorrect. Please try again";
            //                this.popupalert.Show();
            //            }
            //        }

            //        else //If user id and password are correct
            //        {
            //            int intSolID = Convert.ToInt32(dsUF.Tables[0].Rows[0]["sol_id"]);
            //            int intUserLevel = Convert.ToInt32(dsUF.Tables[0].Rows[0]["user_level"]);
            //            int intUserRole = Convert.ToInt32(dsUF.Tables[0].Rows[0]["user_role"]);
            //            int intUserAccess = Convert.ToInt32(dsUF.Tables[0].Rows[0]["user_access"]);
            //            int intUserStatus = Convert.ToInt32(dsUF.Tables[0].Rows[0]["user_status"]);
            //            int intFirstLog = Convert.ToInt32(dsUF.Tables[0].Rows[0]["first_log"]);
            //            int intPasswordChange = Convert.ToInt32(dsUF.Tables[0].Rows[0]["password_change"]);

            //            if (intUserAccess == 1)
            //            {
            //                //Alert
            //                this.lblAlertTitle.InnerText = "Invalid Credentials";
            //                this.lblAlertContent.InnerText = "This user id is blocked.";
            //                this.popupalert.Show();
            //            }

            //            else if (intFirstLog == 0)
            //            {
            //                //First Log
            //                dsUPDATE = DBHelper.GetData("UPDATE userfile SET first_log = 1 WHERE network_id = '" + strUserName + "'");
            //                dsUPDATE.Clear();

            //                Session["Mess"] = "Welcome new user. Please change your password.";
            //                Response.Redirect("changepassword.aspx?userid=" + strUserID + "&cmd=" + "1");
            //                Context.ApplicationInstance.CompleteRequest();
            //            }

            //            else if (intPasswordChange == 0)
            //            {
            //                Session["Mess"] = "Welcome new user. Please change your password.";
            //                Response.Redirect("changepassword.aspx?userid=" + strUserID + "&cmd=" + "1");
            //                Context.ApplicationInstance.CompleteRequest();
            //            }

            //            else
            //            {
            //                // this.popupdisclaimer.Show();

            //                //update user details
            //                dsUPDATE = DBHelper.GetData("UPDATE userfile " +
            //                                            "SET date_time_login = NOW(), " +
            //                                            "user_status = 1 " +
            //                                            "WHERE network_id = '" + strUserName + "' " +
            //                                            "AND user_password = '" + strUserPassword + "' ");
            //                dsUPDATE.Clear();

            //                Response.Redirect("dashboard.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole + "&solid=" + intSolID, false);
            //                Context.ApplicationInstance.CompleteRequest();
            //            }
            //        }
            //    }

            //    dsUF.Clear();
            //}
        }

        //Disclaimer
        protected void btnCancelDisclaimer_Click(object sender, EventArgs e)
        {
            //Response.Redirect("index.aspx", false);
            //Context.ApplicationInstance.CompleteRequest();
        }

        protected void btnAcceptDisclaimer_Click(object sender, EventArgs e)
        {
            //string strUserID = this.txtUserName.Text.Trim();
           
            ////Check if userid exist
            //System.Data.DataSet dsUF = new System.Data.DataSet();
            //dsUF = DBHelper.GetData("SELECT NetworkID, FullName, UserLevel, UserRole FROM UserProfiles WHERE NetworkID = '" + strUserID + "'");

            ////Update user login details
            //System.Data.DataSet dsUPDATE = new System.Data.DataSet();
            //dsUPDATE = DBHelper.GetData("UPDATE UserProfiles SET DateTimeLogin = GETDATE(), LoginStatus = 1, LoginAttempts = '" + Convert.ToInt32(this.lblLoginAttempts.InnerText) + "', IPAddress = '" + this.lblIPAddress.Text.Trim() + "' WHERE NetworkID = '" + strUserID + "'"); 
            //dsUPDATE.Clear();

            ////System Logs
            //strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "User Login", strUserID + " " + "has successfully login with " + this.lblLoginAttempts.InnerText + " invalid login attempts using " + this.lblIPAddress.Text.Trim() + ".", strUserID);

            //Session["userid"] = strUserID;
            //Response.Redirect("homepage.aspx?userid=" + strUserID + "&userlevel=" + Convert.ToInt32(dsUF.Tables[0].Rows[0]["UserLevel"]) + "&userrole=" + Convert.ToInt32(dsUF.Tables[0].Rows[0]["UserRole"]), false);
            //Context.ApplicationInstance.CompleteRequest();
        }

        //Error Message
        protected void btnReload_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        //protected void test_Click(object sender, EventArgs e)
        //{
        //    tankageinventoryBO tankinvBO = new tankageinventoryBO();
        //    tankageinventoryBL tankinvBL = new tankageinventoryBL();
        //    tanksBL tanksBL = new tanksBL();
        //    DlmcObject dlmcobject = new DlmcObject();
        //    DlmcParamObject dlmcparamobject = new DlmcParamObject();
        //    DlmcCalculate dlmccalculate = new DlmcCalculate();
        //    string strUserID = "ractan";


        //    try
        //    { 

        //    //Tankage Inventory
        //    using (SqlDataReader drTANKAGEINVENTORY = tankinvBL.TankageInventory_View(0, "101", "11/17/2021"))
        //    {
        //        while (drTANKAGEINVENTORY.Read())
        //        {
        //            //Inputs
        //            var tankageid = drTANKAGEINVENTORY["tankage_id"].ToString();
        //            var inventorydate = Convert.ToDateTime(drTANKAGEINVENTORY["inventory_date"]);
        //            var tanknumber = drTANKAGEINVENTORY["tank_number"].ToString().Trim();
        //            decimal innage = Math.Round(Convert.ToDecimal(drTANKAGEINVENTORY["innage"]), 0);
        //            decimal water = Math.Round(Convert.ToDecimal(drTANKAGEINVENTORY["water"]), 0);
        //            decimal tanktemperature = Math.Round(Convert.ToDecimal(drTANKAGEINVENTORY["tank_temperature"]), 2);
        //            decimal tankdensity = Math.Round(Convert.ToDecimal(drTANKAGEINVENTORY["density_15"]), 2);
        //            decimal bswpercent = Math.Round(Convert.ToDecimal(drTANKAGEINVENTORY["bsw_percent"]), 2);
        //            decimal vaporpressure = Math.Round(Convert.ToDecimal(drTANKAGEINVENTORY["vapor_pressure"]), 0);
        //            decimal vaportemperature = Math.Round(Convert.ToDecimal(drTANKAGEINVENTORY["vapor_temperature"]), 2);

        //            //Tank Information
        //            tankinvBO.tankageid = Convert.ToInt32(tankageid);
        //            tankinvBO.inventorydate = Convert.ToDateTime(inventorydate.ToString("yyyy-MM-dd"));
        //            tankinvBO.tanknumber = tanknumber;
        //            tankinvBO.remarks = string.Empty;

        //            //Tank Details
        //            using (SqlDataReader drTANKDETAILS = tanksBL.Tanks_View(1, tanknumber))
        //            {
        //                if (drTANKDETAILS.Read())
        //                {
        //                    //Strap Table
        //                    tankinvBO.straptable = Convert.ToInt32(drTANKDETAILS["strap_table"]);

        //                    //LPG and Propylene products
        //                    if (drTANKDETAILS["product_description"].ToString() == "LPG" || drTANKDETAILS["product_description"].ToString() == "PROPYLENE")
        //                    {
        //                        //For turnaround tanks, zero out all values
        //                        if (innage.ToString() == "0" || innage.ToString() == "0.00")
        //                        {
        //                            decimal decZeroOut = 0;

        //                            //Calibration Table
        //                            tankinvBO.innage = decZeroOut;
        //                            tankinvBO.innagevolume = decZeroOut;
        //                            tankinvBO.water = decZeroOut;
        //                            tankinvBO.watervolume = decZeroOut;
        //                            tankinvBO.tanktemperature = decZeroOut;
        //                            tankinvBO.density15 = decZeroOut;
        //                            tankinvBO.bswpercent = decZeroOut;
        //                            tankinvBO.vaporpressure = decZeroOut;
        //                            tankinvBO.vaportemperature = decZeroOut;
        //                            tankinvBO.litersair = decZeroOut;
        //                            tankinvBO.apicorrection = decZeroOut;
        //                            tankinvBO.bswvolume = decZeroOut;
        //                            tankinvBO.netlitersair = decZeroOut;
        //                            tankinvBO.vcfliters = decZeroOut;
        //                            tankinvBO.liters15 = decZeroOut;
        //                            tankinvBO.vcfkilograms = decZeroOut;
        //                            tankinvBO.kg15 = decZeroOut;
        //                            tankinvBO.vcfbarrels = decZeroOut;
        //                            tankinvBO.bbl60 = decZeroOut;

        //                            //Liquid
        //                            tankinvBO.litersairliquid = decZeroOut;
        //                            tankinvBO.bblsairliquid = decZeroOut;
        //                            tankinvBO.bbl60liquid = decZeroOut;
        //                            tankinvBO.liters15liquid = decZeroOut;
        //                            tankinvBO.kg15liquid = decZeroOut;

        //                            //Vapor
        //                            tankinvBO.litersairvapor = decZeroOut;
        //                            tankinvBO.bblsairvapor = decZeroOut;
        //                            tankinvBO.bbl60vapor = decZeroOut;
        //                            tankinvBO.liters15vapor = decZeroOut;
        //                            tankinvBO.kg15vapor = decZeroOut;

        //                            //Total
        //                            tankinvBO.totalliters15 = decZeroOut;
        //                            tankinvBO.totalkg15 = decZeroOut;

        //                            //Net Summary
        //                            tankinvBO.klair = decZeroOut;
        //                            tankinvBO.kl15 = decZeroOut;
        //                            tankinvBO.bblair = decZeroOut;
        //                            tankinvBO.mto = decZeroOut;
        //                        }

        //                        else
        //                        {
        //                            decimal vcflitersfrom = 0;
        //                            decimal vcflitersto = 0;
        //                            decimal vcfliters = 0;

        //                            //Check if inputted tank temperature is out of range in ASTM Table34 (-20 to 120)
        //                            System.Data.DataSet dsASTMTABLE34Temperature = new System.Data.DataSet();
        //                            dsASTMTABLE34Temperature = DBHelper.GetData("SELECT distinct tank_temperature from ASTMTable34 WHERE tank_temperature = '" + baseclass.TemperatureConversion(1, Convert.ToDecimal(tanktemperature.ToString())) + "' ");

        //                            if (dsASTMTABLE34Temperature.Tables[0].Rows.Count == 0)
        //                            {
        //                                //string uploaderror = "● Tank " + tanknumber + " - Tank temperature &deg;c is out of range (Observed temperature &deg;f is from -20 to 120 only).";
        //                                //strUploadError = strUploadError + uploaderror + "<br>";
        //                            }

        //                            else
        //                            {
        //                                //Check if actual density is out of range in ASTM Table34, then use interpolation method to get the value of the VCF
        //                                System.Data.DataSet dsASTMTABLE34 = new System.Data.DataSet();
        //                                dsASTMTABLE34 = DBHelper.GetData("SELECT vcf_value from ASTMTable34 WHERE tank_temperature = '" + baseclass.TemperatureConversion(1, Convert.ToDecimal(tanktemperature.ToString())) + "' AND '" + ((Convert.ToDecimal(tankdensity.ToString())) / 1000) + "' BETWEEN density_from AND density_to");

        //                                if (dsASTMTABLE34.Tables[0].Rows.Count == 0)
        //                                {
        //                                    //Density From
        //                                    decimal densityfrom = Decimal.Truncate(Convert.ToDecimal(tankdensity.ToString())); //Remove decimal
        //                                    densityfrom = (densityfrom / 1000);

        //                                    using (SqlDataReader drDENSITYFROM = tankinvBL.Table34VCF_View(baseclass.TemperatureConversion(1, Convert.ToDecimal(tanktemperature.ToString())), densityfrom))
        //                                    {
        //                                        if (drDENSITYFROM.Read())
        //                                        {
        //                                            vcflitersfrom = Convert.ToDecimal(drDENSITYFROM["vcf_value"]);
        //                                        }
        //                                    }

        //                                    //Density To
        //                                    decimal densityto = (densityfrom + Convert.ToDecimal(0.010));
        //                                    using (SqlDataReader drDENSITYTO = tankinvBL.Table34VCF_View(baseclass.TemperatureConversion(1, Convert.ToDecimal(tanktemperature.ToString())), densityto))
        //                                    {
        //                                        if (drDENSITYTO.Read())
        //                                        {
        //                                            vcflitersto = Convert.ToDecimal(drDENSITYTO["vcf_value"]);
        //                                        }
        //                                    }

        //                                    //Interpolation Formula
        //                                    vcfliters = ((vcflitersfrom - vcflitersto) / ((densityfrom * 1000) - (densityto * 1000)));
        //                                    vcfliters = (vcfliters * (Convert.ToDecimal(tankdensity.ToString()) - (densityto * 1000)));
        //                                    vcfliters = vcfliters + vcflitersto;
        //                                }

        //                                dsASTMTABLE34.Clear();

        //                                //Tankage Inventory Computations Vapor
        //                                using (SqlDataReader drCOMPUTATIONS = tankinvBL.TankageInventory_Computations_Vapor(tanknumber, float.Parse(innage.ToString()), float.Parse(tanktemperature.ToString()), float.Parse(tankdensity.ToString()), float.Parse(vaporpressure.ToString()), float.Parse(vaportemperature.ToString()), float.Parse(vcfliters.ToString())))
        //                                {
        //                                    if (drCOMPUTATIONS.Read())
        //                                    {
        //                                        if (drCOMPUTATIONS["innage"].ToString() != "0" && drCOMPUTATIONS["innage_volume"].ToString() == "0.00")
        //                                        {
        //                                            //string uploaderror = "● Tank " + tanknumber + " - Innage " + innage + " not be found in " + drTANKDETAILS["strap_table_description"] + " strap table.";
        //                                            //strUploadError = strUploadError + uploaderror + "<br>";
        //                                        }

        //                                        //Calibration Table
        //                                        tankinvBO.innage = Convert.ToDecimal(drCOMPUTATIONS["innage"]);
        //                                        tankinvBO.innagevolume = Convert.ToDecimal(drCOMPUTATIONS["innage_volume"]);
        //                                        tankinvBO.water = 0;
        //                                        tankinvBO.watervolume = 0;
        //                                        tankinvBO.tanktemperature = Convert.ToDecimal(drCOMPUTATIONS["tank_temperature"]);
        //                                        tankinvBO.density15 = Convert.ToDecimal(drCOMPUTATIONS["density_15"]);
        //                                        tankinvBO.bswpercent = 0;
        //                                        tankinvBO.vaporpressure = Convert.ToDecimal(drCOMPUTATIONS["vapor_pressure"]);
        //                                        tankinvBO.vaportemperature = Convert.ToDecimal(drCOMPUTATIONS["vapor_temperature"]);
        //                                        tankinvBO.litersair = 0;
        //                                        tankinvBO.apicorrection = 0;
        //                                        tankinvBO.bswvolume = 0;
        //                                        tankinvBO.netlitersair = 0;
        //                                        tankinvBO.vcfliters = Convert.ToDecimal(drCOMPUTATIONS["vcf_liters"]);
        //                                        tankinvBO.liters15 = 0;
        //                                        tankinvBO.vcfkilograms = Convert.ToDecimal(drCOMPUTATIONS["vcf_kilograms"]);
        //                                        tankinvBO.kg15 = 0;
        //                                        tankinvBO.vcfbarrels = Convert.ToDecimal(drCOMPUTATIONS["vcf_barrels"]);
        //                                        tankinvBO.bbl60 = 0;

        //                                        //Liquid
        //                                        tankinvBO.litersairliquid = Convert.ToDecimal(drCOMPUTATIONS["liters_air_liquid"]);
        //                                        tankinvBO.bblsairliquid = Convert.ToDecimal(drCOMPUTATIONS["bbls_air_liquid"]);
        //                                        tankinvBO.bbl60liquid = Convert.ToDecimal(drCOMPUTATIONS["bbl_60_liquid"]);
        //                                        tankinvBO.liters15liquid = Convert.ToDecimal(drCOMPUTATIONS["liters_15_liquid"]);
        //                                        tankinvBO.kg15liquid = Convert.ToDecimal(drCOMPUTATIONS["kg_15_liquid"]);

        //                                        //Vapor
        //                                        tankinvBO.litersairvapor = Convert.ToDecimal(drCOMPUTATIONS["liters_air_vapor"]);
        //                                        tankinvBO.bblsairvapor = Convert.ToDecimal(drCOMPUTATIONS["bbls_air_vapor"]);
        //                                        tankinvBO.bbl60vapor = Convert.ToDecimal(drCOMPUTATIONS["bbl_60_vapor"]);
        //                                        tankinvBO.liters15vapor = Convert.ToDecimal(drCOMPUTATIONS["liters_15_vapor"]);
        //                                        tankinvBO.kg15vapor = Convert.ToDecimal(drCOMPUTATIONS["kg_15_vapor"]);

        //                                        //Total
        //                                        tankinvBO.totalliters15 = Convert.ToDecimal(drCOMPUTATIONS["total_liters_15"]);
        //                                        tankinvBO.totalkg15 = Convert.ToDecimal(drCOMPUTATIONS["total_kg_15"]);

        //                                        //Net Summary
        //                                        tankinvBO.klair = Convert.ToDecimal(drCOMPUTATIONS["kl_air"]);
        //                                        tankinvBO.kl15 = Convert.ToDecimal(drCOMPUTATIONS["kl_15"]);
        //                                        tankinvBO.bblair = Convert.ToDecimal(drCOMPUTATIONS["bbl_air"]);
        //                                        tankinvBO.mto = Convert.ToDecimal(drCOMPUTATIONS["mto"]);
        //                                    }
        //                                }
        //                            }

        //                            dsASTMTABLE34Temperature.Clear();
        //                        }

        //                        //Adjustments
        //                        string strAdjustments = tankinvBL.TankageInventory_Adjustments(tankinvBO, strUserID);
        //                    }

        //                    else //Other Products
        //                    {
        //                        //For turnaround tanks, zero out all values
        //                        if (innage.ToString() == "0" || innage.ToString() == "0.00")
        //                        {
        //                            decimal decZeroOut = 0;

        //                            //Calibration Table
        //                            tankinvBO.innage = decZeroOut;
        //                            tankinvBO.innagevolume = decZeroOut;
        //                            tankinvBO.water = decZeroOut;
        //                            tankinvBO.watervolume = decZeroOut;
        //                            tankinvBO.tanktemperature = decZeroOut;
        //                            tankinvBO.density15 = decZeroOut;
        //                            tankinvBO.bswpercent = decZeroOut;
        //                            tankinvBO.vaporpressure = decZeroOut;
        //                            tankinvBO.vaportemperature = decZeroOut;
        //                            tankinvBO.litersair = decZeroOut;
        //                            tankinvBO.apicorrection = decZeroOut;
        //                            tankinvBO.bswvolume = decZeroOut;
        //                            tankinvBO.netlitersair = decZeroOut;
        //                            tankinvBO.vcfliters = decZeroOut;
        //                            tankinvBO.liters15 = decZeroOut;
        //                            tankinvBO.vcfkilograms = decZeroOut;
        //                            tankinvBO.kg15 = decZeroOut;
        //                            tankinvBO.vcfbarrels = decZeroOut;
        //                            tankinvBO.bbl60 = decZeroOut;

        //                            //Liquid
        //                            tankinvBO.litersairliquid = decZeroOut;
        //                            tankinvBO.bblsairliquid = decZeroOut;
        //                            tankinvBO.bbl60liquid = decZeroOut;
        //                            tankinvBO.liters15liquid = decZeroOut;
        //                            tankinvBO.kg15liquid = decZeroOut;

        //                            //Vapor
        //                            tankinvBO.litersairvapor = decZeroOut;
        //                            tankinvBO.bblsairvapor = decZeroOut;
        //                            tankinvBO.bbl60vapor = decZeroOut;
        //                            tankinvBO.liters15vapor = decZeroOut;
        //                            tankinvBO.kg15vapor = decZeroOut;

        //                            //Total
        //                            tankinvBO.totalliters15 = decZeroOut;
        //                            tankinvBO.totalkg15 = decZeroOut;

        //                            //Net Summary
        //                            tankinvBO.klair = decZeroOut;
        //                            tankinvBO.kl15 = decZeroOut;
        //                            tankinvBO.bblair = decZeroOut;
        //                            tankinvBO.mto = decZeroOut;
        //                        }

        //                        else
        //                        {
        //                            string[] strEvenNumbers = new string[] { "0", "2", "4", "6", "8" };

        //                            //Get VCF values using calculator program (ISTOCK.DLMC)
        //                            dlmcparamobject.product = drTANKDETAILS["product_code"].ToString().Trim();
        //                            dlmcparamobject.productType = drTANKDETAILS["product_type"].ToString().Trim();
        //                            dlmcparamobject.method = baseclass.GetMethodType(drTANKDETAILS["product_type"].ToString().Trim());
        //                            dlmcparamobject.volume = double.Parse("1000");
        //                            dlmcparamobject.baseTemp = 15;
        //                            dlmcparamobject.sampleTemp = 15;
        //                            dlmcparamobject.tankTemp = double.Parse(tanktemperature.ToString());
        //                            dlmcparamobject.pressure = 0;
        //                            decimal vcfliters = 0;

        //                            //Check if inputted density is below 654, then use extrapolation formula //Except BTX, LPG, and PROPYLENE tanks
        //                            if ((drTANKDETAILS["product_code"].ToString() != "2491" || drTANKDETAILS["product_code"].ToString() != "2492" || drTANKDETAILS["product_code"].ToString() != "2271") && float.Parse(tankdensity.ToString()) < 654)
        //                            {
        //                                //Start at 656 and 654 density
        //                                double densityfrom = 656;
        //                                double densityto = 654;

        //                                dlmcparamobject.density = double.Parse(densityfrom.ToString());
        //                                dlmcobject = dlmccalculate.DLMCCalculate(dlmcparamobject);
        //                                decimal vcf_from = Math.Round(Convert.ToDecimal(dlmcobject.vcfAt15.ToString()), 4); //VCF rounded to 4 decimal places

        //                                //To compensate 0.0001 difference in temp 30 between density 658 and 656 computation in DLL vs. Table54 hardcopy
        //                                if ((tanktemperature.ToString() == "30" || tanktemperature.ToString() == "30.00") && (densityfrom.ToString() == "658" || densityfrom.ToString() == "656"))
        //                                {
        //                                    vcf_from = (vcf_from + Convert.ToDecimal("0.0001"));
        //                                }

        //                                dlmcparamobject.density = double.Parse(densityto.ToString());
        //                                dlmcobject = dlmccalculate.DLMCCalculate(dlmcparamobject);
        //                                decimal vcf_to = Math.Round(Convert.ToDecimal(dlmcobject.vcfAt15.ToString()), 4); //VCF rounded to 4 decimal places

        //                                float intCount = float.Parse(tankdensity.ToString());
        //                                int intCounter;

        //                                //Loop into density by 2s; start at density 652
        //                                for (intCounter = 652; intCounter >= intCount; intCounter -= 2)
        //                                {
        //                                    //Extrapolation Formula
        //                                    vcfliters = (vcf_to - (Convert.ToDecimal(densityto) - intCounter) / (Convert.ToDecimal(densityfrom) - Convert.ToDecimal(densityto)) * (vcf_from - vcf_to));
        //                                    vcf_from = vcf_to;
        //                                    vcf_to = vcfliters;
        //                                    densityfrom = densityto;
        //                                    densityto = intCounter;
        //                                }

        //                                //If inputted density is not divisible by 2 or have decimal 
        //                                if (strEvenNumbers.Contains(tankdensity.ToString().Substring(tankdensity.ToString().Length - 1)) == false)
        //                                {
        //                                    vcfliters = (vcf_to - (Convert.ToDecimal(densityto) - Convert.ToDecimal(tankdensity.ToString())) / (Convert.ToDecimal(densityfrom) - Convert.ToDecimal(densityto)) * (vcf_from - vcf_to));
        //                                    vcfliters = Math.Round(vcfliters, 4);
        //                                }
        //                            }

        //                            else
        //                            {
        //                                //Benzene, Toluene, and Mixed Xylene (BTX) products; use 5 decimal places
        //                                if (drTANKDETAILS["product_code"].ToString().Contains("2491") || drTANKDETAILS["product_code"].ToString().Contains("2492") || drTANKDETAILS["product_code"].ToString().Contains("2271"))
        //                                {
        //                                    dlmcparamobject.density = double.Parse(tankdensity.ToString());
        //                                    dlmcobject = dlmccalculate.DLMCCalculate(dlmcparamobject);
        //                                    vcfliters = Convert.ToDecimal(dlmcobject.vcfAt15.ToString());
        //                                }

        //                                else
        //                                {
        //                                    decimal densityfrom = Decimal.Truncate(Convert.ToDecimal(tankdensity.ToString())); //Math.Round(Convert.ToDecimal(this.txtDensity15.Text), 0);

        //                                    //Check if last digit of actual density is an even number
        //                                    if (strEvenNumbers.Contains(densityfrom.ToString().Substring(densityfrom.ToString().Length - 1)))
        //                                    {
        //                                        densityfrom = (densityfrom + 2);
        //                                    }

        //                                    else
        //                                    {
        //                                        densityfrom = (densityfrom - 1) + 2;
        //                                    }

        //                                    dlmcparamobject.density = double.Parse(densityfrom.ToString());
        //                                    dlmcobject = dlmccalculate.DLMCCalculate(dlmcparamobject);
        //                                    decimal vcflitersfrom = Math.Round(Convert.ToDecimal(dlmcobject.vcfAt15.ToString()), 4);

        //                                    decimal densityto = densityfrom - 2;
        //                                    dlmcparamobject.density = double.Parse(densityto.ToString());
        //                                    dlmcobject = dlmccalculate.DLMCCalculate(dlmcparamobject);
        //                                    decimal vcflitersto = Math.Round(Convert.ToDecimal(dlmcobject.vcfAt15.ToString()), 4);

        //                                    //Interpolation Formula
        //                                    vcfliters = ((vcflitersfrom - vcflitersto) / (densityfrom - densityto));
        //                                    vcfliters = (vcfliters * (Convert.ToDecimal(tankdensity.ToString()) - densityto));
        //                                    vcfliters = vcfliters + vcflitersto;
        //                                    vcfliters = Math.Round(vcfliters, 4, MidpointRounding.AwayFromZero);

        //                                    //To compensate 0.0001 difference in temp 30 between density 658 and 656 computation in DLL vs. Table54 hardcopy
        //                                    if ((drTANKDETAILS["product_code"].ToString().Contains("2491") == false || drTANKDETAILS["product_code"].ToString().Contains("2492") == false || drTANKDETAILS["product_code"].ToString().Contains("2271") == false) && (tanktemperature.ToString() == "30" || tanktemperature.ToString() == "30.00") && (tankdensity.ToString() == "658" || tankdensity.ToString() == "656"))
        //                                    {
        //                                        vcfliters = (vcfliters + Convert.ToDecimal("0.0001"));
        //                                    }
        //                                }
        //                            }

        //                            //To handle unverified operation //For LPG and Propylene products
        //                            if (vcfliters.ToString() == "NaN")
        //                            {
        //                                vcfliters = 0;
        //                            }

        //                            //Tankage Inventory Computations
        //                            using (SqlDataReader drCOMPUTATIONS = tankinvBL.TankageInventory_Computations(tanknumber, float.Parse(innage.ToString()), float.Parse(water.ToString()), float.Parse(tanktemperature.ToString()), float.Parse(tankdensity.ToString()), float.Parse(bswpercent.ToString()), float.Parse(vcfliters.ToString())))
        //                            {
        //                                if (drCOMPUTATIONS.Read())
        //                                {
        //                                    if (drCOMPUTATIONS["innage"].ToString() != "0" && drCOMPUTATIONS["innage_volume"].ToString() == "0.00")
        //                                    {
        //                                        //string uploaderror = "● Tank " + tanknumber + " - Innage " + innage + " not be found in " + drTANKDETAILS["strap_table_description"] + " strap table.";
        //                                        //strUploadError = strUploadError + uploaderror + "<br>";
        //                                    }

        //                                    //Calibration Table
        //                                    tankinvBO.innage = Convert.ToDecimal(drCOMPUTATIONS["innage"]);
        //                                    tankinvBO.innagevolume = Convert.ToDecimal(drCOMPUTATIONS["innage_volume"]);
        //                                    tankinvBO.water = Convert.ToDecimal(drCOMPUTATIONS["water"]);
        //                                    tankinvBO.watervolume = Convert.ToDecimal(drCOMPUTATIONS["water_volume"]);
        //                                    tankinvBO.tanktemperature = Convert.ToDecimal(drCOMPUTATIONS["tank_temperature"]);
        //                                    tankinvBO.density15 = Convert.ToDecimal(drCOMPUTATIONS["density_15"]);
        //                                    tankinvBO.bswpercent = Convert.ToDecimal(drCOMPUTATIONS["bsw_percent"]);
        //                                    tankinvBO.vaporpressure = 0;
        //                                    tankinvBO.vaportemperature = 0;
        //                                    tankinvBO.litersair = Convert.ToDecimal(drCOMPUTATIONS["liters_air"]);
        //                                    tankinvBO.apicorrection = Convert.ToDecimal(drCOMPUTATIONS["api_correction"]);
        //                                    tankinvBO.bswvolume = Convert.ToDecimal(drCOMPUTATIONS["bsw_volume"]);
        //                                    tankinvBO.netlitersair = Convert.ToDecimal(drCOMPUTATIONS["net_liters_air"]);
        //                                    tankinvBO.vcfliters = Convert.ToDecimal(drCOMPUTATIONS["vcf_liters"]);
        //                                    tankinvBO.liters15 = Convert.ToDecimal(drCOMPUTATIONS["liters_15"]);
        //                                    tankinvBO.vcfkilograms = Convert.ToDecimal(drCOMPUTATIONS["vcf_kilograms"]);
        //                                    tankinvBO.kg15 = Convert.ToDecimal(drCOMPUTATIONS["kg_15"]);
        //                                    tankinvBO.vcfbarrels = Convert.ToDecimal(drCOMPUTATIONS["vcf_barrels"]);
        //                                    tankinvBO.bbl60 = Convert.ToDecimal(drCOMPUTATIONS["bbl_60"]);

        //                                    //Vapor
        //                                    tankinvBO.litersairliquid = 0;
        //                                    tankinvBO.bblsairliquid = 0;
        //                                    tankinvBO.bbl60liquid = 0;
        //                                    tankinvBO.liters15liquid = 0;
        //                                    tankinvBO.kg15liquid = 0;
        //                                    tankinvBO.litersairvapor = 0;
        //                                    tankinvBO.bblsairvapor = 0;
        //                                    tankinvBO.bbl60vapor = 0;
        //                                    tankinvBO.liters15vapor = 0;
        //                                    tankinvBO.kg15vapor = 0;
        //                                    tankinvBO.totalliters15 = 0;
        //                                    tankinvBO.totalkg15 = 0;

        //                                    //Net Summary
        //                                    tankinvBO.klair = Convert.ToDecimal(drCOMPUTATIONS["kl_air"]);
        //                                    tankinvBO.kl15 = Convert.ToDecimal(drCOMPUTATIONS["kl_15"]);
        //                                    tankinvBO.bblair = Convert.ToDecimal(drCOMPUTATIONS["bbl_air"]);
        //                                    tankinvBO.mto = Convert.ToDecimal(drCOMPUTATIONS["mto"]);
        //                                }
        //                            }
        //                        }

        //                        //Adjustments
        //                        string strAdjustments = tankinvBL.TankageInventory_Adjustments(tankinvBO, strUserID);
        //                    }

        //                    //
        //                }
        //            } //End of Tank Details
                


        //            } //End of while loop
        //        } //End of Tankage Inventory

        //        baseclass.Alert("WTF");
        //    }

        //    catch (Exception ex)
        //    {
        //        this.txtUserName.Text = (ex.Message);
        //    }




        //}


    }
}



//protected void btnLogin_Click(object sender, System.EventArgs e)
//        {
//            string strUserName = this.txtUserName.Text;
//            string strPassword = this.txtPassword.Text;

//            using (DirectoryEntry adsEntry = new DirectoryEntry("LDAP://rbcsvrdc01.robinsonsbank.com.ph"))
//            {
//                bool ValidUser = false;

//                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "robinsonsbank.com.ph"))
//                {
//                    //Validate the credentials
//                    ValidUser = pc.ValidateCredentials(strUserName, strPassword);

//                    if (ValidUser == true)
//                    {
//                        UserPrincipal user = UserPrincipal.FindByIdentity(pc, strUserName);
//                        var userid = user.SamAccountName;
//                        var distinguishedname = user.DistinguishedName;
//                        string[] val = new string[5];
//                        val = distinguishedname.ToString().Split(',');
//                        int intSolID = 0;

//                        //Check if network_id exist
//                        System.Data.DataSet dsUF = new System.Data.DataSet();
//                        dsUF = DBHelper.GetData("SELECT network_id, full_name, user_status, user_access, user_level, sol_id FROM userfile WHERE network_id = '" + strUserName.ToUpper().Trim() + "'");

//                        if (dsUF.Tables[0].Rows.Count == 0)
//                        {
//                            //If network_id is from Branch get SOL ID
//                            if (val[2].ToString().Contains("Branches"))
//                            {
//                                //Branches
//                                System.Data.DataSet dsBRANCHES = new System.Data.DataSet();
//                                dsBRANCHES = DBHelper.GetData("SELECT sol_id, branch_code, branch_name FROM branches WHERE branch_code = '" + this.txtUserName.Text.Substring(0, 3) + "' ");
//                                intSolID = Convert.ToInt32(dsBRANCHES.Tables[0].Rows[0]["sol_id"]);
//                                dsBRANCHES.Clear();

//                                //Create new user
//                                System.Data.DataSet dsUF2 = new System.Data.DataSet();
//                                dsUF2 = DBHelper.GetData("INSERT INTO userfile " +
//                                                         "(network_id, " +
//                                                         " full_name, " +
//                                                         " position, " +
//                                                         " sol_id, " +
//                                                         " user_level, " +
//                                                         " user_status, " +
//                                                         " date_time_login, " +
//                                                         " date_created, " +
//                                                         " time_created, " +
//                                                         " created_by) " +
//                                                         "VALUES " +
//                                                         "('" + user.SamAccountName + "', " +
//                                                         " '" + user.Name + "', " +
//                                                         " '" + user.Description + "', " +
//                                                         " '" + intSolID + "', " +
//                                                         " '3', " +
//                                                         " '1', " +
//                                                         " NOW(), " +
//                                                         " CURDATE(), " +
//                                                         " CURTIME(), " +
//                                                         " '" + user.SamAccountName + "')");
//                                dsUF2.Clear();
//                            }

//                            else
//                            {
//                                //Create new user
//                                System.Data.DataSet dsUF2 = new System.Data.DataSet();
//                                dsUF2 = DBHelper.GetData("INSERT INTO userfile " +
//                                                         "(network_id, " +
//                                                         " full_name, " +
//                                                         " position, " +
//                                                         " sol_id, " +
//                                                         " user_level, " +
//                                                         " user_status, " +
//                                                         " date_time_login, " +
//                                                         " date_created, " +
//                                                         " time_created, " +
//                                                         " created_by) " +
//                                                         "VALUES " +
//                                                         "('" + user.SamAccountName + "', " +
//                                                         " '" + user.Name + "', " +
//                                                         " '" + user.Description + "', " +
//                                                         " '" + intSolID + "', " +
//                                                         " '2', " +
//                                                         " '1', " +
//                                                         " NOW(), " +
//                                                         " CURDATE(), " +
//                                                         " CURTIME(), " +
//                                                         " '" + user.SamAccountName + "')");
//                                dsUF2.Clear();
//                            }

//                            Response.Redirect("home.aspx?userid=" + strUserName + "&userlevel=" + 3 + "&solid=" + intSolID, false);
//                            Context.ApplicationInstance.CompleteRequest();
//                        }

//                        else
//                        {
//                            int intUserStatus = Convert.ToInt32(dsUF.Tables[0].Rows[0]["user_status"]);
//                            int intUserAccess = Convert.ToInt32(dsUF.Tables[0].Rows[0]["user_access"]);
//                            int intUserLevel = Convert.ToInt32(dsUF.Tables[0].Rows[0]["user_level"]);
//                            intSolID = Convert.ToInt32(dsUF.Tables[0].Rows[0]["sol_id"]);

//                            //Update user_status and date_time_login
//                            System.Data.DataSet dsUPDATE = new System.Data.DataSet();
//                            dsUPDATE = DBHelper.GetData("UPDATE userfile SET user_status = 1, date_time_login = NOW() WHERE network_id = '" + strUserName.ToUpper().Trim() + "'");
//                            dsUPDATE.Clear();

//                            SystemLog();
//                            Response.Redirect("home.aspx?userid=" + strUserName + "&userlevel=" + intUserLevel + "&solid=" + intSolID, false);
//                        }

//                        dsUF.Clear();
//                    }

//                    else
//                    {
//                        //Alert
//                        this.lblAlertTitle.InnerText = "Invalid Credentials";
//                        this.lblAlertContent.InnerText = "The user id or password is incorrect";
//                        this.btnOKAlert.Visible = true;
//                        this.btnCancel.Visible = false;
//                        this.btnCreate.Visible = false;
//                        this.popupalert.Show();
//                    }
//                }
//            }
//        }





//protected void btnAcceptDisclaimer_Click(object sender, EventArgs e)
//{
//    string strUserName = this.txtUserName.Text.ToUpper().Trim();

//    //Userfile
//    System.Data.DataSet dsUF = new System.Data.DataSet();
//    dsUF = DBHelper.GetData("SELECT network_id, user_password, user_level, user_role, sol_id FROM userfile WHERE network_id = '" + strUserName + "' ");

//    string strUserPassword = dsUF.Tables[0].Rows[0]["user_password"].ToString().Trim();

//    //update user details
//    System.Data.DataSet dsUPDATE = new System.Data.DataSet();
//    dsUPDATE = DBHelper.GetData("UPDATE userfile SET date_time_login = NOW(), " +
//                                "user_status = 1 " +
//                                "WHERE network_id = '" + strUserName + "' " +
//                                "AND user_password = '" + strUserPassword + "' ");
//    dsUPDATE.Clear();


//    Response.Redirect("home.aspx?userid=" + strUserName + "&userlevel=" + Convert.ToInt32(dsUF.Tables[0].Rows[0]["user_level"]) + "&userrole=" + Convert.ToInt32(dsUF.Tables[0].Rows[0]["user_role"]) + "&solid=" + Convert.ToInt32(dsUF.Tables[0].Rows[0]["sol_id"]), false);
//    Context.ApplicationInstance.CompleteRequest();
//}



