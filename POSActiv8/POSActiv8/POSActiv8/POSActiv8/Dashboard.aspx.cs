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
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;
using System.IO;
using OfficeOpenXml;
using System.Web.Services;
using System.Net;
using System.Web.Script.Serialization;

using BusinessObject;
using BusinessLogic;
using DataAccess;

namespace POSActiv8
{
    public partial class Dashboard : System.Web.UI.Page
    {
        SystemLogsBL slBL = new SystemLogsBL();
        ErrorLogsBL elBL = new ErrorLogsBL();
        RegisterShiftBL regshiftBL = new RegisterShiftBL();
        PointofSaleBL posBL = new PointofSaleBL();
        //DashboardBL dashBL = new DashboardBL();
        //SiteLocationsBL sitelocBL = new SiteLocationsBL();
        //VehicleInventoryBL vehinvBL = new VehicleInventoryBL();
        string strUserID;
        int intUserLevel;
        int intUserRole;
        string strSystemLogs = String.Empty;
        string strErrorLogs = String.Empty;
        DateTime dteTransactionDate;

        //Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                strUserID = Request.QueryString["userid"];
                intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
                intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

                //Error Message
                this.divErrorMessage.Visible = false;

                //Page Title
                this.divPageTitle.Visible = true;
                this.lblReferenceDate.InnerText = System.DateTime.Now.ToString("dddd, MMMM dd, yyyy");

                //Dashboard
                this.divDashboard.Visible = true;

                try
                {
                    //Transaction Date
                    dteTransactionDate = System.DateTime.Now;
                   // this.txtTransactionDate.Text = dteTransactionDate.ToString("yyyy-MM-dd");

                    //Sales Invoice
                    using (SqlDataReader drSALESINVOICE = posBL.POS_SalesInvoice_View(1, string.Empty, dteTransactionDate, dteTransactionDate))
                    {
                        if (drSALESINVOICE.Read())
                        {
                            this.lnkTotalSales.Text = drSALESINVOICE["GrandTotal"].ToString();
                        }

                        else
                        {
                            this.lnkTotalSales.Text = "0.00";
                        }
                    }

                    //Opening and Closing
                    using (SqlDataReader drREGISTERSHIFT = regshiftBL.RegisterShift_View("1", dteTransactionDate.ToString("yyyy-MM-dd")))
                    {
                        if (drREGISTERSHIFT.Read())
                        {
                            this.lnkOpeningAmount.Text = drREGISTERSHIFT["OpeningAmount"].ToString();
                            this.lnkClosingAmount.Text = drREGISTERSHIFT["ClosingAmount"].ToString();
                        }

                        else
                        {
                            this.lnkOpeningAmount.Text = "0.00";
                            this.lnkClosingAmount.Text = "0.00";
                        }
                    }



                        //this.ddlSiteLocation.Focus();

                        ////Operational Vehicle Status
                        //using (SqlDataReader drOPERATIONALVEHICLESTATUS = dashBL.Dashboard_VehicleStatus_View(0, 0))
                        //{
                        //    if (drOPERATIONALVEHICLESTATUS.Read())
                        //    {
                        //        this.lnkTotalUnitsCount.Text = drOPERATIONALVEHICLESTATUS["TotalUnits"].ToString();
                        //    }
                        //}

                        ////Operational Vehicle Status
                        //using (SqlDataReader drOPERATIONALVEHICLESTATUS = dashBL.Dashboard_VehicleStatus_View(0, 2))
                        //{
                        //    if (drOPERATIONALVEHICLESTATUS.Read())
                        //    {
                        //        this.lnkOperationalUnitsCount.Text = drOPERATIONALVEHICLESTATUS["VehicleStatusCount"].ToString();
                        //    }
                        //}

                        ////Breakdown Vehicle Status
                        //using (SqlDataReader drBREAKDOWNVEHICLESTATUS = dashBL.Dashboard_VehicleStatus_View(0, 1))
                        //{
                        //    if (drBREAKDOWNVEHICLESTATUS.Read())
                        //    {
                        //        this.lnkBreakdownUnitsCount.Text = drBREAKDOWNVEHICLESTATUS["VehicleStatusCount"].ToString();
                        //    }
                        //}

                        ////Equipment Breakdown
                        //using (SqlDataReader drEQUIPMENTBREAKDOWN = dashBL.Dashboard_VehicleInventory_View(0))
                        //{
                        //    if (drEQUIPMENTBREAKDOWN.HasRows)
                        //    {
                        //        //Gridview
                        //        this.gvEquipmentBreakDown.DataSource = drEQUIPMENTBREAKDOWN;
                        //        this.gvEquipmentBreakDown.DataBind();
                        //        this.gvEquipmentBreakDown.Visible = true;
                        //    }

                        //    else
                        //    {
                        //        //Gridview
                        //        this.gvEquipmentBreakDown.DataSource = drEQUIPMENTBREAKDOWN;
                        //        this.gvEquipmentBreakDown.DataBind();
                        //        this.gvEquipmentBreakDown.Visible = true;
                        //    }
                        //}

                        ////Vehicle Summary
                        //using (SqlDataReader drVEHICLESUMMARY = vehinvBL.Vehicle_Summary_View(Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd")), Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd"))))
                        //{
                        //    if (drVEHICLESUMMARY.HasRows)
                        //    {
                        //        //Gridview
                        //        this.gvVehicleSummary.DataSource = drVEHICLESUMMARY;
                        //        this.gvVehicleSummary.DataBind();
                        //        this.gvVehicleSummary.Visible = true;
                        //    }

                        //    else
                        //    {
                        //        //Gridview
                        //        this.gvVehicleSummary.DataSource = drVEHICLESUMMARY;
                        //        this.gvVehicleSummary.DataBind();
                        //        this.gvVehicleSummary.Visible = true;
                        //    }
                        //}
                    }

                catch (Exception ex)
                {
                    //Error Message
                    this.divErrorMessage.Visible = true;
                    this.lblErrorMessage.InnerText = ex.Message;
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Dashboard - Page Load", ex.Message, strUserID);

                    //Page Title
                    this.divPageTitle.Visible = false;

                    //Dashboard
                    this.divDashboard.Visible = false;
                }
            }
        }

        //Button Controls
        protected void ddlSiteLocation_TextChanged(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];

            try
            {
                int intSiteLocation = 0;

                //if (this.ddlSiteLocation.SelectedItem.Text != "All Location")
                //{
                //    intSiteLocation = Convert.ToInt32(this.ddlSiteLocation.SelectedValue);
                //}

                ////Operational Vehicle Status
                //using (SqlDataReader drOPERATIONALVEHICLESTATUS = dashBL.Dashboard_VehicleStatus_View(intSiteLocation, 0))
                //{
                //    if (drOPERATIONALVEHICLESTATUS.Read())
                //    {
                //        this.lnkTotalUnitsCount.Text = drOPERATIONALVEHICLESTATUS["TotalUnits"].ToString();
                //    }
                //}

                ////Operational Vehicle Status
                //using (SqlDataReader drOPERATIONALVEHICLESTATUS = dashBL.Dashboard_VehicleStatus_View(intSiteLocation, 2))
                //{
                //    if (drOPERATIONALVEHICLESTATUS.Read())
                //    {
                //        this.lnkOperationalUnitsCount.Text = drOPERATIONALVEHICLESTATUS["VehicleStatusCount"].ToString();
                //    }
                //}

                ////Breakdown Vehicle Status
                //using (SqlDataReader drBREAKDOWNVEHICLESTATUS = dashBL.Dashboard_VehicleStatus_View(intSiteLocation, 1))
                //{
                //    if (drBREAKDOWNVEHICLESTATUS.Read())
                //    {
                //        this.lnkBreakdownUnitsCount.Text = drBREAKDOWNVEHICLESTATUS["VehicleStatusCount"].ToString();
                //    }
                //}

                ////Equipment Breakdown
                //using (SqlDataReader drEQUIPMENTBREAKDOWN = dashBL.Dashboard_VehicleInventory_View(intSiteLocation))
                //{
                //    if (drEQUIPMENTBREAKDOWN.HasRows)
                //    {
                //        //Gridview
                //        this.gvEquipmentBreakDown.DataSource = drEQUIPMENTBREAKDOWN;
                //        this.gvEquipmentBreakDown.DataBind();
                //        this.gvEquipmentBreakDown.Visible = true;
                //    }

                //    else
                //    {
                //        //Gridview
                //        this.gvEquipmentBreakDown.DataSource = drEQUIPMENTBREAKDOWN;
                //        this.gvEquipmentBreakDown.DataBind();
                //        this.gvEquipmentBreakDown.Visible = true;
                //    }
                //}
            }

            catch (Exception ex)
            {
                //Error Message
                this.divErrorMessage.Visible = true;
                this.lblErrorMessage.InnerText = ex.Message;
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Dashboard - Filter by Location", ex.Message, strUserID);

                //Page Title
                this.divPageTitle.Visible = false;

                //Dashboard
                this.divDashboard.Visible = false;
            }
        }

        protected void lnkTotalUnitsCount_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            //Redirect
            Response.Redirect("VehicleProfile.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void lnkOperationalUnitsCount_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            //Redirect
            Response.Redirect("VehicleProfile.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole + "&vehiclestatus=" + "2", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void lnkBreakdownUnitsCount_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            //Redirect
            Response.Redirect("VehicleProfile.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole + "&vehiclestatus=" + "1", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        //Error Message
        protected void btnReload_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            Response.Redirect("Dashboard.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void lnkOpeningAmount_Click(object sender, EventArgs e)
        {

        }
    }
}