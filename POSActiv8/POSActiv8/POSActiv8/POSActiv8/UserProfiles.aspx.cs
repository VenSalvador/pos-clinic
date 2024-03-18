using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;
using System.IO;
using OfficeOpenXml;
using MySql.Data.MySqlClient;

using BusinessObject;
using BusinessLogic;
using DataAccess;

namespace POSActiv8
{
    public partial class UserProfiles : System.Web.UI.Page
    {
        SystemLogsBL slBL = new SystemLogsBL();
        ErrorLogsBL elBL = new ErrorLogsBL();
        SystemParametersBL sysparamBL = new SystemParametersBL();
        UserProfilesBO upBO = new UserProfilesBO();
        UserProfilesBL upBL = new UserProfilesBL();
        string strUserID;
        int intUserLevel;
        int intUserRole;
        string strSystemLogs;
        string strErrorLogs;

        public void UserLevel()
        {
            //User Level
            using (MySqlDataReader dr = sysparamBL.SystemParameters_View(1))
            {
                this.ddlUserLevel.Items.Clear();
                this.ddlUserLevel.DataSource = dr;
                this.ddlUserLevel.DataValueField = "ParameterValue";
                this.ddlUserLevel.DataTextField = "ParameterDescription";
                this.ddlUserLevel.DataBind();
            }

            ddlUserLevel.Items.Insert(0, new ListItem("Choose a user level...", string.Empty));
            this.ddlUserLevel.SelectedIndex = 0;
        }

        public void UserRole()
        {
            //User Role
            using (MySqlDataReader dr = sysparamBL.SystemParameters_View(2))
            {
                this.ddlUserRole.Items.Clear();
                this.ddlUserRole.DataSource = dr;
                this.ddlUserRole.DataValueField = "ParameterValue";
                this.ddlUserRole.DataTextField = "ParameterDescription";
                this.ddlUserRole.DataBind();
            }

            ddlUserRole.Items.Insert(0, new ListItem("Choose a user role...", string.Empty));
        }

        public void UserStatus()
        {
            //User Status
            using (MySqlDataReader dr = sysparamBL.SystemParameters_View(3))
            {
                this.rdoUserStatus.Items.Clear();
                this.rdoUserStatus.DataSource = dr;
                this.rdoUserStatus.DataValueField = "ParameterValue";
                this.rdoUserStatus.DataTextField = "ParameterDescription";
                this.rdoUserStatus.DataBind();
            }

            this.rdoUserStatus.SelectedIndex = 1;
        }

        public void LoginStatus()
        {
            //Login Status
            using (MySqlDataReader dr = sysparamBL.SystemParameters_View(4))
            {
                this.rdoLoginStatus.Items.Clear();
                this.rdoLoginStatus.DataSource = dr;
                this.rdoLoginStatus.DataValueField = "ParameterValue";
                this.rdoLoginStatus.DataTextField = "ParameterDescription";
                this.rdoLoginStatus.DataBind();
            }

            this.rdoLoginStatus.SelectedIndex = 1;
        }

        //Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                strUserID = Request.QueryString["userid"];
                intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
                intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);
                //baseclass.UserInformation(strUserID, intUserLevel, intUserRole);
                //baseclass.UserAccess(strUserID, intUserLevel, intUserRole, 1);

                //Error Message
                this.divErrorMessage.Visible = false;

                //Page Title
                this.divPageTitle.Visible = true;

                //Button Controls
                this.divButtonControls.Visible = true;
                this.btnCreate.Visible = true;

                //User Profiles
                this.divUserProfiles.Visible = true;

                try
                {
                    //User Profiles
                    using (MySqlDataReader drUF = upBL.UserProfiles_View(string.Empty, string.Empty))
                    {
                        this.lblUserProfiles.Visible = true;

                        if (drUF.HasRows)
                        {
                            //Gridview
                            this.gvUserProfiles.DataSource = drUF;
                            this.gvUserProfiles.DataBind();
                            this.gvUserProfiles.Visible = true;
                            this.txtSearch.Visible = true;

                            //Count
                            this.lblUserProfiles.InnerText = "Showing " + string.Format("{0:n0}", this.gvUserProfiles.Rows.Count) + " records";

                            //Button Controls
                            this.btnExport.Visible = true;
                            this.btnCreate.Visible = true;
                        }

                        else
                        {
                            //Gridview
                            this.gvUserProfiles.Visible = false;
                            this.txtSearch.Visible = false;

                            //Count
                            this.lblUserProfiles.InnerText = "No records found";

                            //Button Controls
                            this.btnExport.Visible = false;
                            this.btnCreate.Visible = true;
                        }
                    }
                }

                catch (Exception ex)
                {
                    //Error Message
                    this.divErrorMessage.Visible = true;
                    this.lblErrorMessage.InnerText = ex.Message;
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "User Profiles - Page Load", ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //User Profiles
                    this.divUserProfiles.Visible = false;
                }
            }
        }

        //Griview
        protected void gvUserProfiles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);
            this.lblRecordID.Text = e.CommandArgument.ToString().Trim();

            try
            {
                this.lblUserProfileDetails.Text = "User Profile Details";

                //User Profiles
                using (MySqlDataReader drUSERPROFILES = upBL.UserProfiles_View(this.lblRecordID.Text.Trim(), string.Empty))
                {
                    this.lblUserProfiles.Visible = true;

                    if (drUSERPROFILES.Read())
                    {
                        this.txtUserID.Text = drUSERPROFILES["NetworkID"].ToString().Trim();
                        this.txtUserID.Enabled = false;
                        this.txtUserID.Attributes["style"] = "background-color:#dfdfdf;";
                        this.txtUserName.Text = drUSERPROFILES["FullName"].ToString().Trim();
                        this.txtUserName.Enabled = false;
                        this.txtUserName.Attributes["style"] = "background-color:#dfdfdf;";
                        this.txtEmailAddress.Text = drUSERPROFILES["EmailAddress"].ToString().Trim();
                        this.txtEmailAddress.Enabled = false;
                        this.txtEmailAddress.Attributes["style"] = "background-color:#dfdfdf;";
                        this.txtDepartment.Text = drUSERPROFILES["UserDepartment"].ToString().Trim();
                        this.txtDepartment.Enabled = false;
                        this.txtDepartment.Attributes["style"] = "background-color:#dfdfdf;";
                        UserLevel();
                        this.ddlUserLevel.Text = drUSERPROFILES["UserLevel"].ToString().Trim();
                        UserRole();
                        this.ddlUserRole.Text = drUSERPROFILES["UserRole"].ToString().Trim();
                        UserStatus();
                        this.rdoUserStatus.Text = drUSERPROFILES["UserStatus"].ToString().Trim();
                        this.divUserStatus.Visible = true;
                        LoginStatus();
                        this.rdoLoginStatus.Text = drUSERPROFILES["LoginStatus"].ToString().Trim();
                        this.divLoginStatus.Visible = true;
                    }
                }

                //Button Controls
                this.btnCancelUserProfiles.Visible = true;
                this.btnSaveUserProfiles.Visible = false;
                this.btnUpdateUserProfiles.Visible = true;
                //this.btnUpdateUserProfiles.Visible = baseclass.ButtonControls(intUserLevel, intUserRole, 1, "Update");

                //Show Modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            catch (Exception ex)
            {
                //Error Message
                this.divErrorMessage.Visible = true;
                this.lblErrorMessage.InnerText = ex.Message;
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "User Profiles - View Details", ex.Message, strUserID);

                //Page Title
                this.divPageTitle.Visible = false;

                //Button Controls
                this.divButtonControls.Visible = false;

                //User Profiles
                this.divUserProfiles.Visible = false;
            }
        }

        //Button Controls
        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];

            try
            {
                using (MySqlDataReader drUSERPROFILES = upBL.UserProfiles_View(string.Empty, this.txtSearch.Text.Trim()))
                {
                    this.lblUserProfiles.Visible = true;

                    if (drUSERPROFILES.HasRows)
                    {
                        //Gridview
                        this.gvUserProfiles.DataSource = drUSERPROFILES;
                        this.gvUserProfiles.DataBind();
                        this.gvUserProfiles.Visible = true;

                        //Count
                        this.lblUserProfiles.InnerText = "Showing " + string.Format("{0:n0}", this.gvUserProfiles.Rows.Count) + " records";

                        //Button Controls
                        this.btnExport.Visible = true;
                        this.btnCreate.Visible = true;
                    }

                    else
                    {
                        //Gridview
                        this.gvUserProfiles.Visible = false;

                        //Count
                        this.lblUserProfiles.InnerText = "No records found";

                        //Button Controls
                        this.btnExport.Visible = false;
                        this.btnCreate.Visible = true;
                    }
                }
            }

            catch (Exception ex)
            {
                //Error Message
                this.divErrorMessage.Visible = true;
                this.lblErrorMessage.InnerText = ex.Message;
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "User Profiles - Search", ex.Message, strUserID);

                //Page Title
                this.divPageTitle.Visible = false;

                //Button Controls
                this.divButtonControls.Visible = false;

                //User Profiles
                this.divUserProfiles.Visible = false;
            }
        }

        protected void btnCreate_Click(object sender, System.EventArgs e)
        {
            this.lblUserProfileDetails.Text = "New User Profile";
            UserLevel();
            UserRole();
            UserStatus();
            LoginStatus();

            this.txtUserID.Focus();
            this.txtUserID.Enabled = true;
            this.txtUserID.Attributes["style"] = "background-color:#f9fbcf;";
            this.txtUserName.Enabled = false;
            this.txtUserName.Attributes["style"] = "background-color:#dfdfdf;";
            //this.txtEmailAddress.Enabled = false;
            //this.txtEmailAddress.Attributes["style"] = "background-color:#dfdfdf;";
            //this.txtDepartment.Enabled = false;
            //this.txtDepartment.Attributes["style"] = "background-color:#dfdfdf;";
            //this.divUserStatus.Visible = false;
           // this.divLoginStatus.Visible = false;

            //Button Controls
            this.btnCancelUserProfiles.Visible = true;
            this.btnSaveUserProfiles.Visible = true;
            this.btnUpdateUserProfiles.Visible = false;

            //Show Modal
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            try
            {
                //Reports Generated
                string strReportsGeneratedPath = HttpContext.Current.Server.MapPath(string.Format("~/ReportsGenerated/"));

                //Reference Date
                DateTime dteCurrentDate = System.DateTime.Now;
                string strCurrentDate = dteCurrentDate.ToString("MMddyyyy");

                //Source File
                var strFileName = "UserProfiles_" + strCurrentDate + ".xlsx";
                var strSaveUserProfiles = new FileInfo(Path.Combine(strReportsGeneratedPath, strFileName));

                //Check if file exist then delete
                if (System.IO.File.Exists(strSaveUserProfiles.ToString()))
                {
                    System.IO.File.Delete(strSaveUserProfiles.ToString());
                }

                using (var package = new ExcelPackage(strSaveUserProfiles))
                {
                    //Export User Profiles
                    using (MySqlDataReader drEXPORTUSERPROFILES = upBL.UserProfiles_View(string.Empty, string.Empty))
                    {
                        //Worksheet
                        ExcelWorksheet worksheetEXPORTUSERPROFILES = package.Workbook.Worksheets.Add("UserProfiles");

                        //Header
                        worksheetEXPORTUSERPROFILES.Cells[1, 1].Value = "Network ID";
                        worksheetEXPORTUSERPROFILES.Cells[1, 2].Value = "Full Name";
                        worksheetEXPORTUSERPROFILES.Cells[1, 3].Value = "Department";
                        worksheetEXPORTUSERPROFILES.Cells[1, 4].Value = "User Level";
                        worksheetEXPORTUSERPROFILES.Cells[1, 5].Value = "User Role";
                        worksheetEXPORTUSERPROFILES.Cells[1, 6].Value = "User Status";
                        worksheetEXPORTUSERPROFILES.Cells[1, 7].Value = "Login Status";
                        worksheetEXPORTUSERPROFILES.Cells[1, 8].Value = "IP Address";
                        worksheetEXPORTUSERPROFILES.Cells[1, 9].Value = "Date and Time Login";
                        worksheetEXPORTUSERPROFILES.Cells[1, 10].Value = "Date and Time Logout";
                        worksheetEXPORTUSERPROFILES.Cells[1, 11].Value = "Date and Time Created";
                        worksheetEXPORTUSERPROFILES.Cells[1, 12].Value = "Created By";
                        worksheetEXPORTUSERPROFILES.Cells[1, 13].Value = "Date and Time Updated";
                        worksheetEXPORTUSERPROFILES.Cells[1, 14].Value = "Updated By";
                        worksheetEXPORTUSERPROFILES.Cells[1, 1, 1, 14].Style.Font.Bold = true;

                        int intCounter = 0;

                        while (drEXPORTUSERPROFILES.Read())
                        {
                            //Generate Rows
                            worksheetEXPORTUSERPROFILES.Cells[2 + intCounter, 1].Value = drEXPORTUSERPROFILES["NetworkID"].ToString().Trim();
                            worksheetEXPORTUSERPROFILES.Cells[2 + intCounter, 2].Value = drEXPORTUSERPROFILES["FullName"].ToString().Trim();
                            worksheetEXPORTUSERPROFILES.Cells[2 + intCounter, 3].Value = drEXPORTUSERPROFILES["UserDepartment"].ToString().Trim();
                            worksheetEXPORTUSERPROFILES.Cells[2 + intCounter, 4].Value = drEXPORTUSERPROFILES["UserLevel"].ToString().Trim();
                            worksheetEXPORTUSERPROFILES.Cells[2 + intCounter, 5].Value = drEXPORTUSERPROFILES["UserRole"].ToString().Trim();
                            worksheetEXPORTUSERPROFILES.Cells[2 + intCounter, 6].Value = drEXPORTUSERPROFILES["UserStatus"].ToString().Trim();
                            worksheetEXPORTUSERPROFILES.Cells[2 + intCounter, 7].Value = drEXPORTUSERPROFILES["LoginStatus"].ToString().Trim();
                            worksheetEXPORTUSERPROFILES.Cells[2 + intCounter, 8].Value = drEXPORTUSERPROFILES["IPAddress"].ToString().Trim();
                            worksheetEXPORTUSERPROFILES.Cells[2 + intCounter, 9].Value = drEXPORTUSERPROFILES["DateTimeLogin"].ToString().Trim();
                            worksheetEXPORTUSERPROFILES.Cells[2 + intCounter, 10].Value = drEXPORTUSERPROFILES["DateTimeLogout"].ToString().Trim();
                            worksheetEXPORTUSERPROFILES.Cells[2 + intCounter, 11].Value = drEXPORTUSERPROFILES["DateTimeCreated"].ToString().Trim();
                            worksheetEXPORTUSERPROFILES.Cells[2 + intCounter, 12].Value = drEXPORTUSERPROFILES["CreatedBy"].ToString().Trim();
                            worksheetEXPORTUSERPROFILES.Cells[2 + intCounter, 13].Value = drEXPORTUSERPROFILES["DateTimeUpdated"].ToString().Trim();
                            worksheetEXPORTUSERPROFILES.Cells[2 + intCounter, 14].Value = drEXPORTUSERPROFILES["UpdatedBy"].ToString().Trim();

                            intCounter++;
                        }

                        if (intCounter == 0)
                        {
                            worksheetEXPORTUSERPROFILES.Cells[2 + intCounter, 1].Value = "No records found";
                        }

                        //Column Format
                        worksheetEXPORTUSERPROFILES.Cells[worksheetEXPORTUSERPROFILES.Dimension.Address].AutoFitColumns();

                        //Font Size
                        worksheetEXPORTUSERPROFILES.Cells[worksheetEXPORTUSERPROFILES.Dimension.Address].Style.Font.Size = 10;

                        //Save the file
                        package.Save();

                        //System Logs
                        strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(),  "User Profiles - Export", "Export successfully generated.", strUserID);

                        //Download the report file
                        string filePath = strSaveUserProfiles.ToString();
                        Response.ContentType = ContentType;
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
                        Response.WriteFile(filePath);
                        Response.End();
                    }
                }
            }

            catch (Exception ex)
            {
                //Error Message
                this.divErrorMessage.Visible = true;
                this.lblErrorMessage.InnerText = ex.Message;
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "User Profiles - Export", ex.Message, strUserID);

                //Page Title
                this.divPageTitle.Visible = false;

                //Button Controls
                this.divButtonControls.Visible = false;

                //User Profiles
                this.divUserProfiles.Visible = false;
            }
        }

        //User Details
        protected void txtUserID_TextChanged(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];

            try
            {
                //User Profiles Path //1 localhost //2 UAT //3 PROD
                var strUserProfilesPath = HttpContext.Current.Server.MapPath(string.Format("~/UserProfilesPath/UserCredentials.ini"));

                //Read each authorize user inside the file
                string[] strUserProfiles = System.IO.File.ReadAllLines(strUserProfilesPath);

                string strUserCredential = strUserProfiles[1].Substring(0);

                //Active Directory
                using (DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://pdc.nsjbi.com", strUserID, strUserCredential))
                {                  
                    using (DirectorySearcher directorySearcher = new DirectorySearcher(directoryEntry))
                    {
                        //set the list of properties you are interested in
                        directorySearcher.PropertiesToLoad.Add("sAMAccountName");
                        directorySearcher.PropertiesToLoad.Add("cn");
                        directorySearcher.PropertiesToLoad.Add("mail");
                        directorySearcher.PropertiesToLoad.Add("department");

                        //Set a filters to get only the active users from AD
                        directorySearcher.Filter = string.Format("({0})", "&(objectClass=user)(sAMAccountName=" + this.txtUserID.Text.Trim() + ")");

                        //Set Search Options
                        directorySearcher.SearchScope = SearchScope.Subtree;
                        directorySearcher.SearchRoot.AuthenticationType = AuthenticationTypes.Secure;
                        directorySearcher.PageSize = 100;

                        //run the search and and it will a collection of the entries that are found.
                        using (SearchResultCollection results = directorySearcher.FindAll())
                        {
                            foreach (SearchResult result in results)
                            {
                                this.txtUserID.Text = result.Properties["sAMAccountName"][0].ToString();
                                this.txtUserName.Text = result.Properties["cn"][0].ToString();
                                
                                //if (result.Properties["mail"][0].ToString() == null)
                                //{
                                //    this.txtEmailAddress.Text = string.Empty;
                                //}

                                //else
                                //{
                                //    this.txtEmailAddress.Text = result.Properties["mail"][0].ToString();
                                //}
                                
                               // this.txtDepartment.Text = result.Properties["department"][0].ToString();
                            }
                        }
                    }
                }

                this.ddlUserLevel.Focus();

                //Show Modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            catch (Exception ex)
            {
                //Error Message
                this.divErrorMessage.Visible = true;
                this.lblErrorMessage.InnerText = ex.Message;
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "User Profiles - Search User ID in Active Directory", ex.Message, strUserID);

                //Page Title
                this.divPageTitle.Visible = false;

                //Button Controls
                this.divButtonControls.Visible = false;

                //User Profiles
                this.divUserProfiles.Visible = false;
            }
        }

        protected void btnCancelUserProfiles_Click(object sender, System.EventArgs e)
        {
            this.txtUserID.Text = string.Empty;
            this.txtUserID.Enabled = true;
            this.txtUserID.Attributes["style"] = "background-color:#ffffff;";
            this.txtUserName.Text = string.Empty;
            this.txtEmailAddress.Text = string.Empty;
            this.txtDepartment.Text = string.Empty;
            this.lblValidationUserProfiles.Text = string.Empty;

            //Hide Modal
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalUserProfiles').modal('hide');</script>", false);
        }

        protected void btnSaveUserProfiles_Click(object sender, System.EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);
  
            if (this.txtUserID.Text == string.Empty)
            {
                this.lblValidationUserProfiles.Text = "User ID must not be empty.";
                this.lblValidationUserProfiles.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else if (this.txtUserName.Text == string.Empty)
            {
                this.lblValidationUserProfiles.Text = "Full name must not be empty.";
                this.lblValidationUserProfiles.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else if (this.ddlUserLevel.SelectedItem.Text == "Choose a user level...")
            {
                this.lblValidationUserProfiles.Text = "User level must not be empty.";
                this.lblValidationUserProfiles.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else if (this.ddlUserRole.SelectedItem.Text == "Choose a user role...")
            {
                this.lblValidationUserProfiles.Text = "User role must not be empty.";
                this.lblValidationUserProfiles.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else
            {
                //Check if User ID already exist
                System.Data.DataSet dsUSERPROFILES = new System.Data.DataSet();
                dsUSERPROFILES = DBHelper.GetData("SELECT NetworkID, Fullname FROM UserProfiles WHERE NetworkID = '" + this.txtUserID.Text.Trim() + "'");

                if (dsUSERPROFILES.Tables[0].Rows.Count > 0)
                {
                    this.lblValidationUserProfiles.Text = "User ID already exist.";
                    this.lblValidationUserProfiles.Attributes["style"] = "color:Red;";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
                }

                else
                {
                    upBO.recordid = 0;
                    upBO.networkid = this.txtUserID.Text.Trim();
                    upBO.fullname = this.txtUserName.Text.Trim();
                    upBO.emailaddress = this.txtEmailAddress.Text.Trim();
                    upBO.department = this.txtDepartment.Text.Trim();
                    upBO.userlevel = Convert.ToInt32(this.ddlUserLevel.SelectedValue);
                    upBO.userrole = Convert.ToInt32(this.ddlUserRole.SelectedValue);

                    //Save
                    string strSave = upBL.UserProfiles_Post(upBO, strUserID);

                    //System Logs
                    strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "User Profiles - Save", "Created new user profile for" + " " + this.txtUserID.Text.Trim(), strUserID);

                    //Alert
                    ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Record saved.', '', 'success') .then((value) => { window.location.href = 'UserProfiles.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole + "'; }); ", true);
                }

                dsUSERPROFILES.Clear();
            }
        }

        protected void btnUpdateUserProfiles_Click(object sender, System.EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);
       
            try
            { 
                //Check for changes on the records
                System.Data.DataSet dsUSERPROFILLES = new System.Data.DataSet();
                dsUSERPROFILLES = DBHelper.GetData("SELECT NetworkID, FullName, UserDepartment, UserLevel, UserRole, UserStatus, LoginStatus FROM UserProfiles WHERE NetworkID = '" + this.txtUserID.Text.Trim() + "' ");

                string strUserID2 = dsUSERPROFILLES.Tables[0].Rows[0]["NetworkID"].ToString().ToString();
                string strFullName = dsUSERPROFILLES.Tables[0].Rows[0]["FullName"].ToString().Trim();
                string strDepartment = dsUSERPROFILLES.Tables[0].Rows[0]["UserDepartment"].ToString().Trim();
                int intUserLevel2 = Convert.ToInt32(dsUSERPROFILLES.Tables[0].Rows[0]["UserLevel"]);
                int intUserRole2 = Convert.ToInt32(dsUSERPROFILLES.Tables[0].Rows[0]["UserRole"]);
                int intUserStatus = Convert.ToInt32(dsUSERPROFILLES.Tables[0].Rows[0]["UserStatus"]);
                int intLoginStatus = Convert.ToInt32(dsUSERPROFILLES.Tables[0].Rows[0]["LoginStatus"].ToString());

                if (this.txtUserID.Text == string.Empty)
                {
                    this.lblValidationUserProfiles.Text = "User ID must not be empty";
                    this.lblValidationUserProfiles.Attributes["style"] = "color:Red;";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalUserProfiles').modal('show');</script>", false);
                }

                else if (this.txtUserName.Text == string.Empty)
                {
                    this.lblValidationUserProfiles.Text = "Full name must not be empty";
                    this.lblValidationUserProfiles.Attributes["style"] = "color:Red;";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalUserProfiles').modal('show');</script>", false);
                }

                //else if (this.txtDepartment.Text == string.Empty)
                //{
                //    this.lblValidationUserProfiles.Text = "Department must not be empty";
                //    this.lblValidationUserProfiles.Attributes["style"] = "color:Red;";
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalUserProfiles').modal('show');</script>", false);
                //}

                else if (this.ddlUserLevel.Text == "Choose a user level...")
                {
                    this.lblValidationUserProfiles.Text = "User level must not be empty.";
                    this.lblValidationUserProfiles.Attributes["style"] = "color:Red;";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalUserProfiles').modal('show');</script>", false);
                }

                else if (this.ddlUserRole.SelectedItem.Text == "Choose a user role...")
                {
                    this.lblValidationUserProfiles.Text = "User role must not be empty.";
                    this.lblValidationUserProfiles.Attributes["style"] = "color:Red;";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalUserProfiles').modal('show');</script>", false);
                }

                //else if (this.txtUserID.Text == strUserID2 && this.txtUserName.Text == strFullName && this.ddUserLevel.Text == intUserLevel2.ToString() && this.rbUserStatus.Text == intUserStatus.ToString() && this.rbLoginStatus.Text == intLoginStatus.ToString())
                //{
                //    this.lblValidation.InnerText = "Update failed because no changes has been detected.";
                //    this.lblValidation.Attributes["style"] = "color:red;";
                //    this.popupuserprofile.Show(); 
                //}

                else
                {
                    upBO.networkid = this.txtUserID.Text.Trim();
                    upBO.fullname = this.txtUserName.Text.Trim();
                    upBO.emailaddress = this.txtEmailAddress.Text.Trim();
                    upBO.department = this.txtDepartment.Text.Trim();
                    upBO.userlevel = Convert.ToInt32(this.ddlUserLevel.SelectedValue);
                    upBO.userrole = Convert.ToInt32(this.ddlUserRole.SelectedValue);
                    upBO.userstatus = Convert.ToInt32(this.rdoUserStatus.SelectedValue);
                    upBO.loginstatus = Convert.ToInt32(this.rdoLoginStatus.SelectedValue);

                    //Update
                    string strUpdate = upBL.UserProfiles_Post(upBO, strUserID);

                    //System Logs
                    strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "User Profiles - Update", "Updated user profile" + " " + this.txtUserID.Text.Trim(), strUserID);

                    //Alert
                    ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Record updated.', '', 'success') .then((value) => { window.location.href = 'UserProfiles.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole + "'; }); ", true);
                }
            }

            catch (Exception ex)
            {
                //Error Message
                this.divErrorMessage.Visible = true;
                this.lblErrorMessage.InnerText = ex.Message;
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "User Profiles - Update", ex.Message, strUserID);

                //Page Title
                this.divPageTitle.Visible = false;

                //Button Controls
                this.divButtonControls.Visible = false;

                //User Profiles
                this.divUserProfiles.Visible = false;
            }
        }

        //Error Message
        protected void btnReload_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            Response.Redirect("UserProfiles.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

    }
}