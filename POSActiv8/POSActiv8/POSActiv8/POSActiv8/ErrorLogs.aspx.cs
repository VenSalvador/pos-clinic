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
    public partial class ErrorLogs : System.Web.UI.Page
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
                //baseclass.UserAccess(strUserID, intUserLevel, intUserRole, 4);

                //Error Message
                this.divErrorMessage.Visible = false;

                //Page Title
                this.divPageTitle.Visible = true;

                //Button Controls
                this.divButtonControls.Visible = true;

                //Reference Date
                DateTime dteReferenceDate = System.DateTime.Now;
                string strReferenceDate = dteReferenceDate.ToString("MM/dd/yyyy");

                try
                {
                    //Reference Date
                    dteReferenceDateFrom = System.DateTime.Now;
                    strReferenceDateFrom = dteReferenceDateFrom.ToString("MM/dd/yyyy");
                    dteReferenceDateTo = System.DateTime.Now;
                    strReferenceDateTo = dteReferenceDateTo.ToString("MM/dd/yyyy");
                    this.txtReferenceDate.Text = dteReferenceDateFrom.ToString("yyyy-MM-dd");

                    //Error Logs
                    using (MySqlDataReader drERRORLOGS = elBL.ErrorLogs_View(1, strReferenceDateFrom, strReferenceDateTo))
                    {
                        this.divErrorLogs.Visible = true;
                        this.lblErrorLogs.Visible = true;

                        if (drERRORLOGS.HasRows)
                        {
                            //Gridview
                            this.grdErrorLogs.DataSource = drERRORLOGS;
                            this.grdErrorLogs.DataBind();
                            this.grdErrorLogs.Visible = true;

                            //Count
                            this.lblErrorLogs.InnerText = "Showing " + string.Format("{0:n0}", this.grdErrorLogs.Rows.Count) + " records";

                            //Button Controls
                            this.btnExport.Visible = true;
                        }

                        else
                        {
                            //Gridview
                            this.grdErrorLogs.Visible = false;

                            //Count
                            this.lblErrorLogs.InnerText = "No records found";

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
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Error Logs - Page Load", ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //Error Logs
                    this.divErrorLogs.Visible = false;
                }
            }
        }

        //Button Controls
        protected void txtReferenceDate_TextChanged(object sender, EventArgs e)
        {
            if (!DateTime.TryParse(this.txtReferenceDate.Text, out dteReferenceDateFrom))
            {
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
                    this.txtReferenceDate.Text = strReferenceDateFrom;

                    //Error Logs
                    using (MySqlDataReader drERRORLOGS = elBL.ErrorLogs_View(2, strReferenceDateFrom, strReferenceDateTo))
                    {
                        this.divErrorLogs.Visible = true;
                        this.lblErrorLogs.Visible = true;

                        if (drERRORLOGS.HasRows)
                        {
                            //Gridview
                            this.grdErrorLogs.DataSource = drERRORLOGS;
                            this.grdErrorLogs.DataBind();
                            this.grdErrorLogs.Visible = true;

                            //Count
                            this.lblErrorLogs.InnerText = "Showing " + string.Format("{0:n0}", this.grdErrorLogs.Rows.Count) + " records";

                            //Button Controls
                            this.btnExport.Visible = true;
                        }

                        else
                        {
                            //Gridview
                            this.grdErrorLogs.Visible = false;

                            //Count
                            this.lblErrorLogs.InnerText = "No records found";

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
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Error Logs - Search", ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //Error Logs
                    this.divErrorLogs.Visible = false;
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
        protected void btnCancelExportErrorLogs_Click(object sender, EventArgs e)
        {
            this.txtDateFrom.Text = string.Empty;
            this.txtDateTo.Text = string.Empty;
            this.lblValidationExportErrorLogs.Text = string.Empty;

            //Hide Modal
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "hideModal();", true);
        }

        protected void btnExportErrorLogs_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];

            if (this.txtDateFrom.Text == string.Empty)
            {
                this.lblValidationExportErrorLogs.Text = "Date from must not be empty.";
                this.lblValidationExportErrorLogs.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else if (!DateTime.TryParse(this.txtDateFrom.Text, out dteReferenceDateFrom))
            {
                this.lblValidationExportErrorLogs.Text = "Invalid date from format.";
                this.lblValidationExportErrorLogs.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else if (this.txtDateTo.Text == string.Empty)
            {
                this.lblValidationExportErrorLogs.Text = "Date to must not be empty.";
                this.lblValidationExportErrorLogs.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else if (!DateTime.TryParse(this.txtDateTo.Text, out dteReferenceDateTo))
            {
                this.lblValidationExportErrorLogs.Text = "Invalid date to format.";
                this.lblValidationExportErrorLogs.Attributes["style"] = "color:Red;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showModal();", true);
            }

            else if (Convert.ToDateTime(this.txtDateTo.Text) < Convert.ToDateTime(this.txtDateFrom.Text))
            {
                this.lblValidationExportErrorLogs.Text = "Date to must not be less than the date from.";
                this.lblValidationExportErrorLogs.Attributes["style"] = "color:Red;";
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
                    strFileName = "ErrorLogs_" + dteReferenceDateFrom.ToString("yyyyMMdd") + "_" + dteReferenceDateTo.ToString("yyyyMMdd") + ".xlsx";
                    var strSaveErrorLogs = new FileInfo(Path.Combine(strReportsGeneratedPath, strFileName));

                    //Check if file exist then delete
                    if (System.IO.File.Exists(strSaveErrorLogs.ToString()))
                    {
                        System.IO.File.Delete(strSaveErrorLogs.ToString());
                    }

                    using (var package = new ExcelPackage(strSaveErrorLogs))
                    {
                        //Export System Logs
                        using (MySqlDataReader drEXPORTERRORLOGS = elBL.ErrorLogs_View(2, strReferenceDateFrom, strReferenceDateTo))
                        {
                            //Worksheet
                            ExcelWorksheet worksheetEXPORTERRORLOGS = package.Workbook.Worksheets.Add(strFileName);
                            worksheetEXPORTERRORLOGS.Cells.Style.Font.Size = 10;

                            //Header
                            worksheetEXPORTERRORLOGS.Cells[1, 1].Value = "Source";
                            worksheetEXPORTERRORLOGS.Cells[1, 2].Value = "Name";
                            worksheetEXPORTERRORLOGS.Cells[1, 3].Value = "Description";
                            worksheetEXPORTERRORLOGS.Cells[1, 4].Value = "Date and Time Posted";
                            worksheetEXPORTERRORLOGS.Cells[1, 5].Value = "Posted By";
                            worksheetEXPORTERRORLOGS.Cells[1, 1, 1, 5].Style.Font.Bold = true;

                            int intCounter = 0;

                            while (drEXPORTERRORLOGS.Read())
                            {
                                //Generate Rows
                                worksheetEXPORTERRORLOGS.Cells[2 + intCounter, 1].Value = drEXPORTERRORLOGS.GetValue(1).ToString().Trim();
                                worksheetEXPORTERRORLOGS.Cells[2 + intCounter, 2].Value = drEXPORTERRORLOGS.GetValue(2).ToString().Trim();
                                worksheetEXPORTERRORLOGS.Cells[2 + intCounter, 3].Value = drEXPORTERRORLOGS.GetValue(3).ToString().Trim();
                                worksheetEXPORTERRORLOGS.Cells[2 + intCounter, 4].Value = drEXPORTERRORLOGS.GetValue(4).ToString().Trim();
                                worksheetEXPORTERRORLOGS.Cells[2 + intCounter, 5].Value = drEXPORTERRORLOGS.GetValue(5).ToString().Trim();

                                intCounter++;
                            }

                            if (intCounter == 0)
                            {
                                worksheetEXPORTERRORLOGS.Cells[2 + intCounter, 1].Value = "No records found";
                            }

                            //Column Format
                            worksheetEXPORTERRORLOGS.Cells[worksheetEXPORTERRORLOGS.Dimension.Address].AutoFitColumns();

                            //Save the file
                            package.Save();

                            //System Logs
                            strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Error Logs - Export", strFileName + " has been successfully generated.", strUserID);
                            //
                        }
                    }
                }

                catch (Exception ex)
                {
                    //Error Message
                    this.divErrorMessage.Visible = true;
                    this.lblErrorMessage.InnerText = ex.Message;
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Error Logs - Export", strFileName + " : " + ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //Error Logs
                    this.divErrorLogs.Visible = false;
                }
            }
        }

        //Error Message
        protected void btnReload_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            Response.Redirect("ErrorLogs.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

    }
}