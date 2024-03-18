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
    public partial class ItemMaster : System.Web.UI.Page
    {
        baseclass baseclass = new baseclass();
        SystemLogsBL slBL = new SystemLogsBL();
        ErrorLogsBL elBL = new ErrorLogsBL();
        SequenceBL seqBL = new SequenceBL();
        SystemParametersBL sysparamBL = new SystemParametersBL();
        ItemCategoryBL itemcatBL = new ItemCategoryBL();
        ItemSubCategoryBL itemsubcatBL = new ItemSubCategoryBL();
        ItemMasterBO itemmastBO = new ItemMasterBO();
        ItemMasterBL itemmastBL = new ItemMasterBL();
        string strUserID;
        int intUserLevel;
        int intUserRole;
        string strSystemLogs;
        string strErrorLogs;

        public void ItemCategory()
        {
            //Item Category
            using (MySqlDataReader drITEMCATEGORY = itemcatBL.ItemCategory_View(0, string.Empty))
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

        public void ItemSubCategory()
        {
            //Item Category
            using (MySqlDataReader drITEMSUBCATEGORY = itemsubcatBL.ItemSubCategory_View(1, this.ddlItemCategory.SelectedValue))
            {
                this.ddlItemSubCategory.Items.Clear();
                this.ddlItemSubCategory.DataSource = drITEMSUBCATEGORY;
                this.ddlItemSubCategory.DataValueField = "SubCategoryCode";
                this.ddlItemSubCategory.DataTextField = "SubCategoryName";
                this.ddlItemSubCategory.DataBind();
            }

            this.ddlItemSubCategory.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select SubCategory", string.Empty));
            this.ddlItemSubCategory.SelectedIndex = 0;
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
                    //Item Master
                    using (MySqlDataReader drITEMMASTER = itemmastBL.ItemMaster_View("0", string.Empty))
                    {
                        if (drITEMMASTER.HasRows)
                        {
                            //Gridview
                            this.gvItemMaster.DataSource = drITEMMASTER;
                            this.gvItemMaster.DataBind();
                            this.gvItemMaster.Visible = true;

                            //Count
                            this.lblItemMaster.InnerText = "Showing " + string.Format("{0:n0}", this.gvItemMaster.Rows.Count) + " records";

                            //Button Controls
                            this.btnExport.Visible = true;
                            this.btnCreate.Visible = true;
                        }

                        else
                        {
                            //Gridview
                            this.gvItemMaster.Visible = false;

                            //Count
                            this.lblItemMaster.InnerText = "No records found";

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
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Item Master - Page Load", ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //Item Master
                    this.divItemMaster.Visible = false;
                }
            }
        }

        //Griview
        protected void gvItemMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);
            string strItemCode = e.CommandArgument.ToString().Trim();

            try
            {
                this.lblItemMasterDetails.Text = "Item Details";

                ItemCategory();

                //Item Master
                using (MySqlDataReader drITEMMASTER = itemmastBL.ItemMaster_View(strItemCode, string.Empty))
                {
                    if (drITEMMASTER.Read())
                    {
                        this.txtItemCode.Value = drITEMMASTER["ItemCode"].ToString().Trim();
                        this.txtItemCode.Disabled = true;
                        this.txtItemName.Value = drITEMMASTER["ItemName"].ToString().Trim();
                        this.txtItemDescription.Value = drITEMMASTER["ItemDescription"].ToString().Trim();
                        this.ddlItemCategory.Text = drITEMMASTER["ItemCategory"].ToString().Trim();
                        ItemSubCategory();
                        this.ddlItemSubCategory.Text = drITEMMASTER["ItemSubCategory"].ToString().Trim();
                        this.txtItemPrice.Value = drITEMMASTER["ItemPrice"].ToString().Trim();

                        int intItemStatus = Convert.ToInt32(drITEMMASTER["ItemStatus"]);

                        if (intItemStatus == 1)
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

                    this.txtItemName.Focus();
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
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Item Master - View Details", ex.Message, strUserID);

                //Page Title
                this.divPageTitle.Visible = false;

                //Button Controls
                this.divButtonControls.Visible = false;

                //Item Master
                this.divItemMaster.Visible = false;
            }
        }

        //Button Controls
        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];

            try
            {
                //Item Master
                using (MySqlDataReader drITEMMASTER = itemmastBL.ItemMaster_View("0", this.txtSearch.Text.Trim()))
                {
                    if (drITEMMASTER.HasRows)
                    {
                        //Gridview
                        this.gvItemMaster.DataSource = drITEMMASTER;
                        this.gvItemMaster.DataBind();
                        this.gvItemMaster.Visible = true;

                        //Count
                        this.lblItemMaster.InnerText = "Showing " + string.Format("{0:n0}", this.gvItemMaster.Rows.Count) + " records";

                        //Button Controls
                        this.btnExport.Visible = true;
                        this.btnCreate.Visible = true;
                    }

                    else
                    {
                        //Gridview
                        this.gvItemMaster.Visible = false;

                        //Count
                        this.lblItemMaster.InnerText = "No records found";

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
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Item Master - Search", ex.Message, strUserID);

                //Page Title
                this.divPageTitle.Visible = false;

                //Button Controls
                this.divButtonControls.Visible = false;

                //Item Master
                this.divItemMaster.Visible = false;
            }
        }

        protected void btnCreate_Click(object sender, System.EventArgs e)
        {
            ItemCategory();
            this.lblItemMasterDetails.Text = "New Item";

            this.txtItemCode.Value = String.Empty;
            this.txtItemCode.Disabled = true;
            this.txtItemName.Focus();
            this.txtItemName.Value = string.Empty;
            this.txtItemDescription.Value = string.Empty;
            this.ddlItemCategory.ClearSelection();
            this.ddlItemSubCategory.ClearSelection();
            this.ddlItemSubCategory.Enabled = false;
            this.txtItemPrice.Value = string.Empty;
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

        //Item Master Details
        protected void ddlItemCategory_TextChanged(object sender, EventArgs e)
        {
            ItemSubCategory();
            this.ddlItemSubCategory.Enabled = true;
            this.ddlItemSubCategory.Focus();

            //Show Modal
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
        }

        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);
            
            if (this.txtItemName.Value == string.Empty)
            {
                this.lblValidationMessage.Text = "Item name must not be empty.";
                this.lblValidationMessage.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else if (this.txtItemDescription.Value == string.Empty)
            {
                this.lblValidationMessage.Text = "Item description must not be empty.";
                this.lblValidationMessage.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else if (this.ddlItemCategory.SelectedItem.Text == "Select Category")
            {
                this.lblValidationMessage.Text = "Item category must not be empty.";
                this.lblValidationMessage.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else if (this.ddlItemSubCategory.SelectedItem.Text == "Select SubCategory")
            {
                this.lblValidationMessage.Text = "Item category must not be empty.";
                this.lblValidationMessage.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else if (this.txtItemPrice.Value == string.Empty)
            {
                this.lblValidationMessage.Text = "Item price must not be empty.";
                this.lblValidationMessage.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else if (this.txtItemPrice.Value == "0.00" || this.txtItemPrice.Value == "0" || this.txtItemPrice.Value == ".00")
            {
                this.lblValidationMessage.Text = "Item price must not be equal to zeroes.";
                this.lblValidationMessage.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else
            {
                try
                {
                    //Check if Item already exist
                    System.Data.DataSet dsITEMMASTER = new System.Data.DataSet();
                    dsITEMMASTER = DBHelper.GetData("SELECT RecordID, ItemName FROM ItemMaster WHERE ItemName = '" + this.txtItemName.Value.Trim() + "'");

                    if (dsITEMMASTER.Tables[0].Rows.Count > 0)
                    {
                        this.lblValidationMessage.Text = "Item already exist.";
                        this.lblValidationMessage.Attributes["style"] = "color:Red;";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
                    }

                    else
                    {
                        itemmastBO.itemcode = seqBL.ControlNumber_Generate(1);
                        itemmastBO.itemname = this.txtItemName.Value.Trim();
                        itemmastBO.itemdescription = this.txtItemDescription.Value.Trim();
                        itemmastBO.itemcategory = this.ddlItemCategory.SelectedValue;
                        itemmastBO.itemsubcategory = this.ddlItemSubCategory.SelectedValue;
                        itemmastBO.itemprice = Convert.ToDecimal(this.txtItemPrice.Value);

                        int intItemStatus = 0;

                        if (this.rbStatusActive.Checked)
                        {
                            intItemStatus = 1;
                        }

                        else
                        {
                            intItemStatus = 0;
                        }

                        itemmastBO.itemstatus = intItemStatus;
                        
                        //Save
                        string strSave = itemmastBL.ItemMaster_Post(itemmastBO, strUserID);

                        //System Logs
                        strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Item Master - Save", "Created new item " + this.txtItemName.Value.Trim(), strUserID);

                        //Redirect
                        ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Record saved', '" + this.txtItemName.Value + "' +  ' has been created', 'success') .then((value) => { window.location.href = 'ItemMaster.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole + "'; }); ", true);
                    }

                    dsITEMMASTER.Clear();
                }

                catch (Exception ex)
                {
                    //Error Message
                    this.divErrorMessage.Visible = true;
                    this.lblErrorMessage.InnerText = ex.Message;
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Item Master - Save", ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //Item Master
                    this.divItemMaster.Visible = false;
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
                dsITEMS = DBHelper.GetData("SELECT RecordID, ItemCode, ItemName, ItemDescription, ItemCategory, ItemSubCategory, ItemPrice, ItemStatus FROM ItemMaster WHERE ItemCode = '" + this.txtItemCode.Value.Trim() + "' ");

                string strItemName = dsITEMS.Tables[0].Rows[0]["ItemName"].ToString().Trim();
                string strItemDescription = dsITEMS.Tables[0].Rows[0]["ItemDescription"].ToString().Trim();
                string strItemCategory = dsITEMS.Tables[0].Rows[0]["ItemCategory"].ToString().Trim();
                string strItemSubCategory = dsITEMS.Tables[0].Rows[0]["ItemSubCategory"].ToString().Trim();
                string strItemPrice = dsITEMS.Tables[0].Rows[0]["ItemPrice"].ToString().Trim();
                int intItemStatuss = Convert.ToInt32(dsITEMS.Tables[0].Rows[0]["ItemStatus"]);

                if (this.txtItemName.Value == string.Empty)
                {
                    this.lblValidationMessage.Text = "Item name must not be empty.";
                    this.lblValidationMessage.Attributes["style"] = "color:Red;";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
                }

                else if (this.txtItemDescription.Value == string.Empty)
                {
                    this.lblValidationMessage.Text = "Item description must not be empty.";
                    this.lblValidationMessage.Attributes["style"] = "color:Red;";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
                }

                else if (this.ddlItemCategory.SelectedItem.Text == "Select Category")
                {
                    this.lblValidationMessage.Text = "Item category must not be empty.";
                    this.lblValidationMessage.Attributes["style"] = "color:Red;";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
                }

                else if (this.ddlItemSubCategory.SelectedItem.Text == "Select SubCategory")
                {
                    this.lblValidationMessage.Text = "Item category must not be empty.";
                    this.lblValidationMessage.Attributes["style"] = "color:Red;";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
                }

                else if (this.txtItemPrice.Value == string.Empty)
                {
                    this.lblValidationMessage.Text = "Item price must not be empty.";
                    this.lblValidationMessage.Attributes["style"] = "color:Red;";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
                }

                else if (this.txtItemPrice.Value == "0.00" || this.txtItemPrice.Value == "0" || this.txtItemPrice.Value == ".00")
                {
                    this.lblValidationMessage.Text = "Item price must not be equal to zeroes.";
                    this.lblValidationMessage.Attributes["style"] = "color:Red;";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
                }

                //else if (this.txtItemName.Value == strItemName && this.txtItemDescription.Value == strItemDescription  && this.ddlItemCategory.Text == strItemCategory && this.ddlItemSubCategory.Text == strItemSubCategory && this.txtItemPrice.Value == strItemPrice)
                //{
                //    this.lblValidationMessage.Text = "Update failed because no changes has been detected.";
                //    this.lblValidationMessage.Attributes["style"] = "color:red;";
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
                //}

                else
                {
                    itemmastBO.itemcode = this.txtItemCode.Value.Trim();
                    itemmastBO.itemname = this.txtItemName.Value.Trim();
                    itemmastBO.itemdescription = this.txtItemDescription.Value.Trim();
                    itemmastBO.itemcategory = this.ddlItemCategory.SelectedValue;
                    itemmastBO.itemsubcategory = this.ddlItemSubCategory.SelectedValue;
                    itemmastBO.itemprice = Convert.ToDecimal(this.txtItemPrice.Value);

                    int intItemStatus = 0;

                    if (this.rbStatusActive.Checked)
                    {
                        intItemStatus = 1;
                    }

                    else
                    {
                        intItemStatus = 0;
                    }

                    itemmastBO.itemstatus = intItemStatus;

                    //Update
                    string strUpdate = itemmastBL.ItemMaster_Post(itemmastBO, strUserID);

                    //System Logs
                    strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Item Master - Update", "Updated item " + this.txtItemName.Value.Trim(), strUserID);

                    //Redirect
                    ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Record updated', '" + this.txtItemName.Value + "' +  ' has been updated', 'success') .then((value) => { window.location.href = 'ItemMaster.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole + "'; }); ", true);
                }
            }

            catch (Exception ex)
            {
                //Error Message
                this.divErrorMessage.Visible = true;
                this.lblErrorMessage.InnerText = ex.Message;
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Item Master - Update", ex.Message, strUserID);

                //Page Title
                this.divPageTitle.Visible = false;

                //Button Controls
                this.divButtonControls.Visible = false;

                //Item Master
                this.divItemMaster.Visible = false;
            }
        }

        //Error Message
        protected void btnReload_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            Response.Redirect("ItemMaster.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        
    }
}