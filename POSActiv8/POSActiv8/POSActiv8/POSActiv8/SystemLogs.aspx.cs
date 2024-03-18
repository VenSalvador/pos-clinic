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

namespace POSActiv8
{
    public partial class SystemLogs : System.Web.UI.Page
    {
        SystemLogsBL slBL = new SystemLogsBL();
        ErrorLogsBL elBL = new ErrorLogsBL();
        string strUserID;
        int intUserLevel;
        int intUserRole;
        string strErrorLogs;
        string strSystemLogs;
        DateTime dteReferenceDateFrom;
        DateTime dteReferenceDateTo;
        string strReferenceDateFrom;
        string strReferenceDateTo;

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
                    //Reference Date
                    dteReferenceDateFrom = System.DateTime.Now;
                    strReferenceDateFrom = dteReferenceDateFrom.ToString("MM/dd/yyyy");
                    dteReferenceDateTo = System.DateTime.Now;
                    strReferenceDateTo = dteReferenceDateTo.ToString("MM/dd/yyyy");
                    this.txtReferenceDate.Text = dteReferenceDateFrom.ToString("yyyy-MM-dd");

                    //System Logs
                    using (MySqlDataReader drSYSTEMLOGS = slBL.SystemLogs_View(1, strReferenceDateFrom, strReferenceDateTo))
                    {
                        this.lblSystemLogs.Visible = true;

                        if (drSYSTEMLOGS.HasRows)
                        {
                            //Gridview
                            this.grdSystemLogs.DataSource = drSYSTEMLOGS;
                            this.grdSystemLogs.DataBind();
                            this.grdSystemLogs.Visible = true;

                            //Count
                            this.lblSystemLogs.InnerText = "Showing " + string.Format("{0:n0}", this.grdSystemLogs.Rows.Count) + " records";

                            //Button Controls
                            this.btnExport.Visible = true;
                        }

                        else
                        {
                            //Gridview
                            this.grdSystemLogs.Visible = false;

                            //Count
                            this.lblSystemLogs.InnerText = "No records found";

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
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "System Logs - Page Load", ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //System Logs
                    this.divSystemLogs.Visible = false;
                }
            }
        }

        //Button Controls
        protected void txtReferenceDate_TextChanged(object sender, EventArgs e)
        {
            if (!DateTime.TryParse(this.txtReferenceDate.Text, out dteReferenceDateFrom))
            {
                //Alert
                ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Invalid reference date format', '', 'error'); ", true);
            }

            else
            {
                try
                {
                    //Reference Date
                    dteReferenceDateFrom = Convert.ToDateTime(this.txtReferenceDate.Text);
                    strReferenceDateFrom = dteReferenceDateFrom.ToString("MM/dd/yyyy");
                    dteReferenceDateTo = Convert.ToDateTime(this.txtReferenceDate.Text);
                    strReferenceDateTo = dteReferenceDateTo.ToString("MM/dd/yyyy");
                    this.txtReferenceDate.Text = dteReferenceDateFrom.ToString("yyyy-MM-dd");

                    //System Logs
                    using (MySqlDataReader drSYSTEMLOGS = slBL.SystemLogs_View(2, strReferenceDateFrom, strReferenceDateTo))
                    {
                        this.divSystemLogs.Visible = true;
                        this.lblSystemLogs.Visible = true;

                        if (drSYSTEMLOGS.HasRows)
                        {
                            //Gridview
                            this.grdSystemLogs.DataSource = drSYSTEMLOGS;
                            this.grdSystemLogs.DataBind();
                            this.grdSystemLogs.Visible = true;

                            //Count
                            this.lblSystemLogs.InnerText = "Showing " + string.Format("{0:n0}", this.grdSystemLogs.Rows.Count) + " records";

                            //Button Controls
                            this.btnExport.Visible = true;
                        }

                        else
                        {
                            //Gridview
                            this.grdSystemLogs.Visible = false;

                            //Count
                            this.lblSystemLogs.InnerText = "No records found";

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
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "System Logs - Search", ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //System Logs
                    this.divSystemLogs.Visible = false;
                }
            }
        }

        protected void btnExport_Click(object sender, System.EventArgs e)
        {
            if (this.txtReferenceDate.Text != string.Empty)
            {
                this.txtDateFrom.Text = this.txtReferenceDate.Text;
                this.txtDateTo.Text = this.txtReferenceDate.Text;
            }

            else
            {
                this.txtDateFrom.Text = string.Empty;
                this.txtDateTo.Text = string.Empty;
            }

            //Show Modal
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
        }

        //Export
        protected void btnCancelExportSystemLogs_Click(object sender, EventArgs e)
        {
            this.txtDateFrom.Text = string.Empty;
            this.txtDateTo.Text = string.Empty;
            this.lblValidationExportSystemLogs.Text = string.Empty;

            //Hide Modal
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "hideModal();", true);
        }

        protected void btnExportSystemLogs_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];

            if (this.txtDateFrom.Text == string.Empty)
            {
                this.lblValidationExportSystemLogs.Text = "Date from must not be empty.";
                this.lblValidationExportSystemLogs.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else if (!DateTime.TryParse(this.txtDateFrom.Text, out dteReferenceDateFrom))
            {
                this.lblValidationExportSystemLogs.Text = "Invalid date from format.";
                this.lblValidationExportSystemLogs.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else if (this.txtDateTo.Text == string.Empty)
            {
                this.lblValidationExportSystemLogs.Text = "Date to must not be empty.";
                this.lblValidationExportSystemLogs.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else if (!DateTime.TryParse(this.txtDateTo.Text, out dteReferenceDateTo))
            {
                this.lblValidationExportSystemLogs.Text = "Invalid date to format.";
                this.lblValidationExportSystemLogs.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else if (Convert.ToDateTime(this.txtDateTo.Text) < Convert.ToDateTime(this.txtDateFrom.Text))
            {
                this.lblValidationExportSystemLogs.Text = "Date to must not be less than the date from.";
                this.lblValidationExportSystemLogs.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else
            {
                var strFileName = string.Empty;

                try
                {
                    //Reports Generated
                    string strReportsGeneratedPath = HttpContext.Current.Server.MapPath(string.Format("~/ReportsGenerated/"));

                    //Reference Date
                    dteReferenceDateFrom = Convert.ToDateTime(this.txtDateFrom.Text);
                    strReferenceDateFrom = dteReferenceDateFrom.ToString("MM/dd/yyyy");
                    dteReferenceDateTo = Convert.ToDateTime(this.txtDateTo.Text);
                    strReferenceDateTo = dteReferenceDateTo.ToString("MM/dd/yyyy");

                    //Source File
                    strFileName = "SystemLogs_" + dteReferenceDateFrom.ToString("yyyyMMdd") + "_" + dteReferenceDateTo.ToString("yyyyMMdd") + ".xlsx";
                    var strSaveSystemLogs = new FileInfo(Path.Combine(strReportsGeneratedPath, strFileName));

                    //Check if file exist then delete
                    if (System.IO.File.Exists(strSaveSystemLogs.ToString()))
                    {
                        System.IO.File.Delete(strSaveSystemLogs.ToString());
                    }

                    using (var package = new ExcelPackage(strSaveSystemLogs))
                    {
                        //Export System Logs
                        using (MySqlDataReader drEXPORTSYSTEMLOGS = slBL.SystemLogs_View(2, strReferenceDateFrom, strReferenceDateTo))
                        {
                            //Worksheet
                            ExcelWorksheet worksheetEXPORTSYSTEMLOGS = package.Workbook.Worksheets.Add(strFileName);
                            worksheetEXPORTSYSTEMLOGS.Cells.Style.Font.Size = 10;

                            //Header
                            worksheetEXPORTSYSTEMLOGS.Cells[1, 1].Value = "Source File";
                            worksheetEXPORTSYSTEMLOGS.Cells[1, 2].Value = "Activity";
                            worksheetEXPORTSYSTEMLOGS.Cells[1, 3].Value = "Remarks";
                            worksheetEXPORTSYSTEMLOGS.Cells[1, 4].Value = "Date and Time Posted";
                            worksheetEXPORTSYSTEMLOGS.Cells[1, 5].Value = "Posted By";
                            worksheetEXPORTSYSTEMLOGS.Cells[1, 1, 1, 5].Style.Font.Bold = true;

                            int intCounter = 0;

                            while (drEXPORTSYSTEMLOGS.Read())
                            {
                                //Generate Rows
                                worksheetEXPORTSYSTEMLOGS.Cells[2 + intCounter, 1].Value = drEXPORTSYSTEMLOGS.GetValue(1).ToString().Trim();
                                worksheetEXPORTSYSTEMLOGS.Cells[2 + intCounter, 2].Value = drEXPORTSYSTEMLOGS.GetValue(2).ToString().Trim();
                                worksheetEXPORTSYSTEMLOGS.Cells[2 + intCounter, 3].Value = drEXPORTSYSTEMLOGS.GetValue(3).ToString().Trim();
                                worksheetEXPORTSYSTEMLOGS.Cells[2 + intCounter, 4].Value = drEXPORTSYSTEMLOGS.GetValue(4).ToString().Trim();
                                worksheetEXPORTSYSTEMLOGS.Cells[2 + intCounter, 5].Value = drEXPORTSYSTEMLOGS.GetValue(5).ToString().Trim();

                                intCounter++;
                            }

                            if (intCounter == 0)
                            {
                                worksheetEXPORTSYSTEMLOGS.Cells[2 + intCounter, 1].Value = "No records found";
                            }

                            //Column Format
                            worksheetEXPORTSYSTEMLOGS.Cells[worksheetEXPORTSYSTEMLOGS.Dimension.Address].AutoFitColumns();

                            //Font Size
                            worksheetEXPORTSYSTEMLOGS.Cells[worksheetEXPORTSYSTEMLOGS.Dimension.Address].Style.Font.Size = 10;

                            //Save the file
                            package.Save();

                            //System Logs
                            strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "System Logs - Export", strFileName + " has been successfully generated.", strUserID);

                            ////Download the report file
                            //Response.ContentType = ContentType;
                            //Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(strSaveSystemLogs));
                            //Response.WriteFile(strSaveSystemLogs);
                            //Response.End();
                        }

                    }
                }

                catch (Exception ex)
                {
                    //Error Message
                    this.divErrorMessage.Visible = true;
                    this.lblErrorMessage.InnerText = ex.Message;
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "System Logs - Export", strFileName + " : " + ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //System Logs
                    this.divSystemLogs.Visible = false;
                }
            }
        }

        //Error Message
        protected void btnReload_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            Response.Redirect("systemlogs.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

    }
}