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
    public partial class Tax : System.Web.UI.Page
    {
        baseclass baseclass = new baseclass();
        SystemLogsBL slBL = new SystemLogsBL();
        ErrorLogsBL elBL = new ErrorLogsBL();
        SequenceBL seqBL = new SequenceBL();
        SystemParametersBL sysparamBL = new SystemParametersBL();
        TaxBO taxBO = new TaxBO();
        TaxBL taxBL = new TaxBL();
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
                    //Tax
                    using (MySqlDataReader drTAX = taxBL.Tax_View(0, string.Empty))
                    {
                        if (drTAX.HasRows)
                        {
                            //Gridview
                            this.gvTax.DataSource = drTAX;
                            this.gvTax.DataBind();
                            this.gvTax.Visible = true;

                            //Count
                            this.lblTax.InnerText = "Showing " + string.Format("{0:n0}", this.gvTax.Rows.Count) + " records";

                            //Button Controls
                            this.btnExport.Visible = true;
                            this.btnCreate.Visible = true;
                        }

                        else
                        {
                            //Gridview
                            this.gvTax.Visible = false;

                            //Count
                            this.lblTax.InnerText = "No records to display";

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
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Tax - Page Load", ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //Tax
                    this.divTax.Visible = false;
                }
            }
        }

        //Griview
        protected void gvTax_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);
            int intRecordID = Convert.ToInt32(e.CommandArgument);

            try
            {
                this.lblTaxDetailsTitle.Text = "Tax Details";

                //Tax
                using (MySqlDataReader drTAX = taxBL.Tax_View(intRecordID, string.Empty))
                {
                    if (drTAX.Read())
                    {
                        this.txtTaxName.Value = drTAX["TaxName"].ToString().Trim();
                        this.txtTaxAmount.Value = drTAX["TaxAmount"].ToString().Trim();
                        int intRecordStatus = Convert.ToInt32(drTAX["RecordStatus"]);

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

                    this.txtTaxName.Focus();
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
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Tax - View Details", ex.Message, strUserID);

                //Page Title
                this.divPageTitle.Visible = false;

                //Button Controls
                this.divButtonControls.Visible = false;

                //Tax
                this.divTax.Visible = false;
            }
        }

        //Button Controls
        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];

            try
            {
                //Tax
                using (MySqlDataReader drTAX = taxBL.Tax_View(0, this.txtSearch.Text.Trim()))
                {
                    if (drTAX.HasRows)
                    {
                        //Gridview
                        this.gvTax.DataSource = drTAX;
                        this.gvTax.DataBind();
                        this.gvTax.Visible = true;

                        //Count
                        this.lblTax.InnerText = "Showing " + string.Format("{0:n0}", this.gvTax.Rows.Count) + " records";

                        //Button Controls
                        this.btnExport.Visible = true;
                        this.btnCreate.Visible = true;
                    }

                    else
                    {
                        //Gridview
                        this.gvTax.Visible = false;

                        //Count
                        this.lblTax.InnerText = "No records to display";

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
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Tax - Search", ex.Message, strUserID);

                //Page Title
                this.divPageTitle.Visible = false;

                //Button Controls
                this.divButtonControls.Visible = false;

                //Tax
                this.divTax.Visible = false;
            }
        }

        protected void btnCreate_Click(object sender, System.EventArgs e)
        {
            this.lblTaxDetailsTitle.Text = "New Tax";

            this.txtTaxName.Value = String.Empty;
            this.txtTaxName.Focus();
            this.txtTaxAmount.Value = string.Empty;
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
            
            if (this.txtTaxName.Value == string.Empty)
            {
                this.lblValidationMessage.Text = "Tax name must not be empty.";
                this.lblValidationMessage.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else if (this.txtTaxAmount.Value == string.Empty)
            {
                this.lblValidationMessage.Text = "Tax amount must not be empty.";
                this.lblValidationMessage.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else
            {
                try
                {
                    //Check if Tax already exist
                    System.Data.DataSet dsTAX = new System.Data.DataSet();
                    dsTAX = DBHelper.GetData("SELECT RecordID, TaxName, TaxAmount FROM Tax WHERE TaxName = '" + this.txtTaxName.Value.Trim() + "'");

                    if (dsTAX.Tables[0].Rows.Count > 0)
                    {
                        this.lblValidationMessage.Text = "Tax already exist.";
                        this.lblValidationMessage.Attributes["style"] = "color:Red;";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
                    }

                    else
                    {
                        taxBO.taxname = this.txtTaxName.Value.Trim();
                        taxBO.taxamount = Convert.ToDecimal(this.txtTaxAmount.Value);

                        int intRecordStatus = 0;

                        if (this.rbStatusActive.Checked)
                        {
                            intRecordStatus = 1;
                        }

                        else
                        {
                            intRecordStatus = 0;
                        }

                        taxBO.recordstatus = intRecordStatus;
                        
                        //Save
                        string strSave = taxBL.Tax_Post(taxBO, strUserID);

                        //System Logs
                        strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Tax - Save", "Created new tax " + this.txtTaxName.Value.Trim(), strUserID);

                        //Redirect
                        ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Record saved', '" + this.txtTaxName.Value + "' +  ' has been created', 'success') .then((value) => { window.location.href = 'Tax.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole + "'; }); ", true);
                    }

                    dsTAX.Clear();
                }

                catch (Exception ex)
                {
                    //Error Message
                    this.divErrorMessage.Visible = true;
                    this.lblErrorMessage.InnerText = ex.Message;
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Tax - Save", ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //Tax
                    this.divTax.Visible = false;
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
                System.Data.DataSet dsTAX = new System.Data.DataSet();
                dsTAX = DBHelper.GetData("SELECT RecordID, TaxName, TaxAmount, RecordStatus FROM Tax WHERE TaxName = '" + this.txtTaxName.Value.Trim() + "' ");

                string strTaxName = dsTAX.Tables[0].Rows[0]["TaxName"].ToString().Trim();
                decimal dteTaxAmount  = Convert.ToDecimal(dsTAX.Tables[0].Rows[0]["TaxAmount"]);
                int intRecordStatuss = Convert.ToInt32(dsTAX.Tables[0].Rows[0]["RecordStatus"]);

                if (this.txtTaxName.Value == string.Empty)
                {
                    this.lblValidationMessage.Text = "Tax name must not be empty.";
                    this.lblValidationMessage.Attributes["style"] = "color:Red;";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
                }

                else if (this.txtTaxAmount.Value == string.Empty)
                {
                    this.lblValidationMessage.Text = "Tax amount must not be empty.";
                    this.lblValidationMessage.Attributes["style"] = "color:Red;";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
                }

                else
                {
                    taxBO.taxname = this.txtTaxName.Value.Trim();
                    taxBO.taxamount = Convert.ToDecimal(this.txtTaxAmount);

                    int intRecordStatus = 0;

                    if (this.rbStatusActive.Checked)
                    {
                        intRecordStatus = 1;
                    }

                    else
                    {
                        intRecordStatus = 0;
                    }

                    taxBO.recordstatus = intRecordStatus;

                    //Update
                    string strUpdate = taxBL.Tax_Post(taxBO, strUserID);

                    //System Logs
                    strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Tax - Update", "Updated tax " + this.txtTaxName.Value.Trim(), strUserID);

                    //Redirect
                    ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Record updated', '" + this.txtTaxName.Value + "' +  ' has been updated', 'success') .then((value) => { window.location.href = 'Tax.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole + "'; }); ", true);
                }
            }

            catch (Exception ex)
            {
                //Error Message
                this.divErrorMessage.Visible = true;
                this.lblErrorMessage.InnerText = ex.Message;
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Tax - Update", ex.Message, strUserID);

                //Page Title
                this.divPageTitle.Visible = false;

                //Button Controls
                this.divButtonControls.Visible = false;

                //Tax
                this.divTax.Visible = false;
            }
        }

        //Error Message
        protected void btnReload_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            Response.Redirect("Tax.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        
    }
}