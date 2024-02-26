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

using BusinessObject;
using BusinessLogic;
using DataAccess;
using POSActiv8.Classes;

namespace POSActiv8
{
    public partial class FloorLocation : System.Web.UI.Page
    {
        baseclass baseclass = new baseclass();
        SystemLogsBL slBL = new SystemLogsBL();
        ErrorLogsBL elBL = new ErrorLogsBL();
        SequenceBL seqBL = new SequenceBL();
        SystemParametersBL sysparamBL = new SystemParametersBL();
        FloorLocationBO floorlocBO = new FloorLocationBO();
        FloorLocationBL floorlocBL = new FloorLocationBL();
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
                    //Floor Location
                    using (SqlDataReader drFLOORLOCATION = floorlocBL.FloorLocation_View("0", string.Empty))
                    {
                        if (drFLOORLOCATION.HasRows)
                        {
                            //Gridview
                            this.gvFloorLocation.DataSource = drFLOORLOCATION;
                            this.gvFloorLocation.DataBind();
                            this.gvFloorLocation.Visible = true;

                            //Count
                            this.lblFloorLocation.InnerText = "Showing " + string.Format("{0:n0}", this.gvFloorLocation.Rows.Count) + " records";

                            //Button Controls
                            this.btnExport.Visible = true;
                            this.btnCreate.Visible = true;
                        }

                        else
                        {
                            //Gridview
                            this.gvFloorLocation.Visible = false;

                            //Count
                            this.lblFloorLocation.InnerText = "No records found";

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
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Floor Location - Page Load", ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //Floor Location
                    this.divFloorLocation.Visible = false;
                }
            }
        }

        //Griview
        protected void gvFloorLocation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);
            string strFloorLocationCode = e.CommandArgument.ToString().Trim();

            try
            {
                this.lblFloorLocaitonTitle.Text = "Floor Location Details";

                //Floor Location
                using (SqlDataReader drFLOORLOCATION = floorlocBL.FloorLocation_View(strFloorLocationCode, string.Empty))
                {
                    if (drFLOORLOCATION.Read())
                    {
                        this.txtFloorLocationCode.Value = drFLOORLOCATION["FloorLocationCode"].ToString().Trim();
                        this.txtFloorLocationCode.Disabled = true;
                        this.txtFloorName.Value = drFLOORLOCATION["FloorName"].ToString().Trim();
                        this.txtFloorColor.Value = drFLOORLOCATION["FloorColor"].ToString().Trim();
                        int intRecordStatus = Convert.ToInt32(drFLOORLOCATION["RecordStatus"]);

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

                    this.txtFloorName.Focus();
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
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Floor Location - View Details", ex.Message, strUserID);

                //Page Title
                this.divPageTitle.Visible = false;

                //Button Controls
                this.divButtonControls.Visible = false;

                //Floor Location
                this.divFloorLocation.Visible = false;
            }
        }

        //Button Controls
        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];

            try
            {
                //Floor Location
                using (SqlDataReader drFLOORLOCATION = floorlocBL.FloorLocation_View("0", this.txtSearch.Text.Trim()))
                {
                    if (drFLOORLOCATION.HasRows)
                    {
                        //Gridview
                        this.gvFloorLocation.DataSource = drFLOORLOCATION;
                        this.gvFloorLocation.DataBind();
                        this.gvFloorLocation.Visible = true;

                        //Count
                        this.lblFloorLocation.InnerText = "Showing " + string.Format("{0:n0}", this.gvFloorLocation.Rows.Count) + " records";

                        //Button Controls
                        this.btnExport.Visible = true;
                        this.btnCreate.Visible = true;
                    }

                    else
                    {
                        //Gridview
                        this.gvFloorLocation.Visible = false;

                        //Count
                        this.lblFloorLocation.InnerText = "No records found";

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
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Floor Location - Search", ex.Message, strUserID);

                //Page Title
                this.divPageTitle.Visible = false;

                //Button Controls
                this.divButtonControls.Visible = false;

                //Floor Location
                this.divFloorLocation.Visible = false;
            }
        }

        protected void btnCreate_Click(object sender, System.EventArgs e)
        {
            this.lblFloorLocaitonTitle.Text = "New Floor Location";

            this.txtFloorLocationCode.Value = String.Empty;
            this.txtFloorLocationCode.Disabled = true;
            this.txtFloorName.Focus();
            this.txtFloorName.Value = string.Empty;
            this.txtFloorColor.Value = String.Empty;
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
            
            if (this.txtFloorName.Value == string.Empty)
            {
                this.lblValidationMessage.Text = "Floor name must not be empty.";
                this.lblValidationMessage.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else
            {
                try
                {
                    //Check if Floor Location already exist
                    System.Data.DataSet dsFLOORLOCATION = new System.Data.DataSet();
                    dsFLOORLOCATION = DBHelper.GetData("SELECT RecordID, FloorLocationCode, FloorName FROM FloorLocation WHERE FloorName = '" + this.txtFloorName.Value.Trim() + "'");

                    if (dsFLOORLOCATION.Tables[0].Rows.Count > 0)
                    {
                        this.lblValidationMessage.Text = "Floor location already exist.";
                        this.lblValidationMessage.Attributes["style"] = "color:Red;";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
                    }

                    else
                    {
                        floorlocBO.floorlocationcode = seqBL.ControlNumber_Generate(5);
                        floorlocBO.floorname = this.txtFloorName.Value.Trim();
                        floorlocBO.floorcolor = this.txtFloorColor.Value.Trim();

                        int intRecordStatus = 0;

                        if (this.rbStatusActive.Checked)
                        {
                            intRecordStatus = 1;
                        }

                        else
                        {
                            intRecordStatus = 0;
                        }

                        floorlocBO.recordstatus = intRecordStatus;
                        
                        //Save
                        string strSave = floorlocBL.FloorLocation_Post(floorlocBO, strUserID);

                        //System Logs
                        strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Floor Location - Save", "Created new floor location " + this.txtFloorName.Value.Trim(), strUserID);

                        //Redirect
                        ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Record saved', '" + this.txtFloorName.Value + "' +  ' has been created', 'success') .then((value) => { window.location.href = 'FloorLocation.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole + "'; }); ", true);
                    }

                    dsFLOORLOCATION.Clear();
                }

                catch (Exception ex)
                {
                    //Error Message
                    this.divErrorMessage.Visible = true;
                    this.lblErrorMessage.InnerText = ex.Message;
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Item Category - Save", ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //Floor Location
                    this.divFloorLocation.Visible = false;
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
                System.Data.DataSet dsITEMS = new System.Data.DataSet();
                dsITEMS = DBHelper.GetData("SELECT RecordID, FloorLocationCode, FloorName, FloorColor, RecordStatus FROM FloorLocation WHERE FloorLocationCode = '" + this.txtFloorLocationCode.Value.Trim() + "' ");

                string strFloorLocationCode = dsITEMS.Tables[0].Rows[0]["FloorLocationCode"].ToString().Trim();
                string strFloorName = dsITEMS.Tables[0].Rows[0]["FloorName"].ToString().Trim();
                string strFloorColor = dsITEMS.Tables[0].Rows[0]["FloorColor"].ToString().Trim();
                int intRecordStatuss = Convert.ToInt32(dsITEMS.Tables[0].Rows[0]["RecordStatus"]);

                if (this.txtFloorName.Value == string.Empty)
                {
                    this.lblValidationMessage.Text = "Floor name must not be empty.";
                    this.lblValidationMessage.Attributes["style"] = "color:Red;";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
                }

                else if (this.txtFloorName.Value == strFloorName && this.txtFloorColor.Value == strFloorColor) //to include status in the validation
                {
                    this.lblValidationMessage.Text = "Update failed because no changes has been detected.";
                    this.lblValidationMessage.Attributes["style"] = "color:red;";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
                }

                else
                {
                    floorlocBO.floorlocationcode = this.txtFloorLocationCode.Value.Trim();
                    floorlocBO.floorname = this.txtFloorName.Value.Trim();
                    floorlocBO.floorcolor = this.txtFloorColor.Value.Trim();

                    int intRecordStatus = 0;

                    if (this.rbStatusActive.Checked)
                    {
                        intRecordStatus = 1;
                    }

                    else
                    {
                        intRecordStatus = 0;
                    }

                    floorlocBO.recordstatus = intRecordStatus;

                    //Update
                    string strUpdate = floorlocBL.FloorLocation_Post(floorlocBO, strUserID);

                    //System Logs
                    strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Floor Location - Update", "Updated floor location " + this.txtFloorName.Value.Trim(), strUserID);

                    //Redirect
                    ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Record updated', '" + this.txtFloorName.Value + "' +  ' has been updated', 'success') .then((value) => { window.location.href = 'FloorLocation.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole + "'; }); ", true);
                }
            }

            catch (Exception ex)
            {
                //Error Message
                this.divErrorMessage.Visible = true;
                this.lblErrorMessage.InnerText = ex.Message;
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Floor Location - Update", ex.Message, strUserID);

                //Page Title
                this.divPageTitle.Visible = false;

                //Button Controls
                this.divButtonControls.Visible = false;

                //Floor Location
                this.divFloorLocation.Visible = false;
            }
        }

        //Error Message
        protected void btnReload_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            Response.Redirect("FloorLocation.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        
    }
}