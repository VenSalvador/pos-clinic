using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using OfficeOpenXml;

using BusinessLogic;
using DataAccess;
using OfficeOpenXml.Style;

namespace POSActiv8
{
    public partial class SalesInvoice : System.Web.UI.Page
    {
        SystemLogsBL slBL = new SystemLogsBL();
        ErrorLogsBL elBL = new ErrorLogsBL();
        PointofSaleBL posBL = new PointofSaleBL();
        string strUserID;
        int intUserLevel;
        int intUserRole;
        string strErrorLogs = String.Empty;
        string strSystemLogs = String.Empty;
        DateTime dteTransactionDateFrom;
        DateTime dteTransactionDateTo;

        //Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                strUserID = Request.QueryString["userid"];
                intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
                intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);
                //baseclass.UserInformation(strUserID, intUserLevel, intUserRole);
                //baseclass.UserAccess(strUserID, intUserLevel, intUserRole, 3);

                //Error Message
                this.divErrorMessage.Visible = false;

                //Page Title
                this.divPageTitle.Visible = true;

                //Button Controls
                this.divButtonControls.Visible = true;

                try
                {
                    //Transaction Dates
                    this.txtTransactionDateFrom.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
                    this.txtTransactionDateTo.Text = System.DateTime.Now.ToString("yyyy-MM-dd");

                    //Sales Invoice
                    using (SqlDataReader drSALESINVOICE = posBL.POS_SalesInvoice_View(1, string.Empty, Convert.ToDateTime(this.txtTransactionDateFrom.Text), Convert.ToDateTime(this.txtTransactionDateTo.Text)))
                    {
                        this.lblSalesInvoice.Visible = true;

                        if (drSALESINVOICE.HasRows)
                        {
                            //Gridview
                            this.gvSalesInvoice.DataSource = drSALESINVOICE;
                            this.gvSalesInvoice.DataBind();
                            this.gvSalesInvoice.Visible = true;

                            //Count
                            this.lblSalesInvoice.InnerText = "Showing " + string.Format("{0:n0}", this.gvSalesInvoice.Rows.Count) + " records";

                            //Button Controls
                            this.btnExport.Visible = true;
                        }

                        else
                        {
                            //Gridview
                            this.gvSalesInvoice.Visible = false;

                            //Count
                            this.lblSalesInvoice.InnerText = "No records to display";

                            //Button Controls
                            this.btnExport.Visible = false;
                        }
                    }
                }

                catch (Exception ex)
                {
                    //Error Message
                    this.divErrorMessage.Visible = true;
                    this.lblErrorMessage.InnerText = ex.Message;
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Sales Invoice - Page Load", ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //Sales Invoice
                    this.divSalesInvoice.Visible = false;
                }
            }
        }

        //Gridview
        protected void gvSalesInvoice_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);
            this.hfControlNumber.Value = e.CommandArgument.ToString().Trim();

            try
            {
                //Transaction Date
                //dteTransactionDate = Convert.ToDateTime(this.txtTransactionDate.Text);
                //this.txtTransactionDate.Text = dteTransactionDate.ToString("yyyy-MM-dd");

                this.lblSalesInvoiceDetailsTitle.InnerText = "Sales Invoice - " + this.hfControlNumber.Value;

                //Sales Invoice
                using (SqlDataReader drSALESINVOICE = posBL.POS_SalesInvoice_View(2, this.hfControlNumber.Value, Convert.ToDateTime(this.txtTransactionDateFrom.Text), Convert.ToDateTime(this.txtTransactionDateTo.Text)))
                {
                    if (drSALESINVOICE.HasRows)
                    {
                        //Gridview
                        this.gvSalesInvoiceDetails.DataSource = drSALESINVOICE;
                        this.gvSalesInvoiceDetails.DataBind();
                        this.gvSalesInvoiceDetails.Visible = true;
                    }

                    else
                    {
                        //Gridview
                        this.gvSalesInvoiceDetails.Visible = false;
                    }
                }

                //Sales Invoice
                using (SqlDataReader drSALESINVOICE = posBL.POS_SalesInvoice_View(2, this.hfControlNumber.Value, Convert.ToDateTime(this.txtTransactionDateFrom.Text), Convert.ToDateTime(this.txtTransactionDateTo.Text)))
                {
                    if (drSALESINVOICE.Read())
                    {
                        this.lblGrandTotal.InnerText = "Grand Total: " + drSALESINVOICE["GrandTotal"].ToString();

                        this.lblSalesInvoiceDetailsTitle.InnerText = "Sales Invoice - " + drSALESINVOICE["TableName"].ToString();
                    }
                }

                //Show Modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            catch (Exception ex)
            {
                //Error Message
                this.divErrorMessage.Visible = true;
                this.lblErrorMessage.InnerText = ex.Message;
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Sales Invoice - Details", ex.Message, strUserID);

                //Page Title
                this.divPageTitle.Visible = false;

                //Button Controls
                this.divButtonControls.Visible = false;

                //Sales Invoice
                this.divSalesInvoice.Visible = false;
            }
        }

        //Button Controls
        protected void btnExport_Click(object sender, System.EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            if (this.txtTransactionDateFrom.Text == string.Empty)
            {
                //Alert
                ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Required Field', 'Transaction date from must not be empty.', 'warning'); ", true);
            }

            else if (this.txtTransactionDateTo.Text == string.Empty)
            {
                //Alert
                ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Required Field', 'Transaction date to must not be empty.', 'warning'); ", true);
            }

            else
            {
                var strFileName = string.Empty;

                try
                {
                    //Reports Generated
                    string strReportsGeneratedPath = HttpContext.Current.Server.MapPath(string.Format("~/ReportsGenerated/"));

                    //Reference Date
                    //dteTransactionDate = Convert.ToDateTime(this.txtTransactionDate.Text);

                    //Source File
                    strFileName = "Activ8_SalesInvoice_" + Convert.ToDateTime(this.txtTransactionDateFrom.Text).ToString("yyyyMMdd") + "_" + Convert.ToDateTime(this.txtTransactionDateTo.Text).ToString("yyyyMMdd") + ".xlsx";
                    var strSaveSalesInvoice = new FileInfo(Path.Combine(strReportsGeneratedPath, strFileName));

                    //Check if file exist then delete
                    if (System.IO.File.Exists(strSaveSalesInvoice.ToString()))
                    {
                        System.IO.File.Delete(strSaveSalesInvoice.ToString());
                    }

                    //Create Package
                    using (var package = new ExcelPackage(strSaveSalesInvoice))
                    {
                        //Sales Invoice Header
                        using (SqlDataReader drEXPORTSALESINVOICE = posBL.POS_SalesInvoice_View(1, string.Empty, Convert.ToDateTime(this.txtTransactionDateFrom.Text), Convert.ToDateTime(this.txtTransactionDateTo.Text)))
                        {
                            //Worksheet
                            ExcelWorksheet worksheetSALESINVOICE = package.Workbook.Worksheets.Add("Header");
                            worksheetSALESINVOICE.Protection.IsProtected = true;
                            worksheetSALESINVOICE.Cells.Style.Font.Size = 10;

                            //Title
                            worksheetSALESINVOICE.Cells[1, 1].Value = "Activ8 Sports Bar";
                            worksheetSALESINVOICE.Cells[1, 1].Style.Font.Bold = true;
                            worksheetSALESINVOICE.Cells[1, 1, 1, 4].Merge = true;
                            worksheetSALESINVOICE.Cells[2, 1].Value = "Sales Invoice Report";
                            worksheetSALESINVOICE.Cells[2, 1].Style.Font.Bold = true;
                            worksheetSALESINVOICE.Cells[2, 1, 2, 4].Merge = true;
                            worksheetSALESINVOICE.Cells[3, 1].Value = "Transaction Date :" + " " + System.DateTime.Now.ToString("MMMM dd, yyyy");
                            worksheetSALESINVOICE.Cells[3, 1].Style.Font.Bold = true;
                            worksheetSALESINVOICE.Cells[3, 1, 3, 4].Merge = true;

                            //Header
                            worksheetSALESINVOICE.Cells[5, 1].Value = "Control Number";
                            worksheetSALESINVOICE.Cells[5, 2].Value = "Sub Total";
                            worksheetSALESINVOICE.Cells[5, 3].Value = "Discount Name";
                            worksheetSALESINVOICE.Cells[5, 4].Value = "Discount Amount";
                            worksheetSALESINVOICE.Cells[5, 5].Value = "Discounted Price";
                            worksheetSALESINVOICE.Cells[5, 6].Value = "Discount Remarks";
                            worksheetSALESINVOICE.Cells[5, 7].Value = "Grand Total";
                            worksheetSALESINVOICE.Cells[5, 8].Value = "Payment Type";
                            worksheetSALESINVOICE.Cells[5, 9].Value = "Payment Amount";
                            worksheetSALESINVOICE.Cells[5, 10].Value = "Change Due";
                            worksheetSALESINVOICE.Cells[5, 11].Value = "Card Type";
                            worksheetSALESINVOICE.Cells[5, 12].Value = "Card Number";
                            worksheetSALESINVOICE.Cells[5, 13].Value = "Account Name";
                            worksheetSALESINVOICE.Cells[5, 14].Value = "Expiry Date";
                            worksheetSALESINVOICE.Cells[5, 15].Value = "GCash Reference Number";
                            worksheetSALESINVOICE.Cells[5, 16].Value = "Order Status";
                            worksheetSALESINVOICE.Cells[5, 17].Value = "Date & Time Posted";
                            worksheetSALESINVOICE.Cells[5, 18].Value = "Posted By";
                            worksheetSALESINVOICE.Cells[5, 1, 5, 18].Style.Font.Bold = true;

                            //Freeze upto row 6 and column 4
                            //worksheetSALESINVOICE.View.FreezePanes(6, 4);

                            int intRowCounter = 6;

                            while (drEXPORTSALESINVOICE.Read())
                            {
                                //Generate Rows
                                worksheetSALESINVOICE.Cells[intRowCounter, 1].Value = drEXPORTSALESINVOICE["ControlNumber"].ToString().Trim();
                                worksheetSALESINVOICE.Cells[intRowCounter, 2].Value = Convert.ToDecimal(drEXPORTSALESINVOICE["SubTotal"]);
                                worksheetSALESINVOICE.Cells[intRowCounter, 3].Value = drEXPORTSALESINVOICE["DiscountName"].ToString().Trim();
                                worksheetSALESINVOICE.Cells[intRowCounter, 4].Value = Convert.ToDecimal(drEXPORTSALESINVOICE["DiscountAmount"]);
                                worksheetSALESINVOICE.Cells[intRowCounter, 5].Value = Convert.ToDecimal(drEXPORTSALESINVOICE["DiscountedPrice"]);
                                worksheetSALESINVOICE.Cells[intRowCounter, 6].Value = drEXPORTSALESINVOICE["DiscountRemarks"].ToString().Trim();
                                worksheetSALESINVOICE.Cells[intRowCounter, 7].Value = Convert.ToDecimal(drEXPORTSALESINVOICE["GrandTotal"]);
                                worksheetSALESINVOICE.Cells[intRowCounter, 8].Value = drEXPORTSALESINVOICE["PaymentType"].ToString().Trim();
                                worksheetSALESINVOICE.Cells[intRowCounter, 9].Value = Convert.ToDecimal(drEXPORTSALESINVOICE["PaymentAmount"]);
                                worksheetSALESINVOICE.Cells[intRowCounter, 10].Value = Convert.ToDecimal(drEXPORTSALESINVOICE["ChangeDue"]);
                                worksheetSALESINVOICE.Cells[intRowCounter, 11].Value = drEXPORTSALESINVOICE["CardType"].ToString().Trim();
                                worksheetSALESINVOICE.Cells[intRowCounter, 12].Value = drEXPORTSALESINVOICE["CardNumber"].ToString().Trim();
                                worksheetSALESINVOICE.Cells[intRowCounter, 13].Value = drEXPORTSALESINVOICE["AccountName"].ToString().Trim();
                                worksheetSALESINVOICE.Cells[intRowCounter, 14].Value = drEXPORTSALESINVOICE["ExpiryDate"].ToString().Trim();
                                worksheetSALESINVOICE.Cells[intRowCounter, 15].Value = drEXPORTSALESINVOICE["GCashReferenceNumber"].ToString().Trim();
                                worksheetSALESINVOICE.Cells[intRowCounter, 16].Value = drEXPORTSALESINVOICE["OrderStatus"].ToString().Trim();
                                worksheetSALESINVOICE.Cells[intRowCounter, 17].Value = drEXPORTSALESINVOICE["DateTimePosted"].ToString().Trim();
                                worksheetSALESINVOICE.Cells[intRowCounter, 18].Value = drEXPORTSALESINVOICE["PostedBy"].ToString().Trim();

                                intRowCounter++;
                            }

                            if (intRowCounter == 6)
                            {
                                worksheetSALESINVOICE.Cells[intRowCounter, 1].Value = "No records to display";
                            }

                            //Column Format
                            worksheetSALESINVOICE.Cells[worksheetSALESINVOICE.Dimension.Address].AutoFitColumns();
                            worksheetSALESINVOICE.Column(2).Style.Numberformat.Format = "#,##0.00";
                            worksheetSALESINVOICE.Column(4).Style.Numberformat.Format = "#,##0.00";
                            worksheetSALESINVOICE.Column(5).Style.Numberformat.Format = "#,##0.00";
                            worksheetSALESINVOICE.Column(7).Style.Numberformat.Format = "#,##0.00";
                            worksheetSALESINVOICE.Column(9).Style.Numberformat.Format = "#,##0.00";
                            worksheetSALESINVOICE.Column(10).Style.Numberformat.Format = "#,##0.00";

                            worksheetSALESINVOICE.Column(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            worksheetSALESINVOICE.Column(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            worksheetSALESINVOICE.Column(5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            worksheetSALESINVOICE.Column(7).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            worksheetSALESINVOICE.Column(9).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            worksheetSALESINVOICE.Column(10).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            //Font Size
                            worksheetSALESINVOICE.Cells[worksheetSALESINVOICE.Dimension.Address].Style.Font.Size = 10;
                        }

                        //Sales Invoice Lines
                        using (SqlDataReader drEXPORTSALESINVOICELINES = posBL.POS_SalesInvoice_View(3, string.Empty, Convert.ToDateTime(this.txtTransactionDateFrom.Text), Convert.ToDateTime(this.txtTransactionDateTo.Text)))
                        {
                            //Worksheet
                            ExcelWorksheet worksheetSALESINVOICELINES = package.Workbook.Worksheets.Add("Lines");
                            worksheetSALESINVOICELINES.Protection.IsProtected = true;
                            worksheetSALESINVOICELINES.Cells.Style.Font.Size = 10;

                            //Title
                            worksheetSALESINVOICELINES.Cells[1, 1].Value = "Activ8";
                            worksheetSALESINVOICELINES.Cells[1, 1].Style.Font.Bold = true;
                            worksheetSALESINVOICELINES.Cells[1, 1, 1, 4].Merge = true;
                            worksheetSALESINVOICELINES.Cells[2, 1].Value = "Sales Invoice Report";
                            worksheetSALESINVOICELINES.Cells[2, 1].Style.Font.Bold = true;
                            worksheetSALESINVOICELINES.Cells[2, 1, 2, 4].Merge = true;
                            worksheetSALESINVOICELINES.Cells[3, 1].Value = "Transaction Date :" + " " + System.DateTime.Now.ToString("MMMM dd, yyyy");
                            worksheetSALESINVOICELINES.Cells[3, 1].Style.Font.Bold = true;
                            worksheetSALESINVOICELINES.Cells[3, 1, 3, 4].Merge = true;

                            //Header
                            worksheetSALESINVOICELINES.Cells[5, 1].Value = "Control Number";
                            worksheetSALESINVOICELINES.Cells[5, 2].Value = "Transaction Code";
                            worksheetSALESINVOICELINES.Cells[5, 3].Value = "Table Name";
                            worksheetSALESINVOICELINES.Cells[5, 4].Value = "Floor Name";
                            worksheetSALESINVOICELINES.Cells[5, 5].Value = "Category Name";
                            worksheetSALESINVOICELINES.Cells[5, 6].Value = "Item Name";
                            worksheetSALESINVOICELINES.Cells[5, 7].Value = "Item Price";
                            worksheetSALESINVOICELINES.Cells[5, 8].Value = "Item Quantity";
                            worksheetSALESINVOICELINES.Cells[5, 9].Value = "Discount Type";
                            worksheetSALESINVOICELINES.Cells[5, 10].Value = "Discount Amount";
                            worksheetSALESINVOICELINES.Cells[5, 11].Value = "Discounted Price";
                            worksheetSALESINVOICELINES.Cells[5, 12].Value = "Discount Remarks";
                            worksheetSALESINVOICELINES.Cells[5, 13].Value = "Item Amount";
                            worksheetSALESINVOICELINES.Cells[5, 14].Value = "Payment Type";
                            worksheetSALESINVOICELINES.Cells[5, 15].Value = "Order Status";
                            worksheetSALESINVOICELINES.Cells[5, 16].Value = "Date & Time Posted";
                            worksheetSALESINVOICELINES.Cells[5, 17].Value = "Posted By";
                            worksheetSALESINVOICELINES.Cells[5, 1, 5, 17].Style.Font.Bold = true;

                            //Freeze upto row 6 and column 4
                            //worksheetSALESINVOICE.View.FreezePanes(6, 4);

                            int intRowCounter = 6;

                            while (drEXPORTSALESINVOICELINES.Read())
                            {
                                //Generate Rows
                                worksheetSALESINVOICELINES.Cells[intRowCounter, 1].Value = drEXPORTSALESINVOICELINES["ControlNumber"].ToString().Trim();
                                worksheetSALESINVOICELINES.Cells[intRowCounter, 2].Value = drEXPORTSALESINVOICELINES["TransactionCode"].ToString().Trim();
                                worksheetSALESINVOICELINES.Cells[intRowCounter, 3].Value = drEXPORTSALESINVOICELINES["TableName"].ToString().Trim();
                                worksheetSALESINVOICELINES.Cells[intRowCounter, 4].Value = drEXPORTSALESINVOICELINES["FloorName"].ToString().Trim();
                                worksheetSALESINVOICELINES.Cells[intRowCounter, 5].Value = drEXPORTSALESINVOICELINES["CategoryName"].ToString().Trim();
                                worksheetSALESINVOICELINES.Cells[intRowCounter, 6].Value = drEXPORTSALESINVOICELINES["ItemName"].ToString().Trim();
                                worksheetSALESINVOICELINES.Cells[intRowCounter, 7].Value = Convert.ToDecimal(drEXPORTSALESINVOICELINES["ItemPrice"]);
                                worksheetSALESINVOICELINES.Cells[intRowCounter, 8].Value = drEXPORTSALESINVOICELINES["ItemQuantity"].ToString().Trim();
                                worksheetSALESINVOICELINES.Cells[intRowCounter, 9].Value = drEXPORTSALESINVOICELINES["DiscountName"].ToString().Trim();
                                worksheetSALESINVOICELINES.Cells[intRowCounter, 10].Value = Convert.ToDecimal(drEXPORTSALESINVOICELINES["DiscountAmount"]);
                                worksheetSALESINVOICELINES.Cells[intRowCounter, 11].Value = Convert.ToDecimal(drEXPORTSALESINVOICELINES["DiscountedPrice"]);
                                worksheetSALESINVOICELINES.Cells[intRowCounter, 12].Value = drEXPORTSALESINVOICELINES["DiscountRemarks"].ToString().Trim();
                                worksheetSALESINVOICELINES.Cells[intRowCounter, 13].Value = Convert.ToDecimal(drEXPORTSALESINVOICELINES["ItemAmount"]);
                                worksheetSALESINVOICELINES.Cells[intRowCounter, 14].Value = drEXPORTSALESINVOICELINES["PaymentType"].ToString().Trim();
                                worksheetSALESINVOICELINES.Cells[intRowCounter, 15].Value = drEXPORTSALESINVOICELINES["OrderStatus"].ToString().Trim();
                                worksheetSALESINVOICELINES.Cells[intRowCounter, 16].Value = drEXPORTSALESINVOICELINES["DateTimePosted"].ToString().Trim();
                                worksheetSALESINVOICELINES.Cells[intRowCounter, 17].Value = drEXPORTSALESINVOICELINES["PostedBy"].ToString().Trim();

                                intRowCounter++;
                            }

                            if (intRowCounter == 6)
                            {
                                worksheetSALESINVOICELINES.Cells[intRowCounter, 1].Value = "No records to display";
                            }

                            //Column Format
                            worksheetSALESINVOICELINES.Cells[worksheetSALESINVOICELINES.Dimension.Address].AutoFitColumns();
                            worksheetSALESINVOICELINES.Column(7).Style.Numberformat.Format = "#,##0.00";
                            worksheetSALESINVOICELINES.Column(10).Style.Numberformat.Format = "#,##0.00";
                            worksheetSALESINVOICELINES.Column(11).Style.Numberformat.Format = "#,##0.00";
                            worksheetSALESINVOICELINES.Column(13).Style.Numberformat.Format = "#,##0.00";

                            worksheetSALESINVOICELINES.Column(7).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            worksheetSALESINVOICELINES.Column(10).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            worksheetSALESINVOICELINES.Column(11).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            worksheetSALESINVOICELINES.Column(13).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            //Font Size
                            worksheetSALESINVOICELINES.Cells[worksheetSALESINVOICELINES.Dimension.Address].Style.Font.Size = 10;
                        }

                        //Save the file
                        package.Save();

                        //System Logs
                        strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Sales Invoice - Export", strFileName + " has been successfully generated.", strUserID);

                        //Download the report file
                        string filePath = strSaveSalesInvoice.ToString();
                        Response.ContentType = ContentType;
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
                        Response.WriteFile(filePath);
                        Response.End();
                    }
                }

                catch (Exception ex)
                {
                    //Error Message
                    this.divErrorMessage.Visible = true;
                    this.lblErrorMessage.InnerText = ex.Message;
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Sales Invoice - Export", strFileName + " : " + ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //Sales Invoice
                    this.divSalesInvoice.Visible = false;
                }
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            if (!DateTime.TryParse(this.txtTransactionDateFrom.Text, out dteTransactionDateFrom))
            {
                //Alert
                ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Invalid transaction date from format', '', 'error'); ", true);
            }

            else if (!DateTime.TryParse(this.txtTransactionDateTo.Text, out dteTransactionDateTo))
            {
                //Alert
                ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Invalid transaction date to format', '', 'error'); ", true);
            }

            else if (Convert.ToDateTime(this.txtTransactionDateFrom.Text) > Convert.ToDateTime(this.txtTransactionDateTo.Text))
            {
                //Alert
                ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Date from must not be greater than the date to', '', 'error'); ", true);
            }

            else
            {
                try
                {
                    //Reference Date
                    //dteTransactionDate = Convert.ToDateTime(this.txtTransactionDate.Text);
                    //this.txtTransactionDate.Text = dteTransactionDate.ToString("yyyy-MM-dd");

                    //Sales Invoice
                    using (SqlDataReader drSALESINVOICE = posBL.POS_SalesInvoice_View(1, string.Empty, Convert.ToDateTime(this.txtTransactionDateFrom.Text), Convert.ToDateTime(this.txtTransactionDateTo.Text)))
                    {
                        this.divSalesInvoice.Visible = true;
                        this.lblSalesInvoice.Visible = true;

                        if (drSALESINVOICE.HasRows)
                        {
                            //Gridview
                            this.gvSalesInvoice.DataSource = drSALESINVOICE;
                            this.gvSalesInvoice.DataBind();
                            this.gvSalesInvoice.Visible = true;

                            //Count
                            this.lblSalesInvoice.InnerText = "Showing " + string.Format("{0:n0}", this.gvSalesInvoice.Rows.Count) + " records";

                            //Button Controls
                            this.btnExport.Visible = true;
                        }

                        else
                        {
                            //Gridview
                            this.gvSalesInvoice.Visible = false;

                            //Count
                            this.lblSalesInvoice.InnerText = "No records to display";

                            //Button Controls
                            this.btnExport.Visible = false;
                        }
                    }
                }

                catch (Exception ex)
                {
                    //Error Message
                    this.divErrorMessage.Visible = true;
                    this.lblErrorMessage.InnerText = ex.Message;
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Sales Invoice - Transaction Date", ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //Sales Invoice
                    this.divSalesInvoice.Visible = false;
                }
            }
        }

        //Error Message
        protected void btnReload_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            Response.Redirect("SalesInvoice.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        
    }
}