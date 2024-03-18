using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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
using MySql.Data.MySqlClient;

using BusinessObject;
using BusinessLogic;
using DataAccess;

namespace POSActiv8
{
    public partial class KitchenOrders : System.Web.UI.Page
    {
        SystemLogsBL slBL = new SystemLogsBL();
        ErrorLogsBL elBL = new ErrorLogsBL();
        SystemParametersBL sysparamBL = new SystemParametersBL();
        KitchenOrdersBO kitchenordersBO = new KitchenOrdersBO();
        KitchenOrdersBL kitchordersBL = new KitchenOrdersBL();
        string strUserID;
        int intUserLevel;
        int intUserRole;
        string strSystemLogs = string.Empty;
        string strErrorLogs = string.Empty;
        string strControlNumber = string.Empty;

        //Transaction Date
        public void POSTransactionDate()
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            //Get Computer Name
            var strTerminalName = System.Environment.MachineName;

            //Get POS transaction date
            System.Data.DataSet dsPOSTRANSACTIONDATE = new System.Data.DataSet();
            dsPOSTRANSACTIONDATE = DBHelper.GetData("SELECT TOP 1 SessionCode, SessionStatus, TransactionDate FROM POSSessions WHERE SessionStatus = '1'");

            if (dsPOSTRANSACTIONDATE.Tables[0].Rows.Count > 0)
            {
                this.hfTransactionDate.Value = dsPOSTRANSACTIONDATE.Tables[0].Rows[0]["TransactionDate"].ToString();
            }

            else
            {
                this.hfTransactionDate.Value = string.Empty;
            }

            dsPOSTRANSACTIONDATE.Clear();
        }

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

                //Kitchen Orders
                this.divKitchenOrders.Visible = true;

                POSTransactionDate();
                
                try
                {
                    //Kitchen Orders
                    using (MySqlDataReader drKITCHENORDERS = kitchordersBL.POS_KitchenOrders_View("0", Convert.ToDateTime(this.hfTransactionDate.Value)))
                    {
                        if (drKITCHENORDERS.HasRows)
                        {
                            //Datalist
                            this.dtKitchenOrders.DataSource = drKITCHENORDERS;
                            this.dtKitchenOrders.DataBind();
                            this.dtKitchenOrders.Visible = true;

                            //
                            this.lblKitcherOrders.InnerText = string.Empty;
                        }

                        else
                        {
                            //Datalist
                            this.dtKitchenOrders.Visible = false;

                            //
                            this.lblKitcherOrders.InnerText = "No orders to display";
                        }
                    }
                }

                catch (Exception ex)
                {
                    //Error Message
                    this.divErrorMessage.Visible = true;
                    this.lblErrorMessage.InnerText = ex.Message;
                    strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Kitchen Orders - Page Load", ex.Message, strUserID);

                    //Kitchen Orders
                    this.divKitchenOrders.Visible = false;
                }               
            }
        }

        //Button Controls
        protected void dtKitchenOrders_ItemCommand(object source, DataListCommandEventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            try
            { 
                //Get values from command argument using .split
                string[] val = e.CommandArgument.ToString().Split('|');
                strControlNumber = val[0].ToString();
                string strTableName = val[1].ToString();

                kitchenordersBO.controlnumber = strControlNumber;

                //Post
                string strPost = kitchordersBL.POS_KitchenOrders_Post(kitchenordersBO, strUserID);

                //System Logs
                strSystemLogs = slBL.SystemLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Kitchen Orders - Completed", "Orders completed for " + strTableName + " (" + strControlNumber + ")", strUserID);

                //Alert
                ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", "swal('Orders Delivered', '" + strTableName + "' + ' orders has been delivered (' + '" + strControlNumber + "' + ')', 'success'); ", true);

                //Kitchen Orders
                using (MySqlDataReader drKITCHENORDERS = kitchordersBL.POS_KitchenOrders_View("0", Convert.ToDateTime(this.hfTransactionDate.Value)))
                {
                    if (drKITCHENORDERS.HasRows)
                    {
                        //Datalist
                        this.dtKitchenOrders.DataSource = drKITCHENORDERS;
                        this.dtKitchenOrders.DataBind();
                        this.dtKitchenOrders.Visible = true;

                        //
                        this.lblKitcherOrders.InnerText = string.Empty;
                    }

                    else
                    {
                        //Datalist
                        this.dtKitchenOrders.Visible = false;

                        //
                        this.lblKitcherOrders.InnerText = "No orders to display";
                    }
                }
            }

            catch (Exception ex)
            {
                //Error Message
                this.divErrorMessage.Visible = true;
                this.lblErrorMessage.InnerText = ex.Message;
                strErrorLogs = elBL.ErrorLogs_Save(Path.GetFileName(Request.PhysicalPath).ToString(), "Kitchen Orders - Completed", ex.Message, strUserID);

                //Kitchen Orders
                this.divKitchenOrders.Visible = false;
            }
        }

        protected void dtKitchenOrders_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            GridView gvOrderItems = (GridView)e.Item.FindControl("gvOrderItems");
            Label lblControlNumber = (Label)e.Item.FindControl("lblControlNumber");

            //Get POS transaction date
            System.Data.DataSet dsPOSTRANSACTIONDATE = new System.Data.DataSet();
            dsPOSTRANSACTIONDATE = DBHelper.GetData("SELECT TOP 1 SessionCode, SessionStatus, TransactionDate FROM POSSessions WHERE SessionStatus = '1'");

            if (dsPOSTRANSACTIONDATE.Tables[0].Rows.Count > 0)
            {
                //Order Items
                using (MySqlDataReader drORDERITEMS = kitchordersBL.POS_KitchenOrders_View(lblControlNumber.Text, Convert.ToDateTime(dsPOSTRANSACTIONDATE.Tables[0].Rows[0]["TransactionDate"])))
                {
                    if (drORDERITEMS.HasRows)
                    {
                        //Gridview
                        gvOrderItems.DataSource = drORDERITEMS;
                        gvOrderItems.DataBind();
                        gvOrderItems.Visible = true;
                    }

                    else
                    {
                        //Gridview
                        gvOrderItems.DataSource = drORDERITEMS;
                        gvOrderItems.DataBind();
                        gvOrderItems.Visible = true;
                    }
                }
            }

            else
            {
                //Gridview
                gvOrderItems.Visible = false;
            }

            dsPOSTRANSACTIONDATE.Clear();
        }

        protected void tmrKitchenOrders_Tick(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            Response.Redirect("KitchenOrders.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        //Error Message
        protected void btnReload_Click(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["userid"];
            intUserLevel = Convert.ToInt32(Request.QueryString["userlevel"]);
            intUserRole = Convert.ToInt32(Request.QueryString["userrole"]);

            Response.Redirect("KitchenOrders.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole, false);
            Context.ApplicationInstance.CompleteRequest();
        }

       

    }
}
