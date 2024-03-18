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
using MySql.Data.MySqlClient;

using BusinessLogic;
using DataAccess;
using OfficeOpenXml.Style;

namespace POSActiv8
{
    public partial class VoidItems : System.Web.UI.Page
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

                    //Void Items
                    using (MySqlDataReader drVOIDITEMS = posBL.POS_VoidItems_View(dteTransactionDate))
                    {
                        this.lblVoidItems.Visible = true;

                        if (drVOIDITEMS.HasRows)
                        {
                            //Gridview
                            this.gvVoidItems.DataSource = drVOIDITEMS;
                            this.gvVoidItems.DataBind();
                            this.gvVoidItems.Visible = true;

                            //Count
                            this.lblVoidItems.InnerText = "Showing " + string.Format("{0:n0}", this.gvVoidItems.Rows.Count) + " records";

                            //Button Controls
                            this.btnExport.Visible = true;
                        }

                        else
                        {
                            //Gridview
                            this.gvVoidItems.Visible = false;

                            //Count
                            this.lblVoidItems.InnerText = "No records to display";

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
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Void Items - Page Load", ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //Void Items
                    this.divVoidItems.Visible = false;
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

                    //Void Items
                    using (MySqlDataReader drVOIDITEMS = posBL.POS_VoidItems_View(Convert.ToDateTime(this.txtTransactionDate.Text)))
                    {
                        this.lblVoidItems.Visible = true;

                        if (drVOIDITEMS.HasRows)
                        {
                            //Gridview
                            this.gvVoidItems.DataSource = drVOIDITEMS;
                            this.gvVoidItems.DataBind();
                            this.gvVoidItems.Visible = true;

                            //Count
                            this.lblVoidItems.InnerText = "Showing " + string.Format("{0:n0}", this.gvVoidItems.Rows.Count) + " records";

                            //Button Controls
                            this.btnExport.Visible = true;
                        }

                        else
                        {
                            //Gridview
                            this.gvVoidItems.Visible = false;

                            //Count
                            this.lblVoidItems.InnerText = "No records to display";

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
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Void Items - Transaction Date", ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //Void Items
                    this.divVoidItems.Visible = false;
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
                    strFileName = "Activ8_VoidItems_" + dteTransactionDate.ToString("yyyyMMdd") + ".xlsx";
                    var strSaveVoidItems = new FileInfo(Path.Combine(strReportsGeneratedPath, strFileName));

                    //Check if file exist then delete
                    if (System.IO.File.Exists(strSaveVoidItems.ToString()))
                    {
                        System.IO.File.Delete(strSaveVoidItems.ToString());
                    }

                    //Create Package
                    using (var package = new ExcelPackage(strSaveVoidItems))
                    {
                        //Sales Invoice Header
                        using (MySqlDataReader drEXPORTVOIDITEMS = posBL.POS_VoidItems_View(dteTransactionDate))
                        {
                            //Worksheet
                            ExcelWorksheet worksheetVOIDITEMS = package.Workbook.Worksheets.Add("Void Items");
                            worksheetVOIDITEMS.Protection.IsProtected = true;
                            worksheetVOIDITEMS.Cells.Style.Font.Size = 10;

                            //Title
                            worksheetVOIDITEMS.Cells[1, 1].Value = "Activ8 Sports Bar";
                            worksheetVOIDITEMS.Cells[1, 1].Style.Font.Bold = true;
                            worksheetVOIDITEMS.Cells[1, 1, 1, 4].Merge = true;
                            worksheetVOIDITEMS.Cells[2, 1].Value = "Void Items Report";
                            worksheetVOIDITEMS.Cells[2, 1].Style.Font.Bold = true;
                            worksheetVOIDITEMS.Cells[2, 1, 2, 4].Merge = true;
                            worksheetVOIDITEMS.Cells[3, 1].Value = "Transaction Date :" + " " + System.DateTime.Now.ToString("MMMM dd, yyyy");
                            worksheetVOIDITEMS.Cells[3, 1].Style.Font.Bold = true;
                            worksheetVOIDITEMS.Cells[3, 1, 3, 4].Merge = true;

                            //Header
                            worksheetVOIDITEMS.Cells[5, 1].Value = "Date Voided";
                            worksheetVOIDITEMS.Cells[5, 2].Value = "Control Number";
                            worksheetVOIDITEMS.Cells[5, 3].Value = "Table";
                            worksheetVOIDITEMS.Cells[5, 4].Value = "Floor";
                            worksheetVOIDITEMS.Cells[5, 5].Value = "Item";
                            worksheetVOIDITEMS.Cells[5, 6].Value = "Category";
                            worksheetVOIDITEMS.Cells[5, 7].Value = "Price";
                            worksheetVOIDITEMS.Cells[5, 8].Value = "Quantity";
                            worksheetVOIDITEMS.Cells[5, 9].Value = "Amount";
                            worksheetVOIDITEMS.Cells[5, 10].Value = "Authorized By";
                            worksheetVOIDITEMS.Cells[5, 11].Value = "Voided By";
                            worksheetVOIDITEMS.Cells[5, 1, 5, 11].Style.Font.Bold = true;

                            //Freeze upto row 6 and column 4
                            //worksheetSALESINVOICE.View.FreezePanes(6, 4);

                            int intRowCounter = 6;

                            while (drEXPORTVOIDITEMS.Read())
                            {
                                //Generate Rows
                                worksheetVOIDITEMS.Cells[intRowCounter, 1].Value = drEXPORTVOIDITEMS["DateVoided"].ToString().Trim();
                                worksheetVOIDITEMS.Cells[intRowCounter, 2].Value = drEXPORTVOIDITEMS["ControlNumber"].ToString().Trim();
                                worksheetVOIDITEMS.Cells[intRowCounter, 3].Value = drEXPORTVOIDITEMS["TableName"].ToString().Trim();
                                worksheetVOIDITEMS.Cells[intRowCounter, 4].Value = drEXPORTVOIDITEMS["FloorName"].ToString().Trim();
                                worksheetVOIDITEMS.Cells[intRowCounter, 5].Value = drEXPORTVOIDITEMS["ItemName"].ToString().Trim();
                                worksheetVOIDITEMS.Cells[intRowCounter, 6].Value = drEXPORTVOIDITEMS["CategoryName"].ToString().Trim();
                                worksheetVOIDITEMS.Cells[intRowCounter, 7].Value = Convert.ToDecimal(drEXPORTVOIDITEMS["ItemPrice"]);
                                worksheetVOIDITEMS.Cells[intRowCounter, 8].Value = drEXPORTVOIDITEMS["ItemQuantity"].ToString().Trim();
                                worksheetVOIDITEMS.Cells[intRowCounter, 9].Value = Convert.ToDecimal(drEXPORTVOIDITEMS["ItemAmount"]);
                                worksheetVOIDITEMS.Cells[intRowCounter, 10].Value = drEXPORTVOIDITEMS["VoidAuthorizedBy"].ToString().Trim();
                                worksheetVOIDITEMS.Cells[intRowCounter, 11].Value = drEXPORTVOIDITEMS["VoidedBy"].ToString().Trim();

                                intRowCounter++;
                            }

                            if (intRowCounter == 6)
                            {
                                worksheetVOIDITEMS.Cells[intRowCounter, 1].Value = "No records to display";
                            }

                            //Column Format
                            worksheetVOIDITEMS.Cells[worksheetVOIDITEMS.Dimension.Address].AutoFitColumns();
                            worksheetVOIDITEMS.Column(7).Style.Numberformat.Format = "#,##0.00";
                            worksheetVOIDITEMS.Column(9).Style.Numberformat.Format = "#,##0.00";

                            worksheetVOIDITEMS.Column(7).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            worksheetVOIDITEMS.Column(9).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            //Font Size
                            worksheetVOIDITEMS.Cells[worksheetVOIDITEMS.Dimension.Address].Style.Font.Size = 10;
                        }

                        //Save the file
                        package.Save();

                        //System Logs
                        strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Void Items - Export", strFileName + " has been successfully generated.", strUserID);

                        //Download the report file
                        string filePath = strSaveVoidItems.ToString();
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
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Void Items - Export", strFileName + " : " + ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //Void Items
                    this.divVoidItems.Visible = false;
                }
            }
        }

        //Error Message
        protected void btnReload_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            Response.Redirect("VoidItems.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

    }
}