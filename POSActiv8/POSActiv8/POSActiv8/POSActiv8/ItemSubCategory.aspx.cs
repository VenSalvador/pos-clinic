﻿using System;
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
    public partial class ItemSubCategory : System.Web.UI.Page
    {
        baseclass baseclass = new baseclass();
        SystemLogsBL slBL = new SystemLogsBL();
        ErrorLogsBL elBL = new ErrorLogsBL();
        SequenceBL seqBL = new SequenceBL();
        SystemParametersBL sysparamBL = new SystemParametersBL();
        ItemSubCategoryBO itemsubcatBO = new ItemSubCategoryBO();
        ItemSubCategoryBL itemsubcatBL = new ItemSubCategoryBL();
        ItemCategoryBL itemcatBL = new ItemCategoryBL();
        string strUserID;
        int intUserLevel;
        int intUserRole;
        string strSystemLogs;
        string strErrorLogs;

        public void ItemCategory()
        {
            //Item Category
            using (SqlDataReader drITEMCATEGORY = itemcatBL.ItemCategory_View(0, string.Empty))
            {
                this.ddlItemCategory.Items.Clear();
                this.ddlItemCategory.DataSource = drITEMCATEGORY;
                this.ddlItemCategory.DataValueField = "CategoryCode";
                this.ddlItemCategory.DataTextField = "CategoryName";
                this.ddlItemCategory.DataBind();
            }

            this.ddlItemCategory.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Category", string.Empty));
            this.ddlItemCategory.SelectedIndex = 0;
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
                    //Item SubCategory
                    using (SqlDataReader drITEMSUBCATEGORY = itemsubcatBL.ItemSubCategory_View(0, string.Empty))
                    {
                        if (drITEMSUBCATEGORY.HasRows)
                        {
                            //Gridview
                            this.gvItemSubCateogry.DataSource = drITEMSUBCATEGORY;
                            this.gvItemSubCateogry.DataBind();
                            this.gvItemSubCateogry.Visible = true;

                            //Count
                            this.lblItemSubCategory.InnerText = "Showing " + string.Format("{0:n0}", this.gvItemSubCateogry.Rows.Count) + " records";

                            //Button Controls
                            this.btnExport.Visible = true;
                            this.btnCreate.Visible = true;
                        }

                        else
                        {
                            //Gridview
                            this.gvItemSubCateogry.Visible = false;

                            //Count
                            this.lblItemSubCategory.InnerText = "No records found";

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
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Item SubCategory - Page Load", ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //Item SubCategory
                    this.divItemSubCategory.Visible = false;
                }
            }
        }

        //Griview
        protected void gvItemSubCategory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);
            int intRecordID = Convert.ToInt32(e.CommandArgument);

            try
            {
                ItemCategory();

                this.lblItemSubCategoryDetails.Text = "Item SubCategory Details";

                //Item SubCategory
                using (SqlDataReader drITEMSUBCATEGORY = itemsubcatBL.ItemSubCategory_View(intRecordID, string.Empty))
                {
                    if (drITEMSUBCATEGORY.Read())
                    {
                        this.txtSubCategoryCode.Value = drITEMSUBCATEGORY["SubCategoryCode"].ToString().Trim();
                        this.txtSubCategoryCode.Disabled = true;
                        this.txtSubCategoryName.Value = drITEMSUBCATEGORY["SubCategoryName"].ToString().Trim();
                        this.ddlItemCategory.Text = drITEMSUBCATEGORY["ItemCategoryCode"].ToString().Trim();
                        int intRecordStatus = Convert.ToInt32(drITEMSUBCATEGORY["RecordStatus"]);

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

                    this.txtSubCategoryName.Focus();
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
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Item SubCategory - View Details", ex.Message, strUserID);

                //Page Title
                this.divPageTitle.Visible = false;

                //Button Controls
                this.divButtonControls.Visible = false;

                //Item SubCategory
                this.divItemSubCategory.Visible = false;
            }
        }

        //Button Controls
        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];

            try
            {
                //Item SubCategory
                using (SqlDataReader drITEMSUBCATEGORY = itemsubcatBL.ItemSubCategory_View(0, this.txtSearch.Text.Trim()))
                {
                    if (drITEMSUBCATEGORY.HasRows)
                    {
                        //Gridview
                        this.gvItemSubCateogry.DataSource = drITEMSUBCATEGORY;
                        this.gvItemSubCateogry.DataBind();
                        this.gvItemSubCateogry.Visible = true;

                        //Count
                        this.lblItemSubCategory.InnerText = "Showing " + string.Format("{0:n0}", this.gvItemSubCateogry.Rows.Count) + " records";

                        //Button Controls
                        this.btnExport.Visible = true;
                        this.btnCreate.Visible = true;
                    }

                    else
                    {
                        //Gridview
                        this.gvItemSubCateogry.Visible = false;

                        //Count
                        this.lblItemSubCategory.InnerText = "No records found";

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
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Item SubCategory - Search", ex.Message, strUserID);

                //Page Title
                this.divPageTitle.Visible = false;

                //Button Controls
                this.divButtonControls.Visible = false;

                //Item SubCategory
                this.divItemSubCategory.Visible = false;
            }
        }

        protected void btnCreate_Click(object sender, System.EventArgs e)
        {
            this.lblItemSubCategoryDetails.Text = "New SubCategory";

            this.txtSubCategoryCode.Disabled = true;
            this.txtSubCategoryName.Focus();
            this.txtSubCategoryName.Value = string.Empty;
            this.ddlItemCategory.ClearSelection();
            ItemCategory();
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
            
            if (this.txtSubCategoryName.Value == string.Empty)
            {
                this.lblValidationMessage.Text = "SubCategory name must not be empty.";
                this.lblValidationMessage.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else if (this.ddlItemCategory.SelectedItem.Text == "Select Category")
            {
                this.lblValidationMessage.Text = "Item category must not be empty.";
                this.lblValidationMessage.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else
            {
                try
                {
                    //Check if Item SubCategory already exist
                    System.Data.DataSet dsITEMCATEGORY = new System.Data.DataSet();
                    dsITEMCATEGORY = DBHelper.GetData("SELECT RecordID, SubCategoryCode, SubCategoryName FROM ItemSubCategory WHERE SubCategoryName = '" + this.txtSubCategoryName.Value.Trim() + "'");

                    if (dsITEMCATEGORY.Tables[0].Rows.Count > 0)
                    {
                        this.lblValidationMessage.Text = "SubCategory already exist.";
                        this.lblValidationMessage.Attributes["style"] = "color:Red;";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
                    }

                    else
                    {
                        itemsubcatBO.recordid = 0;
                        itemsubcatBO.subcategorycode = seqBL.ControlNumber_Generate(3);
                        itemsubcatBO.subcategoryname = this.txtSubCategoryName.Value.Trim();
                        itemsubcatBO.itemcategorycode = this.ddlItemCategory.SelectedValue;

                        int intRecordStatus = 0;

                        if (this.rbStatusActive.Checked)
                        {
                            intRecordStatus = 1;
                        }

                        else
                        {
                            intRecordStatus = 0;
                        }

                        itemsubcatBO.recordstatus = intRecordStatus;
                        
                        //Save
                        string strSave = itemsubcatBL.ItemSubCategory_Post(itemsubcatBO, strUserID);

                        //System Logs
                        strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Item SubCategory - Save", "Created new subcategory " + this.txtSubCategoryName.Value.Trim(), strUserID);

                        //Redirect
                        ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Record saved', '" + this.txtSubCategoryName.Value + "' +  ' has been created', 'success') .then((value) => { window.location.href = 'ItemSubCategory.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole + "'; }); ", true);
                    }

                    dsITEMCATEGORY.Clear();
                }

                catch (Exception ex)
                {
                    //Error Message
                    this.divErrorMessage.Visible = true;
                    this.lblErrorMessage.InnerText = ex.Message;
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Item SubCategory - Save", ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //Item SubCategory
                    this.divItemSubCategory.Visible = false;
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
                dsITEMS = DBHelper.GetData("SELECT RecordID, SubCategoryCode, SubCategoryName, ItemCategoryCode, RecordStatus FROM ItemSubCategory WHERE SubCategoryCode = '" + this.txtSubCategoryCode.Value.Trim() + "' ");

                string strSubCategoryCode = dsITEMS.Tables[0].Rows[0]["SubCategoryCode"].ToString().Trim();
                string strSubCategoryName = dsITEMS.Tables[0].Rows[0]["SubCategoryName"].ToString().Trim();
                string strItemCategoryCode = dsITEMS.Tables[0].Rows[0]["ItemCategoryCode"].ToString().Trim();
                int intRecordStatuss = Convert.ToInt32(dsITEMS.Tables[0].Rows[0]["RecordStatus"]);

                if (this.txtSubCategoryName.Value == string.Empty)
                {
                    this.lblValidationMessage.Text = "SubCategory name must not be empty.";
                    this.lblValidationMessage.Attributes["style"] = "color:Red;";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
                }

                else if (this.ddlItemCategory.SelectedItem.Text == "Select Category")
                {
                    this.lblValidationMessage.Text = "Item category must not be empty.";
                    this.lblValidationMessage.Attributes["style"] = "color:Red;";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
                }

                else if (this.txtSubCategoryName.Value == strSubCategoryName && this.ddlItemCategory.Text == strItemCategoryCode)
                {
                    this.lblValidationMessage.Text = "Update failed because no changes has been detected.";
                    this.lblValidationMessage.Attributes["style"] = "color:red;";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
                }

                else
                {
                    itemsubcatBO.subcategorycode = this.txtSubCategoryCode.Value.Trim();
                    itemsubcatBO.subcategoryname = this.txtSubCategoryName.Value.Trim();
                    itemsubcatBO.itemcategorycode = this.ddlItemCategory.SelectedValue;

                    int intRecordStatus = 0;

                    if (this.rbStatusActive.Checked)
                    {
                        intRecordStatus = 1;
                    }

                    else
                    {
                        intRecordStatus = 0;
                    }

                    itemsubcatBO.recordstatus = intRecordStatus;

                    //Update
                    string strUpdate = itemsubcatBL.ItemSubCategory_Post(itemsubcatBO, strUserID);

                    //System Logs
                    strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Item SubCategory - Update", "Updated item " + this.txtSubCategoryName.Value.Trim(), strUserID);

                    //Redirect
                    ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Record updated', '" + this.txtSubCategoryName.Value + "' +  ' has been updated', 'success') .then((value) => { window.location.href = 'ItemSubCategory.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole + "'; }); ", true);
                }
            }

            catch (Exception ex)
            {
                //Error Message
                this.divErrorMessage.Visible = true;
                this.lblErrorMessage.InnerText = ex.Message;
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Item SubCategory - Update", ex.Message, strUserID);

                //Page Title
                this.divPageTitle.Visible = false;

                //Button Controls
                this.divButtonControls.Visible = false;

                //Item SubCategory
                this.divItemSubCategory.Visible = false;
            }
        }

        //Error Message
        protected void btnReload_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            Response.Redirect("ItemSubCategory.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        
    }
}