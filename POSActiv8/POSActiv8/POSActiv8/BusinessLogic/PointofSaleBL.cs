using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using BusinessObject;
using DataAccess;

namespace BusinessLogic
{
    public class PointofSaleBL
    {
        //View
        public SqlDataReader POS_MyOrders_View(string strTableCode, string strUserID)
        {
            try
            {
                PointofSaleDA posDA = new PointofSaleDA();
                return posDA.POS_MyOrders_View(strTableCode, strUserID);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SqlDataReader POS_Orders_View(DateTime dteReferenceDate)
        {
            try
            {
                PointofSaleDA posDA = new PointofSaleDA();
                return posDA.POS_Orders_View(dteReferenceDate);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //
        public SqlDataReader POS_Orders_View(string strControlNumber, string strTableCode, string strItemCode, string strUserID)
        {
            try
            {
                PointofSaleDA posDA = new PointofSaleDA();
                return posDA.POS_Orders_View(strControlNumber, strTableCode, strItemCode, strUserID);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SqlDataReader POS_OrdersTotal_View(string strControlNumber, string strUserID)
        {
            try
            {
                PointofSaleDA posDA = new PointofSaleDA();
                return posDA.POS_OrdersTotal_View(strControlNumber, strUserID);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SqlDataReader POS_SalesInvoice_View(int intCMD, string strControlNumber, DateTime dteReferenceDateFrom, DateTime dteReferenceDateTo)
        {
            try
            {
                PointofSaleDA posDA = new PointofSaleDA();
                return posDA.POS_SalesInvoice_View(intCMD, strControlNumber, dteReferenceDateFrom, dteReferenceDateTo);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SqlDataReader POS_XZReport_View(int intTransactionType, DateTime dteTransactionDate, string strUserID)
        {
            try
            {
                PointofSaleDA posDA = new PointofSaleDA();
                return posDA.POS_XZReport_View(intTransactionType, dteTransactionDate, strUserID);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SqlDataReader POS_VoidItems_View(DateTime dteTransactionDate)
        {
            try
            {
                PointofSaleDA posDA = new PointofSaleDA();
                return posDA.POS_VoidItems_View(dteTransactionDate);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SqlDataReader POS_CancelledOrders_View(DateTime dteTransactionDate)
        {
            try
            {
                PointofSaleDA posDA = new PointofSaleDA();
                return posDA.POS_CancelledOrders_View(dteTransactionDate);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


        //Post
        public string POS_Orders_Select(PointofSaleBO posBO, string strUserID)
        {
            try
            {
                PointofSaleDA posDA = new PointofSaleDA();
                return posDA.POS_Orders_Select(posBO, strUserID);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string POS_Orders_Post(PointofSaleBO posBO, string strUserID)
        {
            try
            {
                PointofSaleDA posDA = new PointofSaleDA();
                return posDA.POS_Orders_Post(posBO, strUserID);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string POS_Orders_Discount(PointofSaleBO posBO, string strUserID)
        {
            try
            {
                PointofSaleDA posDA = new PointofSaleDA();
                return posDA.POS_Orders_Discount(posBO, strUserID);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string POS_Orders_Quantity(PointofSaleBO posBO, string strUserID)
        {
            try
            {
                PointofSaleDA posDA = new PointofSaleDA();
                return posDA.POS_Orders_Quantity(posBO, strUserID);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string POS_Orders_ServiceCharge(PointofSaleBO posBO, string strUserID)
        {
            try
            {
                PointofSaleDA posDA = new PointofSaleDA();
                return posDA.POS_Orders_ServiceCharge(posBO, strUserID);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string POS_Orders_Payment(PointofSaleBO posBO, string strUserID)
        {
            try
            {
                PointofSaleDA posDA = new PointofSaleDA();
                return posDA.POS_Orders_Payment(posBO, strUserID);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public string POS_Orders_Void(PointofSaleBO posBO, string strUserID)
        {
            try
            {
                PointofSaleDA posDA = new PointofSaleDA();
                return posDA.POS_Orders_Void(posBO, strUserID);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string POS_Orders_Cancel(PointofSaleBO posBO, string strUserID)
        {
            try
            {
                PointofSaleDA posDA = new PointofSaleDA();
                return posDA.POS_Orders_Cancel(posBO, strUserID);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
