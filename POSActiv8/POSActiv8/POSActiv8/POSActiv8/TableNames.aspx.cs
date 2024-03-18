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
using System.IO;
using OfficeOpenXml;
using MySql.Data.MySqlClient;

using BusinessObject;
using BusinessLogic;
using DataAccess;
using POSActiv8.Classes;

namespace POSActiv8
{
    public partial class TableNames : System.Web.UI.Page
    {
        baseclass baseclass = new baseclass();
        SystemLogsBL slBL = new SystemLogsBL();
        ErrorLogsBL elBL = new ErrorLogsBL();
        SequenceBL seqBL = new SequenceBL();
        SystemParametersBL sysparamBL = new SystemParametersBL();
        TableNamesBO tblnamesBO = new TableNamesBO();
        TableNamesBL tblnamesBL = new TableNamesBL();
        FloorLocationBL floorlocBL = new FloorLocationBL();
        string strUserID;
        int intUserLevel;
        int intUserRole;
        string strSystemLogs;
        string strErrorLogs;

        public void FloorLocation()
        {
            //Floor Location
            using (MySqlDataReader drFLOORLOCATION = floorlocBL.FloorLocation_View("0", string.Empty))
            {
                this.ddlFloorLocation.Items.Clear();
                this.ddlFloorLocation.DataSource = drFLOORLOCATION;
                this.ddlFloorLocation.DataValueField = "FloorLocationCode";
                this.ddlFloorLocation.DataTextField = "FloorName";
                this.ddlFloorLocation.DataBind();
            }

            this.ddlFloorLocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Floor", string.Empty));
            this.ddlFloorLocation.SelectedIndex = 0;
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
                this.btnCreate.Visible = true; //baseclass.ButtonControls(intUserLevel, intUserRole, 1, "Create");

                try
                {
                    //Table Names
                    using (MySqlDataReader drTABLENAMES = tblnamesBL.TableNames_View("0", string.Empty))
                    {
                        if (drTABLENAMES.HasRows)
                        {
                            //Gridview
                            this.gvTableNames.DataSource = drTABLENAMES;
                            this.gvTableNames.DataBind();
                            this.gvTableNames.Visible = true;

                            //Count
                            this.lblTableNames.InnerText = "Showing " + string.Format("{0:n0}", this.gvTableNames.Rows.Count) + " records";

                            //Button Controls
                            this.btnExport.Visible = true;
                            this.btnCreate.Visible = true;
                        }

                        else
                        {
                            //Gridview
                            this.gvTableNames.Visible = false;

                            //Count
                            this.lblTableNames.InnerText = "No records found";

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
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Table Names - Page Load", ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //Table Names
                    this.divTableNames.Visible = false;
                }
            }
        }

        //Griview
        protected void gvTableNames_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);
            string strTableCode = e.CommandArgument.ToString().Trim();

            try
            {
                this.lblTableNamesTitle.Text = "Table Name Details";

                //Table Names
                using (MySqlDataReader drTABLENAMES = tblnamesBL.TableNames_View(strTableCode, string.Empty))
                {
                    if (drTABLENAMES.Read())
                    {
                        this.txtTableCode.Value = drTABLENAMES["TableCode"].ToString().Trim();
                        this.txtTableCode.Disabled = true;
                        this.txtTableName.Value = drTABLENAMES["TableName"].ToString().Trim();
                        int intRecordStatus = Convert.ToInt32(drTABLENAMES["RecordStatus"]);

                        if (intRecordStatus == 1)
                        {
                            this.rbStatusActive.Checked = true;
                            this.rbStatusInactive.Checked = false;
                        }

                        else
                        {
                            this.rbStatusActive.Checked = false;
                            this.rbStatusInactive.Checked = true;
                        }
                    }

                    this.txtTableName.Focus();
                }

                //Button Controls
                this.btnCancel.Visible = true;
                this.btnSave.Visible = false;
                this.btnUpdate.Visible = true;
                //this.btnUpdateUserProfiles.Visible = baseclass.ButtonControls(intUserLevel, intUserRole, 1, "Update");

                //Show Modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            catch (Exception ex)
            {
                //Error Message
                this.divErrorMessage.Visible = true;
                this.lblErrorMessage.InnerText = ex.Message;
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Table Names - View Details", ex.Message, strUserID);

                //Page Title
                this.divPageTitle.Visible = false;

                //Button Controls
                this.divButtonControls.Visible = false;

                //Table Names
                this.divTableNames.Visible = false;
            }
        }

        //Button Controls
        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];

            try
            {
                //Table Names
                using (MySqlDataReader drTABLENAMES = tblnamesBL.TableNames_View("0", this.txtSearch.Text.Trim()))
                {
                    if (drTABLENAMES.HasRows)
                    {
                        //Gridview
                        this.gvTableNames.DataSource = drTABLENAMES;
                        this.gvTableNames.DataBind();
                        this.gvTableNames.Visible = true;

                        //Count
                        this.lblTableNames.InnerText = "Showing " + string.Format("{0:n0}", this.gvTableNames.Rows.Count) + " records";

                        //Button Controls
                        this.btnExport.Visible = true;
                        this.btnCreate.Visible = true;
                    }

                    else
                    {
                        //Gridview
                        this.gvTableNames.Visible = false;

                        //Count
                        this.lblTableNames.InnerText = "No records found";

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
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Table Names - Search", ex.Message, strUserID);

                //Page Title
                this.divPageTitle.Visible = false;

                //Button Controls
                this.divButtonControls.Visible = false;

                //Table Names
                this.divTableNames.Visible = false;
            }
        }

        protected void btnCreate_Click(object sender, System.EventArgs e)
        {
            this.lblTableNamesTitle.Text = "New Table";

            this.txtTableCode.Disabled = true;
            this.txtTableName.Focus();
            this.txtTableName.Value = string.Empty;
            this.ddlFloorLocation.ClearSelection();
            FloorLocation();
            this.lblValidationMessage.Text = string.Empty;

            //Button Controls
            this.btnCancel.Visible = true;
            this.btnSave.Visible = true;
            this.btnUpdate.Visible = false;

            //Show Modal
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            //strUserID = Request.QueryString["userid"];
            //intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            //intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            //try
            //{
            //    //Reports Generated
            //    string strReportsGeneratedPath = HttpContext.Current.Server.MapPath(string.Format("~/ReportsGenerated/"));

            //    //Reference Date
            //    DateTime dteCurrentDate = System.DateTime.Now;
            //    string strCurrentDate = dteCurrentDate.ToString("MMddyyyy");

            //    //Source File
            //    var strFileName = "Equipments_" + strCurrentDate + ".xlsx";
            //    var strSaveEquipments = new FileInfo(Path.Combine(strReportsGeneratedPath, strFileName));

            //    //Check if file exist then delete
            //    if (System.IO.File.Exists(strSaveEquipments.ToString()))
            //    {
            //        System.IO.File.Delete(strSaveEquipments.ToString());
            //    }

            //    //Create Package
            //    using (var package = new ExcelPackage(strSaveEquipments))
            //    {
            //        //Equipments
            //        using (SqlDataReader drEQUIPMENTS = equipBL.Equipments_View(0, string.Empty))
            //        {
            //            //Worksheet
            //            ExcelWorksheet worksheetEQUIPMENTS = package.Workbook.Worksheets.Add("Equipments");

            //            //Header
            //            worksheetEQUIPMENTS.Cells[1, 1].Value = "Asset Tag";
            //            worksheetEQUIPMENTS.Cells[1, 2].Value = "Equipment Name";
            //            worksheetEQUIPMENTS.Cells[1, 3].Value = "Capacity";
            //            worksheetEQUIPMENTS.Cells[1, 4].Value = "Brand";
            //            worksheetEQUIPMENTS.Cells[1, 5].Value = "Model";
            //            worksheetEQUIPMENTS.Cells[1, 6].Value = "Date and Time Created";
            //            worksheetEQUIPMENTS.Cells[1, 7].Value = "Created By";
            //            worksheetEQUIPMENTS.Cells[1, 8].Value = "Date and Time Updated";
            //            worksheetEQUIPMENTS.Cells[1, 9].Value = "Updated By";
            //            worksheetEQUIPMENTS.Cells[1, 1, 1, 9].Style.Font.Bold = true;

            //            int intCounter = 0;

            //            while (drEQUIPMENTS.Read())
            //            {
            //                //Generate Rows
            //                worksheetEQUIPMENTS.Cells[2 + intCounter, 1].Value = drEQUIPMENTS["AssetTag"].ToString().Trim();
            //                worksheetEQUIPMENTS.Cells[2 + intCounter, 2].Value = drEQUIPMENTS["EquipmentName"].ToString().Trim();
            //                worksheetEQUIPMENTS.Cells[2 + intCounter, 3].Value = drEQUIPMENTS["Capacity"].ToString().Trim();
            //                worksheetEQUIPMENTS.Cells[2 + intCounter, 4].Value = drEQUIPMENTS["BrandName"].ToString().Trim();
            //                worksheetEQUIPMENTS.Cells[2 + intCounter, 5].Value = drEQUIPMENTS["ModelName"].ToString().Trim();
            //                worksheetEQUIPMENTS.Cells[2 + intCounter, 6].Value = drEQUIPMENTS["DateTimeCreated"].ToString().Trim();
            //                worksheetEQUIPMENTS.Cells[2 + intCounter, 7].Value = drEQUIPMENTS["CreatedBy"].ToString().Trim();
            //                worksheetEQUIPMENTS.Cells[2 + intCounter, 8].Value = drEQUIPMENTS["DateTimeUpdated"].ToString().Trim();
            //                worksheetEQUIPMENTS.Cells[2 + intCounter, 9].Value = drEQUIPMENTS["UpdatedBy"].ToString().Trim();

            //                intCounter++;
            //            }

            //            if (intCounter == 0)
            //            {
            //                worksheetEQUIPMENTS.Cells[2 + intCounter, 1].Value = "No records found";
            //            }

            //            //Column Format
            //            worksheetEQUIPMENTS.Cells[worksheetEQUIPMENTS.Dimension.Address].AutoFitColumns();

            //            //Font Size
            //            worksheetEQUIPMENTS.Cells[worksheetEQUIPMENTS.Dimension.Address].Style.Font.Size = 10;

            //            //Save the file
            //            package.Save();

            //            //System Logs
            //            strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Equipments - Export", "Export successfully generated.", strUserID);

            //            //Download the report file
            //            string filePath = strSaveEquipments.ToString();
            //            Response.ContentType = ContentType;
            //            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            //            Response.WriteFile(filePath);
            //            Response.End();
            //        }
            //    }
            //}

            //catch (Exception ex)
            //{
            //    //Error Message
            //    this.divErrorMessage.Visible = true;
            //    this.lblErrorMessage.InnerText = ex.Message;
            //    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Equipments - Export", ex.Message, strUserID);

            //    //Page Title
            //    this.divPageTitle.Visible = false;

            //    //Button Controls
            //    this.divButtonControls.Visible = false;

            //    //Equipments
            //    this.divEquipments.Visible = false;
            //}
        }

        //Item Category
        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);
            
            if (this.txtTableName.Value == string.Empty)
            {
                this.lblValidationMessage.Text = "Table name must not be empty.";
                this.lblValidationMessage.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else if (this.ddlFloorLocation.SelectedItem.Text == "Select Floor")
            {
                this.lblValidationMessage.Text = "Floor location must not be empty.";
                this.lblValidationMessage.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else
            {
                try
                {
                    //Check if Table Name already exist
                    System.Data.DataSet dsTABLENAME = new System.Data.DataSet();
                    dsTABLENAME = DBHelper.GetData("SELECT RecordID, TableCode, TableName FROM TableNames WHERE TableName = '" + this.txtTableName.Value.Trim() + "' AND FloorLocation = '" + this.ddlFloorLocation.SelectedValue + "'");

                    if (dsTABLENAME.Tables[0].Rows.Count > 0)
                    {
                        this.lblValidationMessage.Text = "Table name already exist.";
                        this.lblValidationMessage.Attributes["style"] = "color:Red;";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
                    }

                    else
                    {
                        tblnamesBO.tablecode = seqBL.ControlNumber_Generate(6);
                        tblnamesBO.tablename = this.txtTableName.Value.Trim();
                        tblnamesBO.floorlocationcode = this.ddlFloorLocation.SelectedValue;

                        int intRecordStatus = 0;

                        if (this.rbStatusActive.Checked)
                        {
                            intRecordStatus = 1;
                        }

                        else
                        {
                            intRecordStatus = 0;
                        }

                        tblnamesBO.recordstatus = intRecordStatus;
                        
                        //Save
                        string strSave = tblnamesBL.TableNames_Post(tblnamesBO, strUserID);

                        //System Logs
                        strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Table Names - Save", "Created new table " + this.txtTableName.Value.Trim(), strUserID);

                        //Redirect
                        ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Record saved', 'Table ' + '" + this.txtTableName.Value + "' + ' for ' + '" + this.ddlFloorLocation.SelectedItem.Text + "' + ' has been created', 'success') .then((value) => { window.location.href = 'TableNames.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole + "'; }); ", true);
                    }

                    dsTABLENAME.Clear();
                }

                catch (Exception ex)
                {
                    //Error Message
                    this.divErrorMessage.Visible = true;
                    this.lblErrorMessage.InnerText = ex.Message;
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Table Names - Save", ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //Table Names
                    this.divTableNames.Visible = false;
                }
            }
        }

        protected void btnUpdate_Click(object sender, System.EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            try
            {
                //Check for changes on the records
                System.Data.DataSet dsTABLENAMES = new System.Data.DataSet();
                dsTABLENAMES = DBHelper.GetData("SELECT RecordID, TableCode, TableNames, RecordStatus FROM TableNames WHERE TableCode = '" + this.txtTableName.Value.Trim() + "' ");

                string strTableCode = dsTABLENAMES.Tables[0].Rows[0]["FloorLocationCode"].ToString().Trim();
                string strTableName = dsTABLENAMES.Tables[0].Rows[0]["FloorName"].ToString().Trim();
                int intRecordStatuss = Convert.ToInt32(dsTABLENAMES.Tables[0].Rows[0]["RecordStatus"]);

                if (this.txtTableName.Value == string.Empty)
                {
                    this.lblValidationMessage.Text = "Table name must not be empty.";
                    this.lblValidationMessage.Attributes["style"] = "color:Red;";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
                }

                else if (this.txtTableName.Value == strTableName)
                { 
                    this.lblValidationMessage.Text = "Update failed because no changes has been detected.";
                    this.lblValidationMessage.Attributes["style"] = "color:red;";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
                }

                else
                {
                    tblnamesBO.tablecode = this.txtTableCode.Value.Trim();
                    tblnamesBO.tablename = this.txtTableName.Value.Trim();

                    int intRecordStatus = 0;

                    if (this.rbStatusActive.Checked)
                    {
                        intRecordStatus = 1;
                    }

                    else
                    {
                        intRecordStatus = 0;
                    }

                    tblnamesBO.recordstatus = intRecordStatus;

                    //Update
                    string strUpdate = tblnamesBL.TableNames_Post(tblnamesBO, strUserID);

                    //System Logs
                    strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Table Names - Update", "Updated table name " + this.txtTableName.Value.Trim(), strUserID);

                    //Redirect
                    ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Record updated', 'Table ' + '" + this.txtTableName.Value + "' +  ' has been updated', 'success') .then((value) => { window.location.href = 'TableNames.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole + "'; }); ", true);
                }
            }

            catch (Exception ex)
            {
                //Error Message
                this.divErrorMessage.Visible = true;
                this.lblErrorMessage.InnerText = ex.Message;
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Table Names - Update", ex.Message, strUserID);

                //Page Title
                this.divPageTitle.Visible = false;

                //Button Controls
                this.divButtonControls.Visible = false;

                //Table Names
                this.divTableNames.Visible = false;
            }
        }

        //Error Message
        protected void btnReload_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            Response.Redirect("TableNames.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }
        
    }
}