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
    public partial class RegisterShift : System.Web.UI.Page
    {
        baseclass baseclass = new baseclass();
        SystemLogsBL slBL = new SystemLogsBL();
        ErrorLogsBL elBL = new ErrorLogsBL();
        SequenceBL seqBL = new SequenceBL();
        SystemParametersBL sysparamBL = new SystemParametersBL();
        RegisterShiftBO regshiftBO = new RegisterShiftBO();
        RegisterShiftBL regshiftBL = new RegisterShiftBL();
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
                    System.Data.DataSet dsREGISTER = new System.Data.DataSet();
                    dsREGISTER = DBHelper.GetData("SELECT TOP 1 ControlNumber, TransactionDate, TransactionType, OpeningAmount, ClosingAmount FROM RegisterShift WHERE TransactionType = '1'");

                    if (dsREGISTER.Tables[0].Rows.Count == 0)
                    {
                        this.btnCreate.Text = "Open Register";
                    }

                    else
                    {
                        this.btnCreate.Text = "Close Register";
                    }

                    dsREGISTER.Clear();

                    //Register Shift
                    using (MySqlDataReader drREGISTERSHIFT = regshiftBL.RegisterShift_View("0", string.Empty))
                    {
                        if (drREGISTERSHIFT.HasRows)
                        {
                            //Gridview
                            this.gvRegisterShift.DataSource = drREGISTERSHIFT;
                            this.gvRegisterShift.DataBind();
                            this.gvRegisterShift.Visible = true;

                            //Count
                            this.lblRegisterShift.InnerText = "Showing " + string.Format("{0:n0}", this.gvRegisterShift.Rows.Count) + " records";

                            //Button Controls
                            this.btnCreate.Visible = true;
                        }

                        else
                        {
                            //Gridview
                            this.gvRegisterShift.Visible = false;

                            //Count
                            this.lblRegisterShift.InnerText = "No records found";

                            //Button Controls
                            this.btnCreate.Visible = true;
                        }
                    }
                }

                catch (Exception ex)
                {
                    //Error Message
                    this.divErrorMessage.Visible = true;
                    this.lblErrorMessage.InnerText = ex.Message;
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Register Shift - Page Load", ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //Register Shift
                    this.divRegisterShift.Visible = false;
                }
            }
        }

        //Griview
        protected void gvRegisterShift_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Transaction Status
                Label lblTransactionStatus = (Label)e.Row.FindControl("lblTransactionStatus");

                if (lblTransactionStatus.Text == "Open")
                {
                    lblTransactionStatus.Attributes["style"] = "color:Green;";
                }

                else //Closed
                {
                    lblTransactionStatus.Attributes["style"] = "color:Blue;";
                }
            }
        }

        //Button Controls
        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];

            try
            {
                //Register Shift
                using (MySqlDataReader drREGISTERSHIFT = regshiftBL.RegisterShift_View("0", this.txtSearch.Text.Trim()))
                {
                    if (drREGISTERSHIFT.HasRows)
                    {
                        //Gridview
                        this.gvRegisterShift.DataSource = drREGISTERSHIFT;
                        this.gvRegisterShift.DataBind();
                        this.gvRegisterShift.Visible = true;

                        //Count
                        this.lblRegisterShift.InnerText = "Showing " + string.Format("{0:n0}", this.gvRegisterShift.Rows.Count) + " records";

                        //Button Controls
                        this.btnCreate.Visible = true;
                    }

                    else
                    {
                        //Gridview
                        this.gvRegisterShift.Visible = false;

                        //Count
                        this.lblRegisterShift.InnerText = "No records found";

                        //Button Controls
                        this.btnCreate.Visible = true;
                    }
                }
            }

            catch (Exception ex)
            {
                //Error Message
                this.divErrorMessage.Visible = true;
                this.lblErrorMessage.InnerText = ex.Message;
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Item Category - Search", ex.Message, strUserID);

                //Page Title
                this.divPageTitle.Visible = false;

                //Button Controls
                this.divButtonControls.Visible = false;

                //Register Shift
                this.divRegisterShift.Visible = false;
            }
        }

        protected void btnCreate_Click(object sender, System.EventArgs e)
        {
            //Button Controls
            System.Data.DataSet dsREGISTERSHIFT = new System.Data.DataSet();
            dsREGISTERSHIFT = DBHelper.GetData("SELECT TOP 1 ControlNumber, TransactionDate, OpeningAmount, ClosingAmount, TransactionStatus FROM RegisterShift WHERE TransactionStatus = '1'");

            this.txtTransactionDate.Value = System.DateTime.Now.ToString("yyyy-MM-dd");
            this.txtTransactionDate.Disabled = true;

            if (dsREGISTERSHIFT.Tables[0].Rows.Count == 0)
            {
                this.lblRegisterShiftTitle.Text = "Open Register";

                this.txtOpeningAmount.Enabled = true;
                this.divClosing.Visible = false;
                this.txtOpeningAmount.Text = "0.00";
                this.txtOpeningAmount.Focus();

                //Button Controls
                this.btnCancel.Visible = true;
                this.btnPost.Visible = true;
                this.btnPost.Text = "Open Register";
                this.lblValidationMessage.InnerText = string.Empty;

                //Show Modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);

            }

            else if (dsREGISTERSHIFT.Tables[0].Rows[0]["TransactionStatus"].ToString() == "1" && dsREGISTERSHIFT.Tables[0].Rows[0]["ClosingAmount"].ToString() == "0.00")
            {
                //Check if there are unposted transactions before closing register
                System.Data.DataSet dsREGISTER = new System.Data.DataSet();
                dsREGISTER = DBHelper.GetData("SELECT ControlNumber, TransactionDate, OrderStatus FROM OrderHeader WHERE TransactionDate = '" + Convert.ToDateTime(dsREGISTERSHIFT.Tables[0].Rows[0]["TransactionDate"]) + "' AND OrderStatus = '1'");

                if (dsREGISTER.Tables[0].Rows.Count > 0)
                {
                    //Alert
                    ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Register Cannot Close', 'Register shift for ' + '" + Convert.ToDateTime(dsREGISTER.Tables[0].Rows[0]["TransactionDate"]).ToString("MMMM dd, yyyy") + "' + ' cannot be close since there are still unposted items detected in your POS.', 'warning'); ", true);
                }

                else
                {
                    this.lblRegisterShiftTitle.Text = "Close Register";

                    this.hfControlNumber.Value = dsREGISTERSHIFT.Tables[0].Rows[0]["ControlNumber"].ToString();

                    this.txtOpeningAmount.Enabled = false;
                    this.txtOpeningAmount.Text = Convert.ToDecimal(dsREGISTERSHIFT.Tables[0].Rows[0]["OpeningAmount"]).ToString("N2");
                    this.divClosing.Visible = true;
                    this.txtClosingAmount.Text = "0.00";
                    this.txtClosingAmount.Focus();

                    //Button Controls
                    this.btnCancel.Visible = true;
                    this.btnPost.Visible = true;
                    this.btnPost.Text = "Close Register";

                    this.lblValidationMessage.InnerText = string.Empty;

                    //Show Modal
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
                }

                dsREGISTER.Clear();
            }

            else
            {
                //Alert
                ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Register Shift Closed', 'Register shift for ' + '" + this.txtTransactionDate.Value +  "' + ' is already closed.', 'info'); ", true);

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

            dsREGISTERSHIFT.Clear();

            
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

        //Register Shift
        protected void btnPost_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            if (this.btnPost.Text == "Open Register" && this.txtOpeningAmount.Text == String.Empty)
            {
                this.lblValidationMessage.InnerText = "Opening amount must not be empty.";
                this.lblValidationMessage.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else if (this.btnPost.Text == "Close Register" && this.txtClosingAmount.Text == string.Empty)
            {
                this.lblValidationMessage.InnerText = "Closing amount must not be empty.";
                this.lblValidationMessage.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else if (this.txtRemarks.InnerText == String.Empty)
            {
                this.lblValidationMessage.InnerText = "Remarks must not be empty.";
                this.lblValidationMessage.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else if (this.txtRemarks.InnerText.Length > 200)
            {
                this.lblValidationMessage.InnerText = "Remarks must not be greater than 200 characters.";
                this.lblValidationMessage.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else
            {
                try
                {
                    if (this.btnPost.Text == "Open Register") //Open Register
                    {
                        regshiftBO.controlnumber = seqBL.ControlNumber_Generate(7);
                        regshiftBO.transactiontype = 1;
                        regshiftBO.transactionstatus = 1;
                        regshiftBO.openingamount = Convert.ToDecimal(this.txtOpeningAmount.Text);
                        regshiftBO.closingamount = 0;
                    }

                    else //Close Register
                    {
                        regshiftBO.controlnumber = this.hfControlNumber.Value;
                        regshiftBO.transactiontype = 2;
                        regshiftBO.transactionstatus = 2;
                        regshiftBO.openingamount = Convert.ToDecimal(this.txtOpeningAmount.Text);
                        regshiftBO.closingamount = Convert.ToDecimal(this.txtClosingAmount.Text);
                    }

                    regshiftBO.remarks = this.txtRemarks.Value;

                    //Post
                    string strPost = regshiftBL.RegisterShift_Post(regshiftBO, strUserID);

                    //System Logs
                    strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Register Shift - " + this.btnPost.Text, "Register shift posted with " + this.btnPost.Text, strUserID);

                    //Redirect
                    ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Register Shift Posted', '" + this.btnPost.Text + "' +  ' has been posted', 'success') .then((value) => { window.location.href = 'RegisterShift.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole + "'; }); ", true);

                }

                catch (Exception ex)
                {
                    //Error Message
                    this.divErrorMessage.Visible = true;
                    this.lblErrorMessage.InnerText = ex.Message;
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Register Shift - " + this.btnPost.Text, ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //Register Shift
                    this.divRegisterShift.Visible = false;
                }
            }

        }

        //Error Message
        protected void btnReload_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            Response.Redirect("RegisterShift.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

    }
}