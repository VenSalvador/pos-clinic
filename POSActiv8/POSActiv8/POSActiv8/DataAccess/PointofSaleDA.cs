using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

using BusinessObject;

namespace DataAccess
{
    public class PointofSaleDA
    {
        //View
        public SqlDataReader POS_MyOrders_View(string strTableCode, string strUserID)
        {
            try
            {
                SqlParameter[] myparams = new SqlParameter[]
                {
                    new SqlParameter("@strTableCode", strTableCode),
                    new SqlParameter("@strUserID", strUserID)
                };

                return DBHelper.ExecuteParameterizedReader("POS_MyOrders_View", CommandType.StoredProcedure, myparams);
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
                SqlParameter[] myparams = new SqlParameter[]
                {
                    new SqlParameter("@dteReferenceDate", dteReferenceDate)
                };

                return DBHelper.ExecuteParameterizedReader("POS_Orders_View", CommandType.StoredProcedure, myparams);
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
                SqlParameter[] myparams = new SqlParameter[]
                {
                    new SqlParameter("@strControlNumber", strControlNumber),
                    new SqlParameter("@strTableCode", strTableCode),
                    new SqlParameter("@strItemCode", strItemCode),
                    new SqlParameter("@strUserID", strUserID)
                };

                return DBHelper.ExecuteParameterizedReader("POS_Orders_View", CommandType.StoredProcedure, myparams);
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
                SqlParameter[] myparams = new SqlParameter[]
                {
                    new SqlParameter("@strControlNumber", strControlNumber),
                    new SqlParameter("@strUserID", strUserID)
                };

                return DBHelper.ExecuteParameterizedReader("POS_OrdersTotal_View", CommandType.StoredProcedure, myparams);
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
                SqlParameter[] myparams = new SqlParameter[]
                {
                    new SqlParameter("@intCMD", intCMD),
                    new SqlParameter("@strControlNumber", strControlNumber),
                    new SqlParameter("@dteReferenceDateFrom", dteReferenceDateFrom),
                    new SqlParameter("@dteReferenceDateTo", dteReferenceDateTo)
                };

                return DBHelper.ExecuteParameterizedReader("POS_SalesInvoice_View", CommandType.StoredProcedure, myparams);
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
                SqlParameter[] myparams = new SqlParameter[]
                {
                    new SqlParameter("@intTransactionType", intTransactionType),
                    new SqlParameter("@dteTransactionDate", dteTransactionDate),
                    new SqlParameter("@strUserID", strUserID)
                };

                return DBHelper.ExecuteParameterizedReader("POS_XZReport_View", CommandType.StoredProcedure, myparams);
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
                SqlParameter[] myparams = new SqlParameter[]
                {
                    new SqlParameter("@dteTransactionDate", dteTransactionDate)
                };

                return DBHelper.ExecuteParameterizedReader("POS_VoidItems_View", CommandType.StoredProcedure, myparams);
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
                SqlParameter[] myparams = new SqlParameter[]
                {
                    new SqlParameter("@dteTransactionDate", dteTransactionDate)
                };

                return DBHelper.ExecuteParameterizedReader("POS_CancelledOrders_View", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Post
        public string POS_Orders_Select(PointofSaleBO posBO, string strUserID)
        {
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@strTableCode", posBO.tablecode),
                new SqlParameter("@strCategoryCode", posBO.categorycode),
                new SqlParameter("@strItemCode", posBO.itemcode),
                new SqlParameter("@decItemPrice", posBO.itemprice),
                new SqlParameter("@intItemQuantity", posBO.itemquantity),
                new SqlParameter("@strUserID", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("POS_Orders_Select", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string POS_Orders_Post(PointofSaleBO posBO, string strUserID)
        {
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@strControlNumber", posBO.controlnumber),
                new SqlParameter("@strTableCode", posBO.tablecode),
                new SqlParameter("@strUserID", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("POS_Orders_Post", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string POS_Orders_Discount(PointofSaleBO posBO, string strUserID)
        {
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@strControlNumber", posBO.controlnumber),
                new SqlParameter("@strItemCode", posBO.itemcode),
                new SqlParameter("@intDiscountType", posBO.discounttype),
                new SqlParameter("@decDiscountAmount", posBO.discountamount),
                new SqlParameter("@strUserID", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("POS_Orders_Discount", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string POS_Orders_Quantity(PointofSaleBO posBO, string strUserID)
        {
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@strControlNumber", posBO.controlnumber),
                new SqlParameter("@strTableCode", posBO.tablecode),
                new SqlParameter("@strItemCode", posBO.itemcode),
                new SqlParameter("@intItemQuantity", posBO.itemquantity),
                new SqlParameter("@strUserID", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("POS_Orders_Quantity", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string POS_Orders_ServiceCharge(PointofSaleBO posBO, string strUserID)
        {
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@strControlNumber", posBO.controlnumber),
                new SqlParameter("@decServiceCharge", posBO.servicecharge),
                new SqlParameter("@strUserID", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("POS_Orders_ServiceCharge", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string POS_Orders_Payment(PointofSaleBO posBO, string strUserID)
        {
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@strControlNumber", posBO.controlnumber),
                new SqlParameter("@strTableCode", posBO.tablecode),
                new SqlParameter("@intPaymentType", posBO.paymenttype),
                new SqlParameter("@decPaymentAmount", posBO.paymentamount),
                new SqlParameter("@decChangeDue", posBO.changedue),
                new SqlParameter("@strCardType", posBO.cardtype),
                new SqlParameter("@strCardNumber", posBO.cardnumber),
                new SqlParameter("@strAccountName", posBO.accountname),
                new SqlParameter("@dteExpiryDate", posBO.expirydate),
                new SqlParameter("@strGCashReferenceNumber", posBO.gcashreferencenumber),
                new SqlParameter("@strUserID", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("POS_Orders_Payment", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string POS_Orders_Void(PointofSaleBO posBO, string strUserID)
        {
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@strControlNumber", posBO.controlnumber),
                new SqlParameter("@strItemCode", posBO.itemcode),
                new SqlParameter("@strVoidAuthorizedBy", posBO.voidauthorizedby),
                new SqlParameter("@strUserID", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("POS_Orders_Void", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string POS_Orders_Cancel(PointofSaleBO posBO, string strUserID)
        {
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@strControlNumber", posBO.controlnumber),
                new SqlParameter("@strTableCode", posBO.tablecode),
                new SqlParameter("@intCancelType", posBO.canceltype),
                new SqlParameter("@strCancelAuthorizedBy", posBO.cancelauthorizedby),
                new SqlParameter("@strCancelRemarks", posBO.cancelremarks),
                new SqlParameter("@strUserID", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("POS_Orders_Cancel", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
