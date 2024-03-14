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
using MySql.Data.MySqlClient;


namespace POSActiv8
{
    public partial class CancelledOrders : System.Web.UI.Page
    {
        SystemLogsBL slBL = new SystemLogsBL();
        ErrorLogsBL elBL = new ErrorLogsBL();
        PointofSaleBL posBL = new PointofSaleBL();
        string strUserID;
        int intUserLevel;
        int intUserRole;
        string strErrorLogs = String.Empty;
        string strSystemLogs = String.Empty;
        DateTime dteTransactionDate;

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
                    //Transaction Date
                    dteTransactionDate = System.DateTime.Now;
                    this.txtTransactionDate.Text = dteTransactionDate.ToString("yyyy-MM-dd");

                    //Cancelled Orders
                    using (MySqlDataReader drVOIDITEMS = posBL.POS_CancelledOrders_View(dteTransactionDate))
                    {
                        this.lblCancelledOrders.Visible = true;

                        if (drVOIDITEMS.HasRows)
                        {
                            //Gridview
                            this.gvCancelledOrders.DataSource = drVOIDITEMS;
                            this.gvCancelledOrders.DataBind();
                            this.gvCancelledOrders.Visible = true;

                            //Count
                            this.lblCancelledOrders.InnerText = "Showing " + string.Format("{0:n0}", this.gvCancelledOrders.Rows.Count) + " records";

                            //Button Controls
                            this.btnExport.Visible = true;
                        }

                        else
                        {
                            //Gridview
                            this.gvCancelledOrders.Visible = false;

                            //Count
                            this.lblCancelledOrders.InnerText = "No records to display";

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
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Cancelled Orders - Page Load", ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //Cancelled Orders
                    this.divCancelledOrders.Visible = false;
                }
            }
        }

        //Button Controls
        protected void txtTransactionDate_TextChanged(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            if (!DateTime.TryParse(this.txtTransactionDate.Text, out dteTransactionDate))
            {
                //Alert
                ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Invalid transaction date format', '', 'error'); ", true);
            }

            else
            {
                try
                {
                    //Reference Date
                    dteTransactionDate = Convert.ToDateTime(this.txtTransactionDate.Text);
                    this.txtTransactionDate.Text = dteTransactionDate.ToString("yyyy-MM-dd");

                    //Cancelled Orders
                    using (MySqlDataReader drVOIDITEMS = posBL.POS_CancelledOrders_View(Convert.ToDateTime(this.txtTransactionDate.Text)))
                    {
                        this.lblCancelledOrders.Visible = true;

                        if (drVOIDITEMS.HasRows)
                        {
                            //Gridview
                            this.gvCancelledOrders.DataSource = drVOIDITEMS;
                            this.gvCancelledOrders.DataBind();
                            this.gvCancelledOrders.Visible = true;

                            //Count
                            this.lblCancelledOrders.InnerText = "Showing " + string.Format("{0:n0}", this.gvCancelledOrders.Rows.Count) + " records";

                            //Button Controls
                            this.btnExport.Visible = true;
                        }

                        else
                        {
                            //Gridview
                            this.gvCancelledOrders.Visible = false;

                            //Count
                            this.lblCancelledOrders.InnerText = "No records to display";

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
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Cancelled Orders - Transaction Date", ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //Cancelled Orders
                    this.divCancelledOrders.Visible = false;
                }
            }
        }

        protected void btnExport_Click(object sender, System.EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            if (this.txtTransactionDate.Text == string.Empty)
            {
                //Alert
                ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Required Field', 'Transaction date must not be empty.', 'warning'); ", true);
            }

            else
            {
                var strFileName = string.Empty;

                try
                {
                    //Reports Generated
                    string strReportsGeneratedPath = HttpContext.Current.Server.MapPath(string.Format("~/ReportsGenerated/"));

                    //Reference Date
                    dteTransactionDate = Convert.ToDateTime(this.txtTransactionDate.Text);

                    //Source File
                    strFileName = "Activ8_CancelledOrders_" + dteTransactionDate.ToString("yyyyMMdd") + ".xlsx";
                    var strSaveCancelledOrders = new FileInfo(Path.Combine(strReportsGeneratedPath, strFileName));

                    //Check if file exist then delete
                    if (System.IO.File.Exists(strSaveCancelledOrders.ToString()))
                    {
                        System.IO.File.Delete(strSaveCancelledOrders.ToString());
                    }

                    //Create Package
                    using (var package = new ExcelPackage(strSaveCancelledOrders))
                    {
                        //Sales Invoice Header
                        using (MySqlDataReader drEXPORTCANCELLEDORDERS = posBL.POS_CancelledOrders_View(dteTransactionDate))
                        {
                            //Worksheet
                            ExcelWorksheet worksheetCANCELLEDORDERS = package.Workbook.Worksheets.Add("Cancelled Orders");
                            worksheetCANCELLEDORDERS.Protection.IsProtected = true;
                            worksheetCANCELLEDORDERS.Cells.Style.Font.Size = 10;

                            //Title
                            worksheetCANCELLEDORDERS.Cells[1, 1].Value = "Activ8 Sports Bar";
                            worksheetCANCELLEDORDERS.Cells[1, 1].Style.Font.Bold = true;
                            worksheetCANCELLEDORDERS.Cells[1, 1, 1, 4].Merge = true;
                            worksheetCANCELLEDORDERS.Cells[2, 1].Value = "Cancelled Orders Report";
                            worksheetCANCELLEDORDERS.Cells[2, 1].Style.Font.Bold = true;
                            worksheetCANCELLEDORDERS.Cells[2, 1, 2, 4].Merge = true;
                            worksheetCANCELLEDORDERS.Cells[3, 1].Value = "Transaction Date :" + " " + System.DateTime.Now.ToString("MMMM dd, yyyy");
                            worksheetCANCELLEDORDERS.Cells[3, 1].Style.Font.Bold = true;
                            worksheetCANCELLEDORDERS.Cells[3, 1, 3, 4].Merge = true;

                            //Header
                            worksheetCANCELLEDORDERS.Cells[5, 1].Value = "Date and Time Cancelled";
                            worksheetCANCELLEDORDERS.Cells[5, 2].Value = "Control Number";
                            worksheetCANCELLEDORDERS.Cells[5, 3].Value = "Table";
                            worksheetCANCELLEDORDERS.Cells[5, 4].Value = "Floor";
                            worksheetCANCELLEDORDERS.Cells[5, 5].Value = "Order Count";
                            worksheetCANCELLEDORDERS.Cells[5, 6].Value = "Grand Total";
                            worksheetCANCELLEDORDERS.Cells[5, 7].Value = "Cancellation Type";
                            worksheetCANCELLEDORDERS.Cells[5, 8].Value = "Authorized By";
                            worksheetCANCELLEDORDERS.Cells[5, 9].Value = "Cancelled By";
                            worksheetCANCELLEDORDERS.Cells[5, 1, 5, 11].Style.Font.Bold = true;

                            //Freeze upto row 6 and column 4
                            //worksheetSALESINVOICE.View.FreezePanes(6, 4);

                            int intRowCounter = 6;

                            while (drEXPORTCANCELLEDORDERS.Read())
                            {
                                //Generate Rows
                                worksheetCANCELLEDORDERS.Cells[intRowCounter, 1].Value = drEXPORTCANCELLEDORDERS["DateCancelled"].ToString().Trim();
                                worksheetCANCELLEDORDERS.Cells[intRowCounter, 2].Value = drEXPORTCANCELLEDORDERS["ControlNumber"].ToString().Trim();
                                worksheetCANCELLEDORDERS.Cells[intRowCounter, 3].Value = drEXPORTCANCELLEDORDERS["TableName"].ToString().Trim();
                                worksheetCANCELLEDORDERS.Cells[intRowCounter, 4].Value = drEXPORTCANCELLEDORDERS["FloorName"].ToString().Trim();
                                worksheetCANCELLEDORDERS.Cells[intRowCounter, 5].Value = drEXPORTCANCELLEDORDERS["OrderCount"].ToString().Trim();
                                worksheetCANCELLEDORDERS.Cells[intRowCounter, 6].Value = Convert.ToDecimal(drEXPORTCANCELLEDORDERS["GrandTotal"]);
                                worksheetCANCELLEDORDERS.Cells[intRowCounter, 7].Value = drEXPORTCANCELLEDORDERS["CancellationType"].ToString();
                                worksheetCANCELLEDORDERS.Cells[intRowCounter, 8].Value = drEXPORTCANCELLEDORDERS["CanceldAuthorizedBy"].ToString().Trim();
                                worksheetCANCELLEDORDERS.Cells[intRowCounter, 9].Value = drEXPORTCANCELLEDORDERS["CancelledBy"].ToString().Trim();

                                intRowCounter++;
                            }

                            if (intRowCounter == 6)
                            {
                                worksheetCANCELLEDORDERS.Cells[intRowCounter, 1].Value = "No records to display";
                            }

                            //Column Format
                            worksheetCANCELLEDORDERS.Cells[worksheetCANCELLEDORDERS.Dimension.Address].AutoFitColumns();
                            worksheetCANCELLEDORDERS.Column(6).Style.Numberformat.Format = "#,##0.00";

                            worksheetCANCELLEDORDERS.Column(6).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            //Font Size
                            worksheetCANCELLEDORDERS.Cells[worksheetCANCELLEDORDERS.Dimension.Address].Style.Font.Size = 10;
                        }

                        //Save the file
                        package.Save();

                        //System Logs
                        strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Cancelled Orders - Export", strFileName + " has been successfully generated.", strUserID);

                        //Download the report file
                        string filePath = strSaveCancelledOrders.ToString();
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
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Cancelled Orders - Export", strFileName + " : " + ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //Cancelled Orders
                    this.divCancelledOrders.Visible = false;
                }
            }
        }

        //Error Message
        protected void btnReload_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            Response.Redirect("CancelledOrders.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

    }
}