using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Diagnostics;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using iTextSharp.text.pdf.fonts;

using BusinessObject;
using BusinessLogic;
using DataAccess;
using POSActiv8.Classes;
using System.Net;

namespace POSActiv8
{
    public partial class POS : System.Web.UI.Page
    {
        baseclass baseclass = new baseclass();
        SystemLogsBL slBL = new SystemLogsBL();
        ErrorLogsBL elBL = new ErrorLogsBL();
        POSSessionsBL possessBL = new POSSessionsBL();
        RegisterShiftBL registershiftBL = new RegisterShiftBL();
        SystemParametersBL sysparamBL = new SystemParametersBL();
        SequenceBL seqBL = new SequenceBL();
        TableNamesBL tblnamesBL = new TableNamesBL();
        ItemCategoryBL itemcatBL = new ItemCategoryBL();
        ItemSubCategoryBL itemsubcatBL = new ItemSubCategoryBL();
        ItemMasterBL itemmastBL = new ItemMasterBL();
        PointofSaleBO posBO = new PointofSaleBO();
        PointofSaleBL posBL = new PointofSaleBL();
        DiscountsBL discBL = new DiscountsBL();
        ChargesBL chargesBL = new ChargesBL();
        UserProfilesBL userprofBL = new UserProfilesBL();
        string strUserID;
        int intUserLevel;
        int intUserRole;
        int intCMD;
        string strSystemLogs = string.Empty;
        string strErrorLogs = string.Empty;
        string strControlNumber = string.Empty;
        DateTime dteTransactionDate;
        private BaseColor colorGrey;

        //Transaction Date
        public void POSTransactionDate()
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            //Get IP Address of the machine
            string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //this.hfIPAddress.Value = ipAddress;

            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = Request.ServerVariables["REMOTE_ADDR"];
            }


            //Get Computer Name
            var strTerminalName = ipAddress;

            //Get transaction date from the POS Session
            System.Data.DataSet dsPOSSESSIONS = new System.Data.DataSet();
            dsPOSSESSIONS = DBHelper.GetData("SELECT TOP 1 SessionCode, SessionStatus, TerminalName, TransactionDate, OpeningAmount, ClosingAmount, PostedBy FROM POSSessions WHERE SessionStatus = '1' AND PostedBy = '" + strUserID + "'");

            if (dsPOSSESSIONS.Tables[0].Rows.Count > 0)
            {
                this.hfTransactionDate.Value = dsPOSSESSIONS.Tables[0].Rows[0]["TransactionDate"].ToString();

                //Delete orders without control number yet
                System.Data.DataSet dsDELETEORDERS = new System.Data.DataSet();
                dsDELETEORDERS = DBHelper.GetData("DELETE FROM OrderLines WHERE ControlNumber IS NULL AND FORMAT(TransactionDate, 'yyyy-MM-dd') = '" + Convert.ToDateTime(this.hfTransactionDate.Value).ToString("yyyy-MM-dd") + "' AND PostedBy = '" + strUserID + "'");
                dsDELETEORDERS.Clear();
            }

            else
            {
                this.hfTransactionDate.Value = string.Empty;
            }

            dsPOSSESSIONS.Clear();
        }

        //Discount Type
        public void DiscountType()
        {
            //Discount Type
            using (SqlDataReader drDISCOUNTTYPE = discBL.Discounts_View(0, string.Empty))
            {
                this.ddlDiscountType.Items.Clear();
                this.ddlDiscountType.DataSource = drDISCOUNTTYPE;
                this.ddlDiscountType.DataValueField = "RecordID";
                this.ddlDiscountType.DataTextField = "DiscountName";
                this.ddlDiscountType.DataBind();
            }

            this.ddlDiscountType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Discount", string.Empty));
            this.ddlDiscountType.SelectedIndex = 0;
        }

        //Order Items Quantity
        public void OrderItemsQuantity()
        {
            strUserID = Request.QueryString["userid"];

            //Orders
            using (SqlDataReader drMYORDERS = posBL.POS_Orders_View(this.hfControlNumber.Value, this.hfTableCode.Value, string.Empty, strUserID))
            {
                this.ddlOrderItemsQuantity.Items.Clear();
                this.ddlOrderItemsQuantity.DataSource = drMYORDERS;
                this.ddlOrderItemsQuantity.DataValueField = "ItemCode";
                this.ddlOrderItemsQuantity.DataTextField = "ItemNamePrice";
                this.ddlOrderItemsQuantity.DataBind();
            }

            this.ddlOrderItemsQuantity.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Item", string.Empty));
            this.ddlOrderItemsQuantity.SelectedIndex = 0;
        }

        //Service Charge
        public void ServiceCharge()
        {
            //Service Charge
            using (SqlDataReader drSERVICECHARGE = chargesBL.Charges_View(1, "1"))
            {
                this.ddlServiceCharge.Items.Clear();
                this.ddlServiceCharge.DataSource = drSERVICECHARGE;
                this.ddlServiceCharge.DataValueField = "ChargeAmount";
                this.ddlServiceCharge.DataTextField = "ChargeDescription";
                this.ddlServiceCharge.DataBind();
            }

            //this.ddlServiceCharge.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Charge", string.Empty));
            this.ddlServiceCharge.SelectedIndex = 0;
        }

        //Payment Type
        public void PaymentType()
        {
            //Payment Type
            using (SqlDataReader drPAYMENTTYPE = sysparamBL.SystemParameters_View(9))
            {
                this.ddlPaymentType.Items.Clear();
                this.ddlPaymentType.DataSource = drPAYMENTTYPE;
                this.ddlPaymentType.DataValueField = "ParameterValue";
                this.ddlPaymentType.DataTextField = "ParameterDescription";
                this.ddlPaymentType.DataBind();
            }

            //this.ddlPaymentType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Payment", string.Empty));
            this.ddlPaymentType.SelectedIndex = 0;
        }

        //Cancellation Types
        public void CancellationTypes()
        {
            //Cancellation Types
            using (SqlDataReader drCANCELTYPES = sysparamBL.SystemParameters_View(11))
            {
                this.ddlCancellationTypes.Items.Clear();
                this.ddlCancellationTypes.DataSource = drCANCELTYPES;
                this.ddlCancellationTypes.DataValueField = "ParameterValue";
                this.ddlCancellationTypes.DataTextField = "ParameterDescription";
                this.ddlCancellationTypes.DataBind();
            }

            this.ddlCancellationTypes.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Reason", string.Empty));
            this.ddlCancellationTypes.SelectedIndex = 0;
        }

        //Immediate Supervisor
        public void ImmediateSupervisor()
        {
            //Immediate Supervisor
            using (SqlDataReader drIMMEDIATESUPERVISOR = userprofBL.UserProfiles_View("1", string.Empty))
            {
                this.ddlImmediateSupervisor.Items.Clear();
                this.ddlImmediateSupervisor.DataSource = drIMMEDIATESUPERVISOR;
                this.ddlImmediateSupervisor.DataValueField = "NetworkID";
                this.ddlImmediateSupervisor.DataTextField = "FullName";
                this.ddlImmediateSupervisor.DataBind();
            }

            this.ddlImmediateSupervisor.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Immediate Supervisor", string.Empty));
            this.ddlImmediateSupervisor.SelectedIndex = 0;
        }

        public void OrderTotals()
        {
            strUserID = Request.QueryString["userid"];

            //Order Totals
            using (SqlDataReader drORDERTOTALS = posBL.POS_OrdersTotal_View(this.hfControlNumber.Value, strUserID))
            {
                if (drORDERTOTALS.Read())
                {
                    this.hfControlNumber.Value = drORDERTOTALS["ControlNumber"].ToString();

                    this.lblSubTotal.InnerText = drORDERTOTALS["SubTotal"].ToString();
                    this.lblTaxAmount.InnerText = drORDERTOTALS["TaxAmount"].ToString();
                    this.lblDiscountName.InnerText = drORDERTOTALS["DiscountName"].ToString();
                    this.lblDiscountAmount.InnerText = drORDERTOTALS["DiscountAmount"].ToString();
                    this.lblServiceChargeAmount.InnerText = drORDERTOTALS["ServiceCharge"].ToString();
                    this.lblGrandTotal.InnerText = drORDERTOTALS["GrandTotal"].ToString();
                }

                else
                {
                    this.lblSubTotal.InnerText = "0.00";
                    this.lblTaxAmount.InnerText = "0.00";
                    this.lblDiscountAmount.InnerText = "0.00";
                    this.lblServiceChargeAmount.InnerText = "0.00";
                    this.lblGrandTotal.InnerText = "0.00";
                }
            }
        }

        //Order Receipt

        public void OrderReceipt()
        {
            strUserID = Request.QueryString["userid"];

            //Create Memory Stream
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                Document document = new Document(PageSize.HALFLETTER, 30, 30, 20, 60);
                //document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                Font contentFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                Font fontFooter = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 9);

                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                writer.ViewerPreferences = PdfWriter.PageModeUseOutlines;
                document.Open();

                //Logo
                string imgLogoURL = Server.MapPath(".") + "/Images/activ8-black.jpg";
                iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(imgLogoURL);
                imgLogo.ScaleToFit(50f, 50f);
                imgLogo.SpacingBefore = 10f;
                imgLogo.SpacingAfter = 1f;
                imgLogo.Alignment = Element.ALIGN_CENTER;
                document.Add(imgLogo);

                //document.Add(new Paragraph("\n"));

                //Address
                Paragraph prgAddress = new Paragraph("8th Floor Victoria Sports Tower, EDSA Southbound, Quezon City", contentFont);
                prgAddress.Alignment = Element.ALIGN_CENTER;
                document.Add(prgAddress);

                document.Add(new Paragraph("\n"));

                //Sales Order
                Paragraph prgSalesOrder = new Paragraph("SALES ORDER", boldFont);
                prgSalesOrder.Alignment = Element.ALIGN_CENTER;
                document.Add(prgSalesOrder);

                //document.Add(new Paragraph("\n"));
                document.Add(new Paragraph("\n"));

                //Orders
                using (SqlDataReader drORDERS = posBL.POS_Orders_View(this.hfControlNumber.Value, this.hfTableCode.Value, string.Empty, strUserID))
                {
                    if (drORDERS.Read())
                    {
                        //Header
                        PdfPTable tblHeader = new PdfPTable(2);
                        tblHeader.WidthPercentage = 100;

                        PdfPCell invoice = new PdfPCell(new Phrase("Invoice :", contentFont));
                        invoice.Border = 0;
                        invoice.PaddingBottom = 5;
                        tblHeader.AddCell(invoice);
                        //tblHeader.AddCell(baseclass.getCell("Invoice : ", PdfPCell.ALIGN_LEFT));
                        PdfPCell invoicenumber = new PdfPCell(new Phrase(this.hfControlNumber.Value, contentFont));
                        invoicenumber.Border = 0;
                        invoicenumber.PaddingBottom = 5;
                        invoicenumber.HorizontalAlignment = 2;
                        tblHeader.AddCell(invoicenumber);

                        PdfPCell table = new PdfPCell(new Phrase("Table :", contentFont));
                        table.Border = 0;
                        table.PaddingBottom = 5;
                        tblHeader.AddCell(table);
                        PdfPCell tablename = new PdfPCell(new Phrase(this.lblTableName.InnerText, contentFont));
                        tablename.Border = 0;
                        tablename.PaddingBottom = 5;
                        tablename.HorizontalAlignment = 2;
                        tblHeader.AddCell(tablename);

                        PdfPCell datetime = new PdfPCell(new Phrase("Date/Time :", contentFont));
                        datetime.Border = 0;
                        datetime.PaddingBottom = 5;
                        tblHeader.AddCell(datetime);
                        PdfPCell datetimenow = new PdfPCell(new Phrase(System.DateTime.Now.ToString(), contentFont));
                        datetimenow.Border = 0;
                        datetimenow.PaddingBottom = 5;
                        datetimenow.HorizontalAlignment = 2;
                        tblHeader.AddCell(datetimenow);

                        PdfPCell cashier = new PdfPCell(new Phrase("Cashier :", contentFont));
                        cashier.Border = 0;
                        tblHeader.AddCell(cashier);
                        PdfPCell cashiername = new PdfPCell(new Phrase(drORDERS["PostedBy"].ToString().Trim(), contentFont));
                        cashiername.Border = 0;
                        cashiername.HorizontalAlignment = 2;
                        tblHeader.AddCell(cashiername);
                        document.Add(tblHeader);

                        //LineBreak
                        document.Add(new Paragraph(new Chunk(new LineSeparator(1f, 100f, BaseColor.LIGHT_GRAY, Element.ALIGN_CENTER, -1))));

                        //Order Items
                        PdfPTable tblOrderItems = new PdfPTable(2);
                        tblOrderItems.WidthPercentage = 100;
                        //tblOrderItems.SpacingBefore = 5f;
                        //tblOrderItems.SpacingAfter = 5f;

                        //Order Items
                        using (SqlDataReader drORDERITEMS = posBL.POS_Orders_View(this.hfControlNumber.Value, this.hfTableCode.Value, string.Empty, strUserID))
                        {
                            while (drORDERITEMS.Read())
                            {
                                PdfPCell itemname = new PdfPCell(new Phrase(drORDERITEMS["ItemName"].ToString(), contentFont));
                                itemname.Border = 0;
                                itemname.PaddingTop = 7;
                                itemname.PaddingBottom = 3;
                                itemname.Colspan = 2;
                                tblOrderItems.AddCell(itemname);

                                PdfPCell itempricequantity = new PdfPCell(new Phrase(drORDERITEMS["ItemPrice"].ToString() + " x " + drORDERITEMS["ItemQuantity"].ToString(), contentFont));
                                itempricequantity.Border = 0;
                                tblOrderItems.AddCell(itempricequantity);

                                PdfPCell itemamount = new PdfPCell(new Phrase(drORDERITEMS["ItemAmount"].ToString(), contentFont));
                                itemamount.Border = 0;
                                itemamount.HorizontalAlignment = 2;
                                tblOrderItems.AddCell(itemamount);
                            }
                        }

                        document.Add(tblOrderItems);

                        //LineBreak
                        document.Add(new Paragraph(new Chunk(new LineSeparator(1f, 100f, BaseColor.LIGHT_GRAY, Element.ALIGN_CENTER, -1))));

                        //Order Totals
                        PdfPTable tblOrderTotals = new PdfPTable(2);
                        tblOrderTotals.WidthPercentage = 100;
                        PdfPCell total = new PdfPCell(new Phrase("Total :", boldFont));
                        total.Border = 0;
                        total.PaddingTop = 10;
                        total.PaddingBottom = 5;
                        tblOrderTotals.AddCell(total);
                        PdfPCell totalamount = new PdfPCell(new Phrase("467.50", boldFont));
                        totalamount.Border = 0;
                        totalamount.PaddingTop = 10;
                        totalamount.PaddingBottom = 5;
                        totalamount.HorizontalAlignment = 2;
                        tblOrderTotals.AddCell(totalamount);

                        PdfPCell payment = new PdfPCell(new Phrase("Payment :", contentFont));
                        payment.Border = 0;
                        payment.PaddingBottom = 5;
                        tblOrderTotals.AddCell(payment);
                        PdfPCell paymentamount = new PdfPCell(new Phrase("500.00", contentFont));
                        paymentamount.Border = 0;
                        paymentamount.PaddingBottom = 5;
                        paymentamount.HorizontalAlignment = 2;
                        tblOrderTotals.AddCell(paymentamount);

                        PdfPCell change = new PdfPCell(new Phrase("Change :", contentFont));
                        change.Border = 0;
                        change.PaddingBottom = 5;
                        tblOrderTotals.AddCell(change);
                        PdfPCell changeamount = new PdfPCell(new Phrase("32.00", contentFont));
                        changeamount.Border = 0;
                        changeamount.PaddingBottom = 5;
                        changeamount.HorizontalAlignment = 2;
                        tblOrderTotals.AddCell(changeamount);
                        document.Add(tblOrderTotals);

                    }
                }

                document.Add(new Paragraph("\n"));

                //
                //Footer
                Paragraph footer1 = new Paragraph("THIS IS NOT AN OFFICIAL RECEIPT", fontFooter);
                footer1.Alignment = Element.ALIGN_CENTER;
                document.Add(footer1);

                document.Add(new Paragraph("\n"));
                
                Paragraph footer2 = new Paragraph("THANK YOU FOR DINING WITH US!", fontFooter);
                footer2.Alignment = Element.ALIGN_CENTER;
                document.Add(footer2);

                Paragraph footer3 = new Paragraph("PLEASE COME BACK AGAIN", fontFooter);
                footer3.Alignment = Element.ALIGN_CENTER;
                document.Add(footer3);

                document.Close();
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                Response.Clear();
                Response.ContentType = "application/pdf";

                //File Name
                string strFileName = "OrderReceipt_" + this.hfControlNumber.Value + "_" + System.DateTime.Now.ToString("yyyyMMdd");

                //System Logs
                //strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Orders - Receipt", "Order receipt has been generated (" + this.hfControlNumber.Value + " )", strUserID);

                ////Open PDF in the browser
                //string FilePath = Path.Combine(strFileName);
                //WebClient User = new WebClient();
                //Byte[] FileBuffer = User.DownloadData(FilePath);

                
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strFileName + ".pdf");
                Response.ContentType = "application/pdf";
                Response.Buffer = true;
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                Response.BinaryWrite(bytes);
                Response.End();
                Response.Close();
            }
        }

        public void GenerateOrderReceipt()
        {
            //Generate Order Receipt
            this.lblPOSCode.InnerText = this.hfControlNumber.Value;
            this.lblTableNameReceipt.InnerText = this.lblTableName.InnerText;
            this.lblPrintDateTimeReceipt.InnerText = System.DateTime.Now.ToString();

            //Order Items
            using (SqlDataReader drORDERITEMS = posBL.POS_Orders_View(this.hfControlNumber.Value, this.hfTableCode.Value, string.Empty, strUserID))
            {
                if (drORDERITEMS.Read())
                {
                    this.lblCashier.InnerText = drORDERITEMS["PostedBy"].ToString().Trim();

                }
            }

            //Order Items
            using (SqlDataReader drORDERITEMS = posBL.POS_Orders_View(this.hfControlNumber.Value, this.hfTableCode.Value, string.Empty, strUserID))
            {
                //this.hfTableCode.Value = strTableCode;

                if (drORDERITEMS.HasRows)
                {
                    //Gridview
                    this.gvOrdersReceipt.DataSource = drORDERITEMS;
                    this.gvOrdersReceipt.DataBind();
                }

                else
                {
                    //Gridview
                    this.gvOrdersReceipt.Visible = false;
                }
            }

            //Order Totals
            using (SqlDataReader drORDERTOTALS = posBL.POS_OrdersTotal_View(this.hfControlNumber.Value, strUserID))
            {
                if (drORDERTOTALS.Read())
                {
                    this.lblTotalAmountReceipt.InnerText = drORDERTOTALS["GrandTotal"].ToString();
                    this.lblPaymentReceipt.InnerText = drORDERTOTALS["PaymentAmount"].ToString();
                    this.lblChangeReceipt.InnerText = drORDERTOTALS["ChangeDue"].ToString();
                }

                else
                {
                    this.lblSubTotal.InnerText = "0.00";
                    this.lblTaxAmount.InnerText = "0.00";
                    this.lblDiscountAmount.InnerText = "0.00";
                    this.lblServiceChargeAmount.InnerText = "0.00";
                    this.lblGrandTotal.InnerText = "0.00";
                }
            }

            //Show Offcanvas
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showOrderReceipt();", true);
        }

        //Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                strUserID = Request.QueryString["userid"];
                intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
                intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);
                intCMD = Convert.ToInt32(Request.QueryString["cmd"]);
                //baseclass.UserInformation(strUserID, intUserLevel, intUserRole);

                //Error Message
                this.divErrorMessage.Visible = false;

                //Items
                this.divItems.Visible = true;

                POSTransactionDate();
                
                try
                {
                    //Item Category
                    using (SqlDataReader drITEMCATEGORY = itemcatBL.ItemCategory_View(0, string.Empty))
                    {
                        if (drITEMCATEGORY.HasRows)
                        {
                            //Datalist
                            this.dtItemCategory.DataSource = drITEMCATEGORY;
                            this.dtItemCategory.DataBind();
                            this.dtItemCategory.Visible = true;
                        }

                        else
                        {
                            //Datalist
                            this.dtItemCategory.Visible = false;
                        }
                    }

                    //Item Master
                    using (SqlDataReader drITEMMASTER = itemmastBL.ItemMaster_View("0", "1"))
                    {
                        if (drITEMMASTER.HasRows)
                        {
                            //Gridview
                            this.dtItemMaster.DataSource = drITEMMASTER;
                            this.dtItemMaster.DataBind();
                            this.dtItemMaster.Visible = true;

                        }

                        else
                        {
                            //Gridview
                            this.dtItemMaster.Visible = false;
                        }
                    }

                    //Order Items
                    this.divOrderItems.Visible = true;
                    this.lblTableName.InnerText = "Order Details";
                    DataTable dtMYORDERS = new DataTable();
                    this.gvOrderItems.DataSource = dtMYORDERS;
                    this.gvOrderItems.DataBind();
                    this.gvOrderItems.Visible = true;

                    //Discounts
                    this.divDiscounts.Visible = false;

                    //Cancel Order
                    this.divCancelOrder.Visible = false;

                    //Service Charge
                    this.divServiceCharge.Visible = false;

                    //Order Totals
                    this.lblSubTotal.InnerText = "0.00";
                    this.lblTaxAmount.InnerText = "0.00";
                    this.lblDiscountAmount.InnerText = "0.00";
                    this.lblServiceChargeAmount.InnerText = "0%";
                    this.lblGrandTotal.InnerText = "0.00";

                    //Order Buttons
                    this.lbtnDiscount.Visible = false;
                    this.lbtnQuantity.Visible = false;
                    this.lbtnServiceCharge.Visible = false;
                    this.btnSaveTransaction.Enabled = false;
                    this.btnSaveTransaction.Text = "NO ORDERS";
                    this.btnSaveTransaction.Visible = false;
                    this.btnCancelTransaction.Visible = false;

                    //Button Controls
                    this.divButtonControls.Visible = true;
                }

                catch (Exception ex)
                {
                    //Error Message
                    this.divErrorMessage.Visible = true;
                    this.lblErrorMessage.InnerText = ex.Message;
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "POS - Page Load", ex.Message, strUserID);

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //Items
                    this.divItems.Visible = false;

                    //Order Items
                    this.divOrderItems.Visible = false;
                }               
            }
        }

        //Button Controls
        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];

            try
            {
                //Item Master
                using (SqlDataReader drITEMMASTER = itemmastBL.ItemMaster_View("0", this.txtSearch.Text.Trim()))
                {
                    if (drITEMMASTER.HasRows)
                    {
                        //Datalist
                        this.dtItemMaster.DataSource = drITEMMASTER;
                        this.dtItemMaster.DataBind();
                        this.dtItemMaster.Visible = true;

                        //
                        this.lblItemMaster.InnerText = string.Empty;
                    }

                    else
                    {
                        //Datalist
                        this.dtItemMaster.Visible = false;

                        //
                        this.lblItemMaster.InnerText = "No records to display";
                    }
                }
            }

            catch (Exception ex)
            {
                //Error Message
                this.divErrorMessage.Visible = true;
                this.lblErrorMessage.InnerText = ex.Message;
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Items - Search", ex.Message, strUserID);

                //Button Controls
                this.divButtonControls.Visible = false;

                //Items
                this.divItems.Visible = false;

                //Orders
                this.divOrderItems.Visible = false;
            }
        }

        protected void btnNewSale_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            try
            {
                POSTransactionDate();

                if (this.hfTransactionDate.Value == String.Empty)
                {
                    //Redirect
                    ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('POS Cannot Start', 'Please start your session for ' + '" + System.DateTime.Now.ToString("MMMM dd, yyyy") + "' + '.', 'warning') .then((value) => { window.location.href = 'POS.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole + "'; }); ", true);
                }

                else
                {
                    this.lblTableNamesTitle.InnerText = "Tables - New Sale";
                    this.hfTransactionCode.Value = "10";

                    //Table Names
                    using (SqlDataReader drTABLENAMES = tblnamesBL.TableNames_View("1", "0"))
                    {
                        if (drTABLENAMES.HasRows)
                        {
                            //Gridview
                            this.dtTableNames.DataSource = drTABLENAMES;
                            this.dtTableNames.DataBind();
                            this.dtTableNames.Visible = true;

                            //Count
                            this.lblTableNames.InnerText = "Showing " + string.Format("{0:n0}", this.dtTableNames.Items.Count) + " available tables";
                        }

                        else
                        {
                            //Gridview
                            this.dtTableNames.Visible = false;

                            //Count
                            this.lblTableNames.InnerText = "No available tables";
                        }
                    }

                    //Order Items
                    this.divOrderItems.Visible = true;
                    this.lblTableName.InnerText = "Order Details";
                    this.gvOrderItems.DataSource = null;
                    this.gvOrderItems.DataBind();
                    this.gvOrderItems.Visible = true;

                    //Discounts
                    this.divDiscounts.Visible = false;

                    //Cancel Order
                    this.divCancelOrder.Visible = false;

                    //Service Charge
                    this.divServiceCharge.Visible = false;

                    //Order Buttons
                    this.lbtnDiscount.Visible = false;
                    this.lbtnQuantity.Visible = false;
                    this.lbtnServiceCharge.Visible = false;
                    this.btnSaveTransaction.Enabled = false;
                    this.btnSaveTransaction.Text = "No Orders";
                    this.btnSaveTransaction.Visible = true;
                    this.btnCancelTransaction.Visible = false;

                    //Show Offcanvas
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showTableNames();", true);
                }

                ////Register Shift
                //System.Data.DataSet dsREGISTERSHIFT = new System.Data.DataSet();
                //dsREGISTERSHIFT = DBHelper.GetData("SELECT TOP 1 ControlNumber, TransactionDate, TransactionType, OpeningAmount, ClosingAmount, TransactionStatus FROM RegisterShift WHERE TransactionStatus = '1'");

                //if (dsREGISTERSHIFT.Tables[0].Rows.Count == 0)
                //{
                //    //Redirect
                //    ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('POS Cannot Start', 'No opening amount found for ' + '" + System.DateTime.Now.ToString("MMMM dd, yyyy") + "' + '.', 'warning') .then((value) => { window.location.href = 'POS.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole + "'; }); ", true);
                //}

                //else
                //{
                //    this.lblTableNamesTitle.InnerText = "Tables - New Sale";
                //    this.hfTransactionCode.Value = "10";

                //    //Table Names
                //    using (SqlDataReader drTABLENAMES = tblnamesBL.TableNames_View("1", "0"))
                //    {
                //        if (drTABLENAMES.HasRows)
                //        {
                //            //Gridview
                //            this.dtTableNames.DataSource = drTABLENAMES;
                //            this.dtTableNames.DataBind();
                //            this.dtTableNames.Visible = true;

                //            //Count
                //            this.lblTableNames.InnerText = "Showing " + string.Format("{0:n0}", this.dtTableNames.Items.Count) + " available tables";
                //        }

                //        else
                //        {
                //            //Gridview
                //            this.dtTableNames.Visible = false;

                //            //Count
                //            this.lblTableNames.InnerText = "No available tables";
                //        }
                //    }

                //    //Order Items
                //    this.divOrderItems.Visible = true;
                //    this.lblTableName.InnerText = "Order Details";
                //    this.gvOrderItems.DataSource = null;
                //    this.gvOrderItems.DataBind();
                //    this.gvOrderItems.Visible = true;

                //    //Discounts
                //    this.divDiscounts.Visible = false;

                //    //Cancel Order
                //    this.divCancelOrder.Visible = false;

                //    //Order Buttons
                //    this.lbtnDiscount.Visible = false;
                //    this.lbtnQuantity.Visible = false;
                //    this.btnSaveTransaction.Enabled = false;
                //    this.btnSaveTransaction.Text = "No Orders";
                //    this.btnCancelTransaction.Visible = false;

                //    //Show Offcanvas
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showTableNames();", true);
                //}

                //dsREGISTERSHIFT.Clear();
            }

            catch (Exception ex)
            {
                //Error Message
                this.divErrorMessage.Visible = true;
                this.lblErrorMessage.InnerText = ex.Message;
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Orders - New Sale", ex.Message, strUserID);

                //Button Controls
                this.divButtonControls.Visible = false;

                //Items
                this.divItems.Visible = false;

                //Orders
                this.divOrderItems.Visible = false;
            }
        }

        protected void btnBillOut_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            try
            {
                this.lblTableNamesTitle.InnerText = "Tables - Bill Out";
                this.hfTransactionCode.Value = "30";

                //Table Names
                using (SqlDataReader drTABLENAMES = tblnamesBL.TableNames_View("2", string.Empty))
                {
                    if (drTABLENAMES.HasRows)
                    {
                        //Gridview
                        this.dtTableNames.DataSource = drTABLENAMES;
                        this.dtTableNames.DataBind();
                        this.dtTableNames.Visible = true;

                        //Count
                        this.lblTableNames.InnerText = "Showing " + string.Format("{0:n0}", this.dtTableNames.Items.Count) + " occupied tables";
                    }

                    else
                    {
                        //Gridview
                        this.dtTableNames.Visible = false;

                        //Count
                        this.lblTableNames.InnerText = "No occupied tables";
                    }
                }

                //Order Items
                this.divOrderItems.Visible = true;
                this.lblTableName.InnerText = "Order Details";
                this.gvOrderItems.DataSource = null;
                this.gvOrderItems.DataBind();
                this.gvOrderItems.Visible = true;

                //Discounts
                this.divDiscounts.Visible = false;

                //Cancel Order
                this.divCancelOrder.Visible = false;

                //Service Charge
                this.divServiceCharge.Visible = false;

                //Order Buttons
                this.lbtnDiscount.Visible = false;
                this.lbtnQuantity.Visible = false;
                this.lbtnServiceCharge.Visible = false;
                this.btnSaveTransaction.Enabled = false;
                this.btnSaveTransaction.Visible = false;
                this.btnCancelTransaction.Visible = false;

                //Show Offcanvas
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showTableNames();", true);
            }

            catch (Exception ex)
            {
                //Error Message
                this.divErrorMessage.Visible = true;
                this.lblErrorMessage.InnerText = ex.Message;
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Orders - Bill Out", ex.Message, strUserID);

                //Button Controls
                this.divButtonControls.Visible = false;

                //Items
                this.divItems.Visible = false;

                //Orders
                this.divOrderItems.Visible = false;
            }
        }

        protected void btnVoid_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            try
            {
                this.lblTableNamesTitle.InnerText = "Tables - Void Order";
                this.hfTransactionCode.Value = "40";

                //Table Names
                using (SqlDataReader drTABLENAMES = tblnamesBL.TableNames_View("2", string.Empty))
                {
                    if (drTABLENAMES.HasRows)
                    {
                        //Gridview
                        this.dtTableNames.DataSource = drTABLENAMES;
                        this.dtTableNames.DataBind();
                        this.dtTableNames.Visible = true;

                        //Count
                        this.lblTableNames.InnerText = "Showing " + string.Format("{0:n0}", this.dtTableNames.Items.Count) + " tables to void";
                    }

                    else
                    {
                        //Gridview
                        this.dtTableNames.Visible = false;

                        //Count
                        this.lblTableNames.InnerText = "No tables to display";
                    }
                }

                //Order Items
                this.divOrderItems.Visible = true;
                this.lblTableName.InnerText = "Order Details";
                this.gvOrderItems.DataSource = null;
                this.gvOrderItems.DataBind();
                this.gvOrderItems.Visible = true;

                //Discounts
                this.divDiscounts.Visible = false;

                //Cancel Order
                this.divCancelOrder.Visible = false;

                //Service Charge
                this.divServiceCharge.Visible = false;

                //Order Buttons
                this.lbtnDiscount.Visible = false;
                this.lbtnQuantity.Visible = false;
                this.lbtnServiceCharge.Visible = false;
                this.btnSaveTransaction.Enabled = false;
                this.btnSaveTransaction.Visible = false;
                this.btnCancelTransaction.Visible = false;

                //Show Offcanvas
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showTableNames();", true);
            }

            catch (Exception ex)
            {
                //Error Message
                this.divErrorMessage.Visible = true;
                this.lblErrorMessage.InnerText = ex.Message;
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Orders - Void", ex.Message, strUserID);

                //Button Controls
                this.divButtonControls.Visible = false;

                //Items
                this.divItems.Visible = false;

                //Orders
                this.divOrderItems.Visible = false;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            try
            {
                this.lblTableNamesTitle.InnerText = "Tables - Cancel Order";
                this.hfTransactionCode.Value = "50";

                //Table Names
                using (SqlDataReader drTABLENAMES = tblnamesBL.TableNames_View("2", string.Empty))
                {
                    if (drTABLENAMES.HasRows)
                    {
                        //Gridview
                        this.dtTableNames.DataSource = drTABLENAMES;
                        this.dtTableNames.DataBind();
                        this.dtTableNames.Visible = true;

                        //Count
                        this.lblTableNames.InnerText = "Showing " + string.Format("{0:n0}", this.dtTableNames.Items.Count) + " occupied tables";
                    }

                    else
                    {
                        //Gridview
                        this.dtTableNames.Visible = false;

                        //Count
                        this.lblTableNames.InnerText = "No tables to display";
                    }
                }

                //Order Items
                this.divOrderItems.Visible = true;
                this.lblTableName.InnerText = "Order Details";
                this.gvOrderItems.DataSource = null;
                this.gvOrderItems.DataBind();
                this.gvOrderItems.Visible = true;

                //Discounts
                this.divDiscounts.Visible = false;

                //Cancel Order
                this.divCancelOrder.Visible = false;

                //Service Charge
                this.divServiceCharge.Visible = false;

                //Order Buttons
                this.lbtnDiscount.Visible = false;
                this.lbtnQuantity.Visible = false;
                this.lbtnServiceCharge.Visible = false;
                this.btnSaveTransaction.Enabled = false;
                this.btnSaveTransaction.Visible = false;
                this.btnCancelTransaction.Visible = false;

                //Show Offcanvas
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showTableNames();", true);
            }

            catch (Exception ex)
            {
                //Error Message
                this.divErrorMessage.Visible = true;
                this.lblErrorMessage.InnerText = ex.Message;
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Orders - Cancel", ex.Message, strUserID);

                //Button Controls
                this.divButtonControls.Visible = false;

                //Items
                this.divItems.Visible = false;

                //Orders
                this.divOrderItems.Visible = false;
            }
        }

        protected void lbtnXReport_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            try
            {
                //POS Sessions
                System.Data.DataSet dsPOSSESSIONS = new System.Data.DataSet();
                dsPOSSESSIONS = DBHelper.GetData("SELECT TOP 1 SessionCode, SessionStatus, TransactionDate FROM POSSessions WHERE SessionStatus = '1'");

                if (dsPOSSESSIONS.Tables[0].Rows.Count > 0)
                {
                    dteTransactionDate = Convert.ToDateTime(dsPOSSESSIONS.Tables[0].Rows[0]["TransactionDate"]);
                }

                else
                {
                    dteTransactionDate = System.DateTime.Now;
                }

                //Get IP Address of the machine
                string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                //this.hfIPAddress.Value = ipAddress;

                if (string.IsNullOrEmpty(ipAddress))
                {
                    ipAddress = Request.ServerVariables["REMOTE_ADDR"];
                }

                //Get Computer Name
                var strTerminalName = ipAddress;

                this.lblXZReportTitle.InnerText = "X Report";
                this.lblXZReportTransactionDate.InnerText = dteTransactionDate.ToString("MMMM dd, yyyy");
                
                //XZReport
                using (SqlDataReader drXZREPORT = posBL.POS_XZReport_View(1, Convert.ToDateTime(dteTransactionDate.ToString("yyyy-MM-dd")), strUserID))
                {
                    if (drXZREPORT.Read())
                    {
                        this.lblPOSTerminal.InnerText = drXZREPORT["TerminalName"].ToString();
                        this.lblPOSUser.InnerText = drXZREPORT["POSUser"].ToString();

                        this.lblTotalOrders.InnerText = drXZREPORT["TotalOrders"].ToString();
                        this.lblTotalNetSales.InnerText = drXZREPORT["TotalNetSales"].ToString();
                        this.lblOpeningAmount.InnerText = drXZREPORT["OpeningAmount"].ToString();

                        //Expected Drawer
                        this.divExpectedDrawer.Visible = true;
                        this.divExpectedDrawerAmount.Visible = true;
                        this.lblExpectedDrawer.InnerText = drXZREPORT["ExpectedDrawer"].ToString();

                        //Closing Amount
                        this.divClosing.Visible = false;
                        this.divClosingAmount.Visible = false;

                        //OverShort
                        this.divOverShort.Visible = false;
                        this.divOverShortAmount.Visible = false;

                        //Order Status
                        this.lblNewOrders.InnerText = drXZREPORT["NewOrders"].ToString();
                        this.lblDelivered.InnerText = drXZREPORT["Delivered"].ToString();
                        this.lblPaid.InnerText = drXZREPORT["Paid"].ToString();
                        this.lblVoided.InnerText = drXZREPORT["Voided"].ToString();
                        this.lblCancelled.InnerText = drXZREPORT["Cancelled"].ToString();

                        //Payment Types
                        this.lblCashSales.InnerText = drXZREPORT["CashSales"].ToString();
                        this.lblCreditCard.InnerText = drXZREPORT["CreditCardSales"].ToString();
                        this.lblGCash.InnerText = drXZREPORT["GCashSales"].ToString();
                    }

                    else
                    {
                        this.lblPOSTerminal.InnerText = "N/A";
                        this.lblPOSUser.InnerText = "N/A";

                        this.lblTotalOrders.InnerText = "0";
                        this.lblTotalNetSales.InnerText = "0.00";
                        this.lblOpeningAmount.InnerText = "0.00";
                        this.lblClosingAmount.InnerText = "0.00";

                        //Expected Drawer
                        this.divExpectedDrawer.Visible = false;
                        this.divExpectedDrawerAmount.Visible = false;

                        //Closing Amount
                        this.divClosing.Visible = false;
                        this.divClosingAmount.Visible = false;

                        //OverShort
                        this.divOverShort.Visible = false;
                        this.divOverShortAmount.Visible = false;

                        //Order Status
                        this.lblNewOrders.InnerText = "0.00";
                        this.lblDelivered.InnerText = "0.00";
                        this.lblPaid.InnerText = "0.00";
                        this.lblVoided.InnerText = "0.00";
                        this.lblCancelled.InnerText = "0.00";

                        //Payment Types
                        this.lblCashSales.InnerText = "0.00";
                        this.lblCreditCard.InnerText = "0.00";
                        this.lblGCash.InnerText = "0.00";
                    }
                }

               // this.btnPrintXZReport.Text = "Print X Report";

                //Show Offcanvas
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showXZReport();", true);
            }

            catch (Exception ex)
            {
                //Error Message
                this.divErrorMessage.Visible = true;
                this.lblErrorMessage.InnerText = ex.Message;
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "X Report - View", ex.Message, strUserID);

                //Button Controls
                this.divButtonControls.Visible = false;

                //Items
                this.divItems.Visible = false;

                //Order Items
                this.divOrderItems.Visible = false;
            }
        }

        protected void lbtnZReport_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            try
            {
                //POS Sessions
                System.Data.DataSet dsPOSSESSIONS = new System.Data.DataSet();
                dsPOSSESSIONS = DBHelper.GetData("SELECT TOP 1 SessionCode, SessionStatus, TransactionDate FROM POSSessions WHERE SessionStatus = '1'");

                if (dsPOSSESSIONS.Tables[0].Rows.Count > 0)
                {
                    dteTransactionDate = Convert.ToDateTime(dsPOSSESSIONS.Tables[0].Rows[0]["TransactionDate"]);
                }

                else
                {
                    dteTransactionDate = System.DateTime.Now;
                }

                ////Get IP Address of the machine
                //string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                ////this.hfIPAddress.Value = ipAddress;

                //if (string.IsNullOrEmpty(ipAddress))
                //{
                //    ipAddress = Request.ServerVariables["REMOTE_ADDR"];
                //}

                ////Get Computer Name
                //var strTerminalName = ipAddress;

                this.lblXZReportTitle.InnerText = "Z Report";
                this.lblXZReportTransactionDate.InnerText = dteTransactionDate.ToString("MMMM dd, yyyy");

                //XZReport
                using (SqlDataReader drXZREPORT = posBL.POS_XZReport_View(2, Convert.ToDateTime(dteTransactionDate.ToString("yyyy-MM-dd")), strUserID))
                {
                    if (drXZREPORT.Read())
                    {
                        this.lblPOSTerminal.InnerText = drXZREPORT["TerminalName"].ToString();
                        this.lblPOSUser.InnerText = drXZREPORT["POSUser"].ToString();

                        this.lblTotalOrders.InnerText = drXZREPORT["TotalOrders"].ToString();
                        this.lblTotalNetSales.InnerText = drXZREPORT["TotalNetSales"].ToString();
                        this.lblOpeningAmount.InnerText = drXZREPORT["OpeningAmount"].ToString();

                        //Expected Drawer
                        this.divExpectedDrawer.Visible = false;
                        this.divExpectedDrawerAmount.Visible = false;

                        //Closing Amount
                        this.divClosing.Visible = true;
                        this.divClosingAmount.Visible = true;
                        this.lblClosingAmount.InnerText = drXZREPORT["ClosingAmount"].ToString();

                        //OverShort
                        this.divOverShort.Visible = false;
                        this.lblOverShort.InnerText = drXZREPORT["OverShort"].ToString();
                        this.divOverShortAmount.Visible = false;
                        this.lblOverShortAmount.InnerText = drXZREPORT["OverShortAmount"].ToString();
                        
                        //Order Status
                        this.lblNewOrders.InnerText = drXZREPORT["NewOrders"].ToString();
                        this.lblDelivered.InnerText = drXZREPORT["Delivered"].ToString();
                        this.lblPaid.InnerText = drXZREPORT["Paid"].ToString();
                        this.lblVoided.InnerText = drXZREPORT["Voided"].ToString();
                        this.lblCancelled.InnerText = drXZREPORT["Cancelled"].ToString();

                        //Payment Types
                        this.lblCashSales.InnerText = drXZREPORT["CashSales"].ToString();
                        this.lblCreditCard.InnerText = drXZREPORT["CreditCardSales"].ToString();
                        this.lblGCash.InnerText = drXZREPORT["GCashSales"].ToString();
                    }

                    else
                    {
                        this.lblPOSTerminal.InnerText = "N/A";
                        this.lblPOSUser.InnerText = "N/A";

                        this.lblTotalOrders.InnerText = "0";
                        this.lblTotalNetSales.InnerText = "0.00";
                        this.lblOpeningAmount.InnerText = "0.00";
                        this.lblClosingAmount.InnerText = "0.00";

                        //Expected Drawer
                        this.divExpectedDrawer.Visible = false;
                        this.divExpectedDrawerAmount.Visible = false;

                        //Closing Amount
                        this.divClosing.Visible = true;
                        this.divClosingAmount.Visible = true;

                        //OverShort
                        this.divOverShort.Visible = false;
                        this.divOverShortAmount.Visible = false;

                        //Order Status
                        this.lblNewOrders.InnerText = "0.00";
                        this.lblDelivered.InnerText = "0.00";
                        this.lblPaid.InnerText = "0.00";
                        this.lblVoided.InnerText = "0.00";
                        this.lblCancelled.InnerText = "0.00";

                        //Payment Types
                        this.lblCashSales.InnerText = "0.00";
                        this.lblCreditCard.InnerText = "0.00";
                        this.lblGCash.InnerText = "0.00";
                    }
                }

                //this.btnPrintXZReport.Text = "Print Z Report";

                //Show Modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showXZReport();", true);
            }

            catch (Exception ex)
            {
                //Error Message
                this.divErrorMessage.Visible = true;
                this.lblErrorMessage.InnerText = ex.Message;
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Z Report - View", ex.Message, strUserID);

                //Button Controls
                this.divButtonControls.Visible = false;

                //Items
                this.divItems.Visible = false;

                //Order Items
                this.divOrderItems.Visible = false;
            }
        }

        //Table Names
        protected void dtTableNames_ItemCommand(object source, DataListCommandEventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            try
            {
                //Get values from command argument using .split
                string[] val = new string[2];
                val = e.CommandArgument.ToString().Split('|');
                string strFloorLocation = val[0].ToString();
                string strFloorName = val[1].ToString();
                string strTableCode = val[2].ToString();
                this.hfTableCode.Value = val[2].ToString();
                string strTableName = val[3].ToString();
                string strTableStatus = val[4].ToString();
                string strControlNumber = val[5].ToString();
                this.hfControlNumber.Value = val[5].ToString();

                OrderTotals();

                if (this.hfTransactionCode.Value == "10") //New Sale
                {
                    //Search
                    this.txtSearch.Enabled = true;

                    //Item Category
                    this.dtItemCategory.Enabled = true;
                    this.dtItemCategory.Attributes["style"] = "opacity:100%;";

                    //Items
                    this.dtItemMaster.Enabled = true;
                    this.dtItemMaster.Attributes["style"] = "opacity:100%;";

                    //Discounts
                    this.divDiscounts.Visible = false;

                    //Cancel Order
                    this.divCancelOrder.Visible = false;

                    //Service Charge
                    this.divServiceCharge.Visible = false;

                    //Order Items
                    this.lblTableName.InnerText = strTableName + " - " + strFloorName;
                    this.hfTableCode.Value = strTableCode;

                    this.gvOrderItems.DataSource = null;
                    this.gvOrderItems.DataBind();
                    this.gvOrderItems.Visible = false;

                    //Button Controls
                    this.lbtnDiscount.Visible = false;
                    this.lbtnQuantity.Visible = false;
                    this.lbtnServiceCharge.Visible = false;
                    this.btnSaveTransaction.Enabled = true;
                    this.btnSaveTransaction.Text = "PLACE ORDERS";
                    this.btnSaveTransaction.Visible = true;
                    this.btnCancelTransaction.Visible = true;
                }

                else if (this.hfTransactionCode.Value == "30") //Bill Out
                {
                    this.lblTableName.InnerText = strTableName + " - " + strFloorName;

                    //Search
                    this.txtSearch.Enabled = false;

                    //Item Category
                    this.dtItemCategory.Enabled = false;
                    this.dtItemCategory.Attributes["style"] = "opacity:50%;";

                    //Items
                    this.dtItemMaster.Enabled = false;
                    this.dtItemMaster.Attributes["style"] = "opacity:50%;";

                    //Order Items
                    using (SqlDataReader drORDERITEMS = posBL.POS_Orders_View(strControlNumber, strTableCode, string.Empty, strUserID))
                    {
                        this.hfTableCode.Value = strTableCode;

                        if (drORDERITEMS.HasRows)
                        {
                            //Gridview
                            this.gvOrderItems.DataSource = drORDERITEMS;
                            this.gvOrderItems.DataBind();
                            this.gvOrderItems.Columns[0].Visible = false; //CheckBox
                            this.gvOrderItems.Columns[1].Visible = false; //Item Code
                            this.gvOrderItems.Columns[4].Visible = false; //Order Status
                            this.gvOrderItems.Columns[5].Visible = false; //Delete Button
                        }

                        else
                        {
                            //Gridview
                            this.gvOrderItems.Visible = false;
                        }
                    }

                    //Discounts
                    this.divDiscounts.Visible = false;

                    //Cancel Order
                    this.divCancelOrder.Visible = false;

                    //Service Charge
                    this.divServiceCharge.Visible = false;

                    //Button Controls
                    this.lbtnDiscount.Visible = true;
                    this.lbtnQuantity.Visible = true;
                    this.lbtnServiceCharge.Visible = true;
                    this.btnSaveTransaction.Enabled = true;
                    this.btnSaveTransaction.Text = "ADD PAYMENT";
                    this.btnSaveTransaction.Visible = true;
                    this.btnCancelTransaction.Visible = true;

                }

                else if (this.hfTransactionCode.Value == "40") //Void Order
                {
                    int intOrderCount = 0;

                    //Check order count, if only 1 item do not allow void
                    using (SqlDataReader drORDERCOUNT = posBL.POS_Orders_View(strControlNumber, strTableCode, string.Empty, strUserID))
                    {
                        if (drORDERCOUNT.Read())
                        {
                            intOrderCount = Convert.ToInt32(drORDERCOUNT["RecordCount"]);
                        }
                    }

                    if (intOrderCount == 1)
                    {
                        //Redirect
                        ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Void Not Allowed', 'Orders with only 1 item cannot be voided. Please cancel it instead.', 'info') .then((value) => { window.location.href = 'POS.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole + "'; }); ", true);
                    }

                    else
                    {
                        this.lblTableName.InnerText = strTableName + " - " + strFloorName;

                        //Search
                        this.txtSearch.Enabled = false;

                        //Item Category
                        this.dtItemCategory.Enabled = false;
                        this.dtItemCategory.Attributes["style"] = "opacity:50%;";

                        //Items
                        this.dtItemMaster.Enabled = false;
                        this.dtItemMaster.Attributes["style"] = "opacity:50%;";

                        //Order Items
                        using (SqlDataReader drORDERITEMS = posBL.POS_Orders_View(strControlNumber, strTableCode, string.Empty, strUserID))
                        {
                            this.hfTableCode.Value = strTableCode;

                            if (drORDERITEMS.HasRows)
                            {
                                //Gridview
                                this.gvOrderItems.DataSource = drORDERITEMS;
                                this.gvOrderItems.DataBind();
                                this.gvOrderItems.Columns[0].Visible = true; //CheckBox
                                this.gvOrderItems.Columns[1].Visible = false; //Item Code
                                this.gvOrderItems.Columns[4].Visible = false; //Order Status
                                this.gvOrderItems.Columns[5].Visible = false; //Delete Button
                            }

                            else
                            {
                                //Gridview
                                this.gvOrderItems.Visible = false;
                            }
                        }

                        //Discounts
                        this.divDiscounts.Visible = false;

                        //Cancel Order
                        this.divCancelOrder.Visible = false;

                        //Service Charge
                        this.divServiceCharge.Visible = false;

                        //Button Controls
                        this.lbtnDiscount.Visible = false;
                        this.lbtnQuantity.Visible = false;
                        this.lbtnServiceCharge.Visible = false;
                        this.btnSaveTransaction.Enabled = true;
                        this.btnSaveTransaction.Text = "VOID ORDER";
                        this.btnSaveTransaction.Visible = true;
                        this.btnCancelTransaction.Visible = true;
                    }
                }

                else if (this.hfTransactionCode.Value == "50") //Cancel Order
                {
                    this.lblTableName.InnerText = strTableName + " - " + strFloorName;

                    //Search
                    this.txtSearch.Enabled = false;

                    //Item Category
                    this.dtItemCategory.Enabled = false;
                    this.dtItemCategory.Attributes["style"] = "opacity:50%;";

                    //Items
                    this.dtItemMaster.Enabled = false;
                    this.dtItemMaster.Attributes["style"] = "opacity:50%;";

                    //Order Items
                    using (SqlDataReader drORDERITEMS = posBL.POS_Orders_View(strControlNumber, strTableCode, string.Empty, strUserID))
                    {
                        this.hfTableCode.Value = strTableCode;

                        if (drORDERITEMS.HasRows)
                        {
                            //Gridview
                            this.gvOrderItems.DataSource = drORDERITEMS;
                            this.gvOrderItems.DataBind();
                            this.gvOrderItems.Columns[0].Visible = false; //CheckBox
                            this.gvOrderItems.Columns[1].Visible = false; //Item Code
                            this.gvOrderItems.Columns[4].Visible = false; //Order Status
                            this.gvOrderItems.Columns[5].Visible = false; //Delete Button
                        }

                        else
                        {
                            //Gridview
                            this.gvOrderItems.Visible = false;
                        }
                    }

                    //Discounts
                    this.divDiscounts.Visible = false;

                    //Cancel Order
                    this.divCancelOrder.Visible = true;
                    CancellationTypes();
                    this.ddlCancellationTypes.Focus();

                    //Service Charge
                    this.divServiceCharge.Visible = false;

                    //Button Controls
                    this.lbtnDiscount.Visible = false;
                    this.lbtnQuantity.Visible = false;
                    this.lbtnServiceCharge.Visible = false;
                    this.btnSaveTransaction.Enabled = true;
                    this.btnSaveTransaction.Text = "CANCEL ORDER";
                    this.btnSaveTransaction.Visible = true;
                    this.btnCancelTransaction.Visible = true;
                }
            }

            catch (Exception ex)
            {
                //Error Message
                this.divErrorMessage.Visible = true;
                this.lblErrorMessage.InnerText = ex.Message;
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Table Names - Select", ex.Message, strUserID);

                //Button Controls
                this.divButtonControls.Visible = false;

                //Items
                this.divItems.Visible = false;

                //Orders
                this.divOrderItems.Visible = false;
            }
        }

        //Item Category
        protected void dtItemCategory_ItemCommand(object source, DataListCommandEventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            try
            { 
                //Get values from command argument using .split
                string[] val = new string[2];
                val = e.CommandArgument.ToString().Split('|');
                string strCategoryCode = val[0].ToString();
                string strCategoryName = val[1].ToString();

                //Item Master
                using (SqlDataReader drITEMMASTER = itemmastBL.ItemMaster_View("1", strCategoryCode))
                {
                    if (drITEMMASTER.HasRows)
                    {
                        //Gridview
                        this.dtItemMaster.DataSource = drITEMMASTER;
                        this.dtItemMaster.DataBind();
                        this.dtItemMaster.Visible = true;
                    }

                    else
                    {
                        //Gridview
                        this.dtItemMaster.Visible = false;
                    }
                }
            }

            catch (Exception ex)
            {
                //Error Message
                this.divErrorMessage.Visible = true;
                this.lblErrorMessage.InnerText = ex.Message;
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Item Category - Select", ex.Message, strUserID);

                //Button Controls
                this.divButtonControls.Visible = false;

                //Items
                this.divItems.Visible = false;

                //Order Items
                this.divOrderItems.Visible = false;
            }
        }

        //Item Master
        protected void dtItemMaster_ItemCommand(object source, DataListCommandEventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            try
            {
                //Get values from command argument using .split
                string[] val = e.CommandArgument.ToString().Split('|');
                string strItemCode = val[0].ToString();
                string strItemName = val[1].ToString();
                string strItemCategoryCode = val[2].ToString();
                decimal decItemPrice = Convert.ToDecimal(val[3]);

                if (this.lblTableName.InnerText == "Order Details")
                {
                    //Alert
                    ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Table Required', 'Please select first a table before your order', 'warning'); ", true);
                }

                else
                {
                    posBO.tablecode = this.hfTableCode.Value;
                    posBO.categorycode = strItemCategoryCode;
                    posBO.itemcode = strItemCode;
                    posBO.itemprice = decItemPrice;
                    posBO.itemquantity = 1;

                    this.hfControlNumber.Value =  "0";

                    //Post
                    string strPost = posBL.POS_Orders_Select(posBO, strUserID);

                    //Order Items
                    using (SqlDataReader drORDERITEMS = posBL.POS_Orders_View("0", this.hfTableCode.Value, string.Empty, strUserID))
                    {
                        if (drORDERITEMS.HasRows)
                        {
                            //Gridview
                            this.gvOrderItems.DataSource = drORDERITEMS;
                            this.gvOrderItems.DataBind();
                            this.gvOrderItems.Visible = true;
                            this.gvOrderItems.Columns[0].Visible = false; //CheckBox
                            this.gvOrderItems.Columns[1].Visible = false; //Item Code
                            this.gvOrderItems.Columns[4].Visible = false; //Order Status
                            this.gvOrderItems.Columns[5].Visible = true; //Delete Button
                            OrderTotals();

                            //Button Controls
                            this.btnSaveTransaction.Enabled = true;
                        }

                        else
                        {
                            //Gridview
                            this.gvOrderItems.Visible = false;

                            //Button Controls
                            this.btnSaveTransaction.Enabled = false;
                        }
                    }

                    //Discounts
                    this.divDiscounts.Visible = false;

                    //Cancel Order
                    this.divCancelOrder.Visible = false;

                    //Service Charge
                    this.divServiceCharge.Visible = false;

                    //Button Controls
                    this.lbtnDiscount.Visible = false;
                    this.lbtnQuantity.Visible = false;
                    this.lbtnServiceCharge.Visible = false;
                    this.btnSaveTransaction.Enabled = true;
                    this.btnSaveTransaction.Text = "PLACE ORDERS";
                    this.btnSaveTransaction.Visible = true;
                    this.btnCancelTransaction.Visible = true;
                }
            }

            catch (Exception ex)
            {
                //Error Message
                this.divErrorMessage.Visible = true;
                this.lblErrorMessage.InnerText = ex.Message;
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Item Master - Select", ex.Message, strUserID);

                //Button Controls
                this.divButtonControls.Visible = false;

                //Items
                this.divItems.Visible = false;

                //Orders
                this.divOrderItems.Visible = false;
            }
        }

        //Order Items
        protected void gvOrderItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            try
            {
                //Get values from command argument using .split
                string[] val = e.CommandArgument.ToString().Split('|');
                int intRecordID = Convert.ToInt32(val[0]);
                string strItemName = val[1].ToString();
                string strAction = val[2].ToString();

                //Remove Button
                if (strAction == "Remove")
                {
                    //Delete record from database
                    System.Data.DataSet dsDELETE = new System.Data.DataSet();
                    dsDELETE = DBHelper.GetData("DELETE FROM OrderLines WHERE RecordID = '" + intRecordID + "'");
                    dsDELETE.Clear();

                    //System Logs
                    strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Orders - Remove Item", strItemName + " has been removed.", strUserID);

                    //Display the list again without the deleted record
                    //Order Items
                    using (SqlDataReader drORDERITEMS = posBL.POS_Orders_View("0", this.hfTableCode.Value, string.Empty, strUserID))
                    {
                        if (drORDERITEMS.HasRows)
                        {
                            //Gridview
                            this.gvOrderItems.DataSource = drORDERITEMS;
                            this.gvOrderItems.DataBind();
                            this.gvOrderItems.Visible = true;
                            this.gvOrderItems.Columns[0].Visible = false; //CheckBox
                            this.gvOrderItems.Columns[1].Visible = false; //Item Code
                            this.gvOrderItems.Columns[4].Visible = false; //Order Status
                            this.gvOrderItems.Columns[5].Visible = true; //Delete Button
                            OrderTotals();

                            //Button Controls
                            this.btnSaveTransaction.Enabled = true;
                        }

                        else
                        {
                            //Gridview
                            this.gvOrderItems.Visible = false;

                            //Button Controls
                            this.btnSaveTransaction.Enabled = false;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                //Error Message
                this.divErrorMessage.Visible = true;
                this.lblErrorMessage.InnerText = ex.Message;
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Orders - Remove Item", ex.Message, strUserID);

                //Button Controls
                this.divButtonControls.Visible = false;

                //Items
                this.divItems.Visible = false;

                //Orders
                this.divOrderItems.Visible = false;
            }
        }

        protected void gvOrderItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HtmlInputCheckBox cbOrderItems = (HtmlInputCheckBox)e.Row.FindControl("cbOrderItems");
                Label lblItemName = (Label)e.Row.FindControl("lblItemName");
                Label lblItemAmount = (Label)e.Row.FindControl("lblItemAmount");
                Label lblOrderStatus = (Label)e.Row.FindControl("lblOrderStatus");

                if (lblOrderStatus.Text == "4") //Voided
                {
                    cbOrderItems.Visible = false;
                    lblItemName.Attributes["style"] = "text-decoration: line-through; color:Grey;";
                    lblItemAmount.Attributes["style"] = "text-decoration: line-through; color:Grey;";
                }
            }
        }

        //Discounts
        protected void lbtnDiscount_Click(object sender, EventArgs e)
        {            
            //Discounts
            this.divDiscounts.Visible = true;
            this.divDiscountAmount.Visible = false;
            this.ddlDiscountType.ClearSelection();
            DiscountType();
            this.ddlDiscountType.Focus();
            this.txtDiscountAmount.Value = String.Empty;
            this.txtDiscountAmount.Disabled = true;
            this.hfTransactionCode.Value = "60";

            //Button Control
            this.lbtnDiscount.Visible = false;
            this.lbtnQuantity.Visible = false;
            this.lbtnServiceCharge.Visible = false;
            this.btnSaveTransaction.Text = "SAVE DISCOUNT";
        }

        protected void ddlDiscountType_TextChanged(object sender, EventArgs e)
        {
            if (this.ddlDiscountType.SelectedItem.Text == "Select Discount")
            {
                //Discount Amount
                this.divDiscountAmount.Visible = false;
                this.txtDiscountAmount.Value = String.Empty;
                this.ddlDiscountType.Focus();
            }

            else
            {
                //Discount Type
                using (SqlDataReader drDISCOUNTTYPE = discBL.Discounts_View(Convert.ToInt32(this.ddlDiscountType.SelectedValue), string.Empty))
                {
                    if (drDISCOUNTTYPE.Read())
                    {
                        this.txtDiscountAmount.Value = drDISCOUNTTYPE["DiscountAmount"].ToString();

                        if (this.ddlDiscountType.SelectedValue == "1" || this.ddlDiscountType.SelectedValue == "2") //Senior //PWD
                        {
                            //Order Items
                            this.gvOrderItems.Columns[0].Visible = true; //Check Box
                            this.gvOrderItems.Focus();

                            //Discount Amount
                            this.divDiscountAmount.Visible = true;
                            this.txtDiscountAmount.Disabled = true;
                        }

                        else //Employee //Others
                        {
                            //Order Items
                            this.gvOrderItems.Columns[0].Visible = false; //Check Box

                            //Discount Amount
                            this.divDiscountAmount.Visible = true;
                            this.txtDiscountAmount.Disabled = false;
                            this.txtDiscountAmount.Focus();
                        }
                    }
                }
            }
        }

        //Quantity
        protected void lbtnQuantity_Click(object sender, EventArgs e)
        {
            this.lblQuantityTitle.InnerText = "Quantity (" + this.lblTableName.InnerText + ")";
            this.ddlOrderItemsQuantity.ClearSelection();
            OrderItemsQuantity();
            this.ddlOrderItemsQuantity.Focus();
            this.txtQuantity.Value = "0";
            this.txtQuantity.Disabled = true;
            this.lblValidationQuantity.InnerText = String.Empty;

            //Show Offcanvas
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showQuantity();", true);
        }

        protected void ddlOrderItemsQuantity_TextChanged(object sender, EventArgs e)
        {
            //Order Items
            using (SqlDataReader drORDERITEMS = posBL.POS_Orders_View("1", this.hfTableCode.Value, this.ddlOrderItemsQuantity.SelectedValue, String.Empty))
            {
                if (drORDERITEMS.Read())
                {
                    this.txtQuantity.Value = drORDERITEMS["ItemQuantity"].ToString();
                }
            }

            this.txtQuantity.Disabled = false;
            this.txtQuantity.Focus();

            //Show Offcanvas
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showQuantity();", true);
        }

        protected void btnSaveQuantity_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            if (this.ddlOrderItemsQuantity.SelectedItem.Text == "Select Item")
            {
                this.lblValidationQuantity.InnerText = "Order items must not be empty.";
                this.lblValidationQuantity.Attributes["style"] = "color:Red;";

                //Show Offcanvas
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showQuantity();", true);
            }

            else if (this.txtQuantity.Value == string.Empty)
            {
                this.lblValidationQuantity.InnerText = "Quantity must not be empty.";
                this.lblValidationQuantity.Attributes["style"] = "color:Red;";

                //Show Offcanvas
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showQuantity();", true);
            }

            else if (this.txtQuantity.Value == "0.00" || this.txtQuantity.Value == "0" || this.txtQuantity.Value == "0000")
            {
                this.lblValidationQuantity.InnerText = "Quantity must not be equal to zeroes.";
                this.lblValidationQuantity.Attributes["style"] = "color:Red;";

                //Show Offcanvas
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showQuantity();", true);
            }

            else
            {
                try
                {
                    posBO.controlnumber = this.hfControlNumber.Value;
                    posBO.tablecode = this.hfTableCode.Value;
                    posBO.itemcode = this.ddlOrderItemsQuantity.SelectedValue;
                    posBO.itemquantity = Convert.ToInt32(this.txtQuantity.Value);

                    //Post
                    string strPost = posBL.POS_Orders_Quantity(posBO, strUserID);

                    //System Logs
                    strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Orders - Quantity", "Saved new quantity for Item " + this.ddlOrderItemsQuantity.SelectedItem.Text + " (" + this.hfControlNumber.Value + ")", strUserID);

                    //Alert
                    ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Quantity Saved', '" + this.ddlOrderItemsQuantity.SelectedItem.Text + "' + ' quantity has been saved.', 'success'); ", true);

                    //Orders
                    using (SqlDataReader drORDERS = posBL.POS_Orders_View(this.hfControlNumber.Value, this.hfTableCode.Value, string.Empty, strUserID))
                    {
                        //this.hfTableCode.Value = strTableCode;

                        if (drORDERS.HasRows)
                        {
                            //Gridview
                            this.gvOrderItems.DataSource = drORDERS;
                            this.gvOrderItems.DataBind();
                            this.gvOrderItems.Columns[0].Visible = false; //CheckBox
                            this.gvOrderItems.Columns[1].Visible = false; //Item Code
                            this.gvOrderItems.Columns[4].Visible = false; //Order Status
                            this.gvOrderItems.Columns[5].Visible = false; //Delete Button
                        }

                        else
                        {
                            //Gridview
                            this.gvOrderItems.Visible = false;
                        }
                    }

                    ///Order Totals
                    using (SqlDataReader drORDERTOTALS = posBL.POS_OrdersTotal_View(this.hfControlNumber.Value, strUserID))
                    {
                        if (drORDERTOTALS.Read())
                        {
                            this.hfControlNumber.Value = drORDERTOTALS["ControlNumber"].ToString();

                            this.lblSubTotal.InnerText = drORDERTOTALS["SubTotal"].ToString();
                            this.lblTaxAmount.InnerText = drORDERTOTALS["TaxAmount"].ToString();
                            this.lblDiscountName.InnerText = drORDERTOTALS["DiscountName"].ToString();
                            this.lblDiscountAmount.InnerText = drORDERTOTALS["DiscountAmount"].ToString();
                            this.lblGrandTotal.InnerText = drORDERTOTALS["GrandTotal"].ToString();
                        }

                        else
                        {
                            this.lblSubTotal.InnerText = "0.00";
                            this.lblTaxAmount.InnerText = "0.00";
                            this.lblDiscountAmount.InnerText = "0.00";
                            this.lblServiceChargeAmount.InnerText = "0.00";
                            this.lblGrandTotal.InnerText = "0.00";
                        }
                    }

                    //Discounts
                    this.divDiscounts.Visible = false;

                    //Cancel Order
                    this.divCancelOrder.Visible = false;

                    //Order Buttons
                    this.lbtnDiscount.Visible = true;
                    this.lbtnQuantity.Visible = true;
                    this.btnSaveTransaction.Enabled = true;
                    this.btnSaveTransaction.Text = "Add Payment";
                    this.btnCancel.Visible = true;
                }

                catch (Exception ex)
                {
                    //Error Message
                    this.divErrorMessage.Visible = true;
                    this.lblErrorMessage.InnerText = ex.Message;
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Quantity - Save", ex.Message, strUserID);

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //Items
                    this.divItems.Visible = false;

                    //Orders
                    this.divOrderItems.Visible = false;
                }
            }
        }

        //Service Charge
        protected void lbtnServiceCharge_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];

            //Order Totals
            using (SqlDataReader drORDERTOTALS = posBL.POS_OrdersTotal_View(this.hfControlNumber.Value, strUserID))
            {
                if (drORDERTOTALS.Read())
                {
                    if (drORDERTOTALS["ServiceCharge"].ToString() != "0%")
                    {
                        //Alert
                        ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Service Charge Already Exist', 'Service charge already exist for order items at ' + '" + this.lblTableName.InnerText + "', 'info'); ", true); 
                    }

                    else
                    {
                        //Service Charge
                        this.divServiceCharge.Visible = true;
                        this.ddlServiceCharge.ClearSelection();
                        ServiceCharge();
                        this.ddlServiceCharge.Focus();
                        this.hfTransactionCode.Value = "70";

                        //Button Control
                        this.lbtnDiscount.Visible = false;
                        this.lbtnQuantity.Visible = false;
                        this.lbtnServiceCharge.Visible = false;
                        this.btnSaveTransaction.Text = "SAVE SERVICE CHARGE";
                    }

                }

                else
                {
                    //Service Charge
                    this.divServiceCharge.Visible = true;
                    this.ddlServiceCharge.ClearSelection();
                    ServiceCharge();
                    this.ddlServiceCharge.Focus();
                    this.hfTransactionCode.Value = "70";

                    //Button Control
                    this.lbtnDiscount.Visible = false;
                    this.lbtnQuantity.Visible = false;
                    this.lbtnServiceCharge.Visible = false;
                    this.btnSaveTransaction.Text = "SAVE SERVICE CHARGE";
                }
            }
        }

        //Order Buttons

        protected void btnSaveTransaction_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            try
            {
                if (this.hfTransactionCode.Value == "10") //New Order
                {
                    if (this.gvOrderItems.Rows.Count == 0)
                    {
                        //Alert
                        ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Missing Orders', 'Please select atleast 1 item from the menu', 'warning'); ", true);
                    }

                    else
                    {
                        posBO.controlnumber = seqBL.ControlNumber_Generate(4);
                        posBO.tablecode = this.hfTableCode.Value;

                        //Post
                        string strPost = posBL.POS_Orders_Post(posBO, strUserID);

                        //System Logs
                        strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Orders - Save", "Saved orders for Table " + this.hfTableCode.Value + " (" + posBO.controlnumber + ")", strUserID);

                        //Redirect
                        ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Orders Saved', '" + this.lblTableName.InnerText + "' +  ' orders has been saved','success') .then((value) => { window.location.href = 'POS.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole + "'; }); ", true);
                    }
                }

                else if (this.hfTransactionCode.Value == "30") //Bill Out
                {
                    this.lblPaymentTitle.InnerText = this.lblTableName.InnerText;
                    this.lblAmountDue.InnerText = this.lblGrandTotal.InnerText;
                    PaymentType();

                    //Payment Types //Default is Cash
                    this.divCash.Visible = true;
                    this.divCreditCard.Visible = false;
                    this.divGCash.Visible = false;

                    this.txtCashAmount.Text = "0.00";
                    this.txtChangeDue.Text = "0.00";
                    this.txtChangeDue.Enabled = false;
                    this.txtCashAmount.Focus();

                    //Show Offcanvas
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showPayment();", true);
                }

                else if (this.hfTransactionCode.Value == "40") //Void Order
                {
                    //Verify if atleast 1 order item is selected
                    int intChecker = 0;

                    //Loop inside gridview
                    foreach (GridViewRow row in this.gvOrderItems.Rows)
                    {
                        HtmlInputCheckBox cbOrderItems = (HtmlInputCheckBox)row.FindControl("cbOrderItems");

                        if (cbOrderItems.Checked)
                        {
                            intChecker = intChecker + 1; // add selected Item text to the String .  
                        }
                    }

                    if (intChecker < 1)
                    {
                        //Alert
                        ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Required Field', 'Void orders requires atleast 1 order item to be selected.', 'warning'); ", true);
                    }

                    else
                    {
                        //Authorization
                        this.lblAuthorizationTitle.InnerText = "Authorization - Void Order";
                        ImmediateSupervisor();
                        this.lblValidationAuthorization.InnerText = String.Empty;

                        //Show Modal
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showAuthorization();", true);
                    }
                }

                else if (this.hfTransactionCode.Value == "50") //Cancel Order
                {
                    if (this.ddlCancellationTypes.SelectedItem.Text == "Select Reason")
                    {
                        //Alert
                        ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Required Field', 'Please select a reason for cancellation.', 'warning'); ", true);
                        this.ddlCancellationTypes.Focus();
                    }

                    else
                    {
                        //Authorization
                        this.lblAuthorizationTitle.InnerText = "Authorization - Cancel Order";
                        ImmediateSupervisor();
                        this.lblValidationAuthorization.InnerText = String.Empty;

                        //Show Modal
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showAuthorization();", true);
                    }
                }

                else if (this.hfTransactionCode.Value == "60") //Discounts
                {
                    if (this.ddlDiscountType.SelectedItem.Text == "Select Discount")
                    {
                        //Alert
                        ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Required Field', 'Discount type must not be empty.', 'warning'); ", true);
                    }

                    else if (this.ddlDiscountType.SelectedItem.Text == "Employee" && (this.txtDiscountAmount.Value == string.Empty || this.txtDiscountAmount.Value == "0.00"))
                    {
                        //Alert
                        ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Invalid Discount', 'Discount amount must not be empty or equal to zeroes.', 'warning'); ", true);
                    }

                    else
                    {
                        posBO.controlnumber = this.hfControlNumber.Value;

                        if (this.ddlDiscountType.SelectedValue == "1" || this.ddlDiscountType.SelectedValue == "2") //SC //PWD
                        {
                            //Loop inside gridview
                            foreach (GridViewRow row in this.gvOrderItems.Rows)
                            {
                                HtmlInputCheckBox cbOrderItems = (HtmlInputCheckBox)row.FindControl("cbOrderItems");
                                Label lblItemCode = (Label)row.FindControl("lblItemCode");

                                if (cbOrderItems.Checked)
                                {
                                    posBO.itemcode = lblItemCode.Text;
                                    posBO.discounttype = Convert.ToInt32(this.ddlDiscountType.SelectedValue);
                                    posBO.discountamount = Convert.ToDecimal(this.txtDiscountAmount.Value);

                                    //Post
                                    string strPost = posBL.POS_Orders_Discount(posBO, strUserID);

                                    //System Logs
                                    strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Orders - Discount", "Saved discount for item " + lblItemCode.Text + " for Table " + this.hfTableCode.Value + " (" + posBO.controlnumber + ")", strUserID);
                                }
                            }
                        }

                        else
                        {
                            posBO.itemcode = String.Empty;
                            posBO.discounttype = Convert.ToInt32(this.ddlDiscountType.SelectedValue);
                            posBO.discountamount = Convert.ToDecimal(this.txtDiscountAmount.Value);

                            //Post
                            string strPost = posBL.POS_Orders_Discount(posBO, strUserID);

                            //System Logs
                            strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Orders - Discount", "Saved discount for order items at " + this.hfTableCode.Value + " (" + posBO.controlnumber + ")", strUserID);
                        }

                        //Alert
                        ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Discount Saved', 'Saved discount for order items at ' + '" + this.lblTableName.InnerText + "', 'success'); ", true);

                        //
                        //Search
                        this.txtSearch.Enabled = false;

                        //Item Category
                        this.dtItemCategory.Enabled = false;
                        this.dtItemCategory.Attributes["style"] = "opacity:50%;";

                        //Items
                        this.dtItemMaster.Enabled = false;
                        this.dtItemMaster.Attributes["style"] = "opacity:50%;";

                        //Order Items
                        using (SqlDataReader drORDERITEMS = posBL.POS_Orders_View(this.hfControlNumber.Value, this.hfTableCode.Value, string.Empty, strUserID))
                        {
                            this.hfTransactionCode.Value = "30";

                            if (drORDERITEMS.HasRows)
                            {
                                //Gridview
                                this.gvOrderItems.DataSource = drORDERITEMS;
                                this.gvOrderItems.DataBind();
                                this.gvOrderItems.Columns[0].Visible = false; //CheckBox
                                this.gvOrderItems.Columns[1].Visible = false; //Item Code
                                this.gvOrderItems.Columns[4].Visible = false; //Order Status
                                this.gvOrderItems.Columns[5].Visible = false; //Delete Button
                                OrderTotals();
                            }

                            else
                            {
                                //Gridview
                                this.gvOrderItems.Visible = false;
                            }
                        }

                        //Discounts
                        this.divDiscounts.Visible = false;

                        //Cancel Orders
                        this.divCancelOrder.Visible = false;

                        //Service Charge
                        this.divServiceCharge.Visible = false;

                        //Order Buttons
                        this.lbtnDiscount.Visible = true;
                        this.lbtnQuantity.Visible = true;
                        this.lbtnServiceCharge.Visible = true;
                        this.btnSaveTransaction.Enabled = true;
                        this.btnSaveTransaction.Text = "ADD PAYMENT";
                        this.btnCancelTransaction.Visible = true;
                    }
                }

                else if (this.hfTransactionCode.Value == "70") //Service Charge
                {
                    posBO.controlnumber = this.hfControlNumber.Value;
                    posBO.servicecharge = Convert.ToDecimal(this.ddlServiceCharge.SelectedValue);

                    //Post
                    string strPost = posBL.POS_Orders_ServiceCharge(posBO, strUserID);

                    //System Logs
                    strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Orders - Service Charge", "Saved service charge for order items at " + this.hfTableCode.Value + " (" + posBO.controlnumber + ")", strUserID);

                    //Alert
                    ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Service Charge Saved', 'Saved service charge for order items at ' + '" + this.lblTableName.InnerText + "', 'success'); ", true);

                    //
                    //Search
                    this.txtSearch.Enabled = false;

                    //Item Category
                    this.dtItemCategory.Enabled = false;
                    this.dtItemCategory.Attributes["style"] = "opacity:50%;";

                    //Items
                    this.dtItemMaster.Enabled = false;
                    this.dtItemMaster.Attributes["style"] = "opacity:50%;";

                    //Order Items
                    using (SqlDataReader drORDERITEMS = posBL.POS_Orders_View(this.hfControlNumber.Value, this.hfTableCode.Value, string.Empty, strUserID))
                    {
                        this.hfTransactionCode.Value = "30";

                        if (drORDERITEMS.HasRows)
                        {
                            //Gridview
                            this.gvOrderItems.DataSource = drORDERITEMS;
                            this.gvOrderItems.DataBind();
                            this.gvOrderItems.Columns[0].Visible = false; //CheckBox
                            this.gvOrderItems.Columns[1].Visible = false; //Item Code
                            this.gvOrderItems.Columns[4].Visible = false; //Order Status
                            this.gvOrderItems.Columns[5].Visible = false; //Delete Button
                            OrderTotals();
                        }

                        else
                        {
                            //Gridview
                            this.gvOrderItems.Visible = false;
                        }
                    }

                    //Discounts
                    this.divDiscounts.Visible = false;

                    //Cancel Orders
                    this.divCancelOrder.Visible = false;

                    //Service Charge
                    this.divServiceCharge.Visible = false;

                    //Order Buttons
                    this.lbtnDiscount.Visible = true;
                    this.lbtnQuantity.Visible = true;
                    this.lbtnServiceCharge.Visible = true;
                    this.btnSaveTransaction.Enabled = true;
                    this.btnSaveTransaction.Text = "ADD PAYMENT";
                    this.btnCancelTransaction.Visible = true;
                }
            }

            catch (Exception ex)
            {
                //Error Message
                this.divErrorMessage.Visible = true;
                this.lblErrorMessage.InnerText = ex.Message;
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Orders - Save", ex.Message, strUserID);

                //Button Controls
                this.divButtonControls.Visible = false;

                //Items
                this.divItems.Visible = false;

                //Orders
                this.divOrderItems.Visible = false;
            }
        }

        protected void btnCancelTransaction_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            //Redirect
            Response.Redirect("POS.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        //Payment
        protected void ddlPaymentType_TextChanged(object sender, EventArgs e)
        {
            if (this.ddlPaymentType.SelectedItem.Text == "Cash")
            {
                //Payment Types
                this.divCash.Visible = true;
                this.divCreditCard.Visible = false;
                this.divGCash.Visible = false;

                this.txtCashAmount.Text = "0.00";
                this.txtChangeDue.Text = "0.00";
                this.txtChangeDue.Enabled = false;
                this.txtCashAmount.Focus();
            }

            else if (this.ddlPaymentType.SelectedItem.Text == "Credit Card")
            {
                //Payment Types
                this.divCash.Visible = false;
                this.divCreditCard.Visible = true;
                this.divGCash.Visible = false;
                this.txtCardType.Focus();

                this.txtCreditCardAmount.Text = this.lblAmountDue.InnerText;
                this.txtCreditCardAmount.Enabled = false;
                this.txtCardType.Value = string.Empty;
                this.txtCardNumber.Value = string.Empty;
                this.txtAccountName.Value = string.Empty;
                this.txtExpiryDate.Value = string.Empty;
            }

            else if (this.ddlPaymentType.SelectedItem.Text == "GCash")
            {
                //Payment Types
                this.divCash.Visible = false;
                this.divCreditCard.Visible = false;
                this.divGCash.Visible = true;
                this.txtGCashReferenceNumber.Focus();
                this.txtGCashAmount.Text = this.lblAmountDue.InnerText;
                this.txtGCashAmount.Enabled = false;

                this.txtGCashReferenceNumber.Value = string.Empty;
            }

            this.lblValidationPayment.InnerText = string.Empty;

            //Show Modal
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showPayment();", true);
        }

        protected void txtCashAmount_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(this.txtCashAmount.Text) < Convert.ToDecimal(this.lblGrandTotal.InnerText))
            {
                this.lblValidationPayment.InnerText = "Amount tendered must not be less than the total amount due";
                this.lblValidationPayment.Attributes["style"] = "color:Red;";
                this.txtCashAmount.Focus();                
            }

            else
            {
                decimal decGrandTotal = Convert.ToDecimal(this.lblGrandTotal.InnerText);
                decimal decCashAmount = Convert.ToDecimal(this.txtCashAmount.Text);
                decimal decChangeAmount = 0;

                decChangeAmount = (decCashAmount - decGrandTotal);
                
                this.txtCashAmount.Text = decCashAmount.ToString("N2");
                this.txtChangeDue.Text = decChangeAmount.ToString("N2");
                this.lblValidationPayment.InnerText = string.Empty;
            }

            //Show Offcanvas
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showPayment();", true);
        }

        protected void btnPostPayment_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            if (this.ddlPaymentType.SelectedItem.Text == "Cash" && this.txtCashAmount.Text == string.Empty)
            {
                this.lblValidationPayment.InnerText = "Amount tendered must not be empty.";
                this.lblValidationPayment.Attributes["style"] = "color:Red;";
                this.txtCashAmount.Focus();

                //Show Offcanvas
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showPayment();", true);
            }

            else if (this.ddlPaymentType.SelectedItem.Text == "Cash" && (Convert.ToDecimal(this.txtCashAmount.Text) < Convert.ToDecimal(this.lblGrandTotal.InnerText)))
            {
                this.lblValidationPayment.InnerText = "Amount tendered must not be less than the total amount due.";
                this.lblValidationPayment.Attributes["style"] = "color:Red;";
                this.txtCashAmount.Focus();

                //Show Offcanvas
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showPayment();", true);
            }

            else if (this.ddlPaymentType.SelectedItem.Text == "Credit Card" && this.txtCardType.Value == string.Empty)
            {
                this.lblValidationPayment.InnerText = "Card type must not be empty.";
                this.lblValidationPayment.Attributes["style"] = "color:Red;";
                this.txtCardType.Focus();

                //Show Offcanvas
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showPayment();", true);
            }

            else if (this.ddlPaymentType.SelectedItem.Text == "Credit Card" && this.txtCardNumber.Value == string.Empty)
            {
                this.lblValidationPayment.InnerText = "Card number must not be empty.";
                this.lblValidationPayment.Attributes["style"] = "color:Red;";
                this.txtCardNumber.Focus();

                //Show Offcanvas
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showPayment();", true);
            }

            else if (this.ddlPaymentType.SelectedItem.Text == "Credit Card" && this.txtAccountName.Value == string.Empty)
            {
                this.lblValidationPayment.InnerText = "Account name must not be empty.";
                this.lblValidationPayment.Attributes["style"] = "color:Red;";
                this.txtAccountName.Focus();

                //Show Offcanvas
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showPayment();", true);
            }

            else if (this.ddlPaymentType.SelectedItem.Text == "Credit Card" && this.txtExpiryDate.Value == string.Empty)
            {
                this.lblValidationPayment.InnerText = "Expiry date must not be empty.";
                this.lblValidationPayment.Attributes["style"] = "color:Red;";
                this.txtExpiryDate.Focus();

                //Show Offcanvas
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showPayment();", true);
            }

            else if (this.ddlPaymentType.SelectedItem.Text == "GCash" && this.txtGCashReferenceNumber.Value == string.Empty)
            {
                this.lblValidationPayment.InnerText = "GCash reference number must not be empty.";
                this.lblValidationPayment.Attributes["style"] = "color:Red;";
                this.txtGCashReferenceNumber.Focus();

                //Show Offcanvas
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showPayment();", true);
            }

            else
            {
                try
                {
                    posBO.controlnumber = this.hfControlNumber.Value;
                    posBO.tablecode = this.hfTableCode.Value;
                    posBO.paymenttype = Convert.ToInt32(this.ddlPaymentType.SelectedValue);

                    if (this.ddlPaymentType.SelectedValue == "1") //Cash
                    {
                        posBO.paymentamount = Convert.ToDecimal(this.txtCashAmount.Text);
                    }

                    else if (this.ddlPaymentType.SelectedValue == "2") //Credit Card
                    {
                        posBO.paymentamount = Convert.ToDecimal(this.txtCreditCardAmount.Text);
                    }

                    else //GCash
                    {
                        posBO.paymentamount = Convert.ToDecimal(this.txtGCashAmount.Text);
                    }

                    posBO.changedue = Convert.ToDecimal(this.txtChangeDue.Text);
                    posBO.cardtype = this.txtCardType.Value.Trim();
                    posBO.cardnumber = this.txtCardNumber.Value.Trim();
                    posBO.accountname = this.txtAccountName.Value.Trim();

                    if (this.txtExpiryDate.Value == string.Empty)
                    {
                        posBO.expirydate = Convert.ToDateTime("1901-01-01");
                    }

                    else
                    {
                        posBO.expirydate = Convert.ToDateTime(this.txtExpiryDate.Value);
                    }
                    
                    posBO.gcashreferencenumber = this.txtGCashReferenceNumber.Value.Trim();

                    //Post
                    string strPost = posBL.POS_Orders_Payment(posBO, strUserID);

                    //System Logs
                    strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Orders - Payment", "Saved payment orders for Table " + this.hfTableCode.Value + " (" + posBO.controlnumber + ")", strUserID);

                    //Alert
                    ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Payment Successful', '" + this.lblTableName.InnerText + "' +  ' orders has been paid', 'success'); ", true);

                    GenerateOrderReceipt();
                }

                catch (Exception ex)
                {
                    //Error Message
                    this.divErrorMessage.Visible = true;
                    this.lblErrorMessage.InnerText = ex.Message;
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Orders - Payment", ex.Message, strUserID);

                    //Button Controls
                    this.divButtonControls.Visible = false;

                    //Items
                    this.divItems.Visible = false;

                    //Orders
                    this.divOrderItems.Visible = false;
                }
            }            
        }

        //Receipt
        protected void btnCloseReceipt_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            //Redirect
            Response.Redirect("POS.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void btnPrintReceipt_Click(object sender, EventArgs e)
        {
            OrderReceipt();
        }

        //Authorization
        protected void btnPostAuthorization_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            try
            {
                if (this.ddlImmediateSupervisor.SelectedItem.Text == "Select Immediate Supervisor")
                {
                    this.lblValidationAuthorization.InnerText = "Immediate supervisor must not be empty.";
                    this.lblValidationAuthorization.Attributes["style"] = "color:Red;";

                    //Show Modal
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showAuthorization();", true);
                }

                else if (this.txtImmediateSupervisorPassword.Value == string.Empty)
                {
                    this.lblValidationAuthorization.InnerText = "Immediate supervisor password must not be empty.";
                    this.lblValidationAuthorization.Attributes["style"] = "color:Red;";

                    //Show Modal
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showAuthorization();", true);
                }

                else
                {
                    //Encrypt immediate supervisor password
                    var strImmediateSupervisorPassword = baseclass.ComputeHash(this.txtImmediateSupervisorPassword.Value, new MD5CryptoServiceProvider());

                    //Check if 
                    using (SqlDataReader drIMMEDIATESUPERVISOR = userprofBL.UserProfiles_View(this.ddlImmediateSupervisor.SelectedValue, string.Empty))
                    {
                        if (drIMMEDIATESUPERVISOR.Read())
                        {
                            //Check 
                            if (drIMMEDIATESUPERVISOR["UserPassword"].ToString() == strImmediateSupervisorPassword.ToString())
                            {
                                if (this.hfTransactionCode.Value == "40") //Void Order
                                {
                                    foreach (GridViewRow row in this.gvOrderItems.Rows)
                                    {
                                        HtmlInputCheckBox cbOrderItems = (HtmlInputCheckBox)row.FindControl("cbOrderItems");
                                        Label lblItemCode = (Label)row.FindControl("lblItemCode");

                                        if (cbOrderItems.Checked)
                                        {
                                            posBO.controlnumber = this.hfControlNumber.Value;
                                            posBO.itemcode = lblItemCode.Text.Trim();
                                            posBO.voidauthorizedby = this.ddlImmediateSupervisor.SelectedValue;

                                            //Post
                                            string strPost = posBL.POS_Orders_Void(posBO, strUserID);

                                            //System Logs
                                            strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Void Order", "Voided order item " + lblItemCode.Text + " for Table " + this.hfTableCode.Value + " (" + posBO.controlnumber + ")", strUserID);
                                        }
                                    }

                                    //Alert
                                    ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Void Completed', 'Voided order items for ' + '" + this.lblTableName.InnerText + "' + ' (' + '" + posBO.controlnumber + "' + ')', 'success'); ", true);

                                    //
                                    //Search
                                    this.txtSearch.Enabled = false;

                                    //Item Category
                                    this.dtItemCategory.Enabled = false;
                                    this.dtItemCategory.Attributes["style"] = "opacity:50%;";

                                    //Items
                                    this.dtItemMaster.Enabled = false;
                                    this.dtItemMaster.Attributes["style"] = "opacity:50%;";

                                    //Order Items
                                    using (SqlDataReader drORDERITEMS = posBL.POS_Orders_View(this.hfControlNumber.Value, this.hfTableCode.Value, string.Empty, strUserID))
                                    {
                                        this.hfTransactionCode.Value = "20";

                                        if (drORDERITEMS.HasRows)
                                        {
                                            //Gridview
                                            this.gvOrderItems.DataSource = drORDERITEMS;
                                            this.gvOrderItems.DataBind();
                                            this.gvOrderItems.Columns[0].Visible = false; //CheckBox
                                            this.gvOrderItems.Columns[1].Visible = false; //Item Code
                                            this.gvOrderItems.Columns[4].Visible = false; //Order Status
                                            this.gvOrderItems.Columns[5].Visible = false; //Delete Button
                                            OrderTotals();
                                        }

                                        else
                                        {
                                            //Gridview
                                            this.gvOrderItems.Visible = false;
                                        }
                                    }

                                    //Order Buttons
                                    this.lbtnDiscount.Visible = false;
                                    this.lbtnQuantity.Visible = false;
                                    this.btnSaveTransaction.Enabled = true;
                                    this.btnSaveTransaction.Text = "ADD PAYMENT";
                                    this.btnCancelTransaction.Visible = true;
                                }

                                else if (this.hfTransactionCode.Value == "50") //Cancel Order
                                {
                                    posBO.controlnumber = this.hfControlNumber.Value;
                                    posBO.tablecode = this.hfTableCode.Value;
                                    posBO.canceltype = Convert.ToInt32(this.ddlCancellationTypes.SelectedValue);
                                    posBO.cancelauthorizedby = this.ddlImmediateSupervisor.SelectedValue;
                                    posBO.cancelremarks = String.Empty; //this.txtCancelRemarks.Value.Trim();

                                    //Post
                                    string strPost = posBL.POS_Orders_Cancel(posBO, strUserID);

                                    //System Logs
                                    strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Orders - Cancelled", "Cancelled orders for Table " + this.hfTableCode.Value + " (" + this.hfControlNumber.Value + ")", strUserID);

                                    //Redirect
                                    ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Orders Cancelled', '" + this.lblTableName.InnerText + "' +  ' orders has been cancelled', 'success') .then((value) => { window.location.href = 'POS.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole + "'; }); ", true);
                                }
                            }

                            else
                            {
                                this.lblValidationAuthorization.InnerText = "Immediate supervisor password is incorrect.";
                                this.lblValidationAuthorization.Attributes["style"] = "color:Red;";

                                //Show Offcanvas
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showAuthorization();", true);
                            }
                        }


                        else
                        {
                            this.lblValidationAuthorization.InnerText = "Immediate supervisor unknown.";
                            this.lblValidationAuthorization.Attributes["style"] = "color:Red;";

                            //Show Offcanvas
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Modal", "showAuthorization();", true);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                //Error Message
                this.divErrorMessage.Visible = true;
                this.lblErrorMessage.InnerText = ex.Message;
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Authorization - Post", ex.Message, strUserID);

                //Button Controls
                this.divButtonControls.Visible = false;

                //Items
                this.divItems.Visible = false;

                //Order Items
                this.divOrderItems.Visible = false;
            }
        }

        //Error Message
        protected void btnReload_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            //Redirect
            Response.Redirect("POS.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        
    }
}
