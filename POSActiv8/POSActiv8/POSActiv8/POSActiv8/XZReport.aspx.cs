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
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using iTextSharp.text.pdf.fonts;
using MySql.Data.MySqlClient;

using BusinessLogic;
using DataAccess;

namespace POSActiv8
{
    public partial class XZReport : System.Web.UI.Page
    {
        SystemLogsBL slBL = new SystemLogsBL();
        ErrorLogsBL elBL = new ErrorLogsBL();
        RegisterShiftBL registershiftBL = new RegisterShiftBL();
        PointofSaleBL posBL = new PointofSaleBL();
        POSSessionsBL possessBL = new POSSessionsBL();
        string strUserID;
        int intUserLevel;
        int intUserRole;
        int intTransactionType;
        string strErrorLogs = String.Empty;
        string strSystemLogs = String.Empty;
        DateTime dteTransactionDate;

        public void POSTerminals()
        {
            //POS Terminals
            using (MySqlDataReader drPOSTERMINALS = possessBL.POS_Sessions_View("1", Convert.ToDateTime(this.txtTransactionDate.Text).ToString("yyyy-MM-dd")))
            {
                this.ddlPOSTerminal.Items.Clear();
                this.ddlPOSTerminal.DataSource = drPOSTERMINALS;
                this.ddlPOSTerminal.DataValueField = "PostedBy";
                this.ddlPOSTerminal.DataTextField = "CashierName";
                this.ddlPOSTerminal.DataBind();
            }

            //this.ddlPOSTerminal.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Employee", string.Empty));
            //this.ddlPOSTerminal.SelectedIndex = 0;
        }

        public PdfPCell getCell(String text, int alignment)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text));
            cell.Padding = 0;
            cell.HorizontalAlignment = alignment;
            cell.Border = PdfPCell.NO_BORDER;
            return cell;
        }

        //Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                strUserID = Request.QueryString["userid"];
                intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
                intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);
                intTransactionType = Convert.ToInt32(Request.QueryString["transactiontype"]);

                //Error Message
                this.divErrorMessage.Visible = false;

                //XZReport
                this.divXZReport.Visible = true;

                if (intTransactionType == 1)
                {
                    this.lblPageTitle.InnerText = "X Report";
                }

                else
                {
                    this.lblPageTitle.InnerText = "Z Report";
                }

                try
                {
                    //Transaction Date
                    dteTransactionDate = System.DateTime.Now;
                    this.txtTransactionDate.Text = dteTransactionDate.ToString("yyyy-MM-dd");

                    POSTerminals();
                }

                catch (Exception ex)
                {
                    //Error Message
                    this.divErrorMessage.Visible = true;
                    this.lblErrorMessage.InnerText = ex.Message;
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "XZ Report - Page Load (" + intTransactionType + ")", ex.Message, strUserID);

                    //XReport
                    this.divXZReport.Visible = false;
                }
            }
        }

        //Button Controls
        protected void txtTransactionDate_TextChanged(object sender, EventArgs e)
        {
            //Transaction Date
            dteTransactionDate = Convert.ToDateTime(this.txtTransactionDate.Text);
            this.txtTransactionDate.Text = dteTransactionDate.ToString("yyyy-MM-dd");

            POSTerminals();
        }

        protected void btnExport_Click(object sender, System.EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);
            intTransactionType = Convert.ToInt32(Request.QueryString["transactiontype"]);

            if (this.txtTransactionDate.Text == string.Empty)
            {
                //Alert
                ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Required Field', 'Transaction date must not be empty.', 'warning'); ", true);
            }

            else if (this.lblTotalOrders.InnerText == "0")
            {
                //Alert
                ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Extraction Failed', 'No transaction found on the selected date.', 'warning'); ", true);
            }

            else
            {
                var strFileName = string.Empty;
                var strReportTitle = string.Empty;

                try
                {
                    //Transaction Date
                    dteTransactionDate = Convert.ToDateTime(this.txtTransactionDate.Text);

                    //Source File
                    if (intTransactionType == 1) //X Report
                    {
                        strFileName = "Activ8_XReport_" + dteTransactionDate.ToString("yyyyMMdd");
                        strReportTitle = "POS X REPORT";
                    }

                    else //Z Report
                    {
                        strFileName = "Activ8_ZReport_" + dteTransactionDate.ToString("yyyyMMdd");
                        strReportTitle = "POS Z REPORT";
                    }

                    //Create Memory Stream
                    using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                    {
                        Document document = new Document(PageSize.LETTER, 30, 30, 30, 60);
                        Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                        Font contentFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                        Font fontFooter = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 9);

                        PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                        writer.ViewerPreferences = PdfWriter.PageModeUseOutlines;
                        document.Open();

                        //Logo
                        string imageURL = Server.MapPath(".") + "/images/activ8-black.jpg";
                        iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(imageURL);
                        imgLogo.ScaleToFit(140f, 120f);
                        imgLogo.SpacingBefore = 10f;
                        imgLogo.SpacingAfter = 1f;
                        imgLogo.Alignment = Element.ALIGN_CENTER;
                        document.Add(imgLogo);

                        document.Add(new Paragraph("\n"));

                        //Report Title
                        Chunk chunkTitle = new Chunk(strReportTitle, boldFont);
                        chunkTitle.SetUnderline(0.1f, -2f); //0.1 thick, -2 y-location
                        Paragraph rptTitle = new Paragraph(chunkTitle);
                        rptTitle.Alignment = Element.ALIGN_CENTER;
                        document.Add(rptTitle);

                        document.Add(new Paragraph("\n"));

                        //XZ Report
                        using (MySqlDataReader drXZREPORT = registershiftBL.POS_XZReport_View(intTransactionType, dteTransactionDate))
                        {
                            if (drXZREPORT.Read())
                            {
                                //
                                PdfPTable tblXZReport = new PdfPTable(2);
                                tblXZReport.WidthPercentage = 80;
                                tblXZReport.AddCell(getCell("Number of Transactions : ", PdfPCell.ALIGN_LEFT));
                                tblXZReport.AddCell(getCell(drXZREPORT["TotalTransactions"].ToString(), PdfPCell.ALIGN_RIGHT));
                                tblXZReport.AddCell(getCell("Total Net Sales : ", PdfPCell.ALIGN_LEFT));
                                tblXZReport.AddCell(getCell(drXZREPORT["TotalNetSales"].ToString(), PdfPCell.ALIGN_RIGHT));
                                document.Add(tblXZReport);

                                document.Add(new Paragraph("\n"));

                                if (intTransactionType == 1) //Opening
                                {
                                    PdfPTable tblOpeningClosingAmount = new PdfPTable(2);
                                    tblOpeningClosingAmount.WidthPercentage = 80;
                                    tblOpeningClosingAmount.AddCell(getCell("Opening Amount : ", PdfPCell.ALIGN_LEFT));
                                    tblOpeningClosingAmount.AddCell(getCell(drXZREPORT["OpeningAmount"].ToString(), PdfPCell.ALIGN_RIGHT));
                                    tblOpeningClosingAmount.AddCell(getCell("Expected Drawer : ", PdfPCell.ALIGN_LEFT));
                                    tblOpeningClosingAmount.AddCell(getCell(drXZREPORT["ExpectedDrawer"].ToString(), PdfPCell.ALIGN_RIGHT));
                                    document.Add(tblOpeningClosingAmount);
                                }

                                else //Closing
                                {
                                    PdfPTable tblOpeningClosingAmount = new PdfPTable(2);
                                    tblOpeningClosingAmount.WidthPercentage = 80;
                                    tblOpeningClosingAmount.AddCell(getCell("Opening Amount : ", PdfPCell.ALIGN_LEFT));
                                    tblOpeningClosingAmount.AddCell(getCell(drXZREPORT["OpeningAmount"].ToString(), PdfPCell.ALIGN_RIGHT));
                                    tblOpeningClosingAmount.AddCell(getCell("Closing Amount : ", PdfPCell.ALIGN_LEFT));
                                    tblOpeningClosingAmount.AddCell(getCell(drXZREPORT["ClosingAmount"].ToString(), PdfPCell.ALIGN_RIGHT));
                                    //tblOpeningClosingAmount.AddCell(getCell(drXZREPORT["OverShort"].ToString(), PdfPCell.ALIGN_LEFT));
                                    //tblOpeningClosingAmount.AddCell(getCell(drXZREPORT["OverShortAmount"].ToString(), PdfPCell.ALIGN_RIGHT));
                                    document.Add(tblOpeningClosingAmount);
                                }

                                document.Add(new Paragraph("\n"));

                                PdfPTable tblOrderStatus = new PdfPTable(2);
                                tblOrderStatus.WidthPercentage = 80;
                                tblOrderStatus.AddCell(getCell("Order Status", PdfPCell.ALIGN_LEFT));
                                tblOrderStatus.AddCell(getCell("", PdfPCell.ALIGN_RIGHT));
                                tblOrderStatus.AddCell(getCell("New", PdfPCell.ALIGN_LEFT));
                                tblOrderStatus.AddCell(getCell(drXZREPORT["NewOrders"].ToString(), PdfPCell.ALIGN_RIGHT));
                                tblOrderStatus.AddCell(getCell("Delivered", PdfPCell.ALIGN_LEFT));
                                tblOrderStatus.AddCell(getCell(drXZREPORT["Delivered"].ToString(), PdfPCell.ALIGN_RIGHT));
                                tblOrderStatus.AddCell(getCell("Paid", PdfPCell.ALIGN_LEFT));
                                tblOrderStatus.AddCell(getCell(drXZREPORT["Paid"].ToString(), PdfPCell.ALIGN_RIGHT));
                                tblOrderStatus.AddCell(getCell("Voided", PdfPCell.ALIGN_LEFT));
                                tblOrderStatus.AddCell(getCell(drXZREPORT["Voided"].ToString(), PdfPCell.ALIGN_RIGHT));
                                tblOrderStatus.AddCell(getCell("Cancelled", PdfPCell.ALIGN_LEFT));
                                tblOrderStatus.AddCell(getCell(drXZREPORT["Cancelled"].ToString(), PdfPCell.ALIGN_RIGHT));
                                document.Add(tblOrderStatus);

                                document.Add(new Paragraph("\n"));

                                PdfPTable tblPaymentTypes = new PdfPTable(2);
                                tblPaymentTypes.WidthPercentage = 80;
                                tblPaymentTypes.AddCell(getCell("Payment Types", PdfPCell.ALIGN_LEFT));
                                tblPaymentTypes.AddCell(getCell("", PdfPCell.ALIGN_RIGHT));
                                tblPaymentTypes.AddCell(getCell("Cash Sales", PdfPCell.ALIGN_LEFT));
                                tblPaymentTypes.AddCell(getCell(drXZREPORT["CashSales"].ToString(), PdfPCell.ALIGN_RIGHT));
                                tblPaymentTypes.AddCell(getCell("Credit Card", PdfPCell.ALIGN_LEFT));
                                tblPaymentTypes.AddCell(getCell(drXZREPORT["CreditCardSales"].ToString(), PdfPCell.ALIGN_RIGHT));
                                tblPaymentTypes.AddCell(getCell("GCash", PdfPCell.ALIGN_LEFT));
                                tblPaymentTypes.AddCell(getCell(drXZREPORT["GCashSales"].ToString(), PdfPCell.ALIGN_RIGHT));
                                document.Add(tblPaymentTypes);
                            }
                        }

                        document.Add(new Paragraph("\n"));
                        document.Add(new Paragraph("\n"));

                        UserProfilesBL userprofBL = new UserProfilesBL();
                        var strUserName = string.Empty;

                        using (MySqlDataReader drUSERPROFILE = userprofBL.UserProfiles_View(strUserID, string.Empty))
                        {
                            if (drUSERPROFILE.Read())
                            {
                                strUserName = drUSERPROFILE["FullName"].ToString().Trim();
                            }
                        }

                        //Footer
                        var strFooter = "Generated by " + strUserName + " on " + System.DateTime.Now.ToString("MMMM dd, yyyy");

                        Chunk chunkSystemDateTimeGenerated = new Chunk(strFooter, fontFooter);
                        Paragraph systemdatetimegenerated = new Paragraph(chunkSystemDateTimeGenerated);
                        systemdatetimegenerated.Alignment = Element.ALIGN_CENTER;
                        document.Add(systemdatetimegenerated);

                        document.Close();
                        byte[] bytes = memoryStream.ToArray();
                        memoryStream.Close();
                        Response.Clear();
                        Response.ContentType = "application/pdf";

                        string pdfName = strFileName;

                        Response.AddHeader("Content-Disposition", "attachment; filename=" + pdfName + ".pdf");
                        Response.ContentType = "application/pdf";
                        Response.Buffer = true;
                        Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                        Response.BinaryWrite(bytes);
                        Response.End();
                        Response.Close();
                    }
                }

                catch (Exception ex)
                {
                    //Error Message
                    this.divErrorMessage.Visible = true;
                    this.lblErrorMessage.InnerText = ex.Message;
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "XZ Report - Export", strFileName + " : " + ex.Message, strUserID);

                    //XZReport
                    this.divXZReport.Visible = false;
                }
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);
            intTransactionType = Convert.ToInt32(Request.QueryString["transactiontype"]);

            if (!DateTime.TryParse(this.txtTransactionDate.Text, out dteTransactionDate))
            {
                //Alert
                ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Invalid Date', 'Invalid transaction date format', 'warning'); ", true);
            }

            else if (this.ddlPOSTerminal.Text == String.Empty)
            {
                //Alert
                ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Required Field', 'POS terminal must not be empty.', 'warning'); ", true);
            }

            //else if (this.ddlPOSTerminal.SelectedItem.Text == "Select Terminal")
            //{
            //    //Alert
            //    ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Required Field', 'POS terminal must not be empty.', 'warning'); ", true);
            //}

            else
            {
                var strFileName = string.Empty;
                var strReportTitle = string.Empty;

                try
                {
                    //Transaction Date
                    dteTransactionDate = Convert.ToDateTime(this.txtTransactionDate.Text);

                    //Source File
                    if (intTransactionType == 1) //X Report
                    {
                        strFileName = "Activ8_XReport_" + dteTransactionDate.ToString("yyyyMMdd");
                        strReportTitle = "POS X REPORT";
                    }

                    else //Z Report
                    {
                        strFileName = "Activ8_ZReport_" + dteTransactionDate.ToString("yyyyMMdd");
                        strReportTitle = "POS Z REPORT";
                    }

                    //Create Memory Stream
                    using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                    {
                        Document document = new Document(PageSize.LETTER, 30, 30, 30, 60);
                        Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                        Font contentFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                        Font fontFooter = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 9);

                        PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                        writer.ViewerPreferences = PdfWriter.PageModeUseOutlines;
                        document.Open();

                        //Logo
                        string imageURL = Server.MapPath(".") + "/images/activ8-black.jpg";
                        iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(imageURL);
                        imgLogo.ScaleToFit(140f, 120f);
                        imgLogo.SpacingBefore = 10f;
                        imgLogo.SpacingAfter = 1f;
                        imgLogo.Alignment = Element.ALIGN_CENTER;
                        document.Add(imgLogo);

                        document.Add(new Paragraph("\n"));

                        //Report Title
                        Chunk chunkTitle = new Chunk(strReportTitle, boldFont);
                        chunkTitle.SetUnderline(0.1f, -2f); //0.1 thick, -2 y-location
                        Paragraph rptTitle = new Paragraph(chunkTitle);
                        rptTitle.Alignment = Element.ALIGN_CENTER;
                        document.Add(rptTitle);

                        document.Add(new Paragraph("\n"));

                        //XZ Report
                        using (MySqlDataReader drXZREPORT = posBL.POS_XZReport_View(intTransactionType, dteTransactionDate, this.ddlPOSTerminal.SelectedValue))
                        {
                            if (drXZREPORT.Read())
                            {
                                //
                                PdfPTable tblPOSTerminal = new PdfPTable(2);
                                tblPOSTerminal.WidthPercentage = 80;
                                tblPOSTerminal.AddCell(getCell("POS Terminal : ", PdfPCell.ALIGN_LEFT));
                                tblPOSTerminal.AddCell(getCell(drXZREPORT["TerminalName"].ToString(), PdfPCell.ALIGN_RIGHT));
                                tblPOSTerminal.AddCell(getCell("POS User : ", PdfPCell.ALIGN_LEFT));
                                tblPOSTerminal.AddCell(getCell(drXZREPORT["POSUser"].ToString(), PdfPCell.ALIGN_RIGHT));
                                document.Add(tblPOSTerminal);

                                document.Add(new Paragraph("\n"));

                                PdfPTable tblXZReport = new PdfPTable(2);
                                tblXZReport.WidthPercentage = 80;
                                tblXZReport.AddCell(getCell("Number of Orders : ", PdfPCell.ALIGN_LEFT));
                                tblXZReport.AddCell(getCell(drXZREPORT["TotalOrders"].ToString(), PdfPCell.ALIGN_RIGHT));
                                tblXZReport.AddCell(getCell("Total Net Sales : ", PdfPCell.ALIGN_LEFT));
                                tblXZReport.AddCell(getCell(drXZREPORT["TotalNetSales"].ToString(), PdfPCell.ALIGN_RIGHT));
                                document.Add(tblXZReport);

                                document.Add(new Paragraph("\n"));

                                if (intTransactionType == 1) //Opening
                                {
                                    PdfPTable tblOpeningClosingAmount = new PdfPTable(2);
                                    tblOpeningClosingAmount.WidthPercentage = 80;
                                    tblOpeningClosingAmount.AddCell(getCell("Opening Amount : ", PdfPCell.ALIGN_LEFT));
                                    tblOpeningClosingAmount.AddCell(getCell(drXZREPORT["OpeningAmount"].ToString(), PdfPCell.ALIGN_RIGHT));
                                    tblOpeningClosingAmount.AddCell(getCell("Expected Drawer : ", PdfPCell.ALIGN_LEFT));
                                    tblOpeningClosingAmount.AddCell(getCell(drXZREPORT["ExpectedDrawer"].ToString(), PdfPCell.ALIGN_RIGHT));
                                    document.Add(tblOpeningClosingAmount);
                                }

                                else //Closing
                                {
                                    PdfPTable tblOpeningClosingAmount = new PdfPTable(2);
                                    tblOpeningClosingAmount.WidthPercentage = 80;
                                    tblOpeningClosingAmount.AddCell(getCell("Opening Amount : ", PdfPCell.ALIGN_LEFT));
                                    tblOpeningClosingAmount.AddCell(getCell(drXZREPORT["OpeningAmount"].ToString(), PdfPCell.ALIGN_RIGHT));
                                    tblOpeningClosingAmount.AddCell(getCell("Closing Amount : ", PdfPCell.ALIGN_LEFT));
                                    tblOpeningClosingAmount.AddCell(getCell(drXZREPORT["ClosingAmount"].ToString(), PdfPCell.ALIGN_RIGHT));
                                    //tblOpeningClosingAmount.AddCell(getCell(drXZREPORT["OverShort"].ToString(), PdfPCell.ALIGN_LEFT));
                                    //tblOpeningClosingAmount.AddCell(getCell(drXZREPORT["OverShortAmount"].ToString(), PdfPCell.ALIGN_RIGHT));
                                    document.Add(tblOpeningClosingAmount);
                                }

                                document.Add(new Paragraph("\n"));

                                PdfPTable tblOrderStatus = new PdfPTable(2);
                                tblOrderStatus.WidthPercentage = 80;
                                tblOrderStatus.AddCell(getCell("Order Status", PdfPCell.ALIGN_LEFT));
                                tblOrderStatus.AddCell(getCell("", PdfPCell.ALIGN_RIGHT));
                                tblOrderStatus.AddCell(getCell("New", PdfPCell.ALIGN_LEFT));
                                tblOrderStatus.AddCell(getCell(drXZREPORT["NewOrders"].ToString(), PdfPCell.ALIGN_RIGHT));
                                tblOrderStatus.AddCell(getCell("Delivered", PdfPCell.ALIGN_LEFT));
                                tblOrderStatus.AddCell(getCell(drXZREPORT["Delivered"].ToString(), PdfPCell.ALIGN_RIGHT));
                                tblOrderStatus.AddCell(getCell("Paid", PdfPCell.ALIGN_LEFT));
                                tblOrderStatus.AddCell(getCell(drXZREPORT["Paid"].ToString(), PdfPCell.ALIGN_RIGHT));
                                tblOrderStatus.AddCell(getCell("Voided", PdfPCell.ALIGN_LEFT));
                                tblOrderStatus.AddCell(getCell(drXZREPORT["Voided"].ToString(), PdfPCell.ALIGN_RIGHT));
                                tblOrderStatus.AddCell(getCell("Cancelled", PdfPCell.ALIGN_LEFT));
                                tblOrderStatus.AddCell(getCell(drXZREPORT["Cancelled"].ToString(), PdfPCell.ALIGN_RIGHT));
                                document.Add(tblOrderStatus);

                                document.Add(new Paragraph("\n"));

                                PdfPTable tblPaymentTypes = new PdfPTable(2);
                                tblPaymentTypes.WidthPercentage = 80;
                                tblPaymentTypes.AddCell(getCell("Payment Types", PdfPCell.ALIGN_LEFT));
                                tblPaymentTypes.AddCell(getCell("", PdfPCell.ALIGN_RIGHT));
                                tblPaymentTypes.AddCell(getCell("Cash Sales", PdfPCell.ALIGN_LEFT));
                                tblPaymentTypes.AddCell(getCell(drXZREPORT["CashSales"].ToString(), PdfPCell.ALIGN_RIGHT));
                                tblPaymentTypes.AddCell(getCell("Credit Card", PdfPCell.ALIGN_LEFT));
                                tblPaymentTypes.AddCell(getCell(drXZREPORT["CreditCardSales"].ToString(), PdfPCell.ALIGN_RIGHT));
                                tblPaymentTypes.AddCell(getCell("GCash", PdfPCell.ALIGN_LEFT));
                                tblPaymentTypes.AddCell(getCell(drXZREPORT["GCashSales"].ToString(), PdfPCell.ALIGN_RIGHT));
                                document.Add(tblPaymentTypes);
                            }
                        }

                        document.Add(new Paragraph("\n"));
                        document.Add(new Paragraph("\n"));

                        UserProfilesBL userprofBL = new UserProfilesBL();
                        var strUserName = string.Empty;

                        using (MySqlDataReader drUSERPROFILE = userprofBL.UserProfiles_View(strUserID, string.Empty))
                        {
                            if (drUSERPROFILE.Read())
                            {
                                strUserName = drUSERPROFILE["FullName"].ToString().Trim();
                            }
                        }

                        //Footer
                        var strFooter = "Generated by " + strUserName + " on " + System.DateTime.Now.ToString("MMMM dd, yyyy");

                        Chunk chunkSystemDateTimeGenerated = new Chunk(strFooter, fontFooter);
                        Paragraph systemdatetimegenerated = new Paragraph(chunkSystemDateTimeGenerated);
                        systemdatetimegenerated.Alignment = Element.ALIGN_CENTER;
                        document.Add(systemdatetimegenerated);

                        document.Close();
                        byte[] bytes = memoryStream.ToArray();
                        memoryStream.Close();
                        Response.Clear();
                        Response.ContentType = "application/pdf";

                        string pdfName = strFileName;

                        Response.AddHeader("Content-Disposition", "attachment; filename=" + pdfName + ".pdf");
                        Response.ContentType = "application/pdf";
                        Response.Buffer = true;
                        Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                        Response.BinaryWrite(bytes);
                        Response.End();
                        Response.Close();
                    }
                }

                catch (Exception ex)
                {
                    //Error Message
                    this.divErrorMessage.Visible = true;
                    this.lblErrorMessage.InnerText = ex.Message;
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "XZ Report - Export", strFileName + " : " + ex.Message, strUserID);

                    //XZReport
                    this.divXZReport.Visible = false;
                }


            //try
            //{
            //    //Reference Date
            //    dteTransactionDate = Convert.ToDateTime(this.txtTransactionDate.Text);
            //    this.txtTransactionDate.Text = dteTransactionDate.ToString("yyyy-MM-dd");

            //    POSTerminals();

            //    this.lblXZReportTitle.InnerText = this.lblPageTitle.InnerText;
            //    this.lblXZReportTransactionDate.InnerText = dteTransactionDate.ToString("MMMM dd, yyyy");

            //    //XZReport
            //    using (SqlDataReader drXZREPORT = posBL.POS_XZReport_View(intTransactionType, Convert.ToDateTime(dteTransactionDate.ToString("yyyy-MM-dd")), this.ddlPOSTerminal.SelectedItem.Text.Trim()))
            //    {
            //        if (drXZREPORT.Read())
            //        {
            //            this.lblPOSTerminal.InnerText = drXZREPORT["TerminalName"].ToString();
            //            this.lblCashierName.InnerText = drXZREPORT["Cashier"].ToString();

            //            this.lblTotalOrders.InnerText = drXZREPORT["TotalOrders"].ToString();
            //            this.lblTotalNetSales.InnerText = drXZREPORT["TotalNetSales"].ToString();
            //            this.lblOpeningAmount.InnerText = drXZREPORT["OpeningAmount"].ToString();

            //            if (intTransactionType == 1) //Opening
            //            {
            //                //Expected Drawer
            //                this.divExpectedDrawer.Visible = true;
            //                this.divExpectedDrawerAmount.Visible = true;
            //                this.lblExpectedDrawer.InnerText = drXZREPORT["ExpectedDrawer"].ToString();

            //                //Closing Amount
            //                this.divClosing.Visible = false;
            //                this.divClosingAmount.Visible = false;

            //                //OverShort
            //                this.divOverShort.Visible = false;
            //                this.divOverShortAmount.Visible = false;
            //            }

            //            else //Closing
            //            {
            //                //Expected Drawer
            //                this.divExpectedDrawer.Visible = false;
            //                this.divExpectedDrawerAmount.Visible = false;

            //                //Closing Amount
            //                this.divClosing.Visible = true;
            //                this.divClosingAmount.Visible = true;
            //                this.lblClosingAmount.InnerText = drXZREPORT["ClosingAmount"].ToString();

            //                //OverShort
            //                this.divOverShort.Visible = false;
            //                //this.lblOverShort.InnerText = drXZREPORT["OverShort"].ToString();
            //                this.divOverShortAmount.Visible = false;
            //                //this.lblOverShortAmount.InnerText = drXZREPORT["OverShortAmount"].ToString();
            //            }

            //            //Order Status
            //            this.lblNewOrders.InnerText = drXZREPORT["NewOrders"].ToString();
            //            this.lblDelivered.InnerText = drXZREPORT["Delivered"].ToString();
            //            this.lblPaid.InnerText = drXZREPORT["Paid"].ToString();
            //            this.lblVoided.InnerText = drXZREPORT["Voided"].ToString();
            //            this.lblCancelled.InnerText = drXZREPORT["Cancelled"].ToString();

            //            //Payment Types
            //            this.lblCashSales.InnerText = drXZREPORT["CashSales"].ToString();
            //            this.lblCreditCard.InnerText = drXZREPORT["CreditCardSales"].ToString();
            //            this.lblGCash.InnerText = drXZREPORT["GCashSales"].ToString();
            //        }

            //        else
            //        {
            //            this.lblPOSTerminal.InnerText = "N/A";
            //            this.lblCashierName.InnerText = "N/A";

            //            this.lblTotalOrders.InnerText = "0";
            //            this.lblTotalNetSales.InnerText = "0.00";
            //            this.lblOpeningAmount.InnerText = "0.00";

            //            //Expected Drawer
            //            this.divExpectedDrawer.Visible = false;
            //            this.divExpectedDrawerAmount.Visible = false;

            //            //Closing Amount
            //            this.divClosing.Visible = false;
            //            this.divClosingAmount.Visible = false;

            //            //OverShort
            //            this.divOverShort.Visible = false;
            //            this.divOverShortAmount.Visible = false;

            //            //Order Status
            //            this.lblNewOrders.InnerText = "0.00";
            //            this.lblDelivered.InnerText = "0.00";
            //            this.lblPaid.InnerText = "0.00";
            //            this.lblVoided.InnerText = "0.00";
            //            this.lblCancelled.InnerText = "0.00";

            //            //Payment Types
            //            this.lblCashSales.InnerText = "0.00";
            //            this.lblCreditCard.InnerText = "0.00";
            //            this.lblGCash.InnerText = "0.00";
            //        }
            //    }

            //    //Show Offcanvas
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showXZReport();", true);
            //}

            //catch (Exception ex)
            //{
            //    //Error Message
            //    this.divErrorMessage.Visible = true;
            //    this.lblErrorMessage.InnerText = ex.Message;
            //    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "XZ Report - Transaction Date (" + intTransactionType + ")", ex.Message, strUserID);

            //    //XZReport
            //    this.divXZReport.Visible = false;
            //}
        }
        }

        //Error Message
        protected void btnReload_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);
            intTransactionType = Convert.ToInt32(Request.QueryString["transactiontype"]);

            Response.Redirect("XZReport.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrrole=" + intUserRole + "&transactiontype=" + intTransactionType, false);
            Context.ApplicationInstance.CompleteRequest();
        }

    }
}